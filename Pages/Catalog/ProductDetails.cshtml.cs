using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace JRPC_HMS.Pages.Catalog
{
    [Authorize(Roles = "Admin, User")]
    public class ProductDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProductDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; }
        public IList<Category> Categories { get; set; }
        public IList<StockItem> StockItems { get; set; }
        public IList<StockToProduct> stockToProducts { get; set; }
        public List<StockToProduct_StockItems> StockToProductStockItems { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Products.FirstOrDefaultAsync(m => m.Product_ID == id);
            Categories = _context.Categories.ToList();
            StockItems = _context.Stock.ToList();
            stockToProducts = _context.StockToProducts.Where(stp => stp.ProductId == id).ToList();
            StockToProductStockItems = new List<StockToProduct_StockItems>();
            foreach (var stp in stockToProducts)
            {
                StockToProduct_StockItems stp_si = new StockToProduct_StockItems
                {
                    StockName = StockItems.FirstOrDefault(si => si.Id == stp.StockId).StockName,
                    Quantity = stp.QuantityUse
                };
                StockToProductStockItems.Add(stp_si);
            }
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
    }

    public class StockToProduct_StockItems
    {
        public string StockName { get; set; }
        public decimal Quantity { get; set; }
    }
}