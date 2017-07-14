namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class RequisitionDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequisitionDetail()
        {
            ReceivingDetails = new HashSet<ReceivingDetail>();
            ReceivingDetails1 = new HashSet<ReceivingDetail>();
            StockTransferDetails = new HashSet<StockTransferDetail>();
            //StockTransferDirects = new HashSet<StockTransferDirect>();
        }

        public int ID { get; set; }

        public int RequisitionID { get; set; }

        public int ItemID { get; set; }
        public string ItemCode { get; set; }

        public int? Quantity { get; set; }

        public int? AprovalStatusID { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        public int? PreviousItemID { get; set; }

        public int? PreviousQuantity { get; set; }

        public int? InStock { get; set; }

        public int? Committed { get; set; }

        public int? Ordered { get; set; }

        public int? Available { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual Item Item { get; set; }

        public virtual Item Item1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingDetail> ReceivingDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReceivingDetail> ReceivingDetails1 { get; set; }

        public virtual Requisition Requisition { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDetail> StockTransferDetails { get; set; }

        public int GetCommited {
            get
            {
                RequisitionDetailsRepository reqDetailsRepo = new RequisitionDetailsRepository();
                return reqDetailsRepo.getCommited(RequisitionID, ItemID);

            }
        }
        public int GetInstock
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                RequisitionDetailsRepository repo = new RequisitionDetailsRepository();

                int reqId = Convert.ToInt32(HttpContext.Current.Session["requisitionId"]);

                Item item = entity.Items.Find(ItemID);
                int total = 0;

                if (item != null)
                {
                    total = (repo.getInstocked(reqId, item.Code) - repo.getStockTranfer(ItemID));
                }

                return total;
            }
        }

        public int GetOrdered
        {
            get
            {
                RequisitionDetailsRepository repo = new RequisitionDetailsRepository();

                int total = repo.getPurchaseOrder(ItemID);

                return total;
            }
        }

        public int GetAvailable
        { get { return (GetInstock + GetOrdered) - GetCommited; } }

    }
}
