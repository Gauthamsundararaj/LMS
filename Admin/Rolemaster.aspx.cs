using BLL;
using Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class RoleMaster : System.Web.UI.Page
    {
        private readonly AdminBO objAdminBO = new AdminBO();
        private string[] lblErrorMsg = new string[15];

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorLog();
                if (!IsPostBack)
                {
                    BindGrid();
                    chkActive.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[10], "error");
            }
        }
        private void ErrorLog()
        {
            try
            {
                lblErrorMsg[0] = CommonFunction.GetErrorMessage("", "ERRROLE001"); // Empty role
                lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "ERRROLE002"); // Invalid format
                lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "ERRROLE003"); // Active missing
                lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "ERRROLE004"); // Already exists
                lblErrorMsg[4] = CommonFunction.GetErrorMessage("", "ERRROLE005"); // Unexpected DB response
                lblErrorMsg[5] = CommonFunction.GetErrorMessage("", "ERRROLE006"); // Load error
                lblErrorMsg[6] = CommonFunction.GetErrorMessage("", "ERRROLE007"); // Update error
                lblErrorMsg[7] = CommonFunction.GetErrorMessage("", "ERRROLE008"); // Form cleared  ✅ FIXED
                lblErrorMsg[8] = CommonFunction.GetErrorMessage("", "ERRROLE009"); // Role deleted successfully  ✅ FIXED
                lblErrorMsg[9] = CommonFunction.GetErrorMessage("", "ERRROLE010"); // General error  ✅ FIXED
                lblErrorMsg[10] = CommonFunction.GetErrorMessage("", "ERRROLE011"); //Role Added Successfully.

            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[10], "error");
            }
        }
        protected void gvMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRoleMaster.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string strRoleName = txtRoleName.Text.Trim();

                // Validation
                if (string.IsNullOrWhiteSpace(strRoleName))
                {
                    ShowToastr(lblErrorMsg[0], "error");
                    return;
                }

                if (!Regex.IsMatch(strRoleName, @"^[A-Za-z\s]+$"))
                {
                    ShowToastr(lblErrorMsg[1], "error");
                    return;
                }

                bool isActive = chkActive.Checked;
                DataSet ds = objAdminBO.GetRoleMaster("ADD", 0, strRoleName, isActive);

                int resultCode = Convert.ToInt32(ds.Tables[0].Rows[0]["ResultCode"]);

                switch (resultCode)
                {
                    case 0:
                        ClearFields();
                        BindGrid();
                        ShowToastr(lblErrorMsg[10], "success"); // Role added
                        break;

                    case 1:
                        ShowToastr(lblErrorMsg[0], "error"); // Empty role
                        break;

                    case 2:
                        ShowToastr(lblErrorMsg[3], "warning"); // Duplicate
                        break;

                    default:
                        ShowToastr(lblErrorMsg[4], "error"); // Unexpected
                        break;
                }
            }

            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[10] + ": " + ex.Message, "error");
            }
        }

        protected void gvRoleMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRole")
            {
                try
                {
                    int roleId = Convert.ToInt32(e.CommandArgument);

                    // ✅ CALL SELECT_BY_ID (NOT DELETE)
                    DataSet ds = objAdminBO.GetRoleMaster("SELECT_BY_ID", roleId);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        hfRoleID.Value = dr["RoleID"].ToString();
                        txtRoleName.Text = dr["UserRole"].ToString();
                        chkActive.Checked = Convert.ToBoolean(dr["Active"]);
                        btnSubmit.Visible = false;
                        btnUpdate.Visible = true;
                    }
                    else
                    {
                        ShowToastr(lblErrorMsg[5], "error");
                    }
                }
                catch (Exception ex)
                {
                    MyExceptionLogger.Publish(ex);
                    ShowToastr(lblErrorMsg[5] + ": " + ex.Message, "error");
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(hfRoleID.Value))
                {
                    ShowToastr("No record selected for update.", "error");
                    return;
                }

                string strRoleName = txtRoleName.Text.Trim();

                if (string.IsNullOrWhiteSpace(strRoleName))
                {
                    ShowToastr(lblErrorMsg[0], "error");
                    return;
                }

                if (!Regex.IsMatch(strRoleName, @"^[A-Za-z\s]+$"))
                {
                    ShowToastr(lblErrorMsg[1], "error");
                    return;
                }

                bool isActive = chkActive.Checked;
                int roleId = Convert.ToInt32(hfRoleID.Value);

                DataSet ds = objAdminBO.GetRoleMaster("UPDATE",roleId,strRoleName,isActive);


                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                    string message = ds.Tables[0].Rows[0]["Message"].ToString();

                    if (msgCode == 1)
                    {
                        ClearFields();
                        BindGrid();
                        btnSubmit.Visible = true;
                        btnUpdate.Visible = false;
                        ShowToastr(message, "success");
                    }
                    else if (msgCode == 2)
                    {
                        ShowToastr(lblErrorMsg[3], "warning"); // duplicate
                    }
                    else
                    {
                        ShowToastr(message, "warning");
                    }
                }
                else
                {
                    ShowToastr(lblErrorMsg[4], "error");
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[6] + ": " + ex.Message, "error");
            }
        }

        protected void gvRoleMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int roleId = Convert.ToInt32(gvRoleMaster.DataKeys[e.RowIndex].Value);
                DataSet ds = objAdminBO.GetRoleMaster("DELETE", roleId);
                int resultCode = Convert.ToInt32(ds.Tables[0].Rows[0]["ResultCode"]);

                if (resultCode == 0)
                {
                    BindGrid();
                    ShowToastr(lblErrorMsg[8], "success");
                }
                else if (resultCode == 3)
                {
                    ShowToastr(lblErrorMsg[5], "error"); // Not found
                }
                else
                {
                    ShowToastr(lblErrorMsg[7], "error");
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[7] + ": " + ex.Message, "error");
            }
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFields();

                btnSubmit.Visible = true;
                btnUpdate.Visible = false;

            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[10] + ": " + ex.Message, "error");
            }
        }

        private void ClearFields()
        {
            txtRoleName.Text = "";
            chkActive.Checked = true;
            hfRoleID.Value = "";
        }

        private void BindGrid()
        {
            try
            {
                DataSet ds = objAdminBO.GetRoleMaster("SELECT");

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    gvRoleMaster.DataSource = dt;
                    gvRoleMaster.DataBind();

                    lblRecordCount.Text = dt.Rows.Count + " Records found";

                    int totalRecords = dt.Rows.Count;
                    int pageSize = gvRoleMaster.PageSize;
                    int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                    BuildPager(totalPages, gvRoleMaster.PageIndex);
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[4], "error");
            }
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

            gvRoleMaster.PageIndex = newIndex;
            BindGrid();
        }

        private void ShowToastr(string message, string type)
        {
            message = (message ?? "").Replace("'", "\\'").Replace("\"", "\\\"").Replace(Environment.NewLine, " ").Trim();
            ScriptManager.RegisterStartupScript(this.Page, GetType(), Guid.NewGuid().ToString(), "$(function(){AlertMessage('" + message + "','" + type.ToLower() + "')});", true);
        }

    }
}
