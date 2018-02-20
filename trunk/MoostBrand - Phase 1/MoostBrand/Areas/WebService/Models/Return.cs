namespace MoostBrand.Areas.WebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Return
    {
        public int ID { get; set; }

        public int? ReturnTypeID { get; set; }

        public DateTime? Date { get; set; }

        public int? TransactionTypeID { get; set; }

        public int? ApprovalStatus { get; set; }

        public int? ApprovedBy { get; set; }

        [StringLength(50)]
        public string ApprovalNumber { get; set; }

        [StringLength(150)]
        public string InspectedBy { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }
    }
}
