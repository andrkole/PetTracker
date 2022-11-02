using AutoMapper;
using PetTracker.Server.Entities;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Profiles
{
    public class PetLocationProfile : Profile
    {
        public PetLocationProfile()
        {
            CreateMap<PetLocation, PetLocationDto>()
                .ReverseMap();

            CreateMap<PetAreaCenterLocation, PetLocationDto>()
                .ReverseMap();
        }
    }
}
