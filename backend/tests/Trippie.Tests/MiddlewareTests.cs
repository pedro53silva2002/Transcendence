using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Trippie.Common.Services.GlobalExceptionHandler.Config;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.GlobalExceptionHandler.Mappings;
using Trippie.Common.Services.GlobalExceptionHandler.Middleware;
using Xunit;

namespace Trippie.Tests;

public class MiddlewareTests
{
    private sealed class FakeHostEnvironment(string name) : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = name;
        public string ApplicationName { get; set; } = "Trippie.Tests";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }

    private static ExceptionMapperPipeline NewPipeline() =>
        new(new IExceptionMapper[]
        {
            new DomainExceptionMapper(),
            new FrameworkExceptionMapper(),
            new FallbackExceptionMapper(),
        });

    private static (HttpContext ctx, MemoryStream body) NewContext(string method = "GET", string path = "/api/x")
    {
        var ctx = new DefaultHttpContext();
        ctx.Request.Method = method;
        ctx.Request.Path = path;
        ctx.TraceIdentifier = "test-trace-id";

        var body = new MemoryStream();
        ctx.Response.Body = body;
        return (ctx, body);
    }

    private static GlobalExceptionMiddleware BuildMiddleware(
        RequestDelegate next,
        string envName = "Production",
        bool exposeDetails = false)
    {
        var options = Options.Create(new ErrorHandlingOptions { ExposeDetails = exposeDetails });
        return new GlobalExceptionMiddleware(
            next,
            NewPipeline(),
            NullLogger<GlobalExceptionMiddleware>.Instance,
            new FakeHostEnvironment(envName),
            options);
    }

    private static async Task<JsonElement> ReadJsonAsync(MemoryStream body)
    {
        body.Position = 0;
        using var doc = await JsonDocument.ParseAsync(body);
        return doc.RootElement.Clone();
    }

    [Fact]
    public async Task PassesThrough_WhenNoException()
    {
        var (ctx, body) = NewContext();
        var called = false;
        RequestDelegate next = c => { called = true; c.Response.StatusCode = 204; return Task.CompletedTask; };

        await BuildMiddleware(next).InvokeAsync(ctx);

        called.Should().BeTrue();
        ctx.Response.StatusCode.Should().Be(204);
        body.Length.Should().Be(0);
    }

    [Fact]
    public async Task DomainException_WritesProblemJson()
    {
        var (ctx, body) = NewContext("GET", "/api/users/42");
        RequestDelegate next = _ => throw new NotFoundException("User", 42);

        await BuildMiddleware(next).InvokeAsync(ctx);

        ctx.Response.StatusCode.Should().Be(404);
        ctx.Response.ContentType.Should().Be("application/problem+json; charset=utf-8");
        ctx.Response.Headers["X-Correlation-Id"].ToString().Should().NotBeEmpty();
        ctx.Response.Headers.CacheControl.ToString().Should().Be("no-store");

        var json = await ReadJsonAsync(body);
        json.GetProperty("statusCode").GetInt32().Should().Be(404);
        json.GetProperty("errorCode").GetString().Should().Be("user.not_found");
        json.GetProperty("message").GetString().Should().Contain("User '42'");
        json.GetProperty("traceId").GetString().Should().NotBeNullOrEmpty();
        json.TryGetProperty("metadata", out var meta).Should().BeTrue();
        meta.GetProperty("id").GetInt32().Should().Be(42);
        meta.GetProperty("resource").GetString().Should().Be("User");
    }

    [Fact]
    public async Task ValidationException_PopulatesErrorsField()
    {
        var (ctx, body) = NewContext("POST", "/api/users");
        var errors = new Dictionary<string, IReadOnlyList<string>>
        {
            ["email"] = new[] { "Required.", "Invalid." },
            ["password"] = new[] { "Too short." }
        };
        RequestDelegate next = _ => throw new ValidationException(errors);

        await BuildMiddleware(next).InvokeAsync(ctx);

        ctx.Response.StatusCode.Should().Be(400);

        var json = await ReadJsonAsync(body);
        json.GetProperty("errorCode").GetString().Should().Be("validation.failed");
        var errs = json.GetProperty("errors");
        errs.GetProperty("email").EnumerateArray().Select(e => e.GetString())
            .Should().Equal("Required.", "Invalid.");
        errs.GetProperty("password").EnumerateArray().Select(e => e.GetString())
            .Should().Equal("Too short.");
    }

    [Fact]
    public async Task UnknownException_FallsBackTo500()
    {
        var (ctx, body) = NewContext();
        RequestDelegate next = _ => throw new InvalidOperationException("internal");

        await BuildMiddleware(next).InvokeAsync(ctx);

        ctx.Response.StatusCode.Should().Be(500);
        var json = await ReadJsonAsync(body);
        json.GetProperty("errorCode").GetString().Should().Be("common.unhandled");
        json.GetProperty("message").GetString().Should().NotContain("internal");
    }

    [Fact]
    public async Task FrameworkException_TimeoutMapsTo504()
    {
        var (ctx, body) = NewContext();
        RequestDelegate next = _ => throw new TimeoutException();

        await BuildMiddleware(next).InvokeAsync(ctx);

        ctx.Response.StatusCode.Should().Be(504);
        var json = await ReadJsonAsync(body);
        json.GetProperty("errorCode").GetString().Should().Be("common.timeout");
    }

    [Fact]
    public async Task Development_IncludesDetails()
    {
        var (ctx, body) = NewContext();
        RequestDelegate next = _ => throw new InvalidOperationException("internal-secret");

        await BuildMiddleware(next, envName: Environments.Development).InvokeAsync(ctx);

        var json = await ReadJsonAsync(body);
        json.TryGetProperty("details", out var details).Should().BeTrue();
        details.GetProperty("exceptionType").GetString().Should().Contain("InvalidOperationException");
        details.GetProperty("stackTrace").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Production_OmitsDetails()
    {
        var (ctx, body) = NewContext();
        RequestDelegate next = _ => throw new InvalidOperationException("internal-secret");

        await BuildMiddleware(next, envName: Environments.Production, exposeDetails: false).InvokeAsync(ctx);

        var json = await ReadJsonAsync(body);
        json.TryGetProperty("details", out _).Should().BeFalse(
            "production responses must never leak stack traces or exception types");
    }

    [Fact]
    public async Task Production_WithExposeDetailsOverride_IncludesDetails()
    {
        var (ctx, body) = NewContext();
        RequestDelegate next = _ => throw new InvalidOperationException("internal");

        await BuildMiddleware(next, envName: Environments.Production, exposeDetails: true).InvokeAsync(ctx);

        var json = await ReadJsonAsync(body);
        json.TryGetProperty("details", out _).Should().BeTrue();
    }

    [Fact]
    public async Task SetsCorrelationIdHeader_MatchingBodyTraceId()
    {
        var (ctx, body) = NewContext();
        RequestDelegate next = _ => throw new ConflictException("dup");

        await BuildMiddleware(next).InvokeAsync(ctx);

        var headerId = ctx.Response.Headers["X-Correlation-Id"].ToString();
        var json = await ReadJsonAsync(body);
        var bodyId = json.GetProperty("traceId").GetString();

        headerId.Should().NotBeNullOrEmpty();
        bodyId.Should().Be(headerId);
    }

    [Fact]
    public async Task TimestampIsRecent()
    {
        var (ctx, body) = NewContext();
        RequestDelegate next = _ => throw new ConflictException("dup");

        var before = DateTimeOffset.UtcNow.AddSeconds(-1);
        await BuildMiddleware(next).InvokeAsync(ctx);
        var after = DateTimeOffset.UtcNow.AddSeconds(1);

        var json = await ReadJsonAsync(body);
        var ts = json.GetProperty("timestamp").GetDateTimeOffset();
        ts.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Fact]
    public async Task NullMetadata_IsOmittedFromJson()
    {
        var (ctx, body) = NewContext();
        RequestDelegate next = _ => throw new ConflictException("dup");

        await BuildMiddleware(next).InvokeAsync(ctx);

        var json = await ReadJsonAsync(body);
        json.TryGetProperty("metadata", out _).Should().BeFalse();
        json.TryGetProperty("errors", out _).Should().BeFalse();
    }

    // ── CorrelationIdMiddleware ──────────────────────────────────────────────

    private sealed class FireableResponseFeature : Microsoft.AspNetCore.Http.Features.IHttpResponseFeature
    {
        private readonly List<(Func<object, Task> cb, object st)> _starting = new();
        private readonly List<(Func<object, Task> cb, object st)> _completed = new();

        public int StatusCode { get; set; } = 200;
        public string? ReasonPhrase { get; set; }
        public IHeaderDictionary Headers { get; set; } = new HeaderDictionary();
        public Stream Body { get; set; } = Stream.Null;
        public bool HasStarted { get; private set; }

        public void OnStarting(Func<object, Task> callback, object state)
            => _starting.Add((callback, state));

        public void OnCompleted(Func<object, Task> callback, object state)
            => _completed.Add((callback, state));

        public async Task FireOnStartingAsync()
        {
            if (HasStarted) return;
            HasStarted = true;
            foreach (var (cb, st) in _starting) await cb(st);
        }
    }

    private static (HttpContext ctx, FireableResponseFeature feat) NewCorrelationContext()
    {
        var ctx = new DefaultHttpContext();
        var feat = new FireableResponseFeature();
        ctx.Features.Set<Microsoft.AspNetCore.Http.Features.IHttpResponseFeature>(feat);
        return (ctx, feat);
    }

    [Fact]
    public async Task CorrelationId_UsesInboundHeader_WhenPresent()
    {
        var (ctx, feat) = NewCorrelationContext();
        ctx.Request.Headers["X-Correlation-Id"] = "inbound-id";

        var sut = new CorrelationIdMiddleware(_ => Task.CompletedTask);
        await sut.InvokeAsync(ctx);
        await feat.FireOnStartingAsync();

        ctx.TraceIdentifier.Should().Be("inbound-id");
        feat.Headers["X-Correlation-Id"].ToString().Should().Be("inbound-id");
    }

    [Fact]
    public async Task CorrelationId_GeneratesWhenAbsent()
    {
        var (ctx, feat) = NewCorrelationContext();
        ctx.TraceIdentifier = "auto-generated";

        var sut = new CorrelationIdMiddleware(_ => Task.CompletedTask);
        await sut.InvokeAsync(ctx);
        await feat.FireOnStartingAsync();

        ctx.TraceIdentifier.Should().NotBeNullOrWhiteSpace();
        feat.Headers["X-Correlation-Id"].ToString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task CorrelationId_IgnoresWhitespaceInboundHeader()
    {
        var (ctx, feat) = NewCorrelationContext();
        ctx.Request.Headers["X-Correlation-Id"] = "   ";
        ctx.TraceIdentifier = "fallback-id";

        var sut = new CorrelationIdMiddleware(_ => Task.CompletedTask);
        await sut.InvokeAsync(ctx);
        await feat.FireOnStartingAsync();

        ctx.TraceIdentifier.Should().NotBe("   ");
        feat.Headers["X-Correlation-Id"].ToString().Should().NotBe("   ");
    }

    [Fact]
    public async Task CorrelationId_CallsNext()
    {
        var (ctx, _) = NewCorrelationContext();
        var called = false;
        RequestDelegate next = _ => { called = true; return Task.CompletedTask; };

        await new CorrelationIdMiddleware(next).InvokeAsync(ctx);

        called.Should().BeTrue();
    }
}
