using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Users : DBInterface
{
    #region properties
    public int ID { get; set; }
    public string EmpID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int UsertypeID { get; set; }
    public int Active { get; set; }
    #endregion


    #region public method
    public DataTable Authenticate()
    {
        SqlParameterCollection oParamLocal = new SqlCommand().Parameters;
        oParamLocal.AddWithValue("@username", Username);
        oParamLocal.AddWithValue("@password", Password);
        DataTable dt = this.ExecuteRead("sp_user_authenticate", oParamLocal);

        return dt;
    }
    public DataTable GetAllUsers()
    {
        DataTable dt = this.ExecuteRead("sp_user_list");
        return dt;
    }
    public DataTable GetAllUserTypes()
    {
        DataTable dt = this.ExecuteRead("sp_usertype_list");
        return dt;
    }
    public DataTable EmployeeListPerUserType()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@usertypeID", UsertypeID);
        DataTable _dt = this.ExecuteRead("sp_user_list_perType", oparam);
        return _dt;
    }
    public int UpdateUsertypes()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@empId", EmpID);
            oparam.AddWithValue("@usertypeId", UsertypeID);
            _rowsAffected = this.ExecuteCUD("sp_user_usertype_update", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public int ChangePassword()
    {
        int _rowsAffected = 0;
        SqlParameterCollection oParamLocal = new SqlCommand().Parameters;
        oParamLocal.AddWithValue("@empID", EmpID);
        oParamLocal.AddWithValue("@password", Password);
        _rowsAffected = this.ExecuteCUD("sp_user_change_password", oParamLocal);

        return _rowsAffected;
    }
    #endregion


    #region private method

    #endregion


}
