using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetTracker.Server.Entities
{
    public class Pet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string? Kind { get; set; }
        public string? Breed { get; set; }
        [Precision(3, 1)]
        public decimal? Age { get; set; }
        [Precision(5, 2)]
        public decimal? Weight { get; set; }
        public string? Gender { get; set; }
        public bool? Sterilized { get; set; }
        [JsonIgnore]
        public ApplicationUser? Owner { get; set; }
        public string? OwnerId { get; set; }
        public GpsDevice? GpsDevice { get; set; }
        public AirInfoSensor? AirInfoSensor { get; set; }
        public List<AirInfoData>? AirInfoData { get; set; }
        public List<PetLocation>? PetLocations { get; set; }
        public PetAreaCenterLocation? CenterLocation { get; set; }
        public FlameDetectionSensor? FlameDetectionSensor { get; set; }
        public List<FlameDetectionData>? FlameDetectionData { get; set; }
        public int? Radius { get; set; }
        public int RestingTimeSeconds { get; set; }
        public int ActiveTimeSeconds { get; set; }
        public DateTime? LastSynchronization { get; set; }

        public Pet(string name)
        {
            Name = name;
        }
    }
}
