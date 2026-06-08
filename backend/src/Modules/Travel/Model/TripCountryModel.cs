using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Travel.Model;

public sealed class TripCountry
{
    public required int TripId { get; set; }
    public required int CountryId { get; set; }

    // public Trip? Trip { get; set; }
    // public Country? Country { get; set; }
}
