using PetTracker.Shared.Enums;
using PetTracker.Shared.Validators;
using System.ComponentModel.DataAnnotations;

namespace PetTracker.Shared.Models
{
    public class PetDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Molimo unesite ime ljubimca.")]
        [StringLength(100, ErrorMessage = "Ime je predugo.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Molimo odaberite vrstu ljubimca.")]
        public PetKind? Kind { get; set; }
        public string? Breed { get; set; }
        [Range(1, 30, ErrorMessage = "Molimo unesite starost između 1-30 godina.")]
        [DecimalPrecision(1)]
        public decimal? Age { get; set; }
        [Range(1, 100, ErrorMessage = "Molimo unesite težinu između 1-100kg.")]
        [DecimalPrecision(2)]
        public decimal? Weight { get; set; }
        public PetGender? Gender { get; set; }
        public bool? Sterilized { get; set; }
        public PetLocationDto? CenterLocation { get; set; }
        [Required(ErrorMessage = "Molimo unesite radius (u metrima) sigurne zone iz koje ljubimac ne smije izaći.")]
        public int? Radius { get; set; }
        public bool FirstLocationAsCenter { get; set; } = true;
        public string? OwnerId { get; set; }
        [Required]
        public GpsDeviceDto GpsDevice { get; set; } = new();
        public PetLocationDto? Location { get; set; }
        public AirInfoSensorDto AirInfoSensor { get; set; } = new();
        public AirInfoSensorData? AirInfoSensorData { get; set; }
        public FlameDetectionSensorDto FlameDetectionSensor { get; set; } = new();
        public FlameDetectionSensorData? FlameDetectionSensorData { get; set; }
        public DateTime? LastSynchronization { get; set; }
    }
}
