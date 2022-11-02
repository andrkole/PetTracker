using Newtonsoft.Json;

namespace PetTracker.Shared.Models
{
    public class GoogleMapsPlaces
    {
        [JsonProperty("results")]
        public List<GoogleMapsPlaceInfo>? VetClinics { get; set; }
    }

    public class GoogleMapsPlaceInfo
    {
        public string? Name { get; set; }
        public OpeningHours? OpeningHours { get; set; }
        public string? Vicinity { get; set; }
        public double Rating { get; set; }

    }

    public class OpeningHours
    {
        public bool OpenNow { get; set; }
    }
}
