using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace JRPC_HMS.Pages.QRServ.Admin
{
    [Authorize(Roles = "Admin, User")]
    public class QuestionsIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public QuestionsIndexModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
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

        public IList<Question> Questions { get; set; } = new List<Question>();
        public IList<Form> Forms { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty]
        public List<int> DeleteQuestion { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string QuestionSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string FormSort { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<Question>> InitiateView(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            CurrentSort = sortOrder;
            QuestionSort = String.IsNullOrEmpty(sortOrder) ? "question_desc" : "";
            FormSort = String.IsNullOrEmpty(sortOrder) ? "form_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var questions = from p in _context.Questions
                             select p;

            if (!string.IsNullOrEmpty(SearchString))
            {
                questions = questions.Where(s => s.QuestionName.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "category_desc":
                    questions = questions.OrderByDescending(s => s.QuestionName);
                    break;
                case "form_desc":
                    questions = questions.OrderByDescending(s => s.Form_ID);
                    break;
                default:
                    questions = questions.OrderBy(s => s.Question_ID);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = questions.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;
            Forms = _context.Forms.AsNoTracking().ToList();
            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            return await questions
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            Questions = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task OnPost(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            if (DeleteQuestion != null)
            {
                foreach (var qId in DeleteQuestion)
                {
                    _context.Questions.Remove(_context.Questions.FirstOrDefault(c => c.Question_ID == qId));
                    await _context.SaveChangesAsync();
                    TempData["StatusMessage"] = "Question " + _context.Questions.FirstOrDefault(c => c.Question_ID == qId).QuestionName + " successfully deleted.";
                }
            }
            Questions = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }

        public async Task<IActionResult> OnPostExportAsync(int currentPage, int pageSize, string categoryName, string sortOrder)
        {
            Questions = await InitiateView(currentPage, pageSize, categoryName, sortOrder);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Category Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Categories");
                ExcelWorksheet questionsSheet = package.Workbook.Worksheets["Categories"];
                using (var range = questionsSheet.Cells["A1:D1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                questionsSheet.Cells[1, 1].Value = "Question Id";
                questionsSheet.Cells[1, 2].Value = "Question Name";
                questionsSheet.Cells[1, 3].Value = "Question Description";
                questionsSheet.Cells[1, 4].Value = "Form Name";
                questionsSheet.Cells[1, 1, 1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                for (int r = 0; r < Questions.Count(); r++)
                {
                    Question questionC = _context.Questions.FirstOrDefault(c => c.Question_ID == Questions.ToList()[r].Question_ID);
                    questionsSheet.Cells[r + 2, 1].Value = Questions.ToList()[r].Question_ID;
                    questionsSheet.Cells[r + 2, 2].Value = Questions.ToList()[r].QuestionName;
                    questionsSheet.Cells[r + 2, 3].Value = Questions.ToList()[r].Description;
                    questionsSheet.Cells[r + 2, 4].Value = Forms.FirstOrDefault(f => f.Form_ID == Questions.ToList()[r].Form_ID).FormName;
                    questionsSheet.Cells[1, 1, 1, 4].AutoFitColumns();
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