using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JRPC_HMS.Data;
using JRPC_HMS.Services;
using JRPC_HMS.Services.Mail;
using MimeKit;
using JRPC_HMS.Models;

namespace JRPC_HMS.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                List<InternetAddress> toEmailAddresses = new List<InternetAddress>();
                List<InternetAddress> fromEmailAddresses = new List<InternetAddress>();
                MailboxAddress toAddress = new MailboxAddress(Input.Email);
                toEmailAddresses.Add(toAddress);
                MailboxAddress fromAddress = new MailboxAddress("rudman11@gmail.com");
                fromEmailAddresses.Add(fromAddress);

                EmailMessage emailMessage = new EmailMessage()
                {
                    ToAddresses = toEmailAddresses,
                    FromAddresses = fromEmailAddresses,
                    Subject = "Confirmation Email",
                    Content = callbackUrl
                };

                _emailSender.SendMail(emailMessage);
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
