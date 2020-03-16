using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace JRPC_HMS
{
    public class RoleCreateModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleCreateModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public IdentityRole Role { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _roleManager.CreateAsync(Role);

            return RedirectToPage("./Roles");
        }
    }
}