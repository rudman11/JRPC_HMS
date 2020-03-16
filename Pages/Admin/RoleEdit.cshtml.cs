using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS
{
    public class RoleEditModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleEditModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public IdentityRole Role { get; set; }

        public void OnGet(string id)
        {
            Role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _roleManager.CreateAsync(Role);

            return RedirectToPage("./Roles");
        }
    }
}