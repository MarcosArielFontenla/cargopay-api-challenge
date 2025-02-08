using CargoPay.Application.Services.Interfaces;
using CargoPay.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CargoPay.Tests.Controllers
{
    public class PaymentFeesControllerTests
    {
        private readonly Mock<IPaymentFeeService> _paymentFeeServiceMock;
        private readonly PaymentFeesController _paymentFeesController;

        public PaymentFeesControllerTests()
        {
            _paymentFeeServiceMock = new Mock<IPaymentFeeService>();
            _paymentFeesController = new PaymentFeesController(_paymentFeeServiceMock.Object);
        }

        [Fact]
        public async Task GetCurrentFeeRate_Should_ReturnsFeeRate()
        {
            // Arrange
            decimal feeRate = 0.05m;

            _paymentFeeServiceMock.Setup(s => s.GetCurrentFeeRateAsync())
                                  .ReturnsAsync(feeRate);

            // Act
            var result = await _paymentFeesController.GetCurrentFeeRate();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("FeeRate", okResult.Value.ToString());
        }
    }
}
