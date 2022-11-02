using System.ComponentModel.DataAnnotations;

namespace PetTracker.Shared.Models
{
    public class FlameDetectionSensorDto
    {
        public Guid Id { get; set; }
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Serijski broj mora sadržavati 6 znamenaka.")]
        public string? SerialNumber { get; set; }
    }
}
