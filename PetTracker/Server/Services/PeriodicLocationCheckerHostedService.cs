using PetTracker.Shared.Constants;

namespace PetTracker.Server.Services
{
    public class PeriodicLocationCheckerHostedService : BackgroundService
    {
        private readonly TimeSpan _timerPeriod = TimeSpan.FromSeconds(SharedConstants.SecondsRefreshTime);
        private readonly ILogger<PeriodicLocationCheckerHostedService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PeriodicLocationCheckerHostedService(ILogger<PeriodicLocationCheckerHostedService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(_timerPeriod);

            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await using AsyncServiceScope asyncScope = _serviceScopeFactory.CreateAsyncScope();
                    var sampleService = asyncScope.ServiceProvider.GetRequiredService<GetAndCheckPetsLocationService>();

                    await sampleService.CheckSensorsAndSendAlert();

                    _logger.LogInformation($"Executed {nameof(PeriodicLocationCheckerHostedService)}.");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Failed to execute {PeriodicLocationCheckerHostedService} with exception message {Message}.",
                        nameof(PeriodicLocationCheckerHostedService), ex.Message);
                }
            }
        }
    }
}
