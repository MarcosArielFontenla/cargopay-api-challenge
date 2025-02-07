using CargoPay.Domain.Entities;
using CargoPay.Infrastructure.Data;
using CargoPay.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CargoPay.Infrastructure.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _context;

        public CardRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<decimal> GetCardByNumberAsync(string cardNumber)
        {
            try
            {
                var cardBalance = await _context.Cards.Where(c => c.CardNumber == cardNumber)
                                               .Select(c => c.Balance)
                                               .FirstOrDefaultAsync();
                return cardBalance;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Card with card number: {cardNumber} not found!", ex);
            }
        }

        public async Task<Card> CreateCardAsync(string cardNumber, decimal initialBalance)
        {
            try
            {
                var card = new Card 
                { 
                    CardNumber = cardNumber, 
                    Balance = initialBalance,
                    CreatedAt = DateTime.UtcNow,
                };
                _context.Cards.Add(card);
                await _context.SaveChangesAsync();
                return card;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error ocurred while creating card!", ex);
            }
        }

        public async Task UpdateCardBalanceAsync(string cardNumber, decimal newBalance)
        {
            try
            {
                var card = await _context.Cards.FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
                
                if (card is not null)
                {
                    card.Balance = newBalance;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error to update the card balance!", ex);
            }
        }
    }
}
