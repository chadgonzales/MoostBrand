//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Requisition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Requisition()
        {
            this.RequisitionDetails = new HashSet<RequisitionDetail>();
            this.StockTransfers = new HashSet<StockTransfer>();
        }
    
        public int ID { get; set; }
        public string RefNumber { get; set; }
        public int RequisitionTypeID { get; set; }
        public int RequestedBy { get; set; }
        public System.DateTime RequestedDate { get; set; }
        public int LocationID { get; set; }
        public Nullable<System.DateTime> DateRequired { get; set; }
        public Nullable<int> VendorID { get; set; }
        public Nullable<System.DateTime> ShipmentDate { get; set; }
        public string Customer { get; set; }
        public Nullable<int> ReservationTypeID { get; set; }
        public Nullable<int> ShipmentTypeID { get; set; }
        public Nullable<int> DropShipID { get; set; }
        public string PaymentStatus { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<int> ReservedBy { get; set; }
        public string AuthorizedPerson { get; set; }
        public Nullable<int> ValidatedBy { get; set; }
        public Nullable<int> Destination { get; set; }
        public string TimeDeparted { get; set; }
        public string TimeArrived { get; set; }
        public string Driver { get; set; }
        public string PlateNumber { get; set; }
        public string Others { get; set; }
        public Nullable<int> ApprovalStatus { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
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
