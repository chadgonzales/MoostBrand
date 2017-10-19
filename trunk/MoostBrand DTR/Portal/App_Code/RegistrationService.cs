using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;

/// <summary>
/// Summary description for RegistrationService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class RegistrationService : System.Web.Services.WebService {

    public RegistrationService () {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public int Synchronize(string _log)
    {
        //Get logs from local
        EmployeeRegistration empReg = JsonConvert.DeserializeObject<EmployeeRegistration>(_log);

        //Insert logs to main
        return empReg.Enroll(empReg);
    }


}