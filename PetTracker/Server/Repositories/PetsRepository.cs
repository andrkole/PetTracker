using Newtonsoft.Json;
using PetTracker.Server.Data;
using PetTracker.Server.Entities;
using PetTracker.Server.Exceptions;
using PetTracker.Server.Interfaces;
using PetTracker.Shared.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using PetTracker.Server.Services;
using Microsoft.Data.SqlClient;

namespace PetTracker.Server.Repository
{
    public class PetsRepository : IPetsRepository
    {
        private readonly ILogger<PetsRepository> _logger;
        private readonly HttpClient _httpClient;
        private readonly PetTrackerDbContext _context;
        private readonly IMapper _mapper;

        private string TemperatureDataRelativeUrl { get; init; }
        private string AirInfoDataRelativeUrl { get; init; }
        public string FlameDetectionDataRelativeUrl { get; set; }

        public PetsRepository(ILogger<PetsRepository> logger, IHttpClientFactory httpClientFactory,
            PetTrackerDbContext context, IMapper mapper, string temperatureDataRelativeUrl, string airInfoDataRelativeUrl,
            string flameDetectionDataRelativeUrl)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("PetTracker");
            _context = context;
            _mapper = mapper;
            TemperatureDataRelativeUrl = temperatureDataRelativeUrl;
            AirInfoDataRelativeUrl = airInfoDataRelativeUrl;
            FlameDetectionDataRelativeUrl = flameDetectionDataRelativeUrl;
        }

        public async Task<PetLocationDto?> UpdateAndGetPetLocation(Pet pet)
        {
            _logger.LogInformation($"{nameof(UpdateAndGetPetLocation)} started...");

            PetLocationDto? petLocation = null;
            try
            {
                var gpsDataResponse = await _httpClient.GetAsync(TemperatureDataRelativeUrl);
                gpsDataResponse.EnsureSuccessStatusCode();

                var gpsDataJson = await gpsDataResponse.Content.ReadAsStringAsync();
                var allGpsData = JsonConvert.DeserializeObject<List<GpsSensorData>>(gpsDataJson);

                if (allGpsData is null)
                    throw new DeserializationFailedException();

                if (allGpsData.Count == 0)
                    _logger.LogError("No GPS data found.");

                var petGpsData = allGpsData.Find(d => d.GpsId == pet.GpsDevice?.Id.ToString());
                petLocation = petGpsData?.PetLocation;

                if (petLocation is null)
                    _logger.LogWarning("Couldn't match pet's GPS device ID with device ID from data response. Pet ID: {Id}", pet.Id);
                else
                    UpdatePetLocation(pet, petLocation.Longitude, petLocation.Latitude);

                _logger.LogInformation($"{nameof(UpdateAndGetPetLocation)} completed successfully.");
            }
            catch (DeserializationFailedException deserializationFailedEx)
            {
                _logger.LogError(deserializationFailedEx, "Failed to deserialize pet location data.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve pet location data.");
            }

            return petLocation;
        }

        private void UpdatePetLocation(Pet pet, double lng, double lat)
        {
            _logger.LogInformation($"{nameof(UpdatePetLocation)} started...");

            var petLocationEntity = new PetLocation(pet, lng, lat);

            if (pet.CenterLocation is null || (pet.CenterLocation.Longitude == 0 && pet.CenterLocation.Latitude == 0))
            {
                var petCenterLocation = new PetAreaCenterLocation(lng, lat);
                _context.PetAreaCenterLocations.Add(petCenterLocation);
                pet.CenterLocation = petCenterLocation;
            }

            pet.LastSynchronization = petLocationEntity.Timestamp;
            pet.PetLocations?.Add(petLocationEntity);
            _context.PetLocations.Add(petLocationEntity);
            _context.SaveChanges();

            _logger.LogInformation($"{nameof(UpdatePetLocation)} completed successfully.");
        }

        public (PetLocationDto? location, string? errMsg) GetLatestPetLocation(int petId)
        {
            var pet = GetPetById(petId);

            if (pet is null)
            {
                _logger.LogError("No pet found in DB with ID: {PetId}", petId);
                return (null, $"Pet with ID: {petId} not found.");
            }

            var latestPetLoc = pet.PetLocations?.OrderByDescending(l => l.Timestamp).FirstOrDefault();
            if (latestPetLoc is null)
			{
                return (null, "Failed to retrieve latest pet location.");
			}

            var latestPetLocDto = _mapper.Map<PetLocationDto>(latestPetLoc);
            latestPetLocDto.LastSynchronization = latestPetLoc.Timestamp;

            if (latestPetLoc.Timestamp > DateTime.UtcNow.AddMinutes(-10))
            {
                var distanceFromCenter = DistanceCalculatorService.HaversineDistanceInMeters(
                        latestPetLoc.Longitude, latestPetLoc.Latitude, pet.CenterLocation!.Longitude, pet.CenterLocation.Latitude);
                if (distanceFromCenter > 5 && distanceFromCenter > pet.Radius)
                {
                    latestPetLocDto.LeftSafetyRadius = true;
                }
            }
            
            return (latestPetLocDto, null);
        }

        public async Task<(AirInfoSensorData? airInfo, string? errMsg)> UpdateAndGetPetAirInfo(int petId)
        {
            _logger.LogInformation($"{nameof(UpdateAndGetPetAirInfo)} started...");

            var pet = GetPetById(petId);
            string? errorMsg = null;

            if (pet is null)
            {
                _logger.LogError("No pet found in DB with ID: {PetId}", petId);
                return (null, $"Pet with ID: {petId} not found.");
            }

            AirInfoSensorData? airInfoSensorData = null;
            try
            {
                var airInfoSensorsResponse = await _httpClient.GetAsync(AirInfoDataRelativeUrl);
                airInfoSensorsResponse.EnsureSuccessStatusCode();

                var airInfoSensorsJson = await airInfoSensorsResponse.Content.ReadAsStringAsync();
                var allAirInfoSensors = JsonConvert.DeserializeObject<List<AirInfoSensorData>>(airInfoSensorsJson);

                if (allAirInfoSensors is null)
                {
                    throw new DeserializationFailedException();
                }

                if (allAirInfoSensors.Count == 0)
                {
                    _logger.LogError("No air sensors data found.");
                    return (null, "No air information data found.");
                }

                airInfoSensorData = allAirInfoSensors.Find(d => d.SensorId?.Equals(pet.AirInfoSensor?.Id.ToString(), StringComparison.InvariantCultureIgnoreCase) is true);

                if (airInfoSensorData is null || airInfoSensorData.Data is null)
                {
                    _logger.LogWarning("Couldn't match pet's air sensor ID with sensor ID from data response.");
                    errorMsg = "No air info data found for the provided sensor ID.";
                }
                else
                {
                    UpdatePetAirInfo(pet, airInfoSensorData.Data.AirTemperature, airInfoSensorData.Data.AirHumidity);
                }

                _logger.LogInformation($"{nameof(UpdateAndGetPetAirInfo)} completed successfully.");
            }
            catch (DeserializationFailedException deserializationFailedEx)
            {
                _logger.LogError(deserializationFailedEx, "Failed to deserialize air sensor data.");
                errorMsg = "Failed to retrieve air sensor data.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve air sensor data.");
                errorMsg = "Failed to retrieve air sensor data.";
            }

            return (airInfoSensorData, errorMsg);
        }

        public async Task<(FlameDetectionSensorData? flameDetectionInfo, string? errMsg)> UpdateAndGetFlameDetectionInfo(int petId)
        {
            _logger.LogInformation($"{nameof(UpdateAndGetFlameDetectionInfo)} started...");

            var pet = GetPetById(petId);
            string? errorMsg = null;

            if (pet is null)
            {
                _logger.LogError("No pet found in DB with ID: {PetId}", petId);
                return (null, $"Pet with ID: {petId} not found.");
            }

            FlameDetectionSensorData? flameDetectionData = null;
            try
            {
                var flameDetectionSensorsResponse = await _httpClient.GetAsync(FlameDetectionDataRelativeUrl);
                flameDetectionSensorsResponse.EnsureSuccessStatusCode();

                var flameDetectionSensorsJson = await flameDetectionSensorsResponse.Content.ReadAsStringAsync();
                var allFlameDetectionSensors = JsonConvert.DeserializeObject<List<FlameDetectionSensorData>>(flameDetectionSensorsJson);

                if (allFlameDetectionSensors is null)
                {
                    throw new DeserializationFailedException();
                }

                if (allFlameDetectionSensors.Count == 0)
                {
                    _logger.LogError("No flame detection sensors data found.");
                    return (null, "No flame detection found.");
                }

                flameDetectionData = allFlameDetectionSensors.Find(d => d.SensorId?.Equals(pet.FlameDetectionSensor?.Id.ToString(), StringComparison.InvariantCultureIgnoreCase) is true);

                if (flameDetectionData is null || flameDetectionData.Data is null)
                {
                    _logger.LogWarning("Couldn't match pet's flame detection sensor ID with sensor ID from data response.");
                    errorMsg = "No flame detection data found for the provided sensor ID.";
                }
                else
                {
                    UpdateFlameDetectionInfo(pet, flameDetectionData.Data.FlameDetected);
                }

                _logger.LogInformation($"{nameof(UpdateAndGetFlameDetectionInfo)} completed successfully.");
            }
            catch (DeserializationFailedException deserializationFailedEx)
            {
                _logger.LogError(deserializationFailedEx, "Failed to deserialize flame detection sensor data.");
                errorMsg = "Failed to retrieve flame detection sensor data.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve flame detection sensor data.");
                errorMsg = "Failed to retrieve flame detection sensor data.";
            }

            return (flameDetectionData, errorMsg);
        }

        private void UpdateFlameDetectionInfo(Pet pet, bool flameDetected)
        {
            _logger.LogInformation($"{nameof(UpdateFlameDetectionInfo)} started...");

            var flameDetectionSensorEntity = new FlameDetectionData(pet, flameDetected);

            if (pet.FlameDetectionSensor is not null && pet.FlameDetectionSensor.Data is null)
                pet.FlameDetectionSensor.Data = new List<FlameDetectionData>();

            pet.FlameDetectionSensor?.Data?.Add(flameDetectionSensorEntity);

            _context.FlameDetectionData.Add(flameDetectionSensorEntity);
            _context.SaveChanges();

            _logger.LogInformation($"{nameof(UpdateFlameDetectionInfo)} completed successfully.");
        }

        private void UpdatePetAirInfo(Pet pet, decimal airTemp, decimal airHumidity)
        {
            _logger.LogInformation($"{nameof(UpdatePetAirInfo)} started...");

            var airSensorEntity = new AirInfoData(pet, airTemp, airHumidity);

            if (pet.AirInfoSensor is not null && pet.AirInfoSensor.Data is null)
                pet.AirInfoSensor.Data = new List<AirInfoData>();

            pet.AirInfoSensor?.Data?.Add(airSensorEntity);

            _context.AirInfoData.Add(airSensorEntity);
            _context.SaveChanges();

            _logger.LogInformation($"{nameof(UpdatePetAirInfo)} completed successfully.");
        }

        public Pet AddUserPet(PetDto pet)
        {
            _logger.LogInformation($"{nameof(AddUserPet)} started...");

            var petEntity = _mapper.Map<Pet>(pet);

            if (pet.AirInfoSensor.SerialNumber is null)
                petEntity.AirInfoSensor = null;

            if (pet.FlameDetectionSensor.SerialNumber is null)
                petEntity.FlameDetectionSensor = null;

            var addedPetEntity = _context.Pets.Add(petEntity).Entity;
            var user = _context.Users
                .Include(p => p.Pets)
                .FirstOrDefault(u => u.Id == pet.OwnerId);

            if (user is null)
            {
                _logger.LogError("An error has happened with getting the pet's owner while saving the pet.");
                throw new UserNotFoundException();
            }

            user.Pets.Add(petEntity);

            _context.SaveChanges();

            _logger.LogInformation($"{nameof(AddUserPet)} completed successfully.");

            return addedPetEntity;
        }

        public ICollection<PetDto> GetUserPets(string userId)
        {
            _logger.LogInformation($"{nameof(GetUserPets)} started...");

            var userPets = _context.Pets
                .Include(p => p.GpsDevice)
                .Include(p => p.PetLocations)
                .Include(p => p.CenterLocation)
                .Include(p => p.AirInfoSensor)
                .Include(p => p.AirInfoData)
                .Include(p => p.FlameDetectionSensor)
                .Include(p => p.FlameDetectionData)
                .Where(u => u.OwnerId == userId)
                .AsSplitQuery()
                .ToList();

            if (userPets is null)
            {
                _logger.LogError("An error has happened while getting the user's pets. User ID: {UserId}", userId);
                throw new UserNotFoundException();
            }
                
            var userPetsDto = _mapper.Map<ICollection<Pet>, ICollection<PetDto>>(userPets);

            _logger.LogInformation($"{nameof(GetUserPets)} completed successfully.");

            return userPetsDto;
        }

        public void DeletePet(int petId)
        {
            _logger.LogInformation($"{nameof(DeletePet)} started...");

            var petToDelete = GetPetById(petId);

            if (petToDelete is null)
            {
                _logger.LogError("No pet found in DB with ID: {PetId}", petId);
                return;
            }

            _context.Pets.Remove(petToDelete);
            _context.SaveChanges();

            _logger.LogInformation($"{nameof(DeletePet)} completed successfully.");
        }

        public Pet? UpdatePet(PetDto pet)
        {
            _logger.LogInformation($"{nameof(UpdatePet)} started...");

            var petToUpdate = GetPetById(pet.Id);

            if (petToUpdate is not null)
            {
                petToUpdate.Name = pet.Name ?? petToUpdate.Name;
                petToUpdate.Kind = pet.Kind.ToString();
                petToUpdate.Gender = pet.Gender.ToString();
                petToUpdate.Age = pet.Age;
                petToUpdate.Breed = pet.Breed;
                petToUpdate.Sterilized = pet.Sterilized;
                petToUpdate.Weight = pet.Weight;
                if (petToUpdate.AirInfoSensor?.Id != pet.AirInfoSensor.Id)
                {
                    petToUpdate.AirInfoSensor = _mapper.Map<AirInfoSensor>(pet.AirInfoSensor);
                }
                if (petToUpdate.FlameDetectionSensor?.Id != pet.FlameDetectionSensor.Id)
                {
                    petToUpdate.FlameDetectionSensor = _mapper.Map<FlameDetectionSensor>(pet.FlameDetectionSensor);
                }
                petToUpdate.CenterLocation = _mapper.Map<PetAreaCenterLocation>(pet.CenterLocation);
                petToUpdate.Radius = pet.Radius;

                _context.SaveChanges();

                _logger.LogInformation($"{nameof(UpdatePet)} completed successfully.");

                return petToUpdate;
            }

            _logger.LogError($"{nameof(UpdatePet)} failed.");
            return null;
        }

        public Pet? GetPetById(int petId)
        {
            return _context.Pets
                .Include(p => p.GpsDevice)
                .Include(p => p.PetLocations)
                .Include(p => p.AirInfoSensor)
                .Include(p => p.FlameDetectionSensor)
                .Include(p => p.CenterLocation)
                .AsSplitQuery()
                .FirstOrDefault(p => p.Id == petId);
        }

        public List<Pet> GetAllPets()
        {
            return _context.Pets
                .Include(p => p.GpsDevice)
                .Include(p => p.PetLocations)
                .Include(p => p.AirInfoSensor)
                .Include(p => p.FlameDetectionSensor)
                .Include(p => p.CenterLocation)
                .Include(p => p.Owner)
                .AsSplitQuery()
                .ToList();
        }

        public PetStatistics? GetPetStatistics(DateTime dateFrom, DateTime dateTo, Pet pet)
        {
            _logger.LogInformation($"{nameof(GetPetStatistics)} started...");

            var petLocations = _context.PetLocations
                .Where(loc => loc.PetId == pet.Id)
                .Where(loc => loc.Timestamp >= dateFrom)
                .Where(loc => loc.Timestamp <= dateTo)
                .ToArray();

            if (!petLocations.Any())
            {
                _logger.LogWarning("No pet locations for pet {Name}. ID: {Id}", pet.Name, pet.Id);
                return null;
            }

            var distanceSum = 0d;
            for (var i = 0; i < petLocations.Length - 1; i++)
            {
                var loc1 = petLocations[i];
                var loc2 = petLocations[i + 1];

                distanceSum += DistanceCalculatorService.HaversineDistanceInMeters(loc1.Longitude, loc1.Latitude, loc2.Longitude, loc2.Latitude);
            }

            var petStatistics = new PetStatistics
            {
                DistanceTravelled = distanceSum,
                Pet = _mapper.Map<PetDto>(pet)
            };

            if (pet.AirInfoSensor is not null)
            {
                var petRoomAirInfoData = _context.AirInfoData
                    .Where(d => d.SensorId == pet.AirInfoSensor.Id)
                    .Where(d => d.Timestamp >= dateFrom)
                    .Where(d => d.Timestamp <= dateTo);

                var avgAirTemp = petRoomAirInfoData.Average(d => d.AirTemperature);
                var avgAirHumidity = petRoomAirInfoData.Average(d => d.AirHumidity);

                petStatistics.AvgRoomAirTemperature = avgAirTemp;
                petStatistics.AvgRoomAirHumidity = avgAirHumidity;
            }

            var petIdParam = new SqlParameter("@PetId", pet.Id);
            var datetimeFromParam = new SqlParameter("@DateTimeFrom", dateFrom);
            var datetimeToParam = new SqlParameter("@DateTimeTo", dateTo);
            var movementTypeParamActive = new SqlParameter("@MovementType", "Active");
            var movementTypeParamRest = new SqlParameter("@MovementType", "Rest");

            var activeMovementInfo = _context.PetMovement
                .FromSqlRaw("EXECUTE GetPetsMovementTime @PetId, @MovementType, @DateTimeFrom, @DateTimeTo", petIdParam, movementTypeParamActive, datetimeFromParam, datetimeToParam)
                .AsEnumerable()
                .FirstOrDefault();
            var restingMovementInfo = _context.PetMovement
                .FromSqlRaw("EXECUTE GetPetsMovementTime @PetId, @MovementType, @DateTimeFrom, @DateTimeTo", petIdParam, movementTypeParamRest, datetimeFromParam, datetimeToParam)
                .AsEnumerable()
                .FirstOrDefault();

            if (activeMovementInfo?.MovementSeconds is not null && restingMovementInfo?.MovementSeconds is not null)
            {
                var activeTime = TimeSpan.FromSeconds(activeMovementInfo.MovementSeconds);
                var restingTime = TimeSpan.FromSeconds(restingMovementInfo.MovementSeconds);

                petStatistics.ActiveTime = activeTime;
                petStatistics.RestingTime = restingTime;
            }
           
            _logger.LogInformation($"{nameof(GetPetStatistics)} completed successfully.");

            return petStatistics;
        }

        public void UpdatePetMovementTime(Pet pet, int? restingTime = null, int? activeTime = null)
        {
            _logger.LogInformation($"{nameof(UpdatePetMovementTime)} started...");

            if (restingTime is not null)
                pet.RestingTimeSeconds += (int)restingTime;

            if (activeTime is not null)
                pet.ActiveTimeSeconds += (int)activeTime;

            _logger.LogInformation($"{nameof(UpdatePetMovementTime)} completed successfully.");
        }
    }
}
