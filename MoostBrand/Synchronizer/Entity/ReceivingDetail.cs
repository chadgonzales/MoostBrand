public class ReceivingDetail
{
    public int ID { get; set; }

    public int ReceivingID { get; set; }

    public int? StockTransferDetailID { get; set; }

    public int? Quantity { get; set; }

    public int? AprovalStatusID { get; set; }

    public string Remarks { get; set; }

    public bool? IsSync { get; set; }
}
