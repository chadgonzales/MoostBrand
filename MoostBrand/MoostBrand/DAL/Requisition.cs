namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Requisition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Requisition()
        {
            RequisitionDetails = new HashSet<RequisitionDetail>();
            StockTransfers = new HashSet<StockTransfer>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string RefNumber { get; set; }

        public int RequisitionTypeID { get; set; }

        public int RequestedBy { get; set; }

        public DateTime RequestedDate { get; set; }

        public int LocationID { get; set; }

        public DateTime? DateRequired { get; set; }

        public int? VendorID { get; set; }

        [StringLength(150)]
        public string Customer { get; set; }

        public int? ReservationTypeID { get; set; }

        public int? ShipmentTypeID { get; set; }

        public int? DropShipID { get; set; }

        [StringLength(50)]
        public string PaymentStatus { get; set; }

        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        public int? ReservedBy { get; set; }

        [StringLength(150)]
        public string AuthorizedPerson { get; set; }

        public int? ValidatedBy { get; set; }

        public int? Destination { get; set; }

        [StringLength(20)]
        public string TimeDeparted { get; set; }

        [StringLength(20)]
        public string TimeArrived { get; set; }

        [StringLength(50)]
        public string Driver { get; set; }

        [StringLength(20)]
        public string PlateNumber { get; set; }

        [StringLength(50)]
        public string Others { get; set; }

        public int? ApprovalStatus { get; set; }

        public int? ApprovedBy { get; set; }

        public string Remarks { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual DropShipType DropShipType { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Employee Employee1 { get; set; }

        public virtual Employee Employee2 { get; set; }

        public virtual Employee Employee3 { get; set; }

        public virtual Location Location { get; set; }

        public virtual Location Location1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequisitionDetail> RequisitionDetails { get; set; }

        public virtual RequisitionType RequisitionType { get; set; }

        public virtual ReservationType ReservationType { get; set; }

        public virtual ShipmentType ShipmentType { get; set; }

        public virtual Vendor Vendor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransfer> StockTransfers { get; set; }
    }
}
