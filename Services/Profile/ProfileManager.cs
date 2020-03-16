using JRPC_HMS.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JRPC_HMS.Services.Profile
{
    public class ProfileManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        IHttpContextAccessor _httpContextAccessor;

        private ApplicationUser _currentUser;

        public ProfileManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User).Result;

                return _currentUser;
            }
        }

        public bool IsHasPassword(ApplicationUser user)
        {
            return _userManager.HasPasswordAsync(user).Result;
        }

        public bool IsEmailConfirmed(ApplicationUser user)
        {
            return _userManager.IsEmailConfirmedAsync(user).Result;
        }

        public bool IsHasReadWhatsNew(ClaimsPrincipal user)
        {
            ApplicationUser ur = _userManager.Users.FirstOrDefault(u => u.UserName == user.Identity.Name);
            return ur.ReadWhatsNew == "Yes" ? true : false;
        }
    }
}
