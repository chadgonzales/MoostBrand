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
            StockAllocationDetails = new HashSet<StockAllocationDetail>();
            StockTransferDetails = new HashSet<StockTransferDetail>();
            StockTransferDetails1 = new HashSet<StockTransferDetail>();
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

        public int? ReferenceQuantity { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual Receiving Receiving { get; set; }

        public virtual RequisitionDetail RequisitionDetail { get; set; }

        public virtual RequisitionDetail RequisitionDetail1 { get; set; }

     
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockAllocationDetail> StockAllocationDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDetail> StockTransferDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransferDetail> StockTransferDetails1 { get; set; }

      
        public int GetCommited
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                StockTransferRepository dstrRepo = new StockTransferRepository();

                if (Receiving.ReceivingTypeID != 6)
                {

                    RequisitionDetail reqDetail = entity.RequisitionDetails.Find(RequisitionDetailID);

                    RequisitionDetailsRepository reqDetailsRepo = new RequisitionDetailsRepository();

                    int loc = 0;
                    if (reqDetail.Requisition.Destination == null)
                    {
                        loc = reqDetail.Requisition.LocationID.Value;
                    }
                    else
                    {
                        loc = reqDetail.Requisition.Destination.Value;
                    }

                    return (reqDetailsRepo.getReceivingCommited(loc, reqDetail.ItemID));
                }
                else {
                    var st = entity.StockTransferDetails.Find(RequisitionDetailID);
                    int loc = st.StockTransfer.DestinationID.Value;
                    int itemid = st.Inventories.ItemID.Value;

                    return dstrRepo.getDirectInventory(itemid, loc).Committed.Value;
                }
            }
        }

        public int GetInstock
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                RequisitionDetailsRepository repo = new RequisitionDetailsRepository();
                ReceivingRepository recRepo = new ReceivingRepository();
                StockTransferRepository dstrRepo = new StockTransferRepository();

                //int reqId = Convert.ToInt32(HttpContext.Current.Session["requisitionId"]);
                RequisitionDetail reqDetail = entity.RequisitionDetails.Find(RequisitionDetailID);


                Item item = entity.Items.Find(reqDetail.ItemID);

                int loc = 0;
                if (Receiving.ReceivingTypeID != 6)
                {
                    if (reqDetail.Requisition.Destination == null)
                    {
                        loc = reqDetail.Requisition.LocationID.Value;
                    }
                    else
                    {
                        loc = reqDetail.Requisition.Destination.Value;
                    }
                    int total = 0;
                    try
                    {
                        if (Receiving.ApprovalStatus == 2)
                        {
                            total = repo.getInstockedReceiving(reqDetail.RequisitionID, item.Code);
                        }
                        else
                        {
                            total = repo.getInstockedReceiving(reqDetail.RequisitionID, item.Code);
                            //total = ((repo.getInstockedReceiving(reqDetail.RequisitionID, item.Code) + recRepo.getReceiving(reqDetail.ID)) - repo.getStockTranferReceiving(loc, reqDetail.ItemID));

                        }
                    }
                    catch { total = repo.getInstockedReceiving(reqDetail.RequisitionID, item.Code); }
                    return total;
                }
                else
                {
                    var st = entity.StockTransferDetails.Find(RequisitionDetailID);
                    int loc1 = st.StockTransfer.DestinationID.Value;
                    int itemid = st.Inventories.ItemID.Value;

                    return dstrRepo.getDirectInventory(itemid, loc1).InStock.Value;
                }

              
            }
        }

        public string GetItemDesc
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                StockTransferRepository dstrRepo = new StockTransferRepository();

                if (Receiving.ReceivingTypeID != 6)
                {
                    return RequisitionDetail.Item.DescriptionPurchase;
                }
                else
                {
                    var st = entity.StockTransferDetails.Find(RequisitionDetailID);
                    int loc1 = st.StockTransfer.DestinationID.Value;
                    int itemid = st.Inventories.ItemID.Value;

                    return dstrRepo.getDirectInventory(itemid, loc1).Items.DescriptionPurchase;
                }
            
            }
        }

        public string GetItemCode
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                StockTransferRepository dstrRepo = new StockTransferRepository();

                if (Receiving.ReceivingTypeID != 6)
                {
                    return RequisitionDetail.Item.Code;
                }
               else
                {
                    var st = entity.StockTransferDetails.Find(RequisitionDetailID);
                    int loc1 = st.StockTransfer.DestinationID.Value;
                    int itemid = st.Inventories.ItemID.Value;

                    return dstrRepo.getDirectInventory(itemid, loc1).Items.Code;
                }

            }
        }
        public string GetPrevItem
        {
            get
            {
                MoostBrandEntities entity = new MoostBrandEntities();
                StockTransferRepository dstrRepo = new StockTransferRepository();

                if (Receiving.ReceivingTypeID != 6)
                {
                    return RequisitionDetail1.Item.Description;
                }
               else
                {
                    var st = entity.StockTransferDetails.Find(RequisitionDetailID);
                    int loc = st.StockTransfer.DestinationID.Value;
                    int itemid = st.Inventories.ItemID.Value;

                    return dstrRepo.getDirectInventory(itemid, loc).Items.Description;
                }

            }
        }

        public int GetOrdered
        {
            get
            {
                StockTransferRepository dstrRepo = new StockTransferRepository();
                MoostBrandEntities entity = new MoostBrandEntities();

                RequisitionDetailsRepository repo = new RequisitionDetailsRepository();
                if (Receiving.ReceivingTypeID != 6)
                {

                    RequisitionDetail reqDetail = entity.RequisitionDetails.Find(RequisitionDetailID);

                    int total = repo.getPurchaseOrder(reqDetail.Requisition.LocationID.Value, reqDetail.ItemID);

                    return total;
                }
                else
                {
                    var st = entity.StockTransferDetails.Find(RequisitionDetailID);
                    int loc1 = st.StockTransfer.DestinationID.Value;
                    int itemid = st.Inventories.ItemID.Value;

                    return dstrRepo.getDirectInventory(itemid, loc1).Ordered.Value;
                }
            }
        }
        public int GetAvailable
        { get { return (GetInstock + GetOrdered) - GetCommited; } }


     
    }
}

