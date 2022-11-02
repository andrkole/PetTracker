using PetTracker.Shared.Models;

namespace PetTracker.Server.Services
{
    public class DistanceCalculatorService
    {
        public static double HaversineDistanceInMeters(double lng1, double lat1, double lng2, double lat2)
        {
            var R = 6371d;

            var dLat = ToRadian(lat2 - lat1);
            var dLon = ToRadian(lng2 - lng1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            var d = R * c;

            return d * 1000;
        }

        private static double ToRadian(double val)
        {
            return Math.PI / 180 * val;
        }
    }
}
