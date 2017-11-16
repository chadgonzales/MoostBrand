<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<!--[if IE 8]> <html lang="en" class="ie8"> <![endif]-->
<!--[if IE 9]> <html lang="en" class="ie9"> <![endif]-->
<!--[if !IE]><!-->
<html lang="en">
<!--<![endif]-->
<!-- BEGIN HEAD -->
<head>
    <meta charset="utf-8" />
    <title>MOOSTBRAND TAMS | Login</title>
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
    <!-- END GLOBAL MANDATORY STYLES -->
    <!-- BEGIN PAGE LEVEL STYLES -->
    <link href="metronics/css/pages/login.css" rel="stylesheet" type="text/css" />
    <!-- END PAGE LEVEL STYLES -->
    <link rel="shortcut icon" href="favicon.ico" />
</head>
<!-- END HEAD -->
<!-- BEGIN BODY -->
<body class="login">

    <form id="form1" runat="server">
        
        <!-- BEGIN LOGO -->
        <div class="logo">
            <img src="metronics/img/logo-big.png" alt="" />
        </div>
        <!-- END LOGO -->
        <!-- BEGIN LOGIN -->
        <div class="content">
            <!-- BEGIN LOGIN FORM -->
            <div class="form-vertical login-form">
                <h3 class="form-title">Login to your account</h3>

                <asp:Label ID="lblMsg" runat="server" Visible="false" class="alert alert-error" style="display:block;"></asp:Label>

                <div class="control-group">
                    <!--ie8, ie9 does not support html5 placeholder, so we just show field title for that-->
                    <label class="control-label visible-ie8 visible-ie9">Username</label>
                    <div class="controls">
                        <div class="input-icon left">
                            <i class="icon-user"></i>
                            <asp:TextBox ID="txtUsername" runat="server" class="m-wrap placeholder-no-fix" placeholder="Username"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label visible-ie8 visible-ie9">Password</label>
                    <div class="controls">
                        <div class="input-icon left">
                            <i class="icon-lock"></i>
                            <asp:TextBox ID="txtPassword" runat="server" class="m-wrap placeholder-no-fix" TextMode="Password" placeholder="Password"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="form-actions">

                    <asp:Button ID="btnLogIn" runat="server" Text="Login" OnClick="btnLogIn_Click" class="btn red pull-right" />
                    
                        <%--<i class="m-icon-swapright m-icon-white"></i>--%>
                    
                </div>
            </div>
            <!-- END LOGIN FORM -->
        </div>
        <!-- END LOGIN -->
        <!-- BEGIN COPYRIGHT -->
        <div class="copyright">
            2017 &copy; MOOSTBRAND. HOME DEPOT. TAMS by SDS
        </div>



    </form>

    <!-- END COPYRIGHT -->
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
    <!-- END CORE PLUGINS -->
    <!-- BEGIN PAGE LEVEL PLUGINS -->
    <script src="metronics/plugins/jquery-validation/dist/jquery.validate.min.js" type="text/javascript"></script>
    <!-- END PAGE LEVEL PLUGINS -->
    <!-- BEGIN PAGE LEVEL SCRIPTS -->
    <script src="metronics/scripts/app.js" type="text/javascript"></script>
    <script src="metronics/scripts/login.js" type="text/javascript"></script>
    <!-- END PAGE LEVEL SCRIPTS -->
    <script>
        jQuery(document).ready(function () {
            App.init();
            Login.init();
        });
    </script>
    <!-- END JAVASCRIPTS -->
</body>
<!-- END BODY -->
</html>

