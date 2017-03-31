namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockTransferDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockTransferDetail()
        {
            this.ReturnedItems = new HashSet<ReturnedItem>();
            this.StockAdjustmentDetails = new HashSet<StockAdjustmentDetail>();
        }


        public int ID { get; set; }

        public int? StockTransferID { get; set; }

        public int? RequisitionDetailID { get; set; }

        public int? ReceivingDetailID { get; set; }

        public int? Quantity { get; set; }

        public int? AprovalStatusID { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }
        public virtual ReceivingDetail ReceivingDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReturnedItem> ReturnedItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        public virtual StockTransfer StockTransfer { get; set; }
    }
}


