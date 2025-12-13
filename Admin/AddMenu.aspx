<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMenu.aspx.cs" Inherits="Admin.AddMenu" %>

<%@ Register Src="../Controls/Header.ascx" TagPrefix="uc" TagName="Header" %>
<%@ Register Src="../Controls/Footer.ascx" TagPrefix="uc" TagName="Footer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Menu</title>
    <link rel="icon" href="../assets/images/favicon/favicon.ico" type="image/x-icon" />
    <link rel="shortcut icon" href="../assets/images/favicon/favicon.ico" type="image/x-icon" />
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style>
    .rtl-input input[type="radio"] {
        margin-right: 6px;
    }

    .rtl-input label {
        margin-right: 16px;
        display: inline-block;
    }

    .rtl-input input[type="radio"] + label {
        margin-right: 1rem;
    }

    label {
        font-weight: 700 !important;
    }

    .col-12 {
        margin-bottom: 1rem !important;
    }
</style>

</head>      
<body>
   <uc:header id="Header" runat="server" />
    <form id="form1" runat="server">
        <br />
        <div class="page-body">
            <!-- Container-fluid starts-->
            <div class="container-fluid">
                <div class="user-profile">
                    <div class="row">
                        <div class="card hovercard">
                            <div class="info">
                                <div class="col-sm-12" id="divdegree" runat="server">
                                    <div class="card-no-border pb-2 text-start">
                                        <h3>Add Menu</h3>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col-md-3 col-12">
                                            <label>Menu Name *</label>
                                            <asp:TextBox ID="txtMenuName" runat="server" CssClass="form-control" placeholder="Enter User Name"
                                                MaxLength="80"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 col-12">
                                            <label>Status *</label>
                                            <div class="col-7">
                                                <asp:RadioButtonList ID="rbtnStatus" runat="server" CssClass="form-check-size rtl-input" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="1">Active</asp:ListItem>
                                                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mt-3">
                                        <div class="col-sm-2 col-12">
                                            <div class="checkbox checkbox-primary text-start">
                                                <asp:CheckBox ID="chkLeaf" runat="server"></asp:CheckBox>
                                                <label class="control-label">Is Child Menu</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mt-3" id="divPageName" runat="server">
                                        <div class="col-md-3 col-12">
                                            <label>Page Name *</label>
                                            <asp:TextBox ID="txtPageName" runat="server" CssClass="form-control" placeholder="Enter Password" MaxLength="80"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3 col-12">
                                            <label>Parent Name *</label>
                                            <asp:DropDownList ID="ddlParentMenu" runat="server" CssClass="form-select" />
                                        </div>
                                    </div>
                                    <div class="col-12 mt-3">
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" />
                                        <asp:Button ID="btnclear" runat="server" CssClass="btn btn-danger" Text="Clear" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="divMenudtl" runat="server">
                        <div class="card hovercard text-center">
                            <div class="info">
                                <div class="col-sm-12 col-lg-12 col-xl-10">
                                    <div class="table-responsive">
                                        <asp:DataGrid ID="dgMenu" runat="server" AllowPaging="false" CssClass="table table-responsive-sm table-bordered"
                                            AutoGenerateColumns="false">
                                            <Columns>
                                                <asp:BoundColumn HeaderText="Sr. No.">
                                                    <HeaderStyle HorizontalAlign="center" Font-Bold="true"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="MenuID" HeaderText="MenuID" Visible="false">
                                                    <HeaderStyle HorizontalAlign="left" Font-Bold="true"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="MenuName" HeaderText="Menu Name">
                                                    <HeaderStyle HorizontalAlign="left" Font-Bold="true"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="PageName" HeaderText="Page Name">
                                                    <HeaderStyle HorizontalAlign="left" Font-Bold="true"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="SequenceNo" HeaderText="Sequence No">
                                                    <HeaderStyle HorizontalAlign="left" Font-Bold="true"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="ParentMenuID" HeaderText="Parent Menu">
                                                    <HeaderStyle HorizontalAlign="left" Font-Bold="true"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="IsChildMenu" HeaderText="Is Child Menu">
                                                    <HeaderStyle HorizontalAlign="left" Font-Bold="true"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Active" HeaderText="Status">
                                                    <HeaderStyle HorizontalAlign="left" Font-Bold="true"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMenuID" runat="server" <%--Text='<%# Eval("MenuID") %>'--%> Visible="false" />
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit"
                                                            class="btn btn-info btn-sm mr-1 mb-2"><i class="fa-regular fa-pen-to-square"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete"
                                                            class="btn btn-danger btn-sm mb-2 " OnClientClick="return confirm('Are you sure you want to delete this item?');"><i class="fa-solid fa-trash"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <uc:footer id="Footer1" runat="server" />
</body>
<%--<script type="text/javascript">
    $(document).ready(function () {
        if (!$('#chkLeaf').is(":checked")) {
            $('#divPageName').hide();
        }
        else {
            $('#divPageName').show();
        }
    });
    $('#chkLeaf').change(function () {
        if ($(this).is(":checked")) {
            $('#divPageName').show();
        }
        else {
            $('#divPageName').hide();
        }
    });
    $('#btnSave').click(function () {
        var leaf = $('input[name=chkLeaf]').is(':Checked') == true ? 1 : 0;
        var mname = $('#txtMenuName').val();
        if (mname == '') {
            AlertMessage('<%=lblErrorMsg[0]%>', 'error');
            $('#txtMenuName').focus();
            return false;
        }
        var tstatus = $('input[name=rbtnStatus]').is(':Checked') == true ? 1 : 0;
        if (tstatus == 0) {
            AlertMessage('<%=lblErrorMsg[1]%>', 'error');
            $('input[name=rdlActiveStatus]').focus();
            return false;
        }
        if (leaf == 1) {
            var pname = $('#txtPageName').val();
            if (pname == '') {
                AlertMessage('<%=lblErrorMsg[2]%>', 'error');
                $('#txtPageName').focus();
                return false;
            }
            var FileName = [];
            FileName = $('#txtPageName').val().split('.');
            if (FileName[1] != 'aspx' || FileName[1] == undefined) {
                AlertMessage('<%=lblErrorMsg[4]%>', 'error');
                $('#txtPageName').focus();
                return false;
            }
            var parentname = $('#ddlParentMenu').val();
            if (parentname == '--Select--' || parentname == null || parentname == '0') {
                AlertMessage('<%=lblErrorMsg[3]%>', 'error');
                $('#ddlParentMenu').focus();
                return false;
            }

            return true;
        }
        return true;
    });
</script>--%>
</html>
