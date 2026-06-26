namespace Trippie.Modules.Auth.Dtos;

public sealed class VisitedCountryDto
{
	public required CountryDto Country { get; set; }
    public required DateTime AddedAt { get; set; }
	public required int SourceTripId { get; set; }
}

public sealed class CreateVisitedCountryDto
{
	public required int CountryId { get; set; }
	public required int SourceTripId { get; set; }
}

public sealed class SyncVisitedCountriesResultDto
{
    public required int NewlyAddedCount { get; set; }
    public required List<int> NewlyAddedCountryIds { get; set; }
}