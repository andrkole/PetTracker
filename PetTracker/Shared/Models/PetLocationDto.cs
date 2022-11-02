namespace PetTracker.Shared.Models
{
    public class PetLocationDto
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
		public bool LeftSafetyRadius { get; set; }
        public DateTime? LastSynchronization { get; set; }

        public PetLocationDto(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public PetLocationDto() { }
    }
}
