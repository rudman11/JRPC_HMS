using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JRPC_HMS
{
    public class LogPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LogPageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        public SelectList PageSizeList { get; set; } = new SelectList(new[] { 10, 20, 50, 100, 200 });
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string LoggedSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string LevelSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string PageSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MethodSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MessageSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ExceptionSort { get; set; }
        public IList<LogModel> Log { get; set; }

        public async Task OnGetAsync(int currentPage, int pageSize, string sortOrder)
        {
            CurrentSort = sortOrder;
            LoggedSort = String.IsNullOrEmpty(sortOrder) ? "logged_desc" : "";
            LevelSort = String.IsNullOrEmpty(sortOrder) ? "level_desc" : "";
            PageSort = String.IsNullOrEmpty(sortOrder) ? "page_desc" : "";
            MethodSort = String.IsNullOrEmpty(sortOrder) ? "method_desc" : "";
            MessageSort = String.IsNullOrEmpty(sortOrder) ? "message_desc" : "";
            ExceptionSort = String.IsNullOrEmpty(sortOrder) ? "exception_desc" : "";

            var logs = from l in _context.Log
                       select l;

            switch (sortOrder)
            {
                case "logged_desc":
                    logs = logs.OrderByDescending(s => s.Logged);
                    break;
                case "level_desc":
                    logs = logs.OrderByDescending(s => s.Level);
                    break;
                case "page_desc":
                    logs = logs.OrderByDescending(s => s.Logger);
                    break;
                case "method_desc":
                    logs = logs.OrderByDescending(s => s.Callsite);
                    break;
                case "message_desc":
                    logs = logs.OrderByDescending(s => s.Message);
                    break;
                case "exception_desc":
                    logs = logs.OrderByDescending(s => s.Exception);
                    break;
                default:
                    logs = logs.OrderByDescending(s => s.Id);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = logs.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            Log = await logs
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(int currentPage, int pageSize, string sortOrder)
        {
            List<LogModel> logss = _context.Log.ToList();
            _context.Log.RemoveRange(logss);
            _context.SaveChanges();

            CurrentSort = sortOrder;
            LoggedSort = String.IsNullOrEmpty(sortOrder) ? "logged_desc" : "";
            LevelSort = String.IsNullOrEmpty(sortOrder) ? "level_desc" : "";
            PageSort = String.IsNullOrEmpty(sortOrder) ? "page_desc" : "";
            MethodSort = String.IsNullOrEmpty(sortOrder) ? "method_desc" : "";
            MessageSort = String.IsNullOrEmpty(sortOrder) ? "message_desc" : "";
            ExceptionSort = String.IsNullOrEmpty(sortOrder) ? "exception_desc" : "";

            var logs = from l in _context.Log
                       select l;

            switch (sortOrder)
            {
                case "logged_desc":
                    logs = logs.OrderByDescending(s => s.Logged);
                    break;
                case "level_desc":
                    logs = logs.OrderByDescending(s => s.Level);
                    break;
                case "page_desc":
                    logs = logs.OrderByDescending(s => s.Logger);
                    break;
                case "method_desc":
                    logs = logs.OrderByDescending(s => s.Callsite);
                    break;
                case "message_desc":
                    logs = logs.OrderByDescending(s => s.Message);
                    break;
                case "exception_desc":
                    logs = logs.OrderByDescending(s => s.Exception);
                    break;
                default:
                    logs = logs.OrderByDescending(s => s.Id);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = logs.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            
            Log = await logs
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }
    }
}