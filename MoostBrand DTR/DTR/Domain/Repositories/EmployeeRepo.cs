using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DTR
{
    public class EmployeeRepo : DBInterface
    {
        #region special method
        private Employee GetByRow(DataRow row)
        {
            Employee _employee = new Employee();

            _employee.EMPID = Convert.ToString(row["EMPID"]);
            _employee.FirstName = Convert.ToString(row["FName"]);
            _employee.MiddleName = Convert.ToString(row["MName"]);
            _employee.LastName = Convert.ToString(row["LName"]);
            _employee.Suffix = Convert.ToString(row["Suffix"]);

            return _employee;
        }
        #endregion

        public Employee GetByEmployeeId(string empId)
        {
            param.Clear();
            param.AddWithValue("@EMPID", empId);
            dt = this.ExecuteRead("sp_employee_get_by_id", param);
            if (dt.Rows.Count <= 0) return null;
            return GetByRow(dt.Rows[0]);
        }

        public List<Employee> List()
        {
            dt = this.ExecuteRead("sp_employee_list");
            List<Employee> lstEmployee = new List<Employee>();
            foreach (DataRow row in dt.Rows) lstEmployee.Add(GetByRow(row));
            return lstEmployee;
        }

        public int Save(Employee employee)
        {
            int _rowsAffected = 0;

            try
            {
                SqlParameterCollection oParamLocal = new SqlCommand().Parameters;

                oParamLocal.AddWithValue("@empId", employee.EMPID);
                oParamLocal.AddWithValue("@firstName", employee.FirstName);
                oParamLocal.AddWithValue("@middleName", employee.MiddleName);
                oParamLocal.AddWithValue("@lastName", employee.LastName);
                oParamLocal.AddWithValue("@suffix", employee.Suffix.DbNullIfNull());

                _rowsAffected = this.ExecuteCUD("sp_employee_save", oParamLocal);
            }
            catch { }

            return _rowsAffected;
        }

        public int Log(string empId, bool logType)
        {
            int _rowsAffected = 0;
            try
            {
                param.Clear();
                param.AddWithValue("@empID", empId);
                param.AddWithValue("@scanDate", DateTime.Now);
                param.AddWithValue("@location", ConfigurationManager.AppSettings["Location"].ToString());
                param.AddWithValue("@sent", false);
                param.AddWithValue("@logType", logType);

                _rowsAffected = this.ExecuteCUD("sp_employee_log", param);
            }
            catch { }

            return _rowsAffected;
        }
    }
}