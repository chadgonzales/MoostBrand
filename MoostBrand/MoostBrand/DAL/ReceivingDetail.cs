namespace MoostBrand.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class ReceivingDetail
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReceivingDetail()
        {
            ReturnedItems = new HashSet<ReturnedItem>();
            StockAdjustmentDetails = new HashSet<StockAdjustmentDetail>();
            StockAllocationDetails = new HashSet<StockAllocationDetail>();
            StockTransferDetails = new HashSet<StockTransferDetail>();
            StockTransferDetails1 = new HashSet<StockTransferDetail>();
            //StockTransferDirects = new HashSet<StockTransferDirect>();
        }

        public int ID { get; set; }

        public int ReceivingID { get; set; }

        public int? StockTransferDetailID { get; set; }

        public int? Quantity { get; set; }

        public int? AprovalStatusID { get; set; }

        public string Remarks { get; set; }

        public bool? IsSync { get; set; }

        public int? RequisitionDetailID { get; set; }

        public int? PreviousItemID { get; set; }

        public int? PreviousQuantity { get; set; }

        public int? InStock { get; set; }

        public int? Committed { get; set; }

        public int? Ordered { get; set; }

        public int? Available { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual Receiving Receiving { get; set; }

        public virtual RequisitionDetail RequisitionDetail { get; set; }

        public virtual RequisitionDetail RequisitionDetail1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReturnedItem> ReturnedItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAllocationDetail> StockAllocationDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDetail> StockTransferDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDetail> StockTransferDetails1 { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<StockTransferDirect> StockTransferDirects { get; set; }

        public int GetCommited
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();

                //int requisitionId = Convert.ToInt32(HttpContext.Current.Session["requisitionId"]);

                RequisitionDetail reqDetail = entity.RequisitionDetails.Find(RequisitionDetailID);
                //var item = entity.Items.Find(req.req);


                //reqDetailsRepo.getCommited(requisitionId, )

                //RequisitionDetailsRepository reqDetailsRepo = new RequisitionDetailsRepository();
                //return reqDetailsRepo.getCommited(requisitionId, RequisitionDetail);

                RequisitionDetailsRepository reqDetailsRepo = new RequisitionDetailsRepository();
                //return (reqDetailsRepo.getCommited(0, RequisitionDetail.Item.ID) - Quantity ?? 0);
                return (reqDetailsRepo.getCommited(reqDetail.RequisitionID, reqDetail.ItemID));
            }
        }
        public int GetAvailable
        { get { return (InStock.Value + (Ordered ?? 0)) - GetCommited; } }


        public int GetInstock
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                RequisitionDetailsRepository repo = new RequisitionDetailsRepository();

                //int reqId = Convert.ToInt32(HttpContext.Current.Session["requisitionId"]);
                RequisitionDetail reqDetail = entity.RequisitionDetails.Find(RequisitionDetailID);


                Item item = entity.Items.Find(reqDetail.ItemID);

                int total = (repo.getInstocked(reqDetail.RequisitionID, item.Code) - repo.getStockTranfer(reqDetail.ItemID));

                return total;
            }
        }

        public int GetOrdered
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();

                RequisitionDetailsRepository repo = new RequisitionDetailsRepository();

                RequisitionDetail reqDetail = entity.RequisitionDetails.Find(RequisitionDetailID);

                int total = repo.getPurchaseOrder(reqDetail.ItemID);

                return total;
            }
        }
    }
}

