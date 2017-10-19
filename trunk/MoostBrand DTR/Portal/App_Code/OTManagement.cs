using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OTManagement
/// </summary>
public class OTManagement : DBInterface
{
    #region properties
    public int ID { get; set; }
    public string EmpID { get; set; }
    public string UserID { get; set; }

    public DateTime CreditDate { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime TimeEnd { get; set; }
    public DateTime? BreakStart { get; set; }
    public DateTime? BreakEnd { get; set; }

    public decimal TotalHrs { get; set; }
    public string Reason { get; set; }
    public string ChargeTo { get; set; }


    #endregion
    #region public method
    public DataTable GetAllOTRequests(string _usertype, string _userid)
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@usertype", _usertype);
        oparam.AddWithValue("@userid", _userid);

        DataTable dt = this.ExecuteRead("sp_user_otRequests_list", oparam);
        return dt;
    }
    public int AddUpdateOTRequest()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@ID", ID);
            oparam.AddWithValue("@EmpID", EmpID);
            oparam.AddWithValue("@CreditDate", CreditDate);
            oparam.AddWithValue("@TimeStart", TimeStart);
            oparam.AddWithValue("@TimeEnd", TimeEnd);
            oparam.AddWithValue("@BreakStart",(object)BreakStart??DBNull.Value);
            oparam.AddWithValue("@BreakEnd", (object)BreakEnd ?? DBNull.Value);
            oparam.AddWithValue("@Reason", Reason);
            oparam.AddWithValue("@TotalHrs", TotalHrs);
            oparam.AddWithValue("@ChargeTo", ChargeTo);

            oparam.AddWithValue("@UserID", UserID);
            _rowsAffected = this.ExecuteCUD("sp_user_otrequests_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public int ApproveOTRequest(string _category, string _reasons)
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@ID", ID);
            oparam.AddWithValue("@UserID", UserID);
            oparam.AddWithValue("@category", _category);
            oparam.AddWithValue("@reasons", _reasons);


            _rowsAffected = this.ExecuteCUD("sp_user_otrequests_updateStatus", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    #endregion
}