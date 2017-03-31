namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReceivingDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReceivingDetail()
        {
            this.ReturnedItems = new HashSet<ReturnedItem>();
            this.StockAdjustmentDetails = new HashSet<StockAdjustmentDetail>();
            this.StockAllocationDetails = new HashSet<StockAllocationDetail>();
            this.StockTransferDetails = new HashSet<StockTransferDetail>();
        }

 
        public int ID { get; set; }

        public int ReceivingID { get; set; }

        public int? StockTransferDetailID { get; set; }

        public int? RequisitionDetailID { get; set; }

        public int? Quantity { get; set; }

        public int? AprovalStatusID { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }


        public virtual ApprovalStatu ApprovalStatu { get; set; }
        public virtual Receiving Receiving { get; set; }
        public virtual RequisitionDetail RequisitionDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReturnedItem> ReturnedItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAllocationDetail> StockAllocationDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDetail> StockTransferDetails { get; set; }
    }
}

