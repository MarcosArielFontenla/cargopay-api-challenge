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

        public async Task<decimal> GetCardBalanceByCardNumber(string cardNumber)
        {
            try
            {
                var cardBalance = await _context.Cards.Where(c => c.CardNumber == cardNumber)
                                                      .AsNoTracking()
                                                      .Select(c => c.Balance)
                                                      .SingleOrDefaultAsync();
                return cardBalance;

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Card with card number: {cardNumber} not found!", ex);
            }
        }

        public async Task<decimal> GetCardBalanceByCardId(int id)
        {
            try
            {
                var cardBalance = await _context.Cards.Where(c => c.Id == id)
                                                      .AsNoTracking()
                                                      .Select(c => c.Balance)
                                                      .SingleOrDefaultAsync();

                return cardBalance;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Card with id: {id} not found!", ex);
            }
        }

        public async Task<List<Card>> GetAllCards()
        {
            try
            {
                var cards = await _context.Cards.AsNoTracking().ToListAsync();
                return cards;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Cards not found", ex);
            }
        }

        public async Task<Card> CreateCard(string cardNumber, decimal initialBalance)
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

        public async Task UpdateCardBalance(string cardNumber, decimal newBalance)
        {
            await _context.Cards.Where(c => c.CardNumber == cardNumber)
                                .ExecuteUpdateAsync(s => s.SetProperty(x => x.Balance, newBalance));
        }

        public async Task<bool> RechargeBalance(RechargeBalanceRequest request)
        {
            try
            {
                var card = await _context.Cards.Where(c => c.CardNumber == request.CardNumber)
                                               .SingleOrDefaultAsync();

                if (card is null)
                    throw new ArgumentException($"Card with card number: {request.CardNumber} not found!");

                card.Balance += request.Amount;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error ocurred while recharging card balance!", ex);
            }
        }

        public async Task<bool> DeleteCard(int id)
        {
            try
            {
                var card = await _context.Cards.Where(c => c.Id == id)
                                               .SingleOrDefaultAsync();

                if (card is null)
                    throw new ArgumentException($"Card with id: {id} not found!");

                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error ocurred while deleting card!", ex);
            }
        }
    }
}
