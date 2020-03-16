using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace JRPC_HMS.Pages
{
    public class KitchenModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public KitchenModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<OrderOrderItems> OrderOrderItems { get; set; }
        public IList<Order> Orders { get; set; }

        public void OnGet()
        {
            Orders = _context.Orders.Where(o => o.PreparationStatus == "Incomplete").ToList();
        }

        public PartialViewResult OnGetOrdersWithItems()
        {
            List<OrderOrderItems> ooi = new List<OrderOrderItems>();
            var orders = _context.Orders.Where(o => o.PreparationStatus == "Incomplete").ToList();

            foreach (var order in orders)
            {
                List<OOrderItem> oOrderItems = new List<OOrderItem>();
                var orderitems = _context.OrderItems.Where(oi => oi.OrderId == order.OrderId).ToList();
                foreach (var orderitem in orderitems)
                {
                    oOrderItems.Add(new OOrderItem
                    {
                        OrderId = orderitem.OrderId,
                        ProductName = _context.Products.FirstOrDefault(p => p.Product_ID == orderitem.Product_ID).ProductName,
                        Quantity = orderitem.Quantity
                    });
                }
                ooi.Add(new OrderOrderItems
                {
                    Order = order,
                    OOrderItems = oOrderItems
                });
            }
            OrderOrderItems = ooi;
            return new PartialViewResult {
                ViewName = "_Orders",
                ViewData = new ViewDataDictionary<List<OrderOrderItems>>(ViewData, OrderOrderItems)
            };
        }

        public ActionResult OnPost(int id)
        {
            Orders = _context.Orders.Where(oo => oo.PreparationStatus == "Incomplete").ToList();
            Order o = _context.Orders.FirstOrDefault(or => or.Id == id);
            o.PreparationStatus = "Complete";
            _context.Orders.Update(o);
            _context.SaveChanges();
            return Page();
        }
    }
}