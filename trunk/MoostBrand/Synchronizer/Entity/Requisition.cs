using System;

public class Requisition
{
    public int ID { get; set; }

    public string RefNumber { get; set; }

    public int RequisitionTypeID { get; set; }

    public int RequestedBy { get; set; }

    public DateTime RequestedDate { get; set; }

    public int LocationID { get; set; }

    public DateTime? DateRequired { get; set; }

    public int? VendorID { get; set; }

    public string Customer { get; set; }

    public int? ReservationTypeID { get; set; }

    public int? ShipmentTypeID { get; set; }

    public int? DropShipID { get; set; }

    public string PaymentStatus { get; set; }

    public string InvoiceNumber { get; set; }

    public int? ReservedBy { get; set; }

    public string AuthorizedPerson { get; set; }

    public int? ValidatedBy { get; set; }

    public int? Destination { get; set; }

    public string TimeDeparted { get; set; }

    public string TimeArrived { get; set; }

    public string Driver { get; set; }

    public string PlateNumber { get; set; }

    public string Others { get; set; }

    public int? ApprovalStatus { get; set; }

    public int? ApprovedBy { get; set; }

    public string Remarks { get; set; }

    public bool? IsSync { get; set; }
}