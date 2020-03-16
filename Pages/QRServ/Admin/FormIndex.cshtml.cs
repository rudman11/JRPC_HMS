using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace JRPC_HMS.Pages.QRServ.Admin
{
    public class FormIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public FormIndexModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
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

        public IList<Form> Forms { get; set; } = new List<Form>();
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty]
        public List<int> DeleteForm { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FormSort { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<Form>> InitiateView(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            CurrentSort = sortOrder;
            FormSort = String.IsNullOrEmpty(sortOrder) ? "form_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var forms = from p in _context.Forms
                             select p;

            if (!string.IsNullOrEmpty(SearchString))
            {
                forms = forms.Where(s => s.FormName.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "form_desc":
                    forms = forms.OrderByDescending(s => s.FormName);
                    break;
                default:
                    forms = forms.OrderBy(s => s.Form_ID);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = forms.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            return await forms
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            Forms = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task OnPost(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            if (DeleteForm != null)
            {
                foreach (var formId in DeleteForm)
                {
                    _context.Forms.Remove(_context.Forms.FirstOrDefault(c => c.Form_ID == formId));
                    await _context.SaveChangesAsync();
                    TempData["StatusMessage"] = "Form " + _context.Forms.FirstOrDefault(c => c.Form_ID == formId).FormName + " successfully deleted.";
                }
            }
            Forms = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task<IActionResult> OnPostExportAsync(int currentPage, int pageSize, string categoryName, string sortOrder)
        {
            Forms = await InitiateView(currentPage, pageSize, categoryName, sortOrder);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Forms Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Forms");
                ExcelWorksheet formsSheet = package.Workbook.Worksheets["Forms"];
                using (var range = formsSheet.Cells["A1:C1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                formsSheet.Cells[1, 1].Value = "Form Id";
                formsSheet.Cells[1, 2].Value = "Form Name";
                formsSheet.Cells[1, 3].Value = "Form Status";
                formsSheet.Cells[1, 1, 1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                for (int r = 0; r < Forms.Count(); r++)
                {
                    Form formC = _context.Forms.FirstOrDefault(c => c.Form_ID == Forms.ToList()[r].Form_ID);
                    formsSheet.Cells[r + 2, 1].Value = Forms.ToList()[r].Form_ID;
                    formsSheet.Cells[r + 2, 2].Value = Forms.ToList()[r].FormName;
                    formsSheet.Cells[r + 2, 3].Value = Forms.ToList()[r].Status;
                    formsSheet.Cells[1, 1, 1, 3].AutoFitColumns();
                }

                package.SaveAs(fs);
                package.Dispose();
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            TempData["StatusMessage"] = "Forms successfully exported to file " + sFileName;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}