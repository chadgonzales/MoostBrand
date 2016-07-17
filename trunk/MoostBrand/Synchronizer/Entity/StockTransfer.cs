using System;

public class StockTransfer
{
    public int ID { get; set; }

    public int RequisitionID { get; set; }

    public int LocationID { get; set; }

    public string TransferID { get; set; }

    public DateTime STDAte { get; set; }

    public string StartTime { get; set; }

    public string EndTime { get; set; }

    public string Driver { get; set; }

    public string Helper { get; set; }

    public string TimeReceived { get; set; }

    public int ReceivedBy { get; set; }

    public int RequestedBy { get; set; }

    public int? ApprovedBy { get; set; }

    public int ReleasedBy { get; set; }

    public string Operator { get; set; }

    public int CounterCheckedBy { get; set; }

    public int PostedBy { get; set; }

    public int? ApprovedStatus { get; set; }

    public string Remarks { get; set; }

    public bool? IsSync { get; set; }
}