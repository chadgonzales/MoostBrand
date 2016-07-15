namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReturnedItem
    {
        public int ID { get; set; }

        public int? ReturnID { get; set; }

        public int? ReceivingDetailID { get; set; }

        public int? StockTransferDetailID { get; set; }

        public int? Quantity { get; set; }

        public string Image { get; set; }

        public virtual ReceivingDetail ReceivingDetail { get; set; }

        public virtual Return Return { get; set; }

        public virtual StockTransferDetail StockTransferDetail { get; set; }
    }
}
