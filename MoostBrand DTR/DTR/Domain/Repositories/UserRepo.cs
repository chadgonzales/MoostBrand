using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace DTR
{
    public class UserRepo : DBInterface
    {
        public bool AuthenticateAdmin(string username, string password)
        {
            SqlParameterCollection oParamLocal = new SqlCommand().Parameters;
            oParamLocal.AddWithValue("@username", username);
            oParamLocal.AddWithValue("@password", password);

            DataTable dt = new DataTable();
            dt = this.ExecuteRead("sp_admin_authenticate", oParamLocal);

            return (dt.Rows.Count > 0);
        }
    }
}