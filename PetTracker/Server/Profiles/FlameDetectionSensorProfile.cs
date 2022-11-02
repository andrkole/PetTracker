using AutoMapper;
using PetTracker.Server.Entities;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Profiles
{
    public class FlameDetectionSensorProfile : Profile
    {
        public FlameDetectionSensorProfile()
        {
            CreateMap<FlameDetectionSensor, FlameDetectionSensorDto>()
                .ConvertUsing((src, dest) =>
                {
                    return src is null
                    ? new FlameDetectionSensorDto()
                    : new FlameDetectionSensorDto
                    {
                        Id = src.Id,
                        SerialNumber = src.SerialNumber
                    };
                });

            CreateMap<FlameDetectionSensorDto, FlameDetectionSensor>();

            CreateMap<List<FlameDetectionData>, FlameDetectionSensorData>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src =>
                    src
                    .OrderByDescending(x => x.Timestamp)
                    .FirstOrDefault()))
                .ForMember(dest => dest.SensorId, opt => opt.MapFrom(src =>
                    src
                    .OrderByDescending(x => x.Timestamp)
                    .Select(x => x.SensorId)
                    .FirstOrDefault()))
                .ForMember(dest => dest.FlameDetectedInLast24h, opt => opt.MapFrom(src =>
                    src.Any() 
                    ? 
                    (bool?)src
                    .Where(x => x.Timestamp >= DateTime.UtcNow.AddHours(-24))
                    .Any(x => x.FlameDetected)
                    :
                    null));
                    

            CreateMap<FlameDetectionData, FlameDetectionSensorDataWrapper>();
        }
    }
}
