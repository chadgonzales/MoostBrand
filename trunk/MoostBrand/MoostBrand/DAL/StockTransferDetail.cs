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
        }

        public int ID { get; set; }

        public int? StockTransferID { get; set; }

        public int? RequisitionDetailID { get; set; }

        public int? Status { get; set; }

        public bool? IsReturn { get; set; }

        public virtual ReasonForAdjustment ReasonForAdjustment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingDetail> ReceivingDetails { get; set; }

        public virtual RequisitionDetail RequisitionDetail { get; set; }
    }
}
