using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Featured
/// </summary>
public class EmployeeFile : DBInterface, IPageable
{
    #region "Variables"
    int _id = 0;
    string _empId = string.Empty;
    string _firstName = string.Empty;
    string _middleName = string.Empty;
    string _lastName = string.Empty;
    string _email = string.Empty;
    string _payBasis = string.Empty;

    int _uploadID = 0;
    string _type = string.Empty;
    string _fileName = string.Empty;
    string _origFileName = string.Empty;
    DateTime _dteCreated;
    int _active = 0;
    int _userId = 0;


    public DateTime? TimeIn;
    public DateTime? TimeOut;


    string _password = string.Empty;
    #endregion

    #region properties
    public int ID
    {
        set { _id = value; }
        get { return _id; }
    }
    public string EmployeeID
    {
        set { _empId = value; }
        get { return _empId; }
    }
    public string FirstName
    {
        set { _firstName = value; }
        get { return _firstName; }
    }
    public string MiddleName
    {
        set { _middleName = value; }
        get { return _middleName; }
    }
    public string LastName
    {
        set { _lastName = value; }
        get { return _lastName; }
    }
    public string Email
    {
        set { _email = value; }
        get { return _email; }
    }
    public string PayBasis
    {
        set { _payBasis = value; }
        get { return _payBasis; }
    }
    public int Active
    {
        set { _active = value; }
        get { return _active; }
    }
    public string OrigFileName
    {
        set { _origFileName = value; }
        get { return _origFileName; }
    }
    public string FileName
    {
        set { _fileName = value; }
        get { return _fileName; }
    }
    public string Type
    {
        set { _type = value; }
        get { return _type; }
    }
    public DateTime DateCreated
    {
        set { _dteCreated = value; }
        get { return _dteCreated; }
    }
    public int UserId
    {
        set { _userId = value; }
        get { return _userId; }
    }
    public string Password
    {
        set { _password = value; }
        get { return _password; }
    }

    #endregion


    #region public method


    DataTable IPageable.ListPerPage(string _search, int _start, int _end, params string[] _custom)
    {

        string query = string.Format(
            "WITH Temp_Table as ( " +
            "SELECT UP.id,UP.filename,UP.dateUploaded,username,origFile" +
            ",row_number() OVER (order by UP.id DESC) as rowNum " +
            "FROM uploads AS UP LEFT JOIN users AS U ON(UP.userId = U.id) WHERE " +
            "UP.type = 'employee' AND " +
            "(UP.origFile LIKE {0} " +
            "OR username LIKE {0} " +
            ") AND type = 'employee')" +
            "SELECT * FROM Temp_Table " +
            "WHERE rowNum Between {1} and {2}", "'%" + _search + "%'", _start, _end);

        DataTable _dt = this.FReadDataTable(query);

        return _dt;
    }

    int IPageable.TotalRows(string _search, params string[] _custom)
    {
        string query = string.Format(
            "SELECT COUNT(UP.id) " +
            "FROM uploads AS UP LEFT JOIN users AS U ON(UP.userId = U.id) WHERE " +
            "(UP.origFile LIKE {0} " +
            "OR username LIKE {0} " +
            ") ", "'%" + _search + "%'" + 
            "AND type = 'employee'"
            );

        DataTable _dt = this.FReadDataTable(query);
        if (_dt.Rows.Count > 0)
            return Convert.ToInt32(_dt.Rows[0][0].ToString());
        else
            return 0;
    }

    DataTable IPageable.ListPerPageDTR(string _search, int _start, int _end, params string[] _custom)
    {

        string query = string.Format(
            "WITH Temp_Table as ( " +
            "SELECT UP.id,UP.filename,UP.dateUploaded,username,origFile, status" +
            ",row_number() OVER (order by UP.id DESC) as rowNum " +
            "FROM uploads AS UP LEFT JOIN users AS U ON(UP.userId = U.id) WHERE " +
            "UP.type = 'dtr' AND " +
            "(UP.origFile LIKE {0} " +
            "OR username LIKE {0} " +
            ")AND type = 'dtr')" +
            "SELECT * FROM Temp_Table " +
            "WHERE rowNum Between {1} and {2}", "'%" + _search + "%'", _start, _end);

        DataTable _dt = this.FReadDataTable(query);

        return _dt;
    }

    int IPageable.TotalRowsDTR(string _search, params string[] _custom)
    {
        string query = string.Format(
            "SELECT COUNT(UP.id) " +
            "FROM uploads AS UP LEFT JOIN users AS U ON(UP.userId = U.id) WHERE " +
            "(UP.origFile LIKE {0} " +
            "OR username LIKE {0} " +
            ") ", "'%" + _search + "%'"+
            "AND type = 'dtr'"
            );

        DataTable _dt = this.FReadDataTable(query);
        if (_dt.Rows.Count > 0)
            return Convert.ToInt32(_dt.Rows[0][0].ToString());
        else
            return 0;
    }

    public DataTable ActiveList()
    {
        DataTable _dt = this.ExecuteRead("sp_employee_active_list");
        return _dt;
    }

    public bool Exist()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@id", _id);
        oparam.AddWithValue("@empId", _empId);

        DataTable _dt = this.ExecuteRead("sp_employee_validate_empId", oparam);
        if (_dt.Rows.Count > 0)
            return true;
        return false;
    }

    public int Add()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@empId", _empId);
            oparam.AddWithValue("@firstName", _firstName);
            oparam.AddWithValue("@middleName", _middleName);
            oparam.AddWithValue("@lastName", _lastName);
            oparam.AddWithValue("@email", _email);
            oparam.AddWithValue("@active", _active);

            _rowsAffected = this.ExecuteCUD("sp_employee_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }


    public int Update()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@id", _id);
            oparam.AddWithValue("@empId", _empId);
            oparam.AddWithValue("@firstName", _firstName);
            oparam.AddWithValue("@middleName", _middleName);
            oparam.AddWithValue("@lastName", _lastName);
            oparam.AddWithValue("@email", _email);
            oparam.AddWithValue("@active", _active);

            _rowsAffected = this.ExecuteCUD("sp_employee_edit", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public int Remove()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@id", _id);

            _rowsAffected = this.ExecuteCUD("sp_employee_delete", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public DataTable MasterFiles()
    {
        DataTable _dt = this.ExecuteRead("sp_employee_get_uploaded_files");

        return _dt;
    }
    public int AddMasterFile()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@orig", _origFileName);
            oparam.AddWithValue("@filename", _fileName);
            oparam.AddWithValue("@dateUploaded", _dteCreated);
            oparam.AddWithValue("@type", _type);
            oparam.AddWithValue("@user", _userId);

            _rowsAffected = this.ExecuteCUD("sp_employee_masterfile_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public int AddMasterFileDTR()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@orig", _origFileName);
            oparam.AddWithValue("@filename", _fileName);
            oparam.AddWithValue("@dateUploaded", _dteCreated);
            oparam.AddWithValue("@type", _type);
            oparam.AddWithValue("@user", _userId);

            _rowsAffected = this.ExecuteInsertWithIdentity("sp_employee_masterfile_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public int AddEmployeeMasterfile()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@empId", _empId);
            oparam.AddWithValue("@firstName", _firstName);
            oparam.AddWithValue("@middleName", _middleName);
            oparam.AddWithValue("@lastName", _lastName);
            oparam.AddWithValue("@password", _password);
            oparam.AddWithValue("@PayBasis", _payBasis);


            _rowsAffected = this.ExecuteCUD("sp_employee_add_masterfile", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }

    #endregion

    #region private method

    #endregion
}

