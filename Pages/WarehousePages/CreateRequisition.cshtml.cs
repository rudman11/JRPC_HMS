using DinkToPdf;
using DinkToPdf.Contracts;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using JRPC_HMS.Services.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRPC_HMS
{
    [Authorize(Roles = "Admin, User")]
    public class CreateRequisitionModel : PageModel
    {
        private readonly IConverter _converter;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<CreateRequisitionModel> _logger;

        public CreateRequisitionModel(IConverter converter, ApplicationDbContext context, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, IEmailSender emailSender, ILogger<CreateRequisitionModel> logger)
        {
            _converter = converter;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        public IList<Requisition> Requisitions { get; set; }
        public IList<Supplier> AllSuppliers { get; set; }
        public List<SelectListItem> Suppliers { get; set; }
        public IList<Warehouse> AllWarehouseStock { get; set; }
        public List<SelectListItem> WarehouseStock { get; set; }
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public string SelectedSupplier { get; set; }
        [BindProperty]
        public string SelectedStock { get; set; }
        public decimal Total { get; set; }
        [BindProperty]
        public IList<RequisitionModel> RequisitionModels { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [BindProperty]
        public string ReqNo { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Suppliers = _context.Suppliers.AsNoTracking().Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList();
            WarehouseStock = _context.WarehouseStock.AsNoTracking().Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.StockName
            }).ToList();
            AllSuppliers = _context.Suppliers.AsNoTracking().ToList();
            AllWarehouseStock = _context.WarehouseStock.AsNoTracking().ToList();
            ApplicationUser = await _userManager.GetUserAsync(User);            
            ReqNo = GenerateID();
            Requisitions = _context.Requisitions.AsNoTracking().Where(r => r.ReqNo == ReqNo).ToList();
            return Page();
        }

        public string GenerateID()
        {
            List<Requisition> requisitions = new List<Requisition>();
            requisitions = _context.Requisitions.AsNoTracking().ToList();

            string id = string.Empty;
            int count = 0;
            if (requisitions.Count > 0)
            {
                id = requisitions.Last().ReqNo;
                id = id.Remove(0, 5);
                count = Convert.ToInt32(id);
            }
            else
            {
                count = 0;
            }

            count++;
            id = string.Format("ReqNo{0}", count.ToString("0000000"));
            return id;
        }

        public async Task<IActionResult> OnPost()
        {
            Suppliers = _context.Suppliers.AsNoTracking().Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList();
            WarehouseStock = _context.WarehouseStock.AsNoTracking().Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.StockName
            }).ToList();
            AllSuppliers = _context.Suppliers.AsNoTracking().ToList();
            AllWarehouseStock = _context.WarehouseStock.AsNoTracking().ToList();
            ApplicationUser = await _userManager.GetUserAsync(User);
            Requisitions = _context.Requisitions.AsNoTracking().Where(r => r.ReqNo == ReqNo).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAddItems()
        {
            Suppliers = _context.Suppliers.AsNoTracking().Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList();
            WarehouseStock = _context.WarehouseStock.AsNoTracking().Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.StockName
            }).ToList();
            AllSuppliers = _context.Suppliers.AsNoTracking().ToList();
            AllWarehouseStock = _context.WarehouseStock.AsNoTracking().ToList();
            Warehouse warehouseStock = new Warehouse();
            warehouseStock = _context.WarehouseStock.AsNoTracking().AsNoTracking().FirstOrDefault(s => s.Id == Convert.ToInt32(SelectedStock));
            
            ApplicationUser = await _userManager.GetUserAsync(User);
            Requisition requisition = new Requisition
            {
                StockId = warehouseStock.Id,
                Quantity = Quantity,
                Price = warehouseStock.Price,
                Total = warehouseStock.Price * Quantity,
                Requester = ApplicationUser.UserName,
                ReqDate = DateTime.Now,
                SupplierId = Convert.ToInt32(SelectedSupplier),
                ReqNo = ReqNo,
                Approved = "Waiting Approval",
                ApprovedDate = DateTime.Now
            };
            await _context.Requisitions.AddAsync(requisition);

            await _context.SaveChangesAsync();
            Requisitions = _context.Requisitions.AsNoTracking().Where(r => r.ReqNo == ReqNo).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateReq()
        {
            Suppliers = _context.Suppliers.AsNoTracking().Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.Name
            }).ToList();
            WarehouseStock = _context.WarehouseStock.AsNoTracking().Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = n.StockName
            }).ToList();
            AllSuppliers = _context.Suppliers.AsNoTracking().ToList();
            AllWarehouseStock = _context.WarehouseStock.AsNoTracking().ToList();
            ApplicationUser = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
            {
                try
                {
                    CreatePDF(AllSuppliers.FirstOrDefault(s => s.Id == Convert.ToInt32(SelectedSupplier)).Email, ReqNo);
                }
                catch(Exception ex)
                {
                    _logger.LogError("Error in creating PDF");
                }
                Requisitions = _context.Requisitions.AsNoTracking().Where(r => r.ReqNo == ReqNo).ToList();
                foreach (var req in Requisitions)
                {
                    req.Approved = "Approved";
                    req.ApprovedDate = DateTime.Now;
                    req.Approver = ApplicationUser.UserName;

                    _context.Requisitions.Update(req);
                }
                _context.SaveChanges();
                TempData["StatusMessage"] = "Requisition created, approved and sent to supplier.";
            }
            else
            {
                TempData["StatusMessage"] = "Requisition created and waiting approval.";
            }
            return Redirect("~/WarehousePages/RequisitionIndex");
        }

        public IActionResult OnPostCancelRequisition()
        {
            Requisitions = _context.Requisitions.AsNoTracking().Where(r => r.ReqNo == ReqNo).ToList();

            _context.Requisitions.RemoveRange(Requisitions);
            _context.SaveChanges();
            TempData["StatusMessage"] = "Requisition was cancelled.";
            return Redirect("~/WarehousePages/RequisitionIndex");
        }

        public void CreatePDF(string supplierEmail, string reqNo)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                Out = Path.Combine(_hostingEnvironment.WebRootPath, reqNo + ".pdf")
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = GetHTMLString(supplierEmail),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(_hostingEnvironment.WebRootPath, "/libs/bootstrap/css", "bootstrap.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            _converter.Convert(pdf);

            // Sending Mail
            List<InternetAddress> toEmailAddresses = new List<InternetAddress>();
            List<InternetAddress> fromEmailAddresses = new List<InternetAddress>();
            MailboxAddress toAddress = new MailboxAddress(supplierEmail);
            toEmailAddresses.Add(toAddress);
            MailboxAddress fromAddress = new MailboxAddress("rudman11@gmail.com");
            fromEmailAddresses.Add(fromAddress);
            EmailMessage emailMessage = new EmailMessage()
            {
                ToAddresses = toEmailAddresses,
                FromAddresses = fromEmailAddresses,
                Subject = "New Requisition",
                Content = string.Format(@"Good Day {0}.{1}Please find attached new Requisition.{2}{3}Best Regards,", 
                    AllSuppliers.FirstOrDefault(s => s.Email == supplierEmail).Name, 
                    Environment.NewLine, Environment.NewLine, Environment.NewLine),
                Attachments = Path.Combine(_hostingEnvironment.WebRootPath, reqNo + ".pdf")
            };
            _emailSender.SendMail(emailMessage);
        }

        public string GetHTMLString(string supplierEmail)
        {
            Requisitions = _context.Requisitions.AsNoTracking().ToList();
            var sb = new StringBuilder();
            sb.AppendFormat(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Requisition</h1></div>
                                <table class='table'>
                                    <tr>
                                        <td colspan='2' align='right'>{0}</td>
                                    </tr>            
                                    <tr>            
                                        <td>            
                                            <table class='table'>
                                                <tr>
                                                    <th>Requisitioner Info</th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <td>{1} {2}</td>
                                                </tr>
                                                <tr>
                                                    <th>Phone</th>
                                                    <td>{3}</td>
                                                </tr>
                                                <tr>
                                                    <th>Email</th>
                                                    <td>{4}</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table class='table'>
                                                <tr>
                                                    <th>Supplier Info</th>
                                                </tr>
                                                <tr>
                                                    <th>Name</th>
                                                    <td>{5}</td>
                                                </tr>
                                                <tr>
                                                    <th>Phone</th>
                                                    <td>{6}</td>
                                                </tr>
                                                <tr>
                                                    <th>Address</th>
                                                    <td>{7}</td>
                                                </tr>
                                                <tr>
                                                    <th>Email</th>
                                                    <td>{8}</td>
                                                </tr>
                                                <tr>
                                                    <th>Attention</th>
                                                    <td>{9}</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td colspan='2'>
                                            <table class='table'>
                                                <thead>
                                                    <tr>
                                                        <th>Stock Item</th>
                                                        <th>Quantity</th>
                                                        <th>Unit Price</th>
                                                        <th>Total</th>
                                                    </tr>
                                                </thead>
                                                <tbody>", ReqNo, ApplicationUser.FirstName, ApplicationUser.LastName, ApplicationUser.PhoneNumber, ApplicationUser.Email,
                        AllSuppliers.FirstOrDefault(s => s.Email == supplierEmail).Name, AllSuppliers.FirstOrDefault(s => s.Email == supplierEmail).Phone,
                        AllSuppliers.FirstOrDefault(s => s.Email == supplierEmail).Address, AllSuppliers.FirstOrDefault(s => s.Email == supplierEmail).Email, AllSuppliers.FirstOrDefault(s => s.Email == supplierEmail).Contact);

            foreach (var item in Requisitions)
            {
                sb.AppendFormat(@"
                    <tr>
                        <td>{0}</td>
                        <td>{1}</td>
                        <td>{2}</td>
                        <td>{3}</td>
                    </tr>", AllWarehouseStock.FirstOrDefault(w => w.Id == item.StockId).StockName, item.Quantity, item.Price, item.Total);
            }
            sb.Append(@"
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
    }

    public class RequisitionModel
    {
        public string Item { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}