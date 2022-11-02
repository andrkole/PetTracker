using PetTracker.Server.Entities;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Interfaces
{
    public interface IPetsRepository
    {
        Task<PetLocationDto?> UpdateAndGetPetLocation(Pet pet);
        (PetLocationDto? location, string? errMsg) GetLatestPetLocation(int petId);
        Pet AddUserPet(PetDto pet);
        ICollection<PetDto> GetUserPets(string userId);
        void DeletePet(int petId);
        Pet? UpdatePet(PetDto pet);
        Pet? GetPetById(int petId);
        Task<(AirInfoSensorData? airInfo, string? errMsg)> UpdateAndGetPetAirInfo(int petId);
        List<Pet> GetAllPets();
        PetStatistics? GetPetStatistics(DateTime dateFrom, DateTime dateTo, Pet pet);
        Task<(FlameDetectionSensorData? flameDetectionInfo, string? errMsg)> UpdateAndGetFlameDetectionInfo(int petId);
    }
}
