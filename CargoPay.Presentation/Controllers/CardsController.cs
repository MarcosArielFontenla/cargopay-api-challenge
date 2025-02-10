using CargoPay.Application.Services.Interfaces;
using CargoPay.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoPay.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IValidator<RechargeBalanceRequest> _rechargeBalanceRequestValidator;
        private readonly IValidator<PaymentRequest> _paymentRequestValidator;
        private readonly IValidator<CreateCardRequest> _createCardRequestValidator;

        public CardsController(
            ICardService cardService, 
            IValidator<RechargeBalanceRequest> rechargeBalanceRequestValidator, 
            IValidator<PaymentRequest> paymentRequestValidator,
            IValidator<CreateCardRequest> createCardRequestValidator)
        {
            _cardService = cardService;
            _rechargeBalanceRequestValidator = rechargeBalanceRequestValidator;
            _paymentRequestValidator = paymentRequestValidator;
            _createCardRequestValidator = createCardRequestValidator;
        }

        /// <summary>
        /// Creation of a new card assigning an initial balance.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<ActionResult> CreateCard(CreateCardRequest request)
        {
            var validationResult = await _createCardRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Validation error!",
                    Errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }
            var createdCard = await _cardService.CreateCardAsync(request);

            return Ok(new 
            { 
                Message = "¡Card created successfully!",
                Card = createdCard
            });
        }

        /// <summary>
        /// Realize A payment by applying a fee to the amount entered.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("pay")]
        public async Task<ActionResult> Pay(PaymentRequest request)
        {
            var validationResult = await _paymentRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Validation error!",
                    Errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            await _cardService.PayAsync(request);

            return Ok(new
            {
                Message = "Payment was successful!"
            });
        }

        /// <summary>
        /// Returns the card balance.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        [HttpGet("by-number/{cardNumber}/balance")]
        public async Task<ActionResult> GetCardBalanceByCardNumber(string cardNumber)
        {
            var balance = await _cardService.GetCardBalanceByCardNumberAsync(cardNumber);

            return Ok(new
            {
                Balance = balance
            });
        }

        /// <summary>
        /// Returns card balance by card id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("by-id/{id}/balance")]
        public async Task<ActionResult> GetCardBalanceByCardId(int id)
        {
            var balance = await _cardService.GetCardBalanceByCardIdAsync(id);

            return Ok(new
            {
                Balance = balance
            });
        }

        /// <summary>
        /// Retrieves all cards.
        /// </summary>
        /// <returns></returns>
        [HttpGet("cards")]
        public async Task<ActionResult<List<Card>>> GetAllCards()
        {
            var cards = await _cardService.GetAllCardsAsync().ConfigureAwait(false);
            return Ok(cards);
        }

        /// <summary>
        /// Recharge the balance of a card.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("recharge-balance")]
        public async Task<ActionResult> RechargeBalance(RechargeBalanceRequest request)
        {
            var validationResult = await _rechargeBalanceRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Validation error!",
                    Errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            await _cardService.RechargeBalanceAsync(request);

            return Ok(new
            {
                Message = "Balance recharged successfully!"
            });
        }

        /// <summary>
        /// Delete a card.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCard(int id)
        {
            await _cardService.DeleteCardAsync(id);

            return Ok(new
            {
                Message = "Card deleted successfully!"
            });
        }
    }
}
