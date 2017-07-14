namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class StockTransferDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockTransferDetail()
        {
            this.ReturnedItems = new HashSet<ReturnedItem>();
            this.StockAdjustmentDetails = new HashSet<StockAdjustmentDetail>();
            //StockTransferDirects = new HashSet<StockTransferDirect>();
        }


        public int ID { get; set; }

        public int? StockTransferID { get; set; }

        public int? RequisitionDetailID { get; set; }

        public int? Quantity { get; set; }

        public int? AprovalStatusID { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        public int? ReceivingDetailID { get; set; }

        public int? PreviousItemID { get; set; }

        public int? PreviousQuantity { get; set; }

        public int? InStock { get; set; }

        public int? Committed { get; set; }

        public int? Ordered { get; set; }

        public int? Available { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual ReceivingDetail ReceivingDetail { get; set; }

        public virtual ReceivingDetail ReceivingDetail1 { get; set; }

        public virtual RequisitionDetail RequisitionDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReturnedItem> ReturnedItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }

        public virtual StockTransfer StockTransfer { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<StockTransferDirect> StockTransferDirects { get; set; }

        public int GetCommited
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                RequisitionDetailsRepository reqDetailsRepo = new RequisitionDetailsRepository();

                RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);
                return (reqDetailsRepo.getCommited(0, item.ItemID) - Quantity.Value);

            }
        }

        public int GetOrigCommited
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                RequisitionDetailsRepository reqDetailsRepo = new RequisitionDetailsRepository();

                RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);
                return reqDetailsRepo.getCommited(0, item.ItemID);

            }
        }

        public int GetOrigInstock
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                RequisitionDetailsRepository repo = new RequisitionDetailsRepository();

                //int reqId = Convert.ToInt32(HttpContext.Current.Session["requisitionId"]);
                RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);

                int total = repo.getInstocked(item.RequisitionID, item.Item.Code);

                return total;
            }
        }
        public int GetInstock
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                RequisitionDetailsRepository repo = new RequisitionDetailsRepository();

                //int reqId = Convert.ToInt32(HttpContext.Current.Session["requisitionId"]);
                RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);

                int total = (repo.getInstocked(item.RequisitionID, item.Item.Code) - Quantity.Value);

                return total;
            }
        }

        public int GetOrdered
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                RequisitionDetailsRepository repo = new RequisitionDetailsRepository();

                RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);

                int total = repo.getPurchaseOrder(item.ItemID);

                return total;
            }
        }

        public int GetAvailable
        { get { return (GetInstock + GetOrdered) - GetCommited; } }

    }
}


