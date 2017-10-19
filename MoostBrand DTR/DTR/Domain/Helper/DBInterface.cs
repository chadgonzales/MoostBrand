using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

/// <summary>
/// Summary description for DBInterface
/// </summary>
public class DBInterface
{
    #region "variables"
    public SqlParameterCollection param = new SqlCommand().Parameters;
    public DataTable dt = new DataTable();
    
    private string sErrMessage = string.Empty;
    #endregion
    #region "properties"
    public string ErrorMessage
    {
        get { return sErrMessage; }
    }
    #endregion
    #region "public methods"

    public virtual string GetConnString()
    {
        var userPath = Environment.GetFolderPath(Environment
                                             .SpecialFolder.ApplicationData);
        var filename = Path.Combine(userPath, "mysettings.txt");

        // Read connection string
        var connectionString = File.ReadAllText(filename);

        return connectionString;
        //return ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
    }

    public SqlDataReader FReadData(string spString)
    {
        SqlDataReader oRd = null;
        try
        {
            string cnnStr = GetConnString();
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                SqlCommand cmd = new SqlCommand(spString, cnn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 3000000;
                cnn.Open();
                oRd = cmd.ExecuteReader();
                cnn.Close();
            }
            return oRd;
        }
        catch (SqlException saerr)
        {
            sErrMessage = "SQL Error:  " + saerr.Message;
            return oRd;
        }

        catch (Exception err)
        {
            sErrMessage = " Runtime Error: " + err.Message;
            return oRd;
        }
    }

    public DataTable FReadDataTable(string spString)
    {
        SqlDataReader oRd = null;
        DataTable dt = new DataTable();
        try
        {
            string cnnStr = GetConnString();
            using (SqlConnection cnn = new SqlConnection(cnnStr))
            {
                SqlCommand cmd = new SqlCommand(spString, cnn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 3000000;
                cnn.Open();
                oRd = cmd.ExecuteReader();
                dt.Load(oRd);
                cnn.Close();
            }
            return dt;
        }
        catch (SqlException saerr)
        {
            sErrMessage = "SQL Error:  " + saerr.Message;
            return dt;
        }

        catch (Exception err)
        {
            sErrMessage = " Runtime Error: " + err.Message;
            return dt;
        }
    }

    protected int ExecuteCUD(string sProc, SqlParameterCollection oArrParam)
    {
        int iRecordsAffected = 0;

        string cnnStr = GetConnString();
        using (SqlConnection cnn = new SqlConnection(cnnStr))
        {
            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sProc, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000000;
                foreach (SqlParameter oParam in oArrParam)
                {
                    cmd.Parameters.Add(oParam.ParameterName, oParam.SqlDbType).Value = oParam.Value;
                }
                iRecordsAffected = cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (SqlException sqlerr)
            {
                sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
            }
            catch (Exception err)
            {
                sErrMessage = " Runtime Error: " + err.Message;
            }
        }

        return iRecordsAffected;
    }
    protected int ExecuteCUD(string sProc)
    {
        int iRecordsAffected = 0;

        string cnnStr = GetConnString();
        using (SqlConnection cnn = new SqlConnection(cnnStr))
        {
            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sProc, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000000;
                iRecordsAffected = cmd.ExecuteNonQuery();
                cnn.Close();
            }
            catch (SqlException sqlerr)
            {
                sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
            }
            catch (Exception err)
            {
                sErrMessage = " Runtime Error: " + err.Message;
            }
        }

        return iRecordsAffected;
    }
    protected int ExecuteInsertWithIdentity(string sProc, SqlParameterCollection oArrParam)
    {
        int iReturnIdentity = 0;

        string cnnStr = GetConnString();
        using (SqlConnection cnn = new SqlConnection(cnnStr))
        {
            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sProc, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000000;
                foreach (SqlParameter oParam in oArrParam)
                {
                    cmd.Parameters.Add(oParam.ParameterName, oParam.SqlDbType).Value = oParam.Value;
                }
                iReturnIdentity = Convert.ToInt32(cmd.ExecuteScalar());
                cnn.Close();
            }
            catch (SqlException sqlerr)
            {
                sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
            }
            catch (Exception err)
            {
                sErrMessage = " Runtime Error: " + err.Message;
            }
        }

        return iReturnIdentity;
    }
    protected DataTable ExecuteRead(string sProc, SqlParameterCollection oArrParam)
    {
        DataTable dt = new DataTable();

        string cnnStr = GetConnString();
        using (SqlConnection cnn = new SqlConnection(cnnStr))
        {
            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sProc, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000000;
                foreach (SqlParameter oParam in oArrParam)
                {
                    cmd.Parameters.Add(oParam.ParameterName, oParam.SqlDbType).Value = oParam.Value;
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.Close();
            }
            catch (SqlException sqlerr)
            {
                sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
            }
            catch (Exception err)
            {
                sErrMessage = " Runtime Error: " + err.Message;
            }
        }

        return dt;
    }

    protected DataTable ExecuteRead(string sProc)
    {
        DataTable dt = new DataTable();

        string cnnStr = GetConnString();
        using (SqlConnection cnn = new SqlConnection(cnnStr))
        {
            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sProc, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000000;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                cnn.Close();
            }
            catch (SqlException sqlerr)
            {
                sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
            }
            catch (Exception err)
            {
                sErrMessage = " Runtime Error: " + err.Message;
            }
        }

        return dt;
    }
    protected int ExecuteScalar(string sProc, SqlParameterCollection oArrParam)
    {
        int _return = 0;

        string cnnStr = GetConnString();
        using (SqlConnection cnn = new SqlConnection(cnnStr))
        {
            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sProc, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000000;
                foreach (SqlParameter oParam in oArrParam)
                {
                    cmd.Parameters.Add(oParam.ParameterName, oParam.SqlDbType).Value = oParam.Value;
                }

                _return = Convert.ToInt32(cmd.ExecuteScalar());
                cnn.Close();
            }
            catch (SqlException sqlerr)
            {
                sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
            }
            catch (Exception err)
            {
                sErrMessage = " Runtime Error: " + err.Message;
            }
        }

        return _return;
    }
    protected int ExecuteScalar(string sProc)
    {
        int _return = 0;

        string cnnStr = GetConnString();
        using (SqlConnection cnn = new SqlConnection(cnnStr))
        {
            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sProc, cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 3000000;
                _return = Convert.ToInt32(cmd.ExecuteScalar());
                cnn.Close();
            }
            catch (SqlException sqlerr)
            {
                sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
            }
            catch (Exception err)
            {
                sErrMessage = " Runtime Error: " + err.Message;
            }
        }

        return _return;
    }

    public SqlDataAdapter DataAdapt(string strProc, SqlParameterCollection spcPara)
    {
        SqlDataAdapter da = new SqlDataAdapter();
        try
        {
            SqlConnection con = new SqlConnection(GetConnString());
            SqlCommand cmd = new SqlCommand(strProc, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 30000000;
            foreach (SqlParameter oParam in spcPara)
            {
                cmd.Parameters.Add(oParam.ParameterName, oParam.SqlDbType).Value = oParam.Value;
            }

            da = new SqlDataAdapter(cmd);
            cmd.Dispose();
            cmd = null;
        }
        catch (Exception)
        {
            da = null;
        }
        return da;
    }
    //public void bulkInsert(DataTable _dtCustInfo)
    //{
    //    string cnnStr = GetConnString();
    //    using (SqlConnection cnn = new SqlConnection(cnnStr))
    //    {
    //        try
    //        {
    //            cnn.Open();
    //            SqlBulkCopy objSBC = new SqlBulkCopy(cnn);
    //            // Specify the destination table
    //            objSBC.DestinationTableName = "tb_customers";
    //            // Write the data to the SQL Server
    //            objSBC.WriteToServer(_dtCustInfo);
    //            cnn.Close();
    //        }
    //        catch (SqlException sqlerr)
    //        {
    //            sErrMessage = "SQL Error: Number - " + sqlerr.Number + ", " + sqlerr.Message;
    //        }
    //        catch (Exception err)
    //        {
    //            sErrMessage = " Runtime Error: " + err.Message;
    //        }
    //    }
    //}
    #endregion
    #region "private methods"
    #endregion
}