using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JRPC_HMS.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        [BindProperty]
        public string ErrorDetails { get; set; }

        public async Task OnGetAsync(int? statusCode = null)
        {
            if (statusCode == null)
                statusCode = HttpContext.Response.StatusCode;

            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            string uaString = HttpContext.Request.Headers["User-Agent"].ToString();

            var uid = "Unknown";
            if (User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            StringBuilder sb = new StringBuilder($"An error has occurred on {HttpContext.Request.Host}. \r\n \r\n");
            sb.Append($"RequestId = {RequestId} \r\nStatusCode = {statusCode.ToString()} \r\n");

            sb.Append($"OriginalPath = {feature?.OriginalPath} \r\n \r\n");

            sb.Append($"Path = {Request.Path}. \r\n \r\n");

            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exception != null)
            {
                sb.Append($"Error Message = {exception.Error.Message}. \r\n");
                sb.Append($"Error Source = {exception.Error.Source}. \r\n");

                if (exception.Error.InnerException != null)
                    sb.Append($"Inner Exception = {exception.Error.InnerException.ToString()}. \r\n");
                else
                    sb.Append("Inner Exception = null. \r\n");

                sb.Append($"Error StackTrace = {exception.Error.StackTrace}. \r\n");
            }
            ErrorDetails = sb.ToString();
        }
    }
}
