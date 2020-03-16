using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace JRPC_HMS.Pages.Catalog
{
    [Authorize(Roles = "Admin, User")]
    public class ProductCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductCreateModel(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        [BindProperty]
        public Product Product { get; set; }
        [BindProperty]
        public IFormFile UploadImage { set; get; }
        public IList<SelectListItem> Categories { get; set; }
        public IList<StockItem> StockItems { get; set; }
        public IList<StockToProduct> stockToProducts { get; set; }
        [BindProperty]
        public IList<string> StockItemChecked { get; set; }
        [BindProperty]
        public string SelectedTag { get; set; }
        [BindProperty]
        public IList<string> SelectedStockQuantities { get; set; }

        public IActionResult OnGet()
        {
            Categories = _context.Categories.Select(n => new SelectListItem
            {
                Value = n.Cat_ID.ToString(),
                Text = n.CategoryName
            }).ToList();
            StockItems = _context.Stock.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Categories = _context.Categories.Select(n => new SelectListItem
            {
                Value = n.Cat_ID.ToString(),
                Text = n.CategoryName
            }).ToList();
            StockItems = _context.Stock.ToList();

            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}            

            if (UploadImage != null)
            {
                var fileName = Path.Combine(_hostingEnvironment.WebRootPath, "img", UploadImage.FileName);
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await UploadImage.CopyToAsync(fileStream);
                }
                Product.ImageUrl = "\\img\\" + UploadImage.FileName; // Set the file name
            }
            Product.Cat_ID = Convert.ToInt32(SelectedTag);
            decimal stockPrice = 0;
            if (StockItemChecked != null)
            {
                List<decimal> ssqNums = new List<decimal>();
                foreach (string ssq in SelectedStockQuantities)
                {
                    if (ssq != null)
                    {
                        decimal quan = 0m;
                        quan = decimal.Parse(ssq, CultureInfo.InvariantCulture);
                        ssqNums.Add(quan);
                    }
                }
                for (int s = 0; s < StockItemChecked.Count; s++)
                {
                    StockItem stock = new StockItem();
                    stock = StockItems.FirstOrDefault(st => st.StockName == StockItemChecked[s]);
                    decimal productStockPrice = stock.Price * ssqNums[s];
                    stockPrice = stockPrice + productStockPrice;

                    StockToProduct newStockToProduct = new StockToProduct
                    {
                        StockId = stock.Id,
                        ProductId = Product.Product_ID,
                        QuantityUse = ssqNums[s]
                    };
                    _context.StockToProducts.Add(newStockToProduct);
                    await _context.SaveChangesAsync();
                }
            }
            Product.CostPrice = stockPrice;
            Product.ProfitMargin = Product.SellingPrice - stockPrice;
            _context.Products.Update(Product);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "Product " + Product.ProductName + " successfully created.";
            return RedirectToPage("./ProductIndex");
        }
    }

    public class StockToProductsModel
    {
        public string StockName { get; set; }
        public decimal QuantityUse { get; set; }
    }
}