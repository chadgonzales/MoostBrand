using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTR
{
    public class LogRepo : DBInterface
    {
        #region special method
        private Log GetByRow(DataRow row)
        {
            Log _log = new Log();

            _log.Id = Convert.ToInt32(row["id"]);
            _log.EmpId = Convert.ToString(row["empId"]);
            _log.ScanDate = Convert.ToDateTime(row["scanDate"]);
            _log.Location = Convert.ToString(row["location"]);
            _log.Sent = Convert.ToBoolean(row["sent"]);
            _log.LogType = Convert.ToBoolean(row["logType"]);

            return _log;
        }
        #endregion

        public List<Log> ListUnsynched()
        {
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

        public List<Log> ListRecent()
        {
            dt = this.ExecuteRead("sp_logs_list_recent");
            List<Log> lstLog = new List<Log>();
            foreach (DataRow row in dt.Rows) lstLog.Add(GetByRow(row));

            //NOTE: NEED TO OPTIMIZE THIS:

            return lstLog.FindAll(p => p.ScanDate.Date == DateTime.Now.Date);
        }

        public Log GetLastLogByEmployeeId(string empId) {
            param.Clear();
            param.AddWithValue("@empId", empId);
            dt = this.ExecuteRead("sp_log_get_last_log_by_employeeId", param);
            if (dt.Rows.Count <= 0) return null;
            return GetByRow(dt.Rows[0]);
        }

        //public int Insert(Log _log)
        //{
        //    int _rowsAffected = 0;
        //    try
        //    {
        //        SqlParameterCollection oParamLocal = new SqlCommand().Parameters;

        //        oParamLocal.AddWithValue("@empId", _log.EmpId);
        //        oParamLocal.AddWithValue("@scanDate", _log.ScanDate);
        //        oParamLocal.AddWithValue("@location", _log.Location);

        //        _rowsAffected = ExecuteCUD("sp_log_synch", oParamLocal);
        //    }
        //    catch { }

        //    return _rowsAffected;
        //}
    }
}
