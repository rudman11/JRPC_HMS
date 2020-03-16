using JRPC_HMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JRPC_HMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=JRPC_POS;Integrated Security=SSPI;Persist Security Info=True;");
                //optionsBuilder.UseSqlServer(@"Server=SQL5050.site4now.net;Initial Catalog=DB_A540F0_JRPCPOS;User Id=DB_A540F0_JRPCPOS_admin;Password=M1ckey@M0use;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<StockItem> Stock { get; set; }
        public DbSet<StockToProduct> StockToProducts { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Warehouse> WarehouseStock { get; set; }
        public DbSet<StockTransfer> StockTransfers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<LogModel> Log { get; set; }
        public DbSet<Requisition> Requisitions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Change> ChangeLog { get; set; }
    }
}
