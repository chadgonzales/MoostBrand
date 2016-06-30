//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReceivingDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReceivingDetail()
        {
            this.StockAllocationDetails = new HashSet<StockAllocationDetail>();
        }
    
        public int ID { get; set; }
        public int ReceivingID { get; set; }
        public Nullable<int> StockTransferDetailID { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsReturn { get; set; }
    
        public virtual ReasonForAdjustment ReasonForAdjustment { get; set; }
        public virtual Receiving Receiving { get; set; }
        public virtual StockTransferDetail StockTransferDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAllocationDetail> StockAllocationDetails { get; set; }
    }
}
