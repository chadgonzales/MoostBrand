using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Visible = false;
    }
    protected void btnLogIn_Click(object sender, EventArgs e)
    {
         if ((txtUsername.Text == string.Empty) || (txtUsername.Text.Replace(" ", "") == ""))
        {
            txtUsername.Focus();
            lblMsg.Text = "Username is required.";
            lblMsg.Visible = true;
            return;
        }
        if ((txtPassword.Text == string.Empty) || (txtPassword.Text.Replace(" ", "") == ""))
        {
            txtPassword.Focus();
            lblMsg.Text = "Password is required.";
            lblMsg.Visible = true;
            return;
        }

        Users _user = new Users();
        _user.Username = txtUsername.Text;
        _user.Password = txtPassword.Text;

        DataTable _dtUser = _user.Authenticate();
        if (_dtUser.Rows.Count > 0)
        {
            Session["uid"] = _dtUser.Rows[0]["id"].ToString();
            Session["empID"] = _dtUser.Rows[0]["empID"].ToString();
            Session["upw"] = _dtUser.Rows[0]["password"].ToString();
            Session["uname"] = _dtUser.Rows[0]["username"].ToString();
            Session["utype"] = _dtUser.Rows[0]["type"].ToString();
            Response.Redirect("~/Dashboard.aspx");
        }
        else
        {
            lblMsg.Text = "Username/Password doesn't exist.";
            lblMsg.Visible = true;
            return;
        }
    
    }
}