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

        [Display(Name = "Stock Adjustment Number")]
        public string No { get; set; }

        [Display(Name = "Reference Number")]
        public string ReferenceNo { get; set; }
        public int? TransactionTypeID { get; set; }

        [Display(Name = "Error Date")]
        public DateTime ErrorDate { get; set; }

        public string _ErrorDate
        {
            get { return ErrorDate.ToString("MM/dd/yyyy");  }
        }

        [Required(ErrorMessage = "Location is Required")]
        public int LocationID { get; set; }

        public int? PreparedBy { get; set; }

        public int? AdjustedBy { get; set; }

        public int? ApprovalStatus { get; set; }

        public int? ApprovedBy { get; set; }

        public bool? IsSync { get; set; }

        public int? PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

        public string Comments { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Location Location { get; set; }

        public virtual Employee Employee1 { get; set; }

        public virtual Employee Employee2 { get; set; }

        public virtual Employee Employee3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }

        public virtual TransactionType TransactionType { get; set; }
    }
}
