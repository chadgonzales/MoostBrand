using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL

{
    public class StockAdjustmentRepository
    {
        private MoostBrandEntities entity = new MoostBrandEntities();

        public IQueryable<StockAdjustment> List()
        {
            return entity.StockAdjustments;
        }


        public string GeneratePoNumber()
        {
          
            //get last id
            int lastId = 1;
            int cnt = List().Count();
            if (cnt > 0)
            {
                lastId = cnt + 1;
            }

       
            string Number = lastId.ToString().PadLeft(4, '0');

            bool poExist = entity.StockAdjustments.Count(p => p.No == Number) > 0;

            while (poExist)
            {
                if (cnt > 0)
                {
                    lastId = lastId + 1;
                    Number = lastId.ToString().PadLeft(4, '0');
                    poExist = entity.StockAdjustments.Count(p => p.No == Number) > 0;
                }

            }

            return Number;
        }


        public int GetInventoryQuantity(int ItemID)
        {
            int Number = 0;

            Number = entity.Inventories.Find(ItemID).InStock.Value;

            return Number;
        }

        public string GetInventoryDescription(int ItemID)
        {
            string name = "";

            name = entity.Inventories.Find(ItemID).Description;

            return name;
        }



    }
}