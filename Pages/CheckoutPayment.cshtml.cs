using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JRPC_HMS.Pages
{
    public class CheckoutPaymentModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Order currentOrder;
        public List<OrderItems> items;

        public CheckoutPaymentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Properties
        public IList<Product> Products { get; set; }
        public IList<Product> AllProducts { get; set; }
        public IList<Category> Categories { get; set; }
        public Order Order { get; set; }
        public IList<OrderItems> OrderItemss { get; set; }
        [BindProperty]
        public string PaymentMethod { get; set; }
        public string[] PaymentMethods = new[] { "Cash", "Card" };
        public IList<StockToProduct> StockToProducts { get; set; }
        public IList<StockItem> StockItems { get; set; }
        #endregion

        #region OnGet
        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = _context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (Order == null)
            {
                return NotFound();
            }
            else
            {
                OrderItemss = _context.OrderItems.Where(oi => oi.OrderId == id).ToList();
            }
            AllProducts = _context.Products.ToList();
            PaymentMethod = Order.PaymentMethod;
            return Page();
        }
        #endregion

        #region OnGetAddPaidChange
        public IActionResult OnGetAddPaidChange(int pay, int id)
        {
            decimal paid = 0, change = 0;
            Order currentOrder = _context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (PaymentMethod == "Card")
            {
                paid = currentOrder.GrandTotal;
                change = 0;
            }
            else
            {
                if (pay == 1)
                {
                    paid = currentOrder.Paid + 1;
                }
                else if (pay == 2)
                {
                    paid = currentOrder.Paid + 2;
                }
                else if (pay == 3)
                {
                    paid = currentOrder.Paid + 3;
                }
                else if (pay == 4)
                {
                    paid = currentOrder.Paid + 4;
                }
                else if (pay == 5)
                {
                    paid = currentOrder.Paid + 5;
                }
                else if (pay == 6)
                {
                    paid = currentOrder.Paid + 6;
                }
                else if (pay == 7)
                {
                    paid = currentOrder.Paid + 7;
                }
                else if (pay == 8)
                {
                    paid = currentOrder.Paid + 8;
                }
                else if (pay == 9)
                {
                    paid = currentOrder.Paid + 9;
                }
                else if (pay == 10)
                {
                    paid = currentOrder.Paid + 10;
                }
                else if (pay == 20)
                {
                    paid = currentOrder.Paid + 20;
                }
                else if (pay == 50)
                {
                    paid = currentOrder.Paid + 50;
                }
                else if (pay == 100)
                {
                    paid = currentOrder.Paid + 100;
                }
                else if (pay == 200)
                {
                    paid = currentOrder.Paid + 200;
                }
            }

            if (paid > currentOrder.GrandTotal)
            {
                change = paid - currentOrder.GrandTotal;
            }
            else
            {
                change = 0;
            }
            Order order = new Order();
            List<OrderItems> orderOrderItems = new List<OrderItems>();

            DbContextOptions<ApplicationDbContext> options = new DbContextOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                Order payingOrder = new Order
                {
                    Id = currentOrder.Id,
                    OrderId = id,
                    StoreId = 1,
                    CustomerId = 1,
                    OrderStatus = "In Progress",
                    PaymentStatus = "In Progress",
                    PaymentMethod = PaymentMethod,
                    OrderDiscount = 0,
                    OrderTotal = currentOrder.OrderTotal,
                    OrderDate = currentOrder.OrderDate,
                    Paid = paid,
                    Change = change,
                    VatTotal = currentOrder.VatTotal,
                    GrandTotal = currentOrder.GrandTotal,
                    PreparationStatus = currentOrder.PreparationStatus
                };

                context.Orders.Update(payingOrder);
                context.SaveChanges();
                order = payingOrder;
                orderOrderItems = context.OrderItems.Where<OrderItems>(x => x.OrderId == order.OrderId).ToList();
            }
            Order = order;
            OrderItemss = orderOrderItems;
            AllProducts = _context.Products.ToList();
            PaymentMethod = Order.PaymentMethod;
            return Page();
        }
        #endregion

        #region OnGetCompleteOrder
        public IActionResult OnGetCompleteOrder(int id)
        {
            Order lastOrder = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            StockItems = _context.Stock.ToList();
            OrderItemss = _context.OrderItems.Where(oi => oi.OrderId == id).ToList();
            AllProducts = _context.Products.ToList();
            StockToProducts = _context.StockToProducts.ToList();
            DbContextOptions<ApplicationDbContext> options = new DbContextOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                Order currentOrder = new Order
                {
                    Id = lastOrder.Id,
                    OrderId = lastOrder.OrderId,
                    StoreId = 1,
                    CustomerId = 1,
                    OrderStatus = "Completed",
                    PaymentStatus = "Paid",
                    PaymentMethod = PaymentMethod,
                    OrderDiscount = 0,
                    OrderTotal = lastOrder.OrderTotal,
                    OrderDate = lastOrder.OrderDate,
                    Paid = lastOrder.Paid,
                    Change = lastOrder.Change,
                    VatTotal = lastOrder.VatTotal,
                    GrandTotal = lastOrder.GrandTotal,
                    PreparationStatus = "Incomplete"
                };

                context.Orders.Update(currentOrder);
                context.SaveChanges();

                List<StockItem> updateStockItems = new List<StockItem>();
                Dictionary<StockToProduct, int> updateStockToProducts = new Dictionary<StockToProduct, int>();
                foreach (OrderItems orderItem in OrderItemss)
                {
                    StockToProduct stp = new StockToProduct();
                    stp = StockToProducts.FirstOrDefault(s => s.ProductId == orderItem.Product_ID);
                    if (updateStockToProducts.ContainsKey(stp))
                    {
                        updateStockToProducts[stp] = orderItem.Quantity;
                    }
                    else
                    {
                        updateStockToProducts.Add(stp, orderItem.Quantity);
                    }
                }
                foreach (KeyValuePair<StockToProduct, int> keyValuePair in updateStockToProducts)
                {
                    StockItem stock = new StockItem();
                    stock = StockItems.FirstOrDefault(si => si.Id == keyValuePair.Key.StockId);
                    decimal minusStock = keyValuePair.Key.QuantityUse * keyValuePair.Value;
                    stock.InStock = stock.InStock - minusStock;
                    updateStockItems.Add(stock);
                }
                _context.Stock.UpdateRange(updateStockItems);
                _context.SaveChanges();
            }

            return RedirectToPage("/POS");
        }
        #endregion
    }
}