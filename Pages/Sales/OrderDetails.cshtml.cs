using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace JRPC_HMS.Pages.Sales
{
    [Authorize(Roles = "Admin, User")]
    public class OrderDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }
        public IList<OrderItems> OrderItemss { get; set; }
        public IList<Product> Products { get; set; }
        public Store Store { get; set; }
        public Customer Customer { get; set; }

        public IActionResult OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Products = _context.Products.ToList();
            Order = _context.Orders.FirstOrDefault(m => m.Id == id);
            OrderItemss = _context.OrderItems.Where(oi => oi.OrderId == Order.OrderId).ToList();
            Store = _context.Stores.FirstOrDefault(s => s.Store_ID == Order.StoreId);
            Customer = _context.Customers.FirstOrDefault(c => c.Id == Order.CustomerId);
            if (Order == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}