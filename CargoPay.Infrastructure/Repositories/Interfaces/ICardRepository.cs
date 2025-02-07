using CargoPay.Domain.Entities;

namespace CargoPay.Infrastructure.Repositories.Interfaces
{
    public interface ICardRepository
    {
        Task<Card> CreateCard(string cardNumber, decimal balance);
        Task<decimal> GetCardBalanceByCardNumber(string cardNumber);
        Task<decimal> GetCardBalanceByCardId(int id);
        Task<List<Card>> GetAllCards();
        Task UpdateCardBalance(string cardNumber, decimal newBalance);
    }
}
