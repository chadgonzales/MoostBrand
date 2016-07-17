using System;

public class StockAllocation
{
    public int ID { get; set; }

    public int ReceivingID { get; set; }

    public DateTime? SADate { get; set; }

    public string Time { get; set; }

    public string BatchNumber { get; set; }

    public string Remarks { get; set; }

    public bool? IsSync { get; set; }
}