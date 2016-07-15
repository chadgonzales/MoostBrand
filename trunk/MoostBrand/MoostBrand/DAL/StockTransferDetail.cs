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
            ReceivingDetails = new HashSet<ReceivingDetail>();
            ReturnedItems = new HashSet<ReturnedItem>();
        }

        public int ID { get; set; }

        public int? StockTransferID { get; set; }

        public int? RequisitionDetailID { get; set; }

        public int? Quantity { get; set; }

        public int? AprovalStatusID { get; set; }

        public string Remarks { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingDetail> ReceivingDetails { get; set; }

        public virtual RequisitionDetail RequisitionDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReturnedItem> ReturnedItems { get; set; }

        public virtual StockTransfer StockTransfer { get; set; }
    }
}
