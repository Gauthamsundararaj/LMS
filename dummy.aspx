<%--             <script src="assets/js/toastr/toastr.min.js" type="text/javascript"></script>
        <script src="/assets/js/toasts-custom.js"></script>
        <script type="text/javascript">
            function validateUserMaster() {
                var loginId = $("#<%=txtLoginId.ClientID%>").val().trim();
                var empCode = $("#<%=txtEmpCode.ClientID%>").val().trim();
                var userName = $("#<%=txtUsername.ClientID%>").val().trim();
                var email = $("#<%=txtEmail.ClientID%>").val().trim();
                var phone = $("#<%=TxtPhone.ClientID%>").val().trim();
                var department = $("#<%=ddlDepartment.ClientID%>").val();
                var designation = $("#<%=ddlDesignation.ClientID%>").val();
                var roleType = $("#<%=ddlRoleType.ClientID%>").val();
                var password = $("#<%=txtPassword.ClientID%>").val().trim();

                // Example rule: Login ID validation
                if (loginId === "") {
                    toastr.error("Please enter Login ID."); // later we’ll replace this with XML message
                    return false;
                }
                if (!/^[A-Za-z0-9]+$/.test(loginId)) {
                    toastr.error("Login ID must contain only letters and numbers.");
                    return false;
                }
                if (loginId.length < 5) {
                    toastr.error("Login ID must be at least 5 characters long.");
                    return false;
                }

                // Example rule: Email
                if (email === "") {
                    toastr.error("Please enter Email.");
                    return false;
                }
                if (!/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(email)) {
                    toastr.error("Please enter a valid Email address.");
                    return false;
                }

                // Example rule: Phone number
                if (phone === "") {
                    toastr.error("Please enter Phone Number.");
                    return false;
                }
                if (!/^\d{10}$/.test(phone)) {
                    toastr.error("Phone Number must be 10 digits only.");
                    return false;
                }

                // Example dropdown validation
                if (department === "") {
                    toastr.error("Please select a Department.");
                    return false;
                }

                if (designation === "") {
                    toastr.error("Please select a Designation.");
                    return false;
                }

                if (roleType === "") {
                    toastr.error("Please select a Role Type.");
                    return false;
                }

                // Example password
                if (password === "") {
                    toastr.error("Please enter Password.");
                    return false;
                }
                if (password.length < 6) {
                    toastr.error("Password must be at least 6 characters long.");
                    return false;
                }

                // If all validations pass
                return true;
            }

            // Attach validation to Save button
            $(document).ready(function () {
                $("#<%=btnSave.ClientID%>").click(function (e) {
        if (!validateUserMaster()) {
            e.preventDefault();
        }
    });
});
            function AlertMessage(message, status) {
                toastr.options = {
                    "closeButton": true,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "timeOut": "4000",
                    "showEasing": "linear",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                };
                if (status === 'success') toastr.success(message);
                else if (status === 'info') toastr.info(message);
                else if (status === 'warning') toastr.warning(message);
                else toastr.error(message);
            }--%>
