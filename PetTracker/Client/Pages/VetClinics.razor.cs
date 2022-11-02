using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PetTracker.Shared.Models;

namespace PetTracker.Client.Pages
{
    public partial class VetClinics
    {
        [Inject]
        private HttpClient HttpClient { get; set; } = default!;

        private List<PetDto>? UserPets { get; set; }
        private List<GoogleMapsPlaceInfo>? ClosestVetClinics { get; set; }
        private bool? PetsLoaded { get; set; }
        private bool? VetClinicsLoaded { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            await GetUserPets();
        }

        private async Task GetUserPets()
        {
            PetsLoaded = false;

            var userPetsResponse = await HttpClient.GetAsync("api/pets");

            if (!userPetsResponse.IsSuccessStatusCode)
            {
                PetsLoaded = true;
                return;
            }

            var userPetsJson = await userPetsResponse.Content.ReadAsStringAsync();
            UserPets = JsonConvert.DeserializeObject<List<PetDto>>(userPetsJson);

            PetsLoaded = true;
        }

        private async Task GetClosestVetClinicsForPet(ChangeEventArgs e)
        {
            if (string.IsNullOrEmpty(e.ToString()))
                return;

            VetClinicsLoaded = false;

            var petId = Convert.ToInt32(e.Value);
            var selectedPet = UserPets!.Find(p => p.Id == petId);

            var closestVetClinicsResponse = await HttpClient.GetAsync($"api/gps/clinics?" +
                $"latitude={selectedPet!.Location!.Latitude}" +
                $"&longitude={selectedPet.Location.Longitude}" +
                $"&radius=3000");

            var closestVetClinicsContent = await closestVetClinicsResponse.Content.ReadAsStringAsync();

            ClosestVetClinics = JsonConvert.DeserializeObject<List<GoogleMapsPlaceInfo>>(closestVetClinicsContent);

            VetClinicsLoaded = true;
        }
    }
}
