using BLL;
using Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class RoleMenuMapping : System.Web.UI.Page
    {
        AdminBO objBO = new AdminBO();
        private string[] lblErrorMsg = new string[20];
        int adminUserId;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog();
                adminUserId =  Convert.ToInt32(Session["AdminUserID"] ?? 0);

                if (!IsPostBack)
                {
                    LoadRoleTypes();
                    pnlMenuList.Visible=false;
                }
            }
            catch (Exception ex)
            {
                ShowToastr(CommonFunction.GetErrorMessage("", "ERR999") + " " + ex.Message, "error");
            }
        }

        private void ErrorLog()
        {
            try
            {
                lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "ERRMENU016"); //Unexpected error occurred
                lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "ERRMENU022"); //No record Found
                lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "ERRRRMM005"); //Role Menu Updated Successfully.
                
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[1], "error");
            }
        }
        private void LoadRoleTypes()
        {
            try
            {
                using (DataSet ds = objBO.GetRolesForDropdown())
                {
                    ddlRoleType.Items.Clear();

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlRoleType.DataSource = ds.Tables[0];
                        ddlRoleType.DataTextField = "UserRole"; // column from RoleMaster
                        ddlRoleType.DataValueField = "RoleID";
                        ddlRoleType.DataBind();
                    }

                    ddlRoleType.Items.Insert(0,
                        new ListItem("-- Select Role --", "0"));
                }
            }
            catch (Exception ex)
            {
                ShowToastr(lblErrorMsg[1] + " " + ex.Message, "error");
            }
        }

        

        protected void ddlRoleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvRoleMenu.PageIndex = 0;
                if (ddlRoleType.SelectedValue == "0")
                {
                    gvRoleMenu.DataSource = null;
                    gvRoleMenu.DataBind();
                    pnlMenuList.Visible=false;
                    return;
                }
                pnlMenuList.Visible=true;
                LoadMenuByRole();
            }
            catch (Exception ex)
            {
                ShowToastr(lblErrorMsg[1] + " " + ex.Message, "error");
            }
        }

        private void LoadMenuByRole()
        {
            try
            {
                if (ddlRoleType.SelectedValue=="0")
                {
                    pnlMenuList.Visible=false;
                    return;
                }

                int roleID = Convert.ToInt32(ddlRoleType.SelectedValue);
                using (DataSet ds = objBO.SearchRoleMenu(roleID))
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        gvRoleMenu.DataSource = ds;
                        gvRoleMenu.DataBind();
                        lblRecordCount.Text = ds.Tables[0].Rows.Count + " Records found";
                        int totalRecords = ds.Tables[0].Rows.Count;
                        int pageSize = gvRoleMenu.PageSize;
                        int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                        if (ds.Tables[0].Rows.Count > gvRoleMenu.PageSize)
                        {
                            BuildPager(totalPages, gvRoleMenu.PageIndex);
                            rptPager.Visible = true;
                        }
                        else
                        {
                            rptPager.Visible = false;
                        }
                    }
                    else
                    {
                        gvRoleMenu.DataSource = null;
                        gvRoleMenu.DataBind();
                        lblRecordCount.Text = gvRoleMenu.Rows.Count + " Records found";

                        ShowToastr(lblErrorMsg[2], "info"); //No Record Found
                    }
                    pnlMenuList.Visible=true;
                    lblRecordCount.Text = ds.Tables[0].Rows.Count + " Records found";
                    BuildPager(gvRoleMenu.PageCount, gvRoleMenu.PageIndex);
                }
            }
            catch (Exception ex)
            {
                ShowToastr(lblErrorMsg[1]+ " " + ex.Message, "error");
            }
        }

        protected void chkAllow_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                GridViewRow row = (GridViewRow)chk.NamingContainer;

                int menuID = Convert.ToInt32(gvRoleMenu.DataKeys[row.RowIndex].Value);
                int roleID = Convert.ToInt32(ddlRoleType.SelectedValue);
                int aUserID = Convert.ToInt32(Session["AUserID"]);

                int isChecked = chk.Checked ? 1 : 0;

                using (DataSet ds = objBO.SaveUpdateRoleMenu(roleID, menuID, 0, isChecked, aUserID))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        int MsgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                        {
                            ShowToastr(lblErrorMsg[3], "success");
                        }
                    }
                    LoadMenuByRole();
                    lblRecordCount.Text = gvRoleMenu.Rows.Count + " Records found";
                }
            }
            catch (Exception ex)
            {
                ShowToastr(lblErrorMsg[1] + " " + ex.Message, "error");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ddlRoleType.SelectedIndex = 0;
                gvRoleMenu.PageIndex = 0;
                gvRoleMenu.DataSource = null;
                gvRoleMenu.DataBind();
                lblRecordCount.Text = "";
                pnlMenuList.Visible=false;
            }
            catch (Exception ex)
            {
                ShowToastr(lblErrorMsg[1] + " " + ex.Message, "error");
            }
        }

        protected void gvMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRoleMenu.PageIndex = e.NewPageIndex;
            LoadMenuByRole();

        }
        private void BuildPager(int totalPages, int currentPage)
        {
            var pages = new List<object>();
            int maxPagesToShow = 3;

            int startPage = Math.Max(0, currentPage - 1);
            int endPage = Math.Min(totalPages - 1, startPage + maxPagesToShow - 1);

            if (endPage - startPage < maxPagesToShow - 1)
                startPage = Math.Max(0, endPage - maxPagesToShow + 1);

            // Previous
            pages.Add(new
            {
                PageIndex = currentPage - 1,
                Text = "« Previous",
                Command = "Page",
                Enabled = currentPage > 0,
                IsActive = false   // ✅ REQUIRED
            });

            // Page Numbers
            for (int i = startPage; i <= endPage; i++)
            {
                pages.Add(new
                {
                    PageIndex = i,
                    Text = (i + 1).ToString(),
                    Command = "Page",
                    Enabled = true,
                    IsActive = (i == currentPage) // ✅ ONLY true for active page
                });
            }

            // Next
            pages.Add(new
            {
                PageIndex = currentPage + 1,
                Text = "Next »",
                Command = "Page",
                Enabled = currentPage < totalPages - 1,
                IsActive = false   // ✅ REQUIRED
            });

            rptPager.DataSource = pages;
            rptPager.DataBind();
        }


        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int newIndex = Convert.ToInt32(e.CommandArgument);

            gvRoleMenu.PageIndex = newIndex;
            LoadMenuByRole();
        }

        private void ShowToastr(string message, string type)
        {
            message = (message ?? "").Replace("'", "\\'").Replace("\"", "\\\"").Replace(Environment.NewLine, " ").Trim();
            ScriptManager.RegisterStartupScript(this.Page, GetType(), Guid.NewGuid().ToString(), "$(function(){AlertMessage('" + message + "','" + type.ToLower() + "')});", true);
        }

    }
}
