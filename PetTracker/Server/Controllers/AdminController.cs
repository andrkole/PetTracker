using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTracker.Server.Interfaces;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private IUsersRepository _usersRepository;

        public AdminController(IUsersRepository usersRepository)
		{
            _usersRepository = usersRepository;
		}

		[HttpGet("users")]
        public IActionResult GetAllUsers()
		{
            var allUsers = _usersRepository.GetAllUsers();

            if (allUsers.Count == 0)
                return NotFound("No users found.");

            return Ok(allUsers);
		}

        [HttpPut("users")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto user)
        {
            if (user is null)
                return BadRequest();

            var updateUserResult = await _usersRepository.UpdateUser(user);

            if (updateUserResult.Succeeded)
                return NoContent();
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred while updating the user.");
        }

        [HttpDelete("users")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var deleteUserResult = await _usersRepository.DeleteUser(userId);

            if (deleteUserResult.Succeeded)
                return NoContent();
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred while deleting the user.");
        }
    }
}
