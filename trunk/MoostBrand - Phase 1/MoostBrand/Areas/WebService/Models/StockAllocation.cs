namespace MoostBrand.Areas.WebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockAllocation
    {
        public int ID { get; set; }

        public int ReceivingID { get; set; }

        public DateTime? SADate { get; set; }

        [StringLength(50)]
        public string Time { get; set; }

        [StringLength(50)]
        public string BatchNumber { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }
    }
}
