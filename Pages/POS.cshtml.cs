using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JRPC_HMS.Pages
{
    public class POSModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public POSModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<Product> Products { get; set; }
        public IList<Product> AllProducts { get; set; }
        public IList<Category> Categories { get; set; }
        public Order CurrentOrder { get; set; }
        public IList<OrderItems> OrderItemss { get; set; }
        //public IList<Warehouse> WarehouseStock { get; set; }
        public IList<StockTransfer> StockTransfers { get; set; }
        [BindProperty]
        public int SelectedOrder_Id { get; set; }
        public List<SelectListItem> OutsOrders { get; set; }
        [BindProperty]
        public decimal VatTotal { get; set; }
        [BindProperty]
        public decimal GrandTotal { get; set; }


        public IActionResult OnGet()
        {
            string currentUser = _userManager.GetUserName(HttpContext.User);
            Products = _context.Products.ToList();
            AllProducts = _context.Products.ToList();
            Categories = _context.Categories.ToList();
            Order lastOrder = _context.Orders.LastOrDefault();
            List<Order> currentUserOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided" && o.Username == currentUser).ToList();
            OutsOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided").Select(n => new SelectListItem
            {
                Value = n.OrderId.ToString(),
                Text = n.OrderId.ToString()
            }).ToList();
            if (currentUserOrders.Count == 0)
            {
                DbContextOptions<ApplicationDbContext> options = new DbContextOptions<ApplicationDbContext>();
                using (var context = new ApplicationDbContext(options))
                {
                    Order newOrder = new Order
                    {
                        OrderId = lastOrder.OrderId + 1,
                        StoreId = 1,
                        CustomerId = 1,
                        OrderStatus = "Started",
                        PaymentStatus = "Incomplete",
                        PaymentMethod = "",
                        OrderDiscount = 0,
                        OrderTotal = 0,
                        OrderDate = DateTime.Now,
                        Paid = 0,
                        Change = 0,
                        VatTotal = 0,
                        GrandTotal = 0,
                        PreparationStatus = "Started",
                        Username = currentUser
                    };

                    context.Orders.Add(newOrder);
                    context.SaveChanges();
                    CurrentOrder = newOrder;
                }
            }
            else
            {
                CurrentOrder = currentUserOrders.FirstOrDefault();
            }
            OutsOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided").Select(n => new SelectListItem
            {
                Value = n.OrderId.ToString(),
                Text = n.OrderId.ToString()
            }).ToList();
            SelectedOrder_Id = CurrentOrder.OrderId;
            OrderItemss = _context.OrderItems.Where(oi => oi.OrderId == CurrentOrder.OrderId).ToList();

            return Page();
        }

        public IActionResult OnGetCategories(int catid, int orderid)
        {
            Categories = _context.Categories.ToList();
            AllProducts = _context.Products.ToList();
            Products = _context.Products.Where(p => p.Cat_ID == catid).ToList();
            CurrentOrder = _context.Orders.FirstOrDefault(o => o.OrderId == orderid);
            OrderItemss = _context.OrderItems.Where(oi => oi.OrderId == orderid).ToList();
            OutsOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided").Select(n => new SelectListItem
            {
                Value = n.OrderId.ToString(),
                Text = n.OrderId.ToString()
            }).ToList();
            SelectedOrder_Id = orderid;
            return Page();
        }

        public IActionResult OnPost()
        {
            Products = _context.Products.ToList();
            AllProducts = _context.Products.ToList();
            Categories = _context.Categories.ToList();
            OutsOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided").Select(n => new SelectListItem
            {
                Value = n.OrderId.ToString(),
                Text = n.OrderId.ToString()
            }).ToList();
            CurrentOrder = _context.Orders.FirstOrDefault(o => o.OrderId == SelectedOrder_Id);
            OrderItemss = _context.OrderItems.Where(oi => oi.OrderId == CurrentOrder.OrderId).ToList();
            SelectedOrder_Id = CurrentOrder.OrderId;
            return Page();
        }

        public IActionResult OnGetMinusQuantity(int orderid, int id, int prodId)
        {
            string currentUser = _userManager.GetUserName(HttpContext.User);
            Product product = _context.Products.FirstOrDefault(p => p.Product_ID == prodId);
            CurrentOrder = _context.Orders.FirstOrDefault(o => o.OrderId == orderid);
            OrderItems rorderItem = new OrderItems();
            rorderItem = _context.OrderItems.FirstOrDefault(oi => oi.Id == id && oi.OrderId == orderid);
            DbContextOptions<ApplicationDbContext> options = new DbContextOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                var discountAmount = CurrentOrder.OrderTotal - CurrentOrder.OrderDiscount;
                var orderTotal = discountAmount - product.SellingPrice;
                var vatTotal = orderTotal * 0.15m;
                var grandTotal = orderTotal + vatTotal;
                Order cOrder = new Order
                {
                    Id = CurrentOrder.Id,
                    OrderId = CurrentOrder.OrderId,
                    StoreId = 1,
                    CustomerId = 1,
                    OrderStatus = "In Progress",
                    PaymentStatus = "Incomplete",
                    PaymentMethod = "",
                    OrderDiscount = discountAmount,
                    OrderTotal = orderTotal,
                    OrderDate = CurrentOrder.OrderDate,
                    Paid = CurrentOrder.Paid,
                    Change = CurrentOrder.Change,
                    VatTotal = vatTotal,
                    GrandTotal = grandTotal,
                    PreparationStatus = CurrentOrder.PreparationStatus,
                    Username = currentUser
                };
                context.Orders.Update(cOrder);
                context.SaveChanges();
                CurrentOrder = cOrder;
                if (rorderItem.Quantity > 1)
                {
                    OrderItems updateOrderItem = new OrderItems
                    {
                        Id = rorderItem.Id,
                        OrderId = rorderItem.OrderId,
                        Product_ID = rorderItem.Product_ID,
                        Quantity = rorderItem.Quantity - 1
                    };
                    context.OrderItems.Update(updateOrderItem);
                    context.SaveChanges();
                }
                else
                {
                    context.OrderItems.Remove(rorderItem);
                    context.SaveChanges();
                }

                OrderItemss = context.OrderItems.Where(oi => oi.OrderId == CurrentOrder.OrderId).ToList();
            }
            AllProducts = _context.Products.ToList();
            Products = _context.Products.ToList();
            Categories = _context.Categories.ToList();
            OutsOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided").Select(n => new SelectListItem
            {
                Value = n.OrderId.ToString(),
                Text = n.OrderId.ToString()
            }).ToList();
            SelectedOrder_Id = CurrentOrder.OrderId;
            return Page();
        }

        public IActionResult OnGetOrder(int orderid, int prodId)
        {
            string currentUser = _userManager.GetUserName(HttpContext.User);
            Product product = _context.Products.FirstOrDefault(p => p.Product_ID == prodId);
            CurrentOrder = _context.Orders.FirstOrDefault(o => o.OrderId == orderid);
            List<OrderItems> coItems = _context.OrderItems.Where(oi => oi.OrderId == CurrentOrder.OrderId).ToList();
            Products = _context.Products.ToList();
            Categories = _context.Categories.ToList();
            if (prodId != 0)
            {
                DbContextOptions<ApplicationDbContext> options = new DbContextOptions<ApplicationDbContext>();
                using (var context = new ApplicationDbContext(options))
                {
                    var discountAmount = CurrentOrder.OrderTotal - CurrentOrder.OrderDiscount;
                    var orderTotal = discountAmount - product.SellingPrice;
                    var vatTotal = orderTotal * 0.15m;
                    var grandTotal = orderTotal + vatTotal;
                    Order cOrder = new Order
                    {
                        Id = CurrentOrder.Id,
                        OrderId = CurrentOrder.OrderId,
                        StoreId = 1,
                        CustomerId = 1,
                        OrderStatus = "In Progress",
                        PaymentStatus = "Incomplete",
                        PaymentMethod = "",
                        OrderDiscount = discountAmount,
                        OrderTotal = orderTotal,
                        OrderDate = CurrentOrder.OrderDate,
                        Paid = CurrentOrder.Paid,
                        Change = CurrentOrder.Change,
                        VatTotal = vatTotal,
                        GrandTotal = grandTotal,
                        PreparationStatus = CurrentOrder.PreparationStatus,
                        Username = currentUser
                    };
                    context.Orders.Update(cOrder);
                    context.SaveChanges();
                    CurrentOrder = cOrder;
                    OrderItems orderItem = new OrderItems();
                    foreach (var oI in coItems)
                    {
                        if (oI.Product_ID == product.Product_ID)
                        {
                            orderItem = oI;
                        }
                    }
                    if (orderItem.Id != 0)
                    {
                        OrderItems updateOrderItem = new OrderItems
                        {
                            Id = orderItem.Id,
                            OrderId = CurrentOrder.OrderId,
                            Product_ID = product.Product_ID,
                            Quantity = orderItem.Quantity + 1
                        };
                        context.OrderItems.Update(updateOrderItem);
                        context.SaveChanges();
                    }
                    else
                    {
                        OrderItems newOrderItem = new OrderItems
                        {
                            OrderId = CurrentOrder.OrderId,
                            Product_ID = product.Product_ID,
                            Quantity = 1
                        };
                        context.OrderItems.Add(newOrderItem);
                        context.SaveChanges();
                    }
                    OrderItemss = context.OrderItems.Where(oi => oi.OrderId == CurrentOrder.OrderId).ToList();
                }
            }
            AllProducts = _context.Products.ToList();

            OutsOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided").Select(n => new SelectListItem
            {
                Value = n.OrderId.ToString(),
                Text = n.OrderId.ToString()
            }).ToList();
            SelectedOrder_Id = CurrentOrder.OrderId;
            return Page();
        }

        public IActionResult OnGetVoidOrder(int orderid)
        {
            Order voidedOrder = _context.Orders.FirstOrDefault(o => o.OrderId == orderid);
            voidedOrder.OrderStatus = "Voided";
            _context.Orders.Update(voidedOrder);
            _context.SaveChanges();

            string currentUser = _userManager.GetUserName(HttpContext.User);
            Products = _context.Products.ToList();
            AllProducts = _context.Products.ToList();
            Categories = _context.Categories.ToList();
            Order lastOrder = _context.Orders.LastOrDefault();
            List<Order> currentUserOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided" && o.Username == currentUser).ToList();
            OutsOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided").Select(n => new SelectListItem
            {
                Value = n.OrderId.ToString(),
                Text = n.OrderId.ToString()
            }).ToList();
            if (currentUserOrders.Count == 0)
            {
                DbContextOptions<ApplicationDbContext> options = new DbContextOptions<ApplicationDbContext>();
                using (var context = new ApplicationDbContext(options))
                {
                    Order newOrder = new Order
                    {
                        OrderId = lastOrder.OrderId + 1,
                        StoreId = 1,
                        CustomerId = 1,
                        OrderStatus = "Started",
                        PaymentStatus = "Incomplete",
                        PaymentMethod = "",
                        OrderDiscount = 0,
                        OrderTotal = 0,
                        OrderDate = DateTime.Now,
                        Paid = 0,
                        Change = 0,
                        VatTotal = 0,
                        GrandTotal = 0,
                        PreparationStatus = "Started",
                        Username = currentUser
                    };

                    context.Orders.Add(newOrder);
                    context.SaveChanges();
                    CurrentOrder = newOrder;
                }
            }
            else
            {
                CurrentOrder = currentUserOrders.FirstOrDefault();
            }
            OutsOrders = _context.Orders.Where(o => o.OrderStatus != "Completed" && o.OrderStatus != "Voided").Select(n => new SelectListItem
            {
                Value = n.OrderId.ToString(),
                Text = n.OrderId.ToString()
            }).ToList();
            SelectedOrder_Id = CurrentOrder.OrderId;
            OrderItemss = _context.OrderItems.Where(oi => oi.OrderId == CurrentOrder.OrderId).ToList();
            return Page();
        }
    }
}