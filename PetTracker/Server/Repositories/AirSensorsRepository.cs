using PetTracker.Server.Data;
using PetTracker.Server.Interfaces;

namespace PetTracker.Server.Repositories
{
    public class AirSensorsRepository : IAirSensorsRepository
    {
        private readonly PetTrackerDbContext _context;

        public AirSensorsRepository(PetTrackerDbContext context)
        {
            _context = context;
        }

        public bool IsDeviceInUse(string serialNumber)
        {
            var sensorAlreadyUsed = _context.AirInfoSensors.Any(x => x.SerialNumber == serialNumber);

            return sensorAlreadyUsed;
        }
    }
}
