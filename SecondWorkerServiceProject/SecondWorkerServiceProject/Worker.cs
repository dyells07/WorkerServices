using SecondWorkerServiceProject.Services.v1;

namespace UserEqualizerWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly UserEqualizerService _userService;

        public Worker(ILogger<Worker> logger, UserEqualizerService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting service...");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var result = await _userService.ExecuteService();

                string resultLogMessage = result ? "Successfully processed" : "Processed with failure";

                _logger.LogInformation(resultLogMessage);

                _logger.LogInformation("Stoping service...");

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}