using PetTracker.Shared.Validators;
using System.ComponentModel.DataAnnotations;

namespace PetTracker.Shared.Models
{
    public class PetStatisticsRequest
    {
        [Required(ErrorMessage = "Molimo odaberite datum od kojega želite statistiku.")]
        [MaxDateTime]
        public DateTime? From { get; set; }
        [Required(ErrorMessage = "Molimo odaberite datum do kojega želite statistiku.")]
        [MaxDateTime]
        public DateTime? To { get; set; }
        public int? PetId { get; set; }
    }
}
