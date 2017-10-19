using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LeaveManagement
/// </summary>
public class LeaveManagement : DBInterface
{
    #region properties
    public int ID { get; set; }
    public string UserID { get; set; }
    public int LeaveTypeID { get; set; }
    public string LeaveTypeCode { get; set; }
    public string LeaveTypeDesc { get; set; }
    public int Active { get; set; }

    public string EmpID { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime EndDate { get; set; }
    public int LeaveCredit { get; set; }

    public int FormCredit { get; set; }
    public string Reason { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string ApprovedBy { get; set; }

    public decimal NoOfHours { get; set; }
    public string LeaveStatus { get; set; }

    #endregion


    #region public method
    public DataTable GetAllLeaveTypes()
    {
        DataTable dt = this.ExecuteRead("sp_user_leavetype_list");
        return dt;
    }
    public DataTable CheckLeaveTypesIfExists()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@id", LeaveTypeID);
        oparam.AddWithValue("@code", LeaveTypeCode);
        //oparam.AddWithValue("@desc", LeaveTypeDesc);

        DataTable dt = this.ExecuteRead("sp_user_leavetype_checkIfExists", oparam);

        return dt;
    }
    public int AddUpdateLeaveTypes()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@id", LeaveTypeID);
            oparam.AddWithValue("@code", LeaveTypeCode);
            oparam.AddWithValue("@desc", LeaveTypeDesc);

            _rowsAffected = this.ExecuteCUD("sp_user_leavetype_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }


    //LEAVE CREDIT
    public DataTable CheckLeaveCreditIfExists()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@id", ID);
        oparam.AddWithValue("@empid", EmpID);
        oparam.AddWithValue("@leaveID", LeaveTypeID);
        oparam.AddWithValue("@dateStart", DateStart);
        oparam.AddWithValue("@endDate", EndDate);

        DataTable dt = this.ExecuteRead("sp_user_leaveCredit_checkIfExists", oparam);

        return dt;
    }
    public int AddUpdateLeaveCredits()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@id", ID);
            oparam.AddWithValue("@empid", EmpID);
            oparam.AddWithValue("@leaveID", LeaveTypeID);
            oparam.AddWithValue("@dateStart", DateStart);
            oparam.AddWithValue("@endDate", EndDate);
            oparam.AddWithValue("@leaveCredit", LeaveCredit);

            _rowsAffected = this.ExecuteCUD("sp_user_leaveCredit_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public DataTable GetAllLeaveCredits()
    {
        DataTable dt = this.ExecuteRead("sp_user_leaveCredits_list");
        return dt;
    }

    //LEAVE APPLICATION
    public DataTable CheckLeaveApplicationIfExists()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@id", ID);
        oparam.AddWithValue("@empid", EmpID);
        oparam.AddWithValue("@leaveID", LeaveTypeID);
        oparam.AddWithValue("@dateStart", DateStart);
        oparam.AddWithValue("@endDate", EndDate);

        DataTable dt = this.ExecuteRead("sp_user_leaveApplication_checkIfExists", oparam);

        return dt;
    }
    public int AddUpdateLeaveApplications()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@id", ID);
            oparam.AddWithValue("@empid", EmpID);
            oparam.AddWithValue("@leaveID", LeaveTypeID);
            oparam.AddWithValue("@dateStart", DateStart);
            oparam.AddWithValue("@endDate", EndDate);
            oparam.AddWithValue("@reason", Reason);
            oparam.AddWithValue("@noOfHours", NoOfHours);
            oparam.AddWithValue("@approvedBy", ApprovedBy);
            oparam.AddWithValue("@approvedDate", ApprovedDate);
            oparam.AddWithValue("@leaveStatus", LeaveStatus);
            oparam.AddWithValue("@UserID", UserID);

            
            _rowsAffected = this.ExecuteCUD("sp_user_leaveApplication_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public DataTable GetAllLeaveApplication(string _usertype, string _userid)
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@usertype", _usertype);
        oparam.AddWithValue("@userid", _userid);
        DataTable dt = this.ExecuteRead("sp_user_leaveApplication_list", oparam);
        return dt;
    }
    public int ApproveLeaveRequest(string _reasons)
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@ID", ID);
            oparam.AddWithValue("@UserID", UserID);
            oparam.AddWithValue("@reasons", _reasons);
            _rowsAffected = this.ExecuteCUD("sp_user_leaveRequests_updateStatus", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public DataTable GetLeaveReport(string empID, string leaveType, DateTime dtFrom, DateTime dtTo)
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@empID", empID);
        oparam.AddWithValue("@leaveType", leaveType);
        oparam.AddWithValue("@dtFrom", dtFrom);
        oparam.AddWithValue("@dtTo", dtTo);

        DataTable dt = this.ExecuteRead("sp_report_leave", oparam);
        return dt;
    }
    #endregion


    #region private method

    #endregion


}