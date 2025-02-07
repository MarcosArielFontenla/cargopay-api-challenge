using CargoPay.Application.Services.Interfaces;
using CargoPay.Domain.Entities;
using CargoPay.Infrastructure.Repositories.Interfaces;

namespace CargoPay.Application.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly IPaymentFeeService _paymentFeeService;

        public CardService(ICardRepository cardRepository, IPaymentFeeService paymentFeeService)
        {
            _cardRepository = cardRepository;
            _paymentFeeService = paymentFeeService;
        }

        public async Task<Card> CreateCardAsync(string cardNumber, decimal initialBalance)
        {
            if (cardNumber.Length != 15 || !long.TryParse(cardNumber, out _))
                throw new ArgumentException("Invalid card number format.");

            return await _cardRepository.CreateCardAsync(cardNumber, initialBalance);
        }

        public async Task<decimal> GetCardBalanceAsync(string cardNumber)
        {
            var card = await _cardRepository.GetCardByNumberAsync(cardNumber);

            if (card == null)
                throw new KeyNotFoundException("Card not found.");

            return card.Balance;
        }

        public async Task PayAsync(string cardNumber, decimal amount)
        {
            var feeRate = await _paymentFeeService.GetCurrentFeeRateAsync();
            var totalPayment = amount + (amount * feeRate);
            var card = await _cardRepository.GetCardByNumberAsync(cardNumber);
            
            if (card == null)
                throw new KeyNotFoundException("Card not found.");

            if (card.Balance < totalPayment)
                throw new InvalidOperationException("Insufficient balance.");

            card.Balance -= totalPayment;
            await _cardRepository.UpdateCardBalanceAsync(cardNumber, card.Balance);
        }
    }
}
