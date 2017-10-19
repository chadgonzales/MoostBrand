using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ShiftSchedule
/// </summary>
public class ShiftSchedule : DBInterface
{
    #region properties
    public int ID { get; set; }
    public string UserID { get; set; }

    public string Code { get; set; }
    public string Description { get; set; }

    public DateTime TimeInFrom { get; set; }
    public DateTime? TimeinTo { get; set; }
    public DateTime TimeOutFrom { get; set; }
    public DateTime? TimeOutTo { get; set; }
    public DateTime? BreakStart { get; set; }
    public DateTime? BreakEnd { get; set; }

    public string ShiftCredit { get; set; }
    public string BreakCredit { get; set; }

    public int IsFlexible { get; set; }
    public int IsForceCredit { get; set; }


    //EMPLOYEE SHIFT SCHEDULE
    public string EmpID { get; set; }
    public int ShiftID { get; set; }
    public DateTime EffectivityDate { get; set; }
    public DateTime? Date { get; set; }





    #endregion

    #region public method
    public DataTable GetAllShiftSchedule()
    {
        DataTable dt = this.ExecuteRead("sp_user_shiftSchedule_list");
        return dt;
    }
    public int AddUpdateShiftSchedule()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@ID", ID);
            oparam.AddWithValue("@Code", Code);
            oparam.AddWithValue("@Description", Description);
            oparam.AddWithValue("@IsFlexible", IsFlexible);

            oparam.AddWithValue("@TimeInFrom", TimeInFrom);
            oparam.AddWithValue("@TimeinTo", (object)TimeinTo ?? DBNull.Value);//
            oparam.AddWithValue("@TimeOutFrom", TimeOutFrom);
            oparam.AddWithValue("@TimeOutTo", (object)TimeOutTo ?? DBNull.Value);
            oparam.AddWithValue("@ShiftCredit", ShiftCredit);
            
            oparam.AddWithValue("@BreakStart", (object)BreakStart ?? DBNull.Value);//
            oparam.AddWithValue("@BreakEnd", (object)BreakEnd ?? DBNull.Value);//
            oparam.AddWithValue("@BreakCredit", BreakCredit);
            oparam.AddWithValue("@IsForceCredit", IsForceCredit);
            oparam.AddWithValue("@UserID", UserID);

            _rowsAffected = this.ExecuteCUD("sp_user_shiftSchedule_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public DataTable CheckShiftScheduleIfExists()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@id", ID);
        oparam.AddWithValue("@Code", Code);

        DataTable dt = this.ExecuteRead("sp_user_shiftSchedule_checkIfExists", oparam);

        return dt;
    }

    //EMPLOYEE SHIFT SCHEDULE
    public int AddUpdateShiftScheduleTagging()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@ID", ID);
            oparam.AddWithValue("@EmpID", EmpID);
            oparam.AddWithValue("@ShiftID", ShiftID);
            oparam.AddWithValue("@EffectivityDate", EffectivityDate);
            oparam.AddWithValue("@UserID", UserID);

            _rowsAffected = this.ExecuteCUD("sp_user_shiftScheduleTagging_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public DataTable CheckShiftScheduleTaggingIfExists()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@ID", ID);
        oparam.AddWithValue("@EmpID", EmpID);
        oparam.AddWithValue("@ShiftID", ShiftID);
        oparam.AddWithValue("@EffectivityDate", EffectivityDate);

        DataTable dt = this.ExecuteRead("sp_user_shiftScheduleTagging_checkIfExists", oparam);

        return dt;
    }
    public DataTable GetAllShiftScheduleTagging()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@EmpID", EmpID);
        DataTable dt = this.ExecuteRead("sp_user_shiftScheduleTagging_list_perEmployee", oparam);
        return dt;
    }


    //GET SHIFT PER EMPLOYEE PER DATE
    public DataTable GetShiftPerEmployeePerDate()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@EmpID", EmpID);
        oparam.AddWithValue("@EffectivityDate", EffectivityDate);

        DataTable dt = this.ExecuteRead("sp_user_shift_perEmployee_perDate", oparam);
        return dt;
    }
    public DataTable GetShiftSchedPerEmployeePerDate()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@EmpID", EmpID);
        oparam.AddWithValue("@EffectivityDate", EffectivityDate);

        DataTable dt = this.ExecuteRead("sp_user_shiftSched_perEmployee_perDate", oparam);
        return dt;
    }
    //GET LEAVE per employee per date
    public DataTable GetLeavePerEmployeePerDate()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@EmpID", EmpID);
        oparam.AddWithValue("@EffectivityDate", EffectivityDate);

        DataTable dt = this.ExecuteRead("sp_user_leave_perEmployee_perDate", oparam);
        return dt;
    }
    //GET OT per employee per date
    public DataTable GetOTPerEmployeePerDate()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@EmpID", EmpID);
        oparam.AddWithValue("@EffectivityDate", EffectivityDate);

        DataTable dt = this.ExecuteRead("sp_user_OT_perEmployee_perDate", oparam);
        return dt;
    }
    //GET DTR per employee per date
    public DataTable GetDTRPerEmployeePerDate()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@EmpID", EmpID);
        oparam.AddWithValue("@EffectivityDate", EffectivityDate);

        DataTable dt = this.ExecuteRead("sp_user_dtr_perEmployee_perDate", oparam);
        return dt;
    }


    //IMPORT SHIFT SCHEDULE
    public int SaveImportShiftSchedule()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@EmpID", EmpID);
            oparam.AddWithValue("@Code", Code);
            oparam.AddWithValue("@Date", (object)Date ?? DBNull.Value);
            oparam.AddWithValue("@UserID", UserID);

            _rowsAffected = this.ExecuteCUD("sp_user_shiftSchedule_import", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public int SaveShiftScheduleRequest()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@ID", ID);
            oparam.AddWithValue("@EmpID", EmpID);
            oparam.AddWithValue("@CreditDate", Date);
            oparam.AddWithValue("@ShiftID", ShiftID);
            oparam.AddWithValue("@UserID", UserID);

            _rowsAffected = this.ExecuteCUD("sp_user_shiftSchedule_save", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public DataTable GetAllShiftScheduleRequest(string _usertype, string _userid)
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@usertype", _usertype);
        oparam.AddWithValue("@userid", _userid);

        DataTable dt = this.ExecuteRead("sp_user_changeScheduleRequests_list", oparam);
        return dt;
    }
    #endregion
}