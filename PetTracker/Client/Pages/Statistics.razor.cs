using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PetTracker.Shared.Models;

namespace PetTracker.Client.Pages
{
    public partial class Statistics
    {
        [Inject]
        private HttpClient HttpClient { get; set; } = default!;

        private List<PetDto>? UserPets { get; set; }
        private PetStatisticsRequest PetStatisticsRequest { get; set; } = new();
        private PetStatistics? PetStatistics { get; set; }
        private bool PetsLoaded { get; set; }
        private bool DateFromLaterThanTo { get; set; }
        private bool? PetStatisticsLoaded { get; set; }

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
            }

            PetsLoaded = true;
        }

        private async Task HandleValidSubmit()
        {
            DateFromLaterThanTo = false;

            if (DateTime.Compare((DateTime)PetStatisticsRequest.From!, (DateTime)PetStatisticsRequest.To!) > 0)
            {
                DateFromLaterThanTo = true;
                return;
            }

            PetStatisticsLoaded = false;

            var petStatisticsResponse = await HttpClient.GetAsync($"api/pets/statistics?" +
                $"dateFrom={PetStatisticsRequest.From.Value.ToUniversalTime()}" +
                $"&dateTo={PetStatisticsRequest.To.Value.ToUniversalTime()}" +
                $"&petId={PetStatisticsRequest.PetId}");

            if (petStatisticsResponse.IsSuccessStatusCode)
			{
                var petStatisticsResponseContent = await petStatisticsResponse.Content.ReadAsStringAsync();
                PetStatistics = JsonConvert.DeserializeObject<PetStatistics>(petStatisticsResponseContent);
			}

            PetStatisticsLoaded = true;
        }
    }
}
