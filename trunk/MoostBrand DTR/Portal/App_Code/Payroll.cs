using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Payroll
/// </summary>
public class Payroll:DBInterface
{
    #region properties
    public int ID { get; set; }
    public int Year { get; set; }
    public string Month { get; set; }
    public string Description { get; set; }

    public int PayrollPeriod { get; set; }

    public DateTime PayrollStart { get; set; }
    public DateTime PayrollEnd { get; set; }
    
    public string UserID { get; set; }

    public int IsActive { get; set; }


    #endregion

    #region public method

    public int AddUpdatePayrollPeriod()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@ID", ID);
            oparam.AddWithValue("@Year", Year);
            oparam.AddWithValue("@Month", Month);
            oparam.AddWithValue("@Description", Description);
            oparam.AddWithValue("@PayrollPeriod", PayrollPeriod);
            oparam.AddWithValue("@PayrollStart", PayrollStart);
            oparam.AddWithValue("@PayrollEnd", PayrollEnd);
            oparam.AddWithValue("@UserID", UserID);
            oparam.AddWithValue("@IsActive", IsActive);

            _rowsAffected = this.ExecuteCUD("sp_payrollPeriod_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }

    public DataTable CheckPayrollPeriodIfExists()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@ID", ID);
        oparam.AddWithValue("@Year", Year);
        oparam.AddWithValue("@Month", Month);
        oparam.AddWithValue("@PayrollPeriod", PayrollPeriod);
        
        DataTable dt = this.ExecuteRead("sp_user_payrollPeriod_checkIfExists", oparam);

        return dt;
    }
    public DataTable GetAllPayrollPeriod()
    {
        DataTable dt = this.ExecuteRead("sp_user_payrollPeriod_list");
        return dt;
    }
    public DataTable GetAllPayrollPeriodPerMonth()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@Year", Year);
        oparam.AddWithValue("@Month", Month);
        DataTable dt = this.ExecuteRead("sp_user_payrollPeriod_list_perMonth",oparam);
        return dt;
    }
    public DataTable GetAllPayrollPeriodActive()
    {
        DataTable dt = this.ExecuteRead("sp_user_payrollPeriod_list_active");
        return dt;
    }

    #endregion
}