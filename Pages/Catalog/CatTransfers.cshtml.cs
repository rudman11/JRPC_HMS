using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JRPC_HMS.Data;
using JRPC_HMS.Models;
using JRPC_HMS.Services.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace JRPC_HMS
{
    [Authorize(Roles = "Admin, User")]
    public class CatTransfersModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CatTransfersModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public CatTransfersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<CatTransfersModel> logger, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public Warehouse Warehouse { get; set; }
        [BindProperty]
        public StockItem FromStockItem { get; set; }
        [BindProperty]
        public StockItem ToStockItem { get; set; }
        public IList<StockTransfer> StockTransfers { get; set; }
        [BindProperty]
        public StockTransfer StockTransfer { get; set; }
        public Supplier Supplier { get; set; }
        public IList<SelectListItem> Stores { get; set; }
        public IList<Store> AllStores { get; set; }
        [BindProperty]
        public string StoreId { get; set; }
        [BindProperty]
        public decimal AmountToTransfer { get; set; }
        [BindProperty(SupportsGet =true)]
        public string AppStoreId { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal AppAmountToTransfer { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Message { get; set; }
        [BindProperty(SupportsGet = true)]
        public string AppMessage { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Store FromStore { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }
            FromStockItem = await _context.Stock.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            Stores = _context.Stores.AsNoTracking().Where(s => s.Store_ID != FromStockItem.StoreId).Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();
            AllStores = _context.Stores.AsNoTracking().ToList();

            if (await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
            {
                StockTransfers = _context.StockTransfers.AsNoTracking().Where(t => t.StockId == id && t.Approvals == "True").ToList();
            }
            AppMessage = "";
            Message = "";

            return Page();
        }

        public async Task<IActionResult> OnPostApprovalsAsync(int itemId)
        {
            StockTransfer = _context.StockTransfers.FirstOrDefault(st => st.Id == itemId);
            FromStockItem = await _context.Stock.AsNoTracking().FirstOrDefaultAsync(s => s.Id == StockTransfer.StockId);
            Stores = _context.Stores.AsNoTracking().Where(s => s.Store_ID != FromStockItem.StoreId).Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
            {
                StockTransfers = _context.StockTransfers.AsNoTracking().Where(t => t.StockId == FromStockItem.Id && t.Approvals == "True").ToList();
            }
            AllStores = _context.Stores.AsNoTracking().ToList();

            StockTransfer.Approver = ApplicationUser.UserName;
            StockTransfer.ApprovalStatus = "Approved";
            StockTransfer.Approvals = "False";
            StockTransfer.DateApproved = DateTime.Now;
            await DoTransfers(StockTransfer);
            AppMessage = "Approved";
            Message = "";

            return Page();
        }

        public async Task<IActionResult> OnPostDeclinesAsync(int itemId)
        {
            FromStockItem = await _context.Stock.AsNoTracking().FirstOrDefaultAsync(s => s.Id == itemId);
            Stores = _context.Stores.AsNoTracking().Where(s => s.Store_ID != FromStockItem.StoreId).Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
            {
                StockTransfers = _context.StockTransfers.AsNoTracking().Where(t => t.StockId == FromStockItem.Id && t.Approvals == "True").ToList();
            }
            AllStores = _context.Stores.AsNoTracking().ToList();
            
            StockTransfer stockTransfer = new StockTransfer
            {
                StockId = FromStockItem.Id,
                FromStoreId = FromStockItem.StoreId,
                ToStoreId = Convert.ToInt32(StoreId),
                TransferDate = DateTime.Now,
                Quantity = AmountToTransfer,
                Approvals = "False",
                ApprovalStatus = "Declined",
                Approver = ApplicationUser.UserName,
                DateApproved = DateTime.Now,
                Requester = ApplicationUser.UserName
            };
            FromStockItem.TransferApprovals = "False";
            _context.StockTransfers.Add(StockTransfer);
            _context.Stock.Update(FromStockItem);
            try
            {
                _context.SaveChanges();
                AppMessage = "Declined";
                Message = "";
                List<InternetAddress> toEmailAddresses = new List<InternetAddress>();
                List<InternetAddress> fromEmailAddresses = new List<InternetAddress>();
                MailboxAddress toAddress = new MailboxAddress("roedolf.bothma.123@gmail.com");
                toEmailAddresses.Add(toAddress);
                MailboxAddress fromAddress = new MailboxAddress("rudman11@gmail.com");
                fromEmailAddresses.Add(fromAddress);
                EmailMessage emailMessage = new EmailMessage()
                {
                    ToAddresses = toEmailAddresses,
                    FromAddresses = fromEmailAddresses,
                    Subject = "Stock Approved.",
                    Content = string.Format("Stock {0} was declined on {1}", FromStockItem.StockName, stockTransfer.DateApproved)
                };
                _emailSender.SendMail(emailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Stock Not Saved");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            FromStockItem = await _context.Stock.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            Stores = _context.Stores.AsNoTracking().Where(s => s.Store_ID != FromStockItem.StoreId).Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
            {
                StockTransfers = _context.StockTransfers.AsNoTracking().Where(t => t.StockId == id && t.Approvals == "True").ToList();
            }
            AllStores = _context.Stores.AsNoTracking().ToList();
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
            {
                StockTransfer stockTransfer = new StockTransfer
                {
                    StockId = FromStockItem.Id,
                    FromStoreId = FromStockItem.StoreId,
                    ToStoreId = Convert.ToInt32(StoreId),
                    TransferDate = DateTime.Now,
                    Quantity = AmountToTransfer,
                    DateApproved = DateTime.Now,
                    Approvals = "True",
                    ApprovalStatus = "Waiting Approval",
                    Requester = ApplicationUser.UserName
                };
                FromStockItem.TransferApprovals = "True";
                _context.StockTransfers.Add(stockTransfer);
                _context.Stock.Update(FromStockItem);
                try
                {
                    _context.SaveChanges();

                    List<InternetAddress> toEmailAddresses = new List<InternetAddress>();
                    List<InternetAddress> fromEmailAddresses = new List<InternetAddress>();
                    MailboxAddress toAddress = new MailboxAddress("roedolf.bothma.123@gmail.com");
                    toEmailAddresses.Add(toAddress);
                    MailboxAddress fromAddress = new MailboxAddress("rudman11@gmail.com");
                    fromEmailAddresses.Add(fromAddress);
                    EmailMessage emailMessage = new EmailMessage()
                    {
                        ToAddresses = toEmailAddresses,
                        FromAddresses = fromEmailAddresses,
                        Subject = "Stock Approval Waiting.",
                        Content = string.Format("Stock {0} is waiting approval since {1}", FromStockItem.StockName, stockTransfer.TransferDate)
                    };
                    _emailSender.SendMail(emailMessage);
                    Message = "Waiting Approval";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                AppMessage = "";
                Message = "Waiting Approvals";
            }
            else
            {
                StockTransfer stockTransfer = new StockTransfer
                {
                    StockId = FromStockItem.Id,
                    FromStoreId = FromStockItem.StoreId,
                    ToStoreId = Convert.ToInt32(StoreId),
                    TransferDate = DateTime.Now,
                    Quantity = AmountToTransfer,
                    DateApproved = DateTime.Now,
                    Approvals = "True",
                    ApprovalStatus = "Approved",
                    Requester = ApplicationUser.UserName,
                    Approver = ApplicationUser.UserName
                };
                await DoTransfers(stockTransfer);
                AppMessage = "";
                Message = "Approved";
            }

            return Page();
        }

        #region DoTransfers
        public async Task DoTransfers(StockTransfer stockTransfer)
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                Stores = context.Stores.AsNoTracking().Where(s => s.StoreName != "Warehouse").Select(n => new SelectListItem
                {
                    Value = n.Store_ID.ToString(),
                    Text = n.StoreName
                }).ToList();
                FromStockItem = await context.Stock.AsNoTracking().FirstOrDefaultAsync(s => s.Id == stockTransfer.StockId && s.StoreId == stockTransfer.FromStoreId);
                AllStores = context.Stores.AsNoTracking().ToList();
                ApplicationUser = await _userManager.GetUserAsync(User);

                StockItem isStockSore = new StockItem();
                isStockSore = context.Stock.AsNoTracking().FirstOrDefault(s => s.StoreId == stockTransfer.ToStoreId && s.StockName == FromStockItem.StockName);
                if (isStockSore != null)
                {
                    StockItem toStockItem = new StockItem
                    {
                        Id = isStockSore.Id,
                        StockName = isStockSore.StockName,
                        Unit = isStockSore.Unit,
                        Price = isStockSore.Price,
                        InStock = isStockSore.InStock + stockTransfer.Quantity,
                        StoreId = isStockSore.StoreId,
                        TransferApprovals = "False"
                    };
                    context.Stock.Update(toStockItem);
                }
                else
                {
                    StockItem toStockItem = new StockItem()
                    {
                        StockName = FromStockItem.StockName,
                        Unit = FromStockItem.Unit,
                        Price = FromStockItem.Price,
                        InStock = stockTransfer.Quantity,
                        StoreId = stockTransfer.ToStoreId,
                        TransferApprovals = "False"
                    };
                    context.Stock.Add(toStockItem);
                }

                FromStockItem.TransferApprovals = "False";
                FromStockItem.InStock = FromStockItem.InStock - AmountToTransfer;

                context.StockTransfers.Update(stockTransfer);
                context.Stock.Update(FromStockItem);

                try
                {
                    context.SaveChanges();
                    List<InternetAddress> toEmailAddresses = new List<InternetAddress>();
                    List<InternetAddress> fromEmailAddresses = new List<InternetAddress>();
                    MailboxAddress toAddress = new MailboxAddress("roedolf.bothma.123@gmail.com");
                    toEmailAddresses.Add(toAddress);
                    MailboxAddress fromAddress = new MailboxAddress("rudman11@gmail.com");
                    fromEmailAddresses.Add(fromAddress);
                    EmailMessage emailMessage = new EmailMessage()
                    {
                        ToAddresses = toEmailAddresses,
                        FromAddresses = fromEmailAddresses,
                        Subject = "Stock Approved.",
                        Content = string.Format("Stock {0} was approved on {1}", FromStockItem.StockName, stockTransfer.DateApproved)
                    };
                    _emailSender.SendMail(emailMessage);
                }
                catch (Exception ex)
                {

                }
            }
        }
        #endregion
    }
}