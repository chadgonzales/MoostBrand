namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StockLedger")]
    public partial class StockLedger
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockLedger()
        {
            
        }

        public int ID { get; set; }

        public DateTime? Date { get; set; }

        public int InventoryID { get; set; }

        public string Type { get; set; }

        public string ReferenceNo { get; set; }

        public int? BeginningBalance { get; set; }

        public int? InQty { get; set; }

        public int? OutQty { get; set; }

        public int? Variance { get; set; }

        public int? RemainingBalance { get; set; }

        public virtual Inventory Inventories { get; set; }

    }
}