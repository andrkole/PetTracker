namespace PetTracker.Shared.Models
{
    public class FlameDetectionSensorData
    {
        public string? SensorId { get; set; }
        public FlameDetectionSensorDataWrapper? Data { get; set; }
        public bool? FlameDetectedInLast24h { get; set; }
    }

    public class FlameDetectionSensorDataWrapper
    {
        public bool FlameDetected { get; set; }
    }
}
