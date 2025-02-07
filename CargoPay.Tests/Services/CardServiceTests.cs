using CargoPay.Application.Services;
using CargoPay.Application.Services.Interfaces;
using CargoPay.Domain.Entities;
using CargoPay.Infrastructure.Repositories.Interfaces;
using Moq;

namespace CargoPay.Tests.Services
{
    public class CardServiceTests
    {
        private readonly Mock<ICardRepository> _mockCardRepository;
        private readonly Mock<IPaymentFeeService> _mockPaymentFeeService;
        private readonly CardService _cardService;

        public CardServiceTests()
        {
            _mockCardRepository = new Mock<ICardRepository>();
            _mockPaymentFeeService = new Mock<IPaymentFeeService>();
            _cardService = new CardService(_mockCardRepository.Object, _mockPaymentFeeService.Object);
        }

        [Fact]
        public async Task CreateCardAsync_Should_CreateCard_WhenValidDataIsProvided()
        {
            // Arrange
            var request = new CreateCardRequest
            {
                CardNumber = "123456789012345",
                InitialBalance = 4000,
            };

            var expectedCard = new Card
            {
                CardNumber = request.CardNumber,
                Balance = request.InitialBalance,
                CreatedAt = DateTime.UtcNow
            };

            _mockCardRepository.Setup(repo => repo.CreateCard(request.CardNumber, request.InitialBalance))
                               .ReturnsAsync(expectedCard);

            // Act
            var result = await _cardService.CreateCardAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCard, result);
        }

        [Fact]
        public async Task PayAsync_SufficientBalance_UpdateBalance()
        {
            // Arrange
            var request = new PaymentRequest
            {
                CardNumber = "123123123123123",
                Amount = 100,
            };

            _mockPaymentFeeService.Setup(f => f.GetCurrentFeeRateAsync())
                                  .ReturnsAsync(0.1m);

            _mockCardRepository.Setup(r => r.GetCardBalanceByCardNumber(request.CardNumber))
                               .ReturnsAsync(150m);

            // Act
            await _cardService.PayAsync(request);

            // Assert
            _mockCardRepository.Verify(r => r.UpdateCardBalance(request.CardNumber, 150m - (100m * 1.1m)), Times.Once);
        }

        [Fact]
        public async Task PayAsync_InsufficientBalance_ThrowsException()
        {
            // Arrange
            var request = new PaymentRequest
            {
                CardNumber = "123456789012345",
                Amount = 100
            };

            _mockPaymentFeeService.Setup(f => f.GetCurrentFeeRateAsync())
                         .ReturnsAsync(0.1m);

            _mockCardRepository.Setup(r => r.GetCardBalanceByCardNumber(request.CardNumber))
                    .ReturnsAsync(50m);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _cardService.PayAsync(request)
            );
        }

        [Fact]
        public async Task GetBalance_ExistingCard_ReturnsBalance()
        {
            // Arrange
            var cardNumber = "123456789012345";
            var expectedBalance = 500m;

            _mockCardRepository.Setup(r => r.GetCardBalanceByCardNumber(cardNumber))
                    .ReturnsAsync(expectedBalance);

            // Act
            var result = await _cardService.GetCardBalanceByCardNumberAsync(cardNumber);

            // Assert
            Assert.Equal(expectedBalance, result);
        }

        [Fact]
        public async Task GetCardBalanceByCardIdAsync_NonExistentId_ReturnsZero()
        {
            // Arrange
            int invalidCardId = 999;

            _mockCardRepository.Setup(r => r.GetCardBalanceByCardId(invalidCardId))
                               .ReturnsAsync(0m);

            // Act
            var result = await _cardService.GetCardBalanceByCardIdAsync(invalidCardId);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public async Task GetAllCardsAsync_Should_ReturnsFullList()
        {
            // Arrange
            var mockCards = new List<Card>
            {
                new Card { Id = 1, CardNumber = "123456789012345", Balance = 1000 },
                new Card { Id = 2, CardNumber = "987654321098765", Balance = 2000 }
            };

            _mockCardRepository.Setup(r => r.GetAllCards())
                    .ReturnsAsync(mockCards);

            // Act
            var result = await _cardService.GetAllCardsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.CardNumber == "123456789012345");
        }
    }
}
