namespace MoostBrand.Areas.WebService.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    public partial class MoostBrandEntities : DbContext
    {
        public MoostBrandEntities()
            : base("name=MoostBrandEntities")
        {
        }

        public virtual DbSet<ReceivingDetail> ReceivingDetails { get; set; }
        public virtual DbSet<Receiving> Receivings { get; set; }
        public virtual DbSet<RequisitionDetail> RequisitionDetails { get; set; }
        public virtual DbSet<Requisition> Requisitions { get; set; }
        public virtual DbSet<ReturnedItem> ReturnedItems { get; set; }
        public virtual DbSet<Return> Returns { get; set; }
        public virtual DbSet<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        public virtual DbSet<StockAdjustment> StockAdjustments { get; set; }
        public virtual DbSet<StockAllocationDetail> StockAllocationDetails { get; set; }
        public virtual DbSet<StockAllocation> StockAllocations { get; set; }
        public virtual DbSet<StockTransferDetail> StockTransferDetails { get; set; }
        public virtual DbSet<StockTransfer> StockTransfers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Requisition>().MapToStoredProcedures();
        }
        
    }
}
