using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
/// <summary>
/// Summary description for LogsService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class LogsService : System.Web.Services.WebService {
    Log logRepo = new Log();

    public LogsService () {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public int Synchronize(string _log) {
        //Get logs from local
        Log log = JsonConvert.DeserializeObject<Log>(_log);
        
        //Insert logs to main
        return logRepo.Insert(log);
    }

    [WebMethod]
    public string List() {
        return JsonConvert.SerializeObject(logRepo.List());
    }
}
