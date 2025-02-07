using System.ComponentModel.DataAnnotations;

namespace CargoPay.Domain.Entities
{
    public class PaymentRequest
    {
        [Required]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Card number must be 15 digits")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Card number must contain only numbers")]
        public string CardNumber { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
    }
}
