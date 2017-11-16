using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Featured
/// </summary>
public class Employee : DBInterface, IPageable
{
    #region "Variables"
    int _id = 0;
    string _empId = string.Empty;
    string _firstName = string.Empty;
    string _middleName = string.Empty;
    string _lastName = string.Empty;
    string _suffix = string.Empty;
    string _paybasis = string.Empty;
    string _email = string.Empty;
    int _uploadID = 0;
    string _type = string.Empty;
    string _fileName = string.Empty;
    string _origFileName = string.Empty;
    DateTime _dteCreated;
    int _active = 0;
    string _userId = string.Empty;

    //EMPLOYEE GROUP
    int _groupID = 0;
    string _groupName = string.Empty;

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
    public string Suffix
    {
        set { _suffix = value; }
        get { return _suffix; }
    }
    public string PayBasis
    {
        set { _paybasis = value; }
        get { return _paybasis; }
    }
    public string Email
    {
        set { _email = value; }
        get { return _email; }
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
    public string UserId
    {
        set { _userId = value; }
        get { return _userId; }
    }

    //EMPLOYEE GROUP
    public int GroupID
    {
        set { _groupID = value; }
        get { return _groupID; }
    }
    public string GroupName
    {
        set { _groupName = value; }
        get { return _groupName; }
    }
    #endregion


    #region public method


    DataTable IPageable.ListPerPage(string _search, int _start, int _end, params string[] _custom)
    {
        //string query = string.Format(
        //    "WITH Temp_Table as ( " +
        //    "SELECT id,empId,firstName,middleName,lastName,email,active" +
        //    ",row_number() OVER (order by empId) as rowNum " +
        //    "FROM employees WHERE " +
        //    "empId LIKE {0} " +
        //    "OR firstName LIKE {0} " +
        //    "OR middleName LIKE {0} " +
        //    "OR lastName LIKE {0} " +
        //    "OR email LIKE {0} " +
        //    ")" +
        //    "SELECT * FROM Temp_Table " +
        //    "WHERE rowNum Between {1} and {2}", "'%" + _search + "%'", _start, _end);

        string query = string.Format(
            "WITH Temp_Table as ( " +
            "SELECT EMPID,FName,MName,LName,Suffix,Paybasis,Email" +
            ",row_number() OVER (order by empId) as rowNum " +
            "FROM employees WHERE " +
            "EMPID LIKE {0} " +
            "OR FName LIKE {0} " +
            "OR MName LIKE {0} " +
            "OR LName LIKE {0} " +
            "OR Email LIKE {0} " +
            ")" +
            "SELECT * FROM Temp_Table " +
            "WHERE rowNum Between {1} and {2}", "'%" + _search + "%'", _start, _end);

        DataTable _dt = this.FReadDataTable(query);

        return _dt;
    }

    int IPageable.TotalRows(string _search, params string[] _custom)
    {
        //string query = string.Format(
        //    "SELECT COUNT(id) " +
        //    "FROM employees WHERE " +
        //    "empId LIKE {0} " +
        //    "OR firstName LIKE {0} " +
        //    "OR middleName LIKE {0} " +
        //    "OR lastName LIKE {0} " +
        //    "OR email LIKE {0} ", "'%" + _search + "%'");


        string query = string.Format(
           "SELECT COUNT(EMPID) " +
           "FROM employees WHERE " +
           "EMPID LIKE {0} " +
           "OR FName LIKE {0} " +
           "OR MName LIKE {0} " +
           "OR LName LIKE {0} " +
           "OR Email LIKE {0} ", "'%" + _search + "%'");

        DataTable _dt = this.FReadDataTable(query);
        if (_dt.Rows.Count > 0)
            return Convert.ToInt32(_dt.Rows[0][0].ToString());
        else
            return 0;
    }

    DataTable IPageable.ListPerPageDTR(string _search, int _start, int _end, params string[] _custom)
    {
        //string query = string.Format(
        //    "WITH Temp_Table as ( " +
        //    "SELECT id,empId,firstName,middleName,lastName,email,active" +
        //    ",row_number() OVER (order by empId) as rowNum " +
        //    "FROM employees WHERE " +
        //    "empId LIKE {0} " +
        //    "OR firstName LIKE {0} " +
        //    "OR middleName LIKE {0} " +
        //    "OR lastName LIKE {0} " +
        //    "OR email LIKE {0} " +
        //    ")" +
        //    "SELECT * FROM Temp_Table " +
        //    "WHERE rowNum Between {1} and {2}", "'%" + _search + "%'", _start, _end); 

        string query = string.Format(
            "WITH Temp_Table as ( " +
            "SELECT EMPID,FName,MName,LName,Suffix,Paybasis,Email" +
            ",row_number() OVER (order by empId) as rowNum " +
            "FROM employees WHERE " +
            "EMPID LIKE {0} " +
            "OR FName LIKE {0} " +
            "OR MName LIKE {0} " +
            "OR LName LIKE {0} " +
            "OR Email LIKE {0} " +
            ")" +
            "SELECT * FROM Temp_Table " +
            "WHERE rowNum Between {1} and {2}", "'%" + _search + "%'", _start, _end);


        DataTable _dt = this.FReadDataTable(query);

        return _dt;
    }
    int IPageable.TotalRowsDTR(string _search, params string[] _custom)
    {
        //string query = string.Format(
        //    "SELECT COUNT(id) " +
        //    "FROM employees WHERE " +
        //    "empId LIKE {0} " +
        //    "OR firstName LIKE {0} " +
        //    "OR middleName LIKE {0} " +
        //    "OR lastName LIKE {0} " +
        //    "OR email LIKE {0} ", "'%" + _search + "%'");

        string query = string.Format(
         "SELECT COUNT(EMPID) " +
         "FROM employees WHERE " +
         "EMPID LIKE {0} " +
         "OR FName LIKE {0} " +
         "OR MName LIKE {0} " +
         "OR LName LIKE {0} " +
         "OR Email LIKE {0} ", "'%" + _search + "%'");

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
        //oparam.AddWithValue("@id", _id);
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
            //oparam.AddWithValue("@suffix", _suffix);
            oparam.AddWithValue("@PayBasis", _paybasis);
            oparam.AddWithValue("@email", _email);
            //oparam.AddWithValue("@active", _active);

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
            //oparam.AddWithValue("@id", _id);
            oparam.AddWithValue("@empId", _empId);
            oparam.AddWithValue("@firstName", _firstName);
            oparam.AddWithValue("@middleName", _middleName);
            oparam.AddWithValue("@lastName", _lastName);
            //oparam.AddWithValue("@suffix", _suffix);
            oparam.AddWithValue("@PayBasis", _paybasis);
            oparam.AddWithValue("@email", _email);
            //oparam.AddWithValue("@active", _active);

            //_rowsAffected = this.ExecuteCUD("sp_employee_edit", oparam);
            _rowsAffected = this.ExecuteCUD("sp_employee_add", oparam);
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
            oparam.AddWithValue("@empId", _empId);

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
            oparam.AddWithValue("@type", "employee");
            oparam.AddWithValue("@user", _userId);

            _rowsAffected = this.ExecuteCUD("sp_employee_masterfile_add", oparam);
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
            oparam.AddWithValue("@email", _email);
            oparam.AddWithValue("@active", _active);

            _rowsAffected = this.ExecuteCUD("sp_employee_add_masterfile", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }

    public int AddGroup()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@id", _groupID);
            oparam.AddWithValue("@groupName", _groupName);
            _rowsAffected = this.ExecuteCUD("sp_group_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public bool GroupExist()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@id", _groupID);
        oparam.AddWithValue("@groupName", _groupName);
        DataTable _dt = this.ExecuteRead("sp_group_validate", oparam);
        if (_dt.Rows.Count > 0)
            return true;
        return false;
    }
    public DataTable GroupList()
    {
        DataTable _dt = this.ExecuteRead("sp_group_list");
        return _dt;
    }
    public DataTable GroupList(string _usertype, string _userid)
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@usertype", _usertype);
        oparam.AddWithValue("@userid", _userid);
        DataTable _dt = this.ExecuteRead("sp_group_list_perID", oparam);
        return _dt;
    }

    public DataTable EmployeeList()
    {
        DataTable _dt = this.ExecuteRead("sp_employee_list");
        return _dt;
    }
    public DataTable EmployeeListPerApprover()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@empid", _empId);
        DataTable _dt = this.ExecuteRead("sp_employee_list_approver", oparam);
        return _dt;
    }
    public DataTable EmployeeListWithGroupName()
    {
        DataTable _dt = this.ExecuteRead("sp_employee_list_groupName");
        return _dt;
    }
    public DataTable EmployeeListPerGroup()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@id", _groupID);
        DataTable _dt = this.ExecuteRead("sp_employee_list_perGroup", oparam);
        return _dt;
    }
    public int AddEmpGroup()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@empId", _empId);
            oparam.AddWithValue("@groupId", _groupID);
            _rowsAffected = this.ExecuteCUD("sp_employeeGroup_add", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public DataTable GetEmployeeLogs(DateTime dtFrom, DateTime dtTo)
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@dtFrom", dtFrom);
        oparam.AddWithValue("@dtTo", dtTo);
        oparam.AddWithValue("@empId", _empId);

        DataTable _dt = this.ExecuteRead("sp_employee_logs", oparam);
        return _dt;
    }
    public DataTable GetEmployeeDetails()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@empId", _empId);
        DataTable _dt = this.ExecuteRead("sp_employee_details", oparam);
        return _dt;
    }

    //APPROVER
    public DataTable GetApproverGroupPerGroupID()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@groupID", _groupID);
        DataTable _dt = this.ExecuteRead("sp_employeeGroup_approver_list", oparam);
        return _dt;
    }
    public DataTable CheckIfApproverGroupExists()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@id", _id);
        oparam.AddWithValue("@groupID", _groupID);
        oparam.AddWithValue("@empId", _empId);

        DataTable _dt = this.ExecuteRead("sp_employeeGroup_approver_checkIfExists", oparam);
        return _dt;
    }
    public int SaveApproverGroup()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@id", _id);
            oparam.AddWithValue("@empId", _empId);
            oparam.AddWithValue("@groupId", _groupID);
            oparam.AddWithValue("@userId", _userId);

            _rowsAffected = this.ExecuteCUD("sp_employeeGroup_approver_save", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public int DeleteApproverGroup()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@id", _id);
            _rowsAffected = this.ExecuteCUD("sp_employeeGroup_approver_delete", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }
    public int DeleteGroup()
    {
        int _rowsAffected = 0;
        try
        {
            SqlParameterCollection oparam = new SqlCommand().Parameters;
            oparam.AddWithValue("@groupID", _groupID);
            _rowsAffected = this.ExecuteCUD("sp_employeeGroup_delete", oparam);
        }
        catch
        {
        }
        return _rowsAffected;
    }

    //DTR REPORT
    public DataTable GetDTRReport()
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@empId", _empId);

        DataTable _dt = this.ExecuteRead("sp_employee_logs", oparam);
        return _dt;
    }
    public DataTable GetDTRReport_Absences(string _payrollPeriodID)
    {
        SqlParameterCollection oparam = new SqlCommand().Parameters;
        oparam.AddWithValue("@empId", _empId);
        oparam.AddWithValue("@PayrollPeriodID", _payrollPeriodID);

        DataTable _dt = this.ExecuteRead("sp_dtrReport_getAbsences", oparam);
        return _dt;
    }
    //
    #endregion

    #region private method

    #endregion
}

