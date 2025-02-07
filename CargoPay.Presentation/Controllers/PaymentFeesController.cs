using CargoPay.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CargoPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentFeesController : ControllerBase
    {
        private readonly IPaymentFeeService _paymentFeeService;

        public PaymentFeesController(IPaymentFeeService paymentFeeService)
        {
            _paymentFeeService = paymentFeeService;
        }

        /// <summary>
        /// Get a current fee rate.
        /// </summary>
        /// <returns></returns>
        [HttpGet("current-fee")]
        public async Task<IActionResult> GetCurrentFeeRate()
        {
            var feeRate = await _paymentFeeService.GetCurrentFeeRateAsync();
            return Ok(new
            {
                FeeRate = feeRate
            });
        }
    }
}
