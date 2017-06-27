namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RequisitionDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequisitionDetail()
        {
            ReceivingDetails = new HashSet<ReceivingDetail>();
            ReceivingDetails1 = new HashSet<ReceivingDetail>();
            StockTransferDetails = new HashSet<StockTransferDetail>();
            //StockTransferDirects = new HashSet<StockTransferDirect>();
        }

        public int ID { get; set; }

        public int RequisitionID { get; set; }

        public int ItemID { get; set; }
        public string ItemCode { get; set; }

        public int? Quantity { get; set; }

        public int? AprovalStatusID { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        public int? PreviousItemID { get; set; }

        public int? PreviousQuantity { get; set; }

        public int? InStock { get; set; }

        public int? Committed { get; set; }

        public int? Ordered { get; set; }

        public int? Available { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual Item Item { get; set; }

        public virtual Item Item1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingDetail> ReceivingDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingDetail> ReceivingDetails1 { get; set; }

        public virtual Requisition Requisition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDetail> StockTransferDetails { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<StockTransferDirect> StockTransferDirects { get; set; }
    }
}
