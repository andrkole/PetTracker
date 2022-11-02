namespace PetTracker.Shared.Models
{
    public class AirInfoSensorData
    {
        public string? SensorId { get; set; }
        public AirInfoSensorDataWrapper? Data { get; set; }
    }

    public class AirInfoSensorDataWrapper
    {
        public decimal AirTemperature { get; set; }
        public decimal AirHumidity { get; set; }
    }
}
