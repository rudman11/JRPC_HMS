using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.Catalog
{
    [Authorize(Roles = "Admin, User")]
    public class ProductIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public ProductIndexModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        #region Properties
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        public SelectList PageSizeList { get; set; } = new SelectList(new[] { 10, 20, 50, 100, 200 });

        public IList<Product> Products { get; set; }
        public IList<Category> AllCategories { get; set; }
        public IList<SelectListItem> Categories { get; set; }
        public IList<StockItem> StockItems { get; set; }
        public IList<StockToProduct> StockToProducts { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty]
        public IList<string> DeleteProducts { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ProductSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SellingSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CostSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ProfitSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CategorySort { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<Product>> InitiateView(int currentPage, int pageSize, string sortOrder, string searchString)
        {
            CurrentSort = sortOrder;
            ProductSort = String.IsNullOrEmpty(sortOrder) ? "product_desc" : "";
            SellingSort = String.IsNullOrEmpty(sortOrder) ? "selling_desc" : "";
            CostSort = String.IsNullOrEmpty(sortOrder) ? "cost_desc" : "";
            ProfitSort = String.IsNullOrEmpty(sortOrder) ? "profit_desc" : "";
            CategorySort = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var products = from p in _context.Products
                           select p;

            if (!string.IsNullOrEmpty(SearchString))
            {
                products = products.Where(p => p.ProductName.Contains(SearchString) || p.Cat_ID == _context.Categories.FirstOrDefault(c => c.CategoryName.Contains(SearchString)).Cat_ID);
            }

            switch (sortOrder)
            {
                case "product_desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    break;
                case "selling_desc":
                    products = products.OrderByDescending(s => s.SellingPrice);
                    break;
                case "cost_desc":
                    products = products.OrderByDescending(s => s.CostPrice);
                    break;
                case "profit_desc":
                    products = products.OrderByDescending(s => s.ProfitMargin);
                    break;
                case "category_desc":
                    products = products.OrderByDescending(s => s.Cat_ID);
                    break;
                default:
                    products = products.OrderByDescending(s => s.Product_ID);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = products.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;


            AllCategories = _context.Categories.ToList();
            Categories = _context.Categories.Select(n => new SelectListItem
            {
                Value = n.Cat_ID.ToString(),
                Text = n.CategoryName
            }).ToList();

            return await products
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string sortOrder, string searchString)
        {
            Products = await InitiateView(currentPage, pageSize, sortOrder, searchString);
        }

        public async Task OnPostAsync(int currentPage, int pageSize, string sortOrder, string searchString)
        {
            if (DeleteProducts != null)
            {
                foreach (var prodId in DeleteProducts)
                {
                    _context.Products.Remove(_context.Products.FirstOrDefault(p => p.ProductName == prodId));
                    await _context.SaveChangesAsync();
                    TempData["StatusMessage"] = "Product " + _context.Products.FirstOrDefault(c => c.Product_ID == Convert.ToInt32(prodId)).ProductName + " successfully deleted.";
                }
            }

            Products = await InitiateView(currentPage, pageSize, sortOrder, searchString);
        }

        public async Task<IActionResult> OnPostExport(int currentPage, int pageSize, string sortOrder, string searchString)
        {
            Products = await InitiateView(currentPage, pageSize, sortOrder, searchString);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Product Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Products");
                ExcelWorksheet productsSheet = package.Workbook.Worksheets["Products"];
                using (var range = productsSheet.Cells["A1:I1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                productsSheet.Cells[1, 1].Value = "Product Id";
                productsSheet.Cells[1, 2].Value = "Product Name";
                productsSheet.Cells[1, 3].Value = "Product Short Description";
                productsSheet.Cells[1, 4].Value = "Product Detailed Description";
                productsSheet.Cells[1, 5].Value = "Selling Price";
                productsSheet.Cells[1, 6].Value = "Cost Price";
                productsSheet.Cells[1, 7].Value = "Profit Margin";
                productsSheet.Cells[1, 8].Value = "Category";
                productsSheet.Cells[1, 9].Value = "Stock Used";
                for (int r = 0; r < Products.Count(); r++)
                {
                    productsSheet.Cells[r + 2, 1].Value = Products.ToList()[r].Product_ID;
                    productsSheet.Cells[r + 2, 2].Value = Products.ToList()[r].ProductName;
                    productsSheet.Cells[r + 2, 3].Value = Products.ToList()[r].ProductShortDescription;
                    productsSheet.Cells[r + 2, 4].Value = Products.ToList()[r].ProductDetailedDescription;
                    productsSheet.Cells[r + 2, 5].Value = Products.ToList()[r].SellingPrice;
                    productsSheet.Cells[r + 2, 6].Value = Products.ToList()[r].CostPrice;
                    productsSheet.Cells[r + 2, 7].Value = Products.ToList()[r].ProfitMargin;
                    productsSheet.Cells[r + 2, 8].Value = AllCategories.FirstOrDefault(c => c.Cat_ID == Products.ToList()[r].Cat_ID).CategoryName;
                    string stock = string.Empty;
                    StockToProducts = _context.StockToProducts.ToList();
                    foreach(var stp in StockToProducts.Where(s => s.ProductId == Products.ToList()[r].Product_ID))
                    {
                        if (stock == "")
                        {
                            stock += _context.Stock.FirstOrDefault(st => st.Id == stp.StockId).StockName;
                        }
                        else
                        {
                            stock += ", " + _context.Stock.FirstOrDefault(st => st.Id == stp.StockId).StockName;
                        }
                    }
                    productsSheet.Cells[r + 2, 9].Value = stock;  // List All stock for this product
                    productsSheet.Cells[1, 1, 1, 9].AutoFitColumns();
                }

                package.SaveAs(fs);
                package.Dispose();
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            TempData["StatusMessage"] = "Products successfully exported to file " + sFileName + ".";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}