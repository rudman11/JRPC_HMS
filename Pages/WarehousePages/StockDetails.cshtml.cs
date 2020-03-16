using JRPC_HMS.Data;
using JRPC_HMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JRPC_HMS.Pages.WarehousePages
{
    [Authorize(Roles = "Admin, User")]
    public class StockDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StockDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Warehouse Warehouse { get; set; }
        public Supplier Supplier { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Warehouse = await _context.WarehouseStock.FirstOrDefaultAsync(m => m.Id == id);
            Supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == Warehouse.SupplierId);

            if (Warehouse == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}