using Microsoft.AspNetCore.Mvc;
using Profescipta_Sales_Order.Models;

namespace Profescipta_Sales_Order.Controllers
{
    public class CustomerController : Controller
    {
        private readonly SOContext _context;

        public CustomerController (SOContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetCustomers()
        {
            try
            {
                var customers = _context.ComCustomers.ToList();
                return Json(customers);
            }
            catch (Exception ex)
            {
                return Json(new { statusCode = 500, message = ex });
            }
        }

        [HttpGet]
        public JsonResult GetCustomerById(int id)
        {
            try
            {
                var customer = _context.ComCustomers.Where(c => c.ComCustomerId == id).FirstOrDefault();
                return Json(customer);
            }
            catch (Exception ex)
            {
                return Json(new { statusCode = 500, message = ex });
            }
        }
    }
}
