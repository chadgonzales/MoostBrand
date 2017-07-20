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

        public int getCommited(string code)
        {
            var r = entity.Requisitions.Where(p => p.ApprovalStatus == 2).Select(p => p.ID).ToList();
            var type = new int[] { 2, 3, 4 }; // SABI ni maam carlyn iadd daw ang Branch and Warehouse
            int c = 0;
            var com = entity.RequisitionDetails.Where(model => model.ItemCode == code && model.AprovalStatusID == 2 && type.Contains(model.Requisition.RequisitionTypeID) && r.Contains(model.RequisitionID));
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
            var r = entity.Requisitions.Where(p => p.ApprovalStatus == 2 && p.LocationID == location).Select(p => p.ID).ToList();
            var type = new int[] { 2, 3, 4 }; // SABI ni maam carlyn iadd daw ang Branch and Warehouse
            int c = 0;
            var com = entity.RequisitionDetails.Where(model => model.ItemCode == code && model.AprovalStatusID == 2 && type.Contains(model.Requisition.RequisitionTypeID) && r.Contains(model.RequisitionID));
            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;



            return _committed;
        }

        public int getPurchaseOrder(string code)
        {
            var requi = entity.RequisitionDetails.FirstOrDefault(x => x.AprovalStatusID == 2);
            int po = 0;
            if (requi != null)
            {
                var lstReqDetail = new List<RequisitionDetail>();

                lstReqDetail = entity.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 1 && x.AprovalStatusID == 2 && x.ItemCode == code && x.Requisition.LocationID == requi.Requisition.LocationID).ToList();

                po = lstReqDetail.Sum(x => x.Quantity) ?? 0;
            }
            return po;
        }
        public int getPurchaseOrderReceiving(int location ,string code) 
        {

            var requi = entity.RequisitionDetails.Where(x => x.AprovalStatusID == 2 && x.Requisition.ReqTypeID == 1
                                                                                    && x.Requisition.RequisitionTypeID == 1
                                                                                    && x.Requisition.LocationID == location
                                                                                    && x.Requisition.Status == false
                                                                                    && x.ItemCode == code);
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
                loc = requi.LocationID;
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

            var st = entity.StockTransfers.Where(p => p.ApprovedStatus == 2).Select(p => p.ID).ToList();
            int c = 0;
            var com = entity.StockTransferDetails.Where(model => model.RequisitionDetail.ItemCode == code && model.AprovalStatusID == 2 && st.Contains(model.StockTransferID.Value));
            var committed = com.Sum(x => x.Quantity);
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

            var st = entity.StockTransfers.Where(p => p.ApprovedStatus == 2).Select(p => p.ID).ToList();
            int c = 0;
            var com = entity.StockTransferDetails.Where(model => model.RequisitionDetail.ItemCode == code && model.AprovalStatusID == 2 && st.Contains(model.StockTransferID.Value));
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