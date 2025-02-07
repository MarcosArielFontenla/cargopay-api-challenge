using CargoPay.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoPay.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        public async Task<ActionResult> GetCurrentFeeRate()
        {
            var feeRate = await _paymentFeeService.GetCurrentFeeRateAsync();
            return Ok(new { FeeRate = feeRate });
        }
    }
}
