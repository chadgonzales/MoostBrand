<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employees.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="icon" href="/bootstrap/favicon.ico">

    <title>Time and Attendance Management System</title>

    <!-- Bootstrap core CSS -->
    <link href="/bootstrap/dist/css/bootstrap.css" rel="stylesheet">

    <!-- custom style core CSS -->
    <link href="/bootstrap/dist/css/styles.css" rel="stylesheet">

    <!--responsive-->
    <link href="/bootstrap/dist/css/screen-mobile-360.css" rel="stylesheet">
    <link href="/bootstrap/dist/css/screen-tablet-605.css" rel="stylesheet">
    <link href="/bootstrap/dist/css/screen-tablet-800.css" rel="stylesheet">
    <link href="/bootstrap/dist/css/screen-desktop-1024.css" rel="stylesheet">


    <!--fonts-->
    <link href='http://fonts.googleapis.com/css?family=Open+Sans' rel='stylesheet' type='text/css'>

    <!-- Custom styles for this template
    <link href="navbar-fixed-top.css" rel="stylesheet"> -->

    <!-- Just for debugging purposes. Don't actually copy these 2 lines! -->
    <!--[if lt IE 9]><script src="/../assets/js/ie8-responsive-file-warning.js"></script><![endif]-->
    <script src="/bootstrap/docs/assets/js/ie-emulation-modes-warning.js"></script>

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <form id="form1" runat="server">

        <!-- Fixed navbar -->
        <header class="navbar navbar-default navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">
                    <a class="navbar-brand" href="Employees.aspx">
                        <div class="logo"></div>
                    </a>
                </div>
                <div class="header-right">
                    <span>admin</span>|<a href="#">Logout</a>
                </div>

                <div class="clearfix"></div>

            </div>
        </header>

        <section class="content-holder">
            <div class="menu-holder">
                <div class="container">
                    <ul>
                        <li>
                            <asp:LinkButton ID="lnkEmployees" runat="server" Text="EMPLOYEES" OnClick="lnkEmployees_Click"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>



            <div class="container">

                <asp:Panel ID="pnlList" runat="server">

                    <div class="row">

                        <div class="col-md-6">
                            <div class="title-holder">
                                <h1>Employees</h1>
                            </div>

                        </div>

                        <div class="col-md-6">
                            <div class="btn-holder">
                                <asp:Button ID="btnNew" runat="server" Text="ADD NEW EMPLOYEES" OnClick="btnNew_Click" class="btn btn-blue" />
                                <asp:Button ID="btnUpload" runat="server" Text="UPLOAD MASTERFILE" OnClick="btnUpload_Click" class="btn btn-blue" />
                            </div>
                        </div>

                    </div>

                    <div class="row">

                        <div class="sort-holder">

                            <div class="col-md-6">
                            </div>

                            <div class="col-md-2">
                                <div class="form-group">
                                    <div class="form-label">Show</div>
                                    <asp:DropDownList ID="drpEntries" runat="server" OnSelectedIndexChanged="drpEntries_SelectedIndexChanged" AutoPostBack="True" class="form-control">
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                            <div class="col-md-3">

                                <div class="form-group">
                                    <div class="form-label">Entries</div>
                                    <asp:TextBox ID="txtSearch" runat="server" class="form-control"></asp:TextBox>
                                </div>

                            </div>
                            <div class="col-md-1">

                                <div class="form-group">
                                    <asp:Button ID="btnSearch" runat="server" Text="SEARCH" OnClick="btnSearch_Click" class="btn btn-blue" />
                                </div>

                            </div>

                        </div>
                    </div>

                    <div class="clearfix"></div>

                    <table class="table margin20">
                        <tr>
                            <th>EMPLOYEE ID</th>
                            <th>FIRST NAME</th>
                            <th>MIDDLE NAME</th>
                            <th>LAST NAME</th>
                            <th>EMAIL</th>
                            <th>&nbsp;</th>
                        </tr>

                        <asp:Repeater ID="rptrEmployees" runat="server" OnItemCommand="rptrEmployees_ItemCommand" OnItemDataBound="rptrEmployees_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblActive" runat="server" Text='<%# Bind("active") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblEmpID" runat="server" Text='<%# Bind("empId") %>'></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblFName" runat="server" Text='<%# Bind("firstName") %>'></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMName" runat="server" Text='<%# Bind("middleName") %>'></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblLName" runat="server" Text='<%# Bind("lastName") %>'></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("email") %>'></asp:Label></td>
                                    <td>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="edit" Text="UPDATE" class="btn btn-blue btn-xs" />
                                        <asp:Button ID="btnDelete" runat="server" CommandName="delete" Text="REMOVE" OnClientClick="return confirm('Are you sure you want to remove this employee?');"   class="btn btn-blue btn-xs"/>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>

                    <div class="clearfix"></div>

                    <div class="row">

                        <div class="col-md-12">
                            <div class="pagination">

                                <asp:Label ID="lblShowing" runat="server"></asp:Label>
                                <asp:Label ID="lblEntries" runat="server" Visible="false"></asp:Label>

                                <div class="btn-group">
                                    <asp:LinkButton ID="lnkFirst" runat="server" TabIndex="0" CssClass="paginate_button btn btn-default" OnClick="lnkFirst_Click">First</asp:LinkButton>
                                    <asp:LinkButton ID="lnkPrevious" runat="server" TabIndex="0" CssClass="paginate_button btn btn-default" OnClick="lnkPrevious_Click">Previous</asp:LinkButton>
                                    <asp:LinkButton ID="lnkPage1" runat="server" TabIndex="0" CssClass="paginate_active btn btn-default" OnClick="lnkPage1_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkPage2" runat="server" TabIndex="0" CssClass="paginate_button btn btn-default" OnClick="lnkPage2_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkPage3" runat="server" TabIndex="0" CssClass="paginate_button btn btn-default" OnClick="lnkPage3_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkPage4" runat="server" TabIndex="0" CssClass="paginate_button btn btn-default" OnClick="lnkPage4_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkNext" runat="server" TabIndex="0" CssClass="paginate_button btn btn-default" OnClick="lnkNext_Click">Next</asp:LinkButton>
                                    <asp:LinkButton ID="lnkLast" runat="server" TabIndex="0" CssClass="paginate_button btn btn-default" OnClick="lnkLast_Click">Last</asp:LinkButton>

                                </div>

                            </div>

                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlAdd" runat="server">

                    <div class="row">

                        <div class="col-md-6">
                            <div class="title-holder">
                                <h1>Employees</h1>
                            </div>

                        </div>

                        <div class="col-md-6">
                            <div class="btn-holder">
                                <asp:Button ID="btnBack" runat="server" Text="BACK" OnClick="btnBack_Click" class="btn btn-blue" />
                            </div>
                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-6">

                            <asp:Label ID="lblErr" runat="server" Visible="false"  class="error-hint"></asp:Label>

                            <div class="form-group">
                                <div class="form-label">Empl ID</div>
                                <asp:Label ID="lblEID" runat="server" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtEmpId" runat="server" class="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <div class="form-label">First Name</div>
                                <asp:TextBox ID="txtFName" runat="server" class="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <div class="form-label">Middle Name</div>
                                <asp:TextBox ID="txtMName" runat="server" class="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <div class="form-label">Last Name</div>
                                <asp:TextBox ID="txtLName" runat="server" class="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <div class="form-label">Email</div>
                                <asp:TextBox ID="txtEmail" runat="server" class="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <div class="form-label">Active</div>
                                <asp:CheckBox ID="chkActive" runat="server" class="" />
                            </div>


                            <div class="form-group">
                                <asp:Button ID="btnSave" runat="server" Text="SAVE" OnClick="btnSave_Click" class="btn btn-blue" />
                            </div>



                        </div>
                    </div>



                </asp:Panel>


            </div>

        </section>

        <div>

            <asp:Panel ID="pnlUpload" runat="server">
                <asp:Button ID="btnBackUpload" runat="server" Text="BACK" OnClick="btnBackUpload_Click" /><br />
                <asp:Label ID="lblUploadErr" runat="server"  class="error-hint" Visible="false"></asp:Label><br />
                <asp:FileUpload ID="flEmployee" runat="server" />&nbsp;<asp:Button ID="btnUploadEmployee" runat="server" Text="UPLOAD" OnClick="btnUploadEmployee_Click" /><br />
                <table>
                    <tr>
                        <td>FILE</td>
                        <td>DATE UPLOADED</td>
                        <td>UPLOADED BY</td>
                    </tr>
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
                </table>
            </asp:Panel>
        </div>

        <!-- Footer -->
        <footer class="">
            <div class="container">
                <div class="footer-line"></div>

                <div class="clearfix"></div>
            </div>

            <div class="footer-bg">
                <div class="container">
                    <div class="left">
                        Copyright &copy; 2015 Jentec - Time and Attendance Management System
                    </div>

                </div>

            </div>
        </footer>

    </form>

    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <script src="/bootstrap/dist/js/bootstrap.min.js"></script>
    <!-- IE10 viewport hack for Surface/desktop Windows 8 bug -->
    <script src="/bootstrap/docs/assets/js/ie10-viewport-bug-workaround.js"></script>


    <script>
        $(function () {

            $(".menu-holder li:nth-child(1)").attr("class", "active");

            $(".btn-group .paginate_active").attr("class", "paginate_active btn btn-default");



        });
    </script>
</body>
</html>
