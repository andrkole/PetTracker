using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PetTracker.Shared.Models;
using System.Text;

namespace PetTracker.Client.Pages
{
    public partial class AdminPanel 
    {
        [Inject]
        private HttpClient HttpClient { get; set; } = default!;
		[Inject]
		private IToastService ToastService { get; set; } = default!;

        private List<UserDto>? Users { get; set; }
		private UserDto User { get; set; } = new();
		private bool IsEditUserModalActive { get; set; } = false;
		private bool IsConfirmDeleteModalActive { get; set; } = false;
		private int PageSize { get; set; } = 10;
		private int TotalPages { get; set; } = 1;
		private int CurrentPage { get; set; } = 1;
		private bool? UsersLoaded { get; set; }

        protected override async Task OnInitializedAsync()
		{
			await GetAllUsers();
		}

        private async Task GetAllUsers()
		{
			UsersLoaded = false;

			var allUsersResponse = await HttpClient.GetAsync("api/admin/users");
			if (allUsersResponse.IsSuccessStatusCode)
			{
				var allUsersJson = await allUsersResponse.Content.ReadAsStringAsync();
				var allUsers = JsonConvert.DeserializeObject<List<UserDto>>(allUsersJson);
				if (allUsers is not null)
				{
					TotalPages = (int)Math.Ceiling(decimal.Divide(allUsers.Count, PageSize));
					var skipCount = (CurrentPage - 1) * PageSize;
					Users = allUsers
						.Skip(skipCount)
						.Take(PageSize)
						.ToList();
				}
			}

			UsersLoaded = true;
		}

		private void ToggleEditUserModal() => IsEditUserModalActive = !IsEditUserModalActive;

		private void ToggleConfirmDeleteUserModal() => IsConfirmDeleteModalActive = !IsConfirmDeleteModalActive;

		private async Task HandleValidSubmit()
		{
			var userJson = JsonConvert.SerializeObject(User);
			var userContent = new StringContent(userJson, Encoding.UTF8, "application/json");

			await HttpClient.PutAsync("api/admin/users", userContent);

			ToggleEditUserModal();

			ToastService.ShowSuccess("Korisnik uspješno ažuriran.", "Uspjeh");

			await GetAllUsers();

			StateHasChanged();
		}

		private void EditUser(UserDto user)
        {
			User = user;
			ToggleEditUserModal();
        }

		private async Task DeleteUser(UserDto user)
        {
			Users?.Remove(user);
			var userDeleteResponse = await HttpClient.DeleteAsync($"api/admin/users?userId={user.Id}");

			if (userDeleteResponse.IsSuccessStatusCode)
				ToastService.ShowSuccess("Korisnik uspješno obrisan.", "Uspjeh");
			else
				ToastService.ShowError("Neuspješno brisanje korisnika.", "Greška");

			ToggleConfirmDeleteUserModal();
		}

		protected async Task FilterUsers(string? value, string columnName)
        {
			if (!string.IsNullOrEmpty(value))
            {
				await GetAllUsers();

				Users = columnName switch
				{
					"Id" => Users?.Where(u => u.Id.Contains(value, StringComparison.InvariantCultureIgnoreCase)).ToList(),
					"UserName" => Users?.Where(u => u.UserName.Contains(value, StringComparison.InvariantCultureIgnoreCase)).ToList(),
					"FirstName" => Users?.Where(u => u.FirstName?.Contains(value, StringComparison.InvariantCultureIgnoreCase) == true).ToList(),
					"LastName" => Users?.Where(u => u.LastName?.Contains(value, StringComparison.InvariantCultureIgnoreCase) == true).ToList(),
					"Email" => Users?.Where(u => u.Email?.Contains(value, StringComparison.InvariantCultureIgnoreCase) == true).ToList(),
					_ => Users
				};
            }
			else
            {
				await GetAllUsers();
            }
        }

		private async Task SelectedPage(int selectedPage)
		{
			CurrentPage = selectedPage;
			await GetAllUsers();
		}
	}
}
