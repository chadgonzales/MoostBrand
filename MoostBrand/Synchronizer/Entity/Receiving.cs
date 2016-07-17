using System;

public class Receiving
{
    public int ID { get; set; }

    public int LocationID { get; set; }

    public int ReceivingTypeID { get; set; }

    public string ReceivingID { get; set; }

    public int StockTransferID { get; set; }

    public DateTime ReceivingDate { get; set; }

    public string ReceivingTime { get; set; }

    public int? EncodedBy { get; set; }

    public int? CheckedBy { get; set; }

    public int ReceivedBy { get; set; }

    public string PONumber { get; set; }

    public string DRNumber { get; set; }

    public string InvoiceNumber { get; set; }

    public string VesselNumber { get; set; }

    public string VoyageNumber { get; set; }

    public string VanNumber { get; set; }

    public int? ApprovalStatus { get; set; }

    public int? ApprovedBy { get; set; }

    public string Remarks { get; set; }

    public bool? IsSync { get; set; }
}