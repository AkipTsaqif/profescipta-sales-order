using Microsoft.AspNetCore.Mvc;

namespace Profescipta_Sales_Order.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SalesDetails()
        {
            return View();
        }
    }
}
