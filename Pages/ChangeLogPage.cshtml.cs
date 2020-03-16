using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JRPC_HMS
{
    public class ChangeLogPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangeLogPageModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public bool EnablePrevious => CurrentPage > 1;
        public bool EnableNext => CurrentPage < TotalPages;
        public SelectList PageSizeList { get; set; } = new SelectList(new[] { 5, 10, 20, 50, 100, 200 });
        public IList<Change> ChangeLog { get; set; }
        [BindProperty]
        public string ChangeName { get; set; }
        [BindProperty]
        public string ChangeDescription { get; set; }
        [BindProperty]
        public string UpdateChangeName { get; set; }
        [BindProperty]
        public string UpdateChangeDescription { get; set; }
        [BindProperty]
        public string UpdateChangeCompletion { get; set; }
        [BindProperty]
        public int UpdateFeatureId { get; set; }

        public async Task OnGet(int currentPage, int pageSize)
        {
            var changelog = from nf in _context.ChangeLog.AsNoTracking()
                              select nf;

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = changelog.Count();

            PageSize = pageSize == 0 ? 5 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            ChangeLog = await changelog
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPost(int currentPage, int pageSize)
        {
            Change change = new Change()
            {
                ChangeName = ChangeName,
                Description = ChangeDescription,
                DateAdded = DateTime.Now,
                PercentageCompleted = 0
            };
            _context.ChangeLog.Add(change);
            _context.SaveChanges();

            ChangeName = "";
            ChangeDescription = "";

            var newChange = from nf in _context.ChangeLog.AsNoTracking()
                              select nf;

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = newChange.Count();

            PageSize = pageSize == 0 ? 5 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            ChangeLog = await newChange
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            List<ApplicationUser> users = new List<ApplicationUser>();
            users.AddRange(_userManager.Users);

            foreach (var user in users)
            {
                user.ReadWhatsNew = "No";
                var result = await _userManager.UpdateAsync(user);
            }

            TempData["StatusMessage"] = "Successfully added new change";
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteFeature(int currentPage, int pageSize, int id)
        {
            Change deleteChange = new Change();
            deleteChange = await _context.ChangeLog.AsNoTracking().FirstOrDefaultAsync(nf => nf.Id == id);
            _context.ChangeLog.Remove(deleteChange);
            await _context.SaveChangesAsync();

            var newChange = from nf in _context.ChangeLog.AsNoTracking()
                              select nf;

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = newChange.Count();

            PageSize = pageSize == 0 ? 5 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            ChangeLog = await newChange
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
            TempData["StatusMessage"] = "Change " + deleteChange.ChangeName + " deleted.";
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateFeature(int currentPage, int pageSize)
        {
            Change uNewChange = new Change();
            uNewChange = await _context.ChangeLog.AsNoTracking().FirstOrDefaultAsync(nf => nf.Id == UpdateFeatureId);
            uNewChange.ChangeName = UpdateChangeName;
            uNewChange.Description = UpdateChangeDescription;
            uNewChange.PercentageCompleted = Convert.ToInt32(UpdateChangeCompletion);
            _context.ChangeLog.Update(uNewChange);
            await _context.SaveChangesAsync();
            var newFeatures = from nf in _context.ChangeLog.AsNoTracking()
                              select nf;

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = newFeatures.Count();

            PageSize = pageSize == 0 ? 5 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            ChangeLog = await newFeatures
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
            TempData["StatusMessage"] = "Feature " + uNewChange.ChangeName + " updated.";
            return Page();
        }
    }
}