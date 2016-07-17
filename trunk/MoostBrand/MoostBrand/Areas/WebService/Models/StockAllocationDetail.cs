namespace MoostBrand.Areas.WebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockAllocationDetail
    {
        public int ID { get; set; }

        public int StockAllocationID { get; set; }

        public int ReceivingDetailID { get; set; }

        public int? Quantity { get; set; }

        public int? ContainerStorageID { get; set; }

        public int? ContainerLocationID { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }
    }
}
