using System.ComponentModel.DataAnnotations;

namespace PetTracker.Shared.Enums
{
	public enum PetGender
	{
		[Display(Name = "Muško")]
		Male,
		[Display(Name = "Žensko")]
		Female
	}
}
