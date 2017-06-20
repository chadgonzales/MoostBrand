namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockTransferOperator
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? DeletedOperator { get; set; }

        public int? StockTransferDirectID { get; set; }

        public virtual StockTransferDirect StockTransferDirect { get; set; }
    }
}