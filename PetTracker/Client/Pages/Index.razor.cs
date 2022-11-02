using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using PetTracker.Shared.Constants;
using PetTracker.Shared.Extensions;
using PetTracker.Shared.Models;

namespace PetTracker.Client.Pages
{
    public partial class Index : IDisposable
    {
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Inject]
        private HttpClient HttpClient { get; set; } = default!;
        private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromSeconds(SharedConstants.SecondsRefreshTime));

        private GoogleMap? petMap = new();
        private MapOptions petMapOptions = new();
        private List<PetDto>? UserPets { get; set; }
        private Dictionary<string, Marker>? PetMarkers { get; set; }
        private Dictionary<string, InfoWindow>? PetInfoWindows { get; set; }
        private bool MarkersLoaded { get; set; }
        private bool? PetsLoaded { get; set; }
        private PetDto? SelectedPet { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var currentUser = authState.User.Identity;

            if (currentUser?.IsAuthenticated == true)
            {
                MarkersLoaded = false;
                await GetUserPetsAndLocations();
                RunRetrieveLocationsTimer();
            }
        }

        private async Task GetUserPetsAndLocations()
        {
            PetsLoaded = false;

            var userPetsResponse = await HttpClient.GetAsync("api/pets");

            if (!userPetsResponse.IsSuccessStatusCode)
            {
                PetsLoaded = true;
                return;
            }

            var userPetsJson = await userPetsResponse.Content.ReadAsStringAsync();
            var userPets = JsonConvert.DeserializeObject<List<PetDto>>(userPetsJson);

            if (userPets is not null)
            {
                UserPets = userPets;
                await CreateMapForPets();
            }

            PetsLoaded = true;
            StateHasChanged();
        }

        private async Task CreateMapForPets()
        {
            var pet = UserPets!.FirstOrDefault();
            if (pet is null)
                return;

            if (pet.Location is null)
                await GetPetsLatestLocation();

            var defaultPetLocation = UserPets?.FirstOrDefault()?.Location;

            petMapOptions = new MapOptions()
            {
                Zoom = 22,
                Center = new LatLngLiteral()
                {
                    Lat = defaultPetLocation?.Latitude ?? 43.52321,
                    Lng = defaultPetLocation?.Longitude ?? 16.45058
                },
                ClickableIcons = false,
                StreetViewControl = false,
                MapTypeId = MapTypeId.Satellite
            };
        }

        private async Task GetPetsLatestLocation()
        {
            foreach (var pet in UserPets!)
            {
                if (string.IsNullOrEmpty(pet.GpsDevice?.Id.ToString()))
                    continue;

                var petLocationResponse = await HttpClient.GetAsync($"api/pets/location?petId={pet.Id}");
                if (!petLocationResponse.IsSuccessStatusCode)
                    continue;

                var petLocationJson = await petLocationResponse.Content.ReadAsStringAsync();
                var petLocation = JsonConvert.DeserializeObject<PetLocationDto>(petLocationJson);

                if (petLocation is not null)
                {
                    pet.Location = petLocation;
                    pet.LastSynchronization = petLocation.LastSynchronization;
                }
            }
        }

        private async Task GetLatestPetRoomAirInfo()
        {
            foreach (var pet in UserPets!)
            {
                if (pet.AirInfoSensor.Id == Guid.Empty)
                    continue;

                var airInfoResponse = await HttpClient.GetAsync($"api/pets/air-info?petId={pet.Id}");
                if (!airInfoResponse.IsSuccessStatusCode)
                    continue;

                var airInfoJson = await airInfoResponse.Content.ReadAsStringAsync();
                var airInfo = JsonConvert.DeserializeObject<AirInfoSensorData>(airInfoJson);

                if (airInfo is not null)
                    pet.AirInfoSensorData = airInfo;
            }
        }

        private async Task GetLatestFlameDetectionInfo()
        {
            foreach (var pet in UserPets!)
            {
                if (pet.FlameDetectionSensor.Id == Guid.Empty)
                    continue;

                var flameDetectionInfoResponse = await HttpClient.GetAsync($"api/pets/flame-detection?petId={pet.Id}");
                if (!flameDetectionInfoResponse.IsSuccessStatusCode)
                    continue;

                var flameDetectionInfoJson = await flameDetectionInfoResponse.Content.ReadAsStringAsync();
                var flameDetectionInfo = JsonConvert.DeserializeObject<FlameDetectionSensorData>(flameDetectionInfoJson);

                if (flameDetectionInfo is not null)
                    pet.FlameDetectionSensorData = flameDetectionInfo;
            }
        }

        private async Task CreatePetMarkers()
        {
            foreach (var pet in UserPets!)
            {
                if (pet.Location is null)
                    continue;

                var marker = await Marker.CreateAsync(petMap!.JsRuntime, new MarkerOptions
                {
                    Position = new LatLngLiteral(pet.Location.Latitude, pet.Location.Longitude),
                    Map = petMap.InteropObject,
                    Clickable = true,
                    Title = pet.Id.ToString(),
                    Label = new MarkerLabel
                    {
                        Text = pet.Name ?? string.Empty,
                        FontWeight = "bold",
                        FontSize = "24"
                    },
                });

                var infoWindow = await CreateMapInfoWindow(pet);

                await marker.AddListener("click", async () =>
                {
                    await infoWindow.Open(petMap.InteropObject);
                });

                PetMarkers ??= new Dictionary<string, Marker>();
                PetInfoWindows ??= new Dictionary<string, InfoWindow>();

                PetMarkers.Add(pet.GpsDevice!.Id.ToString(), marker);
                PetInfoWindows.Add(pet.GpsDevice.Id.ToString(), infoWindow);
            }

            MarkersLoaded = true;
        }

        private async Task<InfoWindow> CreateMapInfoWindow(PetDto pet)
        {
            var infoWindowContent =
                    $@"<table class='table is-striped'>
                        <tbody>
                            <tr>
                                <td><b>Ime</b></td>
                                <td>{pet.Name}</td>
                            </tr>
                            <tr>
                                <td><b>Starost</b></td>
                                <td>{(pet.Age.HasValue ? pet.Age.Value.ToString("0.#") : "N/A")} god.</td>
                            </tr>
                            <tr>
                                <td><b>Vrsta</b></td>
                                <td>{pet.Kind?.GetDisplayName()}</td>
                            </tr>
                            <tr>
                                <td><b>Temp. zraka</b></td>
                                <td>{(pet.AirInfoSensorData?.Data?.AirTemperature is null ? "N/A" : pet.AirInfoSensorData.Data.AirTemperature + "°C")}</td>
                            </tr>
                            <tr>
                                <td><b>Vlažnost zraka</b></td>
                                <td>{(pet.AirInfoSensorData?.Data?.AirHumidity is null ? "N/A" : pet.AirInfoSensorData.Data.AirHumidity + "%")}</td>
                            </tr>
                        </tbody>
                    </table>";

            var infoWindow = await InfoWindow.CreateAsync(petMap!.JsRuntime, new InfoWindowOptions
            {
                Position = new LatLngLiteral
                {
                    Lat = pet.Location!.Latitude,
                    Lng = pet.Location!.Longitude
                },
                Content = infoWindowContent,
                PixelOffset = new Size
                {
                    Height = -50
                }
            });

            return infoWindow;
        }

        private async Task UpdatePetMarkers()
        {
            if (PetMarkers is null || !PetMarkers.Any() || PetInfoWindows is null)
                return;

            foreach (var pet in UserPets!)
            {
                if (PetMarkers.TryGetValue(pet.GpsDevice!.Id.ToString(), out var marker)
                    && PetInfoWindows.TryGetValue(pet.GpsDevice!.Id.ToString(), out var infoWindow)
                    && pet.Location is not null)
                {
                    var latLng = new LatLngLiteral(pet.Location.Latitude, pet.Location.Longitude);
                    await marker.SetPosition(latLng);
                    await infoWindow.SetPosition(latLng);
                }
            }

            if (SelectedPet is not null && petMap is not null) 
            {
                var selectedPetlatLngCenter = new LatLngLiteral
                {
                    Lat = SelectedPet!.Location!.Latitude,
                    Lng = SelectedPet.Location.Longitude
                };

                await petMap.InteropObject.SetCenter(selectedPetlatLngCenter);
            }                
        }

#pragma warning disable S3168 // Periodic timer must be "fire and forget"
        private async void RunRetrieveLocationsTimer()
#pragma warning restore S3168
        {
            while (await _periodicTimer.WaitForNextTickAsync())
            {
                await GetPetsLatestLocation();
                await GetLatestPetRoomAirInfo();
                await GetLatestFlameDetectionInfo();
                await UpdatePetMarkers();
                StateHasChanged();
            }
        }

        private async Task SetMapCenter(ChangeEventArgs e)
        {
            var petId = Convert.ToInt32(e.Value);
            SelectedPet = UserPets!.Find(p => p.Id == petId);

            var latLngCenter = new LatLngLiteral
            {
                Lat = SelectedPet!.Location!.Latitude,
                Lng = SelectedPet.Location.Longitude
            };

            await petMap!.InteropObject.SetCenter(latLngCenter);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _periodicTimer.Dispose();
        }
    }
}
