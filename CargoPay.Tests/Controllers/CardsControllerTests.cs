using CargoPay.Application.Services.Interfaces;
using CargoPay.Domain.Entities;
using CargoPay.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CargoPay.Tests.Controllers
{
    public class CardsControllerTests
    {
        private readonly Mock<ICardService> _mockCardService;
        private readonly CardsController _cardsController;

        public CardsControllerTests()
        {
            _mockCardService = new Mock<ICardService>();
            _cardsController = new CardsController(_mockCardService.Object);
        }

        [Fact]
        public async Task CreateCard_ShouldReturnOkResult_WhenCardIsCreatedSuccessfully()
        {
            // Arrange
            var request = new CreateCardRequest 
            { 
                CardNumber = "123456789012345", 
                InitialBalance = 100 
            };

            var card = new Card 
            { 
                Id = 1, 
                CardNumber = request.CardNumber, 
                Balance = request.InitialBalance 
            };

            _mockCardService.Setup(s => s.CreateCardAsync(request))
                            .ReturnsAsync(card);

            // Act
            var result = await _cardsController.CreateCard(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Pay_Should_ReturnPaymentSuccessfull()
        {
            // Arrange
            var request = new PaymentRequest 
            { 
                CardNumber = "123456789012345", 
                Amount = 50 
            };

            _mockCardService.Setup(s => s.PayAsync(request)).Returns(Task.CompletedTask);

            // Act
            var result = await _cardsController.Pay(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetCardBalanceByCardNumber_Should_ReturnsBalance()
        {
            // Arrange
            string cardNumber = "123456789012345";
            decimal balance = 200;

            _mockCardService.Setup(s => s.GetCardBalanceByCardNumberAsync(cardNumber))
                            .ReturnsAsync(balance);

            // Act
            var result = await _cardsController.GetCardBalanceByCardNumber(cardNumber);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Balance", okResult.Value.ToString());
        }

        [Fact]
        public async Task GetCardBalanceByCardId_Should_ReturnsBalance()
        {
            // Arrange
            int cardId = 1;
            string cardNumber = "123456789012345";
            decimal balance = 200;

            _mockCardService.Setup(s => s.GetCardBalanceByCardIdAsync(cardId))
                            .ReturnsAsync(balance);

            // Act
            var result = await _cardsController.GetCardBalanceByCardId(cardId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("Balance", okResult.Value.ToString());
        }

        [Fact]
        public async Task GetCardBalanceByCardId_InvalidId_ReturnsZeroBalance()
        {
            // Arrange
            int invalidCardId = 999;

            _mockCardService.Setup(s => s.GetCardBalanceByCardIdAsync(invalidCardId))
                      .ReturnsAsync(0m);

            // Act
            var result = await _cardsController.GetCardBalanceByCardId(invalidCardId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);

            var balanceProperty = okResult.Value.GetType().GetProperty("Balance");
            Assert.NotNull(balanceProperty);

            var balanceValue = (decimal)balanceProperty.GetValue(okResult.Value);
            Assert.Equal(0m, balanceValue);
        }

        [Fact]
        public async Task GetAllCards_Should_ReturnsCardList()
        {
            // Arrange
            var mockCards = new List<Card>
            {
                new Card { Id = 1, CardNumber = "111122223333444", Balance = 5000 }
            };

            _mockCardService.Setup(s => s.GetAllCardsAsync())
                      .ReturnsAsync(mockCards);

            // Act
            var result = await _cardsController.GetAllCards();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<Card>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var cards = Assert.IsAssignableFrom<List<Card>>(okResult.Value);

            Assert.Single(cards);
            Assert.Equal("111122223333444", cards[0].CardNumber);
        }
    }
}
