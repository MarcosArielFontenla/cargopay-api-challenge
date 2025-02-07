using System.ComponentModel.DataAnnotations;

namespace CargoPay.Domain.Entities
{
    public class CreateCardRequest
    {
        [Required]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Card number must be 15 digits")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Card number must contain only numbers!")]
        public string CardNumber { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Initial balance cannot be negative")]
        public decimal InitialBalance { get; set; }
    }
}
