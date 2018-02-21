namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockAllocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockAllocation()
        {
            StockAllocationDetails = new HashSet<StockAllocationDetail>();
        }

        public int ID { get; set; }

        public int ReceivingID { get; set; }

        public DateTime? SADate { get; set; }

        [StringLength(50)]
        public string Time { get; set; }

        [StringLength(50)]
        public string BatchNumber { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        public virtual Receiving Receiving { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAllocationDetail> StockAllocationDetails { get; set; }
    }
}
