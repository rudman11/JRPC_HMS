using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace JRPC_HMS.Pages.Catalog
{
    [Authorize(Roles = "Admin, User")]
    public class ProductEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductEditModel(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }
        [BindProperty]
        public IFormFile UploadImage { set; get; }
        public List<SelectListItem> Categories { get; set; }
        public IList<Category> AllCategories { get; set; }
        public IList<StockItem> StockItems { get; set; }
        public IList<StockToProduct> StockToProducts { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedTag { get; set; }
        [BindProperty]
        public IList<string> SelectedStockItems { get; set; }
        [BindProperty(SupportsGet = true)]
        public IList<string> StockItemChecked { get; set; }
        [BindProperty]
        public IList<string> SelectedStockQuantities { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            StockToProducts = _context.StockToProducts.ToList();
            Product = _context.Products.FirstOrDefault(m => m.Product_ID == id);
            Categories = _context.Categories.Select(n => new SelectListItem
            {
                Value = n.Cat_ID.ToString(),
                Text = n.CategoryName
            }).ToList();
            AllCategories = _context.Categories.ToList();
            SelectedTag = AllCategories.FirstOrDefault(c => c.Cat_ID == Product.Cat_ID).Cat_ID.ToString();
            StockItems = _context.Stock.ToList();

            List<string> stc = new List<string>();
            List<string> ssq = new List<string>();
            for (int stp = 0; stp < StockToProducts.Count; stp++)
            {
                if (StockToProducts[stp].ProductId == Product.Product_ID)
                {
                    stc.Add(StockItems[stp].StockName);
                    string qty = StockToProducts.FirstOrDefault(s => s.Id == StockToProducts[stp].Id).QuantityUse.ToString();
                    ssq.Add(qty);
                }
            }

            StockItemChecked = stc;
            SelectedStockQuantities = ssq;

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            StockToProducts = _context.StockToProducts.ToList();
            //Product = _context.Products.FirstOrDefault(m => m.Product_ID == Product.Product_ID);
            AllCategories = _context.Categories.ToList();
            StockItems = _context.Stock.ToList();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string imageurl = Product.ImageUrl;
                if (UploadImage != null)
                {
                    var fileName = Path.Combine(_hostingEnvironment.WebRootPath, "img", UploadImage.FileName);
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        await UploadImage.CopyToAsync(fileStream);
                    }
                    Product.ImageUrl = "\\img\\" + UploadImage.FileName; // Set the file name
                }
                else
                {
                    Product.ImageUrl = imageurl;
                }
                Product.Cat_ID = Convert.ToInt32(SelectedTag);
                decimal stockPrice = 0;
                if (StockItemChecked != null || StockItemChecked.Count > 0)
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
                        decimal qty = ssqNums[s];
                        decimal productStockPrice = stock.Price * qty;
                        stockPrice = stockPrice + productStockPrice;
                        StockToProduct stp = new StockToProduct();
                        stp = _context.StockToProducts.FirstOrDefault(st => st.StockId == stock.Id && st.ProductId == Product.Product_ID);
                        if (stp == null)
                        {
                            StockToProduct newStockToProduct = new StockToProduct
                            {
                                StockId = stock.Id,
                                ProductId = Product.Product_ID,
                                QuantityUse = ssqNums[s]
                            };
                            _context.StockToProducts.Add(newStockToProduct);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            stp.QuantityUse = ssqNums[s];
                            _context.StockToProducts.Update(stp);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                Product.CostPrice = stockPrice;
                Product.ProfitMargin = Product.SellingPrice - stockPrice;
                _context.Products.Update(Product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Product_ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["StatusMessage"] = "Product " + Product.ProductName + " successfully edited.";

            return RedirectToPage("./ProductIndex");
        }

        private string GetUniqueName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_" + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Product_ID == id);
        }
    }
}