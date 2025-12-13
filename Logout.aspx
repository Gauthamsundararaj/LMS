<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="LMS.Logout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logout | LMS</title>
     <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" 
     rel="stylesheet" integrity="sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB" 
     crossorigin="anonymous" />
  
     <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />
     <%-- Bootsrapp JS --%>
     <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" 
         integrity="sha384-FKyoEForCGlyvwx9Hj09JcYn3nv7wiPVlz7YYwJrWVcXK/BmnVDxM+D2scQbITxI" 
         crossorigin="anonymous"></script>
</head>
<body class="d-flex justify-content-center align-items-center vh-100 bg-light">
    <form id="form1" runat="server">
        <div class="card shadow-lg p-4 text-center" style="max-width: 400px; width: 100%;">
            <h3 class="text-success fw-semibold mb-3">Logout Successful!</h3>
            <p class="text-muted mb-4">You have been logged out of your account.</p>

            <!-- Link to go back to login page -->
            <a href="Login.aspx" class="btn btn-primary w-100">Go to Login Page</a>
        </div>
    </form>
</body>
</html>
