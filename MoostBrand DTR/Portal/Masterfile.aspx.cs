using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterFiles : PagingIterface
{
    private IPageable GetPageableObject()
    {
        return new EmployeeFile();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblUploadErr.Visible = false;
        try
        {
            int _uid = Convert.ToInt32(Session["uid"].ToString());
           
        }
        catch { Response.Redirect("~/Login.aspx"); }

        ReferenceControls(GetPageableObject(), rptrMaster, txtSearch, lblShowing, drpEntries,
        lnkPage1, lnkPage2, lnkPage3, lnkPage4, lnkFirst, lnkPrevious, lnkNext, lnkLast);

        if (!IsPostBack)
        {
            DefaultItemPerPage = 5;

            InitializePagination();
        }
    }
    protected void btnBackUpload_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Dashboard.aspx");
    }
    protected void rptrMaster_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk");
            Label lblFileName = (Label)e.Item.FindControl("lblFileName");
            string path = MapPath("~/MasterFiles/" + lblFileName.Text);
            string name = Path.GetFileName(path);
            string ext = Path.GetExtension(path);
            string type = "application/vnd.ms-excel";


            Response.AppendHeader("content-disposition",
                "attachment; filename=" + lnk.Text);

            Response.ContentType = type;
            Response.WriteFile(path);
            Response.End();
        }
        catch { }
    }
    private DataTable DataAssemble(DataTable dtFile)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("EMPID");
        dt.Columns.Add("FIRSTNAME");
        dt.Columns.Add("MIDDLENAME");
        dt.Columns.Add("LASTNAME");
        dt.Columns.Add("EMAIL");
        dt.Columns.Add("PAYBASIS");


        int _ctr = 0;
        foreach (DataRow _row in dtFile.Rows)
        {
            if (_ctr > 0)
            {
                DataRow _dr = dt.NewRow();
                _dr["EMPID"] = _row[0].ToString();
                _dr["FIRSTNAME"] = _row[1].ToString();
                _dr["MIDDLENAME"] = _row[2].ToString();
                _dr["LASTNAME"] = _row[3].ToString();
                _dr["EMAIL"] = _row[4].ToString();
                _dr["PAYBASIS"] = _row[5].ToString();

                
                dt.Rows.Add(_dr);
            }
            _ctr++;
        }

        return dt;
    }
    protected void btnUploadEmployee_Click(object sender, EventArgs e)
    {
        if (flEmployee.HasFile)
        {
            string[] FileExt = flEmployee.FileName.Split('.');
            string FileEx = FileExt[FileExt.Length - 1];
            if (FileEx.ToLower() == "xls" || FileEx.ToLower() == "xlsx")
            {
                string _fileName = Path.GetFileName(flEmployee.PostedFile.FileName);
                string _extension = Path.GetExtension(flEmployee.PostedFile.FileName);
                string _fileNameWithoutExtension = Path.GetFileNameWithoutExtension(flEmployee.PostedFile.FileName);
                DateTime _dtNow = DateTime.Now;
                string _newFileName = "masterfile_" + _dtNow.Year.ToString() + _dtNow.Month.ToString() + _dtNow.Day.ToString()
                    + _dtNow.Hour.ToString() + _dtNow.Minute.ToString() + _dtNow.Second.ToString() + _dtNow.Millisecond.ToString() + "." + _extension;

                string _filePath = Server.MapPath("MasterFiles//" + _newFileName);

                flEmployee.SaveAs(_filePath);
                Excel _excel = new Excel();
                _excel.FilePath = _filePath;
                _excel.Extension = _extension;
                DataTable dtSchema = _excel.ExcelSchema();
                DataTable dtAssemble = DataAssemble(dtSchema);

                DateTime _dateUploaded = DateTime.Now;

                EmployeeFile _empUpload = new EmployeeFile();
                _empUpload.FileName = _newFileName;
                _empUpload.OrigFileName = flEmployee.FileName;
                _empUpload.DateCreated = DateTime.Now;
                _empUpload.Type = "employee";
                _empUpload.UserId = Convert.ToInt32(Session["uid"].ToString());
                try { _empUpload.AddMasterFile(); }
                catch { }

                foreach (DataRow _row in dtAssemble.Rows)
                {
                    if (_row[0].ToString().Trim().Length > 0)
                    {
                        EmployeeFile _employee = new EmployeeFile();
                        _employee.EmployeeID = _row[0].ToString().ToUpper();
                        _employee.FirstName = _row[1].ToString().ToUpper();
                        _employee.MiddleName = _row[2].ToString().ToUpper();
                        _employee.LastName = _row[3].ToString().ToUpper();
                        _employee.Email = _row[4].ToString().ToUpper();
                        _employee.PayBasis = _row[5].ToString().ToUpper();

                        _employee.Active = 1;
                        //_employee.Password = AutoGeneratedCode();
                        _employee.Password = _row[0].ToString();
                        

                        if (_employee.AddEmployeeMasterfile() == 0)
                        {
                            lblUploadErr.Text = "Adding employee failed.";
                            lblUploadErr.Visible = true;
                            break;
                        }
                    }
                }


                Paginate();

                Response.Write("<script type=\"text/javascript\">alert('Success.');" + "window.close();</script>");
                return;

            }
            else
            {
                //Response.Write("<script type=\"text/javascript\">alert('Invalid file format!.');" + "window.close();</script>");
                lblUploadErr.Text = "Invalid file format!";
                lblUploadErr.Visible = true;
            }
        }
    }

    private string AutoGeneratedCode()
    {
        string _empPassword = string.Empty;

        int size = 6;
        Random r = new Random();
        string legalChars = "0123456789ABCDEFGH";
        string s_ID = "0";
        while (s_ID[0].ToString() == "0")
        {
        begin:
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < size; i++)
                sb.Append(legalChars.Substring(r.Next(0, legalChars.Length - 1), 1));

            s_ID = sb.ToString();

            if (s_ID[0].ToString() == "0")
                goto begin;

        }
        _empPassword = s_ID;

        return _empPassword;
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Response.Redirect("~/Login.aspx");

    }
}