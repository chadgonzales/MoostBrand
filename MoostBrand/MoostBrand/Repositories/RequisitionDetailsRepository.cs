using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL

{
    public class RequisitionDetailsRepository
    {
        private MoostBrandEntities entity = new MoostBrandEntities();

        public int getCommited(int reservationId, int itemID)
        {
            var requi = entity.Requisitions.Find(reservationId);
            //var requi = entity.RequisitionDetails.FirstOrDefault(x => x.AprovalStatusID == 2);

            int total = 0;
            if (requi != null)
            {
                var lstReqDetail = new List<RequisitionDetail>();

                lstReqDetail = entity.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 4 && x.ItemID == itemID && x.AprovalStatusID == 2 && x.Requisition.LocationID == requi.LocationID).ToList();

                total = lstReqDetail.Sum(x => x.Quantity) ?? 0;
            }

            return total;
        }

        public int getPurchaseOrder(int itemID)
        {
            var requi = entity.RequisitionDetails.FirstOrDefault(x => x.AprovalStatusID == 2);
            int po = 0;
            if (requi != null)
            {
                var lstReqDetail = new List<RequisitionDetail>();

                lstReqDetail = entity.RequisitionDetails.Where(x => x.Requisition.RequisitionTypeID == 1 && x.AprovalStatusID == 2 && x.ItemID == itemID && x.Requisition.LocationID == requi.Requisition.LocationID).ToList();

                po = lstReqDetail.Sum(x => x.Quantity) ?? 0;
            }
            return po;
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


    }
}