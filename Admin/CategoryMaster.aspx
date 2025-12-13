<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CategoryMaster.aspx.cs" Inherits="Admin.CategoryMaster" %>

<%@ Register Src="../Controls/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="../Controls/Footer.ascx" TagPrefix="uc" TagName="Footer" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CategoryMenu</title>

</head>
<body>

    <uc:header id="Header" runat="server" />
    <form id="form1" runat="server">
        <div class="page-body">
            <div class="container-fluid mt-2">
                <div id="divForm" class="card mb-0" runat="server" visible="true">
                    <div class="card-header bg-primary">
                        <h3 class="card-title mb-0">Category Details</h3>
                    </div>
                    <asp:HiddenField ID="hdnCategoryID" runat="server" />
                    <div class="card">
                        <div class="card-body mt-0">
                            <div class="row g-3 needs-validation custom-input tooltip-valid validation-forms" novalidate="">
                                <div class="col-md-5 position-relative ">
                                    <div>
                                        <label class="form-label" for="validationTooltip01">Category Name<span style="color: red">*</span></label>
                                        <asp:TextBox ID="txtCategoryName" class="form-control" Placeholder="Enter Category Name" runat="server" requried="" MaxLength="50"></asp:TextBox>
                                        <div class="valid-tooltip">Looks good!</div>
                                    </div>
                                    <div class="mt-3">
                                        <label class="form-label d-block">Status <span style="color: red">*</span></label>
                                        <div class="form check form-check-inline">
                                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                                            <label class="form-check-label ms-2" for="chkActive">Active</label>
                                        </div>
                                    </div>
                                </div>


                                <div class="col-7">
                                    <label class="form-label" for="exampleFormControlTextarea1">Description</label>
                                    <textarea class="form-control" id="txtDescription" runat="server" rows="4" required=""></textarea>
                                </div>
                            </div>

                            <div class="col-12 mb-0 mt-1">
                                <asp:Button ID="btnAdd" class="btn btn-primary" runat="server" Text="Save" OnClick="Submit_Click" OnClientClick="return validateInput()" />
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-success" OnClick="Update_Click" OnClientClick="return validateInput()" Visible="false" />
                                <asp:Button ID="btnClear" class="btn btn-danger" runat="server" Text="Clear" OnClick="Clear_Click" />

                            </div>
                        </div>
                    </div>
                </div>


                <div class="card" id="divGrid" runat="server">
                    <div class="card-header bg-primary">
                        <h4 class="card-title mb-0 p-0">List Of Categories</h4>
                    </div>
                    <div class="card">
                        <div class="card-body">
                            <div class="col-md-auto d-grid">
                                <asp:Label ID="lblRecordCount" runat="server"
                                    CssClass="fw-bold text-primary"></asp:Label>

                            </div>
                            <div class="table-responsive" style="overflow-x: auto; white-space: nowrap;">
                                <asp:GridView ID="gvCategory" runat="server"
                                    CssClass="table table-bordered table-striped"
                                    AutoGenerateColumns="False"
                                    OnRowCommand="gvCategory_RowCommand"
                                    AllowPaging="True"
                                    PageSize="5"
                                    OnPageIndexChanging="gvCategory_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="Sno" HeaderText="Sno" />
                                        <asp:TemplateField HeaderText="CategoryID" Visible="false">
                                            <ItemTemplate>
                                                <%# Eval("CategoryID") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CategoryName" HeaderText="Category Name" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" />
                                        <asp:BoundField DataField="ActiveStatus" HeaderText="Status" />
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server"
                                                    CommandName="EditCategory"
                                                    CommandArgument='<%# Eval("CategoryID") %>'
                                                    CssClass="btn btn-sm btn-primary me-2"
                                                    ToolTip="Edit Category">
                    <i class="iconly-Edit icli"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server"
                                                    CommandName="DeleteCategory"
                                                    CommandArgument='<%# Eval("CategoryID") %>'
                                                    CssClass='<%# Convert.ToBoolean(Eval("Active")) 
                                ? "btn btn-sm btn-danger" 
                                : "btn btn-sm btn-secondary disabled" %>'
                                                    ToolTip='<%# Convert.ToBoolean(Eval("Active")) 
                                ? "Click to deactivate" 
                                : "Already inactive" %>'
                                                    OnClientClick='<%# Convert.ToBoolean(Eval("Active")) 
                                      ? "return confirm(\"Are you sure you want to deactivate this Category?\");" 
                                      : "return false;" %>'>
                    <i class="iconly-Delete icli"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerTemplate>
                                        <div style="text-align: center; padding: 10px;">

                                            <asp:LinkButton ID="lnkPrev" runat="server"
                                                CommandName="Page" CommandArgument="Prev"
                                                CssClass="btn btn-primary btn-sm">
                                                     Previous
                                            </asp:LinkButton>

                                            <span style="padding: 0 15px;">Page <%# gvCategory.PageIndex + 1 %> of <%# gvCategory.PageCount %>
                                            </span>

                                            <asp:LinkButton ID="lnkNext" runat="server"
                                                CommandName="Page" CommandArgument="Next"
                                                CssClass="btn btn-primary btn-sm">
                                                     Next
                                            </asp:LinkButton>

                                        </div>
                                    </PagerTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <script>
            function validateInput() {
                var namePattern2 = /^[A-Za-z' -]+$/;
                var descPattern = /^[A-Za-z0-9 .,()'":;!?-]+$/;
                var CategoryName = document.getElementById('<%= txtCategoryName.ClientID %>').value.trim();
                var Description = document.getElementById('<%= txtDescription.ClientID %>').value.trim();

                // 1️⃣ Required check
                if (!CategoryName) {
                    AlertMessage("Please enter Category Name.", "error");
                    document.getElementById('<%= txtCategoryName.ClientID %>').focus();
                    return false;
                }
                if (!namePattern2.test(CategoryName)) {
                    AlertMessage("Category Name can only contain alphabets and spaces.", "error");
                    document.getElementById('<%= txtCategoryName.ClientID %>').focus();
                    return false;
                }
                // 1️⃣ Required
                if (description === "") {
                    AlertMessage("Please enter a Description.", "error");
                    document.getElementById('<%= txtDescription.ClientID %>').focus();
                    return false;
                }
                // 2️⃣ Minimum length
                if (description.length < 5) {
                    AlertMessage("Description must be at least 5 characters long.", "error");
                    document.getElementById('<%= txtDescription.ClientID %>').focus();
                    return false;
                }
                // 3️⃣ Maximum length
                if (description.length > 500) {
                    AlertMessage("Description cannot exceed 500 characters.", "error");
                    document.getElementById('<%= txtDescription.ClientID %>').focus();
                    return false;
                }
                // 4️⃣ Pattern check
                if (!descPattern.test(description)) {
                    AlertMessage("Description contains invalid characters.", "error");
                    document.getElementById('<%= txtDescription.ClientID %>').focus();
                    return false;
                }
                return true;
            }
        </script>
    </form>

    <uc:footer id="Footer1" runat="server" />


</body>
</html>
