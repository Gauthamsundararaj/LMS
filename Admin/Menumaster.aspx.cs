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
    public partial class Menumaster : System.Web.UI.Page
    {
        private readonly AdminBO objAdminBO = new AdminBO();
        private string[] lblErrorMsg = new string[20];
        int intAdminUserID;

        protected void Page_Load(object sender, EventArgs e)
        {
            intAdminUserID = Convert.ToInt32(Session["AdminUserID"]);
            try
            {
                ErrorLog();

                if (!IsPostBack)
                {
                  
                    parentMenuDiv.Visible = false;
                    BindGrid();
                    BindParentMenus();
                }

            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[9], "error");
            }
        }
        
        private void ErrorLog()
        {
            try
            {
                
                lblErrorMsg[0] = CommonFunction.GetErrorMessage("", "ERRMENU001"); // Menu Name is required.
                lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "ERRMENU002"); // Invalid Menu Name.
                lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "ERRMENU005"); // Active missing.
                lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "ERRMENU014"); // Duplicate (Menu name already exists.)
                lblErrorMsg[4] = CommonFunction.GetErrorMessage("", "ERRMENU016"); // Unexpected error
                lblErrorMsg[5] = CommonFunction.GetErrorMessage("", "ERRMENU015"); // Menu not found
                lblErrorMsg[6] = CommonFunction.GetErrorMessage("", "ERRMENU012"); // Menu updated successfully (used as generic)
                lblErrorMsg[7] = CommonFunction.GetErrorMessage("", "ERRMENU013"); // Menu deleted successfully (used as generic)
                lblErrorMsg[8] = CommonFunction.GetErrorMessage("", "ERRMENU011"); // Menu added successfully (used as generic)
                lblErrorMsg[9] = CommonFunction.GetErrorMessage("", "ERRMENU016"); // General/unexpected error fallback
                lblErrorMsg[10]= CommonFunction.GetErrorMessage("", "ERRMENU004"); // page name error
                lblErrorMsg[11]= CommonFunction.GetErrorMessage("", "ERRMENU008"); // page name error 
                lblErrorMsg[12]= CommonFunction.GetErrorMessage("", "ERRMENU017"); // Select SearchBy
                lblErrorMsg[13]= CommonFunction.GetErrorMessage("", "ERRMENU018"); // Format invalid for MenuName
                lblErrorMsg[14]= CommonFunction.GetErrorMessage("", "ERRMENU019"); // Format invalid for PageName
                lblErrorMsg[15]= CommonFunction.GetErrorMessage("", "ERRMENU020"); // Search Cleared
                lblErrorMsg[16]= CommonFunction.GetErrorMessage("", "ERRMENU021"); // Enter Search Value.
                lblErrorMsg[17]= CommonFunction.GetErrorMessage("", "ERRMENU022"); // No Record Found
                lblErrorMsg[18]= CommonFunction.GetErrorMessage("", "ERRMENU023"); // Select Parent Menu.
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[9], "error");
            }
        }

        protected void chkIsActive_CheckedChanged(object sender, EventArgs e)
        {
            // If you need to handle anything when active checkbox changes
            bool isActive = chkIsActive.Checked;
        }


        protected void chkIsChild_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                parentMenuDiv.Visible = chkIsChild.Checked;

                if(!chkIsChild.Checked)
                {
                        ddlParentMenu.SelectedValue="0";
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[9] + ": " + ex.Message, "error");
            }
        }

        private int GetNextSequenceNo(int parentMenuID)
        {
            DataTable dt = objAdminBO.MenuMaster ("SELECT_CHILD", 0, "", "", parentMenuID, true, 0, true, intAdminUserID ).Tables[0];

            if (dt.Rows.Count == 0)
                return 1;

            return dt.Rows.Count + 1;
        }

        private int GetNextParentSequence()
        {
            DataTable dt = objAdminBO.MenuMaster("SELECT_PARENT", 0, "", "", 0, false, 0, true, intAdminUserID).Tables[0];

            if (dt.Rows.Count == 0)
                return 1;

            return dt.Rows.Count + 1;
        }


        #region Bind Methods
        private void BindParentMenus()
        {
            try
            {
                ddlParentMenu.Items.Clear();
                ddlParentMenu.Items.Add(new ListItem("-- Select Parent Menu --", "0"));

                DataSet ds = objAdminBO.MenuMaster ("SELECT_PARENT", 0, "", "", 0, false, 0, true, intAdminUserID);

                DataTable dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    ddlParentMenu.Items.Add(new ListItem(
                        dr["MenuName"].ToString(),
                        dr["MenuID"].ToString()
                    ));
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[4] + ": " + ex.Message, "error");
            }
        }


        private void BindGrid()
        {
            try
            {
                string searchBy = ddlSearchBy.SelectedValue;
                string searchValue = txtSearchValue.Text.Trim();

                DataTable dt;


                if (!string.IsNullOrEmpty(searchBy) && !string.IsNullOrEmpty(searchValue))
                {
                    dt = objAdminBO.SearchMenuMaster(searchBy, searchValue);
                }
                else
                {
                    dt = objAdminBO.GetMenuMasterGrid();
                }

                gvMenu.DataSource = dt;
                gvMenu.DataBind();
                lblRecordCount.Text = dt.Rows.Count + " Records found";
                int totalRecords = dt.Rows.Count;
                int pageSize = gvMenu.PageSize;
                int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                BuildPager(totalPages, gvMenu.PageIndex);
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[4] + ": " + ex.Message, "error");
            }
        }
        #endregion

        #region Insert / Update
        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string menuName = txtMenuName.Text.Trim();
        //        string pageName = txtPageName.Text.Trim();
        //        bool isChild = chkIsChild.Checked;
        //        int parentId = 0;
        //        int.TryParse(ddlParentMenu.SelectedValue, out parentId);
        //        bool isActive = chkIsActive.Checked;


        //        if (string.IsNullOrWhiteSpace(menuName))
        //        {
        //            ShowToastr(lblErrorMsg[0], "error");
        //            return;
        //        }

        //        if (!Regex.IsMatch(menuName, @"^[A-Za-z\s]+$"))
        //        {
        //            ShowToastr(lblErrorMsg[1], "error");
        //            return;
        //        }

        //        if (isChild)
        //        {
        //            if (string.IsNullOrWhiteSpace(pageName))
        //            {
        //                ShowToastr(lblErrorMsg[11], "error"); // Please enter Page Name
        //                return;
        //            }

        //            if (!Regex.IsMatch(pageName, @"^[A-Za-z0-9_]+\.aspx$", RegexOptions.IgnoreCase))
        //            {
        //                ShowToastr(lblErrorMsg[10], "error"); // Invalid Page Name
        //                return;
        //            }

        //            if (parentId==0)
        //            {
        //                ShowToastr(lblErrorMsg[18], "error"); //Select Parent Menu
        //                return;
        //            }

        //        }
        //        else
        //        {
        //            if (txtPageName.Text.Length > 0)
        //            {
        //                if (!Regex.IsMatch(txtPageName.Text.Trim(), @"^[A-Za-z0-9_]+\.aspx$", RegexOptions.IgnoreCase))
        //                {
        //                    ShowToastr(lblErrorMsg[10], "error");
        //                    return;
        //                }
        //            }

        //        }

        //        int sequenceNo;

        //        if (isChild)
        //        {
        //            sequenceNo = GetNextSequenceNo(parentId);
        //        }
        //        else
        //        {
        //            sequenceNo = GetNextParentSequence();
        //        }
        //        int createdBy = Convert.ToInt32(Session["UserName"] != null ? Session["UserName"].ToString() : "0");

        //        DataSet ds = objAdminBO.MenuMaster ("INSERT", 0, menuName, pageName, parentId, isChild, sequenceNo, isActive, intAdminUserID);

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
        //            string message = ds.Tables[0].Rows[0]["Message"].ToString();

        //            if (msgCode == 11 || msgCode == 1)
        //            {
        //                ClearFields();
        //                BindGrid();
        //                BindParentMenus();
        //                ShowToastr(message ?? lblErrorMsg[8], "success");
        //            }
        //            else if (msgCode == 14 || msgCode == 2)
        //            {
        //                ShowToastr(lblErrorMsg[3], "warning");
        //            }
        //            else
        //            {
        //                ShowToastr(message ?? lblErrorMsg[4], "warning");
        //            }
        //        }
        //        else
        //        {
        //            ShowToastr(lblErrorMsg[4], "error");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyExceptionLogger.Publish(ex);
        //        ShowToastr(lblErrorMsg[9] + ": " + ex.Message, "error");
        //    }
        //}
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string menuName = txtMenuName.Text.Trim();
                string pageName = txtPageName.Text.Trim();
                bool isChild = chkIsChild.Checked;
                bool isActive = chkIsActive.Checked;

                int parentId = Convert.ToInt32(ddlParentMenu.SelectedValue);

                // ===== VALIDATION =====
                if (string.IsNullOrWhiteSpace(menuName))
                {
                    ShowToastr(lblErrorMsg[0], "error");
                    return;
                }

                if (!Regex.IsMatch(menuName, @"^[A-Za-z\s]+$"))
                {
                    ShowToastr(lblErrorMsg[1], "error");
                    return;
                }

                if (isChild)
                {
                    if (string.IsNullOrWhiteSpace(pageName))
                    {
                        ShowToastr(lblErrorMsg[11], "error");
                        return;
                    }

                    if (parentId == 0)
                    {
                        ShowToastr(lblErrorMsg[18], "error");
                        return;
                    }
                }

                int sequenceNo = isChild
                    ? GetNextSequenceNo(parentId)
                    : GetNextParentSequence();

                DataSet ds = objAdminBO.MenuMaster(
                    "INSERT",
                    0,
                    menuName,
                    pageName,
                    parentId,
                    isChild,
                    sequenceNo,
                    isActive,
                    intAdminUserID
                );

                int resultCode = Convert.ToInt32(ds.Tables[0].Rows[0]["ResultCode"]);

                switch (resultCode)
                {
                    case 0:
                        ClearFields();
                        BindGrid();
                        BindParentMenus();
                        ShowToastr(lblErrorMsg[8], "success"); // Menu added
                        break;

                    case 1:
                        ShowToastr(lblErrorMsg[0], "error");
                        break;

                    case 2:
                        ShowToastr(lblErrorMsg[3], "warning");
                        break;

                    default:
                        ShowToastr(lblErrorMsg[4], "error");
                        break;
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[9], "error");
            }
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(hfMenuID.Value))
                {
                    ShowToastr("No record selected for update.", "error");
                    return;
                }

                string menuName = txtMenuName.Text.Trim();
                string pageName = txtPageName.Text.Trim();
                bool isChild = chkIsChild.Checked;
                int parentId = 0;
                int.TryParse(ddlParentMenu.SelectedValue, out parentId);
                bool isActive = chkIsActive.Checked;
                int menuId = Convert.ToInt32(hfMenuID.Value);

                if (string.IsNullOrWhiteSpace(menuName))
                {
                    ShowToastr(lblErrorMsg[0], "error");
                    return;
                }

                if (!Regex.IsMatch(menuName, @"^[A-Za-z\s]+$"))
                {
                    ShowToastr(lblErrorMsg[1], "error");
                    return;
                }

                if (isChild)
                {
                    if (string.IsNullOrWhiteSpace(pageName))
                    {
                        ShowToastr(lblErrorMsg[11], "error"); // Please enter Page Name
                        return;
                    }

                    if (!Regex.IsMatch(pageName, @"^[A-Za-z0-9_]+\.aspx$", RegexOptions.IgnoreCase))
                    {
                        ShowToastr(lblErrorMsg[10], "error"); // Invalid Page Name
                        return;
                    }

                    if (parentId==0)
                    {
                        ShowToastr(lblErrorMsg[18], "error");  //Select Parent Menu
                        return;
                    }
                }
                else
                {

                    if (txtPageName.Text.Length > 0)
                    {
                        if (!Regex.IsMatch(txtPageName.Text.Trim(), @"^[A-Za-z0-9_]+\.aspx$", RegexOptions.IgnoreCase))
                        {
                            ShowToastr(lblErrorMsg[10], "error");
                            return;
                        }
                    }
                }

                int sequenceNo;

                if (isChild)
                {
                    sequenceNo = GetNextSequenceNo(parentId);
                }
                else
                {
                    sequenceNo = GetNextParentSequence();
                }
                string modifiedBy = Session["UserName"] != null ? Session["UserName"].ToString() : "";

                DataSet ds = objAdminBO.MenuMaster(
    "UPDATE",
    menuId,
    menuName,
    pageName,
    parentId,
    isChild,
    sequenceNo,
    isActive,
    intAdminUserID
);

                int resultCode = Convert.ToInt32(ds.Tables[0].Rows[0]["ResultCode"]);

                switch (resultCode)
                {
                    case 0:
                        ClearFields();
                        BindGrid();
                        BindParentMenus();
                        btnSubmit.Visible = true;
                        btnUpdate.Visible = false;
                        ShowToastr(lblErrorMsg[6], "success"); // updated
                        break;

                    case 2:
                        ShowToastr(lblErrorMsg[3], "warning"); // duplicate
                        break;

                    case 3:
                        ShowToastr(lblErrorMsg[5], "error"); // not found
                        break;

                    default:
                        ShowToastr(lblErrorMsg[4], "error");
                        break;
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[6] + ": " + ex.Message, "error");
            }
        }
        #endregion

        #region Grid Events
        protected void gvMenu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMenu.PageIndex = e.NewPageIndex; 
            BindGrid();
 
        }

        protected void gvMenu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditMenu")
            {
                try
                {
                    int menuId = Convert.ToInt32(e.CommandArgument);

                    DataTable dt = objAdminBO.GetMenuByID(menuId);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];

                        hfMenuID.Value = dr["MenuID"].ToString();
                        txtMenuName.Text = dr["MenuName"].ToString();
                        txtPageName.Text = dr["PageName"].ToString();

                        chkIsChild.Checked = Convert.ToBoolean(dr["IsChildMenu"]);
                        parentMenuDiv.Visible = chkIsChild.Checked;

                        string parentValue = dr["ParentMenuID"] != DBNull.Value ? dr["ParentMenuID"].ToString() : "0";
                        if (ddlParentMenu.Items.FindByValue(parentValue) != null)
                            ddlParentMenu.SelectedValue = parentValue;
                        else
                            ddlParentMenusFallbackSelect(parentValue);

                        chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"]);


                        btnSubmit.Visible = false;
                        btnUpdate.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MyExceptionLogger.Publish(ex);
                    ShowToastr(lblErrorMsg[5] + ": " + ex.Message, "error");
                }
            }
        }

        protected void gvMenu_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int menuId = Convert.ToInt32(gvMenu.DataKeys[e.RowIndex].Value);

                DataSet ds = objAdminBO.MenuMaster("DELETE", menuId);

                int resultCode = Convert.ToInt32(ds.Tables[0].Rows[0]["ResultCode"]);

                if (resultCode == 0)
                {
                    BindGrid();
                    ShowToastr(lblErrorMsg[7], "success");
                }
                else if (resultCode == 3)
                {
                    ShowToastr(lblErrorMsg[5], "error");
                }
                else
                {
                    ShowToastr(lblErrorMsg[4], "error");
                }

            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[9] + ": " + ex.Message, "error");
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchBy = ddlSearchBy.SelectedValue;
                string searchValue = txtSearchValue.Text.Trim();

                if (string.IsNullOrEmpty(searchBy))
                {
                    ShowToastr(lblErrorMsg[12], "warning"); 
                    return;
                }

                if (string.IsNullOrEmpty(searchValue))
                {
                    ShowToastr(lblErrorMsg[16], "error");
                    return;
                }

                if (searchBy == "MenuName")
                {
                    if (!Regex.IsMatch(searchValue, @"^[A-Za-z\s]+$"))
                    {
                        ShowToastr(lblErrorMsg[13], "error"); // Format invalid for MenuName
                        return;
                    }
                }

                if (searchBy == "PageName")
                {
                    if (!Regex.IsMatch(searchValue, @"^[A-Za-z0-9_]+\.aspx$", RegexOptions.IgnoreCase))
                    {
                        ShowToastr(lblErrorMsg[14], "error"); // Format invalid for PageName
                        return;
                    }
                }
                gvMenu.PageIndex = 0;
                BindGrid();
                lblRecordCount.Text = gvMenu.Rows.Count + " Records found";


                if (gvMenu.Rows.Count == 0)
                {
                    ShowToastr(lblErrorMsg[17], "warning"); // No Record Found.
                }

            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[9] + ": " + ex.Message, "error");
            }
        }
   

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            ddlSearchBy.SelectedIndex = 0;
            txtSearchValue.Text = "";

            gvMenu.PageIndex = 0;
            BindGrid();

        }

        protected void gvMenu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool isActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActive"));

                LinkButton deleteBtn = (LinkButton)e.Row.FindControl("btnDelete");

                if (!isActive)
                {
                    deleteBtn.Enabled = false;
                    deleteBtn.CssClass += " disabled";
                    deleteBtn.OnClientClick = "return false;";

                    deleteBtn.Style.Add("opacity", "0.4");
                    deleteBtn.Style.Add("cursor", "not-allowed");
                }
            }
        }

        #endregion
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
                ShowToastr(lblErrorMsg[9] + ": " + ex.Message, "error");
            }
        }

        private void ClearFields()
        {
            txtMenuName.Text = "";
            txtPageName.Text = "";
            chkIsActive.Checked = false;
            chkIsChild.Checked = false;
            parentMenuDiv.Visible = false;
            if (ddlParentMenu.Items.FindByValue("0") != null)
                ddlParentMenu.SelectedValue = "0";
            hfMenuID.Value = "";
        }

        private void ddlParentMenusFallbackSelect(string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ddlParentMenu.Items.Add(new ListItem("Parent (ID:" + value + ")", value));
                    ddlParentMenu.SelectedValue = value;
                }
            }
            catch { }
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

            gvMenu.PageIndex = newIndex;
            BindGrid();
        }
        private void ShowToastr(string message, string type)
        {
            message = (message ?? "").Replace("'", "\\'").Replace("\"", "\\\"").Replace(Environment.NewLine, " ").Trim();
            ScriptManager.RegisterStartupScript(this.Page, GetType(), Guid.NewGuid().ToString(), "$(function(){AlertMessage('" + message + "','" + type.ToLower() + "')});", true);
        }
    }
}
