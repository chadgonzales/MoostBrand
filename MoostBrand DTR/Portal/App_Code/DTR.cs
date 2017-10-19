using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// Summary description for DTR
/// </summary>
public class DTR:DBInterface
{
    #region properties
    public int ID { get; set; }
    public string EmpID { get; set; }

    public DateTime Date { get; set; }
    public int ShiftID { get; set; }

    public DateTime? DateIn { get; set; }
    public DateTime? DateOut { get; set; }

    public int IsProcessed { get; set; }

    public string UserID { get; set; }


    public string Leave { get; set; }
    public string RegHour { get; set; }
    public string Late { get; set; }
    public string UT { get; set; }
    public string OT { get; set; }
    public string POT { get; set; }


    #endregion

    #region public method
    public int AddUpdateDTR()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@ID", ID);
            oparam.AddWithValue("@EmpID", EmpID);
            oparam.AddWithValue("@Date", Date);
            oparam.AddWithValue("@ShiftID", ShiftID);

            oparam.AddWithValue("@DateIn", (object)DateIn ?? DBNull.Value);
            oparam.AddWithValue("@DateOut",(object)DateOut ?? DBNull.Value);

            oparam.AddWithValue("@UserID", UserID);
            
            oparam.AddWithValue("@Leave", Leave);
            oparam.AddWithValue("@RegHour", RegHour);
            oparam.AddWithValue("@Late", Late);
            oparam.AddWithValue("@UT", UT);
            oparam.AddWithValue("@OT", OT);
            oparam.AddWithValue("@POT", POT);

            _rowsAffected = this.ExecuteCUD("sp_dtr_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public int UploadDTR()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@UploadID", ID);
            oparam.AddWithValue("@EmpID", EmpID);
            oparam.AddWithValue("@Date", Date);
            oparam.AddWithValue("@DateIn", (object)DateIn ?? DBNull.Value);
            oparam.AddWithValue("@DateOut", (object)DateOut ?? DBNull.Value);
            oparam.AddWithValue("@UserID", UserID);


            _rowsAffected = this.ExecuteCUD("sp_dtr_upload", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }


    public DataTable GetUnProcessedDTR()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = this.ExecuteRead("sp_dtr_unprocessed");
        }
        catch
        {
        }
        return dt;
    }
    public DataTable GetUnProcessedDTRPerID()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@UploadID", ID);
            dt = this.ExecuteRead("sp_dtr_unprocessed_perID", oparam);
        }
        catch
        {
        }
        return dt;
    }
    public int UpdateDTRStatus()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@UploadID", ID);

            _rowsAffected = this.ExecuteCUD("sp_dtr_update_status", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public DataTable GetDTRPerID(string _payrollPeriodID)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@EmpID", EmpID);
            oparam.AddWithValue("@PayrollPeriodID", _payrollPeriodID);
            dt = this.ExecuteRead("sp_dtr_perID", oparam);
        }
        catch
        {
        }
        return dt;
    }


    //GET TIME IN TIME OUT FROM DTRLOGS(SYNC)
    public DataTable GetTimeInOutFromLogsPerEmployeeDate()
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@EmpID", EmpID);
            oparam.AddWithValue("@Date", Date);
            dt = this.ExecuteRead("sp_dtr_logs_perEmployeePerDate", oparam);
        }
        catch
        {
        }
        return dt;
    }

    #endregion
}