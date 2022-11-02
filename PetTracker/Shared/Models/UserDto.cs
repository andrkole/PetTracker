using System.ComponentModel.DataAnnotations;

namespace PetTracker.Shared.Models
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Molimo unesite ime korisnika.")]
        [StringLength(100, ErrorMessage = "Ime je predugo.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Molimo unesite prezime korisnika.")]
        [StringLength(100, ErrorMessage = "Prezime je predugo.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Molimo unesite email korisnika.")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
		public IList<PetDto>? Pets { get; set; }
    }
}
