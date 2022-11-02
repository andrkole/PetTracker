using System.ComponentModel.DataAnnotations;

namespace PetTracker.Shared.Enums
{
    public enum PetKind
    {
        [Display(Name = "Pas")]
        Dog,
        [Display(Name = "Mačka")]
        Cat
    }
}
