namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StockTransfer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockTransfer()
        {
            this.Helpers = new HashSet<Helper>();
            this.Operators = new HashSet<Operator>();
            this.StockTransferDetails = new HashSet<StockTransferDetail>();
        }
        public int ID { get; set; }

        public int? RequisitionID { get; set; }

        public int LocationID { get; set; }

        [Required]
        [StringLength(50)]
        public string TransferID { get; set; }

        public DateTime STDAte { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Time Started by is Required")]
        public string TimeStarted { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Time Finish by is Required")]
        public string TimeFinish { get; set; }

        [StringLength(150)]
        public string Driver { get; set; }

        [StringLength(150)]
        public string Helper { get; set; }

        [StringLength(20)]
        public string TimeReceived { get; set; }

        [Required(ErrorMessage = "Received by is Required")]
        public int ReceivedBy { get; set; }

        [Required(ErrorMessage = "Requested by is Required")]
        public int RequestedBy { get; set; }

        public int? ApprovedBy { get; set; }

        [Required(ErrorMessage = "Released by is Required")]
        public int ReleasedBy { get; set; }

        //[StringLength(150)]
        //[Required(ErrorMessage = "Operator is Required")]
        public string Operator { get; set; }

        public int CounterCheckedBy { get; set; }

        [Required(ErrorMessage = "Posted by is Required")]
        public int PostedBy { get; set; }

        public int? ApprovedStatus { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        public int? EncodedBy { get; set; }

        [StringLength(50)]
        public string PlateNo { get; set; }

       
        public int? ReceivingID { get; set; }
    
        public int StockTransferTypeID { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Employee Employee1 { get; set; }

        public virtual Employee Employee2 { get; set; }

        public virtual Employee Employee3 { get; set; }

        public virtual Employee Employee4 { get; set; }

        public virtual Employee Employee5 { get; set; }

        public virtual Employee Employee6 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Helper> Helpers { get; set; }

        public virtual Location Location { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Operator> Operators { get; set; }

        public virtual Receiving Receiving { get; set; }

        public virtual Requisition Requisition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDetail> StockTransferDetails { get; set; }

        public virtual StockTransferType StockTransferType { get; set; }

        //public int? _ReservationID { get; set; }
        internal void CreateHelpers(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                Helpers.Add(new Helper());
            }
        }
        internal void CreateOperators(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                Operators.Add(new Operator());
            }
        }


    }
}
