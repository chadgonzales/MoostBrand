using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL

{
    public class ReceivingRepository
    {
        private MoostBrandEntities entity = new MoostBrandEntities();

        int _instock, _ordered, _committed;

       

        public int getReceiving(int reqID)
        {
          
            int c = 0;
            var com = entity.ReceivingDetails.Where(model => model.RequisitionDetailID == reqID && model.AprovalStatusID == 2 && model.AprovalStatusID == 5);
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