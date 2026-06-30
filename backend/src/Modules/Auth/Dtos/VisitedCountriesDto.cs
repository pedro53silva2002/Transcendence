namespace Trippie.Modules.Auth.Dtos;

public sealed class VisitedCountryDto
{
	public required long Id { get; set; }
	public required CountryDto Country { get; set; }
    public required DateTime AddedAt { get; set; }
	public DateTime? DeletedAt { get; set; }
	public int? SourceTripId { get; set; }
}

public sealed class SyncVisitedCountriesResultDto
{
    public required int NewlyAddedCount { get; set; }
    public required List<int> NewlyAddedCountryIds { get; set; }
}
