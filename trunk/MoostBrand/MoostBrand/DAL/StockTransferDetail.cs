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
           // this.ReturnedItems = new HashSet<ReturnedItem>();
            //StockTransferDirects = new HashSet<StockTransferDirect>();
        }


        public int ID { get; set; }

        public int? StockTransferID { get; set; }
        public int? InventoryID { get; set; }
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

        public int? ReferenceQuantity { get; set; }

        public virtual ApprovalStatu ApprovalStatu { get; set; }

        public virtual ReceivingDetail ReceivingDetail { get; set; }

        public virtual ReceivingDetail ReceivingDetail1 { get; set; }

        public virtual RequisitionDetail RequisitionDetail { get; set; }

        public virtual Inventory Inventories { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ReturnedItem> ReturnedItems { get; set; }

        public virtual StockTransfer StockTransfer { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<StockTransferDirect> StockTransferDirects { get; set; }

        public int GetCommited
        {
            get
            {
                try
                {
                    int _rid = 0, _ritemid = 0;
                    MoostBrandEntities entity = new MoostBrandEntities();
                    RequisitionDetailsRepository reqDetailsRepo = new RequisitionDetailsRepository();
                    StockTransferRepository stRepo = new StockTransferRepository();


                    if (RequisitionDetailID != null)
                    {
                        RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);
                        _rid = item.RequisitionID;
                        _ritemid = item.ItemID;
                        return (reqDetailsRepo.getCommited(_rid, _ritemid) - Quantity.Value);

                    }
                    else
                    {
                        ReceivingDetail item1 = entity.ReceivingDetails.Find(ReceivingDetailID);
                        _rid = item1.Receiving.RequisitionID.Value;
                        _ritemid = item1.RequisitionDetail.ItemID;
                        return (stRepo.getCommited_Receiving(_rid, _ritemid) - Quantity.Value);
                    }
                }
                catch
                {
                    return 0; 
                }
              
              
            }
        }

        public int GetOrigCommited
        {
            get
            {
                try
                {
                    int _rid = 0, _ritemid = 0;
                    MoostBrandEntities entity = new MoostBrandEntities();
                    RequisitionDetailsRepository reqDetailsRepo = new RequisitionDetailsRepository();
                    StockTransferRepository stRepo = new StockTransferRepository();


                    if (RequisitionDetailID != null)
                    {
                        RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);
                        _rid = item.RequisitionID;
                        _ritemid = item.ItemID;
                        return (reqDetailsRepo.getCommited(_rid, _ritemid));

                    }
                    else
                    {
                        ReceivingDetail item1 = entity.ReceivingDetails.Find(ReceivingDetailID);
                        _rid = item1.Receiving.RequisitionID.Value;
                        _ritemid = item1.RequisitionDetail.ItemID;
                        return (stRepo.getCommited_Receiving(_rid, _ritemid));
                    }

                }
                catch
                {
                    return 0;
                }

            }
        }

        public int GetOrigInstock
        {
            get
            {
                try
                {
                    int _rid = 0, _ritemid = 0;
                    string _ritemcode = "";
                    MoostBrandEntities entity = new MoostBrandEntities();
                    RequisitionDetailsRepository repo = new RequisitionDetailsRepository();
                    StockTransferRepository stRepo = new StockTransferRepository();

                    //int reqId = Convert.ToInt32(HttpContext.Current.Session["requisitionId"]);
                    // RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);

                    if (RequisitionDetailID != null)
                    {
                        RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);
                        _rid = item.RequisitionID;
                        _ritemid = item.ItemID;
                        _ritemcode = item.Item.Code;

                    }
                    else
                    {
                        ReceivingDetail item1 = entity.ReceivingDetails.Find(ReceivingDetailID);
                        _rid = item1.Receiving.RequisitionID.Value;
                        _ritemid = item1.RequisitionDetail.ItemID;
                        _ritemcode = item1.RequisitionDetail.Item.Code;

                    }
                    int total = repo.getInstocked(_rid, _ritemcode);
                    //int total = (repo.getInstocked(_rid, _ritemcode) - repo.getStockTranfer(_ritemid));

                    return total;
                }
                catch
                    { return 0; }
            }
        }

      
        public int GetInstock
        {
            get
            {
                try
                {
                    int _rid = 0, _ritemid = 0;
                    string _ritemcode = "";

                    MoostBrandEntities entity = new MoostBrandEntities();
                    RequisitionDetailsRepository repo = new RequisitionDetailsRepository();

                    //int reqId = Convert.ToInt32(HttpContext.Current.Session["requisitionId"]);
                    // RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);

                    if (RequisitionDetailID != null)
                    {
                        RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);
                        _rid = item.RequisitionID;
                        _ritemid = item.ItemID;
                        _ritemcode = item.Item.Code;


                    }
                    else
                    {
                        ReceivingDetail item1 = entity.ReceivingDetails.Find(ReceivingDetailID);
                        _rid = item1.Receiving.RequisitionID.Value;
                        _ritemid = item1.RequisitionDetail.ItemID;
                        _ritemcode = item1.RequisitionDetail.Item.Code;

                    }


                    int total = (repo.getInstocked(_rid, _ritemcode) - Quantity.Value);

                    return total;
                }
                catch
                { return 0; }
            }
        }

        public int GetOrdered
        {
            get
            {
                try
                {
                    int _rlocationid = 0, _ritemid = 0;


                    MoostBrandEntities entity = new MoostBrandEntities();
                    RequisitionDetailsRepository repo = new RequisitionDetailsRepository();

                    //RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);


                    if (RequisitionDetailID != null)
                    {
                        RequisitionDetail item = entity.RequisitionDetails.Find(RequisitionDetailID);
                        _rlocationid = item.Requisition.LocationID.Value;
                        _ritemid = item.ItemID;

                    }
                    else
                    {
                        ReceivingDetail item1 = entity.ReceivingDetails.Find(ReceivingDetailID);
                        _rlocationid = item1.RequisitionDetail.Requisition.LocationID.Value;
                        _ritemid = item1.RequisitionDetail.ItemID;

                    }


                    int total = repo.getPurchaseOrder(_rlocationid, _ritemid);

                    return total;
                }
                catch
                { return 0; }
            }
        }

        public int GetAvailable
        { get { return (GetOrigInstock + GetOrdered) - GetOrigCommited; } }

        public int GetAvailable_Direct
        { get
            {
                try
                {
                    return (InventoryInstock + OrderedInstock) - CommittedInstock;
                }
                catch { return 0; }
            }
        }

        public int InventoryInstock
        {
            get
            {
                try
                {
                    MoostBrandEntities entity = new MoostBrandEntities();
                    Inventory inv = entity.Inventories.Find(InventoryID);

                    return inv.InStock.Value;
                }
                catch { return 0; }
            }
        }

        public int CommittedInstock
        {
            get
            {
                try
                {
                MoostBrandEntities entity = new MoostBrandEntities();
                Inventory inv = entity.Inventories.Find(InventoryID);

                return inv.Committed.Value;
                }
                catch { return 0; }
            }
        }

        public int OrderedInstock
        {
            get
            {
                try
                {
                MoostBrandEntities entity = new MoostBrandEntities();
                Inventory inv = entity.Inventories.Find(InventoryID);

                return inv.Ordered.Value;
                }
                catch { return 0; }
            }
        }

        public int pendingInstock
        {
            get
            { 
                return InventoryInstock - Quantity.Value;
            }
        }

        public int pendingAvailable
        {
            get
            {
                return GetAvailable_Direct - Quantity.Value;
            }
        }

        public int approvedInstock
        {
            get
            {
                if (AprovalStatusID == 2)
                    return InventoryInstock - ReferenceQuantity.Value;
                else
                    return InventoryInstock;
            }
        }

        public int approvedAvailable
        {
            get
            {
                if (AprovalStatusID == 2)
                    return GetAvailable_Direct - ReferenceQuantity.Value;
                else
                    return GetAvailable_Direct;
            }
        }

    }
}


