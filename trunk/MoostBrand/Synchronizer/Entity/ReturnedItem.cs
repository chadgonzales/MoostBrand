public class ReturnedItem
{
    public int ID { get; set; }

    public int? ReturnID { get; set; }

    public int? ReceivingDetailID { get; set; }

    public int? StockTransferDetailID { get; set; }

    public int? Quantity { get; set; }

    public string Image { get; set; }

    public string Remarks { get; set; }

    public bool? IsSync { get; set; }
}