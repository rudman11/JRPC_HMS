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
    public class CatIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public CatIndexModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
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

        public IList<Category> Categories { get; set; } = new List<Category>();
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty]
        public List<int> DeleteCategory { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CategorySort { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<Category>> InitiateView(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            CurrentSort = sortOrder;
            CategorySort = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var categories = from p in _context.Categories
                             select p;

            if (!string.IsNullOrEmpty(SearchString))
            {
                categories = categories.Where(s => s.CategoryName.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "category_desc":
                    categories = categories.OrderByDescending(s => s.CategoryName);
                    break;
                default:
                    categories = categories.OrderBy(s => s.Cat_ID);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = categories.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            return await categories
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            Categories = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task OnPost(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            if (DeleteCategory != null)
            {
                foreach (var catId in DeleteCategory)
                {
                    _context.Categories.Remove(_context.Categories.FirstOrDefault(c => c.Cat_ID == catId));
                    await _context.SaveChangesAsync();
                    TempData["StatusMessage"] = "Category " + _context.Categories.FirstOrDefault(c => c.Cat_ID == catId).CategoryName + " successfully deleted.";
                }
            }
            Categories = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task<IActionResult> OnPostExportAsync(int currentPage, int pageSize, string categoryName, string sortOrder)
        {
            Categories = await InitiateView(currentPage, pageSize, categoryName, sortOrder);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Category Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Categories");
                ExcelWorksheet categoriesSheet = package.Workbook.Worksheets["Categories"];
                using (var range = categoriesSheet.Cells["A1:C1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                categoriesSheet.Cells[1, 1].Value = "Category Id";
                categoriesSheet.Cells[1, 2].Value = "Category Name";
                categoriesSheet.Cells[1, 3].Value = "Category Description";
                categoriesSheet.Cells[1, 1, 1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                for (int r = 0; r < Categories.Count(); r++)
                {
                    Category categoryC = _context.Categories.FirstOrDefault(c => c.Cat_ID == Categories.ToList()[r].Cat_ID);
                    categoriesSheet.Cells[r + 2, 1].Value = Categories.ToList()[r].Cat_ID;
                    categoriesSheet.Cells[r + 2, 2].Value = Categories.ToList()[r].CategoryName;
                    categoriesSheet.Cells[r + 2, 3].Value = Categories.ToList()[r].CategoryDescription;
                    categoriesSheet.Cells[1, 1, 1, 3].AutoFitColumns();
                }

                package.SaveAs(fs);
                package.Dispose();
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            TempData["StatusMessage"] = "Categories successfully exported to file " + sFileName;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}