using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL

{
    public class ReqDetails
    {
        public Requisition Requisitions { get; set; }

        private MoostBrandEntities db = new MoostBrandEntities();

        int _instock, _ordered, _committed;
        public int getCommited(int itemID)
        {
            var type = new int[] { 2, 3, 4 }; // SABI ni maam carlyn iadd daw ang Branch and Warehouse
            int c = 0;
            var com = db.RequisitionDetails.Where(model => model.ItemID == itemID && model.AprovalStatusID == 2 && type.Contains( model.Requisition.RequisitionTypeID.Value) );
            var committed = com.Sum(x => x.Quantity);
            c = Convert.ToInt32(committed);
            if (committed == null)
            {
                c = 0;
            }

            _committed = c;
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
            _ordered = po;
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
            _instock = getIS;
            return getIS;
        }

        public int Available
        { get { return (_instock + _ordered) - _committed; } }
    }

  
}