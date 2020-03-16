using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JRPC_HMS.Pages.Catalog
{
    [Authorize(Roles = "Admin, User")]
    public class StockEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StockEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StockItem StockItem { get; set; }
        public IList<SelectListItem> Stores { get; set; }
        [BindProperty]
        public string StoreId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StockItem = await _context.Stock.FirstOrDefaultAsync(m => m.Id == id);
            Stores = _context.Stores.Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();

            StoreId = _context.Stores.FirstOrDefault(s => s.Store_ID == StockItem.StoreId).Store_ID.ToString();

            if (StockItem == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            StockItem.StoreId = Convert.ToInt32(StoreId);
            StockItem.TransferApprovals = "False";
            _context.Stock.Update(StockItem);
            await _context.SaveChangesAsync();

            Stores = _context.Stores.Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();
            TempData["StatusMessage"] = "Stock " + StockItem.StockName + " successfully edited.";
            return RedirectToPage("./StockIndex");
        }

        private bool StockItemExists(int id)
        {
            return _context.Stock.Any(e => e.Id == id);
        }
    }
}