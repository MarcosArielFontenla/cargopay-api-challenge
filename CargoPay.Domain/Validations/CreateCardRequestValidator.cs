using CargoPay.Domain.Entities;
using FluentValidation;

namespace CargoPay.Domain.Validations
{
    public class CreateCardRequestValidator : AbstractValidator<CreateCardRequest>
    {
        public CreateCardRequestValidator()
        {
            RuleFor(x => x.CardNumber).NotEmpty()
                                      .WithMessage("Card number is required.")
                                      .Matches("^[0-9]{15}$")
                                      .WithMessage("Card number must be 15 digits.")
                                      .Matches(@"^\d+$")
                                      .WithMessage("Card number must contain only numbers!");

            RuleFor(x => x.InitialBalance).GreaterThan(0)
                                  .WithMessage("InitialBalance must be greater than 0.");
        }
    }
}
