using System.Net;
using FluentAssertions;
using Trippie.Common.Services.GlobalExceptionHandler.Exceptions;
using Xunit;

namespace Trippie.Tests;

public class ExceptionTests
{
    private sealed class TestException(HttpStatusCode status, string code, string msg,
        System.Exception? inner = null, IReadOnlyDictionary<string, object?>? meta = null)
        : AppException(status, code, msg, inner, meta);

    [Fact]
    public void AppException_StoresAllProperties()
    {
        var inner = new InvalidOperationException("inner");
        var meta = new Dictionary<string, object?> { ["k"] = "v" };

        var ex = new TestException((HttpStatusCode)418, "test.code", "boom", inner, meta);

        ex.StatusCode.Should().Be((HttpStatusCode)418);
        ex.ErrorCode.Should().Be("test.code");
        ex.Message.Should().Be("boom");
        ex.InnerException.Should().BeSameAs(inner);
        ex.Metadata.Should().ContainKey("k").WhoseValue.Should().Be("v");
    }

    [Fact]
    public void AppException_DefaultsMetadataToEmpty()
    {
        var ex = new TestException(HttpStatusCode.BadRequest, "x", "y");
        ex.Metadata.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void ValidationException_Multi_PopulatesErrorsAndStatus()
    {
        var errors = new Dictionary<string, IReadOnlyList<string>>
        {
            ["email"] = new[] { "Required.", "Invalid." },
            ["password"] = new[] { "Too short." }
        };

        var ex = new ValidationException(errors);

        ex.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        ex.ErrorCode.Should().Be("validation.failed");
        ex.Errors.Should().HaveCount(2);
        ex.Errors["email"].Should().Equal("Required.", "Invalid.");
        ex.Errors["password"].Should().Equal("Too short.");
    }

    [Fact]
    public void ValidationException_SingleField_WrapsIntoDictionary()
    {
        var ex = new ValidationException("email", "Required.");

        ex.Errors.Should().HaveCount(1);
        ex.Errors["email"].Should().ContainSingle().Which.Should().Be("Required.");
    }

    [Fact]
    public void NotFoundException_BuildsCodeAndMetadataFromResource()
    {
        var ex = new NotFoundException("User", 42);

        ex.StatusCode.Should().Be(HttpStatusCode.NotFound);
        ex.ErrorCode.Should().Be("user.not_found");
        ex.Message.Should().Be("User '42' was not found.");
        ex.Metadata["resource"].Should().Be("User");
        ex.Metadata["id"].Should().Be(42);
    }

    [Fact]
    public void NotFoundException_For_UsesGenericTypeName()
    {
        var ex = NotFoundException.For<ExceptionTests>("abc");

        ex.ErrorCode.Should().Be("exceptiontests.not_found");
        ex.Metadata["resource"].Should().Be(nameof(ExceptionTests));
        ex.Metadata["id"].Should().Be("abc");
    }

    [Fact]
    public void UnauthorizedException_DefaultsCorrectly()
    {
        var ex = new UnauthorizedException();

        ex.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        ex.ErrorCode.Should().Be("auth.unauthorized");
        ex.Message.Should().Contain("Authentication");
    }

    [Fact]
    public void UnauthorizedException_AcceptsCustomMessage()
    {
        var ex = new UnauthorizedException("Token expired.");
        ex.Message.Should().Be("Token expired.");
    }

    [Fact]
    public void ForbiddenException_DefaultsCorrectly()
    {
        var ex = new ForbiddenException();

        ex.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        ex.ErrorCode.Should().Be("auth.forbidden");
    }

    [Fact]
    public void ConflictException_HasDefaultErrorCode()
    {
        var ex = new ConflictException("Duplicate email.");

        ex.StatusCode.Should().Be(HttpStatusCode.Conflict);
        ex.ErrorCode.Should().Be("common.conflict");
        ex.Message.Should().Be("Duplicate email.");
    }

    [Fact]
    public void ConflictException_AllowsCustomErrorCode()
    {
        var ex = new ConflictException("Taken.", "user.email_taken");
        ex.ErrorCode.Should().Be("user.email_taken");
    }

    [Fact]
    public void InternalServerException_PreservesInner()
    {
        var inner = new System.Exception("root cause");
        var ex = new InternalServerException("Boom.", inner);

        ex.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        ex.ErrorCode.Should().Be("common.internal_error");
        ex.InnerException.Should().BeSameAs(inner);
    }
}
