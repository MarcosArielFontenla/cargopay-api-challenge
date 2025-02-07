using CargoPay.Application.DTOs;
using CargoPay.Domain.Entities;

namespace CargoPay.Application.Services.Interfaces
{
    public interface ICardService
    {
        Task<Card> CreateCardAsync(string cardNumber, decimal balance);
        Task PayAsync(string cardNumber, decimal amount);
        Task<decimal> GetCardBalanceAsync(string cardNumber);
    }
}
