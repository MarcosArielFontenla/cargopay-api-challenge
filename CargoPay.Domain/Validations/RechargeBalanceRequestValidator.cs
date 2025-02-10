using CargoPay.Domain.Entities;
using FluentValidation;

namespace CargoPay.Domain.Validations
{
    public class RechargeBalanceRequestValidator : AbstractValidator<RechargeBalanceRequest>
    {
        public RechargeBalanceRequestValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty()
                                      .WithMessage("Card number is required.")
                                      .Matches("^[0-9]{15}$")
                                      .WithMessage("Card number must be 15 digits.");

            RuleFor(x => x.Amount).GreaterThan(0)
                                  .WithMessage("Amount must be greater than 0.");
        }
    }
}
