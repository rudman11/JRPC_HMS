using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.Catalog
{
    [Authorize(Roles = "Admin, User")]
    public class StockCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StockCreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SelectListItem> Stores { get; set; }

        public IActionResult OnGet()
        {
            Stores = _context.Stores.Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();
            return Page();
        }

        [BindProperty]
        public StockItem Stock { get; set; }
        [BindProperty]
        public string StoreId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Stores = _context.Stores.Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();
            Stock.StoreId = Convert.ToInt32(StoreId);
            Stock.TransferApprovals = "False";
            _context.Stock.Add(Stock);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "Stock " + Stock.StockName + " successfully created into store " + _context.Stores.FirstOrDefault(s => s.Store_ID == Convert.ToInt32(StoreId)).StoreName + ".";
            return RedirectToPage("./StockIndex");
        }
    }
}