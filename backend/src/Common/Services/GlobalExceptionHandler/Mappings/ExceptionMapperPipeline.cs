namespace Trippie.Common.Services.GlobalExceptionHandler.Mappings;

public sealed class ExceptionMapperPipeline(IEnumerable<IExceptionMapper> mappers)
{
    private readonly IReadOnlyList<IExceptionMapper> _mappers = [.. mappers];

    public ErrorMappingResult Map(System.Exception exception)
    {
        foreach (var mapper in _mappers)
        {
            var result = mapper.TryMap(exception);
            if (result is not null) return result;
        }

        throw new InvalidOperationException(
            "ExceptionMapperPipeline has no terminal mapper. Did you forget to call AddExceptionHandling()?"
        );
    }
}
