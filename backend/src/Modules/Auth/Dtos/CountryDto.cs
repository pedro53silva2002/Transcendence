using Trippie.Common.Database;

namespace Trippie.Modules.Auth.Dtos;

public sealed class CountryDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
}
