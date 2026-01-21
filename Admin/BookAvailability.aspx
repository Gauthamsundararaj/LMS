<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookAvailability.aspx.cs" Inherits="Admin.BookAvailability" %>

<%@ Register Src="../Controls/Header.ascx" TagPrefix="uc1" TagName="Header" %>
<%@ Register Src="../Controls/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Book Availability</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../assets/css/customPagination.css" rel="stylesheet" />
    <style>
        .required {
            color: red;
        }
        .authors-header {
            width: 240px;
            white-space: nowrap;
        }  
        .authors-cell {
            width: 240px;
            white-space: normal;
            word-break: normal;
            overflow-wrap: normal;
            line-height: 1.5;
            padding: 6px 8px;
            vertical-align: top;
        }
        .search-wrapper {
            position: relative;
        }

            .search-wrapper input {
                padding-right: 30px;
            }

        .clear-btn {
            position: absolute;
            right: 8px;
            top: 50%;
            transform: translateY(-50%);
            cursor: pointer;
            color: red;
            display: none;
        }

            .clear-btn:hover {
                color: #a71d2a;
            }

        .search-wrapper input:not(:placeholder-shown) ~ .clear-btn {
            display: block;
        }

        .hide-clear-filter {
            display: none !important;
        }

        #gvbookA th,
        #gvbookA td {
            text-align: center;
            vertical-align: middle;
        }

        .btn-group .multiselect-selected-text {
            display: block;
            white-space: nowrap;
            overflow-x: auto;
            overflow-y: hidden;
            max-width: 400px;
        }
    </style>
    <link href="../assets/css/bootstrap-multiselect.min.css" rel="stylesheet" />

</head>

<body>
    <uc1:header runat="server" id="Header" />
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfIsFirstTime" runat="server" Value="1" />
        <div class="page-body">
            <div class="container-fluid pt-2">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary p-2">
                        <div class="row align-items-center justify-content-between">
                            <div class="col-auto">
                                <h4 class="card-title mb-0">Book Availability</h4>
                            </div>
                            <div class="col-auto text-end">
                                <asp:LinkButton  ID="lnkDownloadCSV" runat="server" OnClick="btnDownloadCSV_Click"  ToolTip="Download CSV">
                                     <asp:Image  ID="imgDownload" runat="server" ImageUrl="~/assets/images/icons/csvdownload.png"
                                        AlternateText="Download" CssClass="icon img-fluid" Width="35" Height="35" />
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <asp:HiddenField runat="server" ID="hfRemoveColumnsCSV" Value="BookId, CategoryName, Active" />
                        <div class="row mb-3 align-items-end g-2">
                            <div class="col-md-6">
                                <label class="fw-bold fs-6">
                                    Category <span class="required">*</span>
                                </label>
                                <asp:ListBox ID="ddlCategory"  runat="server" CssClass="form-select" SelectionMode="Multiple" AutoPostBack="false" />
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnSearch"  runat="server" Text="Show Books"  CssClass="btn btn-primary" OnClick="btnShow_Click" />
                                <asp:Button ID="btnClear" runat="server"  Text="Clear"  CssClass="btn btn-warning" OnClick="btnClear_Click" />
                            </div>
                            <div class="col-md-2 col-lg-auto ms-lg-auto text-lg-end p-1">
                                <asp:Label ID="lblRecordCount" runat="server" CssClass="fw-bold text-primary"></asp:Label>
                            </div>
                        </div>
                        <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                            <div class="table-responsive">
                                <asp:GridView ID="gvbookA" runat="server"  AutoGenerateColumns="false"  CssClass="table table-bordered table-striped table-hover"
                                    AllowPaging="true"  PageSize="5"  OnPageIndexChanging="gvbookA_PageIndexChanging"
                                    PagerSettings-Visible="false" ShowHeaderWhenEmpty="true" ClientIDMode="Static"  EmptyDataText="No records found">
                                <EmptyDataTemplate>
                                        <div class="text-center py-4">
                                            <i class="bi bi-search fs-2 text-muted"></i>
                                            <div class="mt-2 fw-semibold text-muted">  No results found </div>
                                            <div class="mt-3">
                                                <asp:LinkButton ID="btnEmptyRefresh"  runat="server" CssClass="btn btn-primary btn-sm"
                                                    ToolTip="Refresh" OnClick="btnRefresh_Click">  <i class="bi bi-arrow-clockwise"></i> Refresh
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </EmptyDataTemplate> 
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <HeaderTemplate>
                                                <div class="fw-bold text-center">S.No</div>
                                                 <div class="d-flex justify-content-center gap-2 mt-1">
                                                    <asp:LinkButton ID="btnApplyFilter" runat="server" CssClass="text-primary" ToolTip="Filter"
                                                        OnClientClick="toggleClearFilterIcon();"  OnClick="btnSearch_Click"> <i class="fa-solid fa-filter"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnClearFilter" runat="server"  ToolTip="Clear Filter" CssClass="text-danger clear-filter-btn"
                                                        Style="display: none;"  OnClick="btnRefresh_Click">  <i class="fa-solid fa-circle-xmark"></i>
                                                    </asp:LinkButton>
                                                 </div>
                                           </HeaderTemplate>
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %> </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div class="fw-bold">ISBN</div>
                                                <div class="search-wrapper">
                                                    <asp:TextBox ID="txtFilterISBN" runat="server" CssClass="form-control form-control-sm" placeholder="Search ISBN"
                                                        MaxLength="13" onkeypress="return allowOnlyNumbers(event);" onkeyup="toggleClearFilterIcon();" />
                                                    <span class="clear-btn" onclick="clearSearch(this)">✖</span>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate> <%# Eval("ISBN") %> </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div class="fw-bold">Book Title</div>
                                                <div class="search-wrapper">
                                                    <asp:TextBox ID="txtFilterBookTitle" runat="server" CssClass="form-control form-control-sm" placeholder="Search Title"
                                                        onkeypress="return allowAlphaNumeric(event);" onkeyup="toggleClearFilterIcon();" />
                                                     <span class="clear-btn" onclick="clearSearch(this)">✖</span>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate> <%# Eval("Book Title") %> </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div class="fw-bold">Author</div>
                                                <div class="search-wrapper">
                                                    <asp:TextBox ID="txtFilterAuthor" runat="server" CssClass="form-control form-control-sm" placeholder="Search Author"
                                                        MaxLength="30" onkeypress="return allowAlphaNumeric(event);" onkeyup="toggleClearFilterIcon();" />
                                                    <span class="clear-btn" onclick="clearSearch(this)">✖</span>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>  <%# Eval("Author Name") %> </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div class="fw-bold">Year</div>
                                                <div class="search-wrapper">
                                                    <asp:TextBox ID="txtFilterYear" runat="server" CssClass="form-control form-control-sm" MaxLength="4"
                                                        onkeypress="return allowOnlyNumbers(event);" onkeyup="toggleClearFilterIcon();" />
                                                    <span class="clear-btn" onclick="clearSearch(this)">✖</span>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate> <%# Eval("Published Year") %> </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div class="fw-bold">Publisher</div>
                                                <div class="search-wrapper">
                                                    <asp:TextBox ID="txtFilterPublisher" runat="server" CssClass="form-control form-control-sm" placeholder="Search Publisher"
                                                        MaxLength="30" onkeypress="return allowAlphaNumeric(event);" onkeyup="toggleClearFilterIcon();" />
                                                    <span class="clear-btn" onclick="clearSearch(this)">✖</span>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate><%# Eval("Publisher Name") %></ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="TotalCopies" HeaderText="Total Copies" />
                                        <asp:BoundField DataField="AvailableCopies" HeaderText="Available Copies" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="pager-fixed">
                                <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPage" runat="server" CssClass='<%# (bool)Eval("IsActive") ? "page-btn active" : "page-btn" %>'
                                            CommandName='<%# Eval("Command") %>' CommandArgument='<%# Eval("PageIndex") %>' Enabled='<%# Eval("Enabled") %>' Text='<%# Eval("Text") %>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <uc1:footer runat="server" id="Footer" />
    <script src="../assets/js/bootstrap-multiselect.min.js"></script>

    <script>
        $(document).ready(function () {

            try {
                if ($('#ddlCategory').data('multiselect'))
                    $('#ddlCategory').multiselect('destroy');
            } catch (e) { }

            $('#ddlCategory').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonWidth: '100%',
                maxHeight: 250,
                numberDisplayed: 10,
                buttonTextAlignment: 'left',
                nonSelectedText: 'Select Category',
            });

            toggleClearFilterIcon();
        });

        function clearSearch(el) {
            var input = el.previousElementSibling;
            input.value = '';
            toggleClearFilterIcon();
            __doPostBack('<%= btnSearch.ClientID %>', '');
        }

        function allowOnlyNumbers(evt) {
            var charCode = evt.which ? evt.which : evt.keyCode;

            // Allow backspace, delete, arrows
            if (charCode === 8 || charCode === 46 || charCode === 37 || charCode === 39) {
                return true;
            }
            // Allow only digits
            if (charCode < 48 || charCode > 57) {
                evt.preventDefault();
                return false;
            }
            return true;
        }

        function allowAlphaNumeric(evt) {
            var charCode = evt.which ? evt.which : evt.keyCode;

            // Allow backspace, space, delete
            if (charCode === 8 || charCode === 32 || charCode === 46) {
                return true;
            }

            // 0-9 A-Z a-z
            if (
                (charCode >= 48 && charCode <= 57) || // numbers
                (charCode >= 65 && charCode <= 90) || // uppercase
                (charCode >= 97 && charCode <= 122)   // lowercase
            ) {
                return true;
            }

            evt.preventDefault();
            return false;
        }

        function toggleClearFilterIcon() {

            const isbn = $('#gvbookA input[id*="txtFilterISBN"]').val().trim();
            const title = $('#gvbookA input[id*="txtFilterBookTitle"]').val().trim();
            const author = $('#gvbookA input[id*="txtFilterAuthor"]').val().trim();
            const year = $('#gvbookA input[id*="txtFilterYear"]').val().trim();
            const publisher = $('#gvbookA input[id*="txtFilterPublisher"]').val().trim();

            const hasValue =
                isbn !== '' ||
                title !== '' ||
                author !== '' ||
                year !== '' ||
                publisher !== '';

            const clearBtn = $('#gvbookA .clear-filter-btn');

            if (hasValue) {
                clearBtn.show();
            } else {
                clearBtn.hide();
            }
        }
    </script>
</body>
</html>
