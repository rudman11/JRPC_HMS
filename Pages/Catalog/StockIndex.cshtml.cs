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
    public class StockIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public StockIndexModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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

        public IList<StockItem> StockItem { get; set; }
        public IList<Store> Stores { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string StockNameSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string InStockSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string StoreNameSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty]
        public List<int> DeleteStock { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<StockItem>> InitiateView(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            CurrentSort = sortOrder;
            StockNameSort = String.IsNullOrEmpty(sortOrder) ? "stockname_desc" : "";
            InStockSort = String.IsNullOrEmpty(sortOrder) ? "instock_desc" : "";
            StoreNameSort = String.IsNullOrEmpty(sortOrder) ? "storename_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var stock = from s in _context.Stock
                        select s;
            if (!string.IsNullOrEmpty(SearchString))
            {
                stock = stock.Where(s => 
                    s.StockName.Contains(SearchString) || 
                    s.StoreId == _context.Stores.FirstOrDefault(st => st.StoreName.Contains(SearchString)).Store_ID);
            }
            switch (sortOrder)
            {
                case "stockname_desc":
                    stock = stock.OrderByDescending(s => s.StockName);
                    break;
                case "instock_desc":
                    stock = stock.OrderByDescending(s => s.InStock);
                    break;
                case "storename_desc":
                    stock = stock.OrderByDescending(s => s.StoreId);
                    break;
                default:
                    stock = stock.OrderBy(s => s.Id);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = stock.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            Stores = _context.Stores.ToList();

            return await stock
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            StockItem = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task OnPostAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            if (DeleteStock != null)
            {
                foreach (var stockId in DeleteStock)
                {
                    _context.Stock.Remove(_context.Stock.FirstOrDefault(p => p.Id == stockId));
                    await _context.SaveChangesAsync();
                    TempData["StatusMessage"] = "Stock " + _context.Stock.FirstOrDefault(c => c.Id == stockId).StockName + " successfully deleted.";
                }
            }
            StockItem = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task<IActionResult> OnPostExport(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            StockItem = await InitiateView(currentPage, pageSize, searchString, sortOrder);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Stock Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Stock");
                ExcelWorksheet stockSheet = package.Workbook.Worksheets["Stock"];
                using (var range = stockSheet.Cells["A1:F1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                stockSheet.Cells[1, 1].Value = "Id";
                stockSheet.Cells[1, 2].Value = "Stock Name";
                stockSheet.Cells[1, 3].Value = "Unit";
                stockSheet.Cells[1, 4].Value = "Price";
                stockSheet.Cells[1, 5].Value = "In Stock";
                stockSheet.Cells[1, 6].Value = "Store";
                for (int r = 0; r < StockItem.Count(); r++)
                {
                    stockSheet.Cells[r + 2, 1].Value = StockItem.ToList()[r].Id;
                    stockSheet.Cells[r + 2, 2].Value = StockItem.ToList()[r].StockName;
                    stockSheet.Cells[r + 2, 3].Value = StockItem.ToList()[r].Unit;
                    stockSheet.Cells[r + 2, 4].Value = StockItem.ToList()[r].Price;
                    stockSheet.Cells[r + 2, 5].Value = StockItem.ToList()[r].InStock;
                    int storeId = StockItem.ToList()[r].StoreId;
                    stockSheet.Cells[r + 2, 6].Value = _context.Stores.FirstOrDefault(s => s.Store_ID == storeId).StoreName;
                    stockSheet.Cells[1, 1, 1, 6].AutoFitColumns();
                }

                package.SaveAs(fs);
                package.Dispose();
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            TempData["StatusMessage"] = "Stock successfully exported to file " + sFileName + ".";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}