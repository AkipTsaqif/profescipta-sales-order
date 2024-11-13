using Microsoft.AspNetCore.Mvc;
using Profescipta_Sales_Order.Models;

namespace Profescipta_Sales_Order.Controllers
{
    public class SalesController : Controller
    {
        private readonly SOContext _context;

        public SalesController(SOContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SalesDetails(string? orderNo)
        {
            if (!string.IsNullOrEmpty(orderNo))
            {
                try { 
                    var sale = GetSalesDetail(Int32.Parse(orderNo));
                    if (sale == null)
                    {
                        return NotFound();
                    }

                    return View(sale);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetSales()
        {
            try
            {
                var sales = _context.SoOrders.ToList();
                return Json(sales);
            }
            catch (Exception ex)
            {
                return Json(new { statusCode = 500, message = ex });
            }
        }

        [HttpGet]
        public Order GetSalesDetail(int id)
        {
            try
            {
                var salesHeader = _context.SoOrders.Where(o => o.SoOrderId == id).FirstOrDefault();
                if (salesHeader == null)
                {
                    return null;
                }

                var salesDetail = _context.SoItems.Where(o => o.SoOrderId == id).ToList();
                Order order = new()
                {
                    SalesHeader = salesHeader,
                    Items = salesDetail
                };

                return order;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult SaveSales([FromBody] Order order)
        {
            if (order == null || order.SalesHeader == null || order.Items == null || order.Items.Count == 0)
            {
                return BadRequest("No order data being submitted");
            }

            try
            {
                var existingOrder = _context.SoOrders
                    .Where(o => o.SoOrderId == order.SalesHeader.SoOrderId)
                    .FirstOrDefault();

                if (existingOrder != null)
                {
                    existingOrder.OrderNo = order.SalesHeader.OrderNo;
                    existingOrder.OrderDate = order.SalesHeader.OrderDate;
                    existingOrder.ComCustomerId = order.SalesHeader.ComCustomerId;
                    existingOrder.Address = order.SalesHeader.Address;

                    var existingItems = _context.SoItems
                                        .Where(i => i.SoOrderId == existingOrder.SoOrderId)
                                        .ToList();

                    foreach (var item in order.Items)
                    {
                        var existingItem = existingItems
                                            .FirstOrDefault(i => i.SoItemId == item.SoItemId);

                        if (existingItem != null)
                        {
                            existingItem.ItemName = item.ItemName;
                            existingItem.Quantity = item.Quantity;
                            existingItem.Price = item.Price;
                        }
                        else
                        {
                            item.SoOrderId = existingOrder.SoOrderId;
                            _context.SoItems.Add(item);
                        }
                    }

                    var incomingItemIds = order.Items.Select(i => i.SoItemId).ToHashSet();
                    var itemsToRemove = existingItems
                                        .Where(i => !incomingItemIds.Contains(i.SoItemId))
                                        .ToList();

                    _context.SoItems.RemoveRange(itemsToRemove);
                }
                else
                {
                    var soOrder = order.SalesHeader;
                    _context.SoOrders.Add(soOrder);
                    _context.SaveChanges();

                    long orderId = soOrder.SoOrderId;

                    foreach (var item in order.Items)
                    {
                        item.SoOrderId = orderId;
                        _context.SoItems.Add(item);
                    }
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok("Sales order successfully made");
        }

        [HttpDelete]
        public IActionResult DeleteSales(string? orderNo)
        {
            try
            {
                if (string.IsNullOrEmpty(orderNo))
                {
                    return BadRequest("No order number being submitted");
                }

                int id = Int32.Parse(orderNo);
                var salesHeader = _context.SoOrders.Where(o => o.SoOrderId == id).FirstOrDefault();
                if (salesHeader == null)
                {
                    return NotFound();
                }

                var salesDetail = _context.SoItems.Where(o => o.SoOrderId == id).ToList();
                _context.SoItems.RemoveRange(salesDetail);
                _context.SoOrders.Remove(salesHeader);
                _context.SaveChanges();

                return Ok("Sales order successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
