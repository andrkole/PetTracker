using AutoMapper;
using PetTracker.Server.Entities;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Profiles
{
    public class PetsProfile : Profile
    {
        public PetsProfile()
        {
            CreateMap<Pet, PetDto>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                    src.PetLocations != null && src.PetLocations.Count > 0 ?
                        src.PetLocations
                            .OrderByDescending(loc => loc.Timestamp)
                            .First() : null))
                .ReverseMap();
        }
    }
}