<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookIssue.aspx.cs" Inherits="Admin.BookIssue" %>


<%@ Register Src="../Controls/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="../Controls/Footer.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Issue Books</title>
    <link rel="stylesheet" href="../assets/css/bootstrap-multiselect.min.css" />
    <style>
        .select2-container .select2-selection--single {
            height: 40px !important;
            line-height: 40px !important;
        }

        .form-label {
            display: block;
        }

        .btn-group {
            width: -webkit-fill-available;
        }

        .multiselect-native-select {
            width: 100%;
        }

        .multiselect-option {
            color: black;
        }

        .form-check-label {
            color: black;
        }

        .multiselect-container {
            width: 100% !important;
            max-height: 200px !important; /* adjust if needed */
            overflow-y: auto !important;
        }

            /* FIX search bar: make it sticky */
            .multiselect-container .multiselect-filter {
                width: 100% !important;
                position: sticky;
                top: 0;
                background: #fff;
                z-index: 100;
            }

            /* Hide "select all" if you don't use it */
            .multiselect-container .multiselect-item.multiselect-all {
                display: none;
            }

        #rblIssueType input[type="radio"] {
            margin-right: 6px;
            margin-left: 10px;
            margin-top: 10px;
            margin-bottom: 10px;
            accent-color: #308e87; /* Bootstrap primary color */
        }
    </style>
</head>
<body>
    <uc:header id="Header" runat="server" />
    <form id="form1" runat="server">

        <div class="page-body">

            <div class="container-fluid pt-2">
                <%--------- Form Start ---------%>
                <div id="divForm" runat="server" visible="true">

                    <%--               <input type="hidden" id="hdnSelectedBookIDs" runat="server" />--%>
                    <div class="card">
                        <div class="card-header bg-primary p-3">
                            <h3 class="mb-0">Issue Books Entry</h3>
                        </div>
                        <div class="card-body">
                            <!-- Select Type -->
                            <div class="row mb-2">
                                <div class="col-md-4">
                                    <label class="form-label fw-bold">Issue To</label>
                                    <asp:RadioButtonList ID="rblIssueType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblIssueType_SelectedIndexChanged">
                                        <asp:ListItem Text="Student ID&nbsp;&nbsp;" Value="Student" CssClass="me-5"></asp:ListItem>
                                        <asp:ListItem Text="Staff ID&nbsp;&nbsp;" Value="Staff" CssClass="ms-5"></asp:ListItem>

                                    </asp:RadioButtonList>
                                </div>
                                <div id="divStudent" class="col-md-4" runat="server" visible="false">
                                    <label class="form-label">Student ID <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtStudentID" runat="server" CssClass="form-control" placeholder="Enter Student ID"></asp:TextBox>

                                </div>

                                <div id="divStaff" class="col-md-4" runat="server" visible="false">
                                    <label class="form-label">Staff ID <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtStaffID" runat="server" CssClass="form-control" placeholder="Enter Staff ID"></asp:TextBox>

                                </div>
                            </div>

                            <!-- Common Book Issue Fields -->
                            <div id="divIssueDetails" class="row mt-2" runat="server" visible="false">
                                <!-- ISBN Multi-select dropdown -->

                                <div class="col-md-6">
                                    <label class="form-label">Select Book ISBN <span class="text-danger">*</span></label>
                                    <asp:ListBox ID="lstIsbn" runat="server"
                                        CssClass="form-select"
                                        SelectionMode="Multiple"></asp:ListBox>
                                </div>

                                <!-- Issue Date -->

                                <div class="col-md-3">
                                    <label class="form-label">Issue Date <span class="text-danger">*</span></label>
                                    <input type="date" id="issueDate" runat="server" class="form-control" />
                                </div>

                                <!-- Due Date -->
                                <div class="col-md-3">
                                    <label class="form-label">Due Date <span class="text-danger">*</span></label>
                                    <input type="date" id="dueDate" runat="server" class="form-control" />
                                </div>

                            </div>
                            <div class="card-footer text-end mt-0">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary me-2" Visible="false" OnClick="btnSave_Click1" />
                                <%--data-bs-toggle="modal" data-bs-target=".bd-example-modal-lg"--%>
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-warning me-2" Visible="false" OnClick="btnClear_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" Visible="false" OnClick="btnCancel_Click" />

                            </div>

                        </div>
                    </div>
                </div>

                <div class="card" id="divGrid" runat="server" visible="false">
                    <div class="card-header bg-primary p-3">
                        <h4 class="card-title mb-0 p-0">Selected Books</h4>
                    </div>
                    <div class="card">
                        <div class="card-body">
                            <div class="table-responsive" style="overflow-x: auto; white-space: nowrap;">
                                <asp:GridView ID="gvSelectedBooks" runat="server" AutoGenerateColumns="False" DataKeyNames="ISBN" CssClass="table table-bordered text-center">
                                    <Columns>
                                        <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                                        <asp:BoundField DataField="BookTitle" HeaderText="Book Title" />
                                        <asp:BoundField DataField="AuthorNames" HeaderText="Author(s)" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <asp:Panel ID="pnlConfirm" runat="server" Visible="false" CssClass="mt-2">
                                <asp:Button ID="btnConfirm" runat="server" Text="Issue Books" CssClass="btn btn-success" OnClick="btnConfirm_Click" />
                                <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancel_Click1" />
                            </asp:Panel>
                        </div>
                    </div>
                </div>

                <%--  <div class="modal fade bd-example-modal-lg" id="confirmModal" tabindex="-1" role="dialog" aria-labelledby="myExtraLargeModal" aria-hidden="true">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h3 class="modal-title fs-5" id="myExtraLargeModal">Confirm Book Issue</h3>
                                <button class="btn-close" type="button" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <h5>Selected Books</h5>
                                <hr />
                                <table class="table table-bordered">
                                    <thead class="table-light">
                                        <tr>
                                            <th>ISBN</th>
                                            <th>Book Name</th>
                                            <th>Authors</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tblConfirmBody" runat="server"></tbody>
                                </table>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnConfirmIssue" runat="server"
                                    CssClass="btn btn-success" Text="Confirm Issue"
                                    OnClick="btnConfirmIssue_Click" />
                                <button class="btn btn-secondary" type="button" data-bs-dismiss="modal">Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>--%>
                <%--------- Form End ---------%>
            </div>
        </div>
    </form>
    <uc:footer id="Footer1" runat="server" />

    <script src="../assets/js/bootstrap-multiselect.min.js"></script>

    <script>
        $(document).ready(function () {
            $('#lstIsbn').multiselect({
                enableFiltering: true,
                buttonTextAlignment: 'left',

            });
        });

        function validateBookIssue() {

            function showErr(msg, id) {
                AlertMessage(msg, "error");
                if (id) document.getElementById(id).focus();
                return false;
            }

            // ISSUE TO (Student/Staff)
            var issueType = document.getElementById('<%= rblIssueType.ClientID %>').value;
            if (issueType === "")
                return showErr("Select Issue To (Student or Staff).", '<%= rblIssueType.ClientID %>');

            // STUDENT ID
            if (issueType === "Student") {
                var studentId = document.getElementById('<%= txtStudentID.ClientID %>').value.trim();
                if (studentId === "")
                    return showErr("Student ID is required.", '<%= txtStudentID.ClientID %>');
            }

            // STAFF ID
            if (issueType === "Staff") {
                var staffId = document.getElementById('<%= txtStaffID.ClientID %>').value.trim();
                if (staffId === "")
                    return showErr("Staff ID is required.", '<%= txtStaffID.ClientID %>');
            }

            // ISBN MULTI SELECT
            var isbnList = $('#<%= lstIsbn.ClientID %>').val();
            if (!isbnList || isbnList.length === 0)
                return showErr("Select at least one Book ISBN.");

            // ISSUE DATE
            var issueDate = document.getElementById('<%= issueDate.ClientID %>').value;
            if (issueDate === "")
                return showErr("Issue Date is required.", '<%= issueDate.ClientID %>');

            // DUE DATE
            var dueDate = document.getElementById('<%= dueDate.ClientID %>').value;
            if (dueDate === "")
                return showErr("Due Date is required.", '<%= dueDate.ClientID %>');

            // DATE VALIDATION
            if (new Date(dueDate) < new Date(issueDate))
                return showErr("Due Date must be greater than Issue Date.", '<%= dueDate.ClientID %>');

            return true;
        }



        // open modal from server-side
        //function openConfirmModal() {
        //    var modal = new bootstrap.Modal(document.getElementById('confirmModal'));
        //    modal.show();
        //}

        //// You can call this manually from console or debugging
        //function closeConfirmModal() {
        //    var modalElement = document.getElementById('confirmModal');
        //    var modal = bootstrap.Modal.getInstance(modalElement);
        //    if (modal) modal.hide();
        //}

        //// highlight selected books count dynamically
        //function updateBookCount(count) {
        //    document.getElementById("lblBookCount").innerText = count;
        //}

        //// optional: warn if no books selected
        //function validateBeforeConfirm() {
        //    const count = document.getElementById("lblBookCount").innerText;
        //    if (count === "0") {
        //        alert("No books selected!");
        //        return false;
        //    }
        //    return true;
        //}




    </script>



</body>
</html>
