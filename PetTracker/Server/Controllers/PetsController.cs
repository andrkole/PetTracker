using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetTracker.Server.Entities;
using PetTracker.Server.Interfaces;
using PetTracker.Shared.Models;
using System.Security.Claims;

namespace PetTracker.Server.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly IPetsRepository _petsRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public PetsController(IPetsRepository petsRepository, UserManager<ApplicationUser> userManager)
        {
            _petsRepository = petsRepository;
            _userManager = userManager;
        }

        [HttpGet("location")]
        public ActionResult<PetLocationDto> GetPetLocation(int petId)
        {
            var (petLocation, errorMsg) = _petsRepository.GetLatestPetLocation(petId);

            if (petLocation is null)
                return BadRequest(errorMsg);

            return Ok(petLocation);
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<PetDto>>> GetUserPets()
        {
            var user = await GetLoggedInUser();

            var userPets = _petsRepository.GetUserPets(user.Id);

            return Ok(userPets);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserPet([FromBody] PetDto pet)
		{
            if (pet is null)
                return BadRequest();

            var user = await GetLoggedInUser();

            pet.OwnerId = user.Id;

            try
            {
                var createdPet = _petsRepository.AddUserPet(pet);
                return Created("pets", createdPet);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error has occurred while adding a new pet.");
            }
		}

        [HttpPut]
        public IActionResult UpdatePet([FromBody] PetDto pet)
        {
            if (pet is null)
                return BadRequest();

            var petToUpdate = _petsRepository.GetPetById(pet.Id);

            if (petToUpdate is null)
                return BadRequest($"Pet with ID {pet.Id} is not found.");

            _petsRepository.UpdatePet(pet);

            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeletePet(int petId)
        {
            _petsRepository.DeletePet(petId);

            return NoContent();
        }

        private async Task<ApplicationUser> GetLoggedInUser()
        {
            var nameId = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var loggedInUser = await _userManager.FindByIdAsync(nameId);

            return loggedInUser;
        }

        [HttpGet("air-info")]
        public async Task<ActionResult> GetPetRoomAirInfo(int petId)
        {
            var (petRoomAirInfo, errorMsg) = await _petsRepository.UpdateAndGetPetAirInfo(petId);

            if (petRoomAirInfo is null)
                return BadRequest(errorMsg);

            return Ok(petRoomAirInfo);
        }

        [HttpGet("flame-detection")]
        public async Task<ActionResult> GetFlameDetectionInfo(int petId)
        {
            var (petFlameDetectionInfo, errorMsg) = await _petsRepository.UpdateAndGetFlameDetectionInfo(petId);

            if (petFlameDetectionInfo is null)
                return BadRequest(errorMsg);

            return Ok(petFlameDetectionInfo);
        }

		[HttpGet("statistics")]
        public ActionResult<PetStatistics> GetPetStatistics(DateTime dateFrom, DateTime dateTo, int petId)
		{
            var pet = _petsRepository.GetPetById(petId);

            if (pet is null)
                return BadRequest($"Pet with ID {petId} is not found.");

            var petStatistics = _petsRepository.GetPetStatistics(dateFrom, dateTo, pet);

            return Ok(petStatistics);
        }
    }
}
