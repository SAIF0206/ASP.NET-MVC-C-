using AD_Project.Models;
using AD_Project.Models.Departments;
using AD_Project.Models.Store;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AD_Project.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //Add DB Sets
        public DbSet<Department> Departments { get; set; }
        public DbSet<CollectionPoint> CollectionPoints { get; set; }
       
        public DbSet<Dept_Staff> Dept_Staffs { get; set; }
        public DbSet<Store_Staff> Store_Staffs { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<EmployeeRequestForm> EmployeeRequestForms { get; set; }
        public DbSet<RequestItem> RequestItems { get; set; }
        public DbSet<CollectionOfRequestedItems> collectionOfRequestedItems { get; set; }
        public DbSet<ConsolidatedRequisition> ConsolidatedRequisitions { get; set; }
        public DbSet<DisburseItem> DisburseItems { get; set; }
        public DbSet<DisbursementList> DisbursementLists { get; set; }
        public DbSet<AdjustedItem> AdjustedItems { get; set; }
        public DbSet<AdjustmentVoucher> AdjustmentVouchers { get; set; }
        public DbSet<Suppliers_Price> Suppliers_Prices { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<ConsolidatedList> ConsolidatedLists { get; set; }
        public DbSet<RetrievalList> RetrievalLists { get; set; }
        public DbSet<StockDetails> Stockdetails { get; set; }
        public DbSet<HeadDelegate> HeadDelegates { get; set; }
        public DbSet<DeptRequestedItems> DeptRequestedItems { get; set; }

        public DbSet<TrendAnalysis> TrendAnalyses { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

       public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}