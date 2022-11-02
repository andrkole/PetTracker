namespace PetTracker.Server.Interfaces
{
    public interface ISensorsBaseRepository
    {
        bool IsDeviceInUse(string serialNumber);
    }
}
