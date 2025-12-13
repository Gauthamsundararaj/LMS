<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookMaster.aspx.cs" Inherits="Admin.BookMaster" %>

<%@ Register Src="../Controls/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="../Controls/Footer.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BookMaster</title>

    <link rel="stylesheet" href="../assets/css/vendors/select2.css" />
    <link rel="stylesheet" href="../assets/css/bootstrap-multiselect.min.css" />
    <style>
        .form-label {
            display: block;
        }

        .btn-group {
            width: -webkit-fill-available;
        }

        .multiselect-native-select {
            width: 100%;
            /* set height */
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
        /*        .pagination-outer a, .pagination-outer span {
            padding: 6px 12px;
            margin: 2px;
            border: 1px solid #ddd;
            color: #007bff;
            background: #fff;
            cursor: pointer;
        }

        .pagination-outer span {
            background: #007bff;
            color: white;
            border-color: #007bff;
        }

        .pagination-outer a:hover {
            background: #e2e6ea;
        }*/

        .table-scroll {
            overflow-x: auto;
            white-space: nowrap;
            width: 100%;
            margin-bottom: 20px;
        }

        /* FIXED pagination bar */
        .pager-fixed {
            position: sticky; /* stays visible even while scrolling */
            bottom: 0;
            background: #fff;
            padding: 10px 0;
            text-align: center;
            z-index: 20;
            border-top: 2px solid #18a999;
        }

        /* Pagination button style */
        .page-btn {
            padding: 6px 12px;
            margin: 0 4px;
            background: #e5ffe5;
            border-radius: 4px;
            border: 1px solid #18a999;
        }

            .page-btn:hover {
                background: #18a999;
                color: #fff;
            }
    </style>

</head>
<body>
    <uc:header id="Header" runat="server" />
    <form id="form1" runat="server">
        <div class="page-body ">
            <div class="container-fluid mt-2">
                <div class="row ">

                    <div class="text-end mb-2">
                        <asp:Button ID="btnAddBooks" runat="server" Text="Add Books" CssClass="btn btn-success mt-2" OnClick="btnAddBooks_Click" />
                    </div>
                </div>
                <div class="card" id="divBookGrid" runat="server" visible="false">
                    <div class="card-header bg-primary">
                        <h3 class="card-title mb-0">Book Details</h3>
                    </div>

                    <div class="card-body">

                        <!-- ---------- SEARCH AREA (Not Scrollable) ---------- -->
                        <div class="row mb-3 align-items-end ">
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlSearchBy" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">---- Search By ----</asp:ListItem>
                                    <asp:ListItem Value="Category">Category Name</asp:ListItem>
                                    <asp:ListItem Value="BookTitle">Book Title</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox ID="txtSearchValue" runat="server" CssClass="form-control"
                                    Placeholder="Enter search text" MaxLength="50"></asp:TextBox>
                            </div>

                            <div class="col-md-auto d-grid">
                                <asp:Button ID="btnSearch" runat="server" Text="Search"
                                    CssClass="btn btn-primary"
                                    OnClick="btnSearch_Click" />
                            </div>

                            <div class="col-md-auto d-grid">
                                <asp:Button ID="btnClearSearch" runat="server" Text="Clear"
                                    CssClass="btn btn-secondary"
                                    OnClick="btnClearSearch_Click" />
                            </div>
                            <div class="col-md-auto d-grid">
                                <asp:Label ID="lblRecordCount" runat="server"
                                    CssClass="fw-bold text-primary"></asp:Label>

                            </div>
                            <div class="col text-end">
                                <asp:Button ID="btnDownloadCSV" runat="server"
                                    Text="Download CSV"
                                    CssClass="btn btn-info"
                                    OnClick="btnDownloadCSV_Click" />
                            </div>


                        </div>

                        <!-- ---------- ONLY GRIDVIEW SCROLLS ---------- -->
                        <div class="table-responsive" style="overflow-y: auto; white-space: nowrap;">
                            <asp:GridView ID="gvBookMaster" runat="server"
                                CssClass="table table-bordered table-striped"
                                AutoGenerateColumns="False"
                                OnRowCommand="gvBookMaster_RowCommand"
                                AllowPaging="True"
                                PageSize="5"
                                OnPageIndexChanging="gvBookMaster_PageIndexChanging"
                                PagerSettings-Visible="false">
                                <Columns>
                                    <asp:BoundField DataField="Sno" HeaderText="Sno" />
                                    <asp:BoundField DataField="BookID" Visible="false" />
                                    <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                                    <asp:BoundField DataField="BookTitle" HeaderText="Book Title" />
                                    <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                                    <asp:BoundField DataField="AuthorNames" HeaderText="Authors" />
                                    <%-- <asp:BoundField DataField="Language" HeaderText="Language" />--%>
                                    <asp:BoundField DataField="PublisherName" HeaderText="Publisher" />
                                    <%-- <asp:BoundField DataField="YearPublished" HeaderText="Year" />
                                        <asp:BoundField DataField="Edition" HeaderText="Edition" />
                                        <asp:BoundField DataField="Price" HeaderText="Price" />--%>
                                    <asp:BoundField DataField="TotalCopies" HeaderText="Total Copies" />
                                    <%--<asp:BoundField DataField="ShelfLocation" HeaderText="Shelf Location" />--%>
                                    <asp:BoundField DataField="ActiveStatus" HeaderText="Status" />
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server"
                                                CommandName="EditBook"
                                                CommandArgument='<%# Eval("BookID") %>'
                                                CssClass="btn btn-sm btn-primary me-2"
                                                ToolTip="Edit Book">
                                    <i class="iconly-Edit icli"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server"
                                                CommandName="DeleteBook"
                                                CommandArgument='<%# Eval("BookID") %>'
                                                CssClass='<%# Convert.ToBoolean(Eval("Active"))
                                            ? "btn btn-sm btn-danger"
                                            : "btn btn-sm btn-secondary disabled" %>'
                                                ToolTip='<%# Convert.ToBoolean(Eval("Active"))
                                            ? "Click to deactivate"
                                            : "Already inactive" %>'
                                                OnClientClick='<%# Convert.ToBoolean(Eval("Active"))
                                                ? "return confirm(\"Are you sure you want to deactivate this book?\");"
                                                : "return false;" %>'>
                                <i class="iconly-Delete icli"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>


                            </asp:GridView>

                        </div>
                        <div class="pager-fixed">
                            <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPage" runat="server"
                                        CommandName="Page"
                                        CommandArgument='<%# Eval("PageIndex") %>'
                                        CssClass="page-btn"
                                        Text='<%# Eval("Text") %>'>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>

                <div id="divForm" runat="server" visible="false">
                    <div class="container mt-4">
                        <input type="hidden" id="hdnBookID" runat="server" />
                        <div class="card">
                            <div class="card-header bg-primary text-white">
                                <h3 class="card-title mb-0">Book Entry</h3>
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-4 ">
                                        <label class="form-label">
                                            Book Title <span
                                                style="color: red">*</span></label>
                                        <asp:TextBox ID="txtBookTitle" runat="server"
                                            CssClass="form-control" placeholder="Enter Book Title"></asp:TextBox>
                                    </div>
                                    <!-- ISBN -->
                                    <div class="col-md-4 ">
                                        <label class="form-label">
                                            ISBN Number <span
                                                style="color: red">*</span></label>
                                        <asp:TextBox ID="txtISBN" runat="server"
                                            CssClass="form-control" MaxLength="13" placeholder="Enter ISBN Number"></asp:TextBox>
                                    </div>


                                  <!-- Category (searchable dropdown) -->
                                  <div class="col-md-4">
                                        <label class="form-label">
                                            Category <span
                                                style="color: red">*</span></label>
                                        <asp:DropDownList ID="ddlCategory" runat="server"
                                            CssClass="form-select js-example-basic-single col-sm-12" data-live-search="true">
                                        </asp:DropDownList>
                                  </div>
                               
                                


                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Author Name <span style="color: red">*</span></label>

                                    <asp:ListBox ID="lstAuthor" runat="server"
                                        CssClass="form-select"
                                        SelectionMode="Multiple"></asp:ListBox>
                                </div>

                                <!-- Language -->
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">
                                        Language <span
                                            style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddlLanguage" runat="server"
                                        CssClass="form-select">
                                        <asp:ListItem Text="Select Language" Value=""
                                            Selected="True" Disabled="True"></asp:ListItem>
                                        <asp:ListItem Text="English" Value="English" />
                                        <asp:ListItem Text="Tamil" Value="Tamil" />
                                        <asp:ListItem Text="Hindi" Value="Hindi" />
                                        <asp:ListItem Text="Malayalam"
                                            Value="Malayalam" />
                                        <asp:ListItem Text="Kannada" Value="Kannada" />
                                        <asp:ListItem Text="Telugu" Value="Telugu" />
                                    </asp:DropDownList>
                                </div>
                                <!-- Publisher -->
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">
                                        Publisher Name <span
                                            style="color: red">*</span></label>
                                    <asp:TextBox ID="txtPublisher" runat="server"
                                        CssClass="form-control" placeholder="Enter Publisher Name" MaxLength="100"></asp:TextBox>
                                </div>
                                <!-- Year Published -->
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">
                                        Year Published <span
                                            style="color: red">*</span></label>
                                    <asp:TextBox ID="txtYearPublished" runat="server"
                                        CssClass="form-control" MaxLength="4" placeholder="Enter Year (YYYY)"></asp:TextBox>
                                </div>
                                <!-- Edition -->
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">Edition <span style="color: red">*</span></label>
                                    <asp:TextBox ID="txtEdition" runat="server"
                                        CssClass="form-control" MaxLength="20" placeholder="Enter Edition"></asp:TextBox>
                                </div>
                                <!-- Price -->
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">
                                        Price
                                    </label>
                                    <asp:TextBox ID="txtPrice" runat="server"
                                        CssClass="form-control" placeholder="Enter Price"></asp:TextBox>
                                </div>
                                <!-- Total Copies -->
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">
                                        Total Copies <span
                                            style="color: red">*</span></label>
                                    <asp:TextBox ID="txtTotalCopies" runat="server"
                                        CssClass="form-control" placeholder="Enter Total Copies"></asp:TextBox>
                                </div>

                                <!-- Available Copies -->

                                <!-- Shelf Location -->
                                <div class="col-md-4 mb-3">
                                    <label class="form-label">
                                        Shelf Location <span
                                            style="color: red">*</span></label>
                                    <asp:TextBox ID="txtShelfLocation" runat="server"
                                        CssClass="form-control" MaxLength="50" placeholder="Enter Shelf Location"></asp:TextBox>
                                </div>
                                <!-- Status -->
                                <div class="col-md-4">
                                    <label class="form-label d-block">Status <span style="color: red">*</span></label>
                                    <div class="form check form-check-inline">
                                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                        <label class="form-check-label ms-2" for="chkActive">Active</label>
                                    </div>
                                </div>
                            </div>
                            <!-- End Row -->
                        </div>
                        <div class="card-footer text-end">
                            <asp:Button ID="btnSave" runat="server" Text="Save"
                                CssClass="btn btn-primary me-2" OnClientClick="return validateBookForm();"
                                OnClick="btnSave_Click" />
                            <asp:Button ID="btnUpdate" runat="server" Text="Update"
                                CssClass="btn btn-success me-2" OnClientClick="return validateBookForm();"
                                OnClick="Update_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="Clear_Click"
                                CssClass="btn btn-warning me-2" OnClientClick="clearForm(); return false;" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel"
                                CssClass="btn btn-danger" OnClientClick="window.history.back(); return false;" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>



            </div>
        </div>
        </div>

    </form>


    <uc:footer id="Footer1" runat="server" />
    <script src="../assets/js/select2/select2.full.min.js"></script>
    <script src="../assets/js/select2/select2-custom.js"></script>
    <script src="../assets/js/bootstrap-multiselect.min.js"></script>
    <script src="../assets/js/custom-inputsearch.js"></script>

    <!-- Client-side validation and selectpicker init -->
    <script>

        $(document).ready(function () {
            $('#lstAuthor').multiselect({
                enableFiltering: true,
                buttonTextAlignment: 'left',

            });
        });


        function validateBookMaster() {

            function showErr(msg, id) {
                AlertMessage(msg, "error");
                if (id) document.getElementById(id).focus();
                return false;
            }
            var title = document.getElementById('<%= txtBookTitle.ClientID %>').value.trim();
            if (title === "") return showErr("Book Title is required.", '<%= txtBookTitle.ClientID %>');

            var isbn = document.getElementById('<%= txtISBN.ClientID %>').value.trim();
            if (isbn === "") return showErr("ISBN is required.", '<%= txtISBN.ClientID %>');
            if (!/^[A-Za-z0-9-]+$/.test(isbn)) return showErr("Invalid ISBN.", '<%= txtISBN.ClientID %>');



            var authors = $('#<%= lstAuthor.ClientID %>').val();
            if (!authors || authors.length === 0) return showErr("Select at least one author.");

            var category = $('#<%= ddlCategory.ClientID %>').val();
            if (!category) return showErr("Select category.");

            var year = document.getElementById('<%= txtYearPublished.ClientID %>').value.trim();
            if (year === "" || !/^\d{4}$/.test(year)) return showErr("Enter valid Year (YYYY).", '<%= txtYearPublished.ClientID %>');

            var price = document.getElementById('<%= txtPrice.ClientID %>').value.trim();
            if (price === "" || !/^\d+(\.\d{1,2})?$/.test(price) || parseFloat(price) <= 0) {
                return showErr("Enter valid Price.", '<%= txtPrice.ClientID %>');
            }

            var total = document.getElementById('<%= txtTotalCopies.ClientID %>').value.trim();
            if (!/^[0-9]+$/.test(total)) return showErr("Total Copies must be whole number.", '<%= txtTotalCopies.ClientID %>');


            return true;
        }

    </script>
</body>
</html>
