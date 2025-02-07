using CargoPay.Domain.Entities;

namespace CargoPay.Infrastructure.Repositories.Interfaces
{
    public interface ICardRepository
    {
        Task<Card> CreateCardAsync(string cardNumber, decimal balance);
        Task<Card> GetCardByNumberAsync(string cardNumber);
        Task UpdateCardBalanceAsync(string cardNumber, decimal newBalance);
    }
}
