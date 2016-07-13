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

        public int? ItemID { get; set; }

        [StringLength(50)]
        public string ItemName { get; set; }

        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        public DateTime? DateOfReturn { get; set; }

        public int? QuantityReturned { get; set; }

        public string Reason { get; set; }

        public string Image { get; set; }

        public int? ApprovedBy { get; set; }

        [StringLength(50)]
        public string ReferenceNo { get; set; }
    }
}
