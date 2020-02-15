using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe.POC.MVC.Models;
using System;
using System.Collections.Generic;

namespace Stripe.POC.MVC.Controllers
{
  [Route("[controller]/[action]")]
  public class CustomersController : Controller
  {

    private const string API_KEY = "sk_test_...";

    public CustomersController()
    {
      StripeConfiguration.ApiKey = API_KEY;
    }

    public IActionResult Index()
    {
      return View("Create");
    }

    public IActionResult Get()
    {
      var options = new CustomerListOptions();
      var service = new CustomerService();
      var customers = service.List(options);

      return View("List", customers.Data);
    }

    [HttpPost]
    public IActionResult Charge(string customerId)
    {
      var options = new ChargeCreateOptions
      {
        Amount = 17 * 100,// 17 UAH
        Currency = "uah",
        Customer = customerId,
        
      };
      var service = new ChargeService();
      var resultCharge = service.Create(options);

      var res = new PaymentResultViewModel
      {
        Amount = resultCharge.Amount,
        PaymentId = resultCharge.Id,
        Detaills = JsonConvert.SerializeObject(resultCharge, Formatting.Indented)
      };

      return View("../Payment/PaymentStatus", res);
    }


    [HttpPost]
    public IActionResult Create(CustomerViewModel model)
    {
      try
      {
        var options = new CustomerCreateOptions
        {
          Description = model.FullName,
          Email = model.Email,
          Source = model.CreditCardToken,
          Metadata = new Dictionary<string, string>
          {
            { "Phone Number", model.Phone }
          }
          
        };
        var service = new CustomerService();
        var customer = service.Create(options);

        var res = new CustomerDetailsModel
        {
          Id = customer.Id,
          Details = JsonConvert.SerializeObject(customer, Formatting.Indented)
        };

        return View("CustomerStatus", res);

      }
      catch (Exception ex)
      {
        ViewData["ErrorMessage"] = ex.Message;
        return View("Create");
      }
    }
  }
}