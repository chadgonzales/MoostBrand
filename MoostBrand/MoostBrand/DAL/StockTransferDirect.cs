namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StockTransferDirect")]
    public partial class StockTransferDirect
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockTransferDirect()
        {
            StockTransferHelpers = new HashSet<StockTransferHelper>();
            StockTransferOperators = new HashSet<StockTransferOperator>();
            StockTransferDirectItems = new HashSet<StockTransferDirectItems>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string TransferID { get; set; }

        public DateTime? STDate { get; set; }

        [StringLength(50)]
        public string TimeStarted { get; set; }

        [StringLength(50)]
        public string TimeFinish { get; set; }

        [StringLength(50)]
        public string Driver { get; set; }

        [StringLength(50)]
        public string Helper { get; set; }

        public int? ReceivedBy { get; set; }

        [StringLength(50)]
        public string TimeReceived { get; set; }

        public int? RequestedBy { get; set; }

        public int? ReleasedBy { get; set; }

        [StringLength(50)]
        public string Operator { get; set; }

        public int? CounterCheckedBy { get; set; }

        public int? PostedBy { get; set; }

        public int? EncodedBy { get; set; }

        public string Remarks { get; set; }

        public int? ApprovedBy { get; set; }

        public int? ApprovedStatus { get; set; }

        public int? LocationID { get; set; }

        public bool? IsSync { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Employee Employee1 { get; set; }

        public virtual Employee Employee2 { get; set; }

        public virtual Employee Employee3 { get; set; }

        public virtual Employee Employee4 { get; set; }

        public virtual Employee Employee5 { get; set; }

        public virtual Employee Employee6 { get; set; }

        public virtual Location Location { get; set; }

        //public virtual StockTransferDetail StockTransferDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDirectItems> StockTransferDirectItems { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferHelper> StockTransferHelpers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferOperator> StockTransferOperators { get; set; }

        internal void CreateHelpers(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                StockTransferHelpers.Add(new StockTransferHelper());
            }
        }
        internal void CreateOperators(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                StockTransferOperators.Add(new StockTransferOperator());
            }
        }
    }
}