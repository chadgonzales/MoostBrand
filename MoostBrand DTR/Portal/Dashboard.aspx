<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>
<!--[if IE 8]> <html lang="en" class="ie8"> <![endif]-->
<!--[if IE 9]> <html lang="en" class="ie9"> <![endif]-->
<!--[if !IE]><!-->
<html lang="en">
<!--<![endif]-->
<!-- BEGIN HEAD -->
<head>
    <meta charset="utf-8" />
    <title>JENTEC | TAMS</title>
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <meta content="" name="description" />
    <meta content="" name="author" />
    <!-- BEGIN GLOBAL MANDATORY STYLES -->
    <link href="metronics/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="metronics/plugins/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <link href="metronics/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="metronics/css/style-metro.css" rel="stylesheet" type="text/css" />
    <link href="metronics/css/style.css" rel="stylesheet" type="text/css" />
    <link href="metronics/css/style-responsive.css" rel="stylesheet" type="text/css" />
    <link href="metronics/css/themes/default.css" rel="stylesheet" type="text/css" id="style_color" />
    <link href="metronics/plugins/uniform/css/uniform.default.css" rel="stylesheet" type="text/css" />


    <link rel="stylesheet" type="text/css" href="metronics/plugins/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="metronics/plugins/bootstrap-timepicker/compiled/timepicker.css" />
    <link rel="stylesheet" type="text/css" href="metronics/plugins/bootstrap-datetimepicker/css/datetimepicker.css" />


    <!-- END GLOBAL MANDATORY STYLES -->
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<!-- END HEAD -->
<!-- BEGIN BODY -->
<body class="page-header-fixed">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <!-- BEGIN HEADER -->
        <div class="header navbar navbar-inverse navbar-fixed-top">
            <!-- BEGIN TOP NAVIGATION BAR -->
            <div class="navbar-inner">
                <div class="container-fluid">
                    <!-- BEGIN LOGO -->
                    <a href="<%= Page.ResolveUrl("~/Dashboard.aspx") %>">
                        <img src="metronics/img/logo.png" alt="logo" />
                    </a>
                    <!-- END LOGO -->
                    <!-- BEGIN RESPONSIVE MENU TOGGLER -->
                    <a href="javascript:;" class="btn-navbar collapsed" data-toggle="collapse" data-target=".nav-collapse">
                        <img src="metronics/img/menu-toggler.png" alt="" />
                    </a>
                    <!-- END RESPONSIVE MENU TOGGLER -->
                    <!-- BEGIN TOP NAVIGATION MENU -->
                    <ul class="nav pull-right">
                        <!-- BEGIN USER LOGIN DROPDOWN -->
                        <li class="dropdown user">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                                <img alt="" src="metronics/img/avatar1_small.jpg" />
                                <asp:Label ID="lblUsername" runat="server" class="username"></asp:Label>
                                <asp:Label ID="lblUserID" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblUsertype" runat="server" Visible="false"></asp:Label>
                                <i class="icon-angle-down"></i>
                            </a>
                            <ul class="dropdown-menu">
                                <li>
                                    <asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogout_Click"><i class="icon-key"></i>Logout</asp:LinkButton>
                                </li>
                            </ul>
                        </li>
                        <!-- END USER LOGIN DROPDOWN -->
                    </ul>
                    <!-- END TOP NAVIGATION MENU -->
                </div>
            </div>
            <!-- END TOP NAVIGATION BAR -->
        </div>
        <!-- END HEADER -->

        <!-- BEGIN CONTAINER -->
        <div class="page-container row-fluid">
            <!-- BEGIN SIDEBAR -->
            <div class="page-sidebar nav-collapse collapse">
                <!-- BEGIN SIDEBAR MENU -->
                <ul class="page-sidebar-menu">
                    <li>
                        <!-- BEGIN SIDEBAR TOGGLER BUTTON -->
                        <div class="sidebar-toggler hidden-phone"></div>
                        <!-- BEGIN SIDEBAR TOGGLER BUTTON -->
                    </li>

                    <li class="start">
                        <asp:LinkButton ID="lnkUploadMasterFile" runat="server" OnClick="lnkUploadMasterFile_Click">
                        <i class="icon-upload"></i>                         
                        <span class="title">Upload Masterfile</span>
                        </asp:LinkButton>
                    </li>
                    <li class="start">
                        <asp:LinkButton ID="lnkProfile" runat="server" OnClick="lnkProfile_Click">
                        <i class="icon-user"></i>                         
                        <span class="title">Profile</span>
                        </asp:LinkButton>
                    </li>
                    <li class="">
                        <asp:LinkButton ID="lnkUserManagement" runat="server" OnClick="lnkUserManagement_Click">
                        <i class="icon-user"></i>                         
                        <span class="title">Users</span>
                        </asp:LinkButton>
                    </li>
                    <li class="">
                        <asp:LinkButton ID="lnkCreateGroups" runat="server" OnClick="lnkCreateGroups_Click">
                        <i class="icon-plus"></i>                         
                        <span class="title">Create Groups</span>
                        </asp:LinkButton>
                    </li>
                    <li class="">
                        <asp:LinkButton ID="lnkEmpGroupings" runat="server" OnClick="lnkEmpGroupings_Click">
                        <i class="icon-group"></i>                         
                        <span class="title">Employee Groupings</span>
                        </asp:LinkButton>
                    </li>
                    <li class="">
                        <asp:LinkButton ID="lnkOTRequest" runat="server" OnClick="lnkOTRequest_Click">
                        <i class="icon-reorder"></i>                         
                        <span class="title">OT Requests</span>
                        </asp:LinkButton>
                    </li>
                    <%--   <li class="">
                        <asp:LinkButton ID="lnkOTManagement" runat="server" OnClick="lnkOTManagement_Click">
                        <i class="icon-reorder"></i>                         
                        <span class="title">OT Management</span>
                        </asp:LinkButton>
                    </li>--%>
                    <li class="">
                        <asp:LinkButton ID="lnkLeaveType" runat="server" OnClick="lnkLeaveType_Click">
                        <i class="icon-reorder"></i>                         
                        <span class="title">Leave Types</span>
                        </asp:LinkButton>
                    </li>
                    <%--  <li class="">
                        <asp:LinkButton ID="lnkLeaveRequestManagement" runat="server" OnClick="lnkLeaveRequestManagement_Click">
                        <i class="icon-reorder"></i>                         
                        <span class="title">Leave Requests</span>
                        </asp:LinkButton>
                    </li>--%>
                    <li class="">
                        <asp:LinkButton ID="lnkLeaveCredits" runat="server" OnClick="lnkLeaveCredits_Click">
                        <i class="icon-reorder"></i>                         
                        <span class="title">Leave Credits</span>
                        </asp:LinkButton>
                    </li>
                    <li class="">
                        <asp:LinkButton ID="lnkLeaveManagement" runat="server" OnClick="lnkLeaveManagement_Click">
                        <i class="icon-reorder"></i>                         
                        <span class="title">Leave Applications</span>
                        </asp:LinkButton>
                    </li>
                    <li class="">
                        <asp:LinkButton ID="lnkShiftSchedule" runat="server" OnClick="lnkShiftSchedule_Click">
                        <i class="icon-calendar"></i>                         
                        <span class="title">Shift Schedules</span>
                        </asp:LinkButton>
                    </li>
                    <li class="">
                        <asp:LinkButton ID="lnkShiftScheduleTagging" runat="server" OnClick="lnkShiftScheduleTagging_Click">
                        <i class="icon-calendar"></i>                         
                        <span class="title">Employee Shift Schedules</span>
                        </asp:LinkButton>
                    </li>
                    <li class="">
                        <asp:LinkButton ID="lnkChangeShiftScheduleRequests" runat="server" OnClick="lnkChangeShiftScheduleRequests_Click">
                        <i class="icon-calendar"></i>                         
                        <span class="title">Change Schedule Request</span>
                        </asp:LinkButton>
                    </li>
                    <li class="">
                        <asp:LinkButton ID="lnkHolidays" runat="server" OnClick="lnkHolidays_Click">
                        <i class="icon-calendar"></i>                         
                        <span class="title">Holidays</span>
                        </asp:LinkButton>
                    </li>
                    <%--<li class="">--%>
                    <asp:LinkButton ID="lnkUploadDTR" runat="server" OnClick="lnkUploadDTR_Click" Visible="false">
                        <%--<i class="icon-calendar"></i>                         
                        <span class="title">Upload DTR</span>--%>
                    </asp:LinkButton>
                    <%-- </li>--%>
                    <li class="">
                        <asp:LinkButton ID="lnkDTR" runat="server" OnClick="lnkDTR_Click">
                        <i class="icon-calendar"></i>                         
                        <span class="title">Daily Time Record</span>
                        </asp:LinkButton>
                    </li>
                    <%-- <li class="">
                        <asp:LinkButton ID="lnkDailyTimeRecord" runat="server" OnClick="lnkDailyTimeRecord_Click">
                        <i class="icon-calendar"></i>                         
                        <span class="title">Daily Time Record</span>
                        </asp:LinkButton>
                    </li>--%>
                    <li class="">
                        <asp:LinkButton ID="lnkPayrollPeriod" runat="server" OnClick="lnkPayrollPeriod_Click">
                        <i class="icon-calendar"></i>                         
                        <span class="title">Payroll Period</span>
                        </asp:LinkButton>
                    </li>
                    <li class="">
                        <asp:LinkButton ID="lnkLogs" runat="server" OnClick="lnkLogs_Click">
                        <i class="icon-calendar"></i>                         
                        <span class="title">Logs</span>
                        </asp:LinkButton>
                    </li>
                    <%--<li class="">
                        <asp:LinkButton ID="lnkDTRSummary" runat="server" OnClick="lnkDTRSummary_Click">
                        <i class="icon-calendar"></i>                         
                        <span class="title">DTR Summary</span>
                        </asp:LinkButton>
                    </li>--%>
                    <li class="last">
                        <asp:LinkButton ID="lnkChangePassword" runat="server" OnClick="lnkChangePassword_Click">
                        <i class="icon-user"></i>                         
                        <span class="title">Change Password</span>
                        </asp:LinkButton>
                    </li>
                    <li class="last">
                        <asp:LinkButton ID="lnkDTRReport" runat="server" OnClick="lnkDTRReport_Click">
                        <i class="icon-user"></i>                         
                        <span class="title">DTR Report</span>
                        </asp:LinkButton>
                    </li>
                     <li class="last">
                        <asp:LinkButton ID="lnkLeaveReport" runat="server" OnClick="lnkLeaveReport_Click">
                        <i class="icon-user"></i>                         
                        <span class="title">Leave Report</span>
                        </asp:LinkButton>
                    </li>
                </ul>
                <!-- END SIDEBAR MENU -->
            </div>
            <!-- END SIDEBAR -->
            <!-- BEGIN PAGE -->
            <div class="page-content">
                <!-- BEGIN PAGE CONTAINER-->
                <div class="container-fluid">

                    <asp:Panel ID="pnlUsers" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Users</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->
                        <!-- BEGIN PAGE CONTENT-->
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN SAMPLE TABLE PORTLET-->
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div class="caption"><i class="icon-user"></i>Users</div>
                                        <div class="tools">
                                        </div>
                                    </div>
                                    <div class="portlet-body">
                                        <div class="span6">
                                        </div>
                                        <div class="span6">
                                            <div style="float: right;">
                                                <asp:DropDownList ID="ddlUserTypes" runat="server" OnSelectedIndexChanged="ddlUserTypes_SelectedIndexChanged" AutoPostBack="true" class="m-wrap medium"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <table class="table table-striped table-hover">
                                            <thead>
                                                <tr>
                                                    <th>Employee ID</th>
                                                    <th>Last Name</th>
                                                    <th>First Name</th>
                                                    <th>Middle Name</th>
                                                    <th>&nbsp;</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptrUsers" runat="server" OnItemDataBound="rptrUsers_ItemDataBound" OnItemCommand="rptrUsers_ItemCommand">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblLastName" runat="server" Text='<%#Bind("lname") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblFirstName" runat="server" Text='<%#Bind("fname") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblMiddleName" runat="server" Text='<%#Bind("mname") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblUserTypeID" runat="server" Text='<%#Bind("usertypeID") %>' Visible="false"></asp:Label>
                                                                <asp:DropDownList ID="ddlUserType" runat="server" Visible="false" class="m-wrap small"></asp:DropDownList>
                                                                <asp:LinkButton ID="lnkUpdate" runat="server" Text="update usertype" CommandName="Update" class="btn blue mini"></asp:LinkButton>
                                                                <asp:LinkButton ID="lnkCancel" runat="server" Text="cancel" CommandName="Cancel" Visible="false" class="btn red mini"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>

                                    </div>
                                </div>
                                <!-- END SAMPLE TABLE PORTLET-->
                            </div>

                        </div>
                    </asp:Panel>


                    <asp:Panel ID="pnlGroups" runat="server">

                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Groups</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->

                        <asp:Panel ID="pnlNewGroup" runat="server" Visible="false">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-group"></i>Add/Edit Group</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">

                                                <div class="control-group span6">
                                                    <asp:Label ID="lblNewGroupMsg" runat="server" class="alert alert-error"></asp:Label>
                                                </div>

                                                <div class="clearfix"></div>

                                                <div class="control-group">
                                                    <asp:Label ID="lblGroupID" runat="server" Text="0" Visible="false"></asp:Label>
                                                    <label class="control-label">Group Name</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtNewGroup" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-actions">
                                                    <asp:Button ID="btnAddGroup" runat="server" Text="SAVE" OnClick="btnAddGroup_Click" class="btn blue" />
                                                    <asp:Button ID="btnBackGroup" runat="server" Text="BACK" OnClick="btnBackGroup_Click" class="btn red" />
                                                </div>
                                            </div>
                                            <!-- END FORM-->
                                        </div>
                                    </div>
                                    <!-- END SAMPLE FORM PORTLET-->
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnlGroupList" runat="server">
                            <!-- BEGIN PAGE CONTENT-->
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-group"></i>Groups</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:Button ID="btnAddNewGroup" runat="server" Text="Add New Group" OnClick="btnAddNewGroup_Click" class="btn blue" />
                                                </div>
                                            </div>

                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Group ID</th>
                                                        <th>Group Name</th>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrGroups" runat="server" OnItemCommand="rptrGroups_ItemCommand">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblGroupName" runat="server" Text='<%#Bind("name") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="btnUpdate" runat="server" Text="Update" CommandName="Update" class="btn blue mini" />
                                                                    <asp:LinkButton ID="btnRemove" runat="server" Text="Remove" CommandName="Remove"
                                                                        OnClientClick="return confirm('Are you sure you want to remove this group?');" class="btn red mini" Visible="true" />
                                                                    <asp:LinkButton ID="btnViewApprover" runat="server" Text="View Approver" CommandName="View Approver" class="btn blue mini" />
                                                                    <asp:LinkButton ID="btnAddApprover" runat="server" Text="Add Approver" CommandName="Approver" class="btn blue mini" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>

                                        </div>
                                    </div>
                                    <!-- END SAMPLE TABLE PORTLET-->
                                </div>

                            </div>

                        </asp:Panel>
                        <asp:Panel ID="pnlApprover" runat="server" Visible="false">
                            <div aria-hidden="false" aria-labelledby="myModalLabel5" role="dialog" tabindex="-1" class="modal hide fade in" id="myModal5" style="display: block;">
                                <div class="modal-header">
                                    <asp:LinkButton ID="lnkApproverCancel" runat="server" OnClick="btnCancelApprover_Click" class="close"></asp:LinkButton>

                                    <h3 id="myModalLabel5">Approver</h3>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="lblApproverMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                    <asp:Label ID="lblApproverID" runat="server" Text="0" Visible="false"></asp:Label>
                                    <asp:Label ID="lblApproverEmpID" runat="server" Text="0" Visible="false"></asp:Label>
                                    <asp:Label ID="lblApproverGroupID" runat="server" Text="0" Visible="false"></asp:Label>
                                    <br />


                                    <label class="control-label">Approver Name: </label>
                                    <asp:DropDownList ID="ddlApprover" runat="server"></asp:DropDownList>
                                </div>
                                <div class="modal-footer">
                                    <asp:LinkButton ID="btnSaveApprover" runat="server" class="btn blue" OnClick="btnSaveApprover_Click">SAVE</asp:LinkButton>
                                    <asp:LinkButton ID="btnCancelApprover" runat="server" OnClick="btnCancelApprover_Click" class="btn red">CANCEL</asp:LinkButton>
                                </div>
                            </div>

                            <div class="modal-backdrop fade in"></div>
                        </asp:Panel>

                        <asp:Panel ID="pnlApproverList" runat="server" Visible="false">
                            <div aria-hidden="false" aria-labelledby="myModalLabel6" role="dialog" tabindex="-1" class="modal hide fade in" id="myModal6" style="display: block;">
                                <div class="modal-header">
                                    <asp:LinkButton ID="lnkCloseApproverList" runat="server" OnClick="lnkCloseApproverList_Click" class="close"></asp:LinkButton>

                                    <h3 id="myModalLabel6">Approver/s</h3>
                                </div>
                                <div class="modal-body">

                                    <table class="table table-striped table-hover">
                                        <thead>
                                            <tr>
                                                <th>Name</th>
                                                <th>&nbsp;</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptrApprover" runat="server" OnItemCommand="rptrApprover_ItemCommand">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblID" runat="server" Visible="false" Text='<%#Bind("id") %>'></asp:Label>
                                                            <asp:Label ID="lblEmpID" runat="server" Visible="false" Text='<%#Bind("empid") %>'></asp:Label>
                                                            <asp:Label ID="lblGroupID" runat="server" Visible="false" Text='<%#Bind("groupID") %>'></asp:Label>

                                                            <asp:Label ID="lblApproverName" runat="server" Text='<%#Bind("fullname") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" CommandName="Remove" OnClientClick="return confirm('Are you sure you want to remove this approver?');"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="modal-backdrop fade in"></div>

                        </asp:Panel>


                    </asp:Panel>

                    <asp:Panel ID="pnlEmpGroupings" runat="server">

                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Employee Groupings</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->


                        <asp:Panel ID="pnlEmpGroupingsList" runat="server">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-group"></i>Employee Groupings</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                                Group:
                                                    <asp:DropDownList ID="ddlGroupList" runat="server" OnSelectedIndexChanged="ddlGroupList_SelectedIndexChanged" AutoPostBack="true" class="m-wrap medium"></asp:DropDownList>

                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:Button ID="btnAddEmpGroup" runat="server" Text="Add Employee Groupings" OnClick="btnAddEmpGroup_Click" class="btn blue" />

                                                </div>
                                            </div>

                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Employee ID</th>
                                                        <th>Last Name</th>
                                                        <th>First Name</th>
                                                        <th>Middle Name</th>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrEmployeeGroups" runat="server" OnItemCommand="rptrEmployeeGroups_ItemCommand" OnItemDataBound="rptrEmployeeGroups_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLastName" runat="server" Text='<%#Bind("lname") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFirstName" runat="server" Text='<%#Bind("fname") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblMiddleName" runat="server" Text='<%#Bind("mname") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblGroupID" runat="server" Text='<%#Bind("groupID") %>' Visible="false"></asp:Label>
                                                                    <asp:DropDownList ID="ddlEmpGroup" runat="server" Visible="false" class="m-wrap small"></asp:DropDownList>
                                                                    <%--<asp:Label ID="lblGroupName" runat="server" Text='<%#Bind("groupName") %>'></asp:Label>--%>
                                                                    <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update Group" CommandName="Update" class="btn blue mini"></asp:LinkButton>
                                                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel" Visible="false" class="btn red mini"></asp:LinkButton>

                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>

                                        </div>
                                    </div>
                                    <!-- END SAMPLE TABLE PORTLET-->
                                </div>

                            </div>

                        </asp:Panel>

                        <asp:Panel ID="pnlNewGroupings" runat="server" Visible="false">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-group"></i>Add/Edit Group</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">



                                                <div class="span6">
                                                    <div>
                                                        <asp:Label ID="lblNewEmpGroupMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                                        <br />
                                                        Group:
                                                        <asp:DropDownList ID="ddlGroup" runat="server" class="m-wrap medium"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <%-- <div class="span6">
                                                    <div style="float: right;">
                                                       
                                                    </div>
                                                </div>--%>

                                                <div class="clearfix"></div>

                                                <table class="table table-striped table-hover">
                                                    <thead>
                                                        <tr>
                                                            <th>
                                                                <asp:CheckBox ID="chkList" runat="server" OnCheckedChanged="chkList_CheckedChanged" AutoPostBack="true" />
                                                            </th>
                                                            <th>Last Name</th>
                                                            <th>First Name</th>
                                                            <th>Middle Name</th>
                                                            <th>Group Name</th>
                                                            <th>&nbsp;</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="rptrEmployeeList" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chk" runat="server" />
                                                                        <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblLastName" runat="server" Text='<%#Bind("lname") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblFirstName" runat="server" Text='<%#Bind("fname") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblMiddleName" runat="server" Text='<%#Bind("mname") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblGroupName" runat="server" Text='<%#Bind("groupName") %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>

                                                <div class="form-actions">
                                                    <asp:Button ID="btnAddtoGroup" runat="server" Text="ADD TO GROUP" OnClick="btnAddtoGroup_Click" class="btn blue" />
                                                    <asp:Button ID="btnBackEmpGroup" runat="server" Text="BACK" OnClick="btnBackEmpGroup_Click" class="btn red" />
                                                </div>

                                            </div>
                                            <!-- END FORM-->

                                        </div>
                                    </div>
                                    <!-- END SAMPLE FORM PORTLET-->
                                </div>
                            </div>



                        </asp:Panel>
                    </asp:Panel>


                    <asp:Panel ID="pnlLeaveTypes" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Leave Types</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->

                        <asp:Panel ID="pnlLeaveTypeLists" runat="server">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Leave Types</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:LinkButton ID="lnkAddLeaveType" runat="server" Text="Add Leave type" OnClick="lnkAddLeaveType_Click" class="btn blue"></asp:LinkButton>

                                                </div>
                                            </div>

                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Code</th>
                                                        <th>Description</th>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrLeaveType" runat="server" OnItemCommand="rptrLeaveType_ItemCommand">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLeaveTypeID" runat="server" Visible="false" Text='<%#Bind("id") %>'></asp:Label>
                                                                    <asp:Label ID="lblLeaveTypeCode" runat="server" Text='<%#Bind("code") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLeaveTypeDesc" runat="server" Text='<%#Bind("description") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkEditLeaveType" runat="server" CommandName="EditLeaveType" Text="Edit"></asp:LinkButton>

                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>

                                        </div>
                                    </div>
                                    <!-- END SAMPLE TABLE PORTLET-->
                                </div>

                            </div>

                        </asp:Panel>
                        <asp:Panel ID="pnlAddLeaveType" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Add/Edit Leave Type</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">

                                                <div class="control-group span6">
                                                    <asp:Label ID="lblLeaveTypeMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>

                                                </div>

                                                <div class="clearfix"></div>

                                                <div class="control-group">
                                                    <asp:Label ID="lblLeaveTypeID" runat="server" Visible="false" Text="0"></asp:Label>
                                                    <label class="control-label">Code</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtLeaveCode" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:Label ID="Label1" runat="server" Visible="false" Text="0"></asp:Label>
                                                    <label class="control-label">Description</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtLeaveDesc" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-actions">
                                                    <asp:Button ID="btnAddLeaveType" runat="server" Text="Save" OnClick="btnAddLeaveType_Click" class="btn blue" />
                                                    <asp:Button ID="btnbackLeaveType" runat="server" Text="Back" OnClick="btnbackLeaveType_Click" class="btn red" />
                                                </div>
                                            </div>
                                            <!-- END FORM-->
                                        </div>
                                    </div>
                                    <!-- END SAMPLE FORM PORTLET-->
                                </div>
                            </div>

                        </asp:Panel>


                    </asp:Panel>


                    <asp:Panel ID="pnlLeaveApplication" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Leave Applications</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->


                        <asp:Panel ID="pnlLeaveApplicationLists" runat="server">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Leave Applications</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:LinkButton ID="lnkLeaveApplicationAdd" runat="server" Text="Add" OnClick="lnkLeaveApplicationAdd_Click" class="btn blue"></asp:LinkButton>

                                                </div>
                                            </div>
                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Employee ID</th>
                                                        <th>Last Name</th>
                                                        <th>First Name</th>
                                                        <th>Leave</th>
                                                        <th>Date Filed</th>
                                                        <th>Date Granted</th>
                                                        <th>Date Start</th>
                                                        <th>End Date</th>
                                                        <th>No. of Hours</th>
                                                        <%--<th>Credit Paid</th>--%>
                                                        <th>Reason</th>
                                                        <%--<th>Level</th>--%>
                                                        <th>Status</th>
                                                        <%-- <th>Modified By</th>--%>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrLeaveApplication" runat="server" OnItemCommand="rptrLeaveApplication_ItemCommand" OnItemDataBound="rptrLeaveApplication_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLAID" runat="server" Text='<%#Bind("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblLALeaveID" runat="server" Text='<%#Bind("leaveType") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblLAEmpID" runat="server" Text='<%#Bind("empID") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLALastName" runat="server" Text='<%#Bind("LName") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLAFirstName" runat="server" Text='<%#Bind("FName") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLADesc" runat="server" Text='<%#Bind("description") %>'></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblLADateFiled" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "requestedDate", "{0:MM/dd/yyyy}") %>'></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblLADateGranted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "approvedDate", "{0:MM/dd/yyyy}") %>'></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblLADateStart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "dateFrom", "{0:MM/dd/yyyy}") %>'></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblLADateEnd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "dateTo", "{0:MM/dd/yyyy}") %>'></asp:Label></td>
                                                                <td>
                                                                    <asp:Label ID="lblLAFormCredit" runat="server" Text='<%#Bind("NoOfHours") %>'></asp:Label></td>
                                                                <%--<td><asp:Label ID="lblLACreditPaid" runat="server" Text='<%#Bind("creditPaid") %>'></asp:Label></td>--%>
                                                                <td>
                                                                    <asp:Label ID="lblLAReason" runat="server" Text='<%#Bind("leaveReason") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLAStatus" runat="server" Text='<%#Bind("leaveStatus") %>'></asp:Label></td>
                                                                <%--<td><asp:Label ID="lblLAModifiedBy" runat="server" Text='<%#Bind("modifiedBy") %>'></asp:Label></td>--%>
                                                                <td>
                                                                    <%-- <asp:LinkButton ID="lnkLAApprove" runat="server" Text="Approve" CommandName="Approve"></asp:LinkButton>--%>
                                                                    <asp:LinkButton ID="lnkLADeny" runat="server" Text="Deny" CommandName="Deny"></asp:LinkButton>
                                                                    <asp:LinkButton ID="lnkLAUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>
                                                                    <%--<asp:LinkButton ID="lnkLADelete" runat="server" Text="Delete" CommandName="Delete"></asp:LinkButton>--%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>

                                        </div>
                                    </div>
                                    <!-- END SAMPLE TABLE PORTLET-->
                                </div>

                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnlLeaveApplicationAdd" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Add/Edit Leave</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">
                                                <div class="control-group span6">
                                                    <asp:Label ID="lblLeaveApplicationMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                                </div>
                                                <div class="clearfix"></div>

                                                <div class="control-group">
                                                    <asp:Label ID="lblLeaveApplicationID" runat="server" Visible="false" Text="0"></asp:Label>
                                                    <label class="control-label">Employee</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlLAEmployee" runat="server" class="m-wrap medium"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:Label ID="Label4" runat="server" Visible="false" Text="0"></asp:Label>
                                                    <label class="control-label">Leave</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlLALeave" runat="server" class="m-wrap medium"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:Label ID="Label2" runat="server" Visible="false" Text="0"></asp:Label>
                                                    <label class="control-label">Date Start</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtLADateStart" runat="server" class="m-wrap medium date-picker"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:Label ID="Label3" runat="server" Visible="false" Text="0"></asp:Label>
                                                    <label class="control-label">End Date</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtLAEndDate" runat="server" class="m-wrap medium date-picker"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:Label ID="Label5" runat="server" Visible="false" Text="0"></asp:Label>
                                                    <label class="control-label">No. of Hours</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtLANoOfHours" runat="server" Text="0" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <asp:Label ID="Label6" runat="server" Visible="false" Text="0"></asp:Label>
                                                    <label class="control-label">Reason</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtLAReason" runat="server" TextMode="MultiLine" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-actions">
                                                    <asp:Button ID="btnLeaveApplicationAdd" runat="server" Text="Save" OnClick="btnLeaveApplicationAdd_Click" class="btn blue" />
                                                    <asp:Button ID="btnLeaveApplicationBack" runat="server" Text="Back" OnClick="btnLeaveApplicationBack_Click" class="btn red" />
                                                </div>
                                            </div>
                                            <!-- END FORM-->
                                        </div>
                                    </div>
                                    <!-- END SAMPLE FORM PORTLET-->
                                </div>
                            </div>
                        </asp:Panel>


                        <asp:Panel ID="pnlLAApprove" runat="server" Visible="false">

                            <div aria-hidden="false" aria-labelledby="myModalLabel3" role="dialog" tabindex="-1" class="modal hide fade in" id="myModal3" style="display: block;">
                                <div class="modal-header">
                                    <asp:LinkButton ID="lnkLAApproveCancel" runat="server" OnClick="lnkLAApproveCancel_Click" class="close"></asp:LinkButton>

                                    <h3 id="myModalLabel3">Leave Request</h3>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="lblLAApproveID" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblLAApproveMsg" runat="server" Visible="false"></asp:Label>

                                    Deny Leave Requests?<br />
                                    <br />

                                    Reason/s:
                                    <asp:TextBox ID="txtLADenyReasons" runat="server" TextMode="MultiLine"></asp:TextBox>

                                </div>
                                <div class="modal-footer">
                                    <asp:LinkButton ID="lnkLAApproveYes" runat="server" OnClick="lnkLAApproveYes_Click" class="btn blue">Deny</asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton4" runat="server" OnClick="lnkLAApproveCancel_Click" class="btn red">Cancel</asp:LinkButton>
                                </div>
                            </div>

                            <div class="modal-backdrop fade in"></div>

                        </asp:Panel>
                    </asp:Panel>


                    <asp:Panel ID="pnlLeaveCredits" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Leave Credits</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->


                        <asp:Panel ID="pnlLeaveCreditsList" runat="server">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Leave Credits</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:LinkButton ID="lnkAddLeaveCredits" runat="server" Text="Add" OnClick="lnkAddLeaveCredits_Click" class="btn blue"></asp:LinkButton>
                                                </div>
                                            </div>

                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Employee ID</th>
                                                        <th>Last Name</th>
                                                        <th>First Name</th>
                                                        <th>Leave</th>
                                                        <th>Date Start</th>
                                                        <th>End Date</th>
                                                        <th>Leave Credit</th>
                                                        <th>Leave Used</th>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrLeaveCreditsList" runat="server" OnItemCommand="rptrLeaveCreditsList_ItemCommand">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLCID" runat="server" Text='<%#Bind("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblLCEmployeeID" runat="server" Text='<%#Bind("empID") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLCLastName" runat="server" Text='<%#Bind("lname") %>'></asp:Label>
                                                                </td>
                                                                <td></td>
                                                                <td>
                                                                    <asp:Label ID="lblLCFirstName" runat="server" Text='<%#Bind("fname") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLCLeaveID" runat="server" Text='<%#Bind("leaveID") %>'></asp:Label>
                                                                    <asp:Label ID="lblLCLeave" runat="server" Text='<%#Bind("description") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLCDateStart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "dateStart", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLCDateEnd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "dateEnd", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLCLeaveCredit" runat="server" Text='<%#Bind("qty") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLCLeaveUsed" runat="server" Text='<%#Bind("qty") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkLCEdit" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>

                                        </div>
                                    </div>
                                    <!-- END SAMPLE TABLE PORTLET-->
                                </div>

                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlLeaveCreditsAdd" runat="server" Visible="false">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Add/Edit Leave Credits</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">
                                                <div class="control-group span6">
                                                    <asp:Label ID="lblLeaveCreditMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                                </div>
                                                <div class="clearfix"></div>

                                                <div class="control-group">
                                                    <asp:Label ID="lblLeaveCreditID" runat="server" Visible="false" Text="0"></asp:Label>
                                                    <label class="control-label">Employee</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlLCEmployee" runat="server" class="m-wrap medium"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Leave</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlLCLeave" runat="server" class="m-wrap medium"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Date Start</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtLCDateStart" runat="server" class="m-wrap medium date-picker"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">End Date</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtLCEndDate" runat="server" class="m-wrap medium date-picker"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Leave Credit</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtLCLeaveCredit" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-actions">
                                                    <asp:Button ID="btnLeaveCreditAdd" runat="server" Text="Save" OnClick="btnLeaveCreditAdd_Click" class="btn blue" />
                                                    <asp:Button ID="btnLeaveCreditBack" runat="server" Text="Back" OnClick="btnLeaveCreditBack_Click" class="btn red" />
                                                </div>
                                            </div>
                                            <!-- END FORM-->
                                        </div>
                                    </div>
                                    <!-- END SAMPLE FORM PORTLET-->
                                </div>
                            </div>

                        </asp:Panel>
                    </asp:Panel>



                    <asp:Panel ID="pnlChangePassword" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Change Password</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->


                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN SAMPLE FORM PORTLET-->
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div class="caption"><i class="icon-user"></i>Change Password</div>
                                        <div class="tools">
                                        </div>
                                    </div>
                                    <div class="portlet-body form">
                                        <!-- BEGIN FORM-->

                                        <div class="form-horizontal">
                                            <div class="control-group span6">
                                                <asp:Label ID="lblChangePwMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                            </div>
                                            <div class="clearfix"></div>

                                            <div class="control-group">
                                                <asp:Label ID="Label8" runat="server" Visible="false" Text="0"></asp:Label>
                                                <label class="control-label">Current Password</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtCurrentPw" runat="server" TextMode="Password" class="m-wrap medium"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">New Password</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtNewPw" runat="server" TextMode="Password" class="m-wrap medium"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="control-group">
                                                <label class="control-label">Confirm Password</label>
                                                <div class="controls">
                                                    <asp:TextBox ID="txtConfirmPw" runat="server" TextMode="Password" class="m-wrap medium"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-actions">
                                                <asp:Button ID="btnChangePw" runat="server" Text="Change Password" OnClick="btnChangePw_Click" class="btn blue" />
                                                <asp:Button ID="btnChangePwBack" runat="server" Text="Back" OnClick="btnChangePwBack_Click" class="btn red" />
                                            </div>
                                        </div>
                                        <!-- END FORM-->
                                    </div>
                                </div>
                                <!-- END SAMPLE FORM PORTLET-->
                            </div>
                        </div>

                    </asp:Panel>

                    <asp:Panel ID="pnlOTRequest" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">OT Requests</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->
                        <asp:Panel ID="pnlOTRequestList" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>OT Requests</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:LinkButton ID="lnkAddOTRequest" runat="server" Text="Add" OnClick="lnkAddOTRequest_Click" class="btn blue"></asp:LinkButton>
                                                </div>
                                            </div>
                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Employee ID</th>
                                                        <th>Last Name</th>
                                                        <th>First Name</th>
                                                        <th>Date Filed</th>
                                                        <th>Date Granted</th>
                                                        <th>Credit Date</th>
                                                        <th>Time Start</th>
                                                        <th>Time End</th>
                                                        <%--<th>Break Start</th>
                                                        <th>Break End</th>--%>
                                                        <th>Total Hrs</th>
                                                        <th>Reason</th>
                                                        <th>Charge To</th>
                                                        <th>Status</th>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrOTRequests" runat="server" OnItemCommand="rptrOTRequests_ItemCommand" OnItemDataBound="rptrOTRequests_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblEmployeeID" runat="server" Text='<%#Bind("empID") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLastName" runat="server" Text='<%#Bind("lname") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFirstName" runat="server" Text='<%#Bind("fname") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDateFiled" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "requestDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDateGranted" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "dateApproved", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblCreditDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "creditDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTimeStart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "timeStart", "{0:HH:mm}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTimeEnd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "timeEnd", "{0:HH:mm}") %>'></asp:Label>
                                                                    <asp:Label ID="lblBreakStart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "breakStart", "{0: HH:mm}") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblBreakEnd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "breakEnd", "{0:HH:mm}") %>' Visible="false"></asp:Label>
                                                                </td>

                                                                <td>
                                                                    <asp:Label ID="lblTotalHrs" runat="server" Text='<%#Bind("TotalHrs") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblReason" runat="server" Text='<%#Bind("reason") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblChargeTo" runat="server" Text='<%#Bind("ChargeTo") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Bind("otstatus") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <%--<asp:LinkButton ID="lnkApprove" runat="server" Text="Approve" CommandName="Approve"></asp:LinkButton>--%>
                                                                    <asp:LinkButton ID="lnkDeny" runat="server" Text="Deny" CommandName="Deny"></asp:LinkButton>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Update" CommandName="Edit"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                        <asp:Panel ID="pnlOTRequestNew" runat="server">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Add/Edit Overtime Application</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">
                                                <div class="control-group span6">
                                                    <asp:Label ID="lblOTRequestMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                                    <asp:Label ID="lblOTRequestID" runat="server" Visible="false" Text="0"></asp:Label>
                                                </div>
                                                <div class="clearfix"></div>

                                                <div class="control-group">
                                                    <label class="control-label">Employee</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlOTRequestEmployee" runat="server" class="m-wrap medium"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Credit Date</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtCreditDate" runat="server" class="m-wrap medium date-picker"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Time Start</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtTimeStart" runat="server" class="m-wrap medium timepicker-24"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Time End</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtTimeEnd" runat="server" class="m-wrap medium timepicker-24"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtBreakStart" runat="server" class="m-wrap medium timepicker-24" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtBreakEnd" runat="server" class="m-wrap medium timepicker-24" Visible="false"></asp:TextBox>
                                                </div>
                                                <%-- <div class="control-group">
                                                    <label class="control-label">Break Start</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtBreakStart" runat="server" class="m-wrap medium timepicker-24"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Break End</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtBreakEnd" runat="server" class="m-wrap medium timepicker-24"></asp:TextBox>
                                                    </div>
                                                </div>--%>
                                                <div class="control-group">
                                                    <label class="control-label">Total Hrs</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtTotalHrs" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Charge To</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtChargeTo" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Reason</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtReason" runat="server" class="m-wrap medium" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-actions">
                                                    <asp:Button ID="btnOTRequestAdd" runat="server" Text="Save" OnClick="btnOTRequestAdd_Click" class="btn blue" />
                                                    <asp:Button ID="btnOTRequestBack" runat="server" Text="Back" OnClick="btnOTRequestBack_Click" class="btn red" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlApprove" runat="server" Visible="false">

                            <div aria-hidden="false" aria-labelledby="myModalLabel2" role="dialog" tabindex="-1" class="modal hide fade in" id="myModal2" style="display: block;">
                                <div class="modal-header">
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkApproveCancel_Click" class="close"></asp:LinkButton>

                                    <h3 id="myModalLabel2">OT Request</h3>
                                </div>
                                <div class="modal-body">
                                    <asp:Label ID="lblApproveID" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblApproveMsg" runat="server" Visible="false"></asp:Label>

                                    <asp:Label ID="lblApproveText" runat="server">Approve</asp:Label><br />
                                    <br />
                                    OT Requests?<br />
                                    <br />

                                    Reason/s:
                                    <asp:TextBox ID="txtDenyReasons" runat="server" TextMode="MultiLine"></asp:TextBox>

                                </div>
                                <div class="modal-footer">
                                    <asp:LinkButton ID="lnkApproveYes" runat="server" OnClick="lnkApproveYes_Click" class="btn blue">Approve</asp:LinkButton>
                                    <asp:LinkButton ID="lnkApproveCancel" runat="server" OnClick="lnkApproveCancel_Click" class="btn red">Cancel</asp:LinkButton>
                                </div>
                            </div>

                            <div class="modal-backdrop fade in"></div>

                        </asp:Panel>
                    </asp:Panel>

                    <asp:Panel ID="pnlShiftSchedule" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Shift Schedules</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->
                        <asp:Panel ID="pnlShiftScheduleList" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Shift Schedules</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:LinkButton ID="lnkAddShiftSchedule" runat="server" Text="Add" OnClick="lnkAddShiftSchedule_Click" class="btn blue"></asp:LinkButton>
                                                </div>
                                            </div>
                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Code</th>
                                                        <th>Description</th>
                                                        <th>Is Flexible</th>
                                                        <th>Time In (From)</th>
                                                        <th>Time In (To)</th>
                                                        <th>Time Out (From)</th>
                                                        <th>Time Out (To)</th>
                                                        <th>Shift Credit</th>
                                                        <th>Break Start</th>
                                                        <th>Break End</th>
                                                        <th>Break Credit</th>
                                                        <th>Is Force Credit</th>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrShiftSchedules" runat="server" OnItemCommand="rptrShiftSchedules_ItemCommand" OnItemDataBound="rptrShiftSchedules_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblCode" runat="server" Text='<%#Bind("code") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("description") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblIsFlexible" runat="server" Text='<%#Bind("IsFlexible") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTimeInFrom" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TimeInFrom", "{0:HH:mm}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTimeInTo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TimeInTo", "{0:HH:mm}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTimeOutFrom" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TimeOutFrom", "{0:HH:mm}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTimeOutTo" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TimeOutTo", "{0:HH:mm}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblShiftCredit" runat="server" Text='<%#Bind("ShiftCredit") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblBreakStart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "breakStart", "{0:HH:mm}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblBreakEnd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "breakEnd", "{0:HH:mm}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblBreakCredit" runat="server" Text='<%#Bind("BreakCredit") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblIsForceCredit" runat="server" Text='<%#Bind("IsForceCredit") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Update" CommandName="Edit"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                        <asp:Panel ID="pnlShiftScheduleNew" runat="server">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Add/Edit Shift Schedule</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">
                                                <div class="control-group span6">
                                                    <asp:Label ID="lblShiftScheduleMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                                    <asp:Label ID="lblShiftScheduleID" runat="server" Visible="false" Text="0"></asp:Label>
                                                </div>
                                                <div class="clearfix"></div>

                                                <div class="control-group">
                                                    <label class="control-label">Code</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSCode" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Description</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSDesc" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Is Flexible</label>
                                                    <div class="controls">
                                                        <asp:RadioButton ID="rbtnFlexibleYes" class="radio" runat="server" GroupName="flexible" AutoPostBack="true" OnCheckedChanged="rbtnFlexibleYes_CheckedChanged" />
                                                        <asp:RadioButton ID="rbtnFlexibleNo" class="radio" runat="server" GroupName="flexible" Checked="true" AutoPostBack="true" OnCheckedChanged="rbtnFlexibleNo_CheckedChanged" />
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Time In (From)</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSTimeInFrom" runat="server" class="m-wrap medium timepicker-24"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <asp:Panel ID="pnlTimeIn" runat="server" class="control-group" Visible="false">
                                                    <label class="control-label">Time In (To)</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSTimeInTo" runat="server" class="m-wrap medium timepicker-24"></asp:TextBox>
                                                    </div>
                                                </asp:Panel>
                                                <div class="control-group">
                                                    <label class="control-label">Time Out (From)</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSTimeOutFrom" runat="server" class="m-wrap timepicker-24"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <asp:Panel ID="pnlTimeOut" runat="server" class="control-group" Visible="false">
                                                    <label class="control-label">Time Out (To)</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSTimeOutTo" runat="server" class="m-wrap medium timepicker-24"></asp:TextBox>
                                                    </div>
                                                </asp:Panel>

                                                <div class="control-group">
                                                    <label class="control-label">Shift Credit</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSShiftCredit" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Break Start</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSBreakStart" runat="server" class="m-wrap medium timepicker-24"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Break End</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSBreakEnd" runat="server" class="m-wrap medium timepicker-24"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Break Credit</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtSSBreakCredit" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Is Force Credit</label>
                                                    <div class="controls">
                                                        <asp:RadioButton ID="rbtnForceCreditYes" class="radio" runat="server" GroupName="forceCredit" />
                                                        <asp:RadioButton ID="rbtnForceCreditNo" class="radio" runat="server" GroupName="forceCredit" Checked="true" />
                                                    </div>
                                                </div>
                                                <div class="form-actions">
                                                    <asp:Button ID="btnShiftScheduleAdd" runat="server" Text="Save" OnClick="btnShiftScheduleAdd_Click" class="btn blue" />
                                                    <asp:Button ID="btnShiftScheduleBack" runat="server" Text="Back" OnClick="btnShiftScheduleBack_Click" class="btn red" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </asp:Panel>


                    <asp:Panel ID="pnlShiftScheduleTagging" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Employee Shift Schedules</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->
                        <asp:Panel ID="pnlShiftScheduleTaggingList" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Employee Shift Schedules</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                                <div class="control-group">
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlEmployeeShiftScheduleSearch" runat="server" OnSelectedIndexChanged="ddlEmployeeShiftScheduleSearch_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:LinkButton ID="lnkAddShiftScheduleTagging" runat="server" Text="Add" OnClick="lnkAddShiftScheduleTagging_Click" class="btn blue" Visible="false"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkImportShiftSchedule" runat="server" Text="Upload Shift Schedule" OnClick="lnkImportShiftSchedule_Click" class="btn blue"></asp:LinkButton>
                                                </div>
                                            </div>

                                            <div class="row-fluid">
                                                <div class="span12">

                                                    <asp:Calendar ID="clndrEmpShiftSchedule" runat="server" Style="display: none;" OnDayRender="clndrEmpShiftSchedule_DayRender" Visible="false" OnVisibleMonthChanged="clndrEmpShiftSchedule_VisibleMonthChanged">

                                                        <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>

                                                    </asp:Calendar>

                                                    <asp:Calendar ID="clndrShiftSchedule" runat="server" OnDayRender="clndrShiftSchedule_DayRender" Visible="false" OnVisibleMonthChanged="clndrShiftSchedule_VisibleMonthChanged">

                                                        <OtherMonthDayStyle ForeColor="LightGray"></OtherMonthDayStyle>

                                                    </asp:Calendar>
                                                </div>
                                            </div>

                                            <table class="table table-striped table-hover" style="display: none;">
                                                <thead>
                                                    <tr>
                                                        <th>Employee</th>
                                                        <th>Code</th>
                                                        <th>Effectivity Date</th>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrShiftSchedulesTagging" runat="server" OnItemCommand="rptrShiftSchedulesTagging_ItemCommand">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblEmpLName" runat="server" Text='<%#Bind("LName") %>'></asp:Label>,
                                                                    <asp:Label ID="lblEmpFName" runat="server" Text='<%#Bind("FName") %>'></asp:Label>
                                                                    <asp:Label ID="lblEmpMName" runat="server" Text='<%#Bind("MName") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblShiftID" runat="server" Text='<%#Bind("schedID") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblShiftCode" runat="server" Text='<%#Bind("Code") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblEffectivityDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "effectiveDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Update" CommandName="Edit"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                        <asp:Panel ID="pnlShiftScheduleTaggingNew" runat="server">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Add/Edit Employee Shift Schedule</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">
                                                <div class="control-group span6">
                                                    <asp:Label ID="lblShiftScheduleTaggingMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                                    <asp:Label ID="lblShiftScheduleTaggingID" runat="server" Visible="false" Text="0"></asp:Label>
                                                </div>
                                                <div class="clearfix"></div>

                                                <div class="control-group">
                                                    <label class="control-label">Employee</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlEmployeeShiftSchedule" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Shift Schedule Code</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlEmployeeShiftScheduleCode" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Effectivity Date</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtEffectivityDate" runat="server" class="m-wrap medium  date-picker"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-actions">
                                                    <asp:Button ID="btnShiftScheduleTaggingAdd" runat="server" Text="Save" OnClick="btnShiftScheduleTaggingAdd_Click" class="btn blue" />
                                                    <asp:Button ID="btnShiftScheduleTaggingBack" runat="server" Text="Back" OnClick="btnShiftScheduleTaggingBack_Click" class="btn red" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlShiftScheduleImport" runat="server" Visible="false">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Upload shift schedule</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->
                                            <div class="form-horizontal">
                                                <div class="control-group span6">
                                                    <asp:Label ID="lblImportMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                                </div>
                                                <div class="clearfix"></div>
                                            </div>
                                            <asp:FileUpload ID="flImportShiftSchedule" runat="server" />
                                            <asp:LinkButton ID="lnkImportSave" runat="server" Text="Upload Shift Schedule" OnClick="lnkImport_Click" class="btn blue"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkImportCancel" runat="server" Text="Back to List" OnClick="lnkImportCancel_Click" class="btn red"></asp:LinkButton>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                    </asp:Panel>

                    <asp:Panel ID="pnlChangeScheduleRequests" runat="server" Visible="false">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Change Schedule Requests</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->
                        <asp:Panel ID="pnlChangeScheduleRequestsList" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Change Schedule Requests</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:LinkButton ID="lnkAddChangeSchedule" runat="server" Text="Add" OnClick="lnkAddChangeSchedule_Click" class="btn blue"></asp:LinkButton>
                                                </div>
                                            </div>
                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Employee ID</th>
                                                        <th>Credit Date</th>
                                                        <th>Shift Code</th>
                                                        <th>Status</th>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrChangeSchedule" runat="server" OnItemCommand="rptrChangeSchedule_ItemCommand" OnItemDataBound="rptrChangeSchedule_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblEmployeeID" runat="server" Text='<%#Bind("empID") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "creditdate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblShiftID" runat="server" Text='<%#Bind("shiftID") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblShiftCode" runat="server" Text='<%#Bind("Code") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Bind("changestatus") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <%--<asp:LinkButton ID="lnkApprove" runat="server" Text="Approve" CommandName="Approve"></asp:LinkButton>
                                                                    <asp:LinkButton ID="lnkDeny" runat="server" Text="Deny" CommandName="Deny"></asp:LinkButton>--%>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Update" CommandName="Edit"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                        <asp:Panel ID="pnlChangeScheduleRequestsNew" runat="server">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Change Schedule Requests</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">
                                                <div class="control-group span6">
                                                    <asp:Label ID="lblChangeScheduleMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                                    <asp:Label ID="lblChangeScheduleID" runat="server" Visible="false" Text="0"></asp:Label>
                                                </div>
                                                <div class="clearfix"></div>

                                                <div class="control-group">
                                                    <label class="control-label">Employee</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlChangeScheduleEmployee" runat="server" class="m-wrap medium"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Credit Date</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtChangeDate" runat="server" class="m-wrap medium date-picker"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Shift Code</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlShiftCode" runat="server" class="m-wrap medium"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-actions">
                                                    <asp:Button ID="btnSaveChangeSchedule" runat="server" Text="Save" OnClick="btnSaveChangeSchedule_Click" class="btn blue" />
                                                    <asp:Button ID="btnChangeChangeSchedule" runat="server" Text="Back" OnClick="btnChangeChangeSchedule_Click" class="btn red" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </asp:Panel>

                    <asp:Panel ID="pnlLogs" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Logs</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->
                        <asp:Panel ID="pnlLogsView" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Logs</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                                Employee
                                                <asp:DropDownList ID="ddlEmployee" runat="server"></asp:DropDownList>
                                            </div>
                                            <br />
                                            <div class="clearfix"></div>
                                            <div class="span8">
                                                From
                                                <asp:TextBox ID="txtDateFrom" runat="server" class="m-wrap medium date-picker"></asp:TextBox>
                                                To
                                                <asp:TextBox ID="txtDateTo" runat="server" class="m-wrap medium date-picker"></asp:TextBox>
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" class="btn blue" />
                                            </div>
                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Employee ID</th>
                                                        <th>Employee Name</th>
                                                        <th>Date</th>
                                                        <th></th>
                                                        <%--  <th>Time In</th>
                                                        <th>Time Out</th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrLogs" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFullName" runat="server" Text='<%#Bind("FullName") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "scanDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblScanDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "scanDate", "{0:HH:mm}") %>'></asp:Label>
                                                                    <%--<asp:Label ID="lblTimeOut" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DateOut", "{0:HH:mm}") %>'></asp:Label>--%>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                    </asp:Panel>

                    <asp:Panel ID="pnlProfile" runat="server">
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE HEADER-->
                                <%--<div class="row-fluid">
                                    <div class="span12">--%>

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Profile</h3>
                                <%--</div>
                                </div>--%>

                                <!-- END PAGE HEADER-->

                                <!-- BEGIN SAMPLE FORM PORTLET-->
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div class="caption"><i class="icon-group"></i>Profile</div>
                                        <div class="tools">
                                        </div>
                                    </div>
                                    <div class="portlet-body form">
                                        <!-- BEGIN FORM-->

                                        <div class="form-horizontal">

                                            <div class="clearfix"></div>

                                            <div class="control-group">
                                                <label class="control-label">Employee ID</label>
                                                <div class="controls">
                                                    <asp:Label ID="lblUserEmployeeID" runat="server" class="m-wrap medium"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Last Name</label>
                                                <div class="controls">
                                                    <asp:Label ID="lblUserLastName" runat="server" class="m-wrap medium"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">First Name</label>
                                                <div class="controls">
                                                    <asp:Label ID="lblUserFirstName" runat="server" class="m-wrap medium"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Middle Name</label>
                                                <div class="controls">
                                                    <asp:Label ID="lblUserMiddleName" runat="server" class="m-wrap medium"></asp:Label>
                                                </div>
                                            </div>

                                            <div class="control-group">
                                                <label class="control-label">Suffix</label>
                                                <div class="controls">
                                                    <asp:Label ID="lblUserSuffix" runat="server" class="m-wrap medium"></asp:Label>
                                                </div>
                                            </div>

                                        </div>
                                        <!-- END FORM-->
                                    </div>
                                </div>
                                <!-- END SAMPLE FORM PORTLET-->
                            </div>
                        </div>

                    </asp:Panel>

                    <asp:Panel ID="pnlHolidays" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Holidays</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->

                        <asp:Panel ID="pnlHolidaysList" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Holidays</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:LinkButton ID="lnkAddHolidays" runat="server" Text="Add" OnClick="lnkAddHolidays_Click" class="btn blue"></asp:LinkButton>
                                                </div>
                                            </div>

                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Date</th>
                                                        <th>Description</th>
                                                        <th>Holiday Type</th>
                                                        <th>Location/s</th>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrHolidays" runat="server" OnItemCommand="rptrHolidays_ItemCommand">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblHDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "HolidayDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblHDesc" runat="server" Text='<%#Bind("HolidayDesc") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblHType" runat="server" Text='<%#Bind("HolidayType") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblHLocID" runat="server" Text='<%#Bind("HolidayLocation") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblHLocName" runat="server" Text='<%#Bind("LocationName") %>'></asp:Label>

                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Update" CommandName="Edit"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                        <asp:Panel ID="pnlHolidaysNew" runat="server">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Add/Edit Holidays</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">
                                                <div class="control-group span6">
                                                    <asp:Label ID="lblHolidaysMsg" runat="server" Visible="false" class="alert alert-error"></asp:Label>
                                                    <asp:Label ID="lblHolidaysID" runat="server" Visible="false" Text="0"></asp:Label>
                                                </div>
                                                <div class="clearfix"></div>

                                                <div class="control-group">
                                                    <label class="control-label">Date</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtHolidaysDate" runat="server" class="m-wrap medium  date-picker"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Description</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtHolidaysDescription" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Holiday Type</label>
                                                    <div class="controls">
                                                        <asp:RadioButton ID="rbtnHolidaysLegal" class="radio" runat="server" GroupName="holidays" Text="Legal" />
                                                        <asp:RadioButton ID="rbtnHolidaysSpecial" class="radio" runat="server" GroupName="holidays" Checked="true" Text="Special" />
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Location</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlHolidaysLocation" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-actions">
                                                    <asp:Button ID="btnHolidaysAdd" runat="server" Text="Save" OnClick="btnHolidaysAdd_Click" class="btn blue" />
                                                    <asp:Button ID="btnHolidaysBack" runat="server" Text="Back" OnClick="btnHolidaysBack_Click" class="btn red" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                    </asp:Panel>
                    <asp:Panel ID="pnlUploadDTR" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Upload DTR</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->

                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN SAMPLE TABLE PORTLET-->
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div class="caption"><i class="icon-reorder"></i>Upload DTR</div>
                                        <div class="tools">
                                        </div>
                                    </div>
                                </div>
                                <div class="portlet-body">
                                    <div class="span6">
                                        Entries
                                        <asp:TextBox ID="txtSearch" runat="server" class="m-wrap medium"></asp:TextBox>
                                        <asp:Button ID="Button1" runat="server" Text="SEARCH" OnClick="btnSearch_Click" class="btn blue" />

                                    </div>
                                    <div class="span6">
                                        <div style="float: right;">

                                            <asp:Label ID="lblUploadErr" runat="server" class="error-hint"></asp:Label>

                                            <asp:FileUpload ID="flEmployee" runat="server" class="btn" Style="padding: 5px 14px; height: 25px;" />

                                            <asp:Button ID="btnUploadEmployee" runat="server" Text="Upload" OnClick="btnUploadEmployee_Click" class="btn blue" />
                                            <asp:Button ID="btnProcessDTR" runat="server" Text="Process" OnClick="btnProcessDTR_Click" class="btn blue" />

                                            <asp:Button ID="btnBackUpload" runat="server" Text="BACK" OnClick="btnBackUpload_Click" class="btn blue" Visible="false" />
                                        </div>
                                    </div>

                                    <table class="table table-striped table-hover">
                                        <thead>
                                            <tr>
                                                <th>File</th>
                                                <th>Date Uploaded</th>
                                                <th>Uploaded By</th>
                                                <th>Status</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptrMaster" runat="server" OnItemCommand="rptrMaster_ItemCommand" OnItemDataBound="rptrMaster_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnk" runat="server" Text='<%# Bind("origFile") %>'></asp:LinkButton>
                                                            <asp:Label ID="lblFileName" runat="server" Text='<%# Bind("filename") %>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDateUploaded" runat="server" Text='<%# Bind("dateUploaded") %>'></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("username") %>'></asp:Label></td>
                                                        <td>
                                                            <asp:Label ID="lblStatusID" runat="server" Text='<%# Bind("status") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("status") %>'></asp:Label></td>

                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>

                                    <div class="row-fluid">
                                        <div class="span6">
                                            Show
                                        <asp:DropDownList ID="drpEntries" runat="server" OnSelectedIndexChanged="drpEntries_SelectedIndexChanged" AutoPostBack="True" class="m-wrap small">
                                            <asp:ListItem>5</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>30</asp:ListItem>
                                        </asp:DropDownList>
                                        </div>

                                        <div class="span6">

                                            <div style="float: left;">

                                                <asp:Label ID="lblShowing" runat="server"></asp:Label>
                                                <asp:Label ID="lblEntries" runat="server" Visible="false"></asp:Label>
                                            </div>
                                            <div style="float: right;">


                                                <div class="pagination">
                                                    <ul>
                                                        <li>
                                                            <asp:LinkButton ID="lnkFirst" runat="server" TabIndex="0" OnClick="lnkFirst_Click">First</asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkPrevious" runat="server" TabIndex="0" OnClick="lnkPrevious_Click">Previous</asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkPage1" runat="server" TabIndex="0" OnClick="lnkPage1_Click"></asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkPage2" runat="server" TabIndex="0" OnClick="lnkPage2_Click"></asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkPage3" runat="server" TabIndex="0" OnClick="lnkPage3_Click"></asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkPage4" runat="server" TabIndex="0" OnClick="lnkPage4_Click"></asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkNext" runat="server" TabIndex="0" OnClick="lnkNext_Click">Next</asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkLast" runat="server" TabIndex="0" OnClick="lnkLast_Click">Last</asp:LinkButton></li>
                                                    </ul>

                                                </div>

                                            </div>

                                        </div>
                                    </div>


                                    <%--                                    <table class="table table-striped table-hover">

                                        <tbody>
                                            <asp:Repeater ID="rptrDTRUpload" runat="server" OnItemDataBound="rptrDTRUpload_ItemDataBound" Visible="false">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="true"></asp:Label>
                                                            <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>' Visible="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Date", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblShiftID" runat="server" Text='<%#Bind("shiftID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblShift" runat="server" Text='<%#Bind("shift") %>'></asp:Label>
                                                            <asp:Label ID="lblTimeInFrom" runat="server" Text='<%#Bind("TimeInFrom") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblTimeInTo" runat="server" Text='<%#Bind("TimeInTo") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblTimeOutFrom" runat="server" Text='<%#Bind("TimeOutFrom") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblTimeOutTo" runat="server" Text='<%#Bind("TimeOutTo") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblIsFlexible" runat="server" Text='<%#Bind("IsFlexible") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblCreditHours" runat="server" Text='<%#Bind("ShiftCredit") %>' Visible="true"></asp:Label>
                                                            <asp:Label ID="lblBreakCredit" runat="server" Text='<%#Bind("BreakCredit") %>' Visible="false"></asp:Label>


                                                            <asp:Label ID="lblDateIn" runat="server" Text='<%#Bind("Date") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblDateOut" runat="server" Text='<%#Bind("Date") %>' Visible="false"></asp:Label>


                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtIn" runat="server" Text='<%#Bind("in") %>' class="m-wrap medium" OnTextChanged="txtIn_TextChanged1" AutoPostBack="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOut" runat="server" Text='<%#Bind("out") %>' class="m-wrap medium" OnTextChanged="txtOut_TextChanged1" AutoPostBack="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblLeave" runat="server" Text='<%#Bind("Leave") %>'></asp:Label><br />
                                                            <asp:Label ID="lblLeaveDetails" runat="server" Text='<%#Bind("Leave") %>' Visible="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRegHour" runat="server" Text='<%#Bind("RegHour") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblLate" runat="server" Text='<%#Bind("Late") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblUT" runat="server" Text='<%#Bind("UT") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblOT" runat="server" Text='<%#Bind("OT") %>'></asp:Label><br />
                                                            <asp:Label ID="lblOTDetails" runat="server" Text='<%#Bind("OT") %>' Visible="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPOT" runat="server" Text='<%#Bind("POT") %>'></asp:Label>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>--%>
                                </div>
                            </div>
                            <!-- END SAMPLE TABLE PORTLET-->
                        </div>

                    </asp:Panel>


                    <asp:Panel ID="pnlDTR" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Daily Time Record</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->

                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN SAMPLE TABLE PORTLET-->
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div class="caption"><i class="icon-reorder"></i>Daily Time Record</div>
                                        <div class="tools">
                                        </div>
                                    </div>
                                    <div class="portlet-body">
                                        <div class="span6">
                                        </div>
                                        <div class="span6">
                                            <div style="float: right;">
                                                <asp:LinkButton ID="lnkSelectPayrollPeriodDTR" runat="server" OnClick="lnkSelectPayrollPeriodDTR_Click" class="btn blue">No selected payroll period</asp:LinkButton>
                                                <asp:Label ID="lblPayrollPeriodIDDTR" runat="server" Visible="false" Text="0"></asp:Label>
                                                <asp:Label ID="lblPayrollPeriodDTR" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblPayrollPeriodStartDTR" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblPayrollPeriodEndDTR" runat="server" Visible="false"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="row">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:Button ID="btnProcessDTR1DTR" runat="server" Text="Process DTR" Visible="false" class="btn blue" OnClick="btnProcessDTR1DTR_Click" Style="vertical-align: top !Important;" />
                                                    <asp:Button ID="btnSaveDTRDTR" runat="server" Text="Save DTR" Visible="false" class="btn blue" OnClick="btnSaveDTRDTR_Click" Style="vertical-align: top !Important;" />

                                                    <asp:DropDownList ID="ddlDTREmployeeDTR" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlDTREmployeeDTR_SelectedIndexChanged" class="m-wrap medium"></asp:DropDownList>

                                                </div>
                                            </div>
                                        </div>
                                        <table class="table table-striped table-hover">
                                            <thead>
                                                <tr>
                                                    <th>#</th>
                                                    <th>Date</th>
                                                    <th>Shift</th>
                                                    <th>IN</th>
                                                    <th>OUT</th>
                                                    <th>Leave</th>
                                                    <th>Shift Credit Hour</th>
                                                    <th>Reg Hour</th>
                                                    <th>Late</th>
                                                    <th>UT</th>
                                                    <th>OT</th>
                                                    <th>POT</th>
                                                    <th>&nbsp;</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptrDailyTimeRecord" runat="server" OnItemDataBound="rptrDailyTimeRecord_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="true"></asp:Label>
                                                                <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>' Visible="false"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Date", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblShiftID" runat="server" Text='<%#Bind("shiftID") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblShift" runat="server" Text='<%#Bind("shift") %>'></asp:Label>
                                                                <asp:Label ID="lblTimeInFrom" runat="server" Text='<%#Bind("TimeInFrom") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblTimeInTo" runat="server" Text='<%#Bind("TimeInTo") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblTimeOutFrom" runat="server" Text='<%#Bind("TimeOutFrom") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblTimeOutTo" runat="server" Text='<%#Bind("TimeOutTo") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblIsFlexible" runat="server" Text='<%#Bind("IsFlexible") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblBreakCredit" runat="server" Text='<%#Bind("BreakCredit") %>' Visible="false"></asp:Label>

                                                                <asp:Label ID="lblDateInOrig" runat="server" Text='<%#Bind("Date") %>' Visible="false"></asp:Label>

                                                                <asp:Label ID="lblDateIn" runat="server" Text='<%#Bind("Date") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblDateOut" runat="server" Text='<%#Bind("Date") %>' Visible="false"></asp:Label>

                                                                <asp:Label ID="lblIsHoliday" runat="server" Text="" Visible="false"></asp:Label>

                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtIn" runat="server" Text='<%#Bind("in") %>' class="m-wrap medium" OnTextChanged="txtIn_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOut" runat="server" Text='<%#Bind("out") %>' class="m-wrap medium" OnTextChanged="txtOut_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblLeave" runat="server" Text='<%#Bind("Leave") %>'></asp:Label><br />
                                                                <asp:Label ID="lblLeaveDetails" runat="server" Text='<%#Bind("Leave") %>' Visible="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCreditHours" runat="server" Text='<%#Bind("ShiftCredit") %>' Visible="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblRegHour" runat="server" Text='<%#Bind("RegHour") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblLate" runat="server" Text='<%#Bind("Late") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblUT" runat="server" Text='<%#Bind("UT") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblOT" runat="server" Text='<%#Bind("OT") %>'></asp:Label><br />
                                                                <asp:Label ID="lblOTDetails" runat="server" Text='<%#Bind("OT") %>' Visible="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPOT" runat="server" Text='<%#Bind("POT") %>'></asp:Label>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>


                    <%--       <asp:Panel ID="pnlDailyTimeRecord" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Daily Time Record</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->

                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN SAMPLE TABLE PORTLET-->
                                <div class="portlet box blue">
                                    <div class="portlet-title">
                                        <div class="caption"><i class="icon-reorder"></i>Daily Time Record</div>
                                        <div class="tools">
                                        </div>
                                    </div>
                                    <div class="portlet-body">
                                        <div class="span6">
                                        </div>
                                        <div class="span6">
                                            <div style="float: right;">
                                                <asp:LinkButton ID="lnkSelectPayrollPeriod" runat="server" OnClick="lnkSelectPayrollPeriod_Click" class="btn blue">No selected payroll period</asp:LinkButton>
                                                <asp:Label ID="lblPayrollPeriodID" runat="server" Visible="false" Text="0"></asp:Label>
                                                <asp:Label ID="lblPayrollPeriod" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblPayrollPeriodStart" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblPayrollPeriodEnd" runat="server" Visible="false"></asp:Label>

                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <%--<div class="span3">
                                            <div style="float: right;">
                                                <asp:Button ID="btnPostDTR" runat="server" Text="Post DTR" Visible="false" class="btn blue" />
                                            </div>
                                        </div>--%>
                    <%--<div class="span3">
                                            <div style="float: right;">
                                                </div>
                                        </div>-
                                        <div class="row">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:Button ID="btnProcessDTR1" runat="server" Text="Process DTR" Visible="true" class="btn blue" OnClick="btnProcessDTR1_Click" Style="vertical-align: top !Important;" />
                                                    <asp:Button ID="btnSaveDTR" runat="server" Text="Save DTR" Visible="false" class="btn blue" OnClick="btnSaveDTR_Click" Style="vertical-align: top !Important;" />

                                                    <asp:DropDownList ID="ddlDTREmployee" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlDTREmployee_SelectedIndexChanged" class="m-wrap medium"></asp:DropDownList>

                                                </div>
                                            </div>
                                        </div>
                                        <table class="table table-striped table-hover">
                                            <thead>
                                                <tr>
                                                    <th>#</th>
                                                    <th>Date</th>
                                                    <th>Shift</th>
                                                    <th>IN</th>
                                                    <th>OUT</th>
                                                    <th>Leave</th>
                                                    <th>Shift Credit Hour</th>
                                                    <th>Reg Hour</th>
                                                    <th>Late</th>
                                                    <th>UT</th>
                                                    <th>OT</th>
                                                    <th>POT</th>
                                                    <th>&nbsp;</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptrDTR" runat="server" OnItemDataBound="rptrDTR_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="true"></asp:Label>
                                                                <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>' Visible="false"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Date", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblShiftID" runat="server" Text='<%#Bind("shiftID") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblShift" runat="server" Text='<%#Bind("shift") %>'></asp:Label>
                                                                <asp:Label ID="lblTimeInFrom" runat="server" Text='<%#Bind("TimeInFrom") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblTimeInTo" runat="server" Text='<%#Bind("TimeInTo") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblTimeOutFrom" runat="server" Text='<%#Bind("TimeOutFrom") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblTimeOutTo" runat="server" Text='<%#Bind("TimeOutTo") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblIsFlexible" runat="server" Text='<%#Bind("IsFlexible") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblBreakCredit" runat="server" Text='<%#Bind("BreakCredit") %>' Visible="false"></asp:Label>


                                                                <asp:Label ID="lblDateIn" runat="server" Text='<%#Bind("Date") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblDateOut" runat="server" Text='<%#Bind("Date") %>' Visible="false"></asp:Label>


                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtIn" runat="server" Text='<%#Bind("in") %>' class="m-wrap medium" OnTextChanged="txtIn_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOut" runat="server" Text='<%#Bind("out") %>' class="m-wrap medium" OnTextChanged="txtOut_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblLeave" runat="server" Text='<%#Bind("Leave") %>'></asp:Label><br />
                                                                <asp:Label ID="lblLeaveDetails" runat="server" Text='<%#Bind("Leave") %>' Visible="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCreditHours" runat="server" Text='<%#Bind("ShiftCredit") %>' Visible="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblRegHour" runat="server" Text='<%#Bind("RegHour") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblLate" runat="server" Text='<%#Bind("Late") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblUT" runat="server" Text='<%#Bind("UT") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblOT" runat="server" Text='<%#Bind("OT") %>'></asp:Label><br />
                                                                <asp:Label ID="lblOTDetails" runat="server" Text='<%#Bind("OT") %>' Visible="true"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPOT" runat="server" Text='<%#Bind("POT") %>'></asp:Label>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>--%>

                    <asp:Panel ID="pnlSelectPayrollPeriod" runat="server" Visible="false">
                        <div aria-hidden="false" aria-labelledby="myModalLabel4" role="dialog" tabindex="-1" class="modal hide fade in" id="myModal4" style="display: block;">
                            <div class="modal-header">
                                <asp:LinkButton ID="lnkPayrollPeriodCancel" runat="server" OnClick="lnkPayrollPeriodCancel_Click" class="close"></asp:LinkButton>

                                <h3 id="myModalLabel4">Payroll Period</h3>
                            </div>
                            <div class="modal-body">
                                <table class="table table-striped table-hover">
                                    <thead>
                                        <tr>
                                            <th></th>
                                            <th>Year</th>
                                            <th>Month</th>
                                            <th>Payroll Description</th>
                                            <th>&nbsp;</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptrSelectPayrollPeriod" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chk" runat="server" OnCheckedChanged="chk_CheckedChanged" AutoPostBack="true" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblID" runat="server" Text='<%#Bind("ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblYear" runat="server" Text='<%#Bind("Year") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblMonth" runat="server" Text='<%#Bind("Month") %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("payrollDesc") %>'></asp:Label>
                                                        <asp:Label ID="lblPPStart" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PayrollStart", "{0:MMM dd, yyyy}") %>'></asp:Label>-
                                                        <asp:Label ID="lblPPEnd" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "PayrollEnd", "{0:MMM dd, yyyy}") %>'></asp:Label>
                                                    </td>

                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div class="modal-backdrop fade in"></div>

                    </asp:Panel>

                    <asp:Panel ID="pnlPayrollPeriod" runat="server">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">
                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Payroll Period</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->
                        <asp:Panel ID="pnlPayrollPeriodList" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Payroll Period</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                            </div>
                                            <div class="span6">
                                                <div style="float: right;">
                                                    <asp:LinkButton ID="lnkAddPayrollPeriod" runat="server" Text="Add" OnClick="lnkAddPayrollPeriod_Click" class="btn blue"></asp:LinkButton>
                                                </div>
                                            </div>

                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Description</th>
                                                        <th>Year</th>
                                                        <th>Month</th>
                                                        <th>Payroll Period</th>
                                                        <th>Is Active</th>
                                                        <th>&nbsp;</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrPayrollPeriod" runat="server" OnItemCommand="rptrPayrollPeriod_ItemCommand" OnItemDataBound="rptrPayrollPeriod_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblPPDesc" runat="server" Text='<%#Bind("payrollDesc") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPPYear" runat="server" Text='<%#Bind("year") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPPMonth" runat="server" Text='<%#Bind("month") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPPPeriodID" runat="server" Text='<%#Bind("payrollPeriod") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblPPPeriod" runat="server" Text='<%#Bind("payrollPeriod") %>'></asp:Label>
                                                                    :
                                                                    <asp:Label ID="lblPPStart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PayrollStart", "{0:MM/dd/yyyy}") %>'></asp:Label>-
                                                                    <asp:Label ID="lblPPEnd" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PayrollEnd", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblIsActive" runat="server" Text='<%#Bind("isActive") %>' Visible="false"></asp:Label>
                                                                    <asp:CheckBox ID="chkIsActive" runat="server" Enabled="false"></asp:CheckBox>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Update" CommandName="Edit"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>
                        <asp:Panel ID="pnlPayrollPeriodNew" runat="server">

                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE FORM PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Add/Edit Holidays</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body form">
                                            <!-- BEGIN FORM-->

                                            <div class="form-horizontal">
                                                <div class="control-group span6">
                                                    <asp:Label ID="lblPPMsg" runat="server" class="alert alert-error"></asp:Label>
                                                    <asp:Label ID="lblPPID" runat="server" Visible="false" Text="0"></asp:Label>
                                                </div>
                                                <div class="clearfix"></div>

                                                <div class="control-group">
                                                    <label class="control-label">Description</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtPPDesc" runat="server" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Year</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlPPYear" runat="server" OnSelectedIndexChanged="ddlPPYear_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Month</label>
                                                    <div class="controls">
                                                        <asp:DropDownList ID="ddlPPMonth" runat="server" OnSelectedIndexChanged="ddlPPMonth_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Text="Select Month" Value="" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="January" Value="January"></asp:ListItem>
                                                            <asp:ListItem Text="February" Value="February"></asp:ListItem>
                                                            <asp:ListItem Text="March" Value="March"></asp:ListItem>
                                                            <asp:ListItem Text="April" Value="April"></asp:ListItem>
                                                            <asp:ListItem Text="May" Value="May"></asp:ListItem>
                                                            <asp:ListItem Text="June" Value="June"></asp:ListItem>
                                                            <asp:ListItem Text="July" Value="July"></asp:ListItem>
                                                            <asp:ListItem Text="August" Value="August"></asp:ListItem>
                                                            <asp:ListItem Text="September" Value="September"></asp:ListItem>
                                                            <asp:ListItem Text="October" Value="October"></asp:ListItem>
                                                            <asp:ListItem Text="November" Value="November"></asp:ListItem>
                                                            <asp:ListItem Text="December" Value="December"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Payroll Period</label>
                                                    <div class="controls">
                                                        <asp:RadioButton ID="rbtnFirstHalf" runat="server" Text="First half" GroupName="payroll" class="radio" Checked="true" OnCheckedChanged="rbtnFirstHalf_CheckedChanged" AutoPostBack="true" />
                                                        <asp:RadioButton ID="rbtnSecondHalf" runat="server" Text="Second half" GroupName="payroll" class="radio" OnCheckedChanged="rbtnSecondHalf_CheckedChanged" AutoPostBack="true" />
                                                    </div>
                                                </div>


                                                <div class="control-group">
                                                    <label class="control-label">Payroll Start</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtPPStart" runat="server" Enabled="false" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="control-group">
                                                    <label class="control-label">Payroll End</label>
                                                    <div class="controls">
                                                        <asp:TextBox ID="txtPPEnd" runat="server" Enabled="false" class="m-wrap medium"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="control-group">
                                                    <label class="control-label">Is Active</label>
                                                    <div class="controls">
                                                        <asp:CheckBox ID="chkActive" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="form-actions">
                                                    <asp:Button ID="btnPayrollPeriodSave" runat="server" Text="Save" OnClick="btnPayrollPeriodSave_Click" class="btn blue" />
                                                    <asp:Button ID="btnPayrollPeriodBack" runat="server" Text="Back" OnClick="btnPayrollPeriodBack_Click" class="btn red" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                    </asp:Panel>

                    <asp:Panel ID="pnlDTRReport" runat="server" Visible="false">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">DTR Report</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->
                        <asp:Panel ID="Panel2" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>DTR Report</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                                <asp:Label ID="lblDTRReportMsg" runat="server" Text="" Visible="false"></asp:Label>
                                            </div>
                                            <br />
                                            <div class="clearfix"></div>
                                            <div class="span4">
                                                Employee
                                                <asp:DropDownList ID="ddlEmployeeDTR" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="span4">
                                                Payroll Period
                                                <asp:DropDownList ID="ddlPayrollPeriod" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="span3" style="margin: 0px;">
                                                <asp:Button ID="btnSearchDTR" runat="server" Text="Search" OnClick="btnSearchDTR_Click" class="btn blue" />
                                                <asp:Button ID="btnGenerateReportDTR" Visible="false" runat="server" Text="Generate Report" OnClick="btnGenerateReportDTR_Click" class="btn blue" />
                                            </div>
                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Employee ID</th>
                                                        <th>Name</th>
                                                        <%-- <th>Shift</th>--%>

                                                        <th>Payroll Days</th>
                                                        <th>Absences (days)</th>
                                                        <th>Leaves (hrs)</th>
                                                        <th>Holidays</th>

                                                        <th>Late (min)</th>
                                                        <th>Undertime (min)</th>

                                                        <th>Reg+NP (hrs)</th>
                                                        <th>RegOT (hrs)</th>
                                                        <th>RegOT+NP (hrs)</th>
                                                        <th>RegOTEx (hrs)</th>
                                                        <th>RegOTEx+NP (hrs)</th>


                                                        <th>LegOT (hrs)</th>
                                                        <th>LegOT+NP (hrs)</th>
                                                        <th>LegOTEx (hrs)</th>
                                                        <th>LegOTEx+NP (hrs)</th>

                                                        <th>SpOT (hrs)</th>
                                                        <th>SpOT+NP (hrs)</th>
                                                        <th>SpOTEx (hrs)</th>
                                                        <th>SpOTEx+NP (hrs)</th>


                                                        <th>RstOT (hrs)</th>
                                                        <th>RstOT+NP (hrs)</th>
                                                        <th>RstOTEx (hrs)</th>
                                                        <th>RstOTEx+NP (hrs)</th>

                                                        <th>LegRstOT (hrs)</th>
                                                        <th>LegRstOT+NP (hrs)</th>
                                                        <th>LegRstOTEx (hrs)</th>
                                                        <th>LegRstOTEx+NP (hrs)</th>

                                                        <th>SpRstOT (hrs)</th>
                                                        <th>SpRstOT+NP (hrs)</th>
                                                        <th>SpRstOTEx (hrs)</th>
                                                        <th>SpRstOTEx+NP (hrs)</th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrDTRReport" runat="server" OnItemDataBound="rptrDTRReport_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFullName" runat="server" Text='<%#Bind("FullName") %>'></asp:Label>

                                                                    <asp:Label ID="lblShift" runat="server" Text="" Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblTimeIn" runat="server" Text="" Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblTimeOut" runat="server" Text="" Visible="false"></asp:Label>
                                                                </td>

                                                                <td>
                                                                    <asp:Label ID="lblPayrollDays" runat="server" Text="15"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblAbsences" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLeaves" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblHolidays" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLate" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblUndertime" runat="server" Text=""></asp:Label>
                                                                </td>



                                                                <td>
                                                                    <asp:Label ID="lblRegNP" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRegOT" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRegOTNP" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRegOTEx" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRegOTExNP" runat="server" Text=""></asp:Label>
                                                                </td>


                                                                <td>
                                                                    <asp:Label ID="lblLegOT" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLegOTNP" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLegOTEx" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLegOTExNP" runat="server" Text=""></asp:Label>
                                                                </td>


                                                                <td>
                                                                    <asp:Label ID="lblSpOT" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSpOTNP" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSpOTEx" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSpOTExNP" runat="server" Text=""></asp:Label>
                                                                </td>



                                                                <td>
                                                                    <asp:Label ID="lblRstOT" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRstOTNP" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRstOTEx" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblRstOTExNP" runat="server" Text=""></asp:Label>
                                                                </td>



                                                                <td>
                                                                    <asp:Label ID="lblLegRstOT" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLegRstOTNP" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLegRstOTEx" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLegRstOTExNP" runat="server" Text=""></asp:Label>
                                                                </td>




                                                                <td>
                                                                    <asp:Label ID="lblSpRstOT" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSpRstOTNP" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSpRstOTEx" runat="server" Text=""></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSpRstOTExNP" runat="server" Text=""></asp:Label>
                                                                </td>

                                                                <%--<td>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Date", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTimeIn" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DateIn", "{0:HH:mm}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTimeOut" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DateOut", "{0:HH:mm}") %>'></asp:Label>
                                                                </td>--%>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>

                                            <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>--%>
                                        </div>
                                    </div>
                                </div>
                                </div>
                        </asp:Panel>
                    </asp:Panel>

                    <asp:Panel ID="pnlRepeater" runat="server" Visible="false">
                        repeater
                                    <table class="table table-striped table-hover">

                                        <tbody>
                                            <asp:Repeater ID="rptrDTRUpload" runat="server" OnItemDataBound="rptrDTRUpload_ItemDataBound" Visible="true">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblID" runat="server" Text='<%#Bind("id") %>' Visible="true"></asp:Label>
                                                            <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>' Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Date", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblShiftID" runat="server" Text='<%#Bind("shiftID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblShift" runat="server" Text='<%#Bind("shift") %>'></asp:Label>
                                                            <asp:Label ID="lblTimeInFrom" runat="server" Text='<%#Bind("TimeInFrom") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblTimeInTo" runat="server" Text='<%#Bind("TimeInTo") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblTimeOutFrom" runat="server" Text='<%#Bind("TimeOutFrom") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblTimeOutTo" runat="server" Text='<%#Bind("TimeOutTo") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblIsFlexible" runat="server" Text='<%#Bind("IsFlexible") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblBreakCredit" runat="server" Text='<%#Bind("BreakCredit") %>' Visible="false"></asp:Label>

                                                            <asp:Label ID="lblDateInOrig" runat="server" Text='<%#Bind("Date") %>' Visible="false"></asp:Label>

                                                            <asp:Label ID="lblDateIn" runat="server" Text='<%#Bind("Date") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblDateOut" runat="server" Text='<%#Bind("Date") %>' Visible="false"></asp:Label>

                                                            <asp:Label ID="lblIsHoliday" runat="server" Text="" Visible="false"></asp:Label>

                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtIn" runat="server" Text='<%#Bind("in") %>' class="m-wrap medium" OnTextChanged="txtIn_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtOut" runat="server" Text='<%#Bind("out") %>' class="m-wrap medium" OnTextChanged="txtOut_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblLeave" runat="server" Text='<%#Bind("Leave") %>'></asp:Label><br />
                                                            <asp:Label ID="lblLeaveDetails" runat="server" Text='<%#Bind("Leave") %>' Visible="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblCreditHours" runat="server" Text='<%#Bind("ShiftCredit") %>' Visible="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblRegHour" runat="server" Text='<%#Bind("RegHour") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblLate" runat="server" Text='<%#Bind("Late") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblUT" runat="server" Text='<%#Bind("UT") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblOT" runat="server" Text='<%#Bind("OT") %>'></asp:Label><br />
                                                            <asp:Label ID="lblOTDetails" runat="server" Text='<%#Bind("OT") %>' Visible="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblPOT" runat="server" Text='<%#Bind("POT") %>'></asp:Label>
                                                        </td>
                                                        <td></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                    </asp:Panel>

                     <asp:Panel ID="pnlLeaveReport" runat="server" Visible="false">
                        <!-- BEGIN PAGE HEADER-->
                        <div class="row-fluid">
                            <div class="span12">

                                <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                                <h3 class="page-title">Leave Report</h3>
                            </div>
                        </div>
                        <!-- END PAGE HEADER-->

                             <asp:Panel ID="Panel1" runat="server">
                            <div class="row-fluid">
                                <div class="span12">
                                    <!-- BEGIN SAMPLE TABLE PORTLET-->
                                    <div class="portlet box blue">
                                        <div class="portlet-title">
                                            <div class="caption"><i class="icon-reorder"></i>Leave Report</div>
                                            <div class="tools">
                                            </div>
                                        </div>
                                        <div class="portlet-body">
                                            <div class="span6">
                                                <asp:Label ID="lblLeaveReportMsg" runat="server" Text="" Visible="false"></asp:Label>
                                            </div>
                                            <br />
                                            <div class="clearfix"></div>
                                            <div class="span4">
                                                Employee
                                                <asp:DropDownList ID="ddlEmployeeLeave" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="span4">
                                                Leave Type
                                                <asp:DropDownList ID="ddlLeaveType" runat="server"></asp:DropDownList>
                                            </div>
                                             <div class="span4">
                                                Date From
                                                <asp:TextBox ID="txtLeaveDateFrom"  runat="server" class="m-wrap medium date-picker"></asp:TextBox>
                                            </div>
                                             <div class="span4">
                                               Date To
                                                <asp:TextBox ID="txtLeaveDateTo" runat="server" class="m-wrap medium date-picker"></asp:TextBox>
                                            </div>
                                            <div class="span3" style="margin: 0px;">
                                                <asp:Button ID="btnSearchLeaveReport" runat="server" Text="Search" OnClick="btnSearchLeaveReport_Click" class="btn blue" />
                                                <asp:Button ID="Button3" Visible="false" runat="server" Text="Generate Report" OnClick="btnGenerateReportDTR_Click" class="btn blue" />
                                            </div>
                                            <table class="table table-striped table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Employee ID</th>
                                                        <th>Name</th>
                                                        <th>Leave Type</th>
                                                        <th>No. of hours</th>
                                                        <th>Reason</th>
                                                        <th>Credit Date</th>
                                                        <th>Status</th>
                                                    <%--    <th>Date Requested</th>--%>
                                                        <th>Date Approved/Denied</th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rptrLeaveReport" runat="server" OnItemDataBound="rptrLeaveReport_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEmpID" runat="server" Text='<%#Bind("empID") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblFullName" runat="server" Text='<%#Bind("FName") %>'></asp:Label> <asp:Label ID="Label7" runat="server" Text='<%#Bind("LName") %>'></asp:Label>
                                                                </td>
                                                                 <td>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Bind("description") %>'></asp:Label>
                                                                </td>
                                                                 <td>
                                                                    <asp:Label ID="lblNoOfHours" runat="server" Text='<%#Bind("NoOfHours") %>'></asp:Label>
                                                                </td>
                                                                 <td>
                                                                    <asp:Label ID="lblReason" runat="server" Text='<%#Bind("leaveReason") %>'></asp:Label>
                                                                </td>
                                                                 <td>
                                                                    <asp:Label ID="lblCreditDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DateFrom", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Bind("leaveStatus") %>'></asp:Label>
                                                                </td>
                                                                   <%--<td>
                                                                    <asp:Label ID="lblRequestedDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "requestedDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>--%>
                                                                <td>
                                                                    <asp:Label ID="lblApproveDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "approvedDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                    <asp:Label ID="lblDeniedDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "deniedDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>

                                            <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>--%>
                                        </div>
                                    </div>
                                </div>
                                </div>
                        </asp:Panel>



                         </asp:Panel>





                    <!-- END PAGE CONTENT-->
                </div>
                <!-- END PAGE CONTAINER-->
            </div>
            <!-- END PAGE -->
        </div>
        <!-- BEGIN FOOTER -->
        <div class="footer">
            <div class="footer-inner">
                2015 &copy; JENTEC STORAGE INC. TAMS by SDS
            </div>
            <div class="footer-tools">
                <span class="go-top">
                    <i class="icon-angle-up"></i>
                </span>
            </div>
        </div>
        <!-- END FOOTER -->



    </form>
    <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
    <!-- BEGIN CORE PLUGINS -->
    <script src="metronics/plugins/jquery-1.10.1.min.js" type="text/javascript"></script>
    <script src="metronics/plugins/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>
    <!-- IMPORTANT! Load jquery-ui-1.10.1.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip -->
    <script src="metronics/plugins/jquery-ui/jquery-ui-1.10.1.custom.min.js" type="text/javascript"></script>
    <script src="metronics/plugins/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <!--[if lt IE 9]>
	<script src="metronics/plugins/excanvas.min.js"></script>
	<script src="metronics/plugins/respond.min.js"></script>  
	<![endif]-->
    <script src="metronics/plugins/jquery-slimscroll/jquery.slimscroll.min.js" type="text/javascript"></script>
    <script src="metronics/plugins/jquery.blockui.min.js" type="text/javascript"></script>
    <script src="metronics/plugins/jquery.cookie.min.js" type="text/javascript"></script>
    <script src="metronics/plugins/uniform/jquery.uniform.min.js" type="text/javascript"></script>


    <script type="text/javascript" src="metronics/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="metronics/plugins/bootstrap-timepicker/js/bootstrap-timepicker.js"></script>
    <script type="text/javascript" src="metronics/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js"></script>
    <!-- END CORE PLUGINS -->
    <script src="metronics/scripts/app.js"></script>
    <script src="metronics/scripts/form-components.js"></script>
    <script>
        jQuery(document).ready(function () {
            App.init();
            FormComponents.init();


            $("input[type = 'submit'].aspNetDisabled").attr('class', 'aspNetDisabled btn disabled');
        });
    </script>
    <!-- END JAVASCRIPTS -->
</body>
<!-- END BODY -->
</html>
