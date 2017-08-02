using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL

{
    public class RequisitionDetailsRepository
    {
        private MoostBrandEntities entity = new MoostBrandEntities();

        int _instock, _ordered, _committed = 0;

        public int getCommited(int reservationId, int itemID)
        {
            try
            {
                var requi = entity.Requisitions.Find(reservationId);


                var type = new int[] { 2, 3, 4 }; // SABI ni maam carlyn iadd daw ang Branch and Warehouse
                int c = 0;
                var com = entity.RequisitionDetails.Where(model => model.ItemID == itemID && model.AprovalStatusID == 2
                                                                                          && type.Contains(model.Requisition.RequisitionTypeID.Value)
                                                                                          && model.Requisition.Status == false
                                                                                          && model.Requisition.LocationID == requi.LocationID);
                var committed = com.Sum(x => x.Quantity);
                c = Convert.ToInt32(committed);
                if (committed == null)
                {
                    c = 0;
                }

                _committed = c;
            }
            catch { }


            return _committed;
        }

      
        public int getReceivingCommited(int locationid, int itemID)
        {
            var type = new int[] { 2, 3, 4 }; // SABI ni maam carlyn iadd daw ang Branch and Warehouse
            int c = 0;
            var com = entity.RequisitionDetails.Where(model => model.ItemID == itemID && model.AprovalStatusID == 2 
                                                                                      && type.Contains(model.Requisition.RequisitionTypeID.Value) 
                                                                                      && model.Requisition.LocationID == locationid
                                                                                      && model.Requisition.Status == false);


            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;



            return _committed;
        }

        public int getPurchaseOrder(int locationid, int itemID)
        {
            var requi = entity.RequisitionDetails.Where(x => x.AprovalStatusID == 2 && x.Requisition.ReqTypeID == 1 
                                                                                    && x.Requisition.RequisitionTypeID == 1
                                                                                    && x.Requisition.LocationID == locationid
                                                                                    && x.Requisition.Status == false
                                                                                    && x.ItemID == itemID);
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
            int total=0;

            try
            {
                var requi = entity.Requisitions.Find(id);

                //var requi = entity.Requisitions.FirstOrDefault(x => x.RequisitionTypeID == 4 || x.RequisitionTypeID == 1);
                var instock = entity.Inventories.FirstOrDefault(x => x.ItemCode == code && x.LocationCode == requi.LocationID);

                if (instock != null)
                {
                    total = Convert.ToInt32(instock.InStock);
                }
                else
                {
                    total = 0;

                }
            }
            catch { }
            return total;
        }

        public int getInstockedReceiving(int id, string code)
        {
            //4 - customer = committed
            //1 - purchase order = ordered

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

            //var requi = entity.Requisitions.FirstOrDefault(x => x.RequisitionTypeID == 4 || x.RequisitionTypeID == 1);
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


        public int getStockTranfer(int itemID)
        {

            var st = entity.StockTransfers.Where(p => p.ApprovedStatus == 1).Select(p => p.ID).ToList();
            int c = 0;
            var com = entity.StockTransferDetails.Where(model => model.RequisitionDetail.ItemID == itemID && model.AprovalStatusID == 2 
                                                                                                          && st.Contains(model.StockTransferID.Value)
                                                                                                          && model.RequisitionDetail.Requisition.Status == false);
            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;
            return _committed;
        }

        public int getStockTranferReceiving(int locationID, int itemID)
        {

            var st = entity.StockTransfers.Where(p => p.ApprovedStatus == 1).Select(p => p.ID).ToList();
            int c = 0;
            var com = entity.StockTransferDetails.Where(model => model.RequisitionDetail.ItemID == itemID && model.AprovalStatusID == 2 
                                                                                                          && st.Contains(model.StockTransferID.Value)
                                                                                                          && model.StockTransfer.LocationID == locationID
                                                                                                          && model.RequisitionDetail.Requisition.Status == false);
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