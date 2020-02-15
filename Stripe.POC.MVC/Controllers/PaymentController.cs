using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe.POC.MVC.Models;
using System;

namespace Stripe.POC.MVC.Controllers
{
  [Route("[controller]/[action]")]
  public class PaymentController : Controller
  {

    private const string API_KEY = "sk_test...";


    public PaymentController()
    {
      StripeConfiguration.ApiKey = API_KEY;
    }

    public IActionResult Index()
    {
      return View("Payment");
    }

    [HttpPost]
    public IActionResult Create(string creditCardToken)
    {
      try
      {
        var options = new ChargeCreateOptions
        {
          Amount = 15 * 100,// 15 UAH
          Currency = "uah",
          Source = creditCardToken,
          Description = "My First Test Charge (created for API docs)",
        };
        var service = new ChargeService();

        Charge resultCharge = service.Create(options);

        var res = new PaymentResultViewModel
        {
          Amount = resultCharge.Amount,
          PaymentId = resultCharge.Id,
          Detaills = JsonConvert.SerializeObject(resultCharge, Formatting.Indented)
        };

        return View("PaymentStatus", res);

      } catch (Exception ex)
      {
        ViewData["ErrorMessage"] = ex.Message;
        return View("Payment");
      }

    }

    [HttpGet]
    public IActionResult Get()
    {
      var options = new ChargeListOptions();
      var service = new ChargeService();
      var charges = service.List(options);

      return View("ChargesHistory", charges.Data);
    }

  }
}