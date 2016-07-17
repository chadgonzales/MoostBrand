namespace MoostBrand.Areas.WebService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockTransfer
    {
        public int ID { get; set; }

        public int RequisitionID { get; set; }

        public int LocationID { get; set; }

        [Required]
        [StringLength(50)]
        public string TransferID { get; set; }

        public DateTime STDAte { get; set; }

        [StringLength(20)]
        public string StartTime { get; set; }

        [StringLength(20)]
        public string EndTime { get; set; }

        [StringLength(150)]
        public string Driver { get; set; }

        [StringLength(150)]
        public string Helper { get; set; }

        [StringLength(20)]
        public string TimeReceived { get; set; }

        public int ReceivedBy { get; set; }

        public int RequestedBy { get; set; }

        public int? ApprovedBy { get; set; }

        public int ReleasedBy { get; set; }

        [StringLength(150)]
        public string Operator { get; set; }

        public int CounterCheckedBy { get; set; }

        public int PostedBy { get; set; }

        public int? ApprovedStatus { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }
    }
}
