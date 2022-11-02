using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetTracker.Server.Entities
{
    public class FlameDetectionData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool FlameDetected { get; set; } 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [ForeignKey("FlameDetectionSensor")]
        public Guid SensorId { get; set; }
        public FlameDetectionSensor? FlameDetectionSensor { get; set; }
        public int PetId { get; set; }
        public Pet? Pet { get; set; }

        public FlameDetectionData(Pet pet, bool flameDetected)
        {
            SensorId = pet.FlameDetectionSensor!.Id;
            FlameDetected = flameDetected;
            Pet = pet;
        }

        public FlameDetectionData() { }
    }
}
