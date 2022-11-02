using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetTracker.Server.Entities
{
    [Index(nameof(SerialNumber), IsUnique = true)]
    public class GpsDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string SerialNumber { get; set; }
        public int PetId { get; set; }
        [JsonIgnore]
        public Pet? Pet { get; set; }

        public GpsDevice(string serialNumber)
        {
            SerialNumber = serialNumber;
        }
    }
}
