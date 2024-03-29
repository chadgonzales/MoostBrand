namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Return
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Return()
        {
            ReturnedItems = new HashSet<ReturnedItem>();
        }

        public int ID { get; set; }

        public int? ReturnTypeID { get; set; }

        public DateTime? Date { get; set; }

        public int? TransactionTypeID { get; set; }

        public int? ApprovalStatus { get; set; }

        public int? ApprovedBy { get; set; }

        [StringLength(50)]
        public string ApprovalNumber { get; set; }

        [Display(Name = "Source Location")]
        public int SourceLocationID { get; set; }

        public int InspectedBy { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual Employee Employee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ReturnedItem> ReturnedItems { get; set; }
        public virtual ICollection<ReturnedItem> ReturnedItems { get; set; }

        public virtual ReturnType ReturnType { get; set; }

        public virtual ReturnTransactionType ReturnTransactionType { get; set; }

    }
}
