namespace MoostBrand.Areas.WebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockAdjustmentDetail
    {
        public int ID { get; set; }

        public int? StockAdjustmentID { get; set; }

        public int? ReceivingDetailID { get; set; }

        public int? StockTransferDetailID { get; set; }

        public int? ReasonForAdjustmentID { get; set; }

        public int? QuantityOrdered { get; set; }

        public int? QuantityReceived { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }
    }
}
