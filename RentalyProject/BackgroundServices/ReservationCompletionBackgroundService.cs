using RentalyProject.Repositories.Interfaces;
using RentalyProject.Services;

namespace RentalyProject.BackgroundServices
{
    public class ReservationCompletionBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ReservationCompletionBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var reservationService = scope.ServiceProvider.GetRequiredService<ReservationService>();
                    await reservationService.CompleteExpiredReservations();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
