using System;
using System.Collections.Generic;
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

namespace JRPC_HMS
{
    [Authorize(Roles = "Admin, User")]
    public class RequisitionIndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly HostingEnvironment _hostingEnvironment;

        public RequisitionIndexModel(ApplicationDbContext context, HostingEnvironment hostingEnvironment)
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

        public IList<Requisition> Requisitions { get; set; }
        public IList<Supplier> Suppliers { get; set; }
        public IList<Warehouse> Stock { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ReqNoSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ReqDateSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SupplierSort { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string CurrentSort { get; set; }
        [BindProperty]
        public List<int> DeleteReq { get; set; }
        #endregion

        #region InitiateView
        public async Task<IList<Requisition>> InitiateView(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            CurrentSort = sortOrder;
            ReqNoSort = String.IsNullOrEmpty(sortOrder) ? "reqno_desc" : "";
            ReqDateSort = String.IsNullOrEmpty(sortOrder) ? "reqdate_desc" : "";
            SupplierSort = String.IsNullOrEmpty(sortOrder) ? "supplier_desc" : "";

            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
            }

            var requisitions = from s in _context.Requisitions
                                 select s;
            if (!string.IsNullOrEmpty(SearchString))
            {
                requisitions = requisitions.Where(s =>
                    s.ReqNo.Contains(SearchString) ||
                    s.SupplierId == _context.Suppliers.FirstOrDefault(st => st.Name.Contains(SearchString)).Id);
            }
            switch (sortOrder)
            {
                case "reqno_desc":
                    requisitions = requisitions.OrderByDescending(s => s.ReqNo);
                    break;
                case "reqdate_desc":
                    requisitions = requisitions.OrderByDescending(s => s.ReqDate);
                    break;
                case "supplier_desc":
                    requisitions = requisitions.OrderByDescending(s => s.SupplierId);
                    break;
                default:
                    requisitions = requisitions.OrderBy(s => s.ReqNo);
                    break;
            }

            CurrentPage = currentPage == 0 ? 1 : currentPage;

            Count = requisitions.Count();

            PageSize = pageSize == 0 ? 10 : pageSize;

            if (CurrentPage > TotalPages)
                CurrentPage = TotalPages;
            Suppliers = _context.Suppliers.ToList();
            Stock = _context.WarehouseStock.ToList();
            return await requisitions
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
        #endregion

        public async Task OnGet(int currentPage, int pageSize, string searchString, string sortOrder)
        {
            Requisitions = await InitiateView(currentPage, pageSize, searchString, sortOrder);
        }
    }
}