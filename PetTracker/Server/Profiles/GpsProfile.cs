using AutoMapper;
using PetTracker.Server.Entities;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Profiles
{
    public class GpsProfile : Profile
    {
        public GpsProfile()
        {
            CreateMap<GpsDevice, GpsDeviceDto>()
                .ReverseMap();
        }
    }
}
