using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Data;
using JRPC_HMS.Pages.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace JRPC_HMS
{
    public class UserCreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UserCreateModel(UserManager<ApplicationUser> userManager, ILogger<RegisterModel> logger, RoleManager<IdentityRole> roleManager, IHostingEnvironment environment)
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
        [BindProperty]
        public IFormFile UploadImage { set; get; }

        public class InputModel
        {
            [Display(Name = "Profile Picture")]
            public string ProfilePicture { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            public string Phone { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Roles")]
            public IList<string> Roles { get; set; }
        }

        public IActionResult OnGet()
        {
            AllRoles = _roleManager.Roles.Select(n => new SelectListItem
            {
                Value = n.Id,
                Text = n.Name
            }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadImage != null)
            {
                var fileName = Path.Combine(_hostingEnvironment.WebRootPath, "img", UploadImage.FileName);
                using (var fileStream = new FileStream(fileName, FileMode.Create))
                {
                    await UploadImage.CopyToAsync(fileStream);
                }
                Input.ProfilePicture = "\\img\\" + UploadImage.FileName; // Set the file name
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { FirstName = Input.FirstName, LastName = Input.LastName, UserName = Input.UserName, Email = Input.Email, PhoneNumber = Input.Phone, ProfilePicture = Input.ProfilePicture, LastLoginDate = DateTime.Now };
                IdentityResult result = null;
                try
                {
                    result = await _userManager.CreateAsync(user, Input.Password);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message, "Add User");
                }
                var roles = await _userManager.GetRolesAsync(user);
                if (Input.Roles != roles)
                {
                    foreach (var role in Input.Roles)
                    {
                        var roleExists = await _roleManager.FindByIdAsync(role);
                        if (await _roleManager.RoleExistsAsync(roleExists.Name))
                        {
                            try
                            {
                                await _userManager.AddToRoleAsync(user, roleExists.Name);
                            }
                            catch(Exception ex)
                            {
                                _logger.LogError(ex.Message, "Add User to Role");
                            }
                        }
                    }
                }
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _logger.LogError(error.Description);
                }
            }
            return RedirectToPage("./Users");
        }
    }
}