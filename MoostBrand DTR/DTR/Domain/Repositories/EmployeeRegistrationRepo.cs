using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTR
{
    public class EmployeeRegistrationRepo : DBInterface
    {
        #region special method
        private EmployeeRegistration GetByRow(DataRow row)
        {
            EmployeeRegistration _employeeRegistration = new EmployeeRegistration();

            _employeeRegistration.Id = Convert.ToInt32(row["id"]);
            _employeeRegistration.EmpId = Convert.ToString(row["empID"]);
            _employeeRegistration.ScanTemplate = row.GetText("scanTemplate");
            _employeeRegistration.ImagePath = row.GetText("imagePath");

            return _employeeRegistration;
        }
        #endregion

        public List<EmployeeRegistration> List()
        {
            DataTable dt = this.ExecuteRead("sp_employee_registration_list");

            List<EmployeeRegistration> lstEmpRegistration = new List<EmployeeRegistration>();
            for (int cnt = 0; cnt < dt.Rows.Count; ++cnt)
            {
                lstEmpRegistration.Add(GetByRow(dt.Rows[cnt]));
            }

            return lstEmpRegistration;
        }

        public EmployeeRegistration GetByEmployeeId(string empId)
        {
            param.Clear();
            param.AddWithValue("@EMPID", empId);
            dt = this.ExecuteRead("sp_employee_registration_get_by_id", param);
            if (dt.Rows.Count <= 0) return null;
            return GetByRow(dt.Rows[0]);
        }

        public int Enroll(EmployeeRegistration _empReg)
        {
            int _rowsAffected = 0;
            try
            {
                param.Clear();
                param.AddWithValue("@empID", _empReg.EmpId);
                param.AddWithValue("@scanTemplate", _empReg.ScanTemplate.DbNullIfNull());
                param.AddWithValue("@imagePath", _empReg.ImagePath);

                _rowsAffected = this.ExecuteCUD("sp_employee_enroll", param);
            }
            catch { }

            return _rowsAffected;
        }
    }
}