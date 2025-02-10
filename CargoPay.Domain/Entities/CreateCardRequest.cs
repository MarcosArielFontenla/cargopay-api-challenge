using System.ComponentModel.DataAnnotations;

namespace CargoPay.Domain.Entities
{
    public class CreateCardRequest
    {
        public string CardNumber { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
    }
}
