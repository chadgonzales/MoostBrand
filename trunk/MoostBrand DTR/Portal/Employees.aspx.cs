using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : PagingIterface
{
    private IPageable GetPageableObject()
    {
        return new Employee();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
        lblUploadErr.Visible = false;
        try
        {
            int _uid = Convert.ToInt32(Session["uid"].ToString());
        }
        catch
        {
            //Response.Redirect("~/Login.aspx");
            Response.Redirect("~/Login.aspx", false);        //write redirect
            Context.ApplicationInstance.CompleteRequest();
        }

        ReferenceControls(GetPageableObject(), rptrEmployees, txtSearch, lblShowing, drpEntries,
        lnkPage1, lnkPage2, lnkPage3, lnkPage4, lnkFirst, lnkPrevious, lnkNext, lnkLast);

        if (!IsPostBack)
        {
            HidePanel();
            pnlList.Visible = true;

            DefaultItemPerPage = 5;

            InitializePagination();
        }
    }
    void HidePanel()
    {
        pnlList.Visible = false;
        pnlAdd.Visible = false;
        pnlUpload.Visible = false;
    }
    protected void rptrEmployees_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label lblID = (Label)e.Item.FindControl("lblID");
        Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");

        if (e.CommandName == "edit")
        {
            //Label lblActive = (Label)e.Item.FindControl("lblActive");
            //Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");
            Label lblFName = (Label)e.Item.FindControl("lblFName");
            Label lblMName = (Label)e.Item.FindControl("lblMName");
            Label lblLName = (Label)e.Item.FindControl("lblLName");
            Label lblEmail = (Label)e.Item.FindControl("lblEmail");
            Label lblPaybasis = (Label)e.Item.FindControl("lblPaybasis");
            //Label lblSuffix = (Label)e.Item.FindControl("lblSuffix");


            //lblEID.Text = lblID.Text;
            txtEmpId.Text = lblEmpID.Text;
            txtFName.Text = lblFName.Text;
            txtMName.Text = lblMName.Text;
            txtLName.Text = lblLName.Text;
            txtEmail.Text = lblEmail.Text;
            txtPayBasis.Text = lblPaybasis.Text;
            //txtSuffix.Text = lblSuffix.Text;
            //if (lblActive.Text == "1")
            //    chkActive.Checked = true;
            //else
            //    chkActive.Checked = false;

            HidePanel();
            pnlAdd.Visible = true;
        }
        else
        {
            Employee _employee = new Employee();
            //_employee.ID = Convert.ToInt32(lblID.Text);
            _employee.EmployeeID = lblEmpID.Text;

            if (_employee.Remove() > 0)
            {
                HidePanel();
                Paginate();
                pnlList.Visible = true;
                Response.Write("<script type=\"text/javascript\">alert('Success.');" + "window.close();</script>");
                return;
            }
            else
            {
                Response.Write("<script type=\"text/javascript\">alert('Removing employee failed.');" + "window.close();</script>");
                return;
            }
        }
    }
    protected void rptrEmployees_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //Label lblActive = (Label)e.Item.FindControl("lblActive");
        //Button btnActive = (Button)e.Item.FindControl("btnActive");

        //if (lblActive.Text == "1")
        //    btnActive.Text = "Deactivate";
        //else
        //    btnActive.Text = "Activate";
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlList.Visible = true;
    }
    void ValidateEntry()
    {
        if ((txtEmpId.Text == string.Empty) || (txtEmpId.Text.Replace(" ", "") == ""))
        {
            txtEmpId.Focus();
            lblErr.Text = "Employee ID is required.";
            lblErr.Visible = true;
            return; 
        }
        if ((txtFName.Text == string.Empty) || (txtFName.Text.Replace(" ", "") == ""))
        {
            txtFName.Focus();
            lblErr.Text = "First name is required.";
            lblErr.Visible = true;
            return;
        }
        if ((txtMName.Text == string.Empty) || (txtMName.Text.Replace(" ", "") == ""))
        {
            txtMName.Focus();
            lblErr.Text = "Middle name is required.";
            lblErr.Visible = true;
            return;
        }
        if ((txtLName.Text == string.Empty) || (txtLName.Text.Replace(" ", "") == ""))
        {
            txtLName.Focus();
            lblErr.Text = "Last name is required.";
            lblErr.Visible = true;
            return;
        }
        if ((txtEmail.Text == string.Empty) || (txtEmail.Text.Replace(" ", "") == ""))
        {
            txtEmail.Focus();
            lblErr.Text = "Email is required.";
            lblErr.Visible = true;
            return;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ValidateEntry();
        Employee _employee = new Employee();
        //_employee.ID = Convert.ToInt32(lblEID.Text);
        _employee.EmployeeID = txtEmpId.Text;
        _employee.FirstName = txtFName.Text;
        _employee.MiddleName = txtMName.Text;
        _employee.LastName = txtLName.Text;
        _employee.Email = txtEmail.Text;
        //_employee.Suffix = txtSuffix.Text;
        _employee.PayBasis = txtPayBasis.Text;
        //if (chkActive.Checked)
        //    _employee.Active = 1;
        //else
        //    _employee.Active = 0;

        //if (_employee.Exist())
        //{
        //    lblErr.Text = "Employee ID already exist.";
        //    lblErr.Visible = true;
        //}
        //else
        //{
        //if (lblEID.Text == "0")
        //{

        if (_employee.Exist())
        {
            if (_employee.Update() > 0)
            {
                HidePanel();
                txtSearch.Text = string.Empty;
                PageNo = 1;
                Paginate();
                pnlList.Visible = true;
                Response.Write("<script type=\"text/javascript\">alert('Success.');" + "window.close();</script>");
                return;
            }
            else
            {
                lblErr.Text = "Updating employee failed.";
                lblErr.Visible = true;
            }
        }

        else
        {
            if (txtEmpId.Text != null)
            {
                if (_employee.Add() > 0)
                {
                    HidePanel();
                    txtSearch.Text = string.Empty;
                    PageNo = 1;
                    Paginate();
                    pnlList.Visible = true;
                    Response.Write("<script type=\"text/javascript\">alert('Success.');" + "window.close();</script>");
                    return;
                }
                else
                {
                    lblErr.Text = "Adding employee failed.";
                    lblErr.Visible = true;
                }
            }
            //}
            //if (_employee.Update() > 0)
            //{
            //    HidePanel();
            //    txtSearch.Text = string.Empty;
            //    PageNo = 1;
            //    Paginate();
            //    pnlList.Visible = true;
            //    Response.Write("<script type=\"text/javascript\">alert('Success.');" + "window.close();</script>");
            //    return;
            //}
            //else
            //{
            //    lblErr.Text = "Updating employee failed.";
            //    lblErr.Visible = true;
            //}
        }
        //}
    }
    void ClearEntry()
    {
        txtEmpId.Text = string.Empty;
        txtFName.Text = string.Empty;
        txtMName.Text = string.Empty;
        txtLName.Text = string.Empty;
        txtEmail.Text = string.Empty;
        //txtSuffix.Text = string.Empty;
        txtPayBasis.Text = string.Empty;
        //chkActive.Checked = true;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        HidePanel();
        lblEID.Text = "0";
        ClearEntry();
        pnlAdd.Visible = true;
    }
    void MasterFileSource()
    {
        Employee _employee = new Employee();
        rptrMaster.DataSource = _employee.MasterFiles();
        rptrMaster.DataBind();
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //HidePanel();
        //pnlUpload.Visible = true;
        //MasterFileSource();

        //Response.Redirect("~/MasterFile.aspx");

        Response.Redirect("~/MasterFile.aspx", false);        //write redirect
        Context.ApplicationInstance.CompleteRequest();
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
                string _newFileName = "masterfile_" +_dtNow.Year.ToString() + _dtNow.Month.ToString() + _dtNow.Day.ToString()
                    + _dtNow.Hour.ToString() + _dtNow.Minute.ToString() + _dtNow.Second.ToString() + _dtNow.Millisecond.ToString() + "." + _extension;

                string _filePath = Server.MapPath("MasterFiles//" + _newFileName);

                flEmployee.SaveAs(_filePath);
                Excel _excel = new Excel();
                _excel.FilePath = _filePath;
                _excel.Extension = _extension;
                DataTable dtSchema = _excel.ExcelSchema();
                DataTable dtAssemble = DataAssemble(dtSchema);

                DateTime _dateUploaded = DateTime.Now;

                Employee _empUpload = new Employee();
                _empUpload.FileName = _newFileName;
                _empUpload.OrigFileName = flEmployee.FileName;
                _empUpload.DateCreated = DateTime.Now;
                _empUpload.Type = "employee";
                _empUpload.UserId =Session["uid"].ToString();
                try { _empUpload.AddMasterFile(); }
                catch { }

                foreach (DataRow _row in dtAssemble.Rows)
                {
                    if (_row[0].ToString().Trim().Length > 0)
                    {
                        Employee _employee = new Employee();
                        _employee.EmployeeID = _row[0].ToString();
                        _employee.FirstName = _row[1].ToString();
                        _employee.MiddleName = _row[2].ToString();
                        _employee.LastName = _row[3].ToString();
                        _employee.Email = _row[4].ToString();
                        _employee.Active = 1;

                        if (_employee.AddEmployeeMasterfile() == 0)
                        {
                            lblUploadErr.Text = "Adding employee failed.";
                            lblUploadErr.Visible = true;
                            break;
                        }
                    }
                }


                MasterFileSource();

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
    protected void lnkEmployees_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/Employees.aspx");

        Response.Redirect("~/Employees.aspx", false);        //write redirect
        Context.ApplicationInstance.CompleteRequest();


    }
    protected void btnBackUpload_Click(object sender, EventArgs e)
    {
        HidePanel();
        PageNo = 1;
        Paginate();
        pnlList.Visible = true;
    }
}