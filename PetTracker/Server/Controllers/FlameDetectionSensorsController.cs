using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTracker.Server.Interfaces;

namespace PetTracker.Server.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class FlameDetectionSensorsController : ControllerBase
    {
        private IFlameDetectionSensorsRepository _flameDetectionSensorsRepository;

        public FlameDetectionSensorsController(IFlameDetectionSensorsRepository flameDetectionSensorsRepository)
        {
            _flameDetectionSensorsRepository = flameDetectionSensorsRepository;
        }

        [HttpGet("validate")]
        public IActionResult ValidateNewAirSensor(string serialNumber)
        {
            if (serialNumber.Length != 6)
                return BadRequest("Serial number is not valid!");

            var isValid = !_flameDetectionSensorsRepository.IsDeviceInUse(serialNumber);

            return isValid ? Ok() : BadRequest("Serial number is not valid!");
        }
    }
}
