using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// Summary description for EmployeeRegistration
/// </summary>
public class EmployeeRegistration : DBInterface
{
    public int Id { get; set; }
    public string EmpId { get; set; }
    public string ScanTemplate { get; set; }
    public string ImagePath { get; set; }

    public int Enroll(EmployeeRegistration _empReg)
    {
        int _rowsAffected = 0;
        try
        {
            
            param.Clear();
            param.AddWithValue("@empID", _empReg.EmpId);
            param.AddWithValue("@scanTemplate", _empReg.ScanTemplate);
            param.AddWithValue("@imagePath", _empReg.ImagePath);

            _rowsAffected = this.ExecuteCUD("sp_employee_enroll", param);
        }
        catch { }

        return _rowsAffected;
    }
}