using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoostBrand.DAL

{
    public class EmployeeRepository
    {
        private MoostBrandEntities entity = new MoostBrandEntities();


        public IQueryable<Employee> List()
        {
            return entity.Employees;
        }

        public string GenerateEmployeeID()
        {
          
            //get last id
            int lastId = 1;
            int cnt = List().Count();
            if (cnt > 0)
            {
                lastId = cnt + 1;
            }

       
            string Number ="MB" +lastId.ToString().PadLeft(7, '0');

            bool poExist = entity.StockAdjustments.Count(p => p.No == Number) > 0;

            while (poExist)
            {
                if (cnt > 0)
                {
                    lastId = lastId + 1;
                    Number = "MB"+lastId.ToString().PadLeft(7, '0');
                    poExist = entity.StockAdjustments.Count(p => p.No == Number) > 0;
                }

            }

            return Number;
        }





    }
}