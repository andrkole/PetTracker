using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetTracker.Server.Entities
{
    public class AirInfoData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal AirTemperature { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal AirHumidity { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        [ForeignKey("AirInfoSensor")]
        public Guid SensorId { get; set; }
        public AirInfoSensor? AirInfoSensor { get; set; }
        public int PetId { get; set; }
        public Pet? Pet { get; set; }

        public AirInfoData(Pet pet, decimal airTemperature, decimal airHumidity)
        {
            Pet = pet;
            SensorId = pet.AirInfoSensor!.Id;
            AirTemperature = airTemperature;
            AirHumidity = airHumidity;
        }

        public AirInfoData() { }
    }
}
