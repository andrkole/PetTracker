using NETCore.MailKit.Core;
using PetTracker.Server.Interfaces;

namespace PetTracker.Server.Services
{
    public class GetAndCheckPetsLocationService
    {
        private readonly ILogger<GetAndCheckPetsLocationService> _logger;
        private readonly IPetsRepository _petsRepository; 
        private readonly IEmailService _emailService;

        public GetAndCheckPetsLocationService(ILogger<GetAndCheckPetsLocationService> logger, IPetsRepository petsRepository, 
            IEmailService emailService)
        {
            _logger = logger;
            _petsRepository = petsRepository;
            _emailService = emailService;
        }

        public async Task CheckSensorsAndSendAlert()
        {
            _logger.LogInformation($"{nameof(CheckSensorsAndSendAlert)} has started...");

            var allPets = _petsRepository.GetAllPets();

            foreach (var pet in allPets)
            {
                var latestPetLocation = await _petsRepository.UpdateAndGetPetLocation(pet);
                var (flameDetectionInfo, _) = await _petsRepository.UpdateAndGetFlameDetectionInfo(pet.Id);

                if (latestPetLocation is null)
                {
                    _logger.LogError("Latest pet location is null. Pet ID: {Id}", pet.Id);
                    continue;
                }

                if (pet.CenterLocation is null)
                {
                    _logger.LogError("Pet center location is null. Pet ID: {Id}", pet.Id);
                    continue;
                }

                if (latestPetLocation.LeftSafetyRadius)
                {
                    var message = $"<h1> Ljubimac {pet.Name} je napustio radius sigurnosne zone (radius {pet.Radius} metara) koji ste postavili. <h1/> <br>" +
                        "<h2 style='color:red;'> Molimo da provjerite sigurnost svog ljubimca! </h2>";

                    await _emailService.SendAsync(pet.Owner!.Email, "Pet Tracker Location Alert", message, true);

                    _logger.LogInformation("Security zone email alert sent for pet {Name} to owner {UserName}", pet.Name, pet.Owner.UserName);
                }

                if (flameDetectionInfo?.Data is null)
                {
                    _logger.LogError("Latest flame detection reading is null. Pet ID: {Id}", pet.Id);
                    continue;
                }

                if (flameDetectionInfo.Data.FlameDetected)
                {
                    var message = $"<h1> Plamen je detektiran pored Vašeg ljubimca {pet.Name}! <h1/> <br>" +
                        "<h2 style='color:red;'> Molimo Vas provjerite sigurnost svog ljubimca. </h2>";

                    await _emailService.SendAsync(pet.Owner!.Email, "Pet Tracker Flame Alert", message, true);

                    _logger.LogInformation("Flame detection email alert sent for pet {Name} to owner {UserName}", pet.Name, pet.Owner.UserName);
                }
            }

            _logger.LogInformation($"{nameof(CheckSensorsAndSendAlert)} completed successfully.");
        }
    }
}
