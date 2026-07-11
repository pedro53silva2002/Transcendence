namespace Trippie.Modules.Auth.Dtos;

public sealed class CityDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int CountryId { get; set; }
}
