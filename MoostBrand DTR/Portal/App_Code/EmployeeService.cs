using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Newtonsoft.Json;

/// <summary>
/// Summary description for EmployeeService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class EmployeeService : System.Web.Services.WebService {

    public EmployeeService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    private Employee GetByRow(DataRow row)
    {
        Employee _employee = new Employee();

        //_employee.ID = Convert.ToInt32(row["empId"]);
        _employee.EmployeeID = Convert.ToString(row["EMPID"]);
        _employee.FirstName = Convert.ToString(row["FName"]);
        _employee.MiddleName = Convert.ToString(row["MName"]);
        _employee.LastName = Convert.ToString(row["LName"]);
        _employee.Suffix = Convert.ToString(row["Suffix"]);

        return _employee;
    }

    [WebMethod]
    public String GetEmployees() {
        Employee emp = new Employee();
        DataTable dt = emp.EmployeeList();

        List<Employee> lstEmployee = new List<Employee>();
        foreach (DataRow row in dt.Rows) lstEmployee.Add(GetByRow(row));
        
        return JsonConvert.SerializeObject(lstEmployee);
    }
}
