using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetTracker.Server.Data;
using PetTracker.Server.Entities;
using PetTracker.Server.Exceptions;
using PetTracker.Server.Interfaces;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ILogger<UsersRepository> _logger;
        private readonly PetTrackerDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersRepository(ILogger<UsersRepository> logger, PetTrackerDbContext context,
            IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public ICollection<UserDto> GetAllUsers()
        {
            var allUsers = _context.Users
                .Include(u => u.Pets)
                .ThenInclude(p => p.GpsDevice)
                .ToList();
 
            if (!allUsers.Any())
			{
                _logger.LogError("No users found in the database.");
                throw new UserNotFoundException();
			}

            var allUsersDto = _mapper.Map<ICollection<ApplicationUser>, ICollection<UserDto>>(allUsers);

            return allUsersDto;
        }

        public async Task<IdentityResult> UpdateUser(UserDto user)
        {
            var userToUpdate = await _userManager.FindByIdAsync(user.Id);

            if (userToUpdate is null)
            {
                _logger.LogError("No user found in DB with ID: {Id}", user.Id);
                return IdentityResult.Failed();
            }

            userToUpdate.FirstName = user.FirstName!;
            userToUpdate.LastName = user.LastName;
            userToUpdate.PhoneNumber = user.PhoneNumber;
            userToUpdate.Email = user.Email;

            var result = await _userManager.UpdateAsync(userToUpdate);

            if (!result.Succeeded)
            {
                _logger.LogError("Failed to update user: {UserName}", userToUpdate.UserName);
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Error code: {Code}. Description: {Description}", error.Code, error.Description);
                }
            }

            return result;
        }

        public async Task<IdentityResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                _logger.LogError("No user found in DB with ID: {UserId}", userId);
                return IdentityResult.Failed();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError("Failed to delete user: {UserName}", user.UserName);
                foreach (var error in result.Errors)
                {
                    _logger.LogError("Error code: {Code}. Description: {Description}", error.Code, error.Description);
                }
            }

            return result;
        }
    }
}
