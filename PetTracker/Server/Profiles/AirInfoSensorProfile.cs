using AutoMapper;
using PetTracker.Server.Entities;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Profiles
{
    public class AirInfoSensorProfile : Profile
    {
        public AirInfoSensorProfile()
        {
            CreateMap<AirInfoSensor, AirInfoSensorDto>()
                .ConvertUsing((src, dest) =>
                {
                    return src is null
                    ? new AirInfoSensorDto()
                    : new AirInfoSensorDto 
                    { 
                        Id = src.Id, 
                        SerialNumber = src.SerialNumber 
                    };
                });

            CreateMap<AirInfoSensorDto, AirInfoSensor>();

            CreateMap<List<AirInfoData>, AirInfoSensorData>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src =>
                    src
                    .OrderByDescending(x => x.Timestamp)
                    .FirstOrDefault()))
                .ForMember(dest => dest.SensorId, opt => opt.MapFrom(src =>
                    src
                    .OrderByDescending(x => x.Timestamp)
                    .Select(x => x.SensorId)
                    .FirstOrDefault()));

            CreateMap<AirInfoData, AirInfoSensorDataWrapper>();
        }
    }
}
