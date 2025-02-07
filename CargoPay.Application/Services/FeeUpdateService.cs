using Microsoft.Extensions.Hosting;

namespace CargoPay.Application.Services
{
    public class FeeUpdateService : BackgroundService
    {
        private static readonly Random _random = new();
        private decimal _currentFeeRate = 1.0m;

        public decimal GetCurrentFeeRate() => _currentFeeRate;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() => UpdateFeeRate(), stoppingToken);
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private void UpdateFeeRate()
        {
            var randomDecimal = _random.NextDouble() * 2;
            _currentFeeRate *= (decimal)randomDecimal;
        }
    }
}
