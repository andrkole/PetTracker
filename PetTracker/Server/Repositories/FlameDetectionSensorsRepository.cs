using PetTracker.Server.Data;
using PetTracker.Server.Interfaces;

namespace PetTracker.Server.Repositories
{
    public class FlameDetectionSensorsRepository : IFlameDetectionSensorsRepository
    {
        private readonly PetTrackerDbContext _context;

        public FlameDetectionSensorsRepository(PetTrackerDbContext context)
        {
            _context = context;
        }

        public bool IsDeviceInUse(string serialNumber)
        {
            var sensorAlreadyUsed = _context.FlameDetectionSensors.Any(x => x.SerialNumber == serialNumber);

            return sensorAlreadyUsed;
        }
    }
}
