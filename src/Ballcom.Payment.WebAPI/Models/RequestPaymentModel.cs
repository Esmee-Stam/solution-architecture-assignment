namespace Ballcom.Payment.WebAPI.Models;

public class RequestPaymentModel
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";

    public string PaymentMethod { get; set; } = "ForwardPay";
}