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

        public async Task<Card> CreateCardAsync(CreateCardRequest request)
        {
            return await _cardRepository.CreateCard(request.CardNumber, request.InitialBalance);
        }

        public async Task<decimal> GetCardBalanceByCardNumberAsync(string cardNumber)
        {
            return await _cardRepository.GetCardBalanceByCardNumber(cardNumber);
        }

        public async Task<decimal> GetCardBalanceByCardIdAsync(int id)
        {
            return await _cardRepository.GetCardBalanceByCardId(id);
        }

        public async Task<List<Card>> GetAllCardsAsync()
        {
            return await _cardRepository.GetAllCards();
        }

        public async Task PayAsync(PaymentRequest request)
        {
            var feeRate = await _paymentFeeService.GetCurrentFeeRateAsync();
            var totalPayment = request.Amount + (request.Amount * feeRate);
            var cardBalance = await _cardRepository.GetCardBalanceByCardNumber(request.CardNumber);

            if (cardBalance < totalPayment)
                throw new InvalidOperationException("Insufficient balance.");

            cardBalance -= totalPayment;
            await _cardRepository.UpdateCardBalance(request.CardNumber, cardBalance);
        }
    }
}
