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
    public class TransfersModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<TransfersModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransfersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<TransfersModel> logger, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public Warehouse Warehouse { get; set; }
        [BindProperty]
        public StockItem StockItem { get; set; }
        [BindProperty]
        public StockTransfer StockTransfer { get; set; }
        public IList<StockTransfer> StockTransfers { get; set; }
        public Supplier Supplier { get; set; }
        public IList<SelectListItem> Stores { get; set; }
        public IList<Store> AllStores { get; set; }
        [BindProperty]
        public string StoreId { get; set; }
        [BindProperty]
        public decimal AmountToTransfer { get; set; }
        [BindProperty(SupportsGet = true)]
        public string AppStoreId { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal AppAmountToTransfer { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Message { get; set; }
        [BindProperty(SupportsGet = true)]
        public string AppMessage { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Warehouse = await _context.WarehouseStock.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            Supplier = await _context.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == Warehouse.SupplierId);
            Stores = _context.Stores.AsNoTracking().Where(s => s.StoreName != "Warehouse").Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();
            AllStores = _context.Stores.AsNoTracking().ToList();
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
            {
                StockTransfers = _context.StockTransfers.AsNoTracking().Where(t => t.StockId == id && t.Approvals == "True" && t.FromStoreId == (AllStores.FirstOrDefault(s => s.StoreName == "Warehouse").Store_ID)).ToList();
            }
            AppMessage = "";
            Message = "";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Stores = _context.Stores.AsNoTracking().Where(s => s.StoreName != "Warehouse").Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();
            Store warehouseStore = await _context.Stores.AsNoTracking().FirstOrDefaultAsync(s => s.StoreName == "Warehouse");
            Warehouse = await _context.WarehouseStock.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            Supplier = await _context.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == Warehouse.SupplierId);
            AllStores = _context.Stores.AsNoTracking().ToList();
            ApplicationUser = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
            {
                StockTransfers = _context.StockTransfers.AsNoTracking().Where(t => t.StockId == id && t.Approvals == "True" && t.FromStoreId == (AllStores.FirstOrDefault(s => s.StoreName == "Warehouse").Store_ID)).ToList();
            }
            if (!await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
            {
                StockTransfer stockTransfer = new StockTransfer
                {
                    StockId = Warehouse.Id,
                    FromStoreId = warehouseStore.Store_ID,
                    ToStoreId = Convert.ToInt32(StoreId),
                    TransferDate = DateTime.Now,
                    Quantity = AmountToTransfer,
                    Requester = ApplicationUser.UserName,
                    DateApproved = DateTime.Now,
                    Approvals = "True",
                    ApprovalStatus = "Waiting Approval"
                };
                Warehouse.TransferApprovals = "True";
                _context.WarehouseStock.Update(Warehouse);
                _context.SaveChanges();
                AppMessage = "";
                Message = "Waiting Approvals";
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
                    Content = string.Format("Stock {0} is waiting approval since {1}", Warehouse.StockName, stockTransfer.TransferDate)
                };
                _emailSender.SendMail(emailMessage);
            }
            else
            {
                StockTransfer stockTransfer = new StockTransfer
                {
                    StockId = Warehouse.Id,
                    FromStoreId = warehouseStore.Store_ID,
                    ToStoreId = Convert.ToInt32(StoreId),
                    TransferDate = DateTime.Now,
                    Quantity = AmountToTransfer,
                    Requester = ApplicationUser.UserName,
                    Approver = ApplicationUser.UserName,
                    DateApproved = DateTime.Now,
                    Approvals = "False",
                    ApprovalStatus = "Approved"
                };
                await DoTransfers(stockTransfer);
                AppMessage = "";
                Message = "Approved";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostApprovalsAsync(int? id)
        {
            Stores = _context.Stores.AsNoTracking().Where(s => s.StoreName != "Warehouse").Select(n => new SelectListItem
            {
                Value = n.Store_ID.ToString(),
                Text = n.StoreName
            }).ToList();
            if (await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
            {
                StockTransfers = _context.StockTransfers.AsNoTracking().Where(t => t.StockId == id && t.Approvals == "True" && t.FromStoreId == (AllStores.FirstOrDefault(s => s.StoreName == "Warehouse").Store_ID)).ToList();
            }
            Store warehouseStore = await _context.Stores.AsNoTracking().FirstOrDefaultAsync(s => s.StoreName == "Warehouse");
            Supplier = await _context.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == Warehouse.SupplierId);
            AllStores = _context.Stores.AsNoTracking().ToList();
            ApplicationUser = await _userManager.GetUserAsync(User);
            StockTransfer = _context.StockTransfers.AsNoTracking().FirstOrDefault(st => st.Id == id);
            StockTransfer.Approver = ApplicationUser.UserName;
            StockTransfer.ApprovalStatus = "Approved";
            StockTransfer.Approvals = "False";
            StockTransfer.DateApproved = DateTime.Now;
            await DoTransfers(StockTransfer);
            AppMessage = "Approved";
            Message = "";
            return Page();
        }

        public async Task<IActionResult> OnPostDeclinesAsync(int? id)
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                Stores = context.Stores.AsNoTracking().Where(s => s.StoreName != "Warehouse").Select(n => new SelectListItem
                {
                    Value = n.Store_ID.ToString(),
                    Text = n.StoreName
                }).ToList();
                if (await _userManager.IsInRoleAsync(ApplicationUser, "Admin"))
                {
                    StockTransfers = context.StockTransfers.AsNoTracking().Where(t => t.StockId == id && t.Approvals == "True" && t.FromStoreId == (AllStores.FirstOrDefault(s => s.StoreName == "Warehouse").Store_ID)).ToList();
                }
                StockTransfer st = new StockTransfer();
                st = context.StockTransfers.AsNoTracking().FirstOrDefault(t => t.Id == id);
                Store warehouseStore = context.Stores.AsNoTracking().FirstOrDefault(s => s.StoreName == "Warehouse");
                Warehouse = context.WarehouseStock.AsNoTracking().FirstOrDefault(w => w.Id == st.StockId);
                StockItem = context.Stock.AsNoTracking().FirstOrDefault(s => s.StockName == Warehouse.StockName);
                Supplier = context.Suppliers.AsNoTracking().FirstOrDefault(s => s.Id == Warehouse.SupplierId);
                AllStores = context.Stores.AsNoTracking().ToList();
                ApplicationUser = await _userManager.GetUserAsync(User);
                StockTransfer stockTransfer = new StockTransfer
                {
                    Id = st.Id,
                    StockId = st.StockId,
                    FromStoreId = st.FromStoreId,
                    ToStoreId = st.ToStoreId,
                    TransferDate = st.TransferDate,
                    Quantity = st.Quantity,
                    Approvals = "False",
                    ApprovalStatus = "Declined",
                    Approver = ApplicationUser.UserName,
                    DateApproved = DateTime.Now,
                    Requester = st.Requester
                };

                context.StockTransfers.Update(StockTransfer);
                try
                {
                    context.SaveChanges();
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
                        Content = string.Format("Stock {0} was declined on {1}", Warehouse.StockName, stockTransfer.DateApproved)
                    };
                    _emailSender.SendMail(emailMessage);
                }
                catch (Exception ex)
                {

                }
            }
            return Page();
        }

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
                Store warehouseStore = await _context.Stores.AsNoTracking().FirstOrDefaultAsync(s => s.Store_ID == stockTransfer.FromStoreId);
                Warehouse = await context.WarehouseStock.AsNoTracking().FirstOrDefaultAsync(w => w.Id == stockTransfer.StockId);
                Supplier = await context.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == Warehouse.SupplierId);
                AllStores = context.Stores.AsNoTracking().ToList();
                ApplicationUser = await _userManager.GetUserAsync(User);

                StockItem isStockSore = new StockItem();
                isStockSore = context.Stock.AsNoTracking().FirstOrDefault(s => s.StoreId == stockTransfer.ToStoreId && s.StockName == Warehouse.StockName);
                if (isStockSore != null)
                {
                    StockItem toStockItem = new StockItem
                    {
                        Id = isStockSore.Id,
                        StockName = Warehouse.StockName,
                        Unit = Warehouse.Unit,
                        Price = Warehouse.Price,
                        InStock = Warehouse.InStock + AmountToTransfer,
                        StoreId = Convert.ToInt32(StoreId),
                        TransferApprovals = "False"
                    };
                    context.Stock.Update(toStockItem);
                }
                else
                {
                    StockItem toStockItem = new StockItem()
                    {
                        StockName = Warehouse.StockName,
                        Unit = Warehouse.Unit,
                        Price = Warehouse.Price,
                        InStock = AmountToTransfer,
                        StoreId = Convert.ToInt32(StoreId),
                        TransferApprovals = "False"
                    };
                    context.Stock.Add(toStockItem);
                }

                Warehouse.InStock = Warehouse.InStock - AmountToTransfer;

                context.StockTransfers.Update(stockTransfer);
                context.WarehouseStock.Update(Warehouse);

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
                        Content = string.Format("Stock {0} was approved on {1}", Warehouse.StockName, stockTransfer.DateApproved)
                    };
                    _emailSender.SendMail(emailMessage);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}