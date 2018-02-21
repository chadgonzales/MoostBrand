namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Downpayment
    {
        public int ID { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? Date { get; set; }
    }
}
