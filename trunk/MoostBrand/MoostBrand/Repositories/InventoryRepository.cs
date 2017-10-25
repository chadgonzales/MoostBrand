using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL

{
    public class InventoryRepository
    {
        private MoostBrandEntities entity = new MoostBrandEntities();

        int _instock, _ordered, _committed;

        public IQueryable<Inventory> List()
        {
            return entity.Inventories;
        }

        public string getItemSalesDesc(string itemcode)
        {
            string item = entity.Items.FirstOrDefault(i => i.Code == itemcode).Description;
         
            return item;
        }

        public int getCommited(string code, int location)
        {
            int item = entity.Items.FirstOrDefault(i => i.Code == code).ID;
            var r = entity.Requisitions.Where(p => p.ApprovalStatus == 2).Select(p => p.ID).ToList();
            var type = new int[] { 2, 3, 4 }; // SABI ni maam carlyn iadd daw ang Branch and Warehouse
            int c = 0;
            var com = entity.RequisitionDetails.Where(model => model.ItemID == item && model.AprovalStatusID == 2 && type.Contains(model.Requisition.RequisitionTypeID.Value) 
                                                                                    && r.Contains(model.RequisitionID)
                                                                                    && model.Requisition.LocationID == location);
            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;



            return _committed;
        }

        public int getCommitedReceiving(int location,string code)
        {
            int item = entity.Items.FirstOrDefault(i => i.Code == code).ID;
            var r = entity.Requisitions.Where(p => p.ApprovalStatus == 2 && p.LocationID == location).Select(p => p.ID).ToList();
            var type = new int[] { 2, 3, 4 }; // SABI ni maam carlyn iadd daw ang Branch and Warehouse
            int c = 0;
            var com = entity.RequisitionDetails.Where(model => model.ItemID == item && model.AprovalStatusID == 2 && model.AprovalStatusID == 5 && type.Contains(model.Requisition.RequisitionTypeID.Value) 
                                                && r.Contains(model.RequisitionID));
            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;



            return _committed;
        }

        public int getPurchaseOrder(string code, int location)
        {
            int item = entity.Items.FirstOrDefault(i => i.Code == code).ID;
           
            var requi = entity.Requisitions.Where(p => p.ApprovalStatus == 2).Select(p => p.ID).ToList();
            int po = 0;
            if (requi != null)
            {
                var lstReqDetail = new List<RequisitionDetail>();

                lstReqDetail = entity.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 1 && x.AprovalStatusID == 2 && x.AprovalStatusID == 5 && x.ItemID == item  
                                                        && requi.Contains(x.RequisitionID) 
                                                        && x.Requisition.LocationID == location).ToList();

                po = lstReqDetail.Sum(x => x.Quantity) ?? 0;
            }
            return po;
        }
        public int getPurchaseOrderReceiving(int location ,string code) 
        {

            int item = entity.Items.FirstOrDefault(i => i.Code == code).ID;
            var requi = entity.RequisitionDetails.Where(x => x.AprovalStatusID == 2 && x.Requisition.ReqTypeID == 1
                                                                                    && x.Requisition.RequisitionTypeID == 1
                                                                                    && x.Requisition.LocationID == location
                                                                                    && x.Requisition.Status == false
                                                                                    && x.ItemID == item);
            int po = 0;
            var ordered = requi.Sum(x => x.Quantity);
            po = Convert.ToInt32(ordered);
            if (ordered == null)
            {
                po = 0;
            }
            _ordered = po;
            return _ordered;
        }



        public int getInstocked(int id, string code)
        {
            //4 - customer = committed
            //1 - purchase order = ordered

            var requi = entity.Requisitions.Find(id);

            //var requi = entity.Requisitions.FirstOrDefault(x => x.RequisitionTypeID == 4 || x.RequisitionTypeID == 1);
            var instock = entity.Inventories.FirstOrDefault(x => x.ItemCode == code && x.LocationCode == requi.LocationID);
            int total;
            if (instock != null)
            {
                total = Convert.ToInt32(instock.InStock);
            }
            else
            {
                total = 0;
            }
            return total;
        }

        public int getInstockedReceiving(int id, string code)
        {
           
            var requi = entity.Requisitions.Find(id);

            int loc = 0;
            if (requi.Destination == null)
            {
                loc = requi.LocationID.Value;
            }
            else
            {
                loc = requi.Destination.Value;
            }

            var instock = entity.Inventories.FirstOrDefault(x => x.ItemCode == code && x.LocationCode == loc);
            int total;
            if (instock != null)
            {
                total = Convert.ToInt32(instock.InStock);
            }
            else
            {
                total = 0;
            }
            return total;
        }

        public int getStockTranfer(string code)
        {
            int item = entity.Items.FirstOrDefault(i => i.Code == code).ID;
            var st = entity.StockTransfers.Where(p => p.ApprovedStatus == 2 && p.ApprovedStatus == 5).Select(p => p.ID).ToList();
            int c = 0;
            var com = entity.StockTransferDetails.Where(model => model.RequisitionDetail.ItemID == item && model.AprovalStatusID == 2 && st.Contains(model.StockTransferID.Value));
            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;



            return _committed;
        }

        public int getTotalStockTranfer(string code, int locationID,DateTime from, DateTime to)
        {
            int item = entity.Items.FirstOrDefault(i => i.Code == code).ID;
            var st = entity.StockTransfers.Where(p => p.ApprovedStatus == 2 && p.ApprovedStatus == 5 && p.LocationID == locationID && (p.STDAte>= from && p.STDAte<= to)).Select(p => p.ID).ToList();
            int c = 0;
            var com = entity.StockTransferDetails.Where(model => model.RequisitionDetail.ItemID == item && model.AprovalStatusID == 2 && st.Contains(model.StockTransferID.Value));
            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;



            return _committed;
        }

        public int getTotalVariance(int invID, int locationID, DateTime from, DateTime to)
        {
          
            var st = entity.StockAdjustments.Where(p => p.ApprovalStatus == 2 && p.ApprovalStatus == 5 && p.LocationID == locationID && (p.ErrorDate >= from && p.ErrorDate <= to)).Select(p => p.ID).ToList();
            int c = 0;
            var com = entity.StockAdjustmentDetails.Where(model => model.ItemID == invID  && st.Contains(model.StockAdjustmentID.Value));
            var committed = com.Sum(x => x.Variance);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;



            return _committed;
        }
        public int getStockTranferReceiving(string code)
        {
            int item = entity.Items.FirstOrDefault(i => i.Code == code).ID;
            var st = entity.StockTransfers.Where(p => p.ApprovedStatus == 2 && p.ApprovedStatus == 5).Select(p => p.ID).ToList();
            int c = 0;
            var com = entity.StockTransferDetails.Where(model => model.RequisitionDetail.ItemID == item && model.AprovalStatusID == 2 && st.Contains(model.StockTransferID.Value));
            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;



            return _committed;
        }



    }
}