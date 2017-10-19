using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard : PagingIterface
{
    private IPageable GetPageableObject()
    {
        return new EmployeeFile();
    }
    DataTable _DayEventsTable = new DataTable();
    string _first = "";
    string _shiftEMPID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int _uid = Convert.ToInt32(Session["uid"].ToString());
            string _empID = Session["empID"].ToString();
            SetLinks();

            lblUsername.Text = Session["uname"].ToString();
            lblUserID.Text = _empID;
            lblUsertype.Text = Session["utype"].ToString();


            SetShowLinks();

        }
        catch { Response.Redirect("~/Login.aspx"); }

        if (!IsPostBack)
        {
            HidePanel();
            //lblPayrollPeriod.Text = "";
            //lblPayrollPeriod.Visible = false;
            //lnkSelectPayrollPeriod.Visible = true;
            //ddlDTREmployee.Visible = false;
            //btnSaveDTR.Visible = false;
            //btnProcessDTR1.Visible = false;


        }
        lblNewGroupMsg.Visible = false;

        ReferenceControls(GetPageableObject(), rptrMaster, txtSearch, lblShowing, drpEntries,
      lnkPage1, lnkPage2, lnkPage3, lnkPage4, lnkFirst, lnkPrevious, lnkNext, lnkLast);

        if (!IsPostBack)
        {
            DefaultItemPerPage = 5;

            InitializePaginationDTR();
        }
    }
    private void SetShowLinks()
    {

        if (lblUsertype.Text.ToLower() == "admin")
        {
            lnkUploadMasterFile.Visible = false;
            lnkUserManagement.Visible = true;
            lnkCreateGroups.Visible = true;
            lnkEmpGroupings.Visible = true;

            lnkLeaveType.Visible = true;
            lnkLeaveManagement.Visible = true;
            lnkLeaveCredits.Visible = true;

            lnkShiftSchedule.Visible = true;
            lnkShiftScheduleTagging.Visible = true;

            lnkOTRequest.Visible = true;
            lnkLogs.Visible = true;
            lnkHolidays.Visible = true;
            //lnkDailyTimeRecord.Visible = true;
            lnkDTR.Visible = true;
            lnkPayrollPeriod.Visible = true;
            //lnkUploadDTR.Visible = true;

            lnkDTRReport.Visible = true;
            lnkLeaveReport.Visible = true;


        }
        else if (lblUsertype.Text.ToLower() == "superuser")
        {
            lnkUploadMasterFile.Visible = true;
        }
        else if (lblUsertype.Text.ToLower() == "supervisor")
        {
            //lnkSchedule.Visible = true;
            lnkLeaveManagement.Visible = true;
            //lnkEmpGroupings.Visible = true;
            lnkOTRequest.Visible = true;
            lnkLeaveManagement.Visible = true;
            lnkLeaveCredits.Visible = false;
            lnkLogs.Visible = true;
            lnkShiftScheduleTagging.Visible = true;
            //lnkDailyTimeRecord.Visible = true;
            lnkDTR.Visible = true;
            lnkLeaveReport.Visible = true;
            //lnkUploadDTR.Visible = true;

        }
        else if (lblUsertype.Text.ToLower() == "user")
        {
            lnkOTRequest.Visible = true;
            lnkLeaveManagement.Visible = true;
            lnkLogs.Visible = true;

        }
        else if (lblUsertype.Text.ToLower() == "clerk")
        {
            lnkOTRequest.Visible = true;
            lnkLeaveManagement.Visible = true;
            lnkLogs.Visible = true;
            //lnkDailyTimeRecord.Visible = true;
            lnkDTR.Visible = true;
        }

    }
    void HidePanel()
    {
        pnlUsers.Visible = false;
        pnlGroups.Visible = false;
        pnlEmpGroupings.Visible = false;
        pnlChangePassword.Visible = false;

        pnlLeaveTypes.Visible = false;
        pnlLeaveTypeLists.Visible = false;
        pnlAddLeaveType.Visible = false;

        pnlLeaveCredits.Visible = false;
        pnlLeaveCreditsAdd.Visible = false;
        pnlLeaveCreditsList.Visible = false;

        pnlLeaveApplication.Visible = false;
        pnlLeaveApplicationAdd.Visible = false;
        pnlLeaveApplicationLists.Visible = false;

        pnlOTRequest.Visible = false;
        pnlOTRequestList.Visible = false;
        pnlOTRequestNew.Visible = false;

        pnlShiftSchedule.Visible = false;
        pnlShiftScheduleList.Visible = false;
        pnlShiftScheduleNew.Visible = false;
        pnlChangeScheduleRequests.Visible = false;

        pnlShiftScheduleTagging.Visible = false;
        pnlShiftScheduleTaggingList.Visible = false;
        pnlShiftScheduleTaggingNew.Visible = false;
        pnlShiftScheduleImport.Visible = false;

        pnlLogs.Visible = false;
        pnlProfile.Visible = false;
        pnlHolidays.Visible = false;

        pnlDTR.Visible = false;
        //pnlDailyTimeRecord.Visible = false;
        pnlPayrollPeriod.Visible = false;
        pnlUploadDTR.Visible = false;

        pnlApprover.Visible = false;
        pnlApproverList.Visible = false;

        pnlDTRReport.Visible = false;
        pnlLeaveReport.Visible = false;
    }
    void SetLinks()
    {
        lnkUploadMasterFile.Visible = false;
        lnkUserManagement.Visible = false;
        lnkCreateGroups.Visible = false;
        lnkEmpGroupings.Visible = false;

        lnkShiftSchedule.Visible = false;
        lnkShiftScheduleTagging.Visible = false;

        //lnkSchedule.Visible = false;

        lnkOTRequest.Visible = false;
        //lnkOTManagement.Visible = false;

        lnkLeaveType.Visible = false;
        lnkLeaveManagement.Visible = false;
        lnkLeaveCredits.Visible = false;
        lnkLogs.Visible = false;
        lnkHolidays.Visible = false;
        lnkUploadDTR.Visible = false;
        //lnkDailyTimeRecord.Visible = false;
        lnkDTR.Visible = false;
        lnkPayrollPeriod.Visible = false;
        lnkDTRReport.Visible = false;
        lnkLeaveReport.Visible = false;

    }
    protected void lnkUploadMasterFile_Click(object sender, EventArgs e)
    {
        HidePanel();
        Response.Redirect("~/Masterfile.aspx");
    }
    protected void lnkUserManagement_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlUsers.Visible = true;
        GetAllUsers();
    }
    protected void lnkCreateGroups_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlGroups.Visible = true;
        GetGroups();
        pnlApprover.Visible = false;
        pnlApproverList.Visible = false;
    }
    protected void lnkEmpGroupings_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlEmpGroupings.Visible = true;
        pnlEmpGroupingsList.Visible = true;
        pnlNewGroupings.Visible = false;
        InitializeEmployeeGroup();
    }

    protected void lnkChangePassword_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlChangePassword.Visible = true;
        lblChangePwMsg.Visible = false;
    }
    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Response.Redirect("~/Login.aspx");
    }



    //USERS
    private void GetAllUsers()
    {
        //List of usertypes
        ddlUserTypes.Items.Clear();
        Users _user = new Users();
        DataTable dtuserTypes = new DataTable();
        dtuserTypes = _user.GetAllUserTypes();

        foreach (DataRow item in dtuserTypes.Rows)
        {
            ddlUserTypes.Items.Add(new ListItem(item["type"].ToString(), item["id"].ToString()));
        }

        //add blank item 
        ddlUserTypes.Items.Insert(0, new ListItem("Select usertype", String.Empty));
        ddlUserTypes.SelectedIndex = 0;


        //List of users
        _user = new Users();
        DataTable dtUsers = new DataTable();
        dtUsers = _user.GetAllUsers();

        rptrUsers.DataSource = dtUsers;
        rptrUsers.DataBind();

    }
    private void GetEmployeePerUsertype()
    {
        int _usertypeID = 0;
        try
        {
            _usertypeID = Convert.ToInt32(ddlUserTypes.SelectedValue.ToString());
        }
        catch
        {

        }

        Users _user = new Users();
        _user.UsertypeID = _usertypeID;
        DataTable dtUsersList = _user.EmployeeListPerUserType();

        rptrUsers.DataSource = dtUsersList;
        rptrUsers.DataBind();
    }
    protected void ddlUserTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetEmployeePerUsertype();
    }
    protected void rptrUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DropDownList ddlUserType = (DropDownList)e.Item.FindControl("ddlUserType");
            Label lblUserTypeID = (Label)e.Item.FindControl("lblUserTypeID");

            foreach (ListItem item in ddlUserTypes.Items)
            {
                if (item.Value == string.Empty)
                {

                }
                else
                {
                    ddlUserType.Items.Add(new ListItem(item.Text, item.Value));
                }
            }
            ddlUserType.SelectedIndex = Convert.ToInt32(lblUserTypeID.Text) - 1;

        }
    }

    protected void rptrUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");
        Label lblUserTypeID = (Label)e.Item.FindControl("lblUserTypeID");

        DropDownList ddlUserType = (DropDownList)e.Item.FindControl("ddlUserType");
        LinkButton lnkUpdate = (LinkButton)e.Item.FindControl("lnkUpdate");
        LinkButton lnkCancel = (LinkButton)e.Item.FindControl("lnkCancel");


        if (e.CommandName == "Update")
        {
            string _commandText = lnkUpdate.Text;
            if (_commandText == "Save")
            {
                //SAVE; Update  usertype per employee
                Users _user = new Users();
                _user.EmpID = lblEmpID.Text;
                _user.UsertypeID = Convert.ToInt32(ddlUserType.SelectedValue.ToString());
                int rowAffected = _user.UpdateUsertypes();
                if (rowAffected > 0)
                {
                    lnkCancel.Visible = false;
                    ddlUserType.Visible = false;
                    lnkUpdate.Text = "Update Usertype";
                    GetAllUsers();
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">alert('Updating employee's usertype failed.');" + "window.close();</script>");
                    return;
                }
            }
            else
            {
                ddlUserType.Visible = true;
                lnkUpdate.Text = "Save";
                lnkCancel.Visible = true;
            }
        }
        if (e.CommandName == "Cancel")
        {
            lnkCancel.Visible = false;
            ddlUserType.Visible = false;
            lnkUpdate.Text = "Update Usertype";
        }
    }

    //GROUPS
    private void GetGroups()
    {
        Employee _employee = new Employee();
        DataTable dtGroups = new DataTable();
        dtGroups = _employee.GroupList();

        rptrGroups.DataSource = dtGroups;
        rptrGroups.DataBind();


    }
    protected void btnAddNewGroup_Click(object sender, EventArgs e)
    {
        pnlNewGroup.Visible = true;
        txtNewGroup.Text = "";
        pnlGroupList.Visible = false;
    }
    protected void btnAddGroup_Click(object sender, EventArgs e)
    {
        if ((txtNewGroup.Text == string.Empty) || (txtNewGroup.Text.Replace(" ", "") == ""))
        {
            txtNewGroup.Focus();
            lblNewGroupMsg.Text = "Group name is required.";
            lblNewGroupMsg.Visible = true;
            return;
        }
        int _groupID = 0;
        try
        {
            _groupID = Convert.ToInt32(lblGroupID.Text);
        }
        catch
        {

        }
        Employee _employee = new Employee();
        _employee.GroupID = _groupID;
        _employee.GroupName = txtNewGroup.Text;
        int _rowAffected = _employee.AddGroup();
        if (_rowAffected > 0)
        {
            txtNewGroup.Text = "";
            GetGroups();
            pnlNewGroup.Visible = false;
            pnlGroupList.Visible = true;
        }
        else
        {
            lblNewGroupMsg.Text = "Failed to save new group.";
            lblNewGroupMsg.Visible = true;
            return;
        }

    }
    protected void rptrGroups_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label lblID = (Label)e.Item.FindControl("lblID");
        Label lblGroupName = (Label)e.Item.FindControl("lblGroupName");

        if (e.CommandName == "Update")
        {
            lblGroupID.Text = lblID.Text;
            txtNewGroup.Text = lblGroupName.Text;
            pnlNewGroup.Visible = true;
            pnlGroupList.Visible = false;
        }
        else if (e.CommandName == "Remove")
        {
            Employee _emp = new Employee();
            _emp.GroupID = Convert.ToInt32(lblID.Text);


            int rowaffected = _emp.DeleteGroup();
            if (rowaffected > 0)
            {
                GetGroups();
            }

        }
        else if (e.CommandName == "Approver")
        {
            pnlApprover.Visible = true;
            //CLEAR CONTROLS

            //GET APPROVER
            if (lblUsertype.Text == "admin")
            {
                //get employee list
                GetEmployeeList();
            }
            else
            {
                GetEmployeeListPerApprover();
            }
            lblApproverMsg.Text = "";
            lblApproverMsg.Visible = false;
            lblApproverID.Text = "0";
            lblApproverEmpID.Text = "0";
            lblApproverGroupID.Text = lblID.Text;
        }
        else if (e.CommandName == "View Approver")
        {
            pnlApproverList.Visible = true;
            Employee _emp = new Employee();
            _emp.GroupID = Convert.ToInt32(lblID.Text);

            DataTable dt = new DataTable();
            dt = _emp.GetApproverGroupPerGroupID();

            rptrApprover.DataSource = null;
            rptrApprover.DataSource = dt;
            rptrApprover.DataBind();
        }

    }
    protected void btnBackGroup_Click(object sender, EventArgs e)
    {
        txtNewGroup.Text = "";
        GetGroups();
        pnlNewGroup.Visible = false;
        pnlGroupList.Visible = true;
    }

    //EMPLOYEE GROUPINGS
    private void InitializeEmployeeGroup()
    {
        lblNewEmpGroupMsg.Visible = false;
        ddlGroup.Items.Clear();
        ddlGroupList.Items.Clear();

        Employee _employee = new Employee();
        DataTable dtGroups = new DataTable();
        dtGroups = _employee.GroupList();

        //dtGroups = _employee.GroupList(lblUsertype.Text, lblUserID.Text);
        //GROUP LIST
        foreach (DataRow item in dtGroups.Rows)
        {
            ddlGroup.Items.Add(new ListItem(item["name"].ToString(), item["id"].ToString()));
            ddlGroupList.Items.Add(new ListItem(item["name"].ToString(), item["id"].ToString()));
        }

        //add blank item 
        ddlGroup.Items.Insert(0, new ListItem("Select Group", String.Empty));
        ddlGroup.SelectedIndex = 0;
        try
        {
            ddlGroupList.SelectedIndex = 0;
        }
        catch
        {

        }

        //EMPLOYEE LIST
        _employee = new Employee();
        DataTable dtEmployee = new DataTable();
        dtEmployee = _employee.EmployeeListWithGroupName();

        rptrEmployeeList.DataSource = dtEmployee;
        rptrEmployeeList.DataBind();

        if (dtEmployee.Rows.Count > 0)
        {
            lblNewEmpGroupMsg.Visible = false;
            btnAddtoGroup.Enabled = true;
        }
        else
        {
            lblNewEmpGroupMsg.Visible = true;
            lblNewEmpGroupMsg.Text = "All employees already have corresponding groups.";
            btnAddtoGroup.Enabled = false;
        }


        GetEmployeePerGroup();

    }
    protected void btnAddtoGroup_Click(object sender, EventArgs e)
    {
        lblNewEmpGroupMsg.Visible = false;

        //VALIDATE GROUP and EMPLOYEE LIST
        if (ddlGroup.SelectedValue.ToString() == string.Empty)
        {
            lblNewEmpGroupMsg.Visible = true;
            lblNewEmpGroupMsg.Text = "Select a group.";
            return;
        }
        int _checkCtr = 0;
        foreach (RepeaterItem item in rptrEmployeeList.Items)
        {
            CheckBox chk = (CheckBox)item.FindControl("chk");
            if (chk.Checked == true)
            {
                _checkCtr++;
            }
        }

        if (_checkCtr > 0)
        {

        }
        else
        {
            lblNewEmpGroupMsg.Visible = true;
            lblNewEmpGroupMsg.Text = "Select employees to group.";
            return;
        }

        int rowAffected = 0;
        //SAVE TO DATABASE
        foreach (RepeaterItem item in rptrEmployeeList.Items)
        {
            CheckBox chk = (CheckBox)item.FindControl("chk");
            Label lblEmpID = (Label)item.FindControl("lblEmpID");

            if (chk.Checked == true)
            {
                Employee _employee = new Employee();
                _employee.ID = 0;
                _employee.EmployeeID = lblEmpID.Text;
                _employee.GroupID = Convert.ToInt32(ddlGroup.SelectedValue.ToString());
                rowAffected = _employee.AddEmpGroup();
            }
        }
        if (rowAffected > 0)
        {
            pnlEmpGroupingsList.Visible = true;
            pnlNewGroupings.Visible = false;
            InitializeEmployeeGroup();
        }
        else
        {
            lblNewEmpGroupMsg.Visible = true;
            lblNewEmpGroupMsg.Text = "Error in adding employee groupings.";
            return;
        }
    }
    protected void chkList_CheckedChanged(object sender, EventArgs e)
    {
        if (chkList.Checked == true)
        {
            foreach (RepeaterItem item in rptrEmployeeList.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chk");
                chk.Checked = true;
            }
        }
        else
        {
            foreach (RepeaterItem item in rptrEmployeeList.Items)
            {
                CheckBox chk = (CheckBox)item.FindControl("chk");
                chk.Checked = false;
            }
        }
    }
    private void GetEmployeePerGroup()
    {
        int _groupID = 0;
        try
        {
            _groupID = Convert.ToInt32(ddlGroupList.SelectedValue.ToString());
        }
        catch
        {

        }
        Employee _employee = new Employee();
        _employee.GroupID = _groupID;
        DataTable dtGroupList = _employee.EmployeeListPerGroup();

        rptrEmployeeGroups.DataSource = dtGroupList;
        rptrEmployeeGroups.DataBind();
    }
    protected void ddlGroupList_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetEmployeePerGroup();
    }
    protected void btnAddEmpGroup_Click(object sender, EventArgs e)
    {
        pnlEmpGroupingsList.Visible = false;
        pnlNewGroupings.Visible = true;
        InitializeEmployeeGroup();
    }
    protected void btnBackEmpGroup_Click(object sender, EventArgs e)
    {
        pnlEmpGroupingsList.Visible = true;
        pnlNewGroupings.Visible = false;
        InitializeEmployeeGroup();
    }
    protected void rptrEmployeeGroups_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");
        Label lblGroupID = (Label)e.Item.FindControl("lblGroupID");

        DropDownList ddlEmpGroup = (DropDownList)e.Item.FindControl("ddlEmpGroup");
        LinkButton lnkUpdate = (LinkButton)e.Item.FindControl("lnkUpdate");
        LinkButton lnkCancel = (LinkButton)e.Item.FindControl("lnkCancel");


        if (e.CommandName == "Update")
        {
            string _commandText = lnkUpdate.Text;
            if (_commandText == "Save")
            {
                //SAVE; Update  group per employee
                Employee _employee = new Employee();
                _employee.EmployeeID = lblEmpID.Text;
                _employee.GroupID = Convert.ToInt32(ddlEmpGroup.SelectedValue.ToString());
                int rowAffected = _employee.AddEmpGroup();
                if (rowAffected > 0)
                {
                    lnkCancel.Visible = false;
                    ddlEmpGroup.Visible = false;
                    lnkUpdate.Text = "Update Group";
                    InitializeEmployeeGroup();
                }
                else
                {
                    Response.Write("<script type=\"text/javascript\">alert('Updating employee's group failed.');" + "window.close();</script>");
                    return;
                }
            }
            else
            {
                ddlEmpGroup.Visible = true;
                lnkUpdate.Text = "Save";
                lnkCancel.Visible = true;
            }
        }
        if (e.CommandName == "Cancel")
        {
            lnkCancel.Visible = false;
            ddlEmpGroup.Visible = false;
            lnkUpdate.Text = "Update Group";
        }
    }
    protected void rptrEmployeeGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //ddlEmpGroup
        if (e.Item.ItemType == ListItemType.Item | e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DropDownList ddlEmpGroup = (DropDownList)e.Item.FindControl("ddlEmpGroup");
            Label lblGroupID = (Label)e.Item.FindControl("lblGroupID");

            foreach (ListItem item in ddlGroup.Items)
            {
                if (item.Value == string.Empty)
                {

                }
                else
                {
                    ddlEmpGroup.Items.Add(new ListItem(item.Text, item.Value));
                }
            }
            ddlEmpGroup.SelectedIndex = Convert.ToInt32(lblGroupID.Text) - 1;

        }
    }

    //CHANGE PASSWORD
    bool ValidateChangePw()
    {
        lblChangePwMsg.Visible = false;
        if (txtCurrentPw.Text == "")
        {
            lblChangePwMsg.Visible = true;
            lblChangePwMsg.Text = "Current Password is required.";
            txtCurrentPw.Focus();
            return false;
        }
        if (txtNewPw.Text == "")
        {
            lblChangePwMsg.Visible = true;
            lblChangePwMsg.Text = "New Password is required.";
            txtNewPw.Focus();
            return false;
        }
        if (txtConfirmPw.Text == "")
        {
            lblChangePwMsg.Visible = true;
            lblChangePwMsg.Text = "Confirm Password is required.";
            txtConfirmPw.Focus();
            return false;
        }

        try
        {
            string _currentPw = Session["upw"].ToString();
            if (txtCurrentPw.Text != _currentPw)
            {
                lblChangePwMsg.Visible = true;
                lblChangePwMsg.Text = "Invalid Current Password.";
                txtCurrentPw.Focus();
                return false;
            }
        }
        catch
        {

        }
        if (txtConfirmPw.Text != txtNewPw.Text)
        {
            lblChangePwMsg.Visible = true;
            lblChangePwMsg.Text = "New Password should match.";
            txtConfirmPw.Focus();
            return false;
        }

        return true;
    }
    protected void btnChangePw_Click(object sender, EventArgs e)
    {
        if (ValidateChangePw())
        {
            //change password
            try
            {
                Users _user = new Users();
                _user.EmpID = Session["empID"].ToString();
                _user.Password = txtNewPw.Text;
                int rowAffected = _user.ChangePassword();
                if (rowAffected > 0)
                {
                    lblChangePwMsg.Visible = true;
                    lblChangePwMsg.Text = "Successfully changed password.";
                    Session["upw"] = txtNewPw.Text;
                }
                else
                {
                    lblChangePwMsg.Visible = true;
                    lblChangePwMsg.Text = "Failed to change password.";
                    return;
                }
            }
            catch
            {

            }

        }
    }
    protected void btnChangePwBack_Click(object sender, EventArgs e)
    {
        HidePanel();
    }
    protected void lnkLeaveType_Click(object sender, EventArgs e)
    {
        HidePanel();
        GetLeaveTypes();
        pnlLeaveTypeLists.Visible = true;
        pnlLeaveTypes.Visible = true;

    }
    protected void lnkShiftSchedule_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlShiftSchedule.Visible = true;
        pnlShiftScheduleList.Visible = true;
        pnlShiftScheduleNew.Visible = false;
        GetShiftSchedule();
    }
    protected void lnkShiftScheduleTagging_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlShiftScheduleTagging.Visible = true;
        pnlShiftScheduleTaggingList.Visible = true;
        pnlShiftScheduleTaggingNew.Visible = false;

        //get employee list
        if (lblUsertype.Text == "admin")
        {
            //get employee list
            GetEmployeeList();
        }
        else
        {
            GetEmployeeListPerApprover();
        }

        GetShiftScheduleTagging();


    }
    protected void lnkHolidays_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlHolidays.Visible = true;
        pnlHolidaysList.Visible = true;
        pnlHolidaysNew.Visible = false;
        GetHolidaysList();
    }
    protected void lnkUploadDTR_Click(object sender, EventArgs e)
    {
        //HidePanel();
        //pnlUploadDTR.Visible = true;
    }
    protected void lnkDailyTimeRecord_Click(object sender, EventArgs e)
    {
        HidePanel();
        //pnlDailyTimeRecord.Visible = true;
        //lblPayrollPeriod.Visible = false;
        //lnkSelectPayrollPeriod.Visible = false;
        //ddlDTREmployee.Visible = false;
        //btnSaveDTR.Visible = false;
        //btnProcessDTR1.Visible = false;
        //rptrDTR.DataSource = null;
        //rptrDTR.DataBind();
        //btnSaveDTR.Visible = false;

        //get employee list
        if (lblUsertype.Text == "admin")
        {
            //get employee list
            GetEmployeeList();
        }
        else
        {
            GetEmployeeListPerApprover();
        }

        //if (lblPayrollPeriod.Text == "")
        //{

        //    ddlDTREmployee.SelectedIndex = 0;
        //    lnkSelectPayrollPeriod.Visible = true;

        //}
        //else
        //{
        //    lblPayrollPeriod.Visible = true;
        //    ddlDTREmployee.Visible = true;
        //    ddlDTREmployee.SelectedIndex = 0;
        //    btnProcessDTR1.Visible = true;

        //}

    }
    protected void lnkPayrollPeriod_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlPayrollPeriod.Visible = true;
        pnlPayrollPeriodList.Visible = true;
        pnlPayrollPeriodNew.Visible = false;

        GetInitialPayrollPeriod();
        GetPayrollPeriod();

    }

    protected void lnkOTRequest_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlOTRequest.Visible = true;
        pnlOTRequestList.Visible = true;
        pnlOTRequestNew.Visible = false;
        GetOTRequests();
    }
    protected void lnkLeaveRequestManagement_Click(object sender, EventArgs e)
    {

    }
    protected void lnkLeaveManagement_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlLeaveApplication.Visible = true;
        pnlLeaveApplicationLists.Visible = true;
        pnlLeaveApplicationAdd.Visible = false;
        GetLeaveApplication();
    }
    //protected void lnkOTManagement_Click(object sender, EventArgs e)
    //{

    //}

    //LEAVE TYPE
    protected void lnkAddLeaveType_Click(object sender, EventArgs e)
    {
        pnlAddLeaveType.Visible = true;
        pnlLeaveTypeLists.Visible = false;
        txtLeaveCode.Text = "";
        txtLeaveDesc.Text = "";
        lblLeaveTypeID.Text = "0";

        lblLeaveTypeMsg.Text = "";
        lblLeaveTypeMsg.Visible = false;
    }
    protected void rptrLeaveType_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label lblLeaveTypeIDR = (Label)e.Item.FindControl("lblLeaveTypeID");
        Label lblLeaveTypeCode = (Label)e.Item.FindControl("lblLeaveTypeCode");
        Label lblLeaveTypeDesc = (Label)e.Item.FindControl("lblLeaveTypeDesc");

        if (e.CommandName == "EditLeaveType")
        {
            pnlLeaveTypeLists.Visible = false;
            pnlAddLeaveType.Visible = true;
            lblLeaveTypeID.Text = lblLeaveTypeIDR.Text;
            txtLeaveCode.Text = lblLeaveTypeCode.Text;
            txtLeaveDesc.Text = lblLeaveTypeDesc.Text;

            lblLeaveTypeMsg.Visible = false;
            lblLeaveTypeMsg.Text = "";
        }
    }
    protected void btnAddLeaveType_Click(object sender, EventArgs e)
    {
        lblLeaveTypeMsg.Visible = false;
        lblLeaveTypeMsg.Text = "";

        if (ValidateLeaveType())
        {
            LeaveManagement _leave = new LeaveManagement();
            _leave.LeaveTypeCode = txtLeaveCode.Text;
            _leave.LeaveTypeDesc = txtLeaveDesc.Text;
            _leave.LeaveTypeID = Convert.ToInt32(lblLeaveTypeID.Text);

            int rowAffected = _leave.AddUpdateLeaveTypes();
            if (rowAffected > 0)
            {
                //BIND
                GetLeaveTypes();
                pnlAddLeaveType.Visible = false;
                pnlLeaveTypeLists.Visible = true;
            }
            else
            {
                lblLeaveTypeMsg.Visible = true;
                lblLeaveTypeMsg.Text = "Failed to save leave type.";
                return;
            }
        }

    }

    private bool ValidateLeaveType()
    {
        lblLeaveTypeMsg.Visible = false;
        lblLeaveTypeMsg.Text = "";

        if (txtLeaveCode.Text == string.Empty)
        {
            lblLeaveTypeMsg.Visible = true;
            lblLeaveTypeMsg.Text = "Leave Code is required.";
            txtLeaveCode.Focus();
            return false;
        }
        if (txtLeaveDesc.Text == string.Empty)
        {
            lblLeaveTypeMsg.Visible = true;
            lblLeaveTypeMsg.Text = "Leave Description is required.";
            txtLeaveDesc.Focus();
            return false;
        }

        //check if leave type already exists
        LeaveManagement _leave = new LeaveManagement();
        _leave.LeaveTypeCode = txtLeaveCode.Text;
        _leave.LeaveTypeDesc = txtLeaveDesc.Text;
        _leave.LeaveTypeID = Convert.ToInt32(lblLeaveTypeID.Text);

        DataTable dtLeaveType = new DataTable();
        dtLeaveType = _leave.CheckLeaveTypesIfExists();

        if (dtLeaveType.Rows.Count > 0)
        {
            lblLeaveTypeMsg.Visible = true;
            lblLeaveTypeMsg.Text = "Leave Code already exists.";
            txtLeaveCode.Focus();
            return false;
        }
        return true;
    }
    private void GetLeaveTypes()
    {
        DataTable dtLeave = new DataTable();
        LeaveManagement _leave = new LeaveManagement();
        dtLeave = _leave.GetAllLeaveTypes();

        rptrLeaveType.DataSource = null;
        rptrLeaveType.DataSource = dtLeave;
        rptrLeaveType.DataBind();
    }

    protected void btnbackLeaveType_Click(object sender, EventArgs e)
    {
        GetLeaveTypes();
        pnlAddLeaveType.Visible = false;
        pnlLeaveTypeLists.Visible = true;
    }
    protected void lnkLeaveCredits_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlLeaveCredits.Visible = true;
        pnlLeaveCreditsList.Visible = true;

        GetLeaveCredits();
    }
    private void GetLeaveCredits()
    {
        DataTable dtLeaveCredit = new DataTable();
        LeaveManagement _leave = new LeaveManagement();
        dtLeaveCredit = _leave.GetAllLeaveCredits();

        rptrLeaveCreditsList.DataSource = null;
        rptrLeaveCreditsList.DataSource = dtLeaveCredit;
        rptrLeaveCreditsList.DataBind();
    }
    private void GetEmployeeList()
    {
        DataTable dtEmployee = new DataTable();
        Employee _employee = new Employee();
        dtEmployee = _employee.EmployeeList();

        //LEAVE CREDITS
        ddlLCEmployee.DataSource = null;
        ddlLCEmployee.DataSource = dtEmployee;
        ddlLCEmployee.DataValueField = "empId";
        ddlLCEmployee.DataTextField = "FullName";
        ddlLCEmployee.DataBind();

        ddlLCEmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlLCEmployee.SelectedIndex = 0;

        //LEAVE APPLICATION
        ddlLAEmployee.DataSource = null;
        ddlLAEmployee.DataSource = dtEmployee;
        ddlLAEmployee.DataValueField = "empId";
        ddlLAEmployee.DataTextField = "FullName";
        ddlLAEmployee.DataBind();

        ddlLAEmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlLAEmployee.SelectedIndex = 0;

        ddlOTRequestEmployee.DataSource = null;
        ddlOTRequestEmployee.DataSource = dtEmployee;
        ddlOTRequestEmployee.DataValueField = "empId";
        ddlOTRequestEmployee.DataTextField = "FullName";
        ddlOTRequestEmployee.DataBind();

        ddlOTRequestEmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlOTRequestEmployee.SelectedIndex = 0;

        ddlEmployee.DataSource = null;
        ddlEmployee.DataSource = dtEmployee;
        ddlEmployee.DataValueField = "empId";
        ddlEmployee.DataTextField = "FullName";
        ddlEmployee.DataBind();

        ddlEmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlEmployee.SelectedIndex = 0;

        //EMPLOYEE SHIFT SCHEDULE
        ddlEmployeeShiftSchedule.DataSource = null;
        ddlEmployeeShiftSchedule.DataSource = dtEmployee;
        ddlEmployeeShiftSchedule.DataValueField = "empId";
        ddlEmployeeShiftSchedule.DataTextField = "FullName";
        ddlEmployeeShiftSchedule.DataBind();

        ddlEmployeeShiftSchedule.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlEmployeeShiftSchedule.SelectedIndex = 0;

        ddlEmployeeShiftScheduleSearch.DataSource = null;
        ddlEmployeeShiftScheduleSearch.DataSource = dtEmployee;
        ddlEmployeeShiftScheduleSearch.DataValueField = "empId";
        ddlEmployeeShiftScheduleSearch.DataTextField = "FullName";
        ddlEmployeeShiftScheduleSearch.DataBind();

        ddlEmployeeShiftScheduleSearch.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlEmployeeShiftScheduleSearch.SelectedIndex = 0;

        //CHANGE SHEDULE 

        ddlChangeScheduleEmployee.DataSource = null;
        ddlChangeScheduleEmployee.DataSource = dtEmployee;
        ddlChangeScheduleEmployee.DataValueField = "empId";
        ddlChangeScheduleEmployee.DataTextField = "FullName";
        ddlChangeScheduleEmployee.DataBind();

        ddlChangeScheduleEmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlChangeScheduleEmployee.SelectedIndex = 0;

        ////DTR
        //ddlDTREmployee.DataSource = null;
        //ddlDTREmployee.DataSource = dtEmployee;
        //ddlDTREmployee.DataValueField = "empId";
        //ddlDTREmployee.DataTextField = "FullName";
        //ddlDTREmployee.DataBind();

        //ddlDTREmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        //ddlDTREmployee.SelectedIndex = 0;

        //DTR
        ddlDTREmployeeDTR.DataSource = null;
        ddlDTREmployeeDTR.DataSource = dtEmployee;
        ddlDTREmployeeDTR.DataValueField = "empId";
        ddlDTREmployeeDTR.DataTextField = "FullName";
        ddlDTREmployeeDTR.DataBind();

        ddlDTREmployeeDTR.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlDTREmployeeDTR.SelectedIndex = 0;



        //APPROVER
        ddlApprover.DataSource = null;
        ddlApprover.DataSource = dtEmployee;
        ddlApprover.DataValueField = "empId";
        ddlApprover.DataTextField = "FullName";
        ddlApprover.DataBind();

        ddlApprover.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlApprover.SelectedIndex = 0;

        //DTR REPORT
        ddlEmployeeDTR.DataSource = null;
        ddlEmployeeDTR.DataSource = dtEmployee;
        ddlEmployeeDTR.DataValueField = "empId";
        ddlEmployeeDTR.DataTextField = "FullName";
        ddlEmployeeDTR.DataBind();

        ddlEmployeeDTR.Items.Insert(0, new ListItem("All Employees", String.Empty));
        //ddlEmployeeDTR.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlEmployeeDTR.SelectedIndex = 0;

        //LEAVE REPORT
        ddlEmployeeLeave.DataSource = null;
        ddlEmployeeLeave.DataSource = dtEmployee;
        ddlEmployeeLeave.DataValueField = "empId";
        ddlEmployeeLeave.DataTextField = "FullName";
        ddlEmployeeLeave.DataBind();

        ddlEmployeeLeave.Items.Insert(0, new ListItem("All Employees", String.Empty));
        ddlEmployeeLeave.SelectedIndex = 0;

    }
    private void GetEmployeeListPerApprover()
    {
        DataTable dtEmployee = new DataTable();
        Employee _employee = new Employee();
        _employee.EmployeeID = lblUserID.Text;
        dtEmployee = _employee.EmployeeListPerApprover();

        //LEAVE CREDITS
        ddlLCEmployee.DataSource = null;
        ddlLCEmployee.DataSource = dtEmployee;
        ddlLCEmployee.DataValueField = "empId";
        ddlLCEmployee.DataTextField = "FullName";
        ddlLCEmployee.DataBind();

        ddlLCEmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlLCEmployee.SelectedIndex = 0;

        //LEAVE APPLICATION
        ddlLAEmployee.DataSource = null;
        ddlLAEmployee.DataSource = dtEmployee;
        ddlLAEmployee.DataValueField = "empId";
        ddlLAEmployee.DataTextField = "FullName";
        ddlLAEmployee.DataBind();

        ddlLAEmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlLAEmployee.SelectedIndex = 0;

        //OT REQUEST
        ddlOTRequestEmployee.DataSource = null;
        ddlOTRequestEmployee.DataSource = dtEmployee;
        ddlOTRequestEmployee.DataValueField = "empId";
        ddlOTRequestEmployee.DataTextField = "FullName";
        ddlOTRequestEmployee.DataBind();

        ddlOTRequestEmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlOTRequestEmployee.SelectedIndex = 0;

        //LOGS
        ddlEmployee.DataSource = null;
        ddlEmployee.DataSource = dtEmployee;
        ddlEmployee.DataValueField = "empId";
        ddlEmployee.DataTextField = "FullName";
        ddlEmployee.DataBind();

        ddlEmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlEmployee.SelectedIndex = 0;

        //EMPLOYEE SHIFT SCHEDULE
        ddlEmployeeShiftSchedule.DataSource = null;
        ddlEmployeeShiftSchedule.DataSource = dtEmployee;
        ddlEmployeeShiftSchedule.DataValueField = "empId";
        ddlEmployeeShiftSchedule.DataTextField = "FullName";
        ddlEmployeeShiftSchedule.DataBind();

        ddlEmployeeShiftSchedule.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlEmployeeShiftSchedule.SelectedIndex = 0;

        ddlEmployeeShiftScheduleSearch.DataSource = null;
        ddlEmployeeShiftScheduleSearch.DataSource = dtEmployee;
        ddlEmployeeShiftScheduleSearch.DataValueField = "empId";
        ddlEmployeeShiftScheduleSearch.DataTextField = "FullName";
        ddlEmployeeShiftScheduleSearch.DataBind();

        ddlEmployeeShiftScheduleSearch.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlEmployeeShiftScheduleSearch.SelectedIndex = 0;

        //CHANGE SHEDULE 

        ddlChangeScheduleEmployee.DataSource = null;
        ddlChangeScheduleEmployee.DataSource = dtEmployee;
        ddlChangeScheduleEmployee.DataValueField = "empId";
        ddlChangeScheduleEmployee.DataTextField = "FullName";
        ddlChangeScheduleEmployee.DataBind();

        ddlChangeScheduleEmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlChangeScheduleEmployee.SelectedIndex = 0;


        ////DTR
        //ddlDTREmployee.DataSource = null;
        //ddlDTREmployee.DataSource = dtEmployee;
        //ddlDTREmployee.DataValueField = "empId";
        //ddlDTREmployee.DataTextField = "FullName";
        //ddlDTREmployee.DataBind();

        //ddlDTREmployee.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        //ddlDTREmployee.SelectedIndex = 0;

        //DTR
        ddlDTREmployeeDTR.DataSource = null;
        ddlDTREmployeeDTR.DataSource = dtEmployee;
        ddlDTREmployeeDTR.DataValueField = "empId";
        ddlDTREmployeeDTR.DataTextField = "FullName";
        ddlDTREmployeeDTR.DataBind();

        ddlDTREmployeeDTR.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlDTREmployeeDTR.SelectedIndex = 0;

        //APPROVER
        ddlApprover.DataSource = null;
        ddlApprover.DataSource = dtEmployee;
        ddlApprover.DataValueField = "empId";
        ddlApprover.DataTextField = "FullName";
        ddlApprover.DataBind();

        ddlApprover.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlApprover.SelectedIndex = 0;

        //DTR REPORT
        ddlEmployeeDTR.DataSource = null;
        ddlEmployeeDTR.DataSource = dtEmployee;
        ddlEmployeeDTR.DataValueField = "empId";
        ddlEmployeeDTR.DataTextField = "FullName";
        ddlEmployeeDTR.DataBind();

        ddlEmployeeDTR.Items.Insert(0, new ListItem("Select Employee", String.Empty));
        ddlEmployeeDTR.SelectedIndex = 0;

        //LEAVE REPORT
        ddlEmployeeLeave.DataSource = null;
        ddlEmployeeLeave.DataSource = dtEmployee;
        ddlEmployeeLeave.DataValueField = "empId";
        ddlEmployeeLeave.DataTextField = "FullName";
        ddlEmployeeLeave.DataBind();

        ddlEmployeeLeave.Items.Insert(0, new ListItem("All Employees", String.Empty));
        ddlEmployeeLeave.SelectedIndex = 0;
    }
    private DataTable GetEmployees()
    {
        DataTable dtEmployee = new DataTable();
        Employee _employee = new Employee();
        dtEmployee = _employee.EmployeeList();
        return dtEmployee;
    }
    private void GetLeaveList()
    {
        DataTable dtLeave = new DataTable();
        LeaveManagement _Leave = new LeaveManagement();
        dtLeave = _Leave.GetAllLeaveTypes();

        //LEAVE CREDITS
        ddlLCLeave.DataSource = null;
        ddlLCLeave.DataSource = dtLeave;
        ddlLCLeave.DataValueField = "id";
        ddlLCLeave.DataTextField = "description";
        ddlLCLeave.DataBind();

        ddlLCLeave.Items.Insert(0, new ListItem("Select Leave", String.Empty));
        ddlLCLeave.SelectedIndex = 0;

        //LEAVE APPLICATION
        ddlLALeave.DataSource = null;
        ddlLALeave.DataSource = dtLeave;
        ddlLALeave.DataValueField = "id";
        ddlLALeave.DataTextField = "description";
        ddlLALeave.DataBind();

        ddlLALeave.Items.Insert(0, new ListItem("Select Leave", String.Empty));
        ddlLALeave.SelectedIndex = 0;


        //LEAVE APPLICATION
        ddlLeaveType.DataSource = null;
        ddlLeaveType.DataSource = dtLeave;
        ddlLeaveType.DataValueField = "id";
        ddlLeaveType.DataTextField = "description";
        ddlLeaveType.DataBind();

        ddlLeaveType.Items.Insert(0, new ListItem("All Leaves", String.Empty));
        ddlLeaveType.SelectedIndex = 0;

    }

    private bool ValidateLeaveCredit()
    {
        lblLeaveCreditMsg.Visible = true;

        if (ddlLCEmployee.SelectedValue == "")
        {
            lblLeaveCreditMsg.Text = "Select employee";
            ddlLCEmployee.Focus();
            return false;
        }
        if (ddlLCLeave.SelectedValue == "")
        {
            lblLeaveCreditMsg.Text = "Select leave";
            ddlLCLeave.Focus();
            return false;
        }
        try
        {
            DateTime dtStart = new DateTime();
            dtStart = Convert.ToDateTime(txtLCDateStart.Text);
        }
        catch
        {
            lblLeaveCreditMsg.Text = "Invalid Date Start";
            txtLCDateStart.Focus();
            return false;
        }
        try
        {
            DateTime dtEnd = new DateTime();
            dtEnd = Convert.ToDateTime(txtLCEndDate.Text);
        }
        catch
        {
            lblLeaveCreditMsg.Text = "Invalid End Date";
            txtLCEndDate.Focus();
            return false;
        }


        try
        {
            int _leaveCredit = 0;
            _leaveCredit = Convert.ToInt32(txtLCLeaveCredit.Text);
            if (_leaveCredit > 0)
            {

            }
            else
            {
                lblLeaveCreditMsg.Text = "Invalid Leave Credit. Must be greater than 0.";
                txtLCLeaveCredit.Focus();
                return false;
            }
        }
        catch
        {
            lblLeaveCreditMsg.Text = "Invalid Leave Credit. Must be numeric.";
            txtLCLeaveCredit.Focus();
            return false;
        }

        //check if leave credit already exists; empID,leave, date start, end date
        LeaveManagement _leave = new LeaveManagement();
        _leave.ID = Convert.ToInt32(lblLeaveCreditID.Text);
        _leave.EmpID = ddlLCEmployee.SelectedValue;
        _leave.LeaveTypeID = Convert.ToInt32(ddlLCLeave.SelectedValue);
        _leave.DateStart = Convert.ToDateTime(txtLCDateStart.Text);
        _leave.EndDate = Convert.ToDateTime(txtLCEndDate.Text);

        DataTable dtLeave = new DataTable();
        dtLeave = _leave.CheckLeaveCreditIfExists();

        if (dtLeave.Rows.Count > 0)
        {
            lblLeaveCreditMsg.Text = "Leave Credit: " + ddlLCLeave.SelectedItem.Text + " of " + ddlLCEmployee.SelectedItem.Text + " already exists.";
            ddlLCLeave.Focus();
            return false;
        }

        lblLeaveCreditMsg.Visible = false;
        lblLeaveCreditMsg.Text = "";
        return true;
    }
    protected void lnkAddLeaveCredits_Click(object sender, EventArgs e)
    {
        pnlLeaveCreditsList.Visible = false;
        pnlLeaveCreditsAdd.Visible = true;

        lblLeaveCreditMsg.Visible = false;
        lblLeaveCreditMsg.Text = "";
        lblLeaveCreditID.Text = "0";

        if (lblUsertype.Text == "admin")
        {
            //get employee list
            GetEmployeeList();
        }
        else
        {
            GetEmployeeListPerApprover();
        }
        //get leave list
        GetLeaveList();

        txtLCDateStart.Text = "";
        txtLCEndDate.Text = "";
        txtLCLeaveCredit.Text = "";
    }

    protected void btnLeaveCreditAdd_Click(object sender, EventArgs e)
    {
        if (ValidateLeaveCredit())
        {
            LeaveManagement _leave = new LeaveManagement();
            _leave.ID = Convert.ToInt32(lblLeaveCreditID.Text);
            _leave.EmpID = ddlLCEmployee.SelectedValue;
            _leave.LeaveTypeID = Convert.ToInt32(ddlLCLeave.SelectedValue);
            _leave.DateStart = Convert.ToDateTime(txtLCDateStart.Text);
            _leave.EndDate = Convert.ToDateTime(txtLCEndDate.Text);
            _leave.LeaveCredit = Convert.ToInt32(txtLCLeaveCredit.Text);

            int rowAffected = _leave.AddUpdateLeaveCredits();
            if (rowAffected > 0)
            {
                lblLeaveCreditMsg.Visible = false;
                //BIND
                GetLeaveCredits();
                pnlLeaveCreditsAdd.Visible = false;
                pnlLeaveCreditsList.Visible = true;
            }
            else
            {
                lblLeaveCreditMsg.Visible = true;
                lblLeaveCreditMsg.Text = "Failed to save leave credits";
                return;
            }
        }
    }
    protected void btnLeaveCreditBack_Click(object sender, EventArgs e)
    {
        pnlLeaveCreditsList.Visible = true;
        pnlLeaveCreditsAdd.Visible = false;

        //BIND
        GetLeaveCredits();
    }


    protected void rptrLeaveCreditsList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        lblLeaveCreditMsg.Visible = false;
        lblLeaveCreditMsg.Text = "";

        pnlLeaveCreditsAdd.Visible = true;
        pnlLeaveCreditsList.Visible = false;

        Label lblLCID = (Label)e.Item.FindControl("lblLCID");
        Label lblLCEmployeeID = (Label)e.Item.FindControl("lblLCEmployeeID");
        Label lblLCLeaveID = (Label)e.Item.FindControl("lblLCLeaveID");
        Label lblLCDateStart = (Label)e.Item.FindControl("lblLCDateStart");
        Label lblLCDateEnd = (Label)e.Item.FindControl("lblLCDateEnd");
        Label lblLCLeaveCredit = (Label)e.Item.FindControl("lblLCLeaveCredit");


        if (e.CommandName == "Edit")
        {
            if (lblUsertype.Text == "admin")
            {
                //get employee list
                GetEmployeeList();
            }
            else
            {
                GetEmployeeListPerApprover();
            }
            //get leave list
            GetLeaveList();

            lblLeaveCreditID.Text = lblLCID.Text;
            ddlLCEmployee.SelectedValue = lblLCEmployeeID.Text;
            ddlLCLeave.SelectedValue = lblLCLeaveID.Text;
            txtLCDateStart.Text = lblLCDateStart.Text;
            txtLCEndDate.Text = lblLCDateEnd.Text;
            txtLCLeaveCredit.Text = lblLCLeaveCredit.Text;
        }
    }




    //LEAVE APPLICATION (LEAVE MANAGEMENT)
    protected void lnkLeaveApplicationAdd_Click(object sender, EventArgs e)
    {
        pnlLeaveApplicationLists.Visible = false;
        pnlLeaveApplicationAdd.Visible = true;

        lblLeaveApplicationMsg.Visible = false;
        lblLeaveApplicationMsg.Text = "";
        lblLeaveApplicationID.Text = "0";

        if (lblUsertype.Text == "admin")
        {
            //get employee list
            GetEmployeeList();
        }
        else
        {
            GetEmployeeListPerApprover();
        }
        //get leave list
        GetLeaveList();

        txtLADateStart.Text = "";
        txtLAEndDate.Text = "";
        txtLANoOfHours.Text = "0";
        txtLAReason.Text = "";

        if (lblUsertype.Text != "admin" && lblUsertype.Text != "supervisor")
        {
            ddlLAEmployee.SelectedValue = lblUserID.Text;
            ddlLAEmployee.Enabled = false;
        }

    }
    protected void btnLeaveApplicationAdd_Click(object sender, EventArgs e)
    {
        if (ValidateLeaveApplication())
        {
            LeaveManagement _leave = new LeaveManagement();
            _leave.ID = Convert.ToInt32(lblLeaveApplicationID.Text);
            _leave.EmpID = ddlLAEmployee.SelectedValue;
            _leave.LeaveTypeID = Convert.ToInt32(ddlLALeave.SelectedValue);
            _leave.DateStart = Convert.ToDateTime(txtLADateStart.Text);
            _leave.EndDate = Convert.ToDateTime(txtLAEndDate.Text);
            _leave.Reason = txtLAReason.Text;
            _leave.NoOfHours = Convert.ToDecimal(txtLANoOfHours.Text);
            _leave.LeaveStatus = "";
            _leave.UserID = lblUserID.Text;

            if (lblLeaveApplicationID.Text == "0")
            {
                _leave.ApprovedDate = null;
            }
            else
            {
                _leave.ApprovedDate = DateTime.Now;
                _leave.ApprovedBy = lblUserID.Text;
            }

            //_leave.LeaveCredit = 0;


            int rowAffected = _leave.AddUpdateLeaveApplications();
            if (rowAffected > 0)
            {
                lblLeaveCreditMsg.Visible = false;
                //BIND
                GetLeaveApplication();
                pnlLeaveApplicationAdd.Visible = false;
                pnlLeaveApplicationLists.Visible = true;
            }
            else
            {
                lblLeaveApplicationMsg.Visible = true;
                lblLeaveApplicationMsg.Text = "Failed to save leave application";
                return;
            }

        }
    }
    protected void btnLeaveApplicationBack_Click(object sender, EventArgs e)
    {
        pnlLeaveApplicationLists.Visible = true;
        pnlLeaveApplicationAdd.Visible = false;

        //BIND
        GetLeaveApplication();
    }
    private void GetLeaveApplication()
    {
        DataTable dtLeaveApplication = new DataTable();
        LeaveManagement _leave = new LeaveManagement();
        dtLeaveApplication = _leave.GetAllLeaveApplication(lblUsertype.Text, lblUserID.Text);

        rptrLeaveApplication.DataSource = null;
        rptrLeaveApplication.DataSource = dtLeaveApplication;
        rptrLeaveApplication.DataBind();
    }
    private bool ValidateLeaveApplication()
    {
        lblLeaveApplicationMsg.Visible = true;

        if (ddlLAEmployee.SelectedValue == "")
        {
            lblLeaveApplicationMsg.Text = "Select employee";
            ddlLAEmployee.Focus();
            return false;
        }
        if (ddlLALeave.SelectedValue == "")
        {
            lblLeaveApplicationMsg.Text = "Select leave";
            ddlLALeave.Focus();
            return false;
        }
        try
        {
            DateTime dtStart = new DateTime();
            dtStart = Convert.ToDateTime(txtLADateStart.Text);
        }
        catch
        {
            lblLeaveApplicationMsg.Text = "Invalid Date Start";
            txtLADateStart.Focus();
            return false;
        }
        try
        {
            DateTime dtEnd = new DateTime();
            dtEnd = Convert.ToDateTime(txtLAEndDate.Text);
        }
        catch
        {
            lblLeaveApplicationMsg.Text = "Invalid End Date";
            txtLAEndDate.Focus();
            return false;
        }
        try
        {
            decimal _noOfHours = 0;
            _noOfHours = Convert.ToDecimal(txtLANoOfHours.Text);
            if (_noOfHours < 0)
            {
                lblLeaveApplicationMsg.Text = "Invalid number of hours.";
                txtLANoOfHours.Focus();
                return false;
            }
        }
        catch
        {
            lblLeaveApplicationMsg.Text = "Invalid number of hours.";
            txtLANoOfHours.Focus();
            return false;
        }
        if (txtLAReason.Text == "")
        {
            lblLeaveApplicationMsg.Text = "Please enter reason";
            txtLAReason.Focus();
            return false;
        }

        //check if leave Application already exists; empID,leave, date start, end date
        LeaveManagement _leave = new LeaveManagement();
        _leave.ID = Convert.ToInt32(lblLeaveApplicationID.Text);
        _leave.EmpID = ddlLAEmployee.SelectedValue;
        _leave.LeaveTypeID = Convert.ToInt32(ddlLALeave.SelectedValue);
        _leave.DateStart = Convert.ToDateTime(txtLADateStart.Text);
        _leave.EndDate = Convert.ToDateTime(txtLAEndDate.Text);
        _leave.NoOfHours = Convert.ToDecimal(txtLANoOfHours.Text);
        _leave.Reason = txtLAReason.Text;


        DataTable dtLeave = new DataTable();
        dtLeave = _leave.CheckLeaveApplicationIfExists();

        if (dtLeave.Rows.Count > 0)
        {
            lblLeaveApplicationMsg.Text = "Leave Application: " + ddlLALeave.SelectedItem.Text + " of " + ddlLAEmployee.SelectedItem.Text + " already exists.";
            ddlLALeave.Focus();
            return false;
        }

        lblLeaveApplicationMsg.Visible = false;
        lblLeaveApplicationMsg.Text = "";
        return true;
    }
    protected void rptrLeaveApplication_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label lblLAID = (Label)e.Item.FindControl("lblLAID");
        Label lblLAEmpID = (Label)e.Item.FindControl("lblLAEmpID");
        Label lblLALeaveID = (Label)e.Item.FindControl("lblLALeaveID");
        Label lblLALastName = (Label)e.Item.FindControl("lblLALastName");
        Label lblLAFirstName = (Label)e.Item.FindControl("lblLAFirstName");

        Label lblLADateStart = (Label)e.Item.FindControl("lblLADateStart");
        Label lblLADateEnd = (Label)e.Item.FindControl("lblLADateEnd");
        Label lblLAFormCredit = (Label)e.Item.FindControl("lblLAFormCredit");
        Label lblLAReason = (Label)e.Item.FindControl("lblLAReason");
        if (e.CommandName == "Deny")
        {
            pnlLAApprove.Visible = true;
            lblLAApproveID.Text = lblLAID.Text;
            lblLAApproveMsg.Text = "";
            lblLAApproveMsg.Visible = false;
        }
        if (e.CommandName == "Update")
        {
            lblLeaveApplicationMsg.Visible = false;
            lblLeaveApplicationMsg.Text = "";

            pnlLeaveApplicationAdd.Visible = true;
            pnlLeaveApplicationLists.Visible = false;
            if (lblUsertype.Text == "admin")
            {
                //get employee list
                GetEmployeeList();
            }
            else
            {
                GetEmployeeListPerApprover();
            }
            //get leave list
            GetLeaveList();

            lblLeaveApplicationID.Text = lblLAID.Text;
            ddlLAEmployee.SelectedValue = lblLAEmpID.Text;
            ddlLALeave.SelectedValue = lblLALeaveID.Text;
            txtLADateStart.Text = lblLADateStart.Text;
            txtLAEndDate.Text = lblLADateEnd.Text;
            txtLANoOfHours.Text = lblLAFormCredit.Text;
            txtLAReason.Text = lblLAReason.Text;

            //
            if (lblUsertype.Text != "admin" && lblUsertype.Text != "supervisor")
            {
                ddlOTRequestEmployee.Enabled = false;
            }
            else
            {
                btnLeaveApplicationAdd.Text = "Approve";
            }

        }
    }
    protected void rptrLeaveApplication_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //LinkButton lnkLAApprove = (LinkButton)e.Item.FindControl("lnkLAApprove");
        LinkButton lnkLADeny = (LinkButton)e.Item.FindControl("lnkLADeny");
        LinkButton lnkLAUpdate = (LinkButton)e.Item.FindControl("lnkLAUpdate");
        Label lblLAStatus = (Label)e.Item.FindControl("lblLAStatus");
        Label lblLAEmpID = (Label)e.Item.FindControl("lblLAEmpID");

        //lnkLAApprove.Visible = false;
        lnkLADeny.Visible = false;
        if (lblUsertype.Text == "supervisor" || lblUsertype.Text == "admin")
        {
            if (lblUsertype.Text == "supervisor" && lblLAEmpID.Text == lblUserID.Text)
            {
                lnkLADeny.Visible = false;
                lnkLAUpdate.Visible = false;
            }
            else
            {
                //lnkLAApprove.Visible = true;
                lnkLADeny.Visible = true;
                lnkLAUpdate.Text = "Update & Approve";

                if (lblLAStatus.Text == "Approved")
                {
                    lnkLADeny.Visible = true;
                    lnkLAUpdate.Visible = false;
                }
                if (lblLAStatus.Text == "Denied")
                {
                    lnkLAUpdate.Visible = true;
                    lnkLADeny.Visible = false;
                }
            }
        }
        else
        {
            if (lblLAStatus.Text == "Approved" || lblLAStatus.Text == "Denied")
            {
                lnkLAUpdate.Visible = false;
                lnkLADeny.Visible = false;
            }
        }


        //if (lblLAStatus.Text == "Approved" || lblLAStatus.Text == "Denied")
        //{
        //    lnkLAUpdate.Visible = false;
        //    //lnkLAApprove.Visible = false;
        //    lnkLADeny.Visible = false;
        //}
    }

    //OT REQUEST
    protected void lnkAddOTRequest_Click(object sender, EventArgs e)
    {
        pnlOTRequestList.Visible = false;
        pnlOTRequestNew.Visible = true;

        lblOTRequestMsg.Visible = false;
        lblOTRequestMsg.Text = "";
        lblOTRequestID.Text = "0";

        if (lblUsertype.Text == "admin")
        {
            //get employee list
            GetEmployeeList();
        }
        else
        {
            GetEmployeeListPerApprover();
        }

        ClearOTRequest();

        //
        if (lblUsertype.Text != "admin" && lblUsertype.Text != "supervisor")
        {
            ddlOTRequestEmployee.SelectedValue = lblUserID.Text;
            ddlOTRequestEmployee.Enabled = false;
        }


    }
    private void GetOTRequests()
    {
        OTManagement _ot = new OTManagement();
        DataTable dt = new DataTable();
        dt = _ot.GetAllOTRequests(lblUsertype.Text, lblUserID.Text);

        rptrOTRequests.DataSource = null;
        rptrOTRequests.DataSource = dt;
        rptrOTRequests.DataBind();

    }
    private void ClearOTRequest()
    {
        txtCreditDate.Text = "";
        txtTimeStart.Text = "00:00";
        txtTimeEnd.Text = "00:00";
        txtBreakStart.Text = "00:00";
        txtBreakEnd.Text = "00:00";
        txtTotalHrs.Text = "";
        txtChargeTo.Text = "";
        txtReason.Text = "";
    }
    private bool ValidateOTRequest()
    {
        lblOTRequestMsg.Visible = true;

        if (ddlOTRequestEmployee.SelectedValue == "")
        {
            lblOTRequestMsg.Text = "Select employee.";
            ddlOTRequestEmployee.Focus();
            return false;
        }
        if (txtCreditDate.Text == "")
        {
            lblOTRequestMsg.Text = "Enter the credit date.";
            txtCreditDate.Focus();
            return false;
        }
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txtCreditDate.Text);
        }
        catch
        {
            lblOTRequestMsg.Text = "Invalid credit date.";
            txtCreditDate.Focus();
            return false;
        }
        if (txtTimeStart.Text == "")
        {
            lblOTRequestMsg.Text = "Enter the time start.";
            txtTimeStart.Focus();
            return false;
        }
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txtCreditDate.Text + " " + txtTimeStart.Text);//
            txtTimeStart.Text = dt.ToString();
        }
        catch
        {
            lblOTRequestMsg.Text = "Invalid time start.";
            txtTimeStart.Focus();
            return false;
        }
        if (txtTimeEnd.Text == "")
        {
            lblOTRequestMsg.Text = "Enter the time end.";
            txtTimeEnd.Focus();
            return false;
        }
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txtCreditDate.Text + " " + txtTimeEnd.Text);//
        }
        catch
        {
            lblOTRequestMsg.Text = "Invalid time end.";
            txtTimeEnd.Focus();
            return false;
        }

        if (txtBreakStart.Text != "")
        {
            try
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(txtCreditDate.Text + " " + txtBreakStart.Text);//
            }
            catch
            {
                lblOTRequestMsg.Text = "Invalid break start.";
                txtBreakStart.Focus();
                return false;
            }
        }
        if (txtBreakEnd.Text != "")
        {
            try
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(txtCreditDate.Text + " " + txtBreakEnd.Text);//
            }
            catch
            {
                lblOTRequestMsg.Text = "Invalid break end.";
                txtBreakEnd.Focus();
                return false;
            }
        }
        //if (txtBreakStart.Text == "")
        //{
        //    lblOTRequestMsg.Text = "Enter the break start.";
        //    txtBreakStart.Focus();
        //    return false;
        //}
        //try
        //{
        //    DateTime dt = new DateTime();
        //    dt = Convert.ToDateTime(txtCreditDate.Text + " " + txtBreakStart.Text);//
        //}
        //catch
        //{
        //    lblOTRequestMsg.Text = "Invalid break start.";
        //    txtBreakStart.Focus();
        //    return false;
        //}
        //if (txtBreakEnd.Text == "")
        //{
        //    lblOTRequestMsg.Text = "Enter the break end.";
        //    txtBreakEnd.Focus();
        //    return false;
        //}
        //try
        //{
        //    DateTime dt = new DateTime();
        //    dt = Convert.ToDateTime(txtCreditDate.Text + " " + txtBreakEnd.Text);//
        //}
        //catch
        //{
        //    lblOTRequestMsg.Text = "Invalid break end.";
        //    txtBreakEnd.Focus();
        //    return false;
        //}
        if (txtTotalHrs.Text == "")
        {
            lblOTRequestMsg.Text = "Enter the total hours.";
            txtTotalHrs.Focus();
            return false;
        }
        try
        {
            decimal _totalHrs = 0;
            _totalHrs = Convert.ToDecimal(txtTotalHrs.Text);

        }
        catch
        {
            lblOTRequestMsg.Text = "Invalid total hours.";
            txtTotalHrs.Focus();
            return false;
        }
        if (txtReason.Text == "")
        {
            lblOTRequestMsg.Text = "Enter the reason.";
            txtReason.Focus();
            return false;
        }
        if (txtChargeTo.Text == "")
        {
            lblOTRequestMsg.Text = "Enter charge to.";
            txtChargeTo.Focus();
            return false;
        }
        return true;
    }
    protected void btnOTRequestAdd_Click(object sender, EventArgs e)
    {
        if (ValidateOTRequest())
        {
            OTManagement _ot = new OTManagement();
            _ot.ID = Convert.ToInt32(lblOTRequestID.Text);
            _ot.EmpID = ddlOTRequestEmployee.SelectedValue;
            _ot.CreditDate = Convert.ToDateTime(txtCreditDate.Text);
            _ot.TimeStart = Convert.ToDateTime(txtTimeStart.Text);
            _ot.TimeEnd = Convert.ToDateTime(txtTimeEnd.Text);

            if (txtBreakStart.Text == "")
                _ot.BreakStart = null;
            else
                _ot.BreakStart = Convert.ToDateTime(txtBreakStart.Text);

            if (txtBreakEnd.Text == "")
                _ot.BreakEnd = null;
            else
                _ot.BreakEnd = Convert.ToDateTime(txtBreakEnd.Text);

            _ot.TotalHrs = Convert.ToDecimal(txtTotalHrs.Text);
            _ot.Reason = txtReason.Text;
            _ot.ChargeTo = txtChargeTo.Text;


            _ot.UserID = lblUserID.Text;

            int rowAffected = _ot.AddUpdateOTRequest();

            if (rowAffected > 0)
            {
                lblOTRequestMsg.Visible = false;
                //BIND
                GetOTRequests();
                pnlOTRequestNew.Visible = false;
                pnlOTRequestList.Visible = true;
            }
            else
            {
                lblOTRequestMsg.Visible = true;
                lblOTRequestMsg.Text = "Failed to save OT request.";
                return;
            }
        }
    }
    protected void btnOTRequestBack_Click(object sender, EventArgs e)
    {
        pnlOTRequestList.Visible = true;
        pnlOTRequestNew.Visible = false;

        //BIND
        GetOTRequests();
    }
    protected void rptrOTRequests_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

        Label lblID = (Label)e.Item.FindControl("lblID");
        Label lblEmployeeID = (Label)e.Item.FindControl("lblEmployeeID");
        Label lblCreditDate = (Label)e.Item.FindControl("lblCreditDate");
        Label lblTimeStart = (Label)e.Item.FindControl("lblTimeStart");
        Label lblTimeEnd = (Label)e.Item.FindControl("lblTimeEnd");
        Label lblBreakStart = (Label)e.Item.FindControl("lblBreakStart");
        Label lblBreakEnd = (Label)e.Item.FindControl("lblBreakEnd");
        Label lblReason = (Label)e.Item.FindControl("lblReason");
        Label lblTotalHrs = (Label)e.Item.FindControl("lblTotalHrs");
        Label lblChargeTo = (Label)e.Item.FindControl("lblChargeTo");



        if (e.CommandName == "Edit")
        {
            lblOTRequestMsg.Visible = false;
            lblOTRequestMsg.Text = "";

            pnlOTRequestNew.Visible = true;
            pnlOTRequestList.Visible = false;

            if (lblUsertype.Text == "admin")
            {
                //get employee list
                GetEmployeeList();
            }
            else
            {
                GetEmployeeListPerApprover();
            }

            lblOTRequestID.Text = lblID.Text;
            ddlOTRequestEmployee.SelectedValue = lblEmployeeID.Text;

            txtCreditDate.Text = lblCreditDate.Text;
            txtTimeStart.Text = lblTimeStart.Text;
            txtTimeEnd.Text = lblTimeEnd.Text;
            txtBreakStart.Text = lblBreakStart.Text;
            txtBreakEnd.Text = lblBreakEnd.Text;
            txtReason.Text = lblReason.Text;
            txtTotalHrs.Text = lblTotalHrs.Text;
            txtChargeTo.Text = lblChargeTo.Text;


            //
            if (lblUsertype.Text != "admin" && lblUsertype.Text != "supervisor")
            {
                ddlOTRequestEmployee.Enabled = false;
            }
            else
            {
                btnOTRequestAdd.Text = "Approve";
            }
        }
        if (e.CommandName == "Approve")
        {
            lblApproveID.Text = lblID.Text;
            lblApproveMsg.Text = "";
            lblApproveMsg.Visible = false;
            pnlApprove.Visible = true;
            lblApproveText.Text = "Approve";
            lnkApproveYes.Text = "Approve";
        }
        if (e.CommandName == "Deny")
        {
            lblApproveID.Text = lblID.Text;
            lblApproveMsg.Text = "";
            lblApproveMsg.Visible = false;
            pnlApprove.Visible = true;
            lblApproveText.Text = "Deny";
            lnkApproveYes.Text = "Deny";

        }

    }
    protected void rptrOTRequests_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblStatus = (Label)e.Item.FindControl("lblStatus");
        LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
        //LinkButton lnkApprove = (LinkButton)e.Item.FindControl("lnkApprove");
        LinkButton lnkDeny = (LinkButton)e.Item.FindControl("lnkDeny");
        Label lblEmployeeID = (Label)e.Item.FindControl("lblEmployeeID");

        //lnkApprove.Visible = false;

        lnkDeny.Visible = false;
        if (lblUsertype.Text == "supervisor" || lblUsertype.Text == "admin")
        {
            if (lblUsertype.Text == "supervisor" && lblEmployeeID.Text == lblUserID.Text)
            {
                lnkDeny.Visible = false;
                lnkEdit.Visible = false;
            }
            else
            {
                //lnkLAApprove.Visible = true;
                lnkDeny.Visible = true;
                lnkEdit.Text = "Update & Approve";

                if (lblStatus.Text == "Approved")
                {
                    lnkDeny.Visible = true;
                    lnkEdit.Visible = false;
                }
                if (lblStatus.Text == "Denied")
                {
                    lnkEdit.Visible = true;
                    lnkDeny.Visible = false;
                }
            }
        }
        else
        {
            if (lblStatus.Text == "Approved" || lblStatus.Text == "Denied")
            {
                lnkEdit.Visible = false;
                lnkDeny.Visible = false;
            }
        }
    }
    protected void lnkApproveCancel_Click(object sender, EventArgs e)
    {
        pnlApprove.Visible = false;
    }
    protected void lnkApproveYes_Click(object sender, EventArgs e)
    {
        if (txtDenyReasons.Text != "")
        {
            OTManagement _ot = new OTManagement();
            _ot.ID = Convert.ToInt32(lblApproveID.Text);
            _ot.UserID = lblUserID.Text;
            int rowAffected = _ot.ApproveOTRequest(lblApproveText.Text, txtDenyReasons.Text);
            if (rowAffected > 0)
            {
                pnlApprove.Visible = false;
                GetOTRequests();
                return;
            }
            else
            {
                lblApproveMsg.Text = "Failed to update request.";
                lblApproveMsg.Visible = true;
                return;
            }
        }
        else
        {
            lblApproveMsg.Text = "Reason is required.";
            lblApproveMsg.Visible = true;
            return;
        }

    }

    //SHIFT SCHEDULE
    private void GetShiftSchedule()
    {
        DataTable dt = new DataTable();
        ShiftSchedule _shift = new ShiftSchedule();
        dt = _shift.GetAllShiftSchedule();

        rptrShiftSchedules.DataSource = null;
        rptrShiftSchedules.DataSource = dt;
        rptrShiftSchedules.DataBind();
    }
    protected void lnkAddShiftSchedule_Click(object sender, EventArgs e)
    {
        pnlShiftScheduleList.Visible = false;
        pnlShiftScheduleNew.Visible = true;

        lblShiftScheduleMsg.Visible = false;
        lblShiftScheduleMsg.Text = "";
        lblShiftScheduleID.Text = "0";

        ClearShiftSchedule();
    }
    private void ClearShiftSchedule()
    {
        txtSSCode.Text = "";
        txtSSDesc.Text = "";
        txtSSTimeInFrom.Text = "00:00";
        txtSSTimeInTo.Text = "00:00";
        txtSSTimeOutFrom.Text = "00:00";
        txtSSTimeOutTo.Text = "00:00";
        txtSSShiftCredit.Text = "";
        txtSSBreakStart.Text = "";
        txtSSBreakEnd.Text = "";
        txtSSBreakCredit.Text = "";

        rbtnFlexibleYes.Checked = false;
        rbtnFlexibleNo.Checked = true;

        rbtnForceCreditYes.Checked = false;
        rbtnForceCreditNo.Checked = true;
    }
    private bool ValidateShiftSchedule()
    {
        lblShiftScheduleMsg.Visible = true;

        if (txtSSCode.Text == "")
        {
            lblShiftScheduleMsg.Text = "Enter the shift code.";
            txtSSCode.Focus();
            return false;
        }
        if (txtSSDesc.Text == "")
        {
            lblShiftScheduleMsg.Text = "Enter the shift description.";
            txtSSDesc.Focus();
            return false;
        }

        //TIME IN
        if (txtSSTimeInFrom.Text == "")
        {
            lblShiftScheduleMsg.Text = "Enter the time in (from).";
            txtSSTimeInFrom.Focus();
            return false;
        }
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txtSSTimeInFrom.Text);
        }
        catch
        {
            lblOTRequestMsg.Text = "Invalid time in (from).";
            txtSSTimeInFrom.Focus();
            return false;
        }
        if (rbtnFlexibleYes.Checked == true)
        {
            if (txtSSTimeInTo.Text == "")
            {
                lblShiftScheduleMsg.Text = "Enter the time in (to).";
                txtSSTimeInTo.Focus();
                return false;
            }
            try
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(txtSSTimeInTo.Text);
            }
            catch
            {
                lblOTRequestMsg.Text = "Invalid time in (to).";
                txtSSTimeInTo.Focus();
                return false;
            }
        }

        //TIME OUT
        if (txtSSTimeOutFrom.Text == "")
        {
            lblShiftScheduleMsg.Text = "Enter the time out (from).";
            txtSSTimeOutFrom.Focus();
            return false;
        }
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txtSSTimeOutFrom.Text);
        }
        catch
        {
            lblOTRequestMsg.Text = "Invalid time out (from).";
            txtSSTimeOutFrom.Focus();
            return false;
        }
        if (rbtnFlexibleYes.Checked == true)
        {
            if (txtSSTimeOutTo.Text == "")
            {
                lblShiftScheduleMsg.Text = "Enter the time out (to).";
                txtSSTimeOutTo.Focus();
                return false;
            }
            try
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(txtSSTimeOutTo.Text);
            }
            catch
            {
                lblOTRequestMsg.Text = "Invalid time out (to).";
                txtSSTimeOutTo.Focus();
                return false;
            }
        }
        if (txtSSShiftCredit.Text == "")
        {
            lblShiftScheduleMsg.Text = "Enter the shift schedule credit.";
            txtSSShiftCredit.Focus();
            return false;
        }
        try
        {
            decimal shiftCredit = Convert.ToDecimal(txtSSShiftCredit.Text);
            txtSSShiftCredit.Text = shiftCredit.ToString("00.00");
        }
        catch
        {
            lblShiftScheduleMsg.Text = "Invalid shift schedule credit.";
            txtSSShiftCredit.Focus();
            return false;
        }
        //BREAK START
        if (txtSSBreakStart.Text != "")
        {
            try
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(txtSSBreakStart.Text);
            }
            catch
            {
                lblOTRequestMsg.Text = "Invalid break start.";
                txtSSBreakStart.Focus();
                return false;
            }

            if (txtSSBreakEnd.Text == "")
            {
                lblOTRequestMsg.Text = "Enter break end.";
                txtSSBreakEnd.Focus();
                return false;
            }
        }
        if (txtSSBreakEnd.Text != "")
        {
            try
            {
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(txtSSBreakEnd.Text);
            }
            catch
            {
                lblOTRequestMsg.Text = "Invalid break end.";
                txtSSBreakEnd.Focus();
                return false;
            }
            if (txtSSBreakStart.Text == "")
            {
                lblOTRequestMsg.Text = "Enter break start.";
                txtSSBreakStart.Focus();
                return false;
            }
        }
        if (txtSSBreakStart.Text != "" || txtSSBreakEnd.Text != "")
        {
            if (txtSSBreakCredit.Text == "")
            {
                lblShiftScheduleMsg.Text = "Enter the shift schedule break credit.";
                txtSSBreakCredit.Focus();
                return false;
            }
        }
        if (txtSSBreakCredit.Text != "")
        {
            if (txtSSBreakStart.Text == "")
            {
                lblOTRequestMsg.Text = "Enter break start.";
                txtSSBreakStart.Focus();
                return false;
            }
            if (txtSSBreakEnd.Text == "")
            {
                lblOTRequestMsg.Text = "Enter break start.";
                txtSSBreakEnd.Focus();
                return false;
            }

            try
            {
                decimal breakCredit = Convert.ToDecimal(txtSSBreakCredit.Text);
                txtSSBreakCredit.Text = breakCredit.ToString("00.00");
            }
            catch
            {
                lblShiftScheduleMsg.Text = "Invalid shift schedule break credit.";
                txtSSBreakCredit.Focus();
                return false;
            }
        }


        //validate per code
        ShiftSchedule _shift = new ShiftSchedule();
        _shift.ID = Convert.ToInt32(lblShiftScheduleID.Text);
        _shift.Code = txtSSCode.Text;

        DataTable dtShift = new DataTable();
        dtShift = _shift.CheckShiftScheduleIfExists();

        if (dtShift.Rows.Count > 0)
        {
            lblShiftScheduleMsg.Text = "Code already exists.";
            txtSSCode.Focus();
            return false;
        }

        return true;
    }
    protected void btnShiftScheduleAdd_Click(object sender, EventArgs e)
    {
        if (ValidateShiftSchedule())
        {
            int _isFlexible = 0;
            int _isForceCredit = 0;

            if (rbtnFlexibleYes.Checked == true)
                _isFlexible = 1;
            if (rbtnForceCreditYes.Checked == true)
                _isForceCredit = 1;

            ShiftSchedule _shift = new ShiftSchedule();
            _shift.ID = Convert.ToInt32(lblShiftScheduleID.Text);
            _shift.UserID = lblUserID.Text;

            _shift.Code = txtSSCode.Text;
            _shift.Description = txtSSDesc.Text;
            _shift.IsFlexible = _isFlexible;
            _shift.TimeInFrom = Convert.ToDateTime(txtSSTimeInFrom.Text);
            _shift.TimeOutFrom = Convert.ToDateTime(txtSSTimeOutFrom.Text);

            _shift.ShiftCredit = txtSSShiftCredit.Text;
            _shift.IsForceCredit = _isForceCredit;
            _shift.BreakCredit = txtSSBreakCredit.Text;

            if (rbtnFlexibleYes.Checked == true)
            {
                _shift.TimeinTo = Convert.ToDateTime(txtSSTimeInTo.Text);
                _shift.TimeOutTo = Convert.ToDateTime(txtSSTimeOutTo.Text);
            }

            if (txtSSBreakStart.Text != "")
            {
                _shift.BreakStart = Convert.ToDateTime(txtSSBreakStart.Text);
            }
            if (txtSSBreakEnd.Text != "")
            {
                _shift.BreakEnd = Convert.ToDateTime(txtSSBreakEnd.Text);
            }


            int rowAffected = _shift.AddUpdateShiftSchedule();

            if (rowAffected > 0)
            {
                lblShiftScheduleMsg.Visible = false;
                //BIND
                GetShiftSchedule();
                pnlShiftScheduleNew.Visible = false;
                pnlShiftScheduleList.Visible = true;
            }
            else
            {
                lblShiftScheduleMsg.Visible = true;
                lblShiftScheduleMsg.Text = "Failed to save shift schedule.";
                return;
            }
        }
    }
    protected void btnShiftScheduleBack_Click(object sender, EventArgs e)
    {
        pnlShiftScheduleList.Visible = true;
        pnlShiftScheduleNew.Visible = false;

        //BIND
        GetShiftSchedule();
    }

    protected void rbtnFlexibleYes_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnFlexibleYes.Checked == true)
        {
            pnlTimeIn.Visible = true;
            pnlTimeOut.Visible = true;
        }
        else
        {
            pnlTimeIn.Visible = false;
            pnlTimeOut.Visible = false;
        }
    }
    protected void rbtnFlexibleNo_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnFlexibleNo.Checked == true)
        {
            pnlTimeIn.Visible = false;
            pnlTimeOut.Visible = false;
        }
        else
        {
            pnlTimeIn.Visible = true;
            pnlTimeOut.Visible = true;
        }
    }
    protected void rptrShiftSchedules_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblIsFlexible = (Label)e.Item.FindControl("lblIsFlexible");
        Label lblIsForceCredit = (Label)e.Item.FindControl("lblIsForceCredit");

        if (lblIsFlexible.Text == "0")
            lblIsFlexible.Text = "No";
        else
            lblIsFlexible.Text = "Yes";

        if (lblIsForceCredit.Text == "0")
            lblIsForceCredit.Text = "No";
        else
            lblIsForceCredit.Text = "Yes";

    }
    protected void rptrShiftSchedules_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label lblID = (Label)e.Item.FindControl("lblID");
        Label lblCode = (Label)e.Item.FindControl("lblCode");
        Label lblDescription = (Label)e.Item.FindControl("lblDescription");
        Label lblIsFlexible = (Label)e.Item.FindControl("lblIsFlexible");
        Label lblTimeInFrom = (Label)e.Item.FindControl("lblTimeInFrom");
        Label lblTimeInTo = (Label)e.Item.FindControl("lblTimeInTo");
        Label lblTimeOutFrom = (Label)e.Item.FindControl("lblTimeOutFrom");
        Label lblTimeOutTo = (Label)e.Item.FindControl("lblTimeOutTo");
        Label lblShiftCredit = (Label)e.Item.FindControl("lblShiftCredit");
        Label lblBreakStart = (Label)e.Item.FindControl("lblBreakStart");
        Label lblBreakEnd = (Label)e.Item.FindControl("lblBreakEnd");
        Label lblBreakCredit = (Label)e.Item.FindControl("lblBreakCredit");
        Label lblIsForceCredit = (Label)e.Item.FindControl("lblIsForceCredit");


        if (e.CommandName == "Edit")
        {
            lblShiftScheduleMsg.Visible = false;
            lblShiftScheduleMsg.Text = "";

            pnlShiftScheduleNew.Visible = true;
            pnlShiftScheduleList.Visible = false;

            lblShiftScheduleID.Text = lblID.Text;
            txtSSCode.Text = lblCode.Text;
            txtSSDesc.Text = lblDescription.Text;
            txtSSTimeInFrom.Text = lblTimeInFrom.Text;
            txtSSTimeInTo.Text = lblTimeInTo.Text;
            txtSSTimeOutFrom.Text = lblTimeOutFrom.Text;
            txtSSTimeOutTo.Text = lblTimeOutTo.Text;
            txtSSShiftCredit.Text = lblShiftCredit.Text;
            txtSSBreakStart.Text = lblBreakStart.Text;
            txtSSBreakEnd.Text = lblBreakEnd.Text;
            txtSSBreakCredit.Text = lblBreakCredit.Text;

            if (lblIsFlexible.Text == "Yes")
            {
                rbtnFlexibleYes.Checked = true;
                rbtnFlexibleNo.Checked = false;
                pnlTimeIn.Visible = true;
                pnlTimeOut.Visible = true;
            }
            else
            {
                rbtnFlexibleYes.Checked = false;
                rbtnFlexibleNo.Checked = true;
                pnlTimeIn.Visible = false;
                pnlTimeOut.Visible = false;
            }

            if (lblIsForceCredit.Text == "Yes")
            {
                rbtnForceCreditYes.Checked = true;
                rbtnForceCreditNo.Checked = false;
            }
            else
            {
                rbtnForceCreditYes.Checked = false;
                rbtnForceCreditNo.Checked = true;
            }

        }
    }

    protected void lnkLogs_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlLogs.Visible = true;
        txtDateFrom.Text = DateTime.Now.ToShortDateString();
        txtDateTo.Text = DateTime.Now.ToShortDateString();


        if (lblUsertype.Text == "admin")
        {
            //get employee list
            GetEmployeeList();
        }
        else
        {
            GetEmployeeListPerApprover();
        }
        GetLogs();
        //
        if (lblUsertype.Text != "admin" && lblUsertype.Text != "supervisor")
        {
            ddlEmployee.SelectedValue = lblUserID.Text;
            ddlEmployee.Enabled = false;
        }
    }
    private void GetLogs()
    {
        DataTable dt = new DataTable();
        Employee _emp = new Employee();

        DateTime dtFrom = new DateTime();
        DateTime dtTo = new DateTime();
        dtFrom = DateTime.Now;
        dtTo = DateTime.Now;

        try
        {
            dtFrom = Convert.ToDateTime(txtDateFrom.Text);
        }
        catch
        {
            txtDateFrom.Text = DateTime.Now.ToShortDateString();
        }
        try
        {
            dtTo = Convert.ToDateTime(txtDateTo.Text);
        }
        catch
        {
            txtDateTo.Text = DateTime.Now.ToShortDateString();
        }


        _emp.EmployeeID = ddlEmployee.SelectedValue.ToString();

        dt = _emp.GetEmployeeLogs(dtFrom, dtTo);
        rptrLogs.DataSource = null;
        rptrLogs.DataSource = dt;
        rptrLogs.DataBind();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetLogs();
    }
    protected void lnkProfile_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlProfile.Visible = true;

        //
        Employee _emp = new Employee();
        _emp.EmployeeID = lblUserID.Text;

        DataTable dtEmployee = new DataTable();
        dtEmployee = _emp.GetEmployeeDetails();
        //GET DETAILS
        if (dtEmployee.Rows.Count > 0)
        {
            lblUserEmployeeID.Text = dtEmployee.Rows[0]["empID"].ToString();
            lblUserLastName.Text = dtEmployee.Rows[0]["LName"].ToString();
            lblUserFirstName.Text = dtEmployee.Rows[0]["FName"].ToString();
            lblUserMiddleName.Text = dtEmployee.Rows[0]["MName"].ToString();
            lblUserSuffix.Text = dtEmployee.Rows[0]["Suffix"].ToString();

        }


    }
    protected void lnkLAApproveCancel_Click(object sender, EventArgs e)
    {
        pnlLAApprove.Visible = false;
    }
    protected void lnkLAApproveYes_Click(object sender, EventArgs e)
    {
        if (txtLADenyReasons.Text != "")
        {
            LeaveManagement _leave = new LeaveManagement();
            _leave.ID = Convert.ToInt32(lblLAApproveID.Text);
            _leave.UserID = lblUserID.Text;
            int rowAffected = _leave.ApproveLeaveRequest(txtLADenyReasons.Text);
            if (rowAffected > 0)
            {
                pnlLAApprove.Visible = false;
                GetLeaveApplication();
                return;
            }
            else
            {
                lblLAApproveMsg.Text = "Failed to update request.";
                lblLAApproveMsg.Visible = true;
                return;
            }
        }
        else
        {
            lblLAApproveMsg.Text = "Reason is required.";
            lblLAApproveMsg.Visible = true;
            return;
        }
    }


    //EMPLOYEE SHIFT SCHEDULE (TAGGING)
    //SHIFT SCHEDULE
    private void GetShiftScheduleTagging()
    {
        try
        {

            DataTable dt = new DataTable();
            ShiftSchedule _shift = new ShiftSchedule();
            _shift.EmpID = ddlEmployeeShiftScheduleSearch.SelectedItem.Value;
            dt = _shift.GetAllShiftScheduleTagging();

            rptrShiftSchedulesTagging.DataSource = null;
            rptrShiftSchedulesTagging.DataSource = dt;
            rptrShiftSchedulesTagging.DataBind();
        }
        catch
        {

        }

    }
    protected void lnkAddShiftScheduleTagging_Click(object sender, EventArgs e)
    {
        pnlShiftScheduleTaggingList.Visible = false;
        pnlShiftScheduleTaggingNew.Visible = true;

        lblShiftScheduleTaggingMsg.Visible = false;
        lblShiftScheduleTaggingMsg.Text = "";
        lblShiftScheduleTaggingID.Text = "0";

        if (lblUsertype.Text == "admin")
        {
            //get employee list
            GetEmployeeList();
        }
        else
        {
            GetEmployeeListPerApprover();
        }
        //get shift code
        GetShiftScheduleList();
        //
        txtEffectivityDate.Text = DateTime.Now.ToShortDateString();
    }
    //SHIFT SCHEDULE
    private void GetShiftScheduleList()
    {
        DataTable dt = new DataTable();
        ShiftSchedule _shift = new ShiftSchedule();
        dt = _shift.GetAllShiftSchedule();

        //EMPLOYEE SHIFT SCHEDULE
        ddlEmployeeShiftScheduleCode.DataSource = null;
        ddlEmployeeShiftScheduleCode.DataSource = dt;
        ddlEmployeeShiftScheduleCode.DataValueField = "Id";
        ddlEmployeeShiftScheduleCode.DataTextField = "Code";
        ddlEmployeeShiftScheduleCode.DataBind();

        ddlEmployeeShiftScheduleCode.Items.Insert(0, new ListItem("Select Shift Code", String.Empty));
        ddlEmployeeShiftScheduleCode.SelectedIndex = 0;

        ddlShiftCode.DataSource = null;
        ddlShiftCode.DataSource = dt;
        ddlShiftCode.DataValueField = "Id";
        ddlShiftCode.DataTextField = "Code";
        ddlShiftCode.DataBind();

        ddlShiftCode.Items.Insert(0, new ListItem("Select Shift Code", String.Empty));
        ddlShiftCode.SelectedIndex = 0;

    }
    private bool ValidateEmployeeShiftSchedule()
    {
        lblShiftScheduleTaggingMsg.Visible = true;

        if (ddlEmployeeShiftSchedule.SelectedValue == "")
        {
            lblShiftScheduleTaggingMsg.Text = "Select employee";
            ddlEmployeeShiftSchedule.Focus();
            return false;
        }

        if (ddlEmployeeShiftScheduleCode.SelectedValue == "")
        {
            lblShiftScheduleTaggingMsg.Text = "Select shift code";
            ddlEmployeeShiftScheduleCode.Focus();
            return false;
        }
        if (txtEffectivityDate.Text == "")
        {
            lblShiftScheduleTaggingMsg.Text = "Enter effectivity date";
            txtEffectivityDate.Focus();
            return false;
        }
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txtEffectivityDate.Text);
        }
        catch
        {
            lblShiftScheduleTaggingMsg.Text = "Invalid effectivity date";
            txtEffectivityDate.Focus();
            return false;
        }

        //validate per code
        ShiftSchedule _shift = new ShiftSchedule();
        _shift.ID = Convert.ToInt32(lblShiftScheduleTaggingID.Text);
        _shift.EmpID = ddlEmployeeShiftSchedule.SelectedItem.Value;
        _shift.ShiftID = Convert.ToInt32(ddlEmployeeShiftScheduleCode.SelectedItem.Value);
        _shift.EffectivityDate = Convert.ToDateTime(txtEffectivityDate.Text);

        DataTable dtShift = new DataTable();
        dtShift = _shift.CheckShiftScheduleTaggingIfExists();

        if (dtShift.Rows.Count > 0)
        {
            lblShiftScheduleTaggingMsg.Text = "Employee Shift Schedule already exists.";
            ddlEmployeeShiftScheduleCode.Focus();
            return false;
        }
        lblShiftScheduleTaggingMsg.Visible = false;
        lblShiftScheduleTaggingMsg.Text = "";

        return true;
    }
    protected void btnShiftScheduleTaggingAdd_Click(object sender, EventArgs e)
    {
        if (ValidateEmployeeShiftSchedule())
        {
            ShiftSchedule _shift = new ShiftSchedule();
            _shift.ID = Convert.ToInt32(lblShiftScheduleTaggingID.Text);
            _shift.EmpID = ddlEmployeeShiftSchedule.SelectedItem.Value;
            _shift.ShiftID = Convert.ToInt32(ddlEmployeeShiftScheduleCode.SelectedItem.Value);
            _shift.EffectivityDate = Convert.ToDateTime(txtEffectivityDate.Text);
            _shift.UserID = lblUserID.Text;

            int rowAffected = _shift.AddUpdateShiftScheduleTagging();
            if (rowAffected > 0)
            {
                lblShiftScheduleTaggingMsg.Visible = false;
                //BIND
                GetShiftScheduleTagging();
                pnlShiftScheduleTaggingNew.Visible = false;
                pnlShiftScheduleTaggingList.Visible = true;
            }
            else
            {
                lblShiftScheduleTaggingMsg.Visible = true;
                lblShiftScheduleTaggingMsg.Text = "Failed to save employee shift schedule.";
                return;
            }

        }
    }
    protected void btnShiftScheduleTaggingBack_Click(object sender, EventArgs e)
    {
        pnlShiftScheduleTaggingList.Visible = true;
        pnlShiftScheduleTaggingNew.Visible = false;

        //BIND
        GetShiftScheduleTagging();
    }
    protected void ddlEmployeeShiftScheduleSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetShiftScheduleTagging();
        //GetCalendar(ddlEmployeeShiftScheduleSearch.SelectedItem.Value);
        GetCalendarShift(ddlEmployeeShiftScheduleSearch.SelectedItem.Value);

    }
    private void GetCalendar(string _empID)
    {
        //_DayEventsTable = new DataTable();
        //_DayEventsTable.Columns.Add("Date");
        //_DayEventsTable.Columns.Add("Title");

        //_DayEventsTable.Rows.Add(DateTime.Now.AddDays(-2).Date.ToString(), "Meeting with Boss");
        //_DayEventsTable.Rows.Add(DateTime.Now.Date.ToString(), "Lunch with Suzan");
        //_DayEventsTable.Rows.Add(DateTime.Now.AddDays(2).Date.ToString(), "Trip to Paris!");

        _shiftEMPID = _empID;

        clndrEmpShiftSchedule.Visible = true;
        clndrEmpShiftSchedule.DayRender += new DayRenderEventHandler(this.clndrEmpShiftSchedule_DayRender);
    }
    private void GetCalendarShift(string _empID)
    {
        _shiftEMPID = _empID;

        clndrShiftSchedule.Visible = true;
        clndrShiftSchedule.DayRender += new DayRenderEventHandler(this.clndrShiftSchedule_DayRender);
    }
    protected void rptrShiftSchedulesTagging_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label lblID = (Label)e.Item.FindControl("lblID");
        Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");
        Label lblShiftID = (Label)e.Item.FindControl("lblShiftID");
        Label lblEffectivityDate = (Label)e.Item.FindControl("lblEffectivityDate");

        if (e.CommandName == "Edit")
        {
            pnlShiftScheduleTaggingList.Visible = false;
            pnlShiftScheduleTaggingNew.Visible = true;

            lblShiftScheduleTaggingMsg.Visible = false;
            lblShiftScheduleTaggingMsg.Text = "";

            if (lblUsertype.Text == "admin")
            {
                //get employee list
                GetEmployeeList();
            }
            else
            {
                GetEmployeeListPerApprover();
            }
            //get shift code
            GetShiftScheduleList();
            //
            lblShiftScheduleTaggingID.Text = lblID.Text;
            ddlEmployeeShiftSchedule.SelectedValue = lblEmpID.Text;
            ddlEmployeeShiftScheduleCode.SelectedValue = lblShiftID.Text;
            txtEffectivityDate.Text = lblEffectivityDate.Text;
        }
    }

    //HOLIDAYS
    private void GetHolidaysList()
    {
        //rptrHolidays
        try
        {
            DataTable dt = new DataTable();
            Holidays _holidays = new Holidays();
            dt = _holidays.GetAllHolidays();

            rptrHolidays.DataSource = null;
            rptrHolidays.DataSource = dt;
            rptrHolidays.DataBind();
        }
        catch
        {

        }
    }
    protected void lnkAddHolidays_Click(object sender, EventArgs e)
    {
        pnlHolidaysList.Visible = false;
        pnlHolidaysNew.Visible = true;

        lblHolidaysMsg.Visible = false;
        lblHolidaysMsg.Text = "";
        lblHolidaysID.Text = "0";

        GetLocationsList();
        ClearHolidays();

    }
    private void ClearHolidays()
    {
        txtHolidaysDate.Text = "";
        txtHolidaysDescription.Text = "";
        rbtnHolidaysSpecial.Checked = true;
        rbtnHolidaysLegal.Checked = false;
        ddlHolidaysLocation.SelectedIndex = 0;
    }
    private void GetLocationsList()
    {
        //ddlHolidaysLocation
        DataTable dt = new DataTable();
        Holidays _shift = new Holidays();
        dt = _shift.GetAllLocations();

        //EMPLOYEE SHIFT SCHEDULE
        ddlHolidaysLocation.DataSource = null;
        ddlHolidaysLocation.DataSource = dt;
        ddlHolidaysLocation.DataValueField = "Id";
        ddlHolidaysLocation.DataTextField = "LocationName";
        ddlHolidaysLocation.DataBind();

        ddlHolidaysLocation.Items.Insert(0, new ListItem("Select Location", String.Empty));
        ddlHolidaysLocation.SelectedIndex = 0;
    }
    private bool ValidateHolidays()
    {
        lblHolidaysMsg.Visible = true;

        if (txtHolidaysDate.Text == "")
        {
            lblHolidaysMsg.Text = "Enter the holiday date";
            txtHolidaysDate.Focus();
            return false;
        }
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txtHolidaysDate.Text);
        }
        catch
        {
            lblHolidaysMsg.Text = "Invalid holiday date";
            txtHolidaysDate.Focus();
            return false;
        }

        if (txtHolidaysDescription.Text == "")
        {
            lblHolidaysMsg.Text = "Enter the holiday description";
            txtHolidaysDescription.Focus();
            return false;
        }

        if (ddlHolidaysLocation.SelectedValue == "")
        {
            lblHolidaysMsg.Text = "Select location";
            ddlHolidaysLocation.Focus();
            return false;
        }
        //CheckHolidaysIfExists
        //validate
        string _holidayType = "Special";
        if (rbtnHolidaysLegal.Checked == true)
        {
            _holidayType = "Legal";
        }
        Holidays _holidays = new Holidays();
        _holidays.ID = Convert.ToInt32(lblHolidaysID.Text);
        _holidays.Date = Convert.ToDateTime(txtHolidaysDate.Text);
        _holidays.Description = txtHolidaysDescription.Text;
        _holidays.HolidayType = _holidayType;
        _holidays.LocationID = Convert.ToInt32(ddlHolidaysLocation.SelectedItem.Value);


        DataTable dtHolidays = new DataTable();
        dtHolidays = _holidays.CheckHolidaysIfExists();

        if (dtHolidays.Rows.Count > 0)
        {
            lblHolidaysMsg.Text = "Holiday already exists.";
            txtHolidaysDescription.Focus();
            return false;
        }
        lblHolidaysMsg.Visible = false;
        lblHolidaysMsg.Text = "";
        return true;
    }
    protected void btnHolidaysAdd_Click(object sender, EventArgs e)
    {
        if (ValidateHolidays())
        {
            string _holidayType = "Special";
            if (rbtnHolidaysLegal.Checked == true)
            {
                _holidayType = "Legal";
            }

            Holidays _holidays = new Holidays();
            _holidays.ID = Convert.ToInt32(lblHolidaysID.Text);
            _holidays.Date = Convert.ToDateTime(txtHolidaysDate.Text);
            _holidays.Description = txtHolidaysDescription.Text;
            _holidays.HolidayType = _holidayType;
            _holidays.LocationID = Convert.ToInt32(ddlHolidaysLocation.SelectedItem.Value);
            _holidays.UserID = lblUserID.Text;

            int rowAffected = _holidays.AddUpdateHolidays();
            if (rowAffected > 0)
            {
                lblHolidaysMsg.Visible = false;
                //BIND
                GetHolidaysList();
                pnlHolidaysNew.Visible = false;
                pnlHolidaysList.Visible = true;
            }
            else
            {
                lblHolidaysMsg.Visible = true;
                lblHolidaysMsg.Text = "Failed to save holiday.";
                return;
            }
        }
    }
    protected void btnHolidaysBack_Click(object sender, EventArgs e)
    {
        pnlHolidaysList.Visible = true;
        pnlHolidaysNew.Visible = false;

        //BIND
        GetHolidaysList();
    }
    protected void rptrHolidays_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label lblID = (Label)e.Item.FindControl("lblID");
        Label lblHDate = (Label)e.Item.FindControl("lblHDate");
        Label lblHDesc = (Label)e.Item.FindControl("lblHDesc");
        Label lblHType = (Label)e.Item.FindControl("lblHType");
        Label lblHLocID = (Label)e.Item.FindControl("lblHLocID");

        if (e.CommandName == "Edit")
        {
            pnlHolidaysList.Visible = false;
            pnlHolidaysNew.Visible = true;

            lblHolidaysMsg.Visible = false;
            lblHolidaysMsg.Text = "";

            //get locations list
            GetLocationsList();
            //
            lblHolidaysID.Text = lblID.Text;
            txtHolidaysDate.Text = lblHDate.Text;
            txtHolidaysDescription.Text = lblHDesc.Text;
            ddlHolidaysLocation.SelectedValue = lblHLocID.Text;

            if (lblHType.Text == "Special")
            {
                rbtnHolidaysLegal.Checked = false;
                rbtnHolidaysSpecial.Checked = true;
            }
            else
            {
                rbtnHolidaysLegal.Checked = true;
                rbtnHolidaysSpecial.Checked = false;
            }
        }
    }
    protected void lnkDTRSummary_Click(object sender, EventArgs e)
    {

    }


    //PAYROLL PERIOD
    private void GetPayrollPeriod()
    {
        DataTable dt = new DataTable();
        Payroll _payroll = new Payroll();
        dt = _payroll.GetAllPayrollPeriod();

        rptrPayrollPeriod.DataSource = null;
        rptrPayrollPeriod.DataSource = dt;
        rptrPayrollPeriod.DataBind();
    }
    private void GetInitialPayrollPeriod()
    {
        lblPPID.Text = "0";
        lblPPMsg.Visible = false;
        lblPPMsg.Text = "";

        ddlPPMonth.SelectedValue = DateTime.Now.ToString("MMMM");

        int _year = DateTime.Now.Year;
        ddlPPYear.Items.Clear();

        int currentYear = DateTime.Now.Year;//2015
        int year = DateTime.Now.Year - 1;//2014

        for (int i = year; i >= year && i <= currentYear + 1; i++)
        {
            ddlPPYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
        ddlPPYear.Items.Insert(0, new ListItem("Select Year", String.Empty));
        ddlPPYear.SelectedValue = DateTime.Now.Year.ToString();

        txtPPDesc.Text = "";
        txtPPStart.Text = "";
        txtPPEnd.Text = "";

        rbtnFirstHalf.Checked = true;
        rbtnSecondHalf.Checked = false;

        try
        {
            DateTime dt = new DateTime();
            string _date = ddlPPMonth.SelectedValue.ToString() + "/01/" + ddlPPYear.SelectedValue.ToString();//October/01/2015
            dt = Convert.ToDateTime(_date);

            txtPPStart.Text = _date;
            txtPPEnd.Text = dt.AddDays(14).ToString("MMMM/dd/yyyy");
        }
        catch
        {

        }

    }
    protected void lnkPayrollPeriodCancel_Click(object sender, EventArgs e)
    {
        pnlSelectPayrollPeriod.Visible = false;
    }
    protected void lnkSelectPayrollPeriod_Click(object sender, EventArgs e)
    {
        pnlSelectPayrollPeriod.Visible = true;

        DataTable dt = new DataTable();
        Payroll _payroll = new Payroll();
        //_payroll.Year = DateTime.Now.Year;
        //_payroll.Month = DateTime.Now.ToString("MMMM");

        //dt = _payroll.GetAllPayrollPeriodPerMonth();

        dt = _payroll.GetAllPayrollPeriodActive();

        rptrSelectPayrollPeriod.DataSource = null;
        rptrSelectPayrollPeriod.DataSource = dt;
        rptrSelectPayrollPeriod.DataBind();

    }


    private bool ValidatePayrollPeriod()
    {
        lblPPMsg.Text = "";
        lblPPMsg.Visible = true;

        if (txtPPDesc.Text == "")
        {
            lblPPMsg.Text = "Enter payroll period description";
            txtPPDesc.Focus();
            return false;
        }

        if (ddlPPYear.SelectedValue.ToString() == "")
        {
            lblPPMsg.Text = "Select year";
            ddlPPYear.Focus();
            return false;
        }
        if (ddlPPMonth.SelectedValue.ToString() == "")
        {
            lblPPMsg.Text = "Select month";
            ddlPPMonth.Focus();
            return false;
        }
        //check if exists
        int _payrollPeriod = 0;
        if (rbtnFirstHalf.Checked == true)
            _payrollPeriod = 1;
        else
            _payrollPeriod = 2;

        Payroll _payroll = new Payroll();
        _payroll.ID = Convert.ToInt32(lblPPID.Text);
        _payroll.Year = Convert.ToInt32(ddlPPYear.SelectedValue.ToString());
        _payroll.Month = ddlPPMonth.SelectedValue.ToString();
        _payroll.PayrollPeriod = _payrollPeriod;
        lblPPMsg.Text = "";

        DataTable dt = new DataTable();
        dt = _payroll.CheckPayrollPeriodIfExists();
        if (dt.Rows.Count > 0)
        {
            lblPPMsg.Text = "Payroll period already exists";
            ddlPPMonth.Focus();
            return false;
        }



        lblPPMsg.Visible = false;
        return true;
    }
    protected void btnPayrollPeriodSave_Click(object sender, EventArgs e)
    {
        if (ValidatePayrollPeriod())
        {
            int _payrollPeriod = 0;
            if (rbtnFirstHalf.Checked == true)
                _payrollPeriod = 1;
            else
                _payrollPeriod = 2;

            Payroll _payroll = new Payroll();
            _payroll.ID = Convert.ToInt32(lblPPID.Text);
            _payroll.Year = Convert.ToInt32(ddlPPYear.SelectedValue.ToString());
            _payroll.Month = ddlPPMonth.SelectedValue.ToString();
            _payroll.Description = txtPPDesc.Text;
            _payroll.PayrollPeriod = _payrollPeriod;
            _payroll.PayrollStart = Convert.ToDateTime(txtPPStart.Text);
            _payroll.PayrollEnd = Convert.ToDateTime(txtPPEnd.Text);
            _payroll.UserID = lblUserID.Text;

            if (chkActive.Checked == true)
                _payroll.IsActive = 1;
            else
                _payroll.IsActive = 0;

            int rowAffected = _payroll.AddUpdatePayrollPeriod();
            if (rowAffected > 0)
            {
                lblPPMsg.Visible = false;
                //BIND
                GetPayrollPeriod();
                pnlPayrollPeriodNew.Visible = false;
                pnlPayrollPeriodList.Visible = true;
            }
            else
            {
                lblPPMsg.Visible = true;
                lblPPMsg.Text = "Failed to save payroll period.";
                return;
            }
        }
    }
    protected void btnPayrollPeriodBack_Click(object sender, EventArgs e)
    {
        GetInitialPayrollPeriod();
        pnlPayrollPeriodList.Visible = true;
        pnlPayrollPeriodNew.Visible = false;
    }
    protected void lnkAddPayrollPeriod_Click(object sender, EventArgs e)
    {
        pnlPayrollPeriodList.Visible = false;
        pnlPayrollPeriodNew.Visible = true;

        GetInitialPayrollPeriod();
    }
    protected void rbtnFirstHalf_CheckedChanged(object sender, EventArgs e)
    {
        SetPPeriod();
    }
    protected void rbtnSecondHalf_CheckedChanged(object sender, EventArgs e)
    {
        SetPPeriod();
    }

    private void SetPPeriod()
    {
        if (rbtnFirstHalf.Checked == true)
        {
            rbtnSecondHalf.Checked = false;

            try
            {
                DateTime dt = new DateTime();
                string _date = ddlPPMonth.SelectedValue.ToString() + "/01/" + ddlPPYear.SelectedValue.ToString();//October/01/2015
                dt = Convert.ToDateTime(_date);

                txtPPStart.Text = _date;
                txtPPEnd.Text = dt.AddDays(14).ToString("MMMM/dd/yyyy");
            }
            catch
            {
                txtPPStart.Text = "";
                txtPPEnd.Text = "";
            }
        }
        if (rbtnSecondHalf.Checked == true)
        {
            rbtnFirstHalf.Checked = false;

            try
            {
                DateTime dt = new DateTime();
                string _date = ddlPPMonth.SelectedValue.ToString() + "/16/" + ddlPPYear.SelectedValue.ToString();//October/01/2015
                dt = Convert.ToDateTime(_date);

                //start of month
                DateTime dtStart = new DateTime();
                dtStart = Convert.ToDateTime(ddlPPMonth.SelectedValue.ToString() + "/01/" + ddlPPYear.SelectedValue.ToString());//October/01/2015);

                txtPPStart.Text = _date;
                txtPPEnd.Text = dtStart.AddMonths(1).AddDays(-1).ToString("MMMM/dd/yyyy");
            }
            catch
            {
                txtPPStart.Text = "";
                txtPPEnd.Text = "";
            }
        }
    }
    protected void ddlPPYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetPPeriod();
    }
    protected void ddlPPMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetPPeriod();
    }
    protected void rptrPayrollPeriod_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            Label lblID = (Label)e.Item.FindControl("lblID");
            Label lblPPDesc = (Label)e.Item.FindControl("lblPPDesc");
            Label lblPPYear = (Label)e.Item.FindControl("lblPPYear");
            Label lblPPMonth = (Label)e.Item.FindControl("lblPPMonth");
            Label lblPPPeriodID = (Label)e.Item.FindControl("lblPPPeriodID");
            Label lblPPPeriod = (Label)e.Item.FindControl("lblPPPeriod");
            Label lblPPStart = (Label)e.Item.FindControl("lblPPStart");
            Label lblPPEnd = (Label)e.Item.FindControl("lblPPEnd");

            Label lblIsActive = (Label)e.Item.FindControl("lblIsActive");
            //CheckBox chkIsActive = (CheckBox)e.Item.FindControl("chkIsActive");

            pnlPayrollPeriodList.Visible = false;
            pnlPayrollPeriodNew.Visible = true;

            GetInitialPayrollPeriod();

            lblPPID.Text = lblID.Text;
            txtPPDesc.Text = lblPPDesc.Text;

            ddlPPYear.SelectedValue = lblPPYear.Text;
            ddlPPMonth.SelectedValue = lblPPMonth.Text;

            if (lblPPPeriodID.Text == "1")
            {
                rbtnFirstHalf.Checked = true;
                rbtnSecondHalf.Checked = false;
            }
            else
            {
                rbtnFirstHalf.Checked = false;
                rbtnSecondHalf.Checked = true;
            }

            txtPPStart.Text = lblPPStart.Text;
            txtPPEnd.Text = lblPPEnd.Text;

            if (lblIsActive.Text == "1" || lblIsActive.Text.ToLower() == "true")
                chkActive.Checked = true;
            else
                chkActive.Checked = false;
        }
    }
    protected void rptrPayrollPeriod_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblPPPeriodID = (Label)e.Item.FindControl("lblPPPeriodID");
        Label lblPPPeriod = (Label)e.Item.FindControl("lblPPPeriod");
        Label lblIsActive = (Label)e.Item.FindControl("lblIsActive");
        CheckBox chkIsActive = (CheckBox)e.Item.FindControl("chkIsActive");


        if (lblIsActive.Text == "1" || lblIsActive.Text.ToLower() == "true")
            chkIsActive.Checked = true;
        else
            chkIsActive.Checked = false;

        if (lblPPPeriodID.Text == "1")
        {
            lblPPPeriod.Text = "First Half";
        }
        else
        {
            lblPPPeriod.Text = "Second Half";
        }

    }

    protected void chk_CheckedChanged(object sender, EventArgs e)
    {
        if (sender != null)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                RepeaterItem ri = (RepeaterItem)chk.NamingContainer;

                Label lblID = (Label)chk.Parent.FindControl("lblID");//PAYROLL ID
                Label lblPPStart = (Label)chk.Parent.FindControl("lblPPStart");//year
                Label lblPPEnd = (Label)chk.Parent.FindControl("lblPPEnd");//PAYROLL ID


                //
                pnlSelectPayrollPeriod.Visible = false;
                
                //lnkSelectPayrollPeriod.Visible = false;
                //lblPayrollPeriod.Visible = true;
                //lblPayrollPeriodID.Text = lblID.Text;
                //lblPayrollPeriod.Text = "Payroll cut off : " + lblPPStart.Text + "-" + lblPPEnd.Text;
                //lblPayrollPeriodStart.Text = lblPPStart.Text;
                //lblPayrollPeriodEnd.Text = lblPPEnd.Text;
                //ddlDTREmployee.Visible = true;
                //btnSaveDTR.Visible = false;
                //btnProcessDTR1.Visible = true;

                //new dtr
                lnkSelectPayrollPeriodDTR.Visible = false;
                pnlSelectPayrollPeriod.Visible = false;

                lblPayrollPeriodDTR.Visible = true;
                lblPayrollPeriodIDDTR.Text = lblID.Text;
                lblPayrollPeriodDTR.Text = "Payroll cut off : " + lblPPStart.Text + "-" + lblPPEnd.Text;
                lblPayrollPeriodStartDTR.Text = lblPPStart.Text;
                lblPayrollPeriodEndDTR.Text = lblPPEnd.Text;

                ddlDTREmployeeDTR.Visible = true;
                btnSaveDTRDTR.Visible = false;
                btnProcessDTR1DTR.Visible = true;


            }
            catch { }
        }
    }
    protected void ddlDTREmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        //rptrDTR.DataSource = null;

        //DataTable dt = new DataTable();

        //dt.Columns.Add("id", Type.GetType("System.String"));
        //dt.Columns.Add("empID", Type.GetType("System.String"));
        //dt.Columns.Add("Date", Type.GetType("System.String"));
        //dt.Columns.Add("shift", Type.GetType("System.String"));
        //dt.Columns.Add("in", Type.GetType("System.String"));
        //dt.Columns.Add("out", Type.GetType("System.String"));
        //dt.Columns.Add("Leave", Type.GetType("System.String"));
        //dt.Columns.Add("RegHour", Type.GetType("System.String"));
        //dt.Columns.Add("Late", Type.GetType("System.String"));
        //dt.Columns.Add("UT", Type.GetType("System.String"));
        //dt.Columns.Add("OT", Type.GetType("System.String"));
        //dt.Columns.Add("POT", Type.GetType("System.String"));

        //dt.Columns.Add("TimeInFrom", Type.GetType("System.String"));
        //dt.Columns.Add("TimeInTo", Type.GetType("System.String"));
        //dt.Columns.Add("TimeOutFrom", Type.GetType("System.String"));
        //dt.Columns.Add("TimeOutTo", Type.GetType("System.String"));
        //dt.Columns.Add("IsFlexible", Type.GetType("System.String"));
        //dt.Columns.Add("ShiftCredit", Type.GetType("System.String"));
        //dt.Columns.Add("ShiftID", Type.GetType("System.String"));

        //dt.Columns.Add("BreakCredit", Type.GetType("System.String"));



        //string _start = lblPayrollPeriodStart.Text;
        //string _end = lblPayrollPeriodEnd.Text;
        //try
        //{
        //    DateTime dtStart = new DateTime();
        //    DateTime dtEnd = new DateTime();

        //    dtStart = Convert.ToDateTime(_start);
        //    dtEnd = Convert.ToDateTime(_end);

        //    int _count = 1;
        //    for (DateTime date = dtStart; date <= dtEnd; date = date.AddDays(1))
        //    {
        //        DataRow dr = null;
        //        dr = dt.NewRow();

        //        dr["id"] = _count;
        //        dr["empid"] = ddlDTREmployee.SelectedValue.ToString();
        //        dr["Date"] = date.ToString("ddd MMM dd, yyyy");
        //        dr["shift"] = "";
        //        dr["in"] = "";
        //        dr["out"] = "";
        //        dr["Leave"] = "0.00";
        //        dr["RegHour"] = "0000";
        //        dr["Late"] = "0000";
        //        dr["UT"] = "0000";
        //        dr["OT"] = "0000";
        //        dr["POT"] = "0000";

        //        dr["TimeInFrom"] = "";
        //        dr["TimeInTo"] = "";
        //        dr["TimeOutFrom"] = "";
        //        dr["TimeOutTo"] = "";
        //        dr["IsFlexible"] = "";
        //        dr["ShiftCredit"] = "";
        //        dr["ShiftID"] = "";
        //        dr["BreakCredit"] = "0000";



        //        dt.Rows.Add(dr);
        //        _count++;

        //    }

        //}
        //catch
        //{

        //}

        //if (ddlDTREmployee.SelectedValue != "")
        //{
        //    btnSaveDTR.Visible = true;

        //    rptrDTR.DataSource = null;
        //    rptrDTR.DataSource = dt;
        //    rptrDTR.DataBind();
        //}
        //else
        //{
        //    btnSaveDTR.Visible = false;

        //    rptrDTR.DataSource = null;
        //    rptrDTR.DataBind();
        //}

    }
    protected void rptrDTR_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");
        Label lblShift = (Label)e.Item.FindControl("lblShift");
        Label lblShiftID = (Label)e.Item.FindControl("lblShiftID");


        Label lblLeave = (Label)e.Item.FindControl("lblLeave");
        Label lblRegHour = (Label)e.Item.FindControl("lblRegHour");
        Label lblLate = (Label)e.Item.FindControl("lblLate");
        Label lblUT = (Label)e.Item.FindControl("lblUT");
        Label lblOT = (Label)e.Item.FindControl("lblOT");
        Label lblPOT = (Label)e.Item.FindControl("lblPOT");

        Label lblTimeInFrom = (Label)e.Item.FindControl("lblTimeInFrom");
        Label lblTimeInTo = (Label)e.Item.FindControl("lblTimeInTo");
        Label lblTimeOutFrom = (Label)e.Item.FindControl("lblTimeOutFrom");
        Label lblTimeOutTo = (Label)e.Item.FindControl("lblTimeOutTo");

        Label lblIsFlexible = (Label)e.Item.FindControl("lblIsFlexible");
        Label lblCreditHours = (Label)e.Item.FindControl("lblCreditHours");
        Label lblBreakCredit = (Label)e.Item.FindControl("lblBreakCredit");


        Label lblDate = (Label)e.Item.FindControl("lblDate");
        Label lblDateIn = (Label)e.Item.FindControl("lblDateIn");
        Label lblDateOut = (Label)e.Item.FindControl("lblDateOut");


        Label lblLeaveDetails = (Label)e.Item.FindControl("lblLeaveDetails");
        Label lblOTDetails = (Label)e.Item.FindControl("lblOTDetails");


        TextBox txtOut = (TextBox)e.Item.FindControl("txtOut");
        TextBox txtIn = (TextBox)e.Item.FindControl("txtIn");

        //get shift
        ShiftSchedule _sched = new ShiftSchedule();
        _sched.EmpID = lblEmpID.Text;
        _sched.EffectivityDate = Convert.ToDateTime(lblDate.Text);

        DataTable dtShift = new DataTable();
        dtShift = _sched.GetShiftPerEmployeePerDate();
        if (dtShift.Rows.Count > 0)
        {
            lblShiftID.Text = dtShift.Rows[0]["ShiftID"].ToString();
            lblShift.Text = dtShift.Rows[0]["Code"].ToString();

            lblTimeInFrom.Text = dtShift.Rows[0]["TimeInFrom"].ToString();
            lblTimeInTo.Text = dtShift.Rows[0]["TimeInTo"].ToString();
            lblTimeOutFrom.Text = dtShift.Rows[0]["TimeOutFrom"].ToString();
            lblTimeOutTo.Text = dtShift.Rows[0]["TimeOutTo"].ToString();
            lblIsFlexible.Text = dtShift.Rows[0]["IsFlexible"].ToString();
            lblCreditHours.Text = dtShift.Rows[0]["ShiftCredit"].ToString();
            lblBreakCredit.Text = dtShift.Rows[0]["BreakCredit"].ToString();


            //RD and NL
            string _shiftCode = dtShift.Rows[0]["Code"].ToString();

            if (_shiftCode == "RD" || _shiftCode == "NL")
            {
                lblRegHour.Text = lblCreditHours.Text;
            }

        }
        else
        {
            txtOut.Enabled = false;
            txtIn.Enabled = false;
        }


        //TIME IN AND OUT
        DataTable dtDTR = new DataTable();
        dtDTR = _sched.GetDTRPerEmployeePerDate();

        if (dtDTR.Rows.Count > 0)
        {
            try
            {
                txtIn.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateIn"].ToString()).ToString("HHmm");
                lblDateIn.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateIn"].ToString()).ToString();
            }
            catch
            {
                txtIn.Text = "";
                lblDateIn.Text = "";
            }
            try
            {
                txtOut.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateOut"].ToString()).ToString("HHmm");
                lblDateOut.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateOut"].ToString()).ToString();

            }
            catch
            {
                txtOut.Text = "";
                lblDateOut.Text = "";
            }


            lblLeave.Text = dtDTR.Rows[0]["leave"].ToString();
            lblRegHour.Text = dtDTR.Rows[0]["regHour"].ToString();
            lblLate.Text = dtDTR.Rows[0]["late"].ToString();
            lblUT.Text = dtDTR.Rows[0]["UT"].ToString();
            lblOT.Text = dtDTR.Rows[0]["OT"].ToString();
            lblPOT.Text = dtDTR.Rows[0]["POT"].ToString();

        }
        //FILED AND APPROVED LEAVE
        lblLeaveDetails.Text = "";
        DataTable dtLeave = new DataTable();
        dtLeave = _sched.GetLeavePerEmployeePerDate();
        if (dtLeave.Rows.Count > 0)
        {
            string _status = "";
            if (dtLeave.Rows[0]["leaveStatus"].ToString() == "")
            {
                _status = "Filed: " + Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
            }
            else
            {
                _status = dtLeave.Rows[0]["leaveStatus"].ToString() + ": " + Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
                lblLeave.Text = Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00");
            }
            if (!_status.Contains("Approved"))
            {
                lblLeave.Text = "0000";
            }
            lblLeaveDetails.Text = _status;
        }

        //FILED AND APPROVED OT
        lblOTDetails.Text = "";
        DataTable dtOT = new DataTable();
        dtOT = _sched.GetOTPerEmployeePerDate();
        if (dtOT.Rows.Count > 0)
        {
            string _otHours = "0000";
            DateTime dtStart = Convert.ToDateTime(dtOT.Rows[0]["timeStart"].ToString());
            DateTime dtEnd = Convert.ToDateTime(dtOT.Rows[0]["timeEnd"].ToString());
            try
            {

                TimeSpan ot = dtEnd.Subtract(dtStart);

                int _hoursOT = 0;
                int _minutesOT = 0;

                _hoursOT = ot.Hours;
                _minutesOT = ot.Minutes;

                if (((_hoursOT * 60) + _minutesOT) < 0)//-45
                    _otHours = "0000";

                else
                    _otHours = Math.Abs(_hoursOT).ToString("00") + Math.Abs(_minutesOT).ToString("00");
            }
            catch
            {

            }
            string _status = "";

            if (dtOT.Rows[0]["OTStatus"].ToString() == "")
            {
                _status = "Filed: " + _otHours + "hr/s";// +Convert.ToDecimal(dtOT.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
            }
            else
            {
                _status = dtOT.Rows[0]["OTStatus"].ToString() + ": " + _otHours + "hr/s";
                lblOT.Text = _otHours;// Convert.ToDecimal(dtOT.Rows[0]["NoOfHours"].ToString()).ToString("0.00");
            }
            if (!_status.Contains("Approved"))
            {
                lblOT.Text = "0000";
            }
            lblOTDetails.Text = _status;
        }


    }
    private DateTime GetDateIN(string _in, string timestringIn, string _date)
    {
        DateTime dtIn = new DateTime();
        DateTime dtInTest = new DateTime();

        //IN
        if (_in.Contains(':'))
        {
            //08:00
            if (DateTime.TryParseExact(timestringIn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtInTest))
            {
                dtIn = dtInTest;
            }
            else if (DateTime.TryParseExact(timestringIn, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtInTest))
            {
                dtIn = dtInTest;
            }

        }
        else
        {
            //0800
            try
            {
                DateTime dt = DateTime.ParseExact(_in, "HHmm", CultureInfo.InvariantCulture);
                timestringIn = dt.ToString("HH:mm:ss");

                if (DateTime.TryParseExact(_date + " " + timestringIn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtInTest))
                {
                    dtIn = dtInTest;
                }
            }
            catch
            {
                DateTime dt = DateTime.ParseExact(_in, "HHmmss", CultureInfo.InvariantCulture);
                timestringIn = dt.ToString("HH:mm:ss");

                if (DateTime.TryParseExact(_date + " " + timestringIn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtInTest))
                {
                    dtIn = dtInTest;
                }
            }

        }

        return dtIn;
    }
    private DateTime GetDateOUT(string _out, string timestringOut, string _date, string _creditHours, string _timeInFrom, string _shift, string _timeIn)
    {
        DateTime dtOut = new DateTime();
        DateTime dtInTest = new DateTime();

        DateTime dtTimeInRaw = new DateTime();

        try
        {
            dtTimeInRaw = Convert.ToDateTime(_timeIn);
        }
        catch { }

        //OUT
        if (_out.Contains(':'))
        {
            //08:00
            if (DateTime.TryParseExact(timestringOut, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtInTest))
            {
                dtOut = dtInTest;
            }
            else if (DateTime.TryParseExact(timestringOut, "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtInTest))
            {
                dtOut = dtInTest;
            }

        }
        else
        {
            //0800
            try
            {
                DateTime dt = DateTime.ParseExact(_out, "HHmm", CultureInfo.InvariantCulture);
                timestringOut = dt.ToString("HH:mm:ss");

                if (DateTime.TryParseExact(_date + " " + timestringOut, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtInTest))
                {
                    dtOut = dtInTest;
                }
            }
            catch
            {
                DateTime dt = DateTime.ParseExact(_out, "HHmmss", CultureInfo.InvariantCulture);
                timestringOut = dt.ToString("HH:mm:ss");

                if (DateTime.TryParseExact(_date + " " + timestringOut, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtInTest))
                {
                    dtOut = dtInTest;
                }
            }


            //check if next day
            //check shift credit from time in from
            string _timeCheck = "";
            DateTime dtCredithoursCheck = new DateTime();
            //get minutes
            if (_creditHours.Contains(':'))//08:00
            {
                _timeCheck = _creditHours;//08:00
                dtCredithoursCheck = Convert.ToDateTime(_date + " " + _timeCheck);
            }
            else if (!_creditHours.Contains('.'))
            {
                //0800
                try
                {
                    DateTime dt = DateTime.ParseExact(_creditHours, "HHmm", CultureInfo.InvariantCulture);
                    _timeCheck = dt.ToString("HH:mm");

                    dtCredithoursCheck = Convert.ToDateTime(_date + " " + _timeCheck);
                }
                catch
                {
                    DateTime dt = DateTime.ParseExact(_creditHours, "HHmm:ss", CultureInfo.InvariantCulture);
                    _timeCheck = dt.ToString("HH:mm:ss");

                    dtCredithoursCheck = Convert.ToDateTime(_date + " " + _timeCheck);
                }

            }
            else
            {
                try
                {
                    //8.00
                    string _hour = "00";
                    string _min = "00";

                    string[] _credit = _creditHours.Split('.');
                    _hour = _credit[0].ToString();
                    _min = _credit[1].ToString();

                    _hour = Convert.ToInt32(_hour).ToString("00");
                    _min = Convert.ToInt32(_min).ToString("00");

                    DateTime dt = DateTime.ParseExact(_hour + _min, "HHmm", CultureInfo.InvariantCulture);
                    _timeCheck = dt.ToString("HH:mm");

                    dtCredithoursCheck = Convert.ToDateTime(_date + " " + _timeCheck);


                }
                catch
                {

                }


            }

            int _shiftCreditInMin = (dtCredithoursCheck.Hour * 60) + dtCredithoursCheck.Minute;//480 minutes

            DateTime dtOutCheck = new DateTime();
            DateTime dtTimeInFrom = new DateTime();
            dtTimeInFrom = Convert.ToDateTime(Convert.ToDateTime(_date).ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(_timeInFrom).ToString("HH:mm:ss"));

            dtOutCheck = dtTimeInFrom.AddMinutes(_shiftCreditInMin);


            //time in  = 8:pm out = 2:30am
            if (Convert.ToDateTime(dtOutCheck.ToString("MM/dd/yyyy")) > Convert.ToDateTime(dtTimeInFrom.ToString("MM/dd/yyyy")))
            {
                dtOut = dtOut.AddDays(1);
            }

            //additional
            else if (dtTimeInFrom > dtOut)
            {
                dtOut = dtOut.AddDays(1);
            }

            //check per shift
            if (_shift == "RD" || _shift == "NL")
            {
                if (dtTimeInRaw > dtOut)
                    dtOut = dtOut.AddDays(1);
            }
        }
        return dtOut;
    }
    private string GetRegHours(DateTime dtIn, DateTime dtOut, string _timeInFrom, string _shiftCode)
    {
        string _regHours = "";
        //GET REG HOURS
        TimeSpan diff = new TimeSpan();
        if (_shiftCode == "RD" || _shiftCode == "NL")
        {
            diff = dtOut.Subtract(dtIn);
        }
        else
        {
            //
            DateTime dtTimeIn = new DateTime();
            dtTimeIn = Convert.ToDateTime(dtIn.ToShortDateString() + " " + Convert.ToDateTime(_timeInFrom).ToString("HH:mm:ss"));

            diff = dtOut.Subtract(dtTimeIn);
        }

        int _hours = 0;
        int _minutes = 0;

        _hours = diff.Hours;
        _minutes = diff.Minutes;

        if (Math.Abs((_hours * 60) + _minutes) < 0)//-45
            _regHours = "-" + Math.Abs(_hours).ToString("00") + Math.Abs(_minutes).ToString("00");
        else
            _regHours = _hours.ToString("00") + _minutes.ToString("00");

        return _regHours;
    }
    private string GetLates(DateTime dtIn, string _timeInFrom, string _shiftCode)
    {
        string _late = string.Empty;

        if (_shiftCode == "RD" || _shiftCode == "NL")
        {
            return _late = "0000";
        }

        //GET LATES 
        DateTime dtLate = new DateTime();
        dtLate = Convert.ToDateTime(dtIn.ToShortDateString() + " " + Convert.ToDateTime(_timeInFrom).ToString("HH:mm:ss"));

        TimeSpan late = dtIn.Subtract(dtLate);

        int _hoursLate = 0;
        int _minutesLate = 0;

        _hoursLate = late.Hours;
        _minutesLate = late.Minutes;

        if (((_hoursLate * 60) + _minutesLate) < 0)//-45
            _late = "0000";

        else
            _late = Math.Abs(_hoursLate).ToString("00") + Math.Abs(_minutesLate).ToString("00");


        return _late;
    }
    private string GetUndertime(int _hours, int _minutes, string _creditHours, string _date, string _shiftCode)
    {
        string _undertime = "";
        if (_shiftCode == "RD" || _shiftCode == "NL")
        {
            return _undertime = "0000";
        }

        //Under time
        //get per minute time in and out
        int _workHours = (_hours * 60) + _minutes;//minutes: 8 hours = 480 minutes
        int _shiftHours = 0;

        //get shift credit in minutes// 0800 = 8 hours
        DateTime dtCredithours = new DateTime();//SHIFT CREDIT  - fix from shift code
        string _time = "";
        if (_creditHours.Contains(':'))//08:00
        {
            _time = _creditHours;//08:00
            dtCredithours = Convert.ToDateTime(_date + " " + _time);
        }
        else if (!_creditHours.Contains('.'))
        {
            //0800
            try
            {
                DateTime dt = DateTime.ParseExact(_creditHours, "HHmm", CultureInfo.InvariantCulture);
                _time = dt.ToString("HH:mm");

                dtCredithours = Convert.ToDateTime(_date + " " + _time);
            }
            catch
            {
                DateTime dt = DateTime.ParseExact(_creditHours, "HHmm:ss", CultureInfo.InvariantCulture);
                _time = dt.ToString("HH:mm:ss");

                dtCredithours = Convert.ToDateTime(_date + " " + _time);
            }


        }
        else
        {
            try
            {
                //8.00
                string _hour = "00";
                string _min = "00";

                string[] _credit = _creditHours.Split('.');
                _hour = _credit[0].ToString();
                _min = _credit[1].ToString();

                _hour = Convert.ToInt32(_hour).ToString("00");
                _min = Convert.ToInt32(_min).ToString("00");

                DateTime dt = DateTime.ParseExact(_hour + _min, "HHmm", CultureInfo.InvariantCulture);
                _time = dt.ToString("HH:mm");

                dtCredithours = Convert.ToDateTime(_date + " " + _time);
            }
            catch
            {

            }


        }

        _shiftHours = (dtCredithours.Hour * 60) + dtCredithours.Minute;


        int _UT = _shiftHours - _workHours;

        if (_UT > 0)
        {
            TimeSpan _tsUT = TimeSpan.FromMinutes(_UT);
            int _hoursUT = 0;
            int _minutesUT = 0;

            _hoursUT = _tsUT.Hours;
            _minutesUT = _tsUT.Minutes;


            if (Math.Abs((_hoursUT * 60) + _minutesUT) < 0)//-45
                _undertime = "-" + Math.Abs(_hoursUT).ToString("00") + Math.Abs(_minutesUT).ToString("00");
            else
                _undertime = _hoursUT.ToString("00") + _minutesUT.ToString("00");

        }
        else
        {
            _undertime = "0000";
        }

        return _undertime;
    }
    private string GetPOT(int _hours, int _minutes, string _creditHours, string _date, string _shiftCode, string _ot)
    {
        string _pot = "";

        int _workHours = (_hours * 60) + _minutes;//minutes: 8 hours = 480 minutes
        if (_shiftCode == "RD" || _shiftCode == "NL")
        {
            return _pot = _hours.ToString("00") + _minutes.ToString("00");
        }

        int _shiftHours = 0;
        //get shift credit in minutes// 0800 = 8 hours
        DateTime dtCredithours = new DateTime();//SHIFT CREDIT  - fix from shift code
        string _time = "";
        if (_creditHours.Contains(':'))//08:00
        {
            _time = _creditHours;//08:00
            dtCredithours = Convert.ToDateTime(_date + " " + _time);
        }
        else if (!_creditHours.Contains('.'))
        {
            //0800
            try
            {
                DateTime dt = DateTime.ParseExact(_creditHours, "HHmm", CultureInfo.InvariantCulture);
                _time = dt.ToString("HH:mm");

                dtCredithours = Convert.ToDateTime(_date + " " + _time);
            }
            catch
            {
                DateTime dt = DateTime.ParseExact(_creditHours, "HHmm:ss", CultureInfo.InvariantCulture);
                _time = dt.ToString("HH:mm:ss");

                dtCredithours = Convert.ToDateTime(_date + " " + _time);
            }


        }
        else
        {
            try
            {
                //8.00
                string _hour = "00";
                string _min = "00";

                string[] _credit = _creditHours.Split('.');
                _hour = _credit[0].ToString();
                _min = _credit[1].ToString();

                _hour = Convert.ToInt32(_hour).ToString("00");
                _min = Convert.ToInt32(_min).ToString("00");

                DateTime dt = DateTime.ParseExact(_hour + _min, "HHmm", CultureInfo.InvariantCulture);
                _time = dt.ToString("HH:mm");

                dtCredithours = Convert.ToDateTime(_date + " " + _time);
            }
            catch
            {

            }

        }

        _shiftHours = (dtCredithours.Hour * 60) + dtCredithours.Minute;


        //POT
        if (_workHours > _shiftHours)//540; 480
        {
            //POT
            int _POT = _workHours - _shiftHours;
            TimeSpan _tsPOT = TimeSpan.FromMinutes(_POT);
            int _hoursPOT = 0;
            int _minutesPOT = 0;

            _hoursPOT = _tsPOT.Hours;
            _minutesPOT = _tsPOT.Minutes;

            int _otHours = 0;
            int _hoursOT = 0;
            int _minutesOT = 0;
            try
            {
                DateTime _dtOT = DateTime.ParseExact(_ot, "HHmm", CultureInfo.InvariantCulture);//0000
                _otHours = (_dtOT.Hour * 60) + _dtOT.Minute;

                TimeSpan _tsOT = TimeSpan.FromMinutes(_otHours);
                _hoursOT = _tsOT.Hours;
                _minutesOT = _tsOT.Minutes;

            }
            catch { }

            int _abs = Math.Abs((_hoursPOT * 60) + _minutesPOT);
            _abs = _abs - ((_hoursOT * 60) + _minutesOT);

            if (_abs < 0)
                _abs = Math.Abs((_hoursPOT * 60) + _minutesPOT);

            TimeSpan span = TimeSpan.FromMinutes(_abs);
            _hoursPOT = span.Hours;
            _minutesPOT = span.Minutes;

            if (_abs < 0)//-45
                _pot = "-" + Math.Abs(_hoursPOT).ToString("00") + Math.Abs(_minutesPOT).ToString("00");
            else
                _pot = _hoursPOT.ToString("00") + _minutesPOT.ToString("00");

        }
        else
        {
            _pot = "0000";
        }


        return _pot;
    }

    protected void txtIn_TextChanged(object sender, EventArgs e)
    {
        if (sender != null)
        {
            string _in = string.Empty;
            _in = ((TextBox)sender).Text;

            TextBox txt = (TextBox)sender;
            //txt.Focus();

            TextBox txtOut = (TextBox)txt.Parent.FindControl("txtOut");

            Label lblDate = (Label)txt.Parent.FindControl("lblDate");
            Label lblRegHour = (Label)txt.Parent.FindControl("lblRegHour");


            Label lblLate = (Label)txt.Parent.FindControl("lblLate");
            Label lblUT = (Label)txt.Parent.FindControl("lblUT");
            Label lblOT = (Label)txt.Parent.FindControl("lblOT");
            Label lblPOT = (Label)txt.Parent.FindControl("lblPOT");

            Label lblTimeInFrom = (Label)txt.Parent.FindControl("lblTimeInFrom");
            Label lblTimeInTo = (Label)txt.Parent.FindControl("lblTimeInTo");//FLEXIBLE
            Label lblTimeOutFrom = (Label)txt.Parent.FindControl("lblTimeOutFrom");
            Label lblTimeOutTo = (Label)txt.Parent.FindControl("lblTimeOutTo");//FLEXIBLE

            Label lblIsFlexible = (Label)txt.Parent.FindControl("lblIsFlexible");
            Label lblCreditHours = (Label)txt.Parent.FindControl("lblCreditHours");
            Label lblBreakCredit = (Label)txt.Parent.FindControl("lblBreakCredit");

            Label lblDateInOrig = (Label)txt.Parent.FindControl("lblDateInOrig");
            Label lblDateIn = (Label)txt.Parent.FindControl("lblDateIn");
            Label lblDateOut = (Label)txt.Parent.FindControl("lblDateOut");
            Label lblShift = (Label)txt.Parent.FindControl("lblShift");

            string _regHour = "0000";

            DateTime dtIn = new DateTime();
            DateTime dtOut = new DateTime();

            string _out = txtOut.Text;

            try
            {
                if (txtOut.Text != "" && _in != "")
                {
                    string _date = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                    string timestringIn = _date + " " + _in;
                    string timestringOut = _date + " " + _out;

                    //GET IN
                    dtIn = GetDateIN(_in, timestringIn, _date);
                    //GET OUT
                    dtOut = GetDateOUT(_out, timestringOut, _date, lblCreditHours.Text, lblTimeInFrom.Text, lblShift.Text, dtIn.ToString());

                    //CHECK IF in is within shiftin
                    #region checkshift
                    DateTime _shiftIn = new DateTime();
                    try
                    {
                        string[] _shift = lblShift.Text.Split('-');
                        string _dateShift = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                        string timeShiftIn = _dateShift + " " + _shift[0];

                        DateTime dt = DateTime.ParseExact(_shift[0], "HHmm", CultureInfo.InvariantCulture);
                        timeShiftIn = dt.ToString("HH:mm:ss");

                        lblDateInOrig.Text = dtIn.ToString();

                        if (DateTime.TryParseExact(_dateShift + " " + timeShiftIn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _shiftIn))
                        {
                            //0706 <= 0800
                            if (dtIn <= _shiftIn)
                                dtIn = _shiftIn;
                        }


                    }
                    catch
                    {
                    }
                    #endregion

                    //GET REG HOURS
                    TimeSpan diff = dtOut.Subtract(dtIn);

                    int _hours = 0; int _minutes = 0;

                    _hours = diff.Hours;
                    _minutes = diff.Minutes;

                    int _breakHour = 0;
                    int _breakMin = 0;

                    try
                    {
                        if (lblBreakCredit.Text != "")
                        {
                            string[] _break = lblBreakCredit.Text.Split('.');
                            _breakHour = Convert.ToInt32(_break[0].ToString());
                            _breakMin = Convert.ToInt32(_break[1].ToString());

                        }
                    }
                    catch { }

                    int _abs = Math.Abs((_hours * 60) + _minutes);
                    _abs = _abs - ((_breakHour * 60) + _breakMin);

                    TimeSpan span = TimeSpan.FromMinutes(_abs);
                    _hours = span.Hours;
                    _minutes = span.Minutes;

                    if (_abs < 0)//-45
                        _regHour = "-" + Math.Abs(_hours).ToString("00") + Math.Abs(_minutes).ToString("00");//negative
                    else
                        _regHour = _hours.ToString("00") + _minutes.ToString("00");

                    ////GET REG HOURS
                    //_regHour = GetRegHours(dtIn, dtOut, Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);

                    //GET LATES AND POSSIBLE OT
                    lblLate.Text = GetLates(Convert.ToDateTime(dtIn.ToString()), Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);
                    //Under time
                    lblUT.Text = GetUndertime(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text);
                    lblPOT.Text = GetPOT(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text, lblOT.Text);

                    //REG HOUR
                    lblRegHour.Text = _regHour;
                    lblDateIn.Text = dtIn.ToString();
                    lblDateOut.Text = dtOut.ToString();

                }
                else
                {
                    lblRegHour.Text = "0000";
                    lblLate.Text = "0000";
                    lblUT.Text = "0000";
                    lblPOT.Text = "0000";
                    lblDateIn.Text = "";
                    lblDateOut.Text = "";
                }

            }
            catch
            {
                lblRegHour.Text = _regHour;
                lblLate.Text = "0000";
                lblUT.Text = "0000";
                lblPOT.Text = "0000";
            }
        }
    }
    protected void txtOut_TextChanged(object sender, EventArgs e)
    {
        if (sender != null)
        {
            string _out = string.Empty;
            _out = ((TextBox)sender).Text;

            TextBox txt = (TextBox)sender;
            //txt.Focus();

            TextBox txtIn = (TextBox)txt.Parent.FindControl("txtIn");

            Label lblDate = (Label)txt.Parent.FindControl("lblDate");
            Label lblRegHour = (Label)txt.Parent.FindControl("lblRegHour");


            Label lblLate = (Label)txt.Parent.FindControl("lblLate");
            Label lblUT = (Label)txt.Parent.FindControl("lblUT");
            Label lblOT = (Label)txt.Parent.FindControl("lblOT");
            Label lblPOT = (Label)txt.Parent.FindControl("lblPOT");

            Label lblTimeInFrom = (Label)txt.Parent.FindControl("lblTimeInFrom");
            Label lblTimeInTo = (Label)txt.Parent.FindControl("lblTimeInTo");//FLEXIBLE
            Label lblTimeOutFrom = (Label)txt.Parent.FindControl("lblTimeOutFrom");
            Label lblTimeOutTo = (Label)txt.Parent.FindControl("lblTimeOutTo");//FLEXIBLE

            Label lblIsFlexible = (Label)txt.Parent.FindControl("lblIsFlexible");
            Label lblCreditHours = (Label)txt.Parent.FindControl("lblCreditHours");
            Label lblBreakCredit = (Label)txt.Parent.FindControl("lblBreakCredit");

            Label lblDateInOrig = (Label)txt.Parent.FindControl("lblDateInOrig");
            Label lblDateIn = (Label)txt.Parent.FindControl("lblDateIn");
            Label lblDateOut = (Label)txt.Parent.FindControl("lblDateOut");
            Label lblShift = (Label)txt.Parent.FindControl("lblShift");

            string _regHour = "0000";

            DateTime dtIn = new DateTime();
            DateTime dtOut = new DateTime();

            string _in = txtIn.Text;

            try
            {
                if (txtIn.Text != "" && _out != "")
                {
                    string _date = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                    string timestringIn = _date + " " + _in;
                    string timestringOut = _date + " " + _out;

                    //GET IN
                    dtIn = GetDateIN(_in, timestringIn, _date);
                    //GET OUT
                    dtOut = GetDateOUT(_out, timestringOut, _date, lblCreditHours.Text, lblTimeInFrom.Text, lblShift.Text, dtIn.ToString());

                    //CHECK IF in is within shiftin
                    #region checkshift
                    DateTime _shiftIn = new DateTime();
                    try
                    {
                        string[] _shift = lblShift.Text.Split('-');
                        string _dateShift = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                        string timeShiftIn = _dateShift + " " + _shift[0];

                        DateTime dt = DateTime.ParseExact(_shift[0], "HHmm", CultureInfo.InvariantCulture);
                        timeShiftIn = dt.ToString("HH:mm:ss");

                        lblDateInOrig.Text = dtIn.ToString();

                        if (DateTime.TryParseExact(_dateShift + " " + timeShiftIn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _shiftIn))
                        {
                            //0706 <= 0800
                            if (dtIn <= _shiftIn)
                                dtIn = _shiftIn;
                        }


                    }
                    catch
                    {
                    }
                    #endregion


                    //GET REG HOURS
                    TimeSpan diff = dtOut.Subtract(dtIn);

                    int _hours = 0; int _minutes = 0;

                    _hours = diff.Hours;
                    _minutes = diff.Minutes;

                    int _breakHour = 0;
                    int _breakMin = 0;

                    try
                    {
                        if (lblBreakCredit.Text != "")
                        {
                            string[] _break = lblBreakCredit.Text.Split('.');
                            _breakHour = Convert.ToInt32(_break[0].ToString());
                            _breakMin = Convert.ToInt32(_break[1].ToString());

                        }
                    }
                    catch { }

                    int _abs = Math.Abs((_hours * 60) + _minutes);
                    _abs = _abs - ((_breakHour * 60) + _breakMin);

                    TimeSpan span = TimeSpan.FromMinutes(_abs);
                    _hours = span.Hours;
                    _minutes = span.Minutes;


                    if (_abs < 0)//-45
                        _regHour = "-" + Math.Abs(_hours).ToString("00") + Math.Abs(_minutes).ToString("00");
                    else
                        _regHour = _hours.ToString("00") + _minutes.ToString("00");

                    ////GET REG HOURS
                    //_regHour = GetRegHours(dtIn, dtOut, Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);

                    //GET LATES AND POSSIBLE OT
                    lblLate.Text = GetLates(Convert.ToDateTime(dtIn.ToString()), Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);
                    //Under time
                    lblUT.Text = GetUndertime(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text);
                    lblPOT.Text = GetPOT(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text, lblOT.Text);

                    //REG HOUR
                    lblRegHour.Text = _regHour;
                    lblDateIn.Text = dtIn.ToString();
                    lblDateOut.Text = dtOut.ToString();

                }
                else
                {
                    lblRegHour.Text = "0000";
                    lblLate.Text = "0000";
                    lblUT.Text = "0000";
                    lblPOT.Text = "0000";

                    lblDateIn.Text = "";
                    lblDateOut.Text = "";
                }

            }
            catch
            {
                lblRegHour.Text = _regHour;
                lblLate.Text = "0000";
                lblUT.Text = "0000";
                lblPOT.Text = "0000";
            }
        }
    }

    protected void btnSaveDTR_Click(object sender, EventArgs e)
    {
        //if (rptrDTR.Items.Count > 0)
        //{
        //    foreach (RepeaterItem item in rptrDTR.Items)
        //    {
        //        Label lblID = (Label)item.FindControl("lblID");
        //        Label lblEmpID = (Label)item.FindControl("lblEmpID");

        //        Label lblDate = (Label)item.FindControl("lblDate");
        //        Label lblShiftID = (Label)item.FindControl("lblShiftID");
        //        TextBox txtIn = (TextBox)item.FindControl("txtIn");
        //        TextBox txtOut = (TextBox)item.FindControl("txtOut");
        //        Label lblDateIn = (Label)item.FindControl("lblDateIn");
        //        Label lblDateOut = (Label)item.FindControl("lblDateOut");

        //        Label lblLeave = (Label)item.FindControl("lblLeave");
        //        Label lblRegHour = (Label)item.FindControl("lblRegHour");
        //        Label lblLate = (Label)item.FindControl("lblLate");
        //        Label lblUT = (Label)item.FindControl("lblUT");
        //        Label lblOT = (Label)item.FindControl("lblOT");
        //        Label lblPOT = (Label)item.FindControl("lblPOT");



        //        DTR _dtr = new DTR();
        //        _dtr.ID = Convert.ToInt32(lblID.Text);
        //        _dtr.EmpID = lblEmpID.Text;
        //        _dtr.Date = Convert.ToDateTime(lblDate.Text);
        //        _dtr.ShiftID = Convert.ToInt32(lblShiftID.Text);

        //        if (txtIn.Text != "")
        //        {
        //            try
        //            {
        //                _dtr.DateIn = Convert.ToDateTime(lblDateIn.Text);
        //            }
        //            catch
        //            {
        //                _dtr.DateIn = null;
        //            }
        //        }
        //        else
        //            _dtr.DateIn = null;
        //        if (txtOut.Text != "")
        //        {
        //            try
        //            {
        //                _dtr.DateOut = Convert.ToDateTime(lblDateOut.Text);
        //            }
        //            catch
        //            {
        //                _dtr.DateOut = null;
        //            }
        //        }
        //        else
        //            _dtr.DateOut = null;

        //        _dtr.UserID = lblUserID.Text;

        //        _dtr.Leave = lblLeave.Text;
        //        _dtr.RegHour = lblRegHour.Text;
        //        _dtr.Late = lblLate.Text;
        //        _dtr.UT = lblUT.Text;
        //        _dtr.OT = lblOT.Text;
        //        _dtr.POT = lblPOT.Text;


        //        int rowAffected = _dtr.AddUpdateDTR();
        //        if (rowAffected > 0)
        //        {
        //            //success
        //        }
        //        else
        //        {

        //        }

        //    }
        //}
    }
    //APPROVER
    protected void btnCancelApprover_Click(object sender, EventArgs e)
    {
        pnlApprover.Visible = false;
        //CLEAR CONTROLS
        //GET APPROVER LIST
    }

    protected void btnSaveApprover_Click(object sender, EventArgs e)
    {
        lblApproverMsg.Visible = false;
        lblApproverMsg.Text = "";

        if (ddlApprover.SelectedValue.ToString() == "")
        {
            lblApproverMsg.Text = "Approver Name is required.";
            lblApproverMsg.Visible = true;
            ddlApprover.Focus();
            return;
        }
        //CHECK IF EXISTS
        DataTable dt = new DataTable();
        Employee _emp = new Employee();
        _emp.ID = Convert.ToInt32(lblApproverID.Text);
        _emp.GroupID = Convert.ToInt32(lblApproverGroupID.Text);
        _emp.EmployeeID = ddlApprover.SelectedValue.ToString();
        _emp.UserId = lblUserID.Text;

        dt = _emp.CheckIfApproverGroupExists();

        if (dt.Rows.Count > 0)
        {
            lblApproverMsg.Text = "Group approver already exists.";
            lblApproverMsg.Visible = true;
            ddlApprover.Focus();
            return;
        }

        //SAVE
        int rowaffected = _emp.SaveApproverGroup();
        if (rowaffected > 0)
        {
            pnlApprover.Visible = false;
        }
        else
        {
            lblApproverMsg.Text = "Failed to save group approver.";
            lblApproverMsg.Visible = true;
            return;
        }

    }
    protected void rptrApprover_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            Label lblID = (Label)e.Item.FindControl("lblID");
            Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");
            Label lblGroupID = (Label)e.Item.FindControl("lblGroupID");

            Employee _emp = new Employee();
            _emp.ID = Convert.ToInt32(lblID.Text);
            _emp.GroupID = Convert.ToInt32(lblGroupID.Text);


            int rowAffected = _emp.DeleteApproverGroup();
            if (rowAffected > 0)
            {
                DataTable dt = new DataTable();
                dt = _emp.GetApproverGroupPerGroupID();

                rptrApprover.DataSource = null;
                rptrApprover.DataSource = dt;
                rptrApprover.DataBind();
            }

        }
    }
    protected void lnkCloseApproverList_Click(object sender, EventArgs e)
    {
        pnlApproverList.Visible = false;
    }
    protected void clndrEmpShiftSchedule_DayRender(object sender, DayRenderEventArgs e)
    {
        e.Day.IsSelectable = false;

        //if (_first == "")
        //    _first = e.Day.Date.ToString();if
        if (_shiftEMPID == "")
        {
            clndrEmpShiftSchedule.Visible = false;
            return;
        }
        //if (e.Day.IsOtherMonth)
        //{
        //    e.Cell.Controls.Clear();
        //    return;
        //}

        try
        {
            //get shift
            ShiftSchedule _sched = new ShiftSchedule();
            _sched.EmpID = _shiftEMPID;
            _sched.EffectivityDate = Convert.ToDateTime(e.Day.Date.ToString());

            DataTable dtShift = new DataTable();
            dtShift = _sched.GetShiftPerEmployeePerDate();
            if (dtShift.Rows.Count > 0)
            {
                string Code = dtShift.Rows[0]["Code"].ToString();
                if (e.Cell.Controls.Count < 2)
                    e.Cell.Controls.Add(new LiteralControl("<p>" + Code + "</p>"));
            }
        }
        catch
        {

        }

        //_DayEventsTable = new DataTable();
        //_DayEventsTable.Columns.Add("Date");
        //_DayEventsTable.Columns.Add("Title");

        //_DayEventsTable.Rows.Add(DateTime.Now.AddDays(-2).Date.ToString(), "Meeting with Boss");
        //_DayEventsTable.Rows.Add(DateTime.Now.Date.ToString(), "Lunch with Suzan");
        //_DayEventsTable.Rows.Add(DateTime.Now.AddDays(2).Date.ToString(), "Trip to Paris!");

        //foreach (DataRow Row in _DayEventsTable.Rows)
        //{
        //    string Date = Row["Date"].ToString();
        //    string Title = Row["Title"].ToString();

        //    if (Date == e.Day.Date.ToString())
        //    {
        //        if (e.Cell.Controls.Count < 2)
        //            e.Cell.Controls.Add(new LiteralControl("<p>" + Title + "</p>"));
        //    }
        //}
    }
    protected void clndrEmpShiftSchedule_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        if (e.NewDate.Month > e.PreviousDate.Month)
        {
            GetCalendar(ddlEmployeeShiftScheduleSearch.SelectedItem.Value);
        }
        else
        {
            GetCalendar(ddlEmployeeShiftScheduleSearch.SelectedItem.Value);
        }
    }

    protected void btnBackUpload_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Dashboard.aspx");
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
                string _newFileName = "dtr_" + _dtNow.Year.ToString() + _dtNow.Month.ToString() + _dtNow.Day.ToString()
                    + _dtNow.Hour.ToString() + _dtNow.Minute.ToString() + _dtNow.Second.ToString() + _dtNow.Millisecond.ToString() + "." + _extension;

                string _filePath = Server.MapPath("DTR//" + _newFileName);

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
                _empUpload.Type = "dtr";
                _empUpload.UserId = Convert.ToInt32(Session["uid"].ToString());

                int rowAffected = 0;
                try { rowAffected = _empUpload.AddMasterFileDTR(); }
                catch { }

                foreach (DataRow _row in dtAssemble.Rows)
                {
                    if (_row[0].ToString().Trim().Length > 0)
                    {
                        DTR _dtr = new DTR();
                        _dtr.ID = rowAffected;
                        _dtr.EmpID = _row[0].ToString();

                        string _date = _row[1].ToString();
                        string _in = _row[2].ToString();
                        string _out = _row[3].ToString();


                        try
                        {
                            _date = Convert.ToDateTime(_date).ToString("MM-dd-yyyy");
                        }
                        catch { }
                        try
                        {
                            _in = Convert.ToDateTime(_in).ToString("HH:mm:ss");
                        }
                        catch { }
                        try
                        {
                            _out = Convert.ToDateTime(_out).ToString("HH:mm:ss");
                        }
                        catch { }

                        try
                        {
                            if (_row[2].ToString() != "")
                            {
                                try
                                {
                                    _dtr.DateIn = Convert.ToDateTime(_date + " " + _in);
                                }
                                catch
                                {
                                    _dtr.DateIn = null;
                                }
                            }
                            else
                                _dtr.DateIn = null;
                        }
                        catch
                        {

                        }
                        try
                        {
                            if (_row[3].ToString() != "")
                            {
                                try
                                {
                                    _dtr.DateOut = Convert.ToDateTime(_date + " " + _out);
                                }
                                catch
                                {
                                    _dtr.DateOut = null;
                                }
                            }
                            else
                                _dtr.DateOut = null;
                        }
                        catch
                        {

                        }

                        _dtr.Date = Convert.ToDateTime(_row[1].ToString());
                        _dtr.UserID = lblUserID.Text;

                        if (_dtr.UploadDTR() == 0)
                        {
                            lblUploadErr.Text = "Adding dtr failed.";
                            lblUploadErr.Visible = true;
                            break;
                        }
                    }
                }


                PaginateDTR();

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
    protected void rptrMaster_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)e.Item.FindControl("lnk");
            Label lblFileName = (Label)e.Item.FindControl("lblFileName");
            string path = MapPath("~/DTR/" + lblFileName.Text);
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
        dt.Columns.Add("DATE");
        dt.Columns.Add("IN");
        dt.Columns.Add("OUT");


        int _ctr = 0;
        foreach (DataRow _row in dtFile.Rows)
        {
            if (_ctr > 0)
            {
                DataRow _dr = dt.NewRow();
                _dr["EMPID"] = _row[0].ToString();
                _dr["DATE"] = _row[1].ToString();
                _dr["IN"] = _row[2].ToString();
                _dr["OUT"] = _row[3].ToString();


                dt.Rows.Add(_dr);
            }
            _ctr++;
        }

        return dt;
    }
    private DataTable DataAssembleShiftSched(DataTable dtFile)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("EMPID");
        dt.Columns.Add("DATE");
        dt.Columns.Add("SHIFTCODE");


        int _ctr = 0;
        foreach (DataRow _row in dtFile.Rows)
        {
            if (_ctr > 0)
            {
                DataRow _dr = dt.NewRow();
                _dr["EMPID"] = _row[0].ToString();
                _dr["DATE"] = _row[1].ToString();
                _dr["SHIFTCODE"] = _row[2].ToString();

                dt.Rows.Add(_dr);
            }
            _ctr++;
        }

        return dt;
    }
    #region UploadDTR
    protected void btnProcessDTR_Click(object sender, EventArgs e)
    {
        //GET ALL UNPROCESSED DTR, order by created date
        DataTable dtUnProcessed = new DataTable();
        DTR _dtr = new DTR();
        dtUnProcessed = _dtr.GetUnProcessedDTR();

        foreach (DataRow item in dtUnProcessed.Rows)
        {
            //GET details per uploadID
            string _uploadID = item["ID"].ToString();

            DataTable dtDetails = new DataTable();
            _dtr = new DTR();
            _dtr.ID = Convert.ToInt32(_uploadID);
            dtDetails = _dtr.GetUnProcessedDTRPerID();


            rptrDTRUpload.DataSource = null;

            DataTable dt = new DataTable();

            dt.Columns.Add("id", Type.GetType("System.String"));
            dt.Columns.Add("empID", Type.GetType("System.String"));
            dt.Columns.Add("Date", Type.GetType("System.String"));
            dt.Columns.Add("shift", Type.GetType("System.String"));
            dt.Columns.Add("in", Type.GetType("System.String"));
            dt.Columns.Add("out", Type.GetType("System.String"));
            dt.Columns.Add("Leave", Type.GetType("System.String"));
            dt.Columns.Add("RegHour", Type.GetType("System.String"));
            dt.Columns.Add("Late", Type.GetType("System.String"));
            dt.Columns.Add("UT", Type.GetType("System.String"));
            dt.Columns.Add("OT", Type.GetType("System.String"));
            dt.Columns.Add("POT", Type.GetType("System.String"));

            dt.Columns.Add("TimeInFrom", Type.GetType("System.String"));
            dt.Columns.Add("TimeInTo", Type.GetType("System.String"));
            dt.Columns.Add("TimeOutFrom", Type.GetType("System.String"));
            dt.Columns.Add("TimeOutTo", Type.GetType("System.String"));
            dt.Columns.Add("IsFlexible", Type.GetType("System.String"));
            dt.Columns.Add("ShiftCredit", Type.GetType("System.String"));
            dt.Columns.Add("ShiftID", Type.GetType("System.String"));

            dt.Columns.Add("BreakCredit", Type.GetType("System.String"));

            int _count = 0;
            foreach (DataRow det in dtDetails.Rows)
            {
                string _empid = det["EmpID"].ToString();
                string _date = det["date"].ToString();
                string _timeIn = det["TimeIn"].ToString();
                string _timeOut = det["TimeOut"].ToString();

                //UPDATE daily time record table

                try
                {
                    DataRow dr = null;

                    dr = dt.NewRow();

                    dr["id"] = _count;
                    dr["empid"] = _empid;
                    dr["Date"] = Convert.ToDateTime(_date).ToString("ddd MMM dd, yyyy");
                    dr["shift"] = "";
                    try
                    {
                        dr["in"] = Convert.ToDateTime(_timeIn).ToString("HHmm");
                    }
                    catch
                    {
                        dr["in"] = "";
                    }
                    try
                    {
                        dr["out"] = Convert.ToDateTime(_timeOut).ToString("HHmm");
                    }
                    catch
                    {
                        dr["out"] = _timeOut;
                    }
                    dr["Leave"] = "0.00";
                    dr["RegHour"] = "0000";
                    dr["Late"] = "0000";
                    dr["UT"] = "0000";
                    dr["OT"] = "0000";
                    dr["POT"] = "0000";

                    dr["TimeInFrom"] = "";
                    dr["TimeInTo"] = "";
                    dr["TimeOutFrom"] = "";
                    dr["TimeOutTo"] = "";
                    dr["IsFlexible"] = "";
                    dr["ShiftCredit"] = "";
                    dr["ShiftID"] = "";
                    dr["BreakCredit"] = "0000";


                    dt.Rows.Add(dr);
                    _count++;

                }
                catch
                {

                }
            }


            rptrDTRUpload.DataSource = null;
            rptrDTRUpload.DataSource = dt;
            rptrDTRUpload.DataBind();

            if (_count > 0)
            {
                //SAVE 
                try
                {
                    if (rptrDTRUpload.Items.Count > 0)
                    {
                        foreach (RepeaterItem items in rptrDTRUpload.Items)
                        {
                            Label lblID = (Label)items.FindControl("lblID");
                            Label lblEmpID = (Label)items.FindControl("lblEmpID");

                            Label lblDate = (Label)items.FindControl("lblDate");
                            Label lblShiftID = (Label)items.FindControl("lblShiftID");
                            TextBox txtIn = (TextBox)items.FindControl("txtIn");
                            TextBox txtOut = (TextBox)items.FindControl("txtOut");
                            Label lblDateInOrig = (Label)items.FindControl("lblDateInOrig");
                            Label lblDateIn = (Label)items.FindControl("lblDateIn");
                            Label lblDateOut = (Label)items.FindControl("lblDateOut");

                            Label lblLeave = (Label)items.FindControl("lblLeave");
                            Label lblRegHour = (Label)items.FindControl("lblRegHour");
                            Label lblLate = (Label)items.FindControl("lblLate");
                            Label lblUT = (Label)items.FindControl("lblUT");
                            Label lblOT = (Label)items.FindControl("lblOT");
                            Label lblPOT = (Label)items.FindControl("lblPOT");



                            _dtr = new DTR();
                            _dtr.ID = Convert.ToInt32(lblID.Text);
                            _dtr.EmpID = lblEmpID.Text;
                            _dtr.Date = Convert.ToDateTime(lblDate.Text);
                            _dtr.ShiftID = Convert.ToInt32(lblShiftID.Text);

                            if (txtIn.Text != "")
                            {
                                try
                                {
                                    _dtr.DateIn = Convert.ToDateTime(lblDateInOrig.Text);
                                }
                                catch
                                {
                                    _dtr.DateIn = null;
                                }
                            }
                            else
                                _dtr.DateIn = null;
                            if (txtOut.Text != "")
                            {
                                try
                                {
                                    _dtr.DateOut = Convert.ToDateTime(lblDateOut.Text);
                                }
                                catch
                                {
                                    _dtr.DateOut = null;
                                }
                            }
                            else
                                _dtr.DateOut = null;

                            _dtr.UserID = lblUserID.Text;

                            _dtr.Leave = lblLeave.Text;
                            _dtr.RegHour = lblRegHour.Text;
                            _dtr.Late = lblLate.Text;
                            _dtr.UT = lblUT.Text;
                            _dtr.OT = lblOT.Text;
                            _dtr.POT = lblPOT.Text;


                            int rowAffected = _dtr.AddUpdateDTR();
                            if (rowAffected > 0)
                            {
                                //success
                            }
                            else
                            {

                            }

                        }
                    }
                }
                catch
                {

                }
                //UPDATE status to process
                _dtr = new DTR();
                _dtr.ID = Convert.ToInt32(_uploadID);
                int rowAffected1 = 0;
                rowAffected1 = _dtr.UpdateDTRStatus();

                //refresh page
                ReferenceControls(GetPageableObject(), rptrMaster, txtSearch, lblShowing, drpEntries,
     lnkPage1, lnkPage2, lnkPage3, lnkPage4, lnkFirst, lnkPrevious, lnkNext, lnkLast);

            }

        }

    }
    protected void rptrMaster_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblStatusID = (Label)e.Item.FindControl("lblStatusID");
        Label lblStatus = (Label)e.Item.FindControl("lblStatus");


        if (lblStatusID.Text == "0")
        {
            lblStatus.Text = "";
        }
        else
        {
            lblStatus.Text = "Processed";
        }
    }
    protected void rptrDTRUpload_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //GET REPEATER ITEM
        #region prop
        Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");
        Label lblShift = (Label)e.Item.FindControl("lblShift");
        Label lblShiftID = (Label)e.Item.FindControl("lblShiftID");

        Label lblDate = (Label)e.Item.FindControl("lblDate");
        Label lblDateInOrig = (Label)e.Item.FindControl("lblDateInOrig");
        Label lblDateIn = (Label)e.Item.FindControl("lblDateIn");
        Label lblDateOut = (Label)e.Item.FindControl("lblDateOut");
        TextBox txtOut = (TextBox)e.Item.FindControl("txtOut");
        TextBox txtIn = (TextBox)e.Item.FindControl("txtIn");

        Label lblIsHoliday = (Label)e.Item.FindControl("lblIsHoliday");


        Label lblTimeInFrom = (Label)e.Item.FindControl("lblTimeInFrom");
        Label lblTimeInTo = (Label)e.Item.FindControl("lblTimeInTo");
        Label lblTimeOutFrom = (Label)e.Item.FindControl("lblTimeOutFrom");
        Label lblTimeOutTo = (Label)e.Item.FindControl("lblTimeOutTo");

        Label lblIsFlexible = (Label)e.Item.FindControl("lblIsFlexible");
        Label lblCreditHours = (Label)e.Item.FindControl("lblCreditHours");
        Label lblBreakCredit = (Label)e.Item.FindControl("lblBreakCredit");

        Label lblLeave = (Label)e.Item.FindControl("lblLeave");
        Label lblRegHour = (Label)e.Item.FindControl("lblRegHour");
        Label lblLate = (Label)e.Item.FindControl("lblLate");
        Label lblUT = (Label)e.Item.FindControl("lblUT");
        Label lblOT = (Label)e.Item.FindControl("lblOT");
        Label lblPOT = (Label)e.Item.FindControl("lblPOT");

        Label lblLeaveDetails = (Label)e.Item.FindControl("lblLeaveDetails");
        Label lblOTDetails = (Label)e.Item.FindControl("lblOTDetails");

        DTR _dtr = new DTR();
        _dtr.EmpID = lblEmpID.Text;
        _dtr.Date = Convert.ToDateTime(lblDate.Text);

        #endregion

        //GET SHIFT DETAILS
        #region GetSHIFT
        //get shift
        ShiftSchedule _sched = new ShiftSchedule();
        _sched.EmpID = lblEmpID.Text;
        _sched.EffectivityDate = Convert.ToDateTime(lblDate.Text);

        DataTable dtShift = new DataTable();
        dtShift = _sched.GetShiftPerEmployeePerDate();
        if (dtShift.Rows.Count > 0)
        {
            lblShiftID.Text = dtShift.Rows[0]["ShiftID"].ToString();
            lblShift.Text = dtShift.Rows[0]["Code"].ToString();

            lblTimeInFrom.Text = dtShift.Rows[0]["TimeInFrom"].ToString();
            lblTimeInTo.Text = dtShift.Rows[0]["TimeInTo"].ToString();
            lblTimeOutFrom.Text = dtShift.Rows[0]["TimeOutFrom"].ToString();
            lblTimeOutTo.Text = dtShift.Rows[0]["TimeOutTo"].ToString();
            lblIsFlexible.Text = dtShift.Rows[0]["IsFlexible"].ToString();
            lblCreditHours.Text = dtShift.Rows[0]["ShiftCredit"].ToString();
            lblBreakCredit.Text = dtShift.Rows[0]["BreakCredit"].ToString();


            //RD and NL
            string _shiftCode = dtShift.Rows[0]["Code"].ToString();

            if (_shiftCode == "RD" || _shiftCode == "NL")
            {
                lblRegHour.Text = lblCreditHours.Text;
            }

        }
        else
        {
            txtOut.Enabled = false;
            txtIn.Enabled = false;
        }

        #endregion

        //GET TIME IN , TIME OUT FROM DTR LOGS
        #region GetTimeINAndOUT

        //check if already in dailytimerecordlatest table
        //TIME IN AND OUT

        DataTable dtDTR = new DataTable();
        dtDTR = _sched.GetDTRPerEmployeePerDate();

        if (dtDTR.Rows.Count > 0 && (dtDTR.Rows[0]["dateIn"].ToString() != "" && dtDTR.Rows[0]["dateOut"].ToString() != ""))
        {


            try
            {
                txtIn.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateIn"].ToString()).ToString("HHmm");
                lblDateIn.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateIn"].ToString()).ToString();
                lblDateInOrig.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateIn"].ToString()).ToString();
            }
            catch
            {
                txtIn.Text = "";
                lblDateIn.Text = "";
                lblDateInOrig.Text = "";
            }
            try
            {
                txtOut.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateOut"].ToString()).ToString("HHmm");
                lblDateOut.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateOut"].ToString()).ToString();

            }
            catch
            {
                txtOut.Text = "";
                lblDateOut.Text = "";
            }


            lblLeave.Text = dtDTR.Rows[0]["leave"].ToString();
            lblRegHour.Text = dtDTR.Rows[0]["regHour"].ToString();
            lblLate.Text = dtDTR.Rows[0]["late"].ToString();
            lblUT.Text = dtDTR.Rows[0]["UT"].ToString();
            lblOT.Text = dtDTR.Rows[0]["OT"].ToString();
            lblPOT.Text = dtDTR.Rows[0]["POT"].ToString();

        }
        else
        {
            //else get logs from dtrLogs (sync)
            DataTable dtGetTimeInOut = new DataTable();
            dtGetTimeInOut = _dtr.GetTimeInOutFromLogsPerEmployeeDate();
            if (dtGetTimeInOut.Rows.Count > 0)
            {
                foreach (DataRow item in dtGetTimeInOut.Rows)
                {
                    string _logType = item["logType"].ToString();
                    if (_logType == "True")
                    {
                        //TIME IN
                        try
                        {
                            txtIn.Text = Convert.ToDateTime(item["scanDate"].ToString()).ToString("HHmm");
                            lblDateIn.Text = Convert.ToDateTime(item["scanDate"].ToString()).ToString();
                            lblDateInOrig.Text = Convert.ToDateTime(item["scanDate"].ToString()).ToString();
                        }
                        catch
                        {
                            txtIn.Text = "";
                            lblDateIn.Text = "";
                            lblDateInOrig.Text = "";
                        }
                    }
                    else
                    {
                        //TIME OUT
                        try
                        {
                            txtOut.Text = Convert.ToDateTime(item["scanDate"].ToString()).ToString("HHmm");
                            lblDateOut.Text = Convert.ToDateTime(item["scanDate"].ToString()).ToString();

                        }
                        catch
                        {
                            txtOut.Text = "";
                            lblDateOut.Text = "";
                        }
                    }
                }

            }
        }
        #endregion

        //SET LATES, LEAVES, OT, POT, UT VALUES
        #region setValues
        // if (sender != null)
        {
            //string _in = string.Empty;
            //_in = ((TextBox)sender).Text;

            //TextBox txt = (TextBox)sender;
            //txt.Focus();


            string _regHour = "0000";

            DateTime dtIn = new DateTime();
            DateTime dtOut = new DateTime();

            string _in = txtIn.Text;
            string _out = txtOut.Text;

            try
            {
                if (txtOut.Text != "" && _in != "")
                {
                    string _date = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                    string timestringIn = _date + " " + _in;
                    string timestringOut = _date + " " + _out;

                    //GET IN
                    dtIn = GetDateIN(_in, timestringIn, _date);
                    //GET OUT
                    dtOut = GetDateOUT(_out, timestringOut, _date, lblCreditHours.Text, lblTimeInFrom.Text, lblShift.Text, dtIn.ToString());

                    //CHECK IF in is within shiftin
                    #region checkshift
                    DateTime _shiftIn = new DateTime();
                    try
                    {
                        string[] _shift = lblShift.Text.Split('-');
                        string _dateShift = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                        string timeShiftIn = _dateShift + " " + _shift[0];

                        DateTime dt = DateTime.ParseExact(_shift[0], "HHmm", CultureInfo.InvariantCulture);
                        timeShiftIn = dt.ToString("HH:mm:ss");

                        if (DateTime.TryParseExact(_dateShift + " " + timeShiftIn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _shiftIn))
                        {
                            //0706 <= 0800
                            if (dtIn <= _shiftIn)
                                dtIn = _shiftIn;
                        }


                    }
                    catch
                    {
                    }
                    #endregion



                    //GET REG HOURS
                    TimeSpan diff = dtOut.Subtract(dtIn);

                    int _hours = 0; int _minutes = 0;

                    _hours = diff.Hours;
                    _minutes = diff.Minutes;

                    int _breakHour = 0;
                    int _breakMin = 0;

                    try
                    {
                        if (lblBreakCredit.Text != "")
                        {
                            string[] _break = lblBreakCredit.Text.Split('.');
                            _breakHour = Convert.ToInt32(_break[0].ToString());
                            _breakMin = Convert.ToInt32(_break[1].ToString());

                        }
                    }
                    catch { }

                    int _abs = Math.Abs((_hours * 60) + _minutes);
                    _abs = _abs - ((_breakHour * 60) + _breakMin);

                    TimeSpan span = TimeSpan.FromMinutes(_abs);
                    _hours = span.Hours;
                    _minutes = span.Minutes;

                    if (_abs < 0)//-45
                        _regHour = "-" + Math.Abs(_hours).ToString("00") + Math.Abs(_minutes).ToString("00");//negative
                    else
                        _regHour = _hours.ToString("00") + _minutes.ToString("00");

                    ////GET REG HOURS
                    //_regHour = GetRegHours(dtIn, dtOut, Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);

                    //GET LATES AND POSSIBLE OT
                    lblLate.Text = GetLates(Convert.ToDateTime(dtIn.ToString()), Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);
                    //Under time
                    lblUT.Text = GetUndertime(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text);
                    lblPOT.Text = GetPOT(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text, lblOT.Text);

                    //REG HOUR
                    lblRegHour.Text = _regHour;
                    lblDateIn.Text = dtIn.ToString();
                    lblDateOut.Text = dtOut.ToString();

                }
                else
                {
                    lblRegHour.Text = "0000";
                    lblLate.Text = "0000";
                    lblUT.Text = "0000";
                    lblPOT.Text = "0000";
                    lblDateIn.Text = "";
                    lblDateOut.Text = "";
                }

            }
            catch
            {
                lblRegHour.Text = _regHour;
                lblLate.Text = "0000";
                lblUT.Text = "0000";
                lblPOT.Text = "0000";
            }
        }
        #endregion

        //GET FILED AND APPROVED LEAVES
        #region GetFileAndApprovedLeaves
        //FILED AND APPROVED LEAVE
        lblLeaveDetails.Text = "";
        DataTable dtLeave = new DataTable();
        dtLeave = _sched.GetLeavePerEmployeePerDate();
        if (dtLeave.Rows.Count > 0)
        {
            string _status = "";
            if (dtLeave.Rows[0]["leaveStatus"].ToString() == "")
            {
                _status = "Filed: " + Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
            }
            else
            {
                _status = dtLeave.Rows[0]["leaveStatus"].ToString() + ": " + Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
                lblLeave.Text = Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00");
            }
            if (!_status.Contains("Approved"))
            {
                lblLeave.Text = "0000";
            }
            lblLeaveDetails.Text = _status;
        }

        //FILED AND APPROVED OT
        lblOTDetails.Text = "";
        DataTable dtOT = new DataTable();
        dtOT = _sched.GetOTPerEmployeePerDate();
        if (dtOT.Rows.Count > 0)
        {
            string _otHours = "0000";
            DateTime dtStart = Convert.ToDateTime(dtOT.Rows[0]["timeStart"].ToString());
            DateTime dtEnd = Convert.ToDateTime(dtOT.Rows[0]["timeEnd"].ToString());
            try
            {

                TimeSpan ot = dtEnd.Subtract(dtStart);

                int _hoursOT = 0;
                int _minutesOT = 0;

                _hoursOT = ot.Hours;
                _minutesOT = ot.Minutes;

                if (((_hoursOT * 60) + _minutesOT) < 0)//-45
                    _otHours = "0000";

                else
                    _otHours = Math.Abs(_hoursOT).ToString("00") + Math.Abs(_minutesOT).ToString("00");
            }
            catch
            {

            }
            string _status = "";

            if (dtOT.Rows[0]["OTStatus"].ToString() == "")
            {
                _status = "Filed: " + _otHours + "hr/s";// +Convert.ToDecimal(dtOT.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
            }
            else
            {
                _status = dtOT.Rows[0]["OTStatus"].ToString() + ": " + _otHours + "hr/s";
                lblOT.Text = _otHours;// Convert.ToDecimal(dtOT.Rows[0]["NoOfHours"].ToString()).ToString("0.00");
            }
            if (!_status.Contains("Approved"))
            {
                lblOT.Text = "0000";
            }
            lblOTDetails.Text = _status;
        }
        #endregion



        //GET HOLIDAYS
        #region GetHolidays
        lblIsHoliday.Visible = false;
        lblIsHoliday.Text = "";


        Holidays _holidays = new Holidays();
        _holidays.Date = Convert.ToDateTime(lblDate.Text);

        DataTable dtHolidays = new DataTable();
        dtHolidays = _holidays.GetHolidaysPerDate();

        if (dtHolidays.Rows.Count > 0)
        {
            lblIsHoliday.Text = " / " + dtHolidays.Rows[0]["HolidayDesc"].ToString();
            lblIsHoliday.Visible = true;
        }
        #endregion


        #region oldcodes
        //Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");
        //Label lblShift = (Label)e.Item.FindControl("lblShift");
        //Label lblShiftID = (Label)e.Item.FindControl("lblShiftID");


        //Label lblLeave = (Label)e.Item.FindControl("lblLeave");
        //Label lblRegHour = (Label)e.Item.FindControl("lblRegHour");
        //Label lblLate = (Label)e.Item.FindControl("lblLate");
        //Label lblUT = (Label)e.Item.FindControl("lblUT");
        //Label lblOT = (Label)e.Item.FindControl("lblOT");
        //Label lblPOT = (Label)e.Item.FindControl("lblPOT");

        //Label lblTimeInFrom = (Label)e.Item.FindControl("lblTimeInFrom");
        //Label lblTimeInTo = (Label)e.Item.FindControl("lblTimeInTo");
        //Label lblTimeOutFrom = (Label)e.Item.FindControl("lblTimeOutFrom");
        //Label lblTimeOutTo = (Label)e.Item.FindControl("lblTimeOutTo");

        //Label lblIsFlexible = (Label)e.Item.FindControl("lblIsFlexible");
        //Label lblCreditHours = (Label)e.Item.FindControl("lblCreditHours");
        //Label lblBreakCredit = (Label)e.Item.FindControl("lblBreakCredit");


        //Label lblDate = (Label)e.Item.FindControl("lblDate");
        //Label lblDateIn = (Label)e.Item.FindControl("lblDateIn");
        //Label lblDateOut = (Label)e.Item.FindControl("lblDateOut");


        //Label lblLeaveDetails = (Label)e.Item.FindControl("lblLeaveDetails");
        //Label lblOTDetails = (Label)e.Item.FindControl("lblOTDetails");


        //TextBox txtOut = (TextBox)e.Item.FindControl("txtOut");
        //TextBox txtIn = (TextBox)e.Item.FindControl("txtIn");

        ////get shift
        //ShiftSchedule _sched = new ShiftSchedule();
        //_sched.EmpID = lblEmpID.Text;
        //_sched.EffectivityDate = Convert.ToDateTime(lblDate.Text);

        //DataTable dtShift = new DataTable();
        //dtShift = _sched.GetShiftPerEmployeePerDate();
        //if (dtShift.Rows.Count > 0)
        //{
        //    lblShiftID.Text = dtShift.Rows[0]["ShiftID"].ToString();
        //    lblShift.Text = dtShift.Rows[0]["Code"].ToString();

        //    lblTimeInFrom.Text = dtShift.Rows[0]["TimeInFrom"].ToString();
        //    lblTimeInTo.Text = dtShift.Rows[0]["TimeInTo"].ToString();
        //    lblTimeOutFrom.Text = dtShift.Rows[0]["TimeOutFrom"].ToString();
        //    lblTimeOutTo.Text = dtShift.Rows[0]["TimeOutTo"].ToString();
        //    lblIsFlexible.Text = dtShift.Rows[0]["IsFlexible"].ToString();
        //    lblCreditHours.Text = dtShift.Rows[0]["ShiftCredit"].ToString();
        //    lblBreakCredit.Text = dtShift.Rows[0]["BreakCredit"].ToString();


        //    //RD and NL
        //    string _shiftCode = dtShift.Rows[0]["Code"].ToString();

        //    if (_shiftCode == "RD" || _shiftCode == "NL")
        //    {
        //        lblRegHour.Text = lblCreditHours.Text;
        //    }

        //    // if (sender != null)
        //    {
        //        //string _in = string.Empty;
        //        //_in = ((TextBox)sender).Text;

        //        //TextBox txt = (TextBox)sender;
        //        //txt.Focus();

        //        string _regHour = "0000";

        //        DateTime dtIn = new DateTime();
        //        DateTime dtOut = new DateTime();

        //        string _in = txtIn.Text;
        //        string _out = txtOut.Text;
        //        //try
        //        //{
        //        //    _in = Convert.ToDateTime(txtIn.Text).ToString("HHmm");
        //        //}
        //        //catch 
        //        //{

        //        //}
        //        //try
        //        //{
        //        //    _out = Convert.ToDateTime(txtOut.Text).ToString("HHmm");
        //        //}
        //        //catch
        //        //{

        //        //}
        //        try
        //        {
        //            if (txtOut.Text != "" && _in != "")
        //            {
        //                string _date = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
        //                string timestringIn = _date + " " + _in;
        //                string timestringOut = _date + " " + _out;

        //                //GET IN
        //                dtIn = GetDateIN(_in, timestringIn, _date);
        //                //GET OUT
        //                dtOut = GetDateOUT(_out, timestringOut, _date, lblCreditHours.Text, lblTimeInFrom.Text, _shiftCode, dtIn.ToString());

        //                //CHECK IF in is within shiftin
        //                #region checkshift
        //                DateTime _shiftIn = new DateTime();
        //                try
        //                {
        //                    string[] _shift = lblShift.Text.Split('-');
        //                    string _dateShift = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
        //                    string timeShiftIn = _dateShift + " " + _shift[0];

        //                    DateTime dt = DateTime.ParseExact(_shift[0], "HHmm", CultureInfo.InvariantCulture);
        //                    timeShiftIn = dt.ToString("HH:mm:ss");

        //                    if (DateTime.TryParseExact(_dateShift + " " + timeShiftIn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _shiftIn))
        //                    {
        //                        //0706 <= 0800
        //                        if (dtIn <= _shiftIn)
        //                            dtIn = _shiftIn;
        //                    }


        //                }
        //                catch
        //                {
        //                }
        //                #endregion



        //                //GET REG HOURS
        //                TimeSpan diff = dtOut.Subtract(dtIn);

        //                int _hours = 0; int _minutes = 0;

        //                _hours = diff.Hours;
        //                _minutes = diff.Minutes;

        //                int _breakHour = 0;
        //                int _breakMin = 0;

        //                try
        //                {
        //                    if (lblBreakCredit.Text != "")
        //                    {
        //                        string[] _break = lblBreakCredit.Text.Split('.');
        //                        _breakHour = Convert.ToInt32(_break[0].ToString());
        //                        _breakMin = Convert.ToInt32(_break[1].ToString());

        //                    }
        //                }
        //                catch { }

        //                int _abs = Math.Abs((_hours * 60) + _minutes);
        //                _abs = _abs - ((_breakHour * 60) + _breakMin);

        //                TimeSpan span = TimeSpan.FromMinutes(_abs);
        //                _hours = span.Hours;
        //                _minutes = span.Minutes;

        //                if (_abs < 0)//-45
        //                    _regHour = "-" + Math.Abs(_hours).ToString("00") + Math.Abs(_minutes).ToString("00");//negative
        //                else
        //                    _regHour = _hours.ToString("00") + _minutes.ToString("00");

        //                ////GET REG HOURS
        //                //_regHour = GetRegHours(dtIn, dtOut, Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);

        //                //GET LATES AND POSSIBLE OT
        //                lblLate.Text = GetLates(Convert.ToDateTime(dtIn.ToString()), Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);
        //                //Under time
        //                lblUT.Text = GetUndertime(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text);
        //                lblPOT.Text = GetPOT(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text, lblOT.Text);

        //                //REG HOUR
        //                lblRegHour.Text = _regHour;
        //                lblDateIn.Text = dtIn.ToString();
        //                lblDateOut.Text = dtOut.ToString();

        //            }
        //            else
        //            {
        //                lblRegHour.Text = "0000";
        //                lblLate.Text = "0000";
        //                lblUT.Text = "0000";
        //                lblPOT.Text = "0000";
        //                lblDateIn.Text = "";
        //                lblDateOut.Text = "";
        //            }

        //        }
        //        catch
        //        {
        //            lblRegHour.Text = _regHour;
        //            lblLate.Text = "0000";
        //            lblUT.Text = "0000";
        //            lblPOT.Text = "0000";
        //        }
        //    }

        //}
        //else
        //{
        //    txtOut.Enabled = false;
        //    txtIn.Enabled = false;
        //}

        ////FILED AND APPROVED LEAVE
        //lblLeaveDetails.Text = "";
        //DataTable dtLeave = new DataTable();
        //dtLeave = _sched.GetLeavePerEmployeePerDate();
        //if (dtLeave.Rows.Count > 0)
        //{
        //    string _status = "";
        //    if (dtLeave.Rows[0]["leaveStatus"].ToString() == "")
        //    {
        //        _status = "Filed: " + Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
        //    }
        //    else
        //    {
        //        _status = dtLeave.Rows[0]["leaveStatus"].ToString() + ": " + Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
        //        lblLeave.Text = Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00");
        //    }
        //    if (!_status.Contains("Approved"))
        //    {
        //        lblLeave.Text = "0000";
        //    }
        //    lblLeaveDetails.Text = _status;
        //}

        ////FILED AND APPROVED OT
        //lblOTDetails.Text = "";
        //DataTable dtOT = new DataTable();
        //dtOT = _sched.GetOTPerEmployeePerDate();
        //if (dtOT.Rows.Count > 0)
        //{
        //    string _otHours = "0000";
        //    DateTime dtStart = Convert.ToDateTime(dtOT.Rows[0]["timeStart"].ToString());
        //    DateTime dtEnd = Convert.ToDateTime(dtOT.Rows[0]["timeEnd"].ToString());
        //    try
        //    {

        //        TimeSpan ot = dtEnd.Subtract(dtStart);

        //        int _hoursOT = 0;
        //        int _minutesOT = 0;

        //        _hoursOT = ot.Hours;
        //        _minutesOT = ot.Minutes;

        //        if (((_hoursOT * 60) + _minutesOT) < 0)//-45
        //            _otHours = "0000";

        //        else
        //            _otHours = Math.Abs(_hoursOT).ToString("00") + Math.Abs(_minutesOT).ToString("00");
        //    }
        //    catch
        //    {

        //    }
        //    string _status = "";

        //    if (dtOT.Rows[0]["OTStatus"].ToString() == "")
        //    {
        //        _status = "Filed: " + _otHours + "hr/s";// +Convert.ToDecimal(dtOT.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
        //    }
        //    else
        //    {
        //        _status = dtOT.Rows[0]["OTStatus"].ToString() + ": " + _otHours + "hr/s";
        //        lblOT.Text = _otHours;// Convert.ToDecimal(dtOT.Rows[0]["NoOfHours"].ToString()).ToString("0.00");
        //    }
        //    if (!_status.Contains("Approved"))
        //    {
        //        lblOT.Text = "0000";
        //    }
        //    lblOTDetails.Text = _status;
        //}

        #endregion
    }
    protected void txtIn_TextChanged1(object sender, EventArgs e)
    {
        if (sender != null)
        {
            string _in = string.Empty;
            _in = ((TextBox)sender).Text;

            TextBox txt = (TextBox)sender;
            //txt.Focus();

            TextBox txtOut = (TextBox)txt.Parent.FindControl("txtOut");

            Label lblDate = (Label)txt.Parent.FindControl("lblDate");
            Label lblRegHour = (Label)txt.Parent.FindControl("lblRegHour");


            Label lblLate = (Label)txt.Parent.FindControl("lblLate");
            Label lblUT = (Label)txt.Parent.FindControl("lblUT");
            Label lblOT = (Label)txt.Parent.FindControl("lblOT");
            Label lblPOT = (Label)txt.Parent.FindControl("lblPOT");

            Label lblTimeInFrom = (Label)txt.Parent.FindControl("lblTimeInFrom");
            Label lblTimeInTo = (Label)txt.Parent.FindControl("lblTimeInTo");//FLEXIBLE
            Label lblTimeOutFrom = (Label)txt.Parent.FindControl("lblTimeOutFrom");
            Label lblTimeOutTo = (Label)txt.Parent.FindControl("lblTimeOutTo");//FLEXIBLE

            Label lblIsFlexible = (Label)txt.Parent.FindControl("lblIsFlexible");
            Label lblCreditHours = (Label)txt.Parent.FindControl("lblCreditHours");
            Label lblBreakCredit = (Label)txt.Parent.FindControl("lblBreakCredit");

            Label lblDateInOrig = (Label)txt.Parent.FindControl("lblDateInOrig");
            Label lblDateIn = (Label)txt.Parent.FindControl("lblDateIn");
            Label lblDateOut = (Label)txt.Parent.FindControl("lblDateOut");
            Label lblShift = (Label)txt.Parent.FindControl("lblShift");

            string _regHour = "0000";

            DateTime dtIn = new DateTime();
            DateTime dtOut = new DateTime();

            string _out = txtOut.Text;

            try
            {
                if (txtOut.Text != "" && _in != "")
                {
                    string _date = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                    string timestringIn = _date + " " + _in;
                    string timestringOut = _date + " " + _out;

                    //GET IN
                    dtIn = GetDateIN(_in, timestringIn, _date);
                    //GET OUT
                    dtOut = GetDateOUT(_out, timestringOut, _date, lblCreditHours.Text, lblTimeInFrom.Text, lblShift.Text, dtIn.ToString());


                    //CHECK IF in is within shiftin
                    #region checkshift
                    DateTime _shiftIn = new DateTime();
                    try
                    {
                        string[] _shift = lblShift.Text.Split('-');
                        string _dateShift = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                        string timeShiftIn = _dateShift + " " + _shift[0];

                        DateTime dt = DateTime.ParseExact(_shift[0], "HHmm", CultureInfo.InvariantCulture);
                        timeShiftIn = dt.ToString("HH:mm:ss");

                        lblDateInOrig.Text = dtIn.ToString();

                        if (DateTime.TryParseExact(_dateShift + " " + timeShiftIn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _shiftIn))
                        {
                            //0706 <= 0800
                            if (dtIn <= _shiftIn)
                                dtIn = _shiftIn;
                        }


                    }
                    catch
                    {
                    }
                    #endregion



                    //GET REG HOURS
                    TimeSpan diff = dtOut.Subtract(dtIn);

                    int _hours = 0; int _minutes = 0;

                    _hours = diff.Hours;
                    _minutes = diff.Minutes;

                    int _breakHour = 0;
                    int _breakMin = 0;

                    try
                    {
                        if (lblBreakCredit.Text != "")
                        {
                            string[] _break = lblBreakCredit.Text.Split('.');
                            _breakHour = Convert.ToInt32(_break[0].ToString());
                            _breakMin = Convert.ToInt32(_break[1].ToString());

                        }
                    }
                    catch { }

                    int _abs = Math.Abs((_hours * 60) + _minutes);
                    _abs = _abs - ((_breakHour * 60) + _breakMin);

                    TimeSpan span = TimeSpan.FromMinutes(_abs);
                    _hours = span.Hours;
                    _minutes = span.Minutes;

                    if (_abs < 0)//-45
                        _regHour = "-" + Math.Abs(_hours).ToString("00") + Math.Abs(_minutes).ToString("00");//negative
                    else
                        _regHour = _hours.ToString("00") + _minutes.ToString("00");

                    ////GET REG HOURS
                    //_regHour = GetRegHours(dtIn, dtOut, Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);

                    //GET LATES AND POSSIBLE OT
                    lblLate.Text = GetLates(Convert.ToDateTime(dtIn.ToString()), Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);
                    //Under time
                    lblUT.Text = GetUndertime(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text);
                    lblPOT.Text = GetPOT(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text, lblOT.Text);

                    //REG HOUR
                    lblRegHour.Text = _regHour;
                    lblDateIn.Text = dtIn.ToString();
                    lblDateOut.Text = dtOut.ToString();

                }
                else
                {
                    lblRegHour.Text = "0000";
                    lblLate.Text = "0000";
                    lblUT.Text = "0000";
                    lblPOT.Text = "0000";
                    lblDateIn.Text = "";
                    lblDateOut.Text = "";
                }

            }
            catch
            {
                lblRegHour.Text = _regHour;
                lblLate.Text = "0000";
                lblUT.Text = "0000";
                lblPOT.Text = "0000";
            }
        }
    }
    protected void txtOut_TextChanged1(object sender, EventArgs e)
    {
        if (sender != null)
        {
            string _out = string.Empty;
            _out = ((TextBox)sender).Text;

            TextBox txt = (TextBox)sender;
            //txt.Focus();

            TextBox txtIn = (TextBox)txt.Parent.FindControl("txtIn");

            Label lblDate = (Label)txt.Parent.FindControl("lblDate");
            Label lblRegHour = (Label)txt.Parent.FindControl("lblRegHour");


            Label lblLate = (Label)txt.Parent.FindControl("lblLate");
            Label lblUT = (Label)txt.Parent.FindControl("lblUT");
            Label lblOT = (Label)txt.Parent.FindControl("lblOT");
            Label lblPOT = (Label)txt.Parent.FindControl("lblPOT");

            Label lblTimeInFrom = (Label)txt.Parent.FindControl("lblTimeInFrom");
            Label lblTimeInTo = (Label)txt.Parent.FindControl("lblTimeInTo");//FLEXIBLE
            Label lblTimeOutFrom = (Label)txt.Parent.FindControl("lblTimeOutFrom");
            Label lblTimeOutTo = (Label)txt.Parent.FindControl("lblTimeOutTo");//FLEXIBLE

            Label lblIsFlexible = (Label)txt.Parent.FindControl("lblIsFlexible");
            Label lblCreditHours = (Label)txt.Parent.FindControl("lblCreditHours");
            Label lblBreakCredit = (Label)txt.Parent.FindControl("lblBreakCredit");

            Label lblDateInOrig = (Label)txt.Parent.FindControl("lblDateInOrig");
            Label lblDateIn = (Label)txt.Parent.FindControl("lblDateIn");
            Label lblDateOut = (Label)txt.Parent.FindControl("lblDateOut");
            Label lblShift = (Label)txt.Parent.FindControl("lblShift");

            string _regHour = "0000";

            DateTime dtIn = new DateTime();
            DateTime dtOut = new DateTime();

            string _in = txtIn.Text;

            try
            {
                if (txtIn.Text != "" && _out != "")
                {
                    string _date = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                    string timestringIn = _date + " " + _in;
                    string timestringOut = _date + " " + _out;

                    //GET IN
                    dtIn = GetDateIN(_in, timestringIn, _date);
                    //GET OUT
                    dtOut = GetDateOUT(_out, timestringOut, _date, lblCreditHours.Text, lblTimeInFrom.Text, lblShift.Text, dtIn.ToString());

                    //CHECK IF in is within shiftin
                    #region checkshift
                    DateTime _shiftIn = new DateTime();
                    try
                    {
                        string[] _shift = lblShift.Text.Split('-');
                        string _dateShift = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                        string timeShiftIn = _dateShift + " " + _shift[0];

                        DateTime dt = DateTime.ParseExact(_shift[0], "HHmm", CultureInfo.InvariantCulture);
                        timeShiftIn = dt.ToString("HH:mm:ss");

                        lblDateInOrig.Text = dtIn.ToString();

                        if (DateTime.TryParseExact(_dateShift + " " + timeShiftIn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _shiftIn))
                        {
                            //0706 <= 0800
                            if (dtIn <= _shiftIn)
                                dtIn = _shiftIn;
                        }


                    }
                    catch
                    {
                    }
                    #endregion



                    //GET REG HOURS
                    TimeSpan diff = dtOut.Subtract(dtIn);

                    int _hours = 0; int _minutes = 0;

                    _hours = diff.Hours;
                    _minutes = diff.Minutes;

                    int _breakHour = 0;
                    int _breakMin = 0;

                    try
                    {
                        if (lblBreakCredit.Text != "")
                        {
                            string[] _break = lblBreakCredit.Text.Split('.');
                            _breakHour = Convert.ToInt32(_break[0].ToString());
                            _breakMin = Convert.ToInt32(_break[1].ToString());

                        }
                    }
                    catch { }

                    int _abs = Math.Abs((_hours * 60) + _minutes);
                    _abs = _abs - ((_breakHour * 60) + _breakMin);

                    TimeSpan span = TimeSpan.FromMinutes(_abs);
                    _hours = span.Hours;
                    _minutes = span.Minutes;


                    if (_abs < 0)//-45
                        _regHour = "-" + Math.Abs(_hours).ToString("00") + Math.Abs(_minutes).ToString("00");
                    else
                        _regHour = _hours.ToString("00") + _minutes.ToString("00");

                    ////GET REG HOURS
                    //_regHour = GetRegHours(dtIn, dtOut, Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);

                    //GET LATES AND POSSIBLE OT
                    lblLate.Text = GetLates(Convert.ToDateTime(dtIn.ToString()), Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);
                    //Under time
                    lblUT.Text = GetUndertime(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text);
                    lblPOT.Text = GetPOT(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text, lblOT.Text);

                    //REG HOUR
                    lblRegHour.Text = _regHour;
                    lblDateIn.Text = dtIn.ToString();
                    lblDateOut.Text = dtOut.ToString();

                }
                else
                {
                    lblRegHour.Text = "0000";
                    lblLate.Text = "0000";
                    lblUT.Text = "0000";
                    lblPOT.Text = "0000";

                    lblDateIn.Text = "";
                    lblDateOut.Text = "";
                }

            }
            catch
            {
                lblRegHour.Text = _regHour;
                lblLate.Text = "0000";
                lblUT.Text = "0000";
                lblPOT.Text = "0000";
            }
        }
    }
    #endregion


    protected void lnkDTRReport_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlDTRReport.Visible = true;
        btnGenerateReportDTR.Visible = false;


        //get employee list
        if (lblUsertype.Text == "admin")
        {
            //get employee list
            GetEmployeeList();
        }
        else
        {
            GetEmployeeListPerApprover();
        }

        //Payroll Period
        DataTable dt = new DataTable();
        Payroll _payroll = new Payroll();
        dt = _payroll.GetAllPayrollPeriod();

        ddlPayrollPeriod.DataSource = null;
        ddlPayrollPeriod.DataSource = dt;
        ddlPayrollPeriod.DataValueField = "id";
        ddlPayrollPeriod.DataTextField = "payrollDesc";
        ddlPayrollPeriod.DataBind();

        ddlPayrollPeriod.Items.Insert(0, new ListItem("Select Payroll Period", String.Empty));
        ddlPayrollPeriod.SelectedIndex = 0;
    }
    private void GetDTRReport()
    {
        rptrDTRReport.DataSource = null;
        rptrDTRReport.DataBind();

        DataTable dt = new DataTable();
        dt.Columns.Add("empID", Type.GetType("System.String"));
        dt.Columns.Add("FullName", Type.GetType("System.String"));


        Employee _emp = new Employee();

        lblDTRReportMsg.Visible = true;
        lblDTRReportMsg.Text = "";
        btnGenerateReportDTR.Visible = false;

        try
        {
            string empid = ddlEmployeeDTR.SelectedValue.ToString();
            _emp.EmployeeID = empid;
        }
        catch
        {
            lblDTRReportMsg.Text = "Select Employee.";

        }
        try
        {
            int payrollPeriod = Convert.ToInt32(ddlPayrollPeriod.SelectedValue.ToString());
        }
        catch
        {
            lblDTRReportMsg.Text = "Select payroll period.";
            return;
        }

        btnGenerateReportDTR.Visible = true;

        //dt = _emp.GetDTRReport();
        DataRow dr = null;
        if (ddlEmployeeDTR.SelectedValue.ToString() == "")
        {
            foreach (ListItem item in ddlEmployeeDTR.Items)
            {
                if (item.Value != "")
                {
                    dr = dt.NewRow();
                    dr["empID"] = item.Value;
                    dr["FullName"] = item.Text;
                    dt.Rows.Add(dr);
                }

            }
        }
        else
        {
            dr = dt.NewRow();
            dr["empID"] = ddlEmployeeDTR.SelectedValue.ToString();
            dr["FullName"] = ddlEmployeeDTR.SelectedItem.Text;
            dt.Rows.Add(dr);
        }


        rptrDTRReport.DataSource = null;
        rptrDTRReport.DataSource = dt;
        rptrDTRReport.DataBind();
    }

    protected void btnSearchDTR_Click(object sender, EventArgs e)
    {
        GetDTRReport();
    }
    protected void rptrDTRReport_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");
        Label lblShift = (Label)e.Item.FindControl("lblShift");
        Label lblTimeIn = (Label)e.Item.FindControl("lblTimeIn");
        Label lblTimeOut = (Label)e.Item.FindControl("lblTimeOut");


        Label lblAbsences = (Label)e.Item.FindControl("lblAbsences");
        Label lblLeaves = (Label)e.Item.FindControl("lblLeaves");
        Label lblHolidays = (Label)e.Item.FindControl("lblHolidays");

        Label lblLate = (Label)e.Item.FindControl("lblLate");
        Label lblUndertime = (Label)e.Item.FindControl("lblUndertime");

        Label lblRegNP = (Label)e.Item.FindControl("lblRegNP");
        Label lblRegOT = (Label)e.Item.FindControl("lblRegOT");
        Label lblRegOTNP = (Label)e.Item.FindControl("lblRegOTNP");
        Label lblRegOTEx = (Label)e.Item.FindControl("lblRegOTEx");
        Label lblRegOTExNP = (Label)e.Item.FindControl("lblRegOTExNP");

        Label lblLegOT = (Label)e.Item.FindControl("lblLegOT");
        Label lblLegOTNP = (Label)e.Item.FindControl("lblLegOTNP");
        Label lblLegOTEx = (Label)e.Item.FindControl("lblLegOTEx");
        Label lblLegOTExNP = (Label)e.Item.FindControl("lblLegOTExNP");

        Label lblSpOT = (Label)e.Item.FindControl("lblSpOT");
        Label lblSpOTNP = (Label)e.Item.FindControl("lblSpOTNP");
        Label lblSpOTEx = (Label)e.Item.FindControl("lblSpOTEx");
        Label lblSpOTExNP = (Label)e.Item.FindControl("lblSpOTExNP");


        Label lblRstOT = (Label)e.Item.FindControl("lblRstOT");
        Label lblRstOTNP = (Label)e.Item.FindControl("lblRstOTNP");
        Label lblRstOTEx = (Label)e.Item.FindControl("lblRstOTEx");
        Label lblRstOTExNP = (Label)e.Item.FindControl("lblRstOTExNP");

        Label lblLegRstOT = (Label)e.Item.FindControl("lblLegRstOT");
        Label lblLegRstOTNP = (Label)e.Item.FindControl("lblLegRstOTNP");
        Label lblLegRstOTEx = (Label)e.Item.FindControl("lblLegRstOTEx");
        Label lblLegRstOTExNP = (Label)e.Item.FindControl("lblLegRstOTExNP");

        Label lblSpRstOT = (Label)e.Item.FindControl("lblSpRstOT");
        Label lblSpRstOTNP = (Label)e.Item.FindControl("lblSpRstOTNP");
        Label lblSpRstOTEx = (Label)e.Item.FindControl("lblSpRstOTEx");
        Label lblSpRstOTExNP = (Label)e.Item.FindControl("lblSpRstOTExNP");



        Employee _emp = new Employee();

        #region variables

        int _absentCount = 0;
        decimal _leaves = 0;
        decimal _holidays = 0;
        decimal _lates = 0;
        decimal _undertime = 0;

        decimal _regNP = 0;
        decimal _regOT = 0;
        decimal _regOTNP = 0;
        decimal _regOTEx = 0;
        decimal _regOTExNP = 0;

        decimal _LegNP = 0;
        decimal _LegOT = 0;
        decimal _LegOTNP = 0;
        decimal _LegOTEx = 0;
        decimal _LegOTExNP = 0;

        decimal _SpNP = 0;
        decimal _SpOT = 0;
        decimal _SpOTNP = 0;
        decimal _SpOTEx = 0;
        decimal _SpOTExNP = 0;


        decimal _RstNP = 0;
        decimal _RstOT = 0;
        decimal _RstOTNP = 0;
        decimal _RstOTEx = 0;
        decimal _RstOTExNP = 0;

        decimal _LegRstNP = 0;
        decimal _LegRstOT = 0;
        decimal _LegRstOTNP = 0;
        decimal _LegRstOTEx = 0;
        decimal _LegRstOTExNP = 0;

        decimal _SpRstNP = 0;
        decimal _SpRstOT = 0;
        decimal _SpRstOTNP = 0;
        decimal _SpRstOTEx = 0;
        decimal _SpRstOTExNP = 0;

        #endregion

        //GET DTR DETAILS
        DataTable dtGetDTR = new DataTable();
        _emp = new Employee();
        _emp.EmployeeID = lblEmpID.Text;
        dtGetDTR = _emp.GetDTRReport_Absences(ddlPayrollPeriod.SelectedValue.ToString());

        if (dtGetDTR.Rows.Count > 0)
        {
            foreach (DataRow item in dtGetDTR.Rows)
            {
                string _shiftCode = item["Code"].ToString();
                string _shiftTimeIn = item["TimeInFrom"].ToString();
                string _shiftTimeOut = item["TimeOutFrom"].ToString();

                string _shiftCredit = item["ShiftCredit"].ToString();
                string _regHour = item["RegHour"].ToString();
                string _timeIn = item["DateIn"].ToString();
                string _timeOut = item["DateOut"].ToString();


                string _leave = item["Leave"].ToString();
                string _late = item["Late"].ToString();
                string _ut = item["UT"].ToString();
                string _ot = item["OT"].ToString();

                string _holidayType = item["HolidayType"].ToString();

                lblShift.Text = _shiftCode;
                lblTimeIn.Text = Convert.ToDateTime(_shiftTimeIn).ToString("HHmm");
                lblTimeOut.Text = Convert.ToDateTime(_shiftTimeOut).ToString("HHmm");

                decimal _credit = 0;

                try
                {
                    _credit = Convert.ToDecimal(_shiftCredit);
                }
                catch { }
                //try
                //{
                //    string first = item["RegHour"].ToString().Substring(0, 2);//04
                //    string last = item["RegHour"].ToString().Substring(2);//00


                //    _regHour = Convert.ToDecimal(first + "." + last);
                //}
                //catch { }

                if (_shiftCode != "RD" && _shiftCode != "NL")
                {
                    if (_regHour == "0000")
                    {
                        _absentCount++;
                    }
                }



                //LEAVES

                try
                {
                    _leaves += Convert.ToDecimal(_leave);
                }
                catch { }


                //HOLIDAYS
                try
                {
                    if (_holidayType != "")
                        _holidays++;
                }
                catch{}



                //LATES
                try
                {
                    string first = _late.Substring(0, 2);//04
                    string last = _late.Substring(2);//00

                    decimal minH = ((Convert.ToDecimal(first) * 60) + Convert.ToDecimal(last));

                    _lates += minH;

                }
                catch { }


                //UNDERTIME
                try
                {
                    string first = _ut.Substring(0, 2);//04
                    string last = _ut.Substring(2);//00

                    decimal minH = ((Convert.ToDecimal(first) * 60) + Convert.ToDecimal(last));
                    _undertime += minH;

                }
                catch { }



                //--------------------------------------------------------------------------


                //REG
                DateTime dtShiftIn = new DateTime();
                DateTime dtShiftOut = new DateTime();

                DateTime dtShiftInReg = new DateTime();
                DateTime dtShiftOutReg = new DateTime();

                DateTime dtTimeIn = new DateTime();
                DateTime dtTimeOut = new DateTime();

                try
                {
                    dtTimeIn = Convert.ToDateTime(Convert.ToDateTime(_timeIn).ToString("MMM dd yyyy HH:mm"));
                }
                catch { }
                try
                {
                    dtTimeOut = Convert.ToDateTime(Convert.ToDateTime(_timeOut).ToString("MMM dd yyyy HH:mm"));
                }
                catch { }

                try
                {
                    dtShiftIn = Convert.ToDateTime(dtTimeIn.ToString("MMM dd yyyy") + " " + Convert.ToDateTime(_shiftTimeIn).ToString("HH:mm"));
                    dtShiftInReg = Convert.ToDateTime(dtTimeIn.ToString("MMM dd yyyy") + " " + "22:00");
                }
                catch { }

                try
                {
                    dtShiftOut = Convert.ToDateTime(dtTimeOut.ToString("MMM dd yyyy") + " " + Convert.ToDateTime(_shiftTimeOut).ToString("HH:mm"));
                    dtShiftOutReg = Convert.ToDateTime(dtTimeOut.ToString("MMM dd yyyy") + " " + "06:00");

                }
                catch { }


                //if between 10 am and 6pm shift/ in and out
                //get time between 10 am an 6am
                //return (input > date1 && input < date2);

                //
                if (Between(dtShiftIn, dtShiftInReg, dtShiftOutReg) || Between(dtShiftOut, dtShiftInReg, dtShiftOutReg))
                {

                    TimeSpan tReg = new TimeSpan();

                    if (Between(dtShiftIn, dtShiftInReg, dtShiftOutReg))
                    {
                        if (dtTimeIn < dtShiftInReg.AddDays(-1))//1:00 am < 10:00 pm
                            dtTimeIn = dtShiftInReg;
                    }
                    else
                    {
                        dtTimeIn = dtShiftInReg;
                    }
                    if (Between(dtShiftOut, dtShiftInReg, dtShiftOutReg))
                    {
                        if (dtTimeOut > dtShiftOutReg)
                            dtTimeOut = dtShiftOutReg;
                    }
                    else
                    {
                        dtTimeOut = dtShiftOutReg;
                    }

                    tReg = dtTimeOut.Subtract(dtTimeIn);//shift out + 1 day

                    int _hoursReg = 0;
                    int _minutesReg = 0;

                    _hoursReg = tReg.Hours;
                    _minutesReg = tReg.Minutes;

                    _hoursReg = _hoursReg * 60;



                    if (_shiftCode == "RD")
                    {
                        if (_holidayType == "Legal")
                            _LegRstNP += _hoursReg;
                        else if (_holidayType == "Special")
                            _SpRstNP += _hoursReg;
                        else
                            _RstNP += _hoursReg;
                    }
                    else
                    {

                        if (_holidayType == "Legal")
                            _LegNP += _hoursReg;
                        else if (_holidayType == "Special")
                            _SpNP += _hoursReg;
                        else
                            _regNP += _hoursReg;
                    }




                }
                else
                {
                    TimeSpan tReg = new TimeSpan();
                    tReg = dtTimeOut.Subtract(dtTimeIn);//shift out + 1 day

                    int _hoursReg = 0;
                    int _minutesReg = 0;

                    _hoursReg = tReg.Hours;
                    _minutesReg = tReg.Minutes;

                    _hoursReg = _hoursReg * 60;


                    if (_shiftCode == "RD")
                    {
                        if (_holidayType == "Legal")
                            _LegRstNP += _hoursReg;
                        else if (_holidayType == "Special")
                            _SpRstNP += _hoursReg;
                        else
                            _RstNP += _hoursReg;
                    }
                }

                //OT
                try
                {
                    string first = _ot.Substring(0, 2);//04
                    string last = _ot.Substring(2);//00

                    decimal minH = ((Convert.ToDecimal(first) * 60) + Convert.ToDecimal(last));

                    if (_shiftCode == "RD")
                    {
                        if (_holidayType == "Legal")
                            _LegRstOT += minH;
                        else if (_holidayType == "Special")
                            _SpRstOT += minH;
                        else
                            _RstOT += minH;
                    }
                    else
                    {
                        if (_holidayType == "Legal")
                            _LegOT += minH;
                        else if (_holidayType == "Special")
                            _SpOT += minH;
                        else
                            _regOT += minH;
                    }


                }
                catch { }

                //REG NP + OT
                #region REG NP + OT

                if (_shiftCode == "RD")
                {
                    if (_holidayType == "Legal")
                        _LegRstOTNP = (_LegRstOT + _LegRstNP);
                    else if (_holidayType == "Special")
                        _SpRstOTNP = (_SpRstOT + _SpRstNP);
                    else
                        _RstOTNP = (_RstOT + _RstNP);
                }
                else
                {
                    if (_holidayType == "Legal")
                        _LegOTNP = (_LegOT + _LegNP);
                    else if (_holidayType == "Special")
                        _SpOTNP = (_SpOT + _SpNP);
                    else
                        _regOTNP = (_regOT + _regNP);
                }

                #endregion


                //REG OT ex
                #region REG OT ex
               

                //8 * 60 = 480 (mins)
                if (_shiftCode == "RD")
                {
                    if (_holidayType == "Legal")
                    {
                        if (_LegRstOT > 480)
                            _LegRstOTEx = _LegRstOT - 480;
                    }
                    else if (_holidayType == "Special")
                    {
                        if (_SpRstOT > 480)
                            _SpRstOTEx = _SpRstOT - 480;
                    }
                    else
                    {
                        if (_RstOT > 480)
                            _RstOTEx = _RstOT - 480;
                    }
                }
                else
                {
                    if (_holidayType == "Legal")
                    {
                        if (_LegOT > 480)
                            _LegOTEx = _LegOT - 480;
                    }
                    else if (_holidayType == "Special")
                    {
                        if (_SpOT > 480)
                            _SpOTEx = _SpOT - 480;
                    }
                    else
                    {
                        if (_regOT > 480)
                            _regOTEx = _regOT - 480;
                    }
                }

                #endregion


                //REG OT ex + NP
                #region REG OT ex + NP

                //8 * 60 = 480 (mins)
                if (_shiftCode == "RD")
                {
                    if (_holidayType == "Legal")
                    {
                        if (_LegRstOTEx > 0)
                            _LegRstOTExNP = _LegRstOTEx + _LegNP;
                    }
                    else if (_holidayType == "Special")
                    {
                        if (_SpRstOTEx > 0)
                            _SpRstOTExNP = _SpRstOTEx + _SpNP;
                    }
                    else
                    {
                        if (_RstOTEx > 0)
                            _RstOTExNP = _RstOTEx + _regNP;
                    }
                }
                else
                {
                    if (_holidayType == "Legal")
                    {
                        if (_LegOTEx > 0)
                            _LegOTExNP = _LegOTEx + _LegNP;
                    }
                    else if (_holidayType == "Special")
                    {
                        if (_SpOTEx > 0)
                            _SpOTExNP = _SpOTEx + _SpNP;
                    }
                    else
                    {
                        if (_regOTEx > 0)
                            _regOTExNP = _regOTEx + _regNP;
                    }
                }
                #endregion
                


            }//end foreach



            if (_absentCount > 0)
                lblAbsences.Text = _absentCount.ToString();
            if (_leaves > 0)
                lblLeaves.Text = _leaves.ToString();
            if (_holidays > 0)
                lblHolidays.Text = _holidays.ToString();

            if (_lates > 0)
                lblLate.Text = _lates.ToString();
            if (_undertime > 0)
                lblUndertime.Text = _undertime.ToString();


            if (_regNP > 0)
                lblRegNP.Text = (_regNP / 60).ToString();
            if (_regOT > 0)
                lblRegOT.Text = (_regOT / 60).ToString();
            if (_regOTNP > 0)
                lblRegOTNP.Text = (_regOTNP / 60).ToString();
            if (_regOTEx > 0)
                lblRegOTEx.Text = (_regOTEx / 60).ToString();
            if (_regOTExNP > 0)
                lblRegOTExNP.Text = (_regOTExNP / 60).ToString();


            if (_LegOT > 0)
                lblLegOT.Text = (_LegOT / 60).ToString();
            if (_LegOTNP > 0)
                lblLegOTNP.Text = (_LegOTNP / 60).ToString();
            if (_LegOTEx > 0)
                lblLegOTEx.Text = (_LegOTEx / 60).ToString();
            if (_LegOTExNP > 0)
                lblLegOTExNP.Text = (_LegOTExNP / 60).ToString();


            if (_SpOT > 0)
                lblSpOT.Text = (_SpOT / 60).ToString();
            if (_SpOTNP > 0)
                lblSpOTNP.Text = (_SpOTNP / 60).ToString();
            if (_SpOTEx > 0)
                lblSpOTEx.Text = (_SpOTEx / 60).ToString();
            if (_SpOTExNP > 0)
                lblSpOTExNP.Text = (_SpOTExNP / 60).ToString();


            if (_RstOT > 0)
                lblRstOT.Text = (_RstOT / 60).ToString();
            if (_RstOTNP > 0)
                lblRstOTNP.Text = (_RstOTNP / 60).ToString();
            if (_RstOTEx > 0)
                lblRstOTEx.Text = (_RstOTEx / 60).ToString();
            if (_RstOTExNP > 0)
                lblRstOTExNP.Text = (_RstOTExNP / 60).ToString();


            if (_LegRstOT > 0)
                lblLegRstOT.Text = (_LegRstOT / 60).ToString();
            if (_LegRstOTNP > 0)
                lblLegRstOTNP.Text = (_LegRstOTNP / 60).ToString();
            if (_LegRstOTEx > 0)
                lblLegRstOTEx.Text = (_LegRstOTEx / 60).ToString();
            if (_LegRstOTExNP > 0)
                lblLegRstOTExNP.Text = (_LegRstOTExNP / 60).ToString();

            if (_SpRstOT > 0)
                lblSpRstOT.Text = (_SpRstOT / 60).ToString();
            if (_SpRstOTNP > 0)
                lblSpRstOTNP.Text = (_SpRstOTNP / 60).ToString();
            if (_SpRstOTEx > 0)
                lblSpRstOTEx.Text = (_SpRstOTEx / 60).ToString();
            if (_SpRstOTExNP > 0)
                lblSpRstOTExNP.Text = (_SpRstOTExNP / 60).ToString();


        }
        else
        {
            lblAbsences.Text = "15";
        }


    }
    public static bool Between(DateTime input, DateTime date1, DateTime date2)
    {
        //05/19/2016 12:00, 05/19/2016 12:00, 05/20/2016 06:00, 
        bool test = false;
        test = (input >= date1 && input <= date2);

        return test;

    }
    protected void btnProcessDTR1_Click(object sender, EventArgs e)
    {

        ////GET ALL employees
        //DTR _dtr = new DTR();
        //DataTable dt = new DataTable();

        //dt.Columns.Add("id", Type.GetType("System.String"));
        //dt.Columns.Add("empID", Type.GetType("System.String"));
        //dt.Columns.Add("Date", Type.GetType("System.String"));
        //dt.Columns.Add("shift", Type.GetType("System.String"));
        //dt.Columns.Add("in", Type.GetType("System.String"));
        //dt.Columns.Add("out", Type.GetType("System.String"));
        //dt.Columns.Add("Leave", Type.GetType("System.String"));
        //dt.Columns.Add("RegHour", Type.GetType("System.String"));
        //dt.Columns.Add("Late", Type.GetType("System.String"));
        //dt.Columns.Add("UT", Type.GetType("System.String"));
        //dt.Columns.Add("OT", Type.GetType("System.String"));
        //dt.Columns.Add("POT", Type.GetType("System.String"));

        //dt.Columns.Add("TimeInFrom", Type.GetType("System.String"));
        //dt.Columns.Add("TimeInTo", Type.GetType("System.String"));
        //dt.Columns.Add("TimeOutFrom", Type.GetType("System.String"));
        //dt.Columns.Add("TimeOutTo", Type.GetType("System.String"));
        //dt.Columns.Add("IsFlexible", Type.GetType("System.String"));
        //dt.Columns.Add("ShiftCredit", Type.GetType("System.String"));
        //dt.Columns.Add("ShiftID", Type.GetType("System.String"));

        //dt.Columns.Add("BreakCredit", Type.GetType("System.String"));
        ////GET details per employees
        //int _count = 0;

        //foreach (ListItem item in ddlDTREmployee.Items)
        //{
        //    DataTable dtDetails = new DataTable();
        //    _dtr = new DTR();
        //    _dtr.EmpID = item.Value;
        //    //_dtr.EmpID = "RA0036";
        //    dtDetails = _dtr.GetDTRPerID(lblPayrollPeriodID.Text);

        //    rptrDTRUpload.DataSource = null;


        //    foreach (DataRow det in dtDetails.Rows)
        //    {
        //        string _empid = det["EmpID"].ToString();
        //        string _date = det["date"].ToString();
        //        string _timeIn = det["datein"].ToString();
        //        string _timeOut = det["dateout"].ToString();

        //        //UPDATE daily time record table

        //        try
        //        {
        //            DataRow dr = null;

        //            dr = dt.NewRow();

        //            dr["id"] = _count;
        //            dr["empid"] = _empid;
        //            dr["Date"] = Convert.ToDateTime(_date).ToString("ddd MMM dd, yyyy");
        //            dr["shift"] = "";
        //            try
        //            {
        //                dr["in"] = Convert.ToDateTime(_timeIn).ToString("HHmm");
        //            }
        //            catch
        //            {
        //                dr["in"] = "";
        //            }
        //            try
        //            {
        //                dr["out"] = Convert.ToDateTime(_timeOut).ToString("HHmm");
        //            }
        //            catch
        //            {
        //                dr["out"] = _timeOut;
        //            }
        //            dr["Leave"] = "0.00";
        //            dr["RegHour"] = "0000";
        //            dr["Late"] = "0000";
        //            dr["UT"] = "0000";
        //            dr["OT"] = "0000";
        //            dr["POT"] = "0000";

        //            dr["TimeInFrom"] = "";
        //            dr["TimeInTo"] = "";
        //            dr["TimeOutFrom"] = "";
        //            dr["TimeOutTo"] = "";
        //            dr["IsFlexible"] = "";
        //            dr["ShiftCredit"] = "";
        //            dr["ShiftID"] = "";
        //            dr["BreakCredit"] = "0000";


        //            dt.Rows.Add(dr);
        //            _count++;

        //        }
        //        catch
        //        {

        //        }
        //    }


        //}


        //rptrDTRUpload.DataSource = null;
        //rptrDTRUpload.DataSource = dt;
        //rptrDTRUpload.DataBind();



        //if (_count > 0)
        //{

        //    int saved = 0;
        //    //SAVE 
        //    try
        //    {
        //        if (rptrDTRUpload.Items.Count > 0)
        //        {
        //            foreach (RepeaterItem items in rptrDTRUpload.Items)
        //            {
        //                Label lblID = (Label)items.FindControl("lblID");
        //                Label lblEmpID = (Label)items.FindControl("lblEmpID");

        //                Label lblDate = (Label)items.FindControl("lblDate");
        //                Label lblShiftID = (Label)items.FindControl("lblShiftID");
        //                TextBox txtIn = (TextBox)items.FindControl("txtIn");
        //                TextBox txtOut = (TextBox)items.FindControl("txtOut");
        //                Label lblDateIn = (Label)items.FindControl("lblDateIn");
        //                Label lblDateOut = (Label)items.FindControl("lblDateOut");

        //                Label lblLeave = (Label)items.FindControl("lblLeave");
        //                Label lblRegHour = (Label)items.FindControl("lblRegHour");
        //                Label lblLate = (Label)items.FindControl("lblLate");
        //                Label lblUT = (Label)items.FindControl("lblUT");
        //                Label lblOT = (Label)items.FindControl("lblOT");
        //                Label lblPOT = (Label)items.FindControl("lblPOT");



        //                _dtr = new DTR();
        //                _dtr.ID = Convert.ToInt32(lblID.Text);
        //                _dtr.EmpID = lblEmpID.Text;
        //                _dtr.Date = Convert.ToDateTime(lblDate.Text);
        //                _dtr.ShiftID = Convert.ToInt32(lblShiftID.Text);

        //                if (txtIn.Text != "")
        //                {
        //                    try
        //                    {
        //                        _dtr.DateIn = Convert.ToDateTime(lblDateIn.Text);
        //                    }
        //                    catch
        //                    {
        //                        _dtr.DateIn = null;
        //                    }
        //                }
        //                else
        //                    _dtr.DateIn = null;
        //                if (txtOut.Text != "")
        //                {
        //                    try
        //                    {
        //                        _dtr.DateOut = Convert.ToDateTime(lblDateOut.Text);
        //                    }
        //                    catch
        //                    {
        //                        _dtr.DateOut = null;
        //                    }
        //                }
        //                else
        //                    _dtr.DateOut = null;

        //                _dtr.UserID = lblUserID.Text;

        //                _dtr.Leave = lblLeave.Text;
        //                _dtr.RegHour = lblRegHour.Text;
        //                _dtr.Late = lblLate.Text;
        //                _dtr.UT = lblUT.Text;
        //                _dtr.OT = lblOT.Text;
        //                _dtr.POT = lblPOT.Text;


        //                int rowAffected = _dtr.AddUpdateDTR();
        //                if (rowAffected > 0)
        //                {
        //                    saved++;
        //                }

        //            }
        //            if (saved > 0)
        //            {
        //                Response.Write("<script type=\"text/javascript\">alert('Success.');" + "window.close();</script>");
        //                return;
        //            }
        //            else
        //            {
        //                Response.Write("<script type=\"text/javascript\">alert('Failed to process dtr.');" + "window.close();</script>");
        //                return;
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}


    }
    protected void lnkImportShiftSchedule_Click(object sender, EventArgs e)
    {
        pnlShiftScheduleTaggingList.Visible = false;
        pnlShiftScheduleImport.Visible = true;

        lblImportMsg.Visible = false;
        lblImportMsg.Text = "";

    }
    protected void lnkImport_Click(object sender, EventArgs e)
    {
        lblImportMsg.Visible = false;
        lblImportMsg.Text = "";

        if (flImportShiftSchedule.HasFile)
        {
            string[] FileExt = flImportShiftSchedule.FileName.Split('.');
            string FileEx = FileExt[FileExt.Length - 1];
            if (FileEx.ToLower() == "xls" || FileEx.ToLower() == "xlsx")
            {
                string _fileName = Path.GetFileName(flImportShiftSchedule.PostedFile.FileName);
                string _extension = Path.GetExtension(flImportShiftSchedule.PostedFile.FileName);
                string _fileNameWithoutExtension = Path.GetFileNameWithoutExtension(flImportShiftSchedule.PostedFile.FileName);
                DateTime _dtNow = DateTime.Now;
                string _newFileName = "shiftSched_" + _dtNow.Year.ToString() + _dtNow.Month.ToString() + _dtNow.Day.ToString()
                    + _dtNow.Hour.ToString() + _dtNow.Minute.ToString() + _dtNow.Second.ToString() + _dtNow.Millisecond.ToString() + "." + _extension;

                string _filePath = Server.MapPath("MasterFiles//" + _newFileName);

                flImportShiftSchedule.SaveAs(_filePath);
                Excel _excel = new Excel();
                _excel.FilePath = _filePath;
                _excel.Extension = _extension;
                DataTable dtSchema = _excel.ExcelSchema();
                DataTable dtAssemble = DataAssembleShiftSched(dtSchema);

                DateTime _dateUploaded = DateTime.Now;

                EmployeeFile _empUpload = new EmployeeFile();
                _empUpload.FileName = _newFileName;
                _empUpload.OrigFileName = flImportShiftSchedule.FileName;
                _empUpload.DateCreated = DateTime.Now;
                _empUpload.Type = "shiftSchedule";
                _empUpload.UserId = Convert.ToInt32(Session["uid"].ToString());
                try { _empUpload.AddMasterFile(); }
                catch { }

                foreach (DataRow _row in dtAssemble.Rows)
                {
                    ShiftSchedule _sched = new ShiftSchedule();
                    _sched.EmpID = _row[0].ToString().ToUpper();
                    _sched.Code = _row[2].ToString().ToUpper();
                    _sched.UserID = lblUserID.Text;

                    //date
                    if (_row[1].ToString().Trim().Length > 0)
                    {
                        try
                        {
                            _sched.Date = Convert.ToDateTime(_row[1].ToString().Trim());
                        }
                        catch
                        {
                            _sched.Date = null;
                        }
                    }
                    else
                        _sched.Date = null;



                    if (_sched.SaveImportShiftSchedule() == 0)
                    {
                        lblImportMsg.Text = "Import failed.";
                        lblImportMsg.Visible = true;
                        break;
                    }
                    else
                    {
                        lblImportMsg.Text = "Success!";
                        lblImportMsg.Visible = true;
                    }
                }




                //Response.Write("<script type=\"text/javascript\">alert('Success.');" + "window.close();</script>");
                //return;

            }
            else
            {
                lblImportMsg.Text = "Invalid file format!";
                lblImportMsg.Visible = true;
            }

        }
    }
    protected void lnkImportCancel_Click(object sender, EventArgs e)
    {
        pnlShiftScheduleTaggingList.Visible = true;
        pnlShiftScheduleImport.Visible = false;

        //BIND
        GetShiftScheduleTagging();
    }
    protected void clndrShiftSchedule_DayRender(object sender, DayRenderEventArgs e)
    {
        e.Day.IsSelectable = false;

        if (_shiftEMPID == "")
        {
            clndrEmpShiftSchedule.Visible = false;
            return;
        }

        try
        {
            //get shift
            ShiftSchedule _sched = new ShiftSchedule();
            _sched.EmpID = _shiftEMPID;
            _sched.EffectivityDate = Convert.ToDateTime(e.Day.Date.ToString());

            DataTable dtShift = new DataTable();
            dtShift = _sched.GetShiftSchedPerEmployeePerDate();
            if (dtShift.Rows.Count > 0)
            {
                string Code = dtShift.Rows[0]["Code"].ToString();
                if (e.Cell.Controls.Count < 2)
                    e.Cell.Controls.Add(new LiteralControl("<p>" + Code + "</p>"));
            }
        }
        catch
        {

        }

    }
    protected void clndrShiftSchedule_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        if (e.NewDate.Month > e.PreviousDate.Month)
        {
            GetCalendarShift(ddlEmployeeShiftScheduleSearch.SelectedItem.Value);
        }
        else
        {
            GetCalendarShift(ddlEmployeeShiftScheduleSearch.SelectedItem.Value);
        }
    }

    //CHANGE SCHEDULE REQUESTS
    protected void lnkChangeShiftScheduleRequests_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlChangeScheduleRequests.Visible = true;
        pnlChangeScheduleRequestsList.Visible = true;
        pnlChangeScheduleRequestsNew.Visible = false;
        GetChangeScheduleRequests();

    }
    private void GetChangeScheduleRequests()
    {
        ShiftSchedule _sched = new ShiftSchedule();
        DataTable dt = new DataTable();
        dt = _sched.GetAllShiftScheduleRequest(lblUsertype.Text, lblUserID.Text);

        rptrChangeSchedule.DataSource = null;
        rptrChangeSchedule.DataSource = dt;
        rptrChangeSchedule.DataBind();

    }
    protected void lnkAddChangeSchedule_Click(object sender, EventArgs e)
    {
        pnlChangeScheduleRequestsList.Visible = false;
        pnlChangeScheduleRequestsNew.Visible = true;

        lblChangeScheduleMsg.Visible = false;
        lblChangeScheduleMsg.Text = "";
        lblChangeScheduleID.Text = "0";

        if (lblUsertype.Text == "admin")
        {
            //get employee list
            GetEmployeeList();
        }
        else
        {
            GetEmployeeListPerApprover();
        }
        //get shift code
        GetShiftScheduleList();

        ClearChangeSchedule();

        //
        if (lblUsertype.Text != "admin" && lblUsertype.Text != "supervisor")
        {
            ddlChangeScheduleEmployee.SelectedValue = lblUserID.Text;
            ddlChangeScheduleEmployee.Enabled = false;
        }
    }
    private void ClearChangeSchedule()
    {
        txtChangeDate.Text = "";
    }
    private bool ValidateChangeSchedule()
    {
        lblChangeScheduleMsg.Visible = true;

        if (ddlChangeScheduleEmployee.SelectedValue == "")
        {
            lblChangeScheduleMsg.Text = "Select employee.";
            ddlChangeScheduleEmployee.Focus();
            return false;
        }
        if (txtChangeDate.Text == "")
        {
            lblChangeScheduleMsg.Text = "Enter the credit date.";
            txtChangeDate.Focus();
            return false;
        }
        try
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txtChangeDate.Text);
        }
        catch
        {
            lblChangeScheduleMsg.Text = "Invalid credit date.";
            txtChangeDate.Focus();
            return false;
        }
        if (ddlShiftCode.SelectedValue == "")
        {
            lblChangeScheduleMsg.Text = "Select shift code.";
            ddlShiftCode.Focus();
            return false;
        }

        return true;
    }
    protected void btnSaveChangeSchedule_Click(object sender, EventArgs e)
    {
        if (ValidateChangeSchedule())
        {
            ShiftSchedule _sched = new ShiftSchedule();
            _sched.ID = Convert.ToInt32(lblChangeScheduleID.Text);
            _sched.EmpID = ddlChangeScheduleEmployee.SelectedValue;
            _sched.Date = Convert.ToDateTime(txtChangeDate.Text);
            _sched.ShiftID = Convert.ToInt32(ddlShiftCode.SelectedValue);
            _sched.UserID = lblUserID.Text;


            int rowAffected = _sched.SaveShiftScheduleRequest();

            if (rowAffected > 0)
            {
                lblChangeScheduleMsg.Visible = false;
                //BIND
                GetChangeScheduleRequests();
                pnlChangeScheduleRequestsNew.Visible = false;
                pnlChangeScheduleRequestsList.Visible = true;
            }
            else
            {
                lblChangeScheduleMsg.Visible = true;
                lblChangeScheduleMsg.Text = "Failed to save change schedule request. Employee's schedule for " + txtChangeDate.Text + " is not yet available.";
                return;
            }
        }
    }
    protected void btnChangeChangeSchedule_Click(object sender, EventArgs e)
    {
        pnlChangeScheduleRequestsList.Visible = true;
        pnlChangeScheduleRequestsNew.Visible = false;

        //BIND
        GetChangeScheduleRequests();
    }
    protected void rptrChangeSchedule_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Label lblID = (Label)e.Item.FindControl("lblID");
        Label lblEmployeeID = (Label)e.Item.FindControl("lblEmployeeID");
        Label lblDate = (Label)e.Item.FindControl("lblDate");
        Label lblShiftID = (Label)e.Item.FindControl("lblShiftID");
        Label lblShiftCode = (Label)e.Item.FindControl("lblShiftCode");
        Label lblStatus = (Label)e.Item.FindControl("lblStatus");


        if (e.CommandName == "Edit")
        {
            lblChangeScheduleMsg.Visible = false;
            lblChangeScheduleMsg.Text = "";

            pnlChangeScheduleRequestsNew.Visible = true;
            pnlChangeScheduleRequestsList.Visible = false;

            if (lblUsertype.Text == "admin")
            {
                //get employee list
                GetEmployeeList();
            }
            else
            {
                GetEmployeeListPerApprover();
            }

            //get shift code
            GetShiftScheduleList();

            lblChangeScheduleID.Text = lblID.Text;

            ddlChangeScheduleEmployee.SelectedValue = lblEmployeeID.Text;
            ddlShiftCode.SelectedValue = lblShiftID.Text;
            txtChangeDate.Text = lblDate.Text;


            //
            if (lblUsertype.Text != "admin" && lblUsertype.Text != "supervisor")
            {
                ddlChangeScheduleEmployee.Enabled = false;
            }
            else
            {
                //btnChangeScheduleAdd.Text = "Approve";
            }
        }
        //if (e.CommandName == "Approve")
        //{
        //    lblApproveID.Text = lblID.Text;
        //    lblApproveMsg.Text = "";
        //    lblApproveMsg.Visible = false;
        //    pnlApprove.Visible = true;
        //    lblApproveText.Text = "Approve";
        //    lnkApproveYes.Text = "Approve";
        //}
        //if (e.CommandName == "Deny")
        //{
        //    lblApproveID.Text = lblID.Text;
        //    lblApproveMsg.Text = "";
        //    lblApproveMsg.Visible = false;
        //    pnlApprove.Visible = true;
        //    lblApproveText.Text = "Deny";
        //    lnkApproveYes.Text = "Deny";

        //}
    }
    protected void rptrChangeSchedule_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblStatus = (Label)e.Item.FindControl("lblStatus");
        LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
        //LinkButton lnkApprove = (LinkButton)e.Item.FindControl("lnkApprove");
        //LinkButton lnkDeny = (LinkButton)e.Item.FindControl("lnkDeny");
        Label lblEmployeeID = (Label)e.Item.FindControl("lblEmployeeID");

        //lnkApprove.Visible = false;

        //lnkDeny.Visible = false;
        if (lblUsertype.Text == "supervisor" || lblUsertype.Text == "admin")
        {
            if (lblUsertype.Text == "supervisor" && lblEmployeeID.Text == lblUserID.Text)
            {
                //lnkDeny.Visible = false;
                lnkEdit.Visible = false;
            }
            else
            {
                //lnkLAApprove.Visible = true;
                //lnkDeny.Visible = true;
                lnkEdit.Text = "Update & Approve";

                if (lblStatus.Text == "Approved")
                {
                    //lnkDeny.Visible = true;
                    lnkEdit.Visible = false;
                }
                if (lblStatus.Text == "Denied")
                {
                    lnkEdit.Visible = true;
                    //lnkDeny.Visible = false;
                }
            }
        }
        else
        {
            if (lblStatus.Text == "Approved" || lblStatus.Text == "Denied")
            {
                lnkEdit.Visible = false;
                //lnkDeny.Visible = false;
            }
        }
    }
    protected void lnkDTR_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlDTR.Visible = true;

        //lblPayrollPeriod.Visible = false;
        //lnkSelectPayrollPeriod.Visible = false;
        //ddlDTREmployee.Visible = false;
        //btnSaveDTR.Visible = false;
        //btnProcessDTR1.Visible = false;
        //btnSaveDTR.Visible = false;
        //rptrDTR.DataSource = null;
        //rptrDTR.DataBind();

        lblPayrollPeriodDTR.Visible = false;
        lnkSelectPayrollPeriodDTR.Visible = false;
        ddlDTREmployeeDTR.Visible = false;
        btnSaveDTRDTR.Visible = false;
        btnProcessDTR1DTR.Visible = false;
        btnSaveDTRDTR.Visible = false;
        rptrDailyTimeRecord.DataSource = null;
        rptrDailyTimeRecord.DataBind();

       


        //get employee list
        if (lblUsertype.Text == "admin")
        {
            //get employee list
            GetEmployeeList();
        }
        else
        {
            GetEmployeeListPerApprover();
        }

        //if (lblPayrollPeriod.Text == "")
        //{

        //    ddlDTREmployee.SelectedIndex = 0;
        //    lnkSelectPayrollPeriod.Visible = true;

        //}
        //else
        //{
        //    lblPayrollPeriod.Visible = true;
        //    ddlDTREmployee.Visible = true;
        //    ddlDTREmployee.SelectedIndex = 0;
        //    btnProcessDTR1.Visible = true;

        //}

        if (lblPayrollPeriodDTR.Text == "")
        {

            ddlDTREmployeeDTR.SelectedIndex = 0;
            lnkSelectPayrollPeriodDTR.Visible = true;

        }
        else
        {
            lblPayrollPeriodDTR.Visible = true;
            ddlDTREmployeeDTR.Visible = true;
            ddlDTREmployeeDTR.SelectedIndex = 0;
            btnProcessDTR1DTR.Visible = true;

        }
    }
    protected void lnkSelectPayrollPeriodDTR_Click(object sender, EventArgs e)
    {
        pnlSelectPayrollPeriod.Visible = true;

        DataTable dt = new DataTable();
        Payroll _payroll = new Payroll();
        //_payroll.Year = DateTime.Now.Year;
        //_payroll.Month = DateTime.Now.ToString("MMMM");

        //dt = _payroll.GetAllPayrollPeriodPerMonth();

        dt = _payroll.GetAllPayrollPeriodActive();

        rptrSelectPayrollPeriod.DataSource = null;
        rptrSelectPayrollPeriod.DataSource = dt;
        rptrSelectPayrollPeriod.DataBind();

    }
    protected void btnProcessDTR1DTR_Click(object sender, EventArgs e)
    {
        //GET ALL EMPLOYEES
        rptrDTRUpload.DataSource = null;
       
        DTR _dtr = new DTR();
        DataTable dt = new DataTable();

        dt.Columns.Add("id", Type.GetType("System.String"));
        dt.Columns.Add("empID", Type.GetType("System.String"));
        dt.Columns.Add("Date", Type.GetType("System.String"));
        dt.Columns.Add("shift", Type.GetType("System.String"));
        dt.Columns.Add("in", Type.GetType("System.String"));
        dt.Columns.Add("out", Type.GetType("System.String"));
        dt.Columns.Add("Leave", Type.GetType("System.String"));
        dt.Columns.Add("RegHour", Type.GetType("System.String"));
        dt.Columns.Add("Late", Type.GetType("System.String"));
        dt.Columns.Add("UT", Type.GetType("System.String"));
        dt.Columns.Add("OT", Type.GetType("System.String"));
        dt.Columns.Add("POT", Type.GetType("System.String"));

        dt.Columns.Add("TimeInFrom", Type.GetType("System.String"));
        dt.Columns.Add("TimeInTo", Type.GetType("System.String"));
        dt.Columns.Add("TimeOutFrom", Type.GetType("System.String"));
        dt.Columns.Add("TimeOutTo", Type.GetType("System.String"));
        dt.Columns.Add("IsFlexible", Type.GetType("System.String"));
        dt.Columns.Add("ShiftCredit", Type.GetType("System.String"));
        dt.Columns.Add("ShiftID", Type.GetType("System.String"));

        dt.Columns.Add("BreakCredit", Type.GetType("System.String"));


        string _start = lblPayrollPeriodStartDTR.Text;
        string _end = lblPayrollPeriodEndDTR.Text;

        if (ddlDTREmployeeDTR.SelectedValue != "")
        {
            try
            {
                DateTime dtStart = new DateTime();
                DateTime dtEnd = new DateTime();

                dtStart = Convert.ToDateTime(_start);
                dtEnd = Convert.ToDateTime(_end);

                int _count = 1;
                for (DateTime date = dtStart; date <= dtEnd; date = date.AddDays(1))
                {
                    DataRow dr = null;
                    dr = dt.NewRow();

                    dr["id"] = _count;
                    dr["empid"] = ddlDTREmployeeDTR.SelectedValue;
                    dr["Date"] = date.ToString("ddd MMM dd, yyyy");
                    dr["shift"] = "";
                    dr["in"] = "";
                    dr["out"] = "";
                    dr["Leave"] = "0.00";
                    dr["RegHour"] = "0000";
                    dr["Late"] = "0000";
                    dr["UT"] = "0000";
                    dr["OT"] = "0000";
                    dr["POT"] = "0000";

                    dr["TimeInFrom"] = "";
                    dr["TimeInTo"] = "";
                    dr["TimeOutFrom"] = "";
                    dr["TimeOutTo"] = "";
                    dr["IsFlexible"] = "";
                    dr["ShiftCredit"] = "";
                    dr["ShiftID"] = "";
                    dr["BreakCredit"] = "0000";



                    dt.Rows.Add(dr);
                    _count++;

                }

            }
            catch
            {

            }
        }
        else
        {
            //ALL EMPLOYEES
            foreach (ListItem item in ddlDTREmployeeDTR.Items)
            {
                try
                {
                    DateTime dtStart = new DateTime();
                    DateTime dtEnd = new DateTime();

                    dtStart = Convert.ToDateTime(_start);
                    dtEnd = Convert.ToDateTime(_end);

                    int _count = 1;
                    for (DateTime date = dtStart; date <= dtEnd; date = date.AddDays(1))
                    {
                        DataRow dr = null;
                        dr = dt.NewRow();

                        dr["id"] = _count;
                        dr["empid"] = item.Value;
                        dr["Date"] = date.ToString("ddd MMM dd, yyyy");
                        dr["shift"] = "";
                        dr["in"] = "";
                        dr["out"] = "";
                        dr["Leave"] = "0.00";
                        dr["RegHour"] = "0000";
                        dr["Late"] = "0000";
                        dr["UT"] = "0000";
                        dr["OT"] = "0000";
                        dr["POT"] = "0000";

                        dr["TimeInFrom"] = "";
                        dr["TimeInTo"] = "";
                        dr["TimeOutFrom"] = "";
                        dr["TimeOutTo"] = "";
                        dr["IsFlexible"] = "";
                        dr["ShiftCredit"] = "";
                        dr["ShiftID"] = "";
                        dr["BreakCredit"] = "0000";



                        dt.Rows.Add(dr);
                        _count++;

                    }

                }
                catch
                {

                }
            }
        }


        rptrDTRUpload.DataSource = null;
        rptrDTRUpload.DataSource = dt;
        rptrDTRUpload.DataBind();

        //GET/SET DTR 
        //SAVE

        if (rptrDTRUpload.Items.Count > 0)
        {
            int saved = 0;
            foreach (RepeaterItem item in rptrDTRUpload.Items)
            {
                Label lblID = (Label)item.FindControl("lblID");
                Label lblEmpID = (Label)item.FindControl("lblEmpID");

                Label lblDate = (Label)item.FindControl("lblDate");
                Label lblShiftID = (Label)item.FindControl("lblShiftID");
                TextBox txtIn = (TextBox)item.FindControl("txtIn");
                TextBox txtOut = (TextBox)item.FindControl("txtOut");
                Label lblDateInOrig = (Label)item.FindControl("lblDateInOrig");
                Label lblDateIn = (Label)item.FindControl("lblDateIn");
                Label lblDateOut = (Label)item.FindControl("lblDateOut");

                Label lblLeave = (Label)item.FindControl("lblLeave");
                Label lblRegHour = (Label)item.FindControl("lblRegHour");
                Label lblLate = (Label)item.FindControl("lblLate");
                Label lblUT = (Label)item.FindControl("lblUT");
                Label lblOT = (Label)item.FindControl("lblOT");
                Label lblPOT = (Label)item.FindControl("lblPOT");

                int _shiftID = 0;
                try
                {
                    _shiftID = Convert.ToInt32(lblShiftID.Text);
                }
                catch
                {


                }


                _dtr = new DTR();
                _dtr.ID = Convert.ToInt32(lblID.Text);
                _dtr.EmpID = lblEmpID.Text;
                _dtr.Date = Convert.ToDateTime(lblDate.Text);
                _dtr.ShiftID = _shiftID;

                if (txtIn.Text != "")
                {
                    try
                    {
                        _dtr.DateIn = Convert.ToDateTime(lblDateInOrig.Text);
                    }
                    catch
                    {
                        _dtr.DateIn = null;
                    }
                }
                else
                    _dtr.DateIn = null;
                if (txtOut.Text != "")
                {
                    try
                    {
                        _dtr.DateOut = Convert.ToDateTime(lblDateOut.Text);
                    }
                    catch
                    {
                        _dtr.DateOut = null;
                    }
                }
                else
                    _dtr.DateOut = null;

                _dtr.UserID = lblUserID.Text;

                _dtr.Leave = lblLeave.Text;
                _dtr.RegHour = lblRegHour.Text;
                _dtr.Late = lblLate.Text;
                _dtr.UT = lblUT.Text;
                _dtr.OT = lblOT.Text;
                _dtr.POT = lblPOT.Text;


                int rowAffected = _dtr.AddUpdateDTR();
                 if (rowAffected > 0)
                 {
                     saved++;
                 }
                else
                {

                }

            }

             if (saved > 0)
             {
                 Response.Write("<script type=\"text/javascript\">alert('Success.');" + "window.close();</script>");
                 return;
             }
             else
             {
                 Response.Write("<script type=\"text/javascript\">alert('Failed to process dtr.');" + "window.close();</script>");
                 return;
             }
        }




        #region oldcodes

        ////GET ALL employees
        //DTR _dtr = new DTR();
        //DataTable dt = new DataTable();

        //dt.Columns.Add("id", Type.GetType("System.String"));
        //dt.Columns.Add("empID", Type.GetType("System.String"));
        //dt.Columns.Add("Date", Type.GetType("System.String"));
        //dt.Columns.Add("shift", Type.GetType("System.String"));
        //dt.Columns.Add("in", Type.GetType("System.String"));
        //dt.Columns.Add("out", Type.GetType("System.String"));
        //dt.Columns.Add("Leave", Type.GetType("System.String"));
        //dt.Columns.Add("RegHour", Type.GetType("System.String"));
        //dt.Columns.Add("Late", Type.GetType("System.String"));
        //dt.Columns.Add("UT", Type.GetType("System.String"));
        //dt.Columns.Add("OT", Type.GetType("System.String"));
        //dt.Columns.Add("POT", Type.GetType("System.String"));

        //dt.Columns.Add("TimeInFrom", Type.GetType("System.String"));
        //dt.Columns.Add("TimeInTo", Type.GetType("System.String"));
        //dt.Columns.Add("TimeOutFrom", Type.GetType("System.String"));
        //dt.Columns.Add("TimeOutTo", Type.GetType("System.String"));
        //dt.Columns.Add("IsFlexible", Type.GetType("System.String"));
        //dt.Columns.Add("ShiftCredit", Type.GetType("System.String"));
        //dt.Columns.Add("ShiftID", Type.GetType("System.String"));

        //dt.Columns.Add("BreakCredit", Type.GetType("System.String"));
        ////GET details per employees
        //int _count = 0;

        //foreach (ListItem item in ddlDTREmployeeDTR.Items)
        //{
        //    DataTable dtDetails = new DataTable();
        //    _dtr = new DTR();
        //    _dtr.EmpID = item.Value;
        //    //_dtr.EmpID = "RA0036";
        //    dtDetails = _dtr.GetDTRPerID(lblPayrollPeriodIDDTR.Text);

        //    rptrDTRUpload.DataSource = null;


        //    foreach (DataRow det in dtDetails.Rows)
        //    {
        //        string _empid = det["EmpID"].ToString();
        //        string _date = det["date"].ToString();
        //        string _timeIn = det["datein"].ToString();
        //        string _timeOut = det["dateout"].ToString();

        //        //UPDATE daily time record table

        //        try
        //        {
        //            DataRow dr = null;

        //            dr = dt.NewRow();

        //            dr["id"] = _count;
        //            dr["empid"] = _empid;
        //            dr["Date"] = Convert.ToDateTime(_date).ToString("ddd MMM dd, yyyy");
        //            dr["shift"] = "";
        //            try
        //            {
        //                dr["in"] = Convert.ToDateTime(_timeIn).ToString("HHmm");
        //            }
        //            catch
        //            {
        //                dr["in"] = "";
        //            }
        //            try
        //            {
        //                dr["out"] = Convert.ToDateTime(_timeOut).ToString("HHmm");
        //            }
        //            catch
        //            {
        //                dr["out"] = _timeOut;
        //            }
        //            dr["Leave"] = "0.00";
        //            dr["RegHour"] = "0000";
        //            dr["Late"] = "0000";
        //            dr["UT"] = "0000";
        //            dr["OT"] = "0000";
        //            dr["POT"] = "0000";

        //            dr["TimeInFrom"] = "";
        //            dr["TimeInTo"] = "";
        //            dr["TimeOutFrom"] = "";
        //            dr["TimeOutTo"] = "";
        //            dr["IsFlexible"] = "";
        //            dr["ShiftCredit"] = "";
        //            dr["ShiftID"] = "";
        //            dr["BreakCredit"] = "0000";


        //            dt.Rows.Add(dr);
        //            _count++;

        //        }
        //        catch
        //        {

        //        }
        //    }


        //}


        //rptrDTRUpload.DataSource = null;
        //rptrDTRUpload.DataSource = dt;
        //rptrDTRUpload.DataBind();







        //if (_count > 0)
        //{

        //    int saved = 0;
        //    //SAVE 
        //    try
        //    {
        //        if (rptrDTRUpload.Items.Count > 0)
        //        {
        //            foreach (RepeaterItem items in rptrDTRUpload.Items)
        //            {
        //                Label lblID = (Label)items.FindControl("lblID");
        //                Label lblEmpID = (Label)items.FindControl("lblEmpID");

        //                Label lblDate = (Label)items.FindControl("lblDate");
        //                Label lblShiftID = (Label)items.FindControl("lblShiftID");
        //                TextBox txtIn = (TextBox)items.FindControl("txtIn");
        //                TextBox txtOut = (TextBox)items.FindControl("txtOut");
        //                Label lblDateIn = (Label)items.FindControl("lblDateIn");
        //                Label lblDateOut = (Label)items.FindControl("lblDateOut");

        //                Label lblLeave = (Label)items.FindControl("lblLeave");
        //                Label lblRegHour = (Label)items.FindControl("lblRegHour");
        //                Label lblLate = (Label)items.FindControl("lblLate");
        //                Label lblUT = (Label)items.FindControl("lblUT");
        //                Label lblOT = (Label)items.FindControl("lblOT");
        //                Label lblPOT = (Label)items.FindControl("lblPOT");



        //                _dtr = new DTR();
        //                _dtr.ID = Convert.ToInt32(lblID.Text);
        //                _dtr.EmpID = lblEmpID.Text;
        //                _dtr.Date = Convert.ToDateTime(lblDate.Text);
        //                _dtr.ShiftID = Convert.ToInt32(lblShiftID.Text);

        //                if (txtIn.Text != "")
        //                {
        //                    try
        //                    {
        //                        _dtr.DateIn = Convert.ToDateTime(lblDateIn.Text);
        //                    }
        //                    catch
        //                    {
        //                        _dtr.DateIn = null;
        //                    }
        //                }
        //                else
        //                    _dtr.DateIn = null;
        //                if (txtOut.Text != "")
        //                {
        //                    try
        //                    {
        //                        _dtr.DateOut = Convert.ToDateTime(lblDateOut.Text);
        //                    }
        //                    catch
        //                    {
        //                        _dtr.DateOut = null;
        //                    }
        //                }
        //                else
        //                    _dtr.DateOut = null;

        //                _dtr.UserID = lblUserID.Text;

        //                _dtr.Leave = lblLeave.Text;
        //                _dtr.RegHour = lblRegHour.Text;
        //                _dtr.Late = lblLate.Text;
        //                _dtr.UT = lblUT.Text;
        //                _dtr.OT = lblOT.Text;
        //                _dtr.POT = lblPOT.Text;


        //                int rowAffected = _dtr.AddUpdateDTR();
        //                if (rowAffected > 0)
        //                {
        //                    saved++;
        //                }

        //            }
        //            if (saved > 0)
        //            {
        //                Response.Write("<script type=\"text/javascript\">alert('Success.');" + "window.close();</script>");
        //                return;
        //            }
        //            else
        //            {
        //                Response.Write("<script type=\"text/javascript\">alert('Failed to process dtr.');" + "window.close();</script>");
        //                return;
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}

        #endregion

    }
    protected void btnSaveDTRDTR_Click(object sender, EventArgs e)
    {
        if (rptrDailyTimeRecord.Items.Count > 0)
        {
            foreach (RepeaterItem item in rptrDailyTimeRecord.Items)
            {
                Label lblID = (Label)item.FindControl("lblID");
                Label lblEmpID = (Label)item.FindControl("lblEmpID");

                Label lblDate = (Label)item.FindControl("lblDate");
                Label lblShiftID = (Label)item.FindControl("lblShiftID");
                TextBox txtIn = (TextBox)item.FindControl("txtIn");
                TextBox txtOut = (TextBox)item.FindControl("txtOut");
                Label lblDateInOrig = (Label)item.FindControl("lblDateInOrig");
                Label lblDateIn = (Label)item.FindControl("lblDateIn");
                Label lblDateOut = (Label)item.FindControl("lblDateOut");

                Label lblLeave = (Label)item.FindControl("lblLeave");
                Label lblRegHour = (Label)item.FindControl("lblRegHour");
                Label lblLate = (Label)item.FindControl("lblLate");
                Label lblUT = (Label)item.FindControl("lblUT");
                Label lblOT = (Label)item.FindControl("lblOT");
                Label lblPOT = (Label)item.FindControl("lblPOT");

                int _shiftID = 0;
                try
                {
                    _shiftID = Convert.ToInt32(lblShiftID.Text);
                }
                catch
                {


                }


                DTR _dtr = new DTR();
                _dtr.ID = Convert.ToInt32(lblID.Text);
                _dtr.EmpID = lblEmpID.Text;
                _dtr.Date = Convert.ToDateTime(lblDate.Text);
                _dtr.ShiftID = _shiftID;

                if (txtIn.Text != "")
                {
                    try
                    {
                        _dtr.DateIn = Convert.ToDateTime(lblDateInOrig.Text);
                    }
                    catch
                    {
                        _dtr.DateIn = null;
                    }
                }
                else
                    _dtr.DateIn = null;
                if (txtOut.Text != "")
                {
                    try
                    {
                        _dtr.DateOut = Convert.ToDateTime(lblDateOut.Text);
                    }
                    catch
                    {
                        _dtr.DateOut = null;
                    }
                }
                else
                    _dtr.DateOut = null;

                _dtr.UserID = lblUserID.Text;

                _dtr.Leave = lblLeave.Text;
                _dtr.RegHour = lblRegHour.Text;
                _dtr.Late = lblLate.Text;
                _dtr.UT = lblUT.Text;
                _dtr.OT = lblOT.Text;
                _dtr.POT = lblPOT.Text;


                int rowAffected = _dtr.AddUpdateDTR();
                if (rowAffected > 0)
                {
                    //success
                }
                else
                {

                }

            }
        }
    }
    protected void ddlDTREmployeeDTR_SelectedIndexChanged(object sender, EventArgs e)
    {
        rptrDailyTimeRecord.DataSource = null;

        DataTable dt = new DataTable();

        dt.Columns.Add("id", Type.GetType("System.String"));
        dt.Columns.Add("empID", Type.GetType("System.String"));
        dt.Columns.Add("Date", Type.GetType("System.String"));
        dt.Columns.Add("shift", Type.GetType("System.String"));
        dt.Columns.Add("in", Type.GetType("System.String"));
        dt.Columns.Add("out", Type.GetType("System.String"));
        dt.Columns.Add("Leave", Type.GetType("System.String"));
        dt.Columns.Add("RegHour", Type.GetType("System.String"));
        dt.Columns.Add("Late", Type.GetType("System.String"));
        dt.Columns.Add("UT", Type.GetType("System.String"));
        dt.Columns.Add("OT", Type.GetType("System.String"));
        dt.Columns.Add("POT", Type.GetType("System.String"));

        dt.Columns.Add("TimeInFrom", Type.GetType("System.String"));
        dt.Columns.Add("TimeInTo", Type.GetType("System.String"));
        dt.Columns.Add("TimeOutFrom", Type.GetType("System.String"));
        dt.Columns.Add("TimeOutTo", Type.GetType("System.String"));
        dt.Columns.Add("IsFlexible", Type.GetType("System.String"));
        dt.Columns.Add("ShiftCredit", Type.GetType("System.String"));
        dt.Columns.Add("ShiftID", Type.GetType("System.String"));

        dt.Columns.Add("BreakCredit", Type.GetType("System.String"));



        string _start = lblPayrollPeriodStartDTR.Text;
        string _end = lblPayrollPeriodEndDTR.Text;
        try
        {
            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();

            dtStart = Convert.ToDateTime(_start);
            dtEnd = Convert.ToDateTime(_end);

            int _count = 1;
            for (DateTime date = dtStart; date <= dtEnd; date = date.AddDays(1))
            {
                DataRow dr = null;
                dr = dt.NewRow();

                dr["id"] = _count;
                dr["empid"] = ddlDTREmployeeDTR.SelectedValue.ToString();
                dr["Date"] = date.ToString("ddd MMM dd, yyyy");
                dr["shift"] = "";
                dr["in"] = "";
                dr["out"] = "";
                dr["Leave"] = "0.00";
                dr["RegHour"] = "0000";
                dr["Late"] = "0000";
                dr["UT"] = "0000";
                dr["OT"] = "0000";
                dr["POT"] = "0000";

                dr["TimeInFrom"] = "";
                dr["TimeInTo"] = "";
                dr["TimeOutFrom"] = "";
                dr["TimeOutTo"] = "";
                dr["IsFlexible"] = "";
                dr["ShiftCredit"] = "";
                dr["ShiftID"] = "";
                dr["BreakCredit"] = "0000";



                dt.Rows.Add(dr);
                _count++;

            }

        }
        catch
        {

        }

        if (ddlDTREmployeeDTR.SelectedValue != "")
        {
            btnSaveDTRDTR.Visible = true;

            rptrDailyTimeRecord.DataSource = null;
            rptrDailyTimeRecord.DataSource = dt;
            rptrDailyTimeRecord.DataBind();
        }
        else
        {
            btnSaveDTRDTR.Visible = false;

            rptrDailyTimeRecord.DataSource = null;
            rptrDailyTimeRecord.DataBind();
        }
    }
    protected void rptrDailyTimeRecord_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        //GET REPEATER ITEM
        #region prop
        Label lblEmpID = (Label)e.Item.FindControl("lblEmpID");
        Label lblShift = (Label)e.Item.FindControl("lblShift");
        Label lblShiftID = (Label)e.Item.FindControl("lblShiftID");

        Label lblDate = (Label)e.Item.FindControl("lblDate");
        Label lblDateIn = (Label)e.Item.FindControl("lblDateIn");
        Label lblDateInOrig = (Label)e.Item.FindControl("lblDateInOrig");
        Label lblDateOut = (Label)e.Item.FindControl("lblDateOut");
        TextBox txtOut = (TextBox)e.Item.FindControl("txtOut");
        TextBox txtIn = (TextBox)e.Item.FindControl("txtIn");

        Label lblIsHoliday = (Label)e.Item.FindControl("lblIsHoliday");


        Label lblTimeInFrom = (Label)e.Item.FindControl("lblTimeInFrom");
        Label lblTimeInTo = (Label)e.Item.FindControl("lblTimeInTo");
        Label lblTimeOutFrom = (Label)e.Item.FindControl("lblTimeOutFrom");
        Label lblTimeOutTo = (Label)e.Item.FindControl("lblTimeOutTo");

        Label lblIsFlexible = (Label)e.Item.FindControl("lblIsFlexible");
        Label lblCreditHours = (Label)e.Item.FindControl("lblCreditHours");
        Label lblBreakCredit = (Label)e.Item.FindControl("lblBreakCredit");

        Label lblLeave = (Label)e.Item.FindControl("lblLeave");
        Label lblRegHour = (Label)e.Item.FindControl("lblRegHour");
        Label lblLate = (Label)e.Item.FindControl("lblLate");
        Label lblUT = (Label)e.Item.FindControl("lblUT");
        Label lblOT = (Label)e.Item.FindControl("lblOT");
        Label lblPOT = (Label)e.Item.FindControl("lblPOT");

        Label lblLeaveDetails = (Label)e.Item.FindControl("lblLeaveDetails");
        Label lblOTDetails = (Label)e.Item.FindControl("lblOTDetails");

        DTR _dtr = new DTR();
        _dtr.EmpID = lblEmpID.Text;
        _dtr.Date = Convert.ToDateTime(lblDate.Text);

        #endregion

        //GET SHIFT DETAILS
        #region GetSHIFT
        //get shift
        ShiftSchedule _sched = new ShiftSchedule();
        _sched.EmpID = lblEmpID.Text;
        _sched.EffectivityDate = Convert.ToDateTime(lblDate.Text);

        DataTable dtShift = new DataTable();
        dtShift = _sched.GetShiftPerEmployeePerDate();
        if (dtShift.Rows.Count > 0)
        {
            lblShiftID.Text = dtShift.Rows[0]["ShiftID"].ToString();
            lblShift.Text = dtShift.Rows[0]["Code"].ToString();

            lblTimeInFrom.Text = dtShift.Rows[0]["TimeInFrom"].ToString();
            lblTimeInTo.Text = dtShift.Rows[0]["TimeInTo"].ToString();
            lblTimeOutFrom.Text = dtShift.Rows[0]["TimeOutFrom"].ToString();
            lblTimeOutTo.Text = dtShift.Rows[0]["TimeOutTo"].ToString();
            lblIsFlexible.Text = dtShift.Rows[0]["IsFlexible"].ToString();
            lblCreditHours.Text = dtShift.Rows[0]["ShiftCredit"].ToString();
            lblBreakCredit.Text = dtShift.Rows[0]["BreakCredit"].ToString();


            //RD and NL
            string _shiftCode = dtShift.Rows[0]["Code"].ToString();

            if (_shiftCode == "RD" || _shiftCode == "NL")
            {
                lblRegHour.Text = lblCreditHours.Text;
            }

        }
        else
        {
            txtOut.Enabled = false;
            txtIn.Enabled = false;
        }

        #endregion

        //GET TIME IN , TIME OUT FROM DTR LOGS
        #region GetTimeINAndOUT

        //check if already in dailytimerecordlatest table
        //TIME IN AND OUT

        DataTable dtDTR = new DataTable();
        dtDTR = _sched.GetDTRPerEmployeePerDate();

        if (dtDTR.Rows.Count > 0 && (dtDTR.Rows[0]["dateIn"].ToString() != "" && dtDTR.Rows[0]["dateOut"].ToString() != ""))
        {


            try
            {
                txtIn.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateIn"].ToString()).ToString("HHmm");
                lblDateIn.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateIn"].ToString()).ToString();
                lblDateInOrig.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateIn"].ToString()).ToString();

            }
            catch
            {
                txtIn.Text = "";
                lblDateIn.Text = "";
                lblDateInOrig.Text = "";

            }
            try
            {
                txtOut.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateOut"].ToString()).ToString("HHmm");
                lblDateOut.Text = Convert.ToDateTime(dtDTR.Rows[0]["dateOut"].ToString()).ToString();

            }
            catch
            {
                txtOut.Text = "";
                lblDateOut.Text = "";
            }


            lblLeave.Text = dtDTR.Rows[0]["leave"].ToString();
            lblRegHour.Text = dtDTR.Rows[0]["regHour"].ToString();
            lblLate.Text = dtDTR.Rows[0]["late"].ToString();
            lblUT.Text = dtDTR.Rows[0]["UT"].ToString();
            lblOT.Text = dtDTR.Rows[0]["OT"].ToString();
            lblPOT.Text = dtDTR.Rows[0]["POT"].ToString();

        }
        else
        {
            //else get logs from dtrLogs (sync)
            DataTable dtGetTimeInOut = new DataTable();
            dtGetTimeInOut = _dtr.GetTimeInOutFromLogsPerEmployeeDate();
            if (dtGetTimeInOut.Rows.Count > 0)
            {
                foreach (DataRow item in dtGetTimeInOut.Rows)
                {
                    string _logType = item["logType"].ToString();
                    if (_logType == "True")
                    {
                        //TIME IN
                        try
                        {
                            txtIn.Text = Convert.ToDateTime(item["scanDate"].ToString()).ToString("HHmm");
                            lblDateIn.Text = Convert.ToDateTime(item["scanDate"].ToString()).ToString();
                            lblDateInOrig.Text = Convert.ToDateTime(item["scanDate"].ToString()).ToString();
                        }
                        catch
                        {
                            txtIn.Text = "";
                            lblDateIn.Text = "";
                            lblDateInOrig.Text = "";
                        }
                    }
                    else
                    {
                        //TIME OUT
                        try
                        {
                            txtOut.Text = Convert.ToDateTime(item["scanDate"].ToString()).ToString("HHmm");
                            lblDateOut.Text = Convert.ToDateTime(item["scanDate"].ToString()).ToString();

                        }
                        catch
                        {
                            txtOut.Text = "";
                            lblDateOut.Text = "";
                        }
                    }
                }

            }
        }
        #endregion

        //SET LATES, LEAVES, OT, POT, UT VALUES
        #region setValues
        // if (sender != null)
        {
            //string _in = string.Empty;
            //_in = ((TextBox)sender).Text;

            //TextBox txt = (TextBox)sender;
            //txt.Focus();


            string _regHour = "0000";

            DateTime dtIn = new DateTime();
            DateTime dtOut = new DateTime();

            string _in = txtIn.Text;
            string _out = txtOut.Text;

            try
            {
                if (txtOut.Text != "" && _in != "")
                {
                    string _date = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                    string timestringIn = _date + " " + _in;
                    string timestringOut = _date + " " + _out;

                    //GET IN
                    dtIn = GetDateIN(_in, timestringIn, _date);
                    //GET OUT
                    dtOut = GetDateOUT(_out, timestringOut, _date, lblCreditHours.Text, lblTimeInFrom.Text, lblShift.Text, dtIn.ToString());

                    //CHECK IF in is within shiftin
                    #region checkshift
                    DateTime _shiftIn = new DateTime();
                    try
                    {
                        string[] _shift = lblShift.Text.Split('-');
                        string _dateShift = Convert.ToDateTime(lblDate.Text).ToString("MM/dd/yyyy");
                        string timeShiftIn = _dateShift + " " + _shift[0];

                        DateTime dt = DateTime.ParseExact(_shift[0], "HHmm", CultureInfo.InvariantCulture);
                        timeShiftIn = dt.ToString("HH:mm:ss");

                        lblDateInOrig.Text = dtIn.ToString();

                        if (DateTime.TryParseExact(_dateShift + " " + timeShiftIn, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _shiftIn))
                        {
                            //0706 <= 0800
                            if (dtIn <= _shiftIn)
                                dtIn = _shiftIn;
                        }


                    }
                    catch
                    {
                    }
                    #endregion



                    //GET REG HOURS
                    TimeSpan diff = dtOut.Subtract(dtIn);

                    int _hours = 0; int _minutes = 0;

                    _hours = diff.Hours;
                    _minutes = diff.Minutes;

                    int _breakHour = 0;
                    int _breakMin = 0;

                    try
                    {
                        if (lblBreakCredit.Text != "")
                        {
                            string[] _break = lblBreakCredit.Text.Split('.');
                            _breakHour = Convert.ToInt32(_break[0].ToString());
                            _breakMin = Convert.ToInt32(_break[1].ToString());

                        }
                    }
                    catch { }

                    int _abs = Math.Abs((_hours * 60) + _minutes);
                    _abs = _abs - ((_breakHour * 60) + _breakMin);

                    TimeSpan span = TimeSpan.FromMinutes(_abs);
                    _hours = span.Hours;
                    _minutes = span.Minutes;

                    if (_abs < 0)//-45
                        _regHour = "-" + Math.Abs(_hours).ToString("00") + Math.Abs(_minutes).ToString("00");//negative
                    else
                        _regHour = _hours.ToString("00") + _minutes.ToString("00");

                    ////GET REG HOURS
                    //_regHour = GetRegHours(dtIn, dtOut, Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);

                    //GET LATES AND POSSIBLE OT
                    lblLate.Text = GetLates(Convert.ToDateTime(dtIn.ToString()), Convert.ToDateTime(lblTimeInFrom.Text).ToString("HH:mm:ss"), lblShift.Text);
                    //Under time
                    lblUT.Text = GetUndertime(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text);
                    lblPOT.Text = GetPOT(_hours, _minutes, lblCreditHours.Text, lblDate.Text, lblShift.Text, lblOT.Text);

                    //REG HOUR
                    lblRegHour.Text = _regHour;
                    lblDateIn.Text = dtIn.ToString();
                    lblDateOut.Text = dtOut.ToString();

                }
                else
                {
                    lblRegHour.Text = "0000";
                    lblLate.Text = "0000";
                    lblUT.Text = "0000";
                    lblPOT.Text = "0000";
                    lblDateIn.Text = "";
                    lblDateOut.Text = "";
                }

            }
            catch
            {
                lblRegHour.Text = _regHour;
                lblLate.Text = "0000";
                lblUT.Text = "0000";
                lblPOT.Text = "0000";
            }
        }
        #endregion

        //GET FILED AND APPROVED LEAVES
        #region GetFileAndApprovedLeaves
        //FILED AND APPROVED LEAVE
        lblLeaveDetails.Text = "";
        DataTable dtLeave = new DataTable();
        dtLeave = _sched.GetLeavePerEmployeePerDate();
        if (dtLeave.Rows.Count > 0)
        {
            string _status = "";
            if (dtLeave.Rows[0]["leaveStatus"].ToString() == "")
            {
                _status = "Filed: " + Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
            }
            else
            {
                _status = dtLeave.Rows[0]["leaveStatus"].ToString() + ": " + Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
                lblLeave.Text = Convert.ToDecimal(dtLeave.Rows[0]["NoOfHours"].ToString()).ToString("0.00");
            }
            if (!_status.Contains("Approved"))
            {
                lblLeave.Text = "0000";
            }
            lblLeaveDetails.Text = _status;
        }

        //FILED AND APPROVED OT
        lblOTDetails.Text = "";
        DataTable dtOT = new DataTable();
        dtOT = _sched.GetOTPerEmployeePerDate();
        if (dtOT.Rows.Count > 0)
        {
            string _otHours = "0000";
            DateTime dtStart = Convert.ToDateTime(dtOT.Rows[0]["timeStart"].ToString());
            DateTime dtEnd = Convert.ToDateTime(dtOT.Rows[0]["timeEnd"].ToString());
            try
            {

                TimeSpan ot = dtEnd.Subtract(dtStart);

                int _hoursOT = 0;
                int _minutesOT = 0;

                _hoursOT = ot.Hours;
                _minutesOT = ot.Minutes;

                if (((_hoursOT * 60) + _minutesOT) < 0)//-45
                    _otHours = "0000";

                else
                    _otHours = Math.Abs(_hoursOT).ToString("00") + Math.Abs(_minutesOT).ToString("00");
            }
            catch
            {

            }
            string _status = "";

            if (dtOT.Rows[0]["OTStatus"].ToString() == "")
            {
                _status = "Filed: " + _otHours + "hr/s";// +Convert.ToDecimal(dtOT.Rows[0]["NoOfHours"].ToString()).ToString("0.00") + "hr/s";
            }
            else
            {
                _status = dtOT.Rows[0]["OTStatus"].ToString() + ": " + _otHours + "hr/s";
                lblOT.Text = _otHours;// Convert.ToDecimal(dtOT.Rows[0]["NoOfHours"].ToString()).ToString("0.00");
            }
            if (!_status.Contains("Approved"))
            {
                lblOT.Text = "0000";
            }
            lblOTDetails.Text = _status;
        }
        #endregion



        //GET HOLIDAYS
        #region GetHolidays
        lblIsHoliday.Visible = false;
        lblIsHoliday.Text = "";


        Holidays _holidays = new Holidays();
        _holidays.Date = Convert.ToDateTime(lblDate.Text);

        DataTable dtHolidays = new DataTable();
        dtHolidays = _holidays.GetHolidaysPerDate();

        if (dtHolidays.Rows.Count > 0)
        {
            lblIsHoliday.Text = " / " + dtHolidays.Rows[0]["HolidayDesc"].ToString();
            lblIsHoliday.Visible = true;
        }
        #endregion

    }
    protected void btnGenerateReportDTR_Click(object sender, EventArgs e)
    {

        lblDTRReportMsg.Visible = true;
        lblDTRReportMsg.Text = "";

        try
        {
            int payrollPeriod = Convert.ToInt32(ddlPayrollPeriod.SelectedValue.ToString());
        }
        catch
        {
            lblDTRReportMsg.Text = "Select payroll period.";
            return;
        }



        //GENERATE
        //Get the data from database into datatable
        DataTable dt = new DataTable();
        dt.Columns.Add("Employee ID", Type.GetType("System.String"));
        dt.Columns.Add("Name", Type.GetType("System.String"));
        dt.Columns.Add("Payroll Days", Type.GetType("System.String"));
        dt.Columns.Add("Absences (days)", Type.GetType("System.String"));
        dt.Columns.Add("Leaves (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("Holidays", Type.GetType("System.String"));

        dt.Columns.Add("Late (min)", Type.GetType("System.String"));
        dt.Columns.Add("Undertime (min)", Type.GetType("System.String"));



        dt.Columns.Add("Reg+NP (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("RegOT (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("RegOT+NP (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("RegOTEx (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("RegOTEx+NP (hrs)", Type.GetType("System.String"));

        dt.Columns.Add("LegOT (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("LegOT+NP (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("LegOTEx (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("LegOTEx+NP (hrs)", Type.GetType("System.String"));

        dt.Columns.Add("SpOT (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("SpOT+NP (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("SpOTEx (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("SpOTEx+NP (hrs)", Type.GetType("System.String"));



        dt.Columns.Add("RstOT (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("RstOT+NP (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("RstOTEx (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("RstOTEx+NP (hrs)", Type.GetType("System.String"));

        dt.Columns.Add("LegRstOT (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("LegRstOT+NP (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("LegRstOTEx (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("LegRstOTEx+NP (hrs)", Type.GetType("System.String"));

        dt.Columns.Add("SpRstOT (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("SpRstOT+NP (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("SpRstOTEx (hrs)", Type.GetType("System.String"));
        dt.Columns.Add("SpRstOTEx+NP (hrs)", Type.GetType("System.String"));



        DataRow drFirst = null;
        drFirst = dt.NewRow();
        drFirst["Employee ID"] = ddlPayrollPeriod.SelectedItem.Text;
        drFirst["Name"] = ddlPayrollPeriod.SelectedItem.Value;
        dt.Rows.Add(drFirst);

        DataRow drHeader = null;
        drHeader= dt.NewRow();
        drHeader["Employee ID"] = "Employee ID";
        drHeader["Name"] ="Name";
        drHeader["Payroll Days"] = "Payroll Days";
        drHeader["Absences (days)"] = "Absences (days)";
        drHeader["Leaves (hrs)"] = "Leaves (hrs)";
        drHeader["Holidays"] = "Holidays";

        drHeader["Late (min)"] = "Late (min)";
        drHeader["Undertime (min)"] = "Undertime (min)";

        drHeader["Reg+NP (hrs)"] = "Reg+NP (hrs)";
        drHeader["RegOT (hrs)"] = "RegOT (hrs)";
        drHeader["RegOT+NP (hrs)"] = "RegOT+NP (hrs)";
        drHeader["RegOTEx (hrs)"] = "RegOTEx (hrs)";
        drHeader["RegOTEx+NP (hrs)"] = "RegOTEx+NP (hrs)";

        drHeader["LegOT (hrs)"] = "LegOT (hrs)";
        drHeader["LegOT+NP (hrs)"] = "LegOT+NP (hrs)";
        drHeader["LegOTEx (hrs)"] = "LegOTEx (hrs)";
        drHeader["LegOTEx+NP (hrs)"] = "LegOTEx+NP (hrs)";

        drHeader["SpOT (hrs)"] = "SpOT (hrs)";
        drHeader["SpOT+NP (hrs)"] = "SpOT+NP (hrs)";
        drHeader["SpOTEx (hrs)"] = "SpOTEx (hrs)";
        drHeader["SpOTEx+NP (hrs)"] = "SpOTEx+NP (hrs)";

        drHeader["RstOT (hrs)"] = "RstOT (hrs)";
        drHeader["RstOT+NP (hrs)"] = "RstOT+NP (hrs)";
        drHeader["RstOTEx (hrs)"] = "RstOTEx (hrs)";
        drHeader["RstOTEx+NP (hrs)"] = "RstOTEx+NP (hrs)";

        drHeader["LegRstOT (hrs)"] = "LegRstOT (hrs)";
        drHeader["LegRstOT+NP (hrs)"] = "LegRstOT+NP (hrs)";
        drHeader["LegRstOTEx (hrs)"] = "LegRstOTEx (hrs)";
        drHeader["LegRstOTEx+NP (hrs)"] = "LegRstOTEx+NP (hrs)";

        drHeader["SpRstOT (hrs)"] = "SpRstOT (hrs)";
        drHeader["SpRstOT+NP (hrs)"] = "SpRstOT+NP (hrs)";
        drHeader["SpRstOTEx (hrs)"] = "SpRstOTEx (hrs)";
        drHeader["SpRstOTEx+NP (hrs)"] = "SpRstOTEx+NP (hrs)";

        dt.Rows.Add(drHeader);



        DataRow dr = null;
        foreach (RepeaterItem item in rptrDTRReport.Items)
        {
            Label lblEmpID = (Label)item.FindControl("lblEmpID");
            Label lblFullName = (Label)item.FindControl("lblFullName");
            Label lblPayrollDays = (Label)item.FindControl("lblPayrollDays");
            Label lblAbsences = (Label)item.FindControl("lblAbsences");
            Label lblLeaves = (Label)item.FindControl("lblLeaves");
            Label lblHolidays = (Label)item.FindControl("lblHolidays");

            Label lblLate = (Label)item.FindControl("lblLate");
            Label lblUndertime = (Label)item.FindControl("lblUndertime");


            Label lblRegNP = (Label)item.FindControl("lblRegNP");
            Label lblRegOT = (Label)item.FindControl("lblRegOT");
            Label lblRegOTNP = (Label)item.FindControl("lblRegOTNP");
            Label lblRegOTEx = (Label)item.FindControl("lblRegOTEx");
            Label lblRegOTExNP = (Label)item.FindControl("lblRegOTExNP");

            Label lblLegOT = (Label)item.FindControl("lblLegOT");
            Label lblLegOTNP = (Label)item.FindControl("lblLegOTNP");
            Label lblLegOTEx = (Label)item.FindControl("lblLegOTEx");
            Label lblLegOTExNP = (Label)item.FindControl("lblLegOTExNP");

            Label lblSpOT = (Label)item.FindControl("lblSpOT");
            Label lblSpOTNP = (Label)item.FindControl("lblSpOTNP");
            Label lblSpOTEx = (Label)item.FindControl("lblSpOTEx");
            Label lblSpOTExNP = (Label)item.FindControl("lblSpOTExNP");


            Label lblRstOT = (Label)item.FindControl("lblRstOT");
            Label lblRstOTNP = (Label)item.FindControl("lblRstOTNP");
            Label lblRstOTEx = (Label)item.FindControl("lblRstOTEx");
            Label lblRstOTExNP = (Label)item.FindControl("lblRstOTExNP");

            Label lblLegRstOT = (Label)item.FindControl("lblLegRstOT");
            Label lblLegRstOTNP = (Label)item.FindControl("lblLegRstOTNP");
            Label lblLegRstOTEx = (Label)item.FindControl("lblLegRstOTEx");
            Label lblLegRstOTExNP = (Label)item.FindControl("lblLegRstOTExNP");

            Label lblSpRstOT = (Label)item.FindControl("lblSpRstOT");
            Label lblSpRstOTNP = (Label)item.FindControl("lblSpRstOTNP");
            Label lblSpRstOTEx = (Label)item.FindControl("lblSpRstOTEx");
            Label lblSpRstOTExNP = (Label)item.FindControl("lblSpRstOTExNP");



            dr = dt.NewRow();
            dr["Employee ID"] = lblEmpID.Text;
            dr["Name"] = lblFullName.Text;
            dr["Payroll Days"] = lblPayrollDays.Text;
            dr["Absences (days)"] = lblAbsences.Text;
            dr["Leaves (hrs)"] = lblLeaves.Text;
            dr["Holidays"] = lblHolidays.Text;


            dr["Late (min)"] = lblLate.Text;
            dr["Undertime (min)"] = lblUndertime.Text;




            dr["Reg+NP (hrs)"] = lblRegNP.Text;
            dr["RegOT (hrs)"] = lblRegOT.Text;
            dr["RegOT+NP (hrs)"] = lblRegOTNP.Text;
            dr["RegOTEx (hrs)"] = lblRegOTEx.Text;
            dr["RegOTEx+NP (hrs)"] = lblRegOTExNP.Text;

            dr["LegOT (hrs)"] = lblLegOT.Text;
            dr["LegOT+NP (hrs)"] = lblLegOTNP.Text;
            dr["LegOTEx (hrs)"] = lblLegOTEx.Text;
            dr["LegOTEx+NP (hrs)"] = lblLegOTExNP.Text;

            dr["SpOT (hrs)"] = lblSpOT.Text;
            dr["SpOT+NP (hrs)"] = lblSpOTNP.Text;
            dr["SpOTEx (hrs)"] = lblSpOTEx.Text;
            dr["SpOTEx+NP (hrs)"] = lblSpOTExNP.Text;



            dr["RstOT (hrs)"] = lblRstOT.Text;
            dr["RstOT+NP (hrs)"] = lblRstOTNP.Text;
            dr["RstOTEx (hrs)"] = lblRstOTEx.Text;
            dr["RstOTEx+NP (hrs)"] = lblRstOTExNP.Text;

            dr["LegRstOT (hrs)"] = lblLegRstOT.Text;
            dr["LegRstOT+NP (hrs)"] = lblLegRstOTNP.Text;
            dr["LegRstOTEx (hrs)"] = lblLegRstOTEx.Text;
            dr["LegRstOTEx+NP (hrs)"] = lblLegRstOTExNP.Text;

            dr["SpRstOT (hrs)"] = lblSpRstOT.Text;
            dr["SpRstOT+NP (hrs)"] = lblSpRstOTNP.Text;
            dr["SpRstOTEx (hrs)"] = lblSpRstOTEx.Text;
            dr["SpRstOTEx+NP (hrs)"] = lblSpRstOTExNP.Text;

            dt.Rows.Add(dr);
        }

        DataRow drLast = null;
        drLast = dt.NewRow();
        drLast["Employee ID"] = ddlPayrollPeriod.SelectedItem.Text;
        drLast["Name"] = ddlPayrollPeriod.SelectedItem.Value;

        dt.Rows.Add(drLast);



        //Create a dummy GridView
        GridView GridView1 = new GridView();
        GridView1.AllowPaging = false;
        GridView1.ShowHeader = false;

        GridView1.DataSource = dt;
        GridView1.DataBind();


        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition",
         "attachment;filename=DTR_Report.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        //for (int i = 0; i < GridView1.Rows.Count; i++)
        //{
        //    //Apply text style to each Row
        //    GridView1.Rows[i].Attributes.Add("class", "textmode");
        //}
        GridView1.ShowHeader = false;
        GridView1.GridLines = GridLines.None;

        GridView1.RenderControl(hw);
        

        //style to format numbers to string
        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();


    }
    protected void lnkLeaveReport_Click(object sender, EventArgs e)
    {
        HidePanel();
        pnlLeaveReport.Visible = true;

        //get employee list
        if (lblUsertype.Text == "admin")
            GetEmployeeList();
        else
            GetEmployeeListPerApprover();

        //get leave list
        GetLeaveList();


        txtLeaveDateFrom.Text = DateTime.Now.ToString("MM/dd/yyyy");
        txtLeaveDateTo.Text = DateTime.Now.ToString("MM/dd/yyyy");



    }
    protected void btnSearchLeaveReport_Click(object sender, EventArgs e)
    {
        GetLeaveReport();
    }

    private void GetLeaveReport()
    {
        string _leaveType = ddlLeaveType.SelectedItem.Text;
        string _empID = ddlEmployeeLeave.SelectedItem.Value;


        if (ddlLeaveType.SelectedItem.Text == "All Leaves")
        {
            _leaveType = "";
        }
      
        DateTime dtFrom = Convert.ToDateTime(txtLeaveDateFrom.Text);
        DateTime dtTo = Convert.ToDateTime(txtLeaveDateTo.Text);


        DataTable dt = new DataTable();

        LeaveManagement _leave = new LeaveManagement();
        dt = _leave.GetLeaveReport(_empID, _leaveType, dtFrom, dtTo);


        rptrLeaveReport.DataSource = null;
        rptrLeaveReport.DataSource = dt;
        rptrLeaveReport.DataBind();



    }
    protected void rptrLeaveReport_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblStatus = (Label)e.Item.FindControl("lblStatus");
        Label lblApproveDate = (Label)e.Item.FindControl("lblApproveDate");
        Label lblDeniedDate = (Label)e.Item.FindControl("lblDeniedDate");

        lblApproveDate.Visible = false;
        lblDeniedDate.Visible = false;

        if (lblStatus.Text == "Approved")
            lblApproveDate.Visible = true;
        else if (lblStatus.Text == "Denied")
            lblDeniedDate.Visible = true;
        else
            lblStatus.Text = "Pending";

    }
}
