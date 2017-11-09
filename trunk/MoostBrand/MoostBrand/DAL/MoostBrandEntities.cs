namespace MoostBrand.DAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MoostBrandEntities : DbContext
    {
        public MoostBrandEntities()
            : base("name=MoostBrandEntities")
        {
        }

        public virtual DbSet<ApprovalStatu> ApprovalStatus { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<ContainerLocation> ContainerLocations { get; set; }
        public virtual DbSet<ContainerStorage> ContainerStorages { get; set; }
        public virtual DbSet<Downpayment> Downpayments { get; set; }
        public virtual DbSet<DropShipType> DropShipTypes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Helper> Helpers { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryStatu> InventoryStatus { get; set; }
        public virtual DbSet<ItemDetails> ItemDetail { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationType> LocationTypes { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Operator> Operators { get; set; }
        public virtual DbSet<PaymentStatu> PaymentStatus { get; set; }
        public virtual DbSet<ReasonForAdjustment> ReasonForAdjustments { get; set; }
        public virtual DbSet<ReceivingDetail> ReceivingDetails { get; set; }
        public virtual DbSet<Receiving> Receivings { get; set; }
        public virtual DbSet<ReceivingType> ReceivingTypes { get; set; }
        public virtual DbSet<ReqType> ReqTypes { get; set; }
        public virtual DbSet<RequisitionDetail> RequisitionDetails { get; set; }
        public virtual DbSet<Requisition> Requisitions { get; set; }
        public virtual DbSet<RequisitionType> RequisitionTypes { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationType> ReservationTypes { get; set; }
        public virtual DbSet<ReturnedItem> ReturnedItems { get; set; }
        public virtual DbSet<Return> Returns { get; set; }
        public virtual DbSet<ReturnTransactionType> ReturnTransactionTypes { get; set; }
        public virtual DbSet<ReturnType> ReturnTypes { get; set; }
        public virtual DbSet<ShipmentType> ShipmentTypes { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        public virtual DbSet<StockAdjustment> StockAdjustments { get; set; }
        public virtual DbSet<StockAllocationDetail> StockAllocationDetails { get; set; }
        public virtual DbSet<StockAllocation> StockAllocations { get; set; }
        public virtual DbSet<StockTransferDetail> StockTransferDetails { get; set; }
        public virtual DbSet<StockTransferDirect> StockTransferDirects { get; set; }
        public virtual DbSet<StockTransferHelper> StockTransferHelpers { get; set; }
        public virtual DbSet<StockTransferOperator> StockTransferOperators { get; set; }
        public virtual DbSet<StockLedger> StockLedgers { get; set; }
        public virtual DbSet<StockTransfer> StockTransfers { get; set; }
        public virtual DbSet<StockTransferType> StockTransferTypes { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<SubCategoriesType> SubCategoriesTypes { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }
        public virtual DbSet<UserAccess> UserAccesses { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalStatu>()
                .HasMany(e => e.ReceivingDetails)
                .WithOptional(e => e.ApprovalStatu)
                .HasForeignKey(e => e.AprovalStatusID);

            modelBuilder.Entity<ApprovalStatu>()
                .HasMany(e => e.Receivings)
                .WithOptional(e => e.ApprovalStatu)
                .HasForeignKey(e => e.ApprovalStatus);

            modelBuilder.Entity<ApprovalStatu>()
                .HasMany(e => e.RequisitionDetails)
                .WithOptional(e => e.ApprovalStatu)
                .HasForeignKey(e => e.AprovalStatusID);

            modelBuilder.Entity<ApprovalStatu>()
                .HasMany(e => e.Requisitions)
                .WithOptional(e => e.ApprovalStatu)
                .HasForeignKey(e => e.ApprovalStatus);

            modelBuilder.Entity<ApprovalStatu>()
                .HasMany(e => e.Returns)
                .WithOptional(e => e.ApprovalStatu)
                .HasForeignKey(e => e.ApprovalStatus);

            modelBuilder.Entity<ApprovalStatu>()
                .HasMany(e => e.StockAdjustments)
                .WithOptional(e => e.ApprovalStatu)
                .HasForeignKey(e => e.ApprovalStatus);

            modelBuilder.Entity<ApprovalStatu>()
                .HasMany(e => e.StockTransferDetails)
                .WithOptional(e => e.ApprovalStatu)
                .HasForeignKey(e => e.AprovalStatusID);

            modelBuilder.Entity<ApprovalStatu>()
                .HasMany(e => e.StockTransferDirects)
                .WithOptional(e => e.ApprovalStatu)
                .HasForeignKey(e => e.ApprovedStatus);

            modelBuilder.Entity<ApprovalStatu>()
                .HasMany(e => e.StockTransfers)
                .WithOptional(e => e.ApprovalStatu)
                .HasForeignKey(e => e.ApprovedStatus);

            modelBuilder.Entity<Downpayment>()
                .Property(e => e.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<DropShipType>()
                .HasMany(e => e.Requisitions)
                .WithOptional(e => e.DropShipType)
                .HasForeignKey(e => e.DropShipID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransfers)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.ApprovedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransfers1)
                .WithRequired(e => e.Employee1)
                .HasForeignKey(e => e.CounterCheckedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransfers2)
                .WithRequired(e => e.Employee2)
                .HasForeignKey(e => e.PostedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransfers3)
                .WithRequired(e => e.Employee3)
                .HasForeignKey(e => e.ReceivedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransfers4)
                .WithRequired(e => e.Employee4)
                .HasForeignKey(e => e.ReleasedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransfers5)
                .WithRequired(e => e.Employee5)
                .HasForeignKey(e => e.RequestedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Receivings)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.EncodedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Receivings1)
                .WithOptional(e => e.Employee1)
                .HasForeignKey(e => e.CheckedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Receivings2)
                .WithRequired(e => e.Employee2)
                .HasForeignKey(e => e.ReceivedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Receivings3)
                .WithOptional(e => e.Employee3)
                .HasForeignKey(e => e.ApprovedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Requisitions)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.ApprovedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Requisitions1)
                .WithRequired(e => e.Employee1)
                .HasForeignKey(e => e.RequestedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Requisitions2)
                .WithOptional(e => e.Employee2)
                .HasForeignKey(e => e.ReservedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Requisitions3)
                .WithOptional(e => e.Employee3)
                .HasForeignKey(e => e.ValidatedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Returns)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.ApprovedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockAdjustments)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.PreparedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockAdjustments1)
                .WithOptional(e => e.Employee1)
                .HasForeignKey(e => e.AdjustedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockAdjustments2)
                .WithOptional(e => e.Employee2)
                .HasForeignKey(e => e.ApprovedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockAdjustments3)
                .WithOptional(e => e.Employee3)
                .HasForeignKey(e => e.PostedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransferDirects)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.ReceivedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransferDirects1)
                .WithOptional(e => e.Employee1)
                .HasForeignKey(e => e.RequestedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransferDirects2)
                .WithOptional(e => e.Employee2)
                .HasForeignKey(e => e.ApprovedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransferDirects3)
                .WithOptional(e => e.Employee3)
                .HasForeignKey(e => e.ReleasedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransferDirects4)
                .WithOptional(e => e.Employee4)
                .HasForeignKey(e => e.CounterCheckedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransferDirects5)
                .WithOptional(e => e.Employee5)
                .HasForeignKey(e => e.PostedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransferDirects6)
                .WithOptional(e => e.Employee6)
                .HasForeignKey(e => e.EncodedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StockTransfers6)
                .WithOptional(e => e.Employee6)
                .HasForeignKey(e => e.EncodedBy);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.UserAccesses)
                .WithOptional(e => e.Employee)
                .WillCascadeOnDelete();

            modelBuilder.Entity<InventoryStatu>()
                .HasMany(e => e.Inventories)
                .WithOptional(e => e.InventoryStatu)
                .HasForeignKey(e => e.InventoryStatus);

            modelBuilder.Entity<ItemDetails>()
                .Property(e => e.Cost)
                .HasPrecision(12, 2);

            modelBuilder.Entity<ItemDetails>()
                .Property(e => e.WeightedAverageCost)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Item>()
                .Property(e => e.LastUnitCost)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Item>()
                .Property(e => e.WeightedAverageCost)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Item>()
                .Property(e => e.Price)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.RequisitionDetails)
                .WithRequired(e => e.Item)
                .HasForeignKey(e => e.ItemID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.RequisitionDetails1)
                .WithOptional(e => e.Item1)
                .HasForeignKey(e => e.PreviousItemID);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Inventories)
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.LocationCode);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.StockTransfers)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Receivings)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Requisitions)
                .WithRequired(e => e.Location)
                .HasForeignKey(e => e.LocationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Requisitions1)
                .WithOptional(e => e.Location1)
                .HasForeignKey(e => e.Destination);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.UserAccesses)
                .WithOptional(e => e.Module)
                .WillCascadeOnDelete();

            modelBuilder.Entity<PaymentStatu>()
                .HasMany(e => e.Requisitions)
                .WithOptional(e => e.PaymentStatu)
                .HasForeignKey(e => e.PaymentStatusID);

            modelBuilder.Entity<ReceivingDetail>()
                .HasMany(e => e.StockTransferDetails)
                .WithOptional(e => e.ReceivingDetail)
                .HasForeignKey(e => e.ReceivingDetailID);

            modelBuilder.Entity<ReceivingDetail>()
                .HasMany(e => e.StockTransferDetails1)
                .WithOptional(e => e.ReceivingDetail1)
                .HasForeignKey(e => e.PreviousItemID);

            modelBuilder.Entity<Receiving>()
                .HasMany(e => e.StockAllocations)
                .WithRequired(e => e.Receiving)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReceivingType>()
                .HasMany(e => e.Receivings)
                .WithRequired(e => e.ReceivingType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RequisitionDetail>()
                .HasMany(e => e.ReceivingDetails)
                .WithOptional(e => e.RequisitionDetail)
                .HasForeignKey(e => e.RequisitionDetailID);

            modelBuilder.Entity<RequisitionDetail>()
                .HasMany(e => e.ReceivingDetails1)
                .WithOptional(e => e.RequisitionDetail1)
                .HasForeignKey(e => e.PreviousItemID);

            modelBuilder.Entity<Requisition>()
                .HasMany(e => e.Receivings)
                .WithRequired(e => e.Requisition)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RequisitionType>()
                .HasMany(e => e.Requisitions)
                .WithRequired(e => e.RequisitionType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Return>()
                .HasMany(e => e.ReturnedItems)
                .WithOptional(e => e.Return)
                .WillCascadeOnDelete();

            modelBuilder.Entity<ReturnTransactionType>()
                .HasMany(e => e.Returns)
                .WithOptional(e => e.ReturnTransactionType)
                .HasForeignKey(e => e.TransactionTypeID);

            modelBuilder.Entity<StockAdjustment>()
                .HasMany(e => e.StockAdjustmentDetails)
                .WithOptional(e => e.StockAdjustment)
                .WillCascadeOnDelete();

            modelBuilder.Entity<StockAllocation>()
                .HasMany(e => e.StockAllocationDetails)
                .WithOptional(e => e.StockAllocation)
                .WillCascadeOnDelete();

            modelBuilder.Entity<StockTransfer>()
                .HasMany(e => e.StockTransferDetails)
                .WithOptional(e => e.StockTransfer)
                .WillCascadeOnDelete();

            modelBuilder.Entity<StockTransferType>()
                .HasMany(e => e.StockTransfers)
                .WithRequired(e => e.StockTransferType)
                .WillCascadeOnDelete(false);





            modelBuilder.Entity<UnitOfMeasurement>()
                .Property(e => e.QuantityOfMeasure)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UserType>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.UserType)
                .WillCascadeOnDelete(false);

            //////////////////

            modelBuilder.Entity<SubCategoriesType>()
                .HasMany(e => e.SubCategories)
                .WithOptional(e => e.SubCategoriesTypes)
                .HasForeignKey(e => e.SubCategoryTypeID);

        
        }
    }
}
