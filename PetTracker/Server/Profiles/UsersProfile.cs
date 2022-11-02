using AutoMapper;
using PetTracker.Server.Entities;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Profiles
{
	public class UsersProfile : Profile
	{
		public UsersProfile()
		{
			CreateMap<ApplicationUser, UserDto>()
				.ReverseMap();
		}
	}
}
