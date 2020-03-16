using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace JRPC_HMS
{
    public class RolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HostingEnvironment _hostingEnvironment;

        public RolesModel(RoleManager<IdentityRole> roleManager, HostingEnvironment hostingEnvironment)
        {
            _roleManager = roleManager;
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

        public IList<IdentityRole> Roles { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<string> DeleteRoles { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<IdentityRole>> InitiateView(int currentPage, int pageSize, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var roles = from u in _roleManager.Roles
                        select u;

            if (!string.IsNullOrEmpty(SearchString))
            {
                roles = roles.Where(s => s.Name.Contains(SearchString));
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = roles.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;

            return await roles
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGetAsync(int currentPage, int pageSize, string searchString)
        {
            Roles = await InitiateView(currentPage, pageSize, searchString);
        }

        public async Task OnPost(int currentPage, int pageSize, string searchString)
        {
            if (DeleteRoles != null || DeleteRoles.Count != 0)
            {
                foreach (var id in DeleteRoles)
                {
                    await _roleManager.DeleteAsync(_roleManager.Roles.FirstOrDefault(u => u.Id == id));
                }
            }

            Roles = await InitiateView(currentPage, pageSize, searchString);
        }

        public async Task<IActionResult> OnPostExportAsync(int currentPage, int pageSize, string searchString)
        {
            Roles = await InitiateView(currentPage, pageSize, searchString);

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"Roles Data.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                ExcelPackage package = new ExcelPackage();
                package.Workbook.Worksheets.Add("Roles");
                ExcelWorksheet usersSheet = package.Workbook.Worksheets["Roles"];
                using (var range = usersSheet.Cells["A1:B1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 121, 186));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                usersSheet.Cells[1, 1].Value = "Id";
                usersSheet.Cells[1, 2].Value = "Name";
                usersSheet.Cells[1, 1, 1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
                for (int r = 0; r < Roles.Count(); r++)
                {
                    usersSheet.Cells[r + 2, 1].Value = Roles.ToList()[r].Id;
                    usersSheet.Cells[r + 2, 2].Value = Roles.ToList()[r].Name;
                    usersSheet.Cells[1, 1, 1, 2].AutoFitColumns();
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