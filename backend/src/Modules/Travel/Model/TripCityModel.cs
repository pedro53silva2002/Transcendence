using Trippie.Modules.Auth.Model;

namespace Trippie.Modules.Travel.Model;

public sealed class TripCity
{
    public required int TripId { get; set; }
    public required int CityId { get; set; }

    // public Trip? Trip { get; set; }
    // public City? City { get; set; }
}
