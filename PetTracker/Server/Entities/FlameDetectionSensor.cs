using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetTracker.Server.Entities
{
    public class FlameDetectionSensor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string SerialNumber { get; set; }
        public List<FlameDetectionData>? Data { get; set; }
        public int PetId { get; set; }
        [JsonIgnore]
        public Pet? Pet { get; set; }

        public FlameDetectionSensor(string serialNumber)
        {
            SerialNumber = serialNumber;
        }
    }
}
