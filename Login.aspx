<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LMS.Login" UnobtrusiveValidationMode="None" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Admiro admin is super flexible, powerful, clean &amp; modern responsive bootstrap 5 admin template with unlimited possibilities.">
    <meta name="keywords" content="admin template, Admiro admin template, best javascript admin, dashboard template, bootstrap admin template, responsive admin template, web app">
    <meta name="author" content="pixelstrap">
    <title>Lms-Login</title>
    <!-- Favicon icon-->
    <link rel="icon" href="assets/images/favicon.png" type="image/x-icon">
    <link rel="shortcut icon" href="assets/images/favicon.png" type="image/x-icon">
    <!-- Google font-->
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="">
    <link href="https://fonts.googleapis.com/css2?family=Nunito+Sans:opsz,wght@6..12,200;6..12,300;6..12,400;6..12,500;6..12,600;6..12,700;6..12,800;6..12,900;6..12,1000&amp;display=swap" rel="stylesheet">
    <!-- Flag icon css -->
    <link rel="stylesheet" href="assets/css/vendors/flag-icon.css">
    <!-- iconly-icon-->
    <link rel="stylesheet" href="assets/css/iconly-icon.css">
    <link rel="stylesheet" href="assets/css/bulk-style.css">
    <!-- iconly-icon-->
    <link rel="stylesheet" href="assets/css/themify.css">
    <!--fontawesome-->
    <link rel="stylesheet" href="assets/css/fontawesome-min.css">
   
  
    <!-- App css -->
    <link rel="stylesheet" href="assets/css/style.css">
    <link id="color" rel="stylesheet" href="assets/css/color-1.css" media="screen">
    <link href="assets/js/toastr/toastr.min.css" rel="stylesheet" />
    <script src="assets/js/vendors/jquery/jquery.min.js"></script>
      
</head>
<body>
    <!-- tap on top starts-->
    <div class="tap-top"><i class="iconly-Arrow-Up icli"></i></div>
    <!-- tap on tap ends-->
    <!-- loader-->
    <div class="loader-wrapper">
        <div class="loader"><span></span><span></span><span></span><span></span><span></span></div>
    </div>
    <!-- login page start-->

    <div class="container-fluid bg-cover">
        <div class="row">

            <div class="login-card login-dark login-bg">
                <div>
                    <div class="login-main bg-white rounded-3">
                        <form class="theme-form" runat="server">
                            <h2 class="text-center fw-bold text-primary pb-1">Library Management System</h2>
                            <h3 class="text-center">Login to Account</h3>
                             <asp:ScriptManager ID="ScriptManager1" runat="server" />
                            <div class="form-group">
                                <label class="col-form-label fw-semibold fs-5">Login ID</label>
                                <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" placeholder="Enter LoginID"></asp:TextBox>
                              
                            </div>
                            <div class="form-group">
                                <label class="col-form-label fw-semibold fs-5">Password</label>
                                <div class="form-input position-relative">
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control mt-0" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
                                 
                                </div>
                            </div>
                            <div class="form-group mb-0">
                                <a class="link" href="forget-password.html">Forgot password?</a>
                                <div class="text-end mt-3">
                                    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-100 fw-bold fs-4 rounded-pill p-2" OnClick="btnLogin_Click" OnClientClick="return validateLogin();"/>

                                   
                                </div>
                            </div>
                        </form>
                        
                    </div>
                </div>
            </div>
        </div>
       

       
       <script src="assets/js/toastr/toastr.min.js" type="text/javascript"></script>
        <!-- bootstrap js-->
        <script src="assets/js/vendors/bootstrap/dist/js/bootstrap.bundle.min.js" defer=""></script>
        <script src="assets/js/vendors/bootstrap/dist/js/popper.min.js" defer=""></script>
        <!--fontawesome-->
        <script src="assets/js/vendors/font-awesome/fontawesome-min.js"></script>

         
        <!-- password_show-->
        <script src="assets/js/password.js"></script>
        <!-- custom script -->
        <script src="assets/js/script.js"></script>
        <script src="assets/js/Custom-Toasts.js"></script>
        <script type="text/javascript">
           
            function validateLogin() {
            var loginId = document.getElementById('<%= txtUserId.ClientID %>').value.trim();
            var password = document.getElementById('<%= txtPassword.ClientID %>').value.trim();
            var loginIdPattern = /^[A-Za-z0-9 ]+$/

            if (!loginId) {
                AlertMessage("Please enter Login ID.", "error");
                document.getElementById('<%= txtUserId.ClientID %>').focus();
                return false;
            }
            if (!loginIdPattern.test(loginId)) {
                AlertMessage("Login ID must contain only letters and numbers and spaces.", "error");
                document.getElementById('<%= txtUserId.ClientID %>').focus();
                return false;
            }
            if (!password) {
                AlertMessage("Please enter Password.", "error");
                document.getElementById('<%= txtPassword.ClientID %>').focus();
                    return false;
                }
                return true;
            }
        </script>
    </div>
   
  
</body>

<%--<body >
    <div class="tap-top"><i class="iconly-Arrow-Up icli"></i></div>
        <!-- tap on tap ends-->
        <!-- loader-->
        <div class="loader-wrapper">
          <div class="loader"><span></span><span></span><span></span><span></span><span></span></div>
        </div>
    <div class="bg-cover min-vh-100 d-flex align-items-center justify-content-center">
    <form id="form1" runat="server" class="w-100">
        <div class="bg-cover min-vh-100 d-flex align-items-center justify-content-center">
        <div class="container">
            <div class="row justify-content-center align-items-center min-vh-100">
                <div class="col-12 col-sm-10 col-md-8 col-lg-7 col-xl-auto">
                    <div class="login-card p-5 rounded-4 shadow">
                          <h2 class="text-center fw-bold text-primary">Library Management System</h2>
                        <h3 class="text-center mb-3 fw-bold">Login</h3>

                        <div class="mb-0">
                            <label class="form-label fw-semibold fs-5">Login ID</label>
                            <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control fs-5" placeholder="Enter LoginID"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtUserId"
                                ErrorMessage="Login ID is required"
                                CssClass="text-danger small">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator 
                                ID="RegexUserIdValidator" 
                                runat="server"
                                ControlToValidate="txtUserId"
                                ErrorMessage="Only letters and numbers are allowed"
                                CssClass="text-danger small"
                                ValidationExpression="^[a-zA-Z0-9]+$">
                            </asp:RegularExpressionValidator>
                          
                                
                        </div>

                        <div class="mb-0">
                            <label class="form-label fw-semibold fs-5">Password</label>
                            <div class="position-relative">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control pe-5 fs-5" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
                               
                            </div>
                            <asp:RequiredFieldValidator 
                                ID="RequiredFieldValidator2" 
                                runat="server"
                                ControlToValidate="txtPassword"
                                ErrorMessage="Password is required"
                                CssClass="text-danger small">
                            </asp:RequiredFieldValidator>
                        </div>

                        <div class="d-flex justify-content-between align-items-center mb-2">
                            <a href="#" class="forgotPassword small text-decoration-none fs-5">Forgot Password?</a>
                        </div>

                        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-100 fw-bold fs-5 rounded-pill p-3" OnClick="btnLogin_Click" />
                        
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger text-center d-block mt-3 fw-semibold"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
           
    </form>
 </div>
    
   
</body>--%>
</html>


<%--<div class="login-main">
     <form class="form" runat="server">
         <h2 class="text-center fw-bold text-primary pb-1">Library Management System</h2>
         <h3 class="text-center">Login to Account</h3>
         
         <div class="form-group">
             <label class="col-form-label fw-semibold fs-5">Login ID</label>
             <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" placeholder="Enter LoginID"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                  ControlToValidate="txtUserId"
                 ErrorMessage="Login ID is required"
                 CssClass="text-danger small">
             </asp:RequiredFieldValidator>
             <asp:RegularExpressionValidator 
                 ID="RegexUserIdValidator" 
                 runat="server"
                 ControlToValidate="txtUserId"
                 ErrorMessage="Only letters and numbers are allowed"
                 CssClass="text-danger small"
                 ValidationExpression="^[a-zA-Z0-9]+$">
             </asp:RegularExpressionValidator>
   
             </div>
         <div class="form-group">
             <label class="col-form-label fw-semibold fs-5">Password</label>
             <div class="form-input position-relative">
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
                  <asp:RequiredFieldValidator 
                       ID="RequiredFieldValidator2" 
                       runat="server"
                       ControlToValidate="txtPassword"
                       ErrorMessage="Password is required"
                       CssClass="text-danger small">
                   </asp:RequiredFieldValidator>
             </div>
         </div>
         <div class="form-group mb-0">
             <a class="link" href="forget-password.html">Forgot password?</a>
             <div class="text-end mt-3">
                  <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-100 fw-bold fs-4 rounded-pill p-2" OnClick="btnLogin_Click" />
 
                  <asp:Label ID="lblMessage" runat="server" CssClass="text-danger text-center d-block mt-3 fw-semibold"></asp:Label>
             </div>
         </div>
     </form>
 </div>--%>