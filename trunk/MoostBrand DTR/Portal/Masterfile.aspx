<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MasterFile.aspx.cs" Inherits="MasterFiles" %>

<!DOCTYPE html>
<!--[if IE 8]> <html lang="en" class="ie8"> <![endif]-->
<!--[if IE 9]> <html lang="en" class="ie9"> <![endif]-->
<!--[if !IE]><!-->
<html lang="en">
<!--<![endif]-->
<!-- BEGIN HEAD -->
<head>
    <meta charset="utf-8" />
    <title>MOOSTBRAND | TAMS</title>
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

        <!-- Fixed navbar -->
        <!-- BEGIN HEADER -->
        <div class="header navbar navbar-inverse navbar-fixed-top">
            <!-- BEGIN TOP NAVIGATION BAR -->
            <div class="navbar-inner">
                <div class="container-fluid">
                    <!-- BEGIN LOGO -->
                    <a class="brand" href="index.html">
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

                    <li class="start ">
                        <a href="#">
                            <i class="icon-upload"></i>
                            <span class="title">Upload Masterfile</span>
                        </a>
                    </li>
                    <li class="last">
                        <a href="#">
                            <i class="icon-user"></i>
                            <span class="title">Change Password</span>
                        </a>
                    </li>
                </ul>
                <!-- END SIDEBAR MENU -->
            </div>
            <!-- END SIDEBAR -->
            <!-- BEGIN PAGE -->
            <div class="page-content">
                <!-- BEGIN PAGE CONTAINER-->
                <div class="container-fluid">

                    <div class="row-fluid">
                        <div class="span12">

                            <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                            <h3 class="page-title">Upload Masterfile</h3>
                        </div>
                    </div>
                    <!-- END PAGE HEADER-->


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
                                        Entries
                                        <asp:TextBox ID="txtSearch" runat="server" class="m-wrap medium"></asp:TextBox>
                                        <asp:Button ID="btnSearch" runat="server" Text="SEARCH" OnClick="btnSearch_Click" class="btn blue" />

                                    </div>
                                    <div class="span6">
                                        <div style="float: right;">

                                            <asp:Label ID="lblUploadErr" runat="server" class="error-hint"></asp:Label>

                                            <asp:FileUpload ID="flEmployee" runat="server" class="btn" Style="padding: 5px 14px; height: 25px;" />

                                            <asp:Button ID="btnUploadEmployee" runat="server" Text="Upload" OnClick="btnUploadEmployee_Click" class="btn blue" />

                                            <asp:Button ID="btnBackUpload" runat="server" Text="BACK" OnClick="btnBackUpload_Click" class="btn blue" Visible="false" />
                                        </div>
                                    </div>

                                    <table class="table table-striped table-hover">
                                        <thead>
                                            <tr>
                                                <th>File</th>
                                                <th>Date UploadedDATE UPLOADED</th>
                                                <th>Uploaded By</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptrMaster" runat="server" OnItemCommand="rptrMaster_ItemCommand">
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

                                </div>
                            </div>
                            <!-- END SAMPLE TABLE PORTLET-->
                        </div>

                    </div>



                    <!-- END PAGE CONTENT-->
                </div>
                <!-- END PAGE CONTAINER-->
            </div>
            <!-- END PAGE -->
        </div>
        <!-- BEGIN FOOTER -->


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

