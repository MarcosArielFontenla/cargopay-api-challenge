namespace CargoPay.Application.DTOs
{
    public class PayRequest
    {
        public string CardNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
