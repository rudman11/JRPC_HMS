using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JRPC_HMS.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToPage("/Settings/Profile");
        }

        public IActionResult TwoFactorAuth()
        {
            return RedirectToPage("/Settings/TwoFactorAuth/Config");
        }
    }
}