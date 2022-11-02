using AutoMapper;
using Newtonsoft.Json;
using PetTracker.Server.Data;
using PetTracker.Server.Entities;
using PetTracker.Server.Exceptions;
using PetTracker.Server.Interfaces;
using PetTracker.Shared.Models;
using System.Globalization;
using System.Web;

namespace PetTracker.Server.Repositories
{
    public class GpsRepository : IGpsRepository
    {
        private readonly ILogger<GpsRepository> _logger;
        private readonly HttpClient _httpClient;
        private readonly PetTrackerDbContext _context;
        private readonly IMapper _mapper;

        public string GoogleMapsAPIKey { get; init; }

        public GpsRepository(ILogger<GpsRepository> logger, IHttpClientFactory httpClientFactory,
            PetTrackerDbContext context, IMapper mapper, string googleMapsAPIKey)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            GoogleMapsAPIKey = googleMapsAPIKey;
            _httpClient = httpClientFactory.CreateClient("GMPlaces");
        }
        
        public bool IsDeviceInUse(string serialNumber)
        {
            var isDeviceAlreadyUsed = _context.GpsDevices.Any(x => x.SerialNumber == serialNumber);

            return isDeviceAlreadyUsed;
        }

        public async Task<List<GoogleMapsPlaceInfo>?> GetClosestVetClinics(double latitude, double longitude, int radius)
        {
            _logger.LogInformation($"{nameof(GetClosestVetClinics)} started...");

            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("location", latitude.ToString(CultureInfo.InvariantCulture) + "," + longitude.ToString(CultureInfo.InvariantCulture));
            query.Add("radius", radius.ToString());
            query.Add("type", "veterinary_care");
            query.Add("key", GoogleMapsAPIKey);

            var uriBuilder = new UriBuilder(_httpClient.BaseAddress + "nearbysearch/json")
            {
                Query = query.ToString()
            };

            try
            {
                var vetClinicsResponse = await _httpClient.GetAsync(uriBuilder.Uri);
                vetClinicsResponse.EnsureSuccessStatusCode();

                var vetClinicsJson = await vetClinicsResponse.Content.ReadAsStringAsync();
                var closestVetClinics = JsonConvert.DeserializeObject<GoogleMapsPlaces>(vetClinicsJson);

                if (closestVetClinics is null || closestVetClinics.VetClinics is null)
                    throw new DeserializationFailedException("Deserialization failed for closest veterinary clinics.");
                
                _logger.LogInformation($"{nameof(GetClosestVetClinics)} completed successfully.");
                return closestVetClinics.VetClinics;
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Failed to retrieve closest vetinary clinics Google Maps Places API.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(GetClosestVetClinics)} failed.");
            }

            return null;
        }
    }
}
