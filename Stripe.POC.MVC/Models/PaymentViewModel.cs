namespace Stripe.POC.MVC.Models
{
  public class PaymentViewModel
  {
    public decimal Amount { get; set; }

    public string CreditCardToken { get; set; }
  }
}
