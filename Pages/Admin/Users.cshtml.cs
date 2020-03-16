using JRPC_HMS.Data;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
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

namespace JRPC_HMS
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HostingEnvironment _hostingEnvironment;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, HostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _roleManager = roleManager;
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

        public IList<ApplicationUser> Users { get; set; }
        public IList<IdentityRole> Roles { get; set; }
        public UserManager<ApplicationUser> UserManager { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<string> DeleteUsers { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<ApplicationUser>> InitiateView(int currentPage, int pageSize, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var users = from u in _userManager.Users
                             select u;

            if (!string.IsNullOrEmpty(SearchString))
            {
                users = users.Where(s => s.UserName.Contains(SearchString));
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = users.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            Roles = await _roleManager.Roles.ToListAsync();
            UserManager = _userManager;

            return await users
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string searchString)
        {
            Users = await InitiateView(currentPage, pageSize, searchString);
        }

        public async Task OnPost(int currentPage, int pageSize, string searchString)
        {
            if (DeleteUsers != null || DeleteUsers.Count != 0)
            {
                foreach (var userId in DeleteUsers)
                {
                    ApplicationUser user = new ApplicationUser();
                    user = await _userManager.FindByIdAsync(userId);
                    await _userManager.DeleteAsync(user);
                }
            }

            Users = await InitiateView(currentPage, pageSize, searchString);
        }

        public async Task<IActionResult> OnPostExportAsync(int currentPage, int pageSize, string searchString)
        {
            Users = await InitiateView(currentPage, pageSize, searchString);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Users Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Users");
                ExcelWorksheet usersSheet = package.Workbook.Worksheets["Users"];
                using (var range = usersSheet.Cells["A1:C1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                usersSheet.Cells[1, 1].Value = "Id";
                usersSheet.Cells[1, 2].Value = "Username";
                usersSheet.Cells[1, 3].Value = "Email";
                usersSheet.Cells[1, 4].Value = "Phone Number";
                usersSheet.Cells[1, 1, 1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                for (int r = 0; r < Users.Count(); r++)
                {
                    usersSheet.Cells[r + 2, 1].Value = Users.ToList()[r].Id;
                    usersSheet.Cells[r + 2, 2].Value = Users.ToList()[r].UserName;
                    usersSheet.Cells[r + 2, 3].Value = Users.ToList()[r].Email;
                    usersSheet.Cells[r + 2, 4].Value = Users.ToList()[r].PhoneNumber;
                    usersSheet.Cells[1, 1, 1, 4].AutoFitColumns();
                }

                package.SaveAs(fs);
                package.Dispose();
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
    }
}