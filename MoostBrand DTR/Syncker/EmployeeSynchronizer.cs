using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using DTR;

namespace Syncker
{
    class EmployeeVM {
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
    }

    class EmployeeSynchronizer
    {
        MyServiceReferences.EmployeeServiceSoapClient empService = new MyServiceReferences.EmployeeServiceSoapClient();
        EmployeeRepo empRepo = new EmployeeRepo();

        //Synchronize employees from main to local
        public void Synchronize()
        {
            //Get list of employees from main
            List<EmployeeVM> lstEmployeeMain = JsonConvert.DeserializeObject<List<EmployeeVM>>(empService.GetEmployees());
            
            foreach (EmployeeVM emp in lstEmployeeMain)
            {
                Employee employee = new Employee
                {
                    EMPID = emp.EmployeeID,
                    FirstName = emp.FirstName,
                    MiddleName = emp.MiddleName,
                    LastName = emp.LastName
                };

                //Save emmployee to local
                empRepo.Save(employee);
            }
        }
    }
}