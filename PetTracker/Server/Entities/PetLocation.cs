using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetTracker.Server.Entities
{
    public class PetLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int PetId { get; set; } 
        public Pet? Pet { get; set; }
        [ForeignKey("GpsDevice")]
        public Guid GpsDeviceId { get; set; }
        public GpsDevice? GpsDevice { get; set; }

        public PetLocation(Pet pet, double lng, double lat)
        {
            Pet = pet;
            Longitude = lng;
            Latitude = lat;
            GpsDevice = pet.GpsDevice;
        }

        public PetLocation() { }
    }
}
