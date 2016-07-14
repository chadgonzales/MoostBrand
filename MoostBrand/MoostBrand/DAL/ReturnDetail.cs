using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL
{
    public class ReturnDetail
    {
        public int ID { get; set; }
        public int ReturnID { get; set; }
        public int? ReceivingDetailID { get; set; }
        public int? StockTransferID { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }

        public virtual Return Return { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDetail> StockTransferDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingDetail> ReceivingDetail { get; set; }
    }
}