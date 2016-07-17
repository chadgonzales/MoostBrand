using System;

public class StockAdjustment
{
    public int ID { get; set; }

    public int? TransactionTypeID { get; set; }

    public int? ReturnTypeID { get; set; }

    public DateTime? Date { get; set; }

    public int? PreparedBy { get; set; }

    public int? AdjustedBy { get; set; }

    public int? ApprovalStatus { get; set; }

    public int? ApprovedBy { get; set; }

    public string Remarks { get; set; }

    public bool? IsSync { get; set; }
}