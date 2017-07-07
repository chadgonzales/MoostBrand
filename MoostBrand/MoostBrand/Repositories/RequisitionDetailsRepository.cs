using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL

{
    public class RequisitionDetailsRepository
    {
        private MoostBrandEntities db = new MoostBrandEntities();

        public int getCommited(int itemID)
        {
            int c = 0;
            var com = db.RequisitionDetails.Where(model => model.ItemID == itemID && model.AprovalStatusID == 2 && model.Requisition.RequisitionTypeID == 4);
            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }
            return c;
        }

        public int getPurchaseOrder(int itemID)
        {
            int po = 0;
            var pur = db.RequisitionDetails.Where(model => model.Requisition.RequisitionTypeID == 1 && model.AprovalStatusID == 2 && model.ItemID == itemID);
            var porder = pur.Sum(x => x.Quantity);
            po = Convert.ToInt32(porder);
            if (porder == null)
            {
                po = 0;
            }
            return po;
        }

        public int getInstocked(string description)
        {
            int getIS = 0;
            var query = db.Inventories.FirstOrDefault(x => x.Description == description);
            if (query != null)
            {
                getIS = Convert.ToInt32(query.InStock);
            }
            return getIS;
        }


    }
}