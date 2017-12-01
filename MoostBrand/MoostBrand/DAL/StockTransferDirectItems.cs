namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StockTransferDirectItems")]
    public partial class StockTransferDirectItems
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockTransferDirectItems()
        {
           
        }

        public int ID { get; set; }
        public int? StockTransferDirectID { get; set; }
        public int? ItemID { get; set; }

        [StringLength(250)]
        public string ItemName { get; set; }

        public int? Quantity { get; set; }

        public virtual StockTransferDirect StockTransferDirects { get; set; }
    }
}