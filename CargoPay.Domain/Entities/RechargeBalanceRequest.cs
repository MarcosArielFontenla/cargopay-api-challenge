namespace CargoPay.Domain.Entities
{
    public class RechargeBalanceRequest
    {
        public string CardNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
