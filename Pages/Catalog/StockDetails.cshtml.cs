using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.Catalog
{
    [Authorize(Roles = "Admin, User")]
    public class StockDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StockDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public StockItem StockItem { get; set; }
        public Store Store { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StockItem = await _context.Stock.FirstOrDefaultAsync(m => m.Id == id);
            Store = await _context.Stores.FirstOrDefaultAsync(s => s.Store_ID == StockItem.StoreId);

            if (StockItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}