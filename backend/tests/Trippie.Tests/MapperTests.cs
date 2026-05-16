using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Trippie.Common.Services.GlobalExceptionHandler.Mappings;
using Xunit;

namespace Trippie.Tests;

public class MapperTests
{
    // ── DomainExceptionMapper ────────────────────────────────────────────────

    [Fact]
    public void Domain_ReturnsNull_ForNonAppException()
    {
        var sut = new DomainExceptionMapper();
        sut.TryMap(new InvalidOperationException()).Should().BeNull();
    }

    [Fact]
    public void Domain_MapsNotFoundException()
    {
        var sut = new DomainExceptionMapper();
        var result = sut.TryMap(new NotFoundException("User", 7));

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(HttpStatusCode.NotFound);
        result.ErrorCode.Should().Be("user.not_found");
        result.LogLevel.Should().Be(LogLevel.Warning);
        result.Metadata.Should().NotBeNull();
        result.Metadata!["id"].Should().Be(7);
        result.Errors.Should().BeNull();
    }

    [Fact]
    public void Domain_MapsValidationException_PopulatesErrors()
    {
        var sut = new DomainExceptionMapper();
        var result = sut.TryMap(new ValidationException("email", "Required."));

        result.Should().NotBeNull();
        result!.Errors.Should().NotBeNull();
        result.Errors!["email"].Should().Equal("Required.");
        result.LogLevel.Should().Be(LogLevel.Warning);
    }

    [Fact]
    public void Domain_500_GetsErrorLogLevel()
    {
        var sut = new DomainExceptionMapper();
        var result = sut.TryMap(new InternalServerException("boom"));

        result!.LogLevel.Should().Be(LogLevel.Error);
    }

    [Fact]
    public void Domain_OmitsEmptyMetadata()
    {
        var sut = new DomainExceptionMapper();
        var result = sut.TryMap(new ConflictException("x"));

        result!.Metadata.Should().BeNull();
    }

    // ── FrameworkExceptionMapper ─────────────────────────────────────────────

    [Theory]
    [MemberData(nameof(FrameworkCases))]
    public void Framework_MapsKnownExceptions(System.Exception input, HttpStatusCode expectedStatus, string expectedCode)
    {
        var sut = new FrameworkExceptionMapper();
        var result = sut.TryMap(input);

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(expectedStatus);
        result.ErrorCode.Should().Be(expectedCode);
    }

    public static IEnumerable<object[]> FrameworkCases() => new[]
    {
        new object[] { new UnauthorizedAccessException(),    HttpStatusCode.Unauthorized,    "auth.unauthorized"     },
        new object[] { new BadHttpRequestException("x"),     HttpStatusCode.BadRequest,      "request.bad"           },
        new object[] { CreateJsonException(),                HttpStatusCode.BadRequest,      "request.invalid_json"  },
        new object[] { new OperationCanceledException(),     (HttpStatusCode)499,            "request.cancelled"     },
        new object[] { new TimeoutException(),               HttpStatusCode.GatewayTimeout,  "common.timeout"        },
        new object[] { new NotImplementedException(),        HttpStatusCode.NotImplemented,  "common.not_implemented"},
    };

    private static JsonException CreateJsonException()
    {
        try { JsonSerializer.Deserialize<int>("\"not-an-int\""); return null!; }
        catch (JsonException ex) { return ex; }
    }

    [Fact]
    public void Framework_ReturnsNull_ForUnhandled()
    {
        var sut = new FrameworkExceptionMapper();
        sut.TryMap(new InvalidOperationException()).Should().BeNull();
    }

    [Fact]
    public void Framework_OperationCancelled_LogsAsInformation()
    {
        var sut = new FrameworkExceptionMapper();
        var result = sut.TryMap(new OperationCanceledException());
        result!.LogLevel.Should().Be(LogLevel.Information);
    }

    // ── FallbackExceptionMapper ──────────────────────────────────────────────

    [Fact]
    public void Fallback_AlwaysReturns500()
    {
        var sut = new FallbackExceptionMapper();
        var result = sut.TryMap(new System.Exception("anything"));

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        result.ErrorCode.Should().Be("common.unhandled");
        result.LogLevel.Should().Be(LogLevel.Error);
    }

    [Fact]
    public void Fallback_DoesNotLeakInternalMessage()
    {
        var sut = new FallbackExceptionMapper();
        var result = sut.TryMap(new System.Exception("SECRET internal detail"));

        result!.ClientMessage.Should().NotContain("SECRET");
    }

    // ── ExceptionMapperPipeline ──────────────────────────────────────────────

    [Fact]
    public void Pipeline_FirstNonNullWins()
    {
        var pipeline = new ExceptionMapperPipeline(new IExceptionMapper[]
        {
            new DomainExceptionMapper(),
            new FrameworkExceptionMapper(),
            new FallbackExceptionMapper(),
        });

        var result = pipeline.Map(new NotFoundException("User", 1));
        result.ErrorCode.Should().Be("user.not_found");
    }

    [Fact]
    public void Pipeline_FallsThroughToFallback()
    {
        var pipeline = new ExceptionMapperPipeline(new IExceptionMapper[]
        {
            new DomainExceptionMapper(),
            new FrameworkExceptionMapper(),
            new FallbackExceptionMapper(),
        });

        var result = pipeline.Map(new InvalidOperationException("oops"));
        result.ErrorCode.Should().Be("common.unhandled");
        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public void Pipeline_FrameworkBeatsFallback()
    {
        var pipeline = new ExceptionMapperPipeline(new IExceptionMapper[]
        {
            new DomainExceptionMapper(),
            new FrameworkExceptionMapper(),
            new FallbackExceptionMapper(),
        });

        var result = pipeline.Map(new TimeoutException());
        result.ErrorCode.Should().Be("common.timeout");
    }

    [Fact]
    public void Pipeline_ThrowsIfNoTerminalMapper()
    {
        var pipeline = new ExceptionMapperPipeline(new IExceptionMapper[]
        {
            new DomainExceptionMapper(),
        });

        Action act = () => pipeline.Map(new InvalidOperationException());
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*terminal mapper*");
    }
}
