using JRPC_HMS.Data;
using JRPC_HMS.Pages.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace JRPC_HMS
{
    public class UserEditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UserEditModel(UserManager<ApplicationUser> userManager, ILogger<RegisterModel> logger, RoleManager<IdentityRole> roleManager, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _hostingEnvironment = environment;
        }

        [BindProperty]
        public IList<SelectListItem> AllRoles { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public ApplicationUser User { get; set; }
        [BindProperty]
        public IFormFile UploadImage { set; get; }

        public class InputModel
        {
            public string Id { get; set; }
            [Display(Name = "Profile Picture")]
            public string ProfilePicture { get; set; }
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [Display(Name = "Username")]
            public string UserName { get; set; }
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Phone]
            [Display(Name = "Phone Number")]
            public string Phone { get; set; }
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Display(Name = "Roles")]
            public IList<string> Roles { get; set; }
        }

        public async Task<IActionResult> OnGet(string id)
        {
            AllRoles = _roleManager.Roles.Select(n => new SelectListItem
            {
                Value = n.Id,
                Text = n.Name
            }).ToList();
            User = _userManager.Users.FirstOrDefault(u => u.Id == id);
            InputModel input = new InputModel();
            input.Id = User.Id;
            input.ProfilePicture = User.ProfilePicture;
            input.FirstName = User.FirstName;
            input.LastName = User.LastName;
            input.UserName = User.UserName;
            input.Email = User.Email;
            input.Phone = User.PhoneNumber;
            input.Password = User.PasswordHash;
            input.ConfirmPassword = User.PasswordHash;
            input.Roles = await _userManager.GetRolesAsync(User);
            Input = input;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);

                if (UploadImage != null)
                {
                    var fileName = Path.Combine(_hostingEnvironment.WebRootPath, "img", UploadImage.FileName);
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        await UploadImage.CopyToAsync(fileStream);
                    }
                    user.ProfilePicture = "\\img\\" + UploadImage.FileName; // Set the file name
                }

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.UserName = Input.UserName;
                user.Email = Input.Email;
                user.PhoneNumber = Input.Phone;
                if (Input.Password != "")
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, Input.Password);
                }
                var result = await _userManager.UpdateAsync(user);
                var roles = await _userManager.GetRolesAsync(user);
                if (Input.Roles != roles)
                {
                    foreach (var role in Input.Roles)
                    {
                        var roleExists = await _roleManager.FindByIdAsync(role);
                        if (await _roleManager.RoleExistsAsync(roleExists.Name))
                        {
                            await _userManager.AddToRoleAsync(user, roleExists.Name);
                        }
                    }
                }
                if (result.Succeeded)
                {
                    _logger.LogInformation("User updated.");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToPage("./Users");
        }
    }
}