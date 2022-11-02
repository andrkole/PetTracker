using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PetTracker.Shared.Models;
using System.Text;

namespace PetTracker.Client.Pages
{
    public partial class Pets
    {
        [Inject]
        private HttpClient HttpClient { get; set; } = default!;
        [Inject]
        IToastService ToastService { get; set; } = default!;

        private List<PetDto>? UserPets { get; set; }
        private PetDto Pet { get; set; } = new();
        private bool IsPetEdit { get; set; } = false;
        private bool? IsGpsSNValid { get; set; }
        private bool? IsAirSensorSNValid { get; set; }
        private bool? IsFlameDetectionSensorSNValid { get; set; }
        private bool IsModalActive { get; set; } = false;
        private bool? PetsLoaded { get; set; }
        private bool IsConfirmDeleteModalActive { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await GetUserPets();
        }

        private async Task GetUserPets()
        {
            PetsLoaded = false;

            var userPetsResponse = await HttpClient.GetAsync("api/pets");
            if (userPetsResponse.IsSuccessStatusCode)
            {
                var userPetsJson = await userPetsResponse.Content.ReadAsStringAsync();
                UserPets = JsonConvert.DeserializeObject<List<PetDto>>(userPetsJson);

                if (UserPets is not null)
                {
                    foreach (var pet in UserPets)
                    {
                        pet.FirstLocationAsCenter = pet.CenterLocation?.Longitude == pet.Location?.Longitude && pet.CenterLocation?.Latitude == pet.Location?.Latitude;                        
                    }
                }
            }

            PetsLoaded = true;
        }

        private void TogglePetModal() => IsModalActive = !IsModalActive;

        private void ToggleAddNewPetModal()
        {
            IsModalActive = !IsModalActive;
            IsPetEdit = false;
            IsGpsSNValid = null;
            IsAirSensorSNValid = null;
            IsFlameDetectionSensorSNValid = null;
            Pet = new();
        }

        private async Task HandleValidSubmit()
        {
            var petJson = JsonConvert.SerializeObject(Pet);
            var petContent = new StringContent(petJson, Encoding.UTF8, "application/json");

            if (IsPetEdit)
            {
                try
                {                 
                    var updatePetResp = await HttpClient.PutAsync("api/pets", petContent);
                    updatePetResp.EnsureSuccessStatusCode();

                    ToastService.ShowSuccess("Ljubimac uspješno ažuriran.", "Uspjeh");
                }
                catch
                {
                    ToastService.ShowError("Neuspješno ažuriranje ljubimca.", "Greška");
                    TogglePetModal();
                    return;
                }
            }
            else
            {
                try
                {
                    var addPetResp = await HttpClient.PostAsync("api/pets", petContent);
                    addPetResp.EnsureSuccessStatusCode();

                    ToastService.ShowSuccess("Ljubimac uspješno dodan.", "Uspjeh");
                }
                catch
                {
                    ToastService.ShowError("Neuspješno dodavanje ljubimca.", "Greška");
                    TogglePetModal();
                    return;
                }
            }

            TogglePetModal();

            await GetUserPets();

            StateHasChanged();
        }

        private async Task DeletePet(PetDto pet)
        {
            UserPets?.Remove(pet);
            var petDeleteResponse = await HttpClient.DeleteAsync($"api/pets?petId={pet.Id}");

            if (petDeleteResponse.IsSuccessStatusCode)
                ToastService.ShowSuccess("Ljubimac uspješno izbrisan.", "Uspjeh");
            else
                ToastService.ShowError("Neuspješno brisanje ljubimca.", "Greška");

            ToggleConfirmDeletePetModal();
        }

        private void EditPet(PetDto pet)
        {
            IsPetEdit = true;
            IsGpsSNValid = null;
            IsAirSensorSNValid = null;
            IsFlameDetectionSensorSNValid = null;
            Pet = pet;
            TogglePetModal();
        }

        private async Task ValidateGpsSN()
        {
            var snValidationResponse = await HttpClient.GetAsync($"api/pets/gps/validate?serialNumber={Pet.GpsDevice.SerialNumber}");

            IsGpsSNValid = snValidationResponse.IsSuccessStatusCode;
        }

        private async Task ValidateAirSensorSN()
        {
            var snValidationResponse = await HttpClient.GetAsync($"api/airsensors/validate?serialNumber={Pet.AirInfoSensor.SerialNumber}");

            IsAirSensorSNValid = snValidationResponse.IsSuccessStatusCode;
        }

        private async Task ValidateFlameDetectionSensorSn()
        {
            var snValidationResponse = await HttpClient.GetAsync($"api/flamedetectionsensors/validate?serialNumber={Pet.FlameDetectionSensor.SerialNumber}");

            IsFlameDetectionSensorSNValid = snValidationResponse.IsSuccessStatusCode;
        }

        private void ToggleConfirmDeletePetModal() => IsConfirmDeleteModalActive = !IsConfirmDeleteModalActive;
    }
}
