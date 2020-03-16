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

namespace JRPC_HMS.Pages.WarehousePages
{
    [Authorize(Roles = "Admin, User")]
    public class StockCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public StockCreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Warehouse Warehouse { get; set; }
        public List<SelectListItem> Suppliers { get; set; }
        [BindProperty]
        public string SelectedTag { get; set; }

        public IActionResult OnGet()
        {
            Suppliers = _context.Suppliers.Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Warehouse.SupplierId = Convert.ToInt32(SelectedTag);
            Warehouse.TransferApprovals = "False";

            _context.WarehouseStock.Add(Warehouse);
            await _context.SaveChangesAsync();

            Suppliers = _context.Suppliers.Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList();

            TempData["StatusMessage"] = "Warehouse stock successfully created.";

            return RedirectToPage("./StockIndex");
        }
    }
}