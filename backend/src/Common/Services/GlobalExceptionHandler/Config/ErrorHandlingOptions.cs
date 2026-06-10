namespace Trippie.Common.Services.GlobalExceptionHandler.Config;

public sealed class ErrorHandlingOptions
{
    public const string SectionName = "ErrorHandling";
    public bool ExposeDetails { get; set; } = false;
}
