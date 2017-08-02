using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL

{
    public class StockTransferRepository
    {
        private MoostBrandEntities entity = new MoostBrandEntities();

        int _instock, _ordered, _committed;



        public int getStockTranfer(int itemID, int _stID)
        {

            var st = entity.StockTransfers.Where(p => p.ApprovedStatus == 2 && p.ID == _stID).Select(p => p.ID).ToList();
            int c = 0;

            var com = entity.StockTransferDetails.Where(model => model.RequisitionDetail.ItemID == itemID && model.AprovalStatusID == 2
                                                                                                          && st.Contains(model.StockTransferID.Value)
                                                                                                          && model.RequisitionDetail.Requisition.Status == false);

            var committed = com.Sum(x => x.Quantity);
            if (com == null)
            {
                var com1 = entity.StockTransferDetails.Where(model => model.ReceivingDetail.RequisitionDetail.ItemID == itemID && model.AprovalStatusID == 2
                                                                                                         && st.Contains(model.StockTransferID.Value)
                                                                                                         && model.ReceivingDetail.RequisitionDetail.Requisition.Status == false);

                committed = com1.Sum(x => x.Quantity);
            }
             
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;
            return _committed;
        }


        public int getCommited_Receiving(int reservationId, int itemID)
        {
            try
            {
                var requi = entity.Receivings.Find(reservationId);


                var type = new int[] { 2, 3, 4 }; // SABI ni maam carlyn iadd daw ang Branch and Warehouse
                int c = 0;
                var com = entity.ReceivingDetails.Where(model => model.RequisitionDetail.ItemID == itemID && model.AprovalStatusID == 2
                                                                                          && type.Contains(model.RequisitionDetail.Requisition.RequisitionTypeID.Value)
                                                                                          && model.RequisitionDetail.Requisition.Status == false
                                                                                          && model.RequisitionDetail.Requisition.LocationID == requi.LocationID);
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




    }
}