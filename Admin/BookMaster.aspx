<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookMaster.aspx.cs" Inherits="Admin.BookMaster" %>

<%@ Register Src="../Controls/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="../Controls/Footer.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BookMaster</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="../assets/css/vendors/select2.css" />
    <link rel="stylesheet" href="../assets/css/bootstrap-multiselect.min.css" />
    <link href="../assets/css/CustomPagination.css" rel="stylesheet" />
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

        label.form-check-label {
            color: black !important;
            font-weight: bold;
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

        .wrap-header {
            width: 240px;
            white-space: normal; /* allow wrapping */
            word-break: normal;
            overflow-wrap: normal;
            text-align: center
        }

        /* Cell */
        .wraping-cell {
            width: 240px;
            white-space: normal; /* allow wrapping */
            word-break: normal; /* DO NOT break words */
            overflow-wrap: normal; /* wrap only at spaces */

            line-height: 1.5;
            padding: 6px 8px;
            vertical-align: top;
        }

        .icon-btn {
            width: 38px;
            height: 38px;
            background-color: #0ea5a0; /* teal matching header */
            border-radius: 8px;
            color: #ffffff;
            text-decoration: none;
            transition: all 0.2s ease-in-out;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.15);
        }

            .icon-btn i {
                font-size: 18px;
            }
    </style>

</head>
<body>
    <uc:header id="Header" runat="server" />
    <form id="form1" runat="server">
        <div class="page-body ">
            <div class="container-fluid pt-2">
                <input type="hidden" id="hdnBookID" runat="server" />
                <div class="row ">
                    <asp:HiddenField runat="server" ID="hfRemoveColumnsCSV" Value="BookId,Active,ActiveStatus" />
                    <div class="text-end mb-2">
                        <asp:Button ID="btnAddBooks" runat="server" Text="Add Books" CssClass="btn btn-success mt-2" OnClick="btnAddBooks_Click" />
                    </div>
                </div>
                <div class="card" id="divBookGrid" runat="server" visible="false">
                    <div class="card-header bg-primary p-2">
                        <div class="row align-items-center justify-content-between">
                            <div class="col-auto">
                                <h4 class="card-title mb-0">Book Details</h4>
                            </div>
                            <div class="col-auto text-end">
                                <asp:LinkButton
                                    ID="lnkDownloadCSV"
                                    runat="server"
                                    OnClick="btnDownloadCSV_Click"
                                    ToolTip="Download CSV">

                                    <asp:Image
                                        ID="imgDownload"
                                        runat="server"
                                        ImageUrl="../assets/images/icons/csvdownload.png"
                                        AlternateText="Download"
                                        CssClass="icon img-fluid"
                                        Width="35" Height="35" />
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <div class="card-body p-3">
                        <!-- ---------- SEARCH AREA (Not Scrollable) ---------- -->
                        <div class="row mb-3 align-items-end g-2">
                            <!-- Left controls -->
                            <div class="col-12 col-lg d-flex flex-wrap gap-2 align-items-end">

                                <div class="col-12 col-sm-6 col-md-3">
                                    <asp:DropDownList ID="ddlSearchBy" runat="server" CssClass="form-select w-100">
                                        <asp:ListItem Value="">------- Search By -------</asp:ListItem>
                                        <asp:ListItem Value="Category">Category Name</asp:ListItem>
                                        <asp:ListItem Value="BookTitle">Book Title</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="col-12 col-sm-6 col-md-3">
                                    <asp:TextBox ID="txtSearchValue" runat="server"
                                        CssClass="form-control w-100"
                                        Placeholder="Enter search text"
                                        MaxLength="50"></asp:TextBox>
                                </div>

                                <div class="col-auto">
                                    <asp:Button ID="btnSearch" runat="server"
                                        Text="Search"
                                        CssClass="btn btn-primary w-100"
                                        OnClick="btnSearch_Click" />
                                </div>

                                <div class="col-auto">
                                    <asp:Button ID="btnClearSearch" runat="server"
                                        Text="Clear"
                                        CssClass="btn btn-secondary w-100"
                                        OnClick="btnClearSearch_Click" />
                                </div>
                            </div>

                            <!-- Right record count -->
                            <div class="col-12 col-lg-auto ms-lg-auto text-lg-end">
                                <asp:Label ID="lblRecordCount" runat="server"
                                    CssClass="fw-bold text-primary"></asp:Label>
                            </div>
                        </div>

                        <!-- ---------- ONLY GRIDVIEW SCROLLS ---------- -->
                        <div class="table-responsive" style="overflow-y: auto; white-space: nowrap;">
                            <asp:GridView ID="gvBookMaster" runat="server"
                                CssClass="table table-bordered table-striped text-center"
                                AutoGenerateColumns="False"
                                OnRowCommand="gvBookMaster_RowCommand"
                                AllowPaging="True"
                                PageSize="5"
                                OnPageIndexChanging="gvBookMaster_PageIndexChanging"
                                PagerSettings-Visible="false"
                                EmptyDataText="No records found">
                                <Columns>
                                    <asp:BoundField DataField="Sno" HeaderText="S.No." />
                                    <asp:BoundField DataField="BookID" Visible="false" />
                                    <asp:BoundField DataField="ISBN" HeaderText="ISBN" />
                                    <asp:TemplateField HeaderText="Book Title">
                                        <ItemStyle CssClass="wraping-cell" />
                                        <ItemTemplate>
                                            <%# Eval("Book Title") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Category" HeaderText="Category" />

                                    <asp:TemplateField HeaderText="Authors">
                                        <ItemStyle CssClass="wraping-cell" />
                                        <ItemTemplate>
                                            <%# Eval("Authors") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Publisher" HeaderText="Publisher" />


                                    <asp:TemplateField HeaderText="Total Copies">
                                        <HeaderStyle CssClass="wrap-header" />
                                        <ItemTemplate>
                                            <%# Eval("Total Copies") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ActiveStatus" HeaderText="Status" />
                                    <asp:BoundField DataField="Active" Visible="false" />
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
                                        CssClass='<%# (bool)Eval("IsActive") ? "page-btn active" : "page-btn" %>'
                                        CommandName='<%# Eval("Command") %>'
                                        CommandArgument='<%# Eval("PageIndex") %>'
                                        Enabled='<%# Eval("Enabled") %>'
                                        Text='<%# Eval("Text") %>'>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                    </div>

                </div>

                <div class="card" id="divForm" runat="server" visible="false">
                    <div class="card-header bg-primary p-3">
                        <h3 class="card-title mb-0">Book Entry</h3>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4 ">
                                <label class="form-label">
                                    Book Title <span
                                        style="color: red">*</span></label>
                                <asp:TextBox ID="txtBookTitle" runat="server"
                                    CssClass="form-control" placeholder="Enter Book Title" MaxLength="50"></asp:TextBox>
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
                                    CssClass="form-control" placeholder="Enter Publisher Name" MaxLength="50"></asp:TextBox>
                            </div>
                            <!-- Year Published -->
                            <div class="col-md-4 mb-3">
                                <label class="form-label">
                                    Year Published <span
                                        style="color: red">*</span></label>
                                <asp:TextBox ID="txtYearPublished" runat="server"
                                    CssClass="form-control" MaxLength="4" placeholder="Enter Year (YYYY)"   oninput="this.value = this.value.replace(/[^0-9]/g, '')"></asp:TextBox>
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
                                    CssClass="form-control" placeholder="Enter Price" MaxLength="8" onkeypress="return allowPriceChars(event);"
                                    onpaste="return false;"></asp:TextBox>
                            </div>
                            <!-- Total Copies -->
                            <div class="col-md-4 mb-3">
                                <label class="form-label">
                                    Total Copies <span
                                        style="color: red">*</span></label>
                                <asp:TextBox ID="txtTotalCopies" runat="server" CssClass="form-control" placeholder="Enter Total Copies"
                                    MaxLength="10"
                                    oninput="this.value = this.value.replace(/[^0-9]/g, '')">
                                </asp:TextBox>
                            </div>

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
                            CssClass="btn btn-danger" OnClick="btnCancel_Click" />
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


            //var pricePattern = /^(?:\d+|\d+\.\d{1,2})$/;

            //if (price === "" || !pricePattern.test(price) ||Number.isNaN(parseFloat(price)) || parseFloat(price) <= 0 ) {
            //////    return showErr("Enter a valid Price (positive number, up to 2 decimals).", '<%= txtPrice.ClientID %>');
            //}


            var total = document.getElementById('<%= txtTotalCopies.ClientID %>').value.trim();
            if (!/^[0-9]+$/.test(total)) return showErr("Total Copies must be whole number.", '<%= txtTotalCopies.ClientID %>');


            return true;
        }
        function allowPriceChars(evt) {
            const key = evt.key;
            const input = evt.target.value;

            // Allow control keys
            if (
                key === "Backspace" || key === "Delete" || key === "Tab" || key === "ArrowLeft" || key === "ArrowRight"
            ) {
                return true;
            }
            // Allow digits
            if (key >= '0' && key <= '9') {
                // If decimal exists, limit to 2 digits after dot
                if (input.includes('.')) {
                    const parts = input.split('.');
                    if (evt.target.selectionStart > input.indexOf('.')) {
                        return parts[1].length < 2;
                    }
                }
                return true;
            }

            // Allow only ONE dot
            if (key === '.') {
                return !input.includes('.');
            }

            return false;
        }
    </script>
</body>
</html>
