using System.ComponentModel.DataAnnotations;

namespace PetTracker.Shared.Models
{
    public class GpsDeviceDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Molimo unesite serijski broj GPS-a.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Serijski broj mora sadržavati 8 znamenaka.")]
        public string? SerialNumber { get; set; }
    }
}
