using CargoPay.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CargoPay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        /// <summary>
        /// Creation of a new card assigning an initial balance.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateCard([FromBody] string cardNumber, decimal initialBalance)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 15 || !long.TryParse(cardNumber, out _))
                return BadRequest("the cardNumber must have 15 digits.");

            if (initialBalance < 0)
                throw new ArgumentOutOfRangeException("The initialBalance cant be negative!");

            var card = await _cardService.CreateCardAsync(cardNumber, initialBalance);

            return Ok(new 
            { 
                Message = "¡Card created successfully!.",
                Card = card
            });
        }

        /// <summary>
        /// Realize A payment by applying a fee to the amount entered.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPost("{cardNumber}/pay")]
        public async Task<IActionResult> Pay(string cardNumber, [FromBody] decimal amount)
        {
            try
            {
                await _cardService.PayAsync(cardNumber, amount);
                return Ok(new
                {
                    Message = "Payment was successful!"
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("The card doesnt exist.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Returns the card balance.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        [HttpGet("{cardNumber}/balance")]
        public async Task<IActionResult> GetCardBalance(string cardNumber)
        {
            try
            {
                var balance = await _cardService.GetCardBalanceAsync(cardNumber);

                return Ok(new
                {
                    Balance = balance
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("The card doesnt exist.");
            }
        }
    }
}
