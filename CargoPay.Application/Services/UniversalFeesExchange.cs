using CargoPay.Application.Services.Interfaces;

namespace CargoPay.Application.Services
{
    public class UniversalFeesExchange : IPaymentFeeService
    {
        private readonly FeeUpdateService _feeUpdateService;
        public static UniversalFeesExchange Instance { get; private set; }

        public UniversalFeesExchange(FeeUpdateService feeUpdateService)
        {
            _feeUpdateService = feeUpdateService;
        }

        public Task<decimal> GetCurrentFeeRateAsync()
        {
            return Task.FromResult(_feeUpdateService.GetCurrentFeeRate());
        }

        public static void Initialize(FeeUpdateService feesUpdateService) 
        {
            Instance = new UniversalFeesExchange(feesUpdateService);
        }
    }
}
