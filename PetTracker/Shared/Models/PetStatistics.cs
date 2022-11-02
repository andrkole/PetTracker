namespace PetTracker.Shared.Models
{
	public class PetStatistics
	{
		public double? DistanceTravelled { get; set; }
		public decimal? AvgRoomAirTemperature { get; set; }
		public decimal? AvgRoomAirHumidity { get; set; }
        public TimeSpan? ActiveTime { get; set; }
        public TimeSpan? RestingTime { get; set; }
        public PetDto? Pet { get; set; }
    }
}
