using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetTracker.Server.Interfaces;
using PetTracker.Shared.Models;

namespace PetTracker.Server.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class GpsController : ControllerBase
    {
        private IGpsRepository _gpsRepository;

        public GpsController(IGpsRepository gpsRepository)
        {
            _gpsRepository = gpsRepository;
        }

        [HttpGet("validate")]
        public IActionResult ValidateNewGpsDevice(string serialNumber)
        {
            if (serialNumber.Length != 8)
                return BadRequest("Serial number is not valid!");

            var isValid = !_gpsRepository.IsDeviceInUse(serialNumber);

            return isValid ? Ok() : BadRequest("Serial number is not valid!");
        }

        [HttpGet("clinics")]
        public async Task<ActionResult<List<GoogleMapsPlaceInfo>>> GetVetClinics(double longitude, double latitude, int radius)
        {
            if (radius > 50000)
                return BadRequest("Radius cannot exceed 50 kilometers.");

            var closestVetClinics = await _gpsRepository.GetClosestVetClinics(latitude, longitude, radius);

            return Ok(closestVetClinics);
        }
    }
}
