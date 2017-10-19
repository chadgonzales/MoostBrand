using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using DTR;

namespace Syncker
{
    public class EmployeeRegistrationSynchronizer
    {
        RegistrationService.RegistrationServiceSoapClient regService = new RegistrationService.RegistrationServiceSoapClient();
        EmployeeRegistrationRepo empRegRepo = new EmployeeRegistrationRepo();

        public void Synchronize()
        {
            //Get list of unsynchronized registration from local
            List<EmployeeRegistration> lstRegistration = empRegRepo.List();

            //Initialize registration web service from main
            RegistrationService.RegistrationServiceSoapClient regServiceMain = new RegistrationService.RegistrationServiceSoapClient();

            foreach (EmployeeRegistration _reg in lstRegistration)
            {
                int isSyncSuccessful;
                isSyncSuccessful = regServiceMain.Synchronize(JsonConvert.SerializeObject(_reg));
                if (isSyncSuccessful == 1)
                {
                    empRegRepo.Enroll(_reg);
                }
            }
        }
    }
}
