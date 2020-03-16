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

namespace JRPC_HMS.Pages.WarehousePages
{
    [Authorize(Roles = "Admin, User")]
    public class StockEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StockEditModel(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Warehouse Warehouse { get; set; }
        public IList<Supplier> AllSuppliers { get; set; }
        public List<SelectListItem> Suppliers { get; set; }
        [BindProperty]
        public string SelectedTag { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Warehouse = await _context.WarehouseStock.FirstOrDefaultAsync(m => m.Id == id);
            AllSuppliers = _context.Suppliers.ToList();
            Suppliers = _context.Suppliers.Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList();

            SelectedTag = AllSuppliers.FirstOrDefault(s => s.Id == Warehouse.SupplierId).Id.ToString();

            if (Warehouse == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Warehouse.SupplierId = Convert.ToInt32(SelectedTag);
            Warehouse.TransferApprovals = "False";

            _context.WarehouseStock.Update(Warehouse);
            _context.SaveChanges();

            AllSuppliers = _context.Suppliers.ToList();
            Suppliers = _context.Suppliers.Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList();

            TempData["StatusMessage"] = "Warehouse stock \"" + Warehouse.StockName + "\" successfully edited.";

            return RedirectToPage("./StockIndex");
        }

        private bool WarehouseExists(int id)
        {
            return _context.WarehouseStock.Any(e => e.Id == id);
        }
    }
}