using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetTracker.Server.Entities
{
    public class PetAreaCenterLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public List<Pet>? Pets { get; set; }

        public PetAreaCenterLocation(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}
