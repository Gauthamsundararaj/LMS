<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthorMaster.aspx.cs" Inherits="Admin.AuthorMaster" %>

<%@ Register Src="../Controls/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="../Controls/Footer.ascx" TagPrefix="uc" TagName="Footer" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>AuthorMenu</title>
    <link href="../assets/css/CustomPagination.css" rel="stylesheet" />

</head>
<body>
    <uc:header id="Header" runat="server" />
    <form id="form1" runat="server">
        <div class="page-body">
            <div class="container-fluid pt-2">
                <!-- Form Card -->
                <div id="divForm" class="card mb-3" runat="server" visible="true">
                    <div class="card-header bg-primary p-3 ">
                        <h3 class="card-title mb-0">Author Details</h3>
                    </div>
                    <asp:HiddenField ID="hdnAuthorID" runat="server" />
                    <div class="card-body">

                        <div class="row g-3 needs-validation  validation-forms">
                            <!-- Author Name -->
                            <div class="col-12 col-md-6 col-lg-4 position-relative">
                                <label class="form-label" for="<%= txtAuthorName.ClientID %>">
                                    Author Name <span class="text-danger">*</span>
                                </label>
                                <asp:TextBox ID="txtAuthorName"
                                    runat="server"
                                    CssClass="form-control"
                                    MaxLength="50"
                                    placeholder="Enter Author Name"></asp:TextBox>
                               
                            </div>

                            <!-- Author Type -->
                            <div class="col-12 col-md-6 col-lg-4">
                                <label class="form-label" for="<%= ddlAuthorType.ClientID %>">
                                    Type of Author <span class="text-danger">*</span>
                                </label>
                                <asp:DropDownList ID="ddlAuthorType"
                                    runat="server"
                                    CssClass="form-select">
                                    <asp:ListItem Text="Choose..." Value="" Disabled="True" Selected="True" runat="server" />
                                    <asp:ListItem Text="Main Author" Value="Main" runat="server" />
                                    <asp:ListItem Text="Co-Author" Value="Co-Author" runat="server" />
                                </asp:DropDownList>
                            </div>

                            <!-- Status -->
                            <div class="col-12 col-lg-4">

                                <label class="form-label d-block">Status <span class="text-danger">*</span></label>
                                <div class="form check form-check-inline">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true"/>
                                    <label class="form-check-label ms-2" for="chkActive">Active</label>
                                </div>
                            </div>

                            <!-- Action Buttons -->
                            <div class="col-12">
                                <!-- Stack buttons on small screens, inline on md+ -->
                                <div class="d-grid gap-2 d-sm-flex">
                                    <asp:Button ID="btnAdd"
                                        runat="server"
                                        Text="Save"
                                        CssClass="btn btn-primary"
                                        OnClick="Submit_Click"
                                        OnClientClick="return validateInput()" />
                                    <asp:Button ID="btnUpdate"
                                        runat="server"
                                        Text="Update"
                                        CssClass="btn btn-success"
                                        OnClick="Update_Click"
                                        OnClientClick="return validateInput()"
                                        Visible="false" />
                                    <asp:Button ID="btnClear"
                                        runat="server"
                                        Text="Clear"
                                        CssClass="btn btn-warning"
                                        OnClick="Clear_Click" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <!-- Grid Card -->
                <div class="card" id="divGrid" runat="server">
                    <div class="card-header bg-primary p-3">
                        <h3 class="card-title mb-0">List Of Authors</h3>
                    </div>

                    <div class="card-body pt-0">
                        <div class="col-12 col-lg-auto ms-lg-auto text-lg-end p-1">
                            <asp:Label ID="lblRecordCount" runat="server"
                                CssClass="fw-bold text-primary"></asp:Label>
                        </div>

                        <!-- Table wrapper ensures horizontal scroll on small screens -->
                        <div class="table-responsive" style="overflow-x: auto; white-space: nowrap;">
                            <asp:GridView ID="gvAuthor"
                                runat="server"
                                CssClass="table table-bordered table-striped align-middle mb-0 text-center"
                                AutoGenerateColumns="False"
                                OnRowCommand="gvAuthor_RowCommand"
                                AllowPaging="True"
                                PageSize="5"
                                OnPageIndexChanging="gvAuthor_PageIndexChanging"
                                PagerSettings-Visible="false">

                                <Columns>
                                    <asp:BoundField DataField="Sno" HeaderText="Sno" />
                                    <asp:TemplateField HeaderText="AuthorID" Visible="false">
                                        <ItemTemplate>
                                            <%# Eval("AuthorID") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="AuthorName" HeaderText="Author Name" />
                                    <asp:BoundField DataField="AuthorType" HeaderText="Author Type" />
                                    <asp:BoundField DataField="ActiveStatus" HeaderText="Status" />
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit"
                                                runat="server"
                                                CommandName="EditAuthor"
                                                CommandArgument='<%# Eval("AuthorID") %>'
                                                CssClass="btn btn-sm btn-primary me-2"
                                                ToolTip="Edit Author">
                                    <i class="iconly-Edit icli"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete"
                                                runat="server"
                                                CommandName="DeleteAuthor"
                                                CommandArgument='<%# Eval("AuthorID") %>'
                                                CssClass='<%# Convert.ToBoolean(Eval("Active")) 
                                                          ? "btn btn-sm btn-danger" 
                                                          : "btn btn-sm btn-secondary disabled" %>'
                                                ToolTip='<%# Convert.ToBoolean(Eval("Active")) 
                                                         ? "Click to deactivate" 
                                                         : "Already inactive" %>'
                                                OnClientClick='<%# Convert.ToBoolean(Eval("Active")) 
                                                               ? "return confirm(\"Are you sure you want to deactivate this Author?\");" 
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

            </div>



            <%--  <div id="divForm" class="card mb-0" runat="server" visible="true">
                    <div class="card-header bg-primary">
                        <h3 class="card-title mb-0">Author Details</h3>
                    </div>
                    <asp:HiddenField ID="hdnAuthorID" runat="server" />
                    <div class="card">
                        <div class="card-body mt-0">
                            <div class="row g-3 needs-validation custom-input tooltip-valid validation-forms " novalidate="">
                                <div class="col-md-4 position-relative">
                                    <label class="form-label" for="validationTooltip01">Author Name<span style="color: red">*</span></label>
                                    <asp:TextBox ID="txtAuthorName" class="form-control" Placeholder="Enter Author Name" runat="server" requried="" MaxLength="50"></asp:TextBox>
                                    <div class="valid-tooltip">Looks good!</div>
                                </div>

                                <div class="col-md-4">
                                    <label class="form-label" for="validationDefault04">Type of Author<span style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddlAuthorType" class="form-select" runat="server" requried="">
                                        <asp:ListItem Text="Choose..." Value="" Disabled="True" Selected="True" runat="server"></asp:ListItem>
                                        <asp:ListItem Text="Main Author" Value="Main" runat="server"></asp:ListItem>
                                        <asp:ListItem Text="Co-Author" Value="Co-Author" runat="server"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label d-block">Status <span style="color: red">*</span></label>
                                    <div class="form check form-check-inline">
                                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                        <label class="form-check-label ms-2" for="chkActive">Active</label>
                                    </div>
                                </div>
                                <div class="col-12 mb-0">
                                    <asp:Button ID="btnAdd" class="btn btn-primary" runat="server" Text="Save" OnClick="Submit_Click" OnClientClick="return validateInput()" />
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success" OnClick="Update_Click" OnClientClick="return validateInput()" Visible="false" />
                                    <asp:Button ID="btnClear" class="btn btn-danger" runat="server" Text="Clear" OnClick="Clear_Click" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card" id="divGrid" runat="server">
                    <div class="card-header bg-primary">
                        <h3 class="card-title mb-0 ">List Of Authors</h3>
                    </div>
                    <div class="card">
                        <div class="card-body">
                            <div class="col-md-auto d-grid">
                                <asp:Label ID="lblRecordCount" runat="server"
                                    CssClass="fw-bold text-primary"></asp:Label>

                            </div>
                            <div class="table-responsive" style="overflow-x: auto; white-space: nowrap;">


                                <asp:GridView ID="gvAuthor" runat="server"
                                    CssClass="table table-bordered table-striped"
                                    AutoGenerateColumns="False"
                                    OnRowCommand="gvAuthor_RowCommand"
                                    AllowPaging="True"
                                    PageSize="5"
                                    OnPageIndexChanging="gvAuthor_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="Sno" HeaderText="Sno" />
                                        <asp:TemplateField HeaderText="AuthorID" Visible="false">
                                            <ItemTemplate>
                                                <%# Eval("AuthorID") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AuthorName" HeaderText="Author Name" />
                                        <asp:BoundField DataField="AuthorType" HeaderText="Author Type" />
                                        <asp:BoundField DataField="ActiveStatus" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server"
                                                    CommandName="EditAuthor"
                                                    CommandArgument='<%# Eval("AuthorID") %>'
                                                    CssClass="btn btn-sm btn-primary me-2"
                                                    ToolTip="Edit Author">
                    <i class="iconly-Edit icli"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server"
                                                    CommandName="DeleteAuthor"
                                                    CommandArgument='<%# Eval("AuthorID") %>'
                                                    CssClass='<%# Convert.ToBoolean(Eval("Active")) 
                                                    ? "btn btn-sm btn-danger" 
                                                    : "btn btn-sm btn-secondary disabled" %>'
                                                    ToolTip='<%# Convert.ToBoolean(Eval("Active")) 
                                                    ? "Click to deactivate" 
                                                    : "Already inactive" %>'
                                                    OnClientClick='<%# Convert.ToBoolean(Eval("Active")) 
                                                          ? "return confirm(\"Are you sure you want to deactivate this Author?\");" 
                                                          : "return false;" %>'>
                                        <i class="iconly-Delete icli"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>


                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </div>--%>
        </div>

        <script>
            function validateInput() {


                var namePattern2 = /^[A-Za-z'. -]+$/;
                var AuthorName = document.getElementById('<%= txtAuthorName.ClientID %>').value.trim();
                var AuthorType = document.getElementById('<%= ddlAuthorType.ClientID %>').value.trim();

                // 1️⃣ Required check
                if (!AuthorName) {
                    AlertMessage("Please enter Author Name.", "error");
                    document.getElementById('<%= txtAuthorName.ClientID %>').focus();
                    return false;
                }
                if (!namePattern2.test(AuthorName)) {
                    AlertMessage("Author Name can only contain alphabets and spaces.", "error");
                    document.getElementById('<%= txtAuthorName.ClientID %>').focus();
                    return false;
                }
                if (AuthorType === "") {
                    AlertMessage("Please select a Author Type.", "error");
                    document.getElementById('<%= ddlAuthorType.ClientID %>').focus();
                    return false;
                }


                // ✅ If all checks pass
                return true;
            }
        </script>
    </form>
    <uc:footer id="Footer1" runat="server" />



</body>
</html>
