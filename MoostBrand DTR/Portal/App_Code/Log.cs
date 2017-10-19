using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Log
/// </summary>
public class Log : DBInterface
{
    public int Id { get; set; }
    public string EmpId { get; set; }
    public DateTime ScanDate { get; set; }
    public string Location { get; set; }
    public bool Sent { get; set; }
    public bool LogType { get; set; }

    #region special method
    private Log GetByRow(DataRow row)
    {
        Log _log = new Log();

        _log.Id = Convert.ToInt32(row["EMPID"]);
        _log.EmpId = Convert.ToString(row["FName"]);
        _log.ScanDate = Convert.ToDateTime(row["MName"]);
        _log.Location = Convert.ToString(row["LName"]);
        _log.Sent = Convert.ToBoolean(row["LName"]);

        return _log;
    }
    #endregion

    public List<Log> List() {
        DataTable dt = new DataTable();
        dt = this.ExecuteRead("sp_logs_list");
        List<Log> lstLog = new List<Log>();
        foreach (DataRow row in dt.Rows) lstLog.Add(GetByRow(row));
        return lstLog;
    }

    public List<Log> ListUnsynched()
    {
        DataTable dt = new DataTable();
        dt = this.ExecuteRead("sp_logs_list_unsynched");
        List<Log> lstLog = new List<Log>();
        foreach (DataRow row in dt.Rows) lstLog.Add(GetByRow(row));
        return lstLog;
    }

    public int MarkAsSent(int logId)
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oParamLocal = new SqlCommand().Parameters;

            oParamLocal.AddWithValue("@logId", logId);

            _rowsAffected = this.ExecuteCUD("sp_log_mark_as_sent", oParamLocal);
        }
        catch { }

        return _rowsAffected;
    }

    public int Insert(Log _log) {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oParamLocal = new SqlCommand().Parameters;

            oParamLocal.AddWithValue("@empId", _log.EmpId);
            oParamLocal.AddWithValue("@scanDate", _log.ScanDate);
            oParamLocal.AddWithValue("@location", _log.Location);
            oParamLocal.AddWithValue("@logType", _log.LogType);

            _rowsAffected = ExecuteCUD("sp_log_synch", oParamLocal);
        }
        catch { }

        return _rowsAffected;
    }
}