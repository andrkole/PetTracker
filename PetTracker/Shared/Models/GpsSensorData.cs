using Newtonsoft.Json;

namespace PetTracker.Shared.Models
{
    public class GpsSensorData
    {
        public string? GpsId { get; set; }
        [JsonProperty("data")]
        public PetLocationDto? PetLocation { get; set; }
    }
}
