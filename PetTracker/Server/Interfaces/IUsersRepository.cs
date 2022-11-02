using Microsoft.AspNetCore.Identity;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Interfaces
{
    public interface IUsersRepository
    {
        ICollection<UserDto> GetAllUsers();
        Task<IdentityResult> UpdateUser(UserDto user);
        Task<IdentityResult> DeleteUser(string userId);
    }
}
