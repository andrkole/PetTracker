using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PetTracker.Server.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string? LastName { get; set; }
        public ICollection<Pet> Pets { get; set; }
            = new List<Pet>();
    }
}