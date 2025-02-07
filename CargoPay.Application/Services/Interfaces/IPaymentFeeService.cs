namespace CargoPay.Application.Services.Interfaces
{
    public interface IPaymentFeeService
    {
        Task<decimal> GetCurrentFeeRateAsync();
    }
}
