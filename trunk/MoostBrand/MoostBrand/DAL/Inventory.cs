namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Inventory")]
    public partial class Inventory
    {
        public Inventory()
        {
            StockTransferDetails = new HashSet<StockTransferDetail>();
        }
        public int ID { get; set; }

        [StringLength(50)]
        public string Year { get; set; }

        [StringLength(100)]
        public string ItemCode { get; set; }

        [StringLength(100)]
        public string POSBarCode { get; set; }

        [StringLength(100)]
        public string Description { get; set; }
        public string SalesDescription { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        [StringLength(100)]
        public string InventoryUoM { get; set; }
        
        public int? InventoryStatus { get; set; }
        
        public int? LocationCode { get; set; }

        public int? ReOrder { get; set; }

        public int? InStock { get; set; }

        public int? Ordered { get; set; }

        public int? Committed { get; set; }

        public int? Available { get; set; }

        public int? MinimumInventory { get; set; }

        public int? MaximumInventory { get; set; }

        public int? DailyAverageUsage { get; set; }

        public int? LeadTime { get; set; }
        public int? ItemID { get; set; }

        public virtual InventoryStatu InventoryStatu { get; set; }
        public virtual Location Location { get; set; }

        public virtual Item Items { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDetail> StockTransferDetails { get; set; }
    }
}
