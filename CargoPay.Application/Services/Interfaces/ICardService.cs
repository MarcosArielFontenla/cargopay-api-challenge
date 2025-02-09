using CargoPay.Domain.Entities;

namespace CargoPay.Application.Services.Interfaces
{
    public interface ICardService
    {
        Task<Card> CreateCardAsync(CreateCardRequest request);
        Task PayAsync(PaymentRequest request);
        Task<decimal> GetCardBalanceByCardNumberAsync(string cardNumber);
        Task<decimal> GetCardBalanceByCardIdAsync(int id);
        Task<List<Card>> GetAllCardsAsync();
        Task<bool> RechargeBalanceAsync(RechargeBalanceRequest request);
        Task<bool> DeleteCardAsync(int id);
    }
}
