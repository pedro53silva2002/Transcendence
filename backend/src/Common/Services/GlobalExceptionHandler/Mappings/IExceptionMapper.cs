namespace Trippie.Common.Services.GlobalExceptionHandler.Mappings;

public interface IExceptionMapper
{
    ErrorMappingResult? TryMap(System.Exception exception);
}
