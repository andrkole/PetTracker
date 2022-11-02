using PetTracker.Shared.Models;

namespace PetTracker.Server.Interfaces
{
    public interface IGpsRepository : ISensorsBaseRepository
    {
        Task<List<GoogleMapsPlaceInfo>?> GetClosestVetClinics(double latitude, double longitude, int radius);
    }
}
