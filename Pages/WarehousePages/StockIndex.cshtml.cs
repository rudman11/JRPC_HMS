using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Hosting.Internal;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace JRPC_HMS.Pages.WarehousePages
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

        public IList<Warehouse> WarehouseStock { get; set; }
        public IList<Supplier> Suppliers { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string StockNameSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string InStockSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SupplierSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty]
        public List<int> DeleteStock { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<Warehouse>> InitiateView(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            CurrentSort = sortOrder;
            StockNameSort = String.IsNullOrEmpty(sortOrder) ? "stockname_desc" : "";
            InStockSort = String.IsNullOrEmpty(sortOrder) ? "instock_desc" : "";
            SupplierSort = String.IsNullOrEmpty(sortOrder) ? "supplier_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var warehousestock = from s in _context.WarehouseStock
                        select s;
            if (!string.IsNullOrEmpty(SearchString))
            {
                warehousestock = warehousestock.Where(s =>
                    s.StockName.Contains(SearchString) ||
                    s.SupplierId == _context.Suppliers.FirstOrDefault(st => st.Name.Contains(SearchString)).Id);
            }
            switch (sortOrder)
            {
                case "stockname_desc":
                    warehousestock = warehousestock.OrderByDescending(s => s.StockName);
                    break;
                case "instock_desc":
                    warehousestock = warehousestock.OrderByDescending(s => s.InStock);
                    break;
                case "supplier_desc":
                    warehousestock = warehousestock.OrderByDescending(s => s.SupplierId);
                    break;
                default:
                    warehousestock = warehousestock.OrderBy(s => s.Id);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = warehousestock.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            Suppliers = _context.Suppliers.ToList();
            return await warehousestock
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            WarehouseStock = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task OnPostAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            if (DeleteStock != null)
            {
                foreach (var stockId in DeleteStock)
                {
                    Warehouse warehouse = _context.WarehouseStock.FirstOrDefault(p => p.Id == stockId);
                    _context.WarehouseStock.Remove(warehouse);
                    await _context.SaveChangesAsync();
                    TempData["StatusMessage"] = "Warehouse stock \"" + warehouse.StockName + "\" succcessfully deleted.";
                }
            }

            WarehouseStock = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task<IActionResult> OnPostExportAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            WarehouseStock = await InitiateView(currentPage, pageSize, searchString, sortOrder);
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Warehouse Stock Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Warehouse Stock");
                ExcelWorksheet categoriesSheet = package.Workbook.Worksheets["Warehouse Stock"];
                using (var range = categoriesSheet.Cells["A1:F1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                categoriesSheet.Cells[1, 1].Value = "Id";
                categoriesSheet.Cells[1, 2].Value = "Stock Name";
                categoriesSheet.Cells[1, 3].Value = "Unit";
                categoriesSheet.Cells[1, 4].Value = "Price";
                categoriesSheet.Cells[1, 5].Value = "In Stock";
                categoriesSheet.Cells[1, 6].Value = "Supplier";
                for (int r = 0; r < WarehouseStock.Count(); r++)
                {
                    categoriesSheet.Cells[r + 2, 1].Value = WarehouseStock.ToList()[r].Id;
                    categoriesSheet.Cells[r + 2, 2].Value = WarehouseStock.ToList()[r].StockName;
                    categoriesSheet.Cells[r + 2, 3].Value = WarehouseStock.ToList()[r].Unit;
                    categoriesSheet.Cells[r + 2, 4].Value = WarehouseStock.ToList()[r].Price;
                    categoriesSheet.Cells[r + 2, 5].Value = WarehouseStock.ToList()[r].InStock;
                    categoriesSheet.Cells[r + 2, 6].Value = _context.Suppliers.FirstOrDefault(s => s.Id == WarehouseStock.ToList()[r].SupplierId).Name;
                    categoriesSheet.Cells[1, 1, 1, 6].AutoFitColumns();
                }

                package.SaveAs(fs);
                package.Dispose();
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            TempData["StatusMessage"] = "Warehouse stock list succcessfully exported to file " + sFileName + ".";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}