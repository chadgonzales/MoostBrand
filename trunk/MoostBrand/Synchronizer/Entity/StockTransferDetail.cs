public class StockTransferDetail
{
    public int ID { get; set; }

    public int? StockTransferID { get; set; }

    public int? RequisitionDetailID { get; set; }

    public int? Quantity { get; set; }

    public int? AprovalStatusID { get; set; }

    public string Remarks { get; set; }

    public bool? IsSync { get; set; }
}