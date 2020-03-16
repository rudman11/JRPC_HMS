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

namespace JRPC_HMS.Pages.WarehousePages
{
    [Authorize(Roles = "Admin, User")]
    public class SupplierIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public SupplierIndexModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
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

        public IList<Supplier> Suppliers { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty]
        public List<int> DeleteSupplier { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SupplierSort { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<Supplier>> InitiateView(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            CurrentSort = sortOrder;
            SupplierSort = String.IsNullOrEmpty(sortOrder) ? "supplier_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var suppliers = from p in _context.Suppliers
                             select p;

            if (!string.IsNullOrEmpty(SearchString))
            {
                suppliers = suppliers.Where(s => s.Name.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "supplier_desc":
                    suppliers = suppliers.OrderByDescending(s => s.Name);
                    break;
                default:
                    suppliers = suppliers.OrderBy(s => s.Id);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = suppliers.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            return await suppliers
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            Suppliers = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task OnPostAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            if (DeleteSupplier != null)
            {
                foreach (var stockId in DeleteSupplier)
                {
                    Supplier supplier = _context.Suppliers.FirstOrDefault(p => p.Id == stockId);
                    _context.Suppliers.Remove(supplier);
                    await _context.SaveChangesAsync();
                    TempData["StatusMessage"] = "Warehouse stock \"" + supplier.Name + "\" succcessfully deleted.";
                }
            }

            Suppliers = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task<IActionResult> OnPostExport(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            Suppliers = await InitiateView(currentPage, pageSize, searchString, sortOrder);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Suppliers Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Suppliers");
                ExcelWorksheet categoriesSheet = package.Workbook.Worksheets["Suppliers"];
                using (var range = categoriesSheet.Cells["A1:F1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                categoriesSheet.Cells[1, 1].Value = "Id";
                categoriesSheet.Cells[1, 2].Value = "Name";
                categoriesSheet.Cells[1, 3].Value = "Email";
                categoriesSheet.Cells[1, 4].Value = "Phone";
                categoriesSheet.Cells[1, 5].Value = "Address";
                categoriesSheet.Cells[1, 6].Value = "Contact";
                for (int r = 0; r < Suppliers.Count(); r++)
                {
                    categoriesSheet.Cells[r + 2, 1].Value = Suppliers.ToList()[r].Id;
                    categoriesSheet.Cells[r + 2, 2].Value = Suppliers.ToList()[r].Name;
                    categoriesSheet.Cells[r + 2, 3].Value = Suppliers.ToList()[r].Email;
                    categoriesSheet.Cells[r + 2, 4].Value = Suppliers.ToList()[r].Phone;
                    categoriesSheet.Cells[r + 2, 5].Value = Suppliers.ToList()[r].Address;
                    categoriesSheet.Cells[r + 2, 6].Value = Suppliers.ToList()[r].Contact;
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
            TempData["StatusMessage"] = "Supplier list successfully exported to file " + sFileName + ".";
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}