using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTR
{
    public class Employee
    {
        public string EMPID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }

        public string name {
            get {
                return Common.FirstCharToUpper(LastName) + ", " +
                    Common.FirstCharToUpper(FirstName) + " " +
                    Common.FirstCharToUpper(MiddleName); 
            }
        }

        public string ImagePath{
            get {
                EmployeeRegistrationRepo _empRegRepo = new EmployeeRegistrationRepo();
                EmployeeRegistration _empReg = _empRegRepo.GetByEmployeeId(EMPID);
                if (_empReg != null)
                {
                    return _empReg.ImagePath;
                }
                else {
                    return "";
                }
            }
        }
    }
}
