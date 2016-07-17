public class RequisitionDetail
{
    public int ID { get; set; }

    public int RequisitionID { get; set; }

    public int ItemID { get; set; }

    public int Quantity { get; set; }

    public int? AprovalStatusID { get; set; }

    public string Remarks { get; set; }

    public bool? IsSync { get; set; }
}