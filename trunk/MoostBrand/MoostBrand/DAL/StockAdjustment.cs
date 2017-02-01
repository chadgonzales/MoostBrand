namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockAdjustment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockAdjustment()
        {
            StockAdjustmentDetails = new HashSet<StockAdjustmentDetail>();
        }

        public int ID { get; set; }

        public int? TransactionTypeID { get; set; }

        public int? ReturnTypeID { get; set; }

        public DateTime? Date { get; set; }

        public int? PreparedBy { get; set; }

        public int? AdjustedBy { get; set; }

        public int? ApprovalStatus { get; set; }

        public int? ApprovedBy { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        public int? PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

        public DateTime? EncodedDate { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Employee Employee1 { get; set; }

        public virtual Employee Employee2 { get; set; }

        public virtual Employee Employee3 { get; set; }

        public virtual ReturnType ReturnType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }

        public virtual TransactionType TransactionType { get; set; }
    }
}
