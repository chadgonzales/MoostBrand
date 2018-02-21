namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockTransferHelper
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? DeletedHelper { get; set; }

        public int? StockTransferDirectID { get; set; }

        public virtual StockTransferDirect StockTransferDirect { get; set; }
    }
}