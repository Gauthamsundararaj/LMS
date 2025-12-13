using BLL;
using Library;
using System;
using System.Data;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace Admin
{
    public partial class UserMaster : System.Web.UI.Page
    {
        AdminBO objAdminBO = new AdminBO();
        private string[] lblErrorMsg = new string[10];
        int intAdminUserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                intAdminUserID = Convert.ToInt32(Session["AdminUserID"] ?? 0);
                if (!Page.IsPostBack)
                {
                    BindRoleType();
                    BindUserGrid();

                    //divForm.Style["display"] = "none";
                    //divGrid.Style["display"] = "block";
                }

                // Load all user messages
                lblErrorMsg = new string[30];
                lblErrorMsg[0] = CommonFunction.GetErrorMessage("", "ERRUSR001");
                lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "ERRUSR002");
                lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "ERRUSR003");
                lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "ERRUSR004");
                lblErrorMsg[4] = CommonFunction.GetErrorMessage("", "ERRUSR005");
                lblErrorMsg[5] = CommonFunction.GetErrorMessage("", "ERRUSR006");
                lblErrorMsg[6] = CommonFunction.GetErrorMessage("", "ERRUSR007");
                lblErrorMsg[7] = CommonFunction.GetErrorMessage("", "ERRUSR008");
                lblErrorMsg[8] = CommonFunction.GetErrorMessage("", "ERRUSR009");
                lblErrorMsg[9] = CommonFunction.GetErrorMessage("", "ERRUSR010");
                lblErrorMsg[10] = CommonFunction.GetErrorMessage("", "ERRUSR011");
                lblErrorMsg[11] = CommonFunction.GetErrorMessage("", "ERRUSR012");
                lblErrorMsg[12] = CommonFunction.GetErrorMessage("", "ERRUSR013");
                lblErrorMsg[13] = CommonFunction.GetErrorMessage("", "ERRUSR014");
                lblErrorMsg[14] = CommonFunction.GetErrorMessage("", "ERRUSR015");
                lblErrorMsg[15] = CommonFunction.GetErrorMessage("", "ERRUSR016");
                lblErrorMsg[16] = CommonFunction.GetErrorMessage("", "ERRUSR017");
                lblErrorMsg[17] = CommonFunction.GetErrorMessage("", "ERRUSR018");
                lblErrorMsg[18] = CommonFunction.GetErrorMessage("", "ERRUSR019");
                lblErrorMsg[19] = CommonFunction.GetErrorMessage("", "ERRUSR020");
                lblErrorMsg[20] = CommonFunction.GetErrorMessage("", "ERRUSR021");
                lblErrorMsg[21] = CommonFunction.GetErrorMessage("", "ERRUSR022");
                lblErrorMsg[22] = CommonFunction.GetErrorMessage("", "SUCUSR001");
                lblErrorMsg[23] = CommonFunction.GetErrorMessage("", "INFOUSR01");
                lblErrorMsg[24] = CommonFunction.GetErrorMessage("", "ERRUSRDS018");
                lblErrorMsg[25] = CommonFunction.GetErrorMessage("", "SUCUSR002");
                lblErrorMsg[26] = CommonFunction.GetErrorMessage("", "SUCUSR003");

            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }
        private void BindRoleType()
        {
            try
            {
                using (DataSet ds = objAdminBO.RoleType("SELECT"))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlRoleType.DataSource = ds.Tables[0];
                        ddlRoleType.DataTextField = "UserRole";
                        ddlRoleType.DataValueField = "RoleID";
                        ddlRoleType.DataBind();
                    }
                    ddlRoleType.Items.Insert(0, new ListItem("Select Role", ""));
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);

            }
        }
        private void BindUserGrid()
        {
            try
            {

                using (DataSet ds = objAdminBO.UserMaster("SELECT"))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        gvUser.DataSource = dt;
                        gvUser.DataBind();
                        divGrid.Visible = true;
                    }
                    else
                    {
                        divGrid.Visible = false;
                    }
                    lblRecordCount.Text = ds.Tables[0].Rows.Count + " Records found";

                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //bool isUpdate = btnSave.Text == "Update";
            try
            {
                // ===== Trim and collect all values =====
                string strLoginId = txtLoginId.Text.Trim();
                string strEmpCode = txtEmpCode.Text.Trim();
                string strUserName = txtUsername.Text.Trim();
                string strEmail = txtEmail.Text.Trim();
                string strAltEmail = txtAlternateEmail.Text.Trim();
                string strDepartment = ddlDepartment.SelectedValue.Trim();
                string strDesignation = txtDesignation.Text.Trim();
                int roleID = string.IsNullOrEmpty(ddlRoleType.SelectedValue) ? 0 : Convert.ToInt32(ddlRoleType.SelectedValue);

                string strPassword = txtPassword.Text.Trim();

                bool boolIsActive = chkActive.Checked;
                string strGender = rblGender.SelectedValue;

                string strPhone = TxtPhone.Text.Trim();
                string strAltPhone = txtAlternatePhone.Text.Trim();

                // ===== Validations =====
                int userId = 0;
                if (!string.IsNullOrWhiteSpace(hdnUserID.Value))
                {
                    userId = Convert.ToInt32(hdnUserID.Value);
                }
                // Login ID
                if (string.IsNullOrWhiteSpace(strLoginId))
                {
                    ShowAlert(lblErrorMsg[0], "error"); return;
                }
                if (!CommonFunction.IsAlphaNumeric(strLoginId))
                {
                    ShowAlert(lblErrorMsg[1], "error"); return;
                }
                if (strLoginId.Length < 5)
                {
                    ShowAlert(lblErrorMsg[2], "error"); return;
                }

                // Employee Code
                if (string.IsNullOrWhiteSpace(strEmpCode))
                {
                    ShowAlert(lblErrorMsg[3], "error"); return;
                }
                if (!CommonFunction.IsAlphaNumeric(strEmpCode))
                {
                    ShowAlert(lblErrorMsg[4], "error"); return;
                }

                // User Name
                if (string.IsNullOrWhiteSpace(strUserName))
                {
                    ShowAlert(lblErrorMsg[5], "error"); return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(strUserName, @"^[A-Za-z .]+$"))
                {
                    ShowAlert(lblErrorMsg[6], "error"); return;
                }

                // Gender
                if (string.IsNullOrWhiteSpace(strGender))
                {
                    ShowAlert(lblErrorMsg[7], "error"); return;
                }

                // Email
                if (string.IsNullOrWhiteSpace(strEmail))
                {
                    ShowAlert(lblErrorMsg[8], "error"); return;
                }
                if (!CommonFunction.CheckEmail(strEmail))
                {
                    ShowAlert(lblErrorMsg[9], "error"); return;
                }

                // Alternate Email
                if (!string.IsNullOrWhiteSpace(strAltEmail) && !CommonFunction.CheckEmail(strAltEmail))
                {
                    ShowAlert(lblErrorMsg[10], "error"); return;
                }

                // Phone
                if (string.IsNullOrWhiteSpace(strPhone))
                {
                    ShowAlert(lblErrorMsg[11], "error"); return;
                }
                if (!Regex.IsMatch(strPhone, @"^\d{10}$"))
                {
                    ShowAlert(lblErrorMsg[12], "error"); return;
                }

                // Alternate Phone
                if (!string.IsNullOrWhiteSpace(strAltPhone) && !Regex.IsMatch(strAltPhone, @"^\d{10}$"))
                {
                    ShowAlert(lblErrorMsg[15], "error"); return;
                }

                // Department
                if (string.IsNullOrWhiteSpace(strDepartment))
                {
                    ShowAlert(lblErrorMsg[16], "error"); return;
                }

                // Designation
                if (string.IsNullOrWhiteSpace(strDesignation))
                {
                    ShowAlert(lblErrorMsg[17], "error"); return;
                }
                if (!Regex.IsMatch(strDesignation, @"^[A-Za-z' -]+$"))
                {
                    ShowAlert(lblErrorMsg[24], "error"); return;
                }
                // Role Type
                if (roleID == 0)
                {
                    ShowAlert(lblErrorMsg[18], "error");
                    return;
                }

                // Password
                if (string.IsNullOrWhiteSpace(strPassword))
                {
                    ShowAlert(lblErrorMsg[19], "error"); return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(strPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{6,}$"))
                {
                    ShowAlert(lblErrorMsg[21], "error"); return;
                }
                using (DataSet ds = objAdminBO.UserMaster("INSERT", 0, strLoginId, strEmpCode, strUserName,
                    strGender, strEmail, strAltEmail, strPhone, strAltPhone, strDepartment, strDesignation, roleID,
                   Security.CryptTripleDES(true, strPassword), boolIsActive, intAdminUserID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                        if (msgCode == 1)
                        {
                            ShowAlert(lblErrorMsg[22], "success"); // insert success
                            ClearFormFields();
                            divGrid.Visible = true;
                            divForm.Visible = false;
                            btnAddUser.Visible = true;
                            BindUserGrid();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("An unexpected error occurred. Please try again.", "error");
            }
        }
        protected void gvUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // Ignore paging commands
                if (e.CommandName == "Page")
                    return;

                string command = e.CommandName;

                // Safely convert only when it is numeric
                int userID;
                if (!int.TryParse(e.CommandArgument.ToString(), out userID))
                {
                    ShowAlert("Invalid userID ID.", "error");
                    return;
                }

                if (command == "EditUser")
                {
                    LoadUserForEdit(userID);
                    divForm.Visible = true;
                    divGrid.Visible = false;
                    btnSave.Visible = false;
                    btnUpdate.Visible = true;
                }
                else if (command == "DeleteUser")
                {
                    using (DataSet ds = objAdminBO.UserMaster("DELETE", userID))
                    {
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                            if (msgCode == 1)
                            {
                                ShowAlert(lblErrorMsg[26], "success");
                                BindUserGrid();
                            }
                            else
                            {
                                ShowAlert(lblErrorMsg[0], "error");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }

        private void LoadUserForEdit(int userId)
        {
            try
            {
                var dt = GetUserById(userId);
                if (dt.Rows.Count == 0) return;
                var dr = dt.Rows[0];
                hdnUserID.Value = dr["UserID"].ToString();
                txtLoginId.Text = dr["LoginID"].ToString();
                txtEmpCode.Text = dr["EmployeeCode"].ToString();
                txtUsername.Text = dr["UserName"].ToString();
                rblGender.SelectedValue = dr["Gender"].ToString();
                txtEmail.Text = dr["Email"].ToString();
                txtAlternateEmail.Text = dr["AlternateEmail"].ToString();
                TxtPhone.Text = dr["PhoneNumber"].ToString();
                txtAlternatePhone.Text = dr["AlternatePhoneNumber"].ToString();
                ddlDepartment.SelectedValue = dr["DepartmentName"].ToString();
                txtDesignation.Text = dr["Designation"].ToString();
                ddlRoleType.SelectedValue = dr["RoleType"].ToString().Trim();

                // Decrypt Password
                txtPassword.Text = Security.CryptTripleDES(false, dr["Password"].ToString());

                chkActive.Checked = Convert.ToBoolean(dr["Active"]);


            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("Error while fetching user details.", "error");
            }
        }
        private DataTable GetUserById(int userID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DataSet ds = objAdminBO.UserMaster("SELECTBYID", userID))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
            return dt;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int userID = Convert.ToInt32(hdnUserID.Value);
                string strLoginId = txtLoginId.Text.Trim();
                string strEmpCode = txtEmpCode.Text.Trim();
                string strUserName = txtUsername.Text.Trim();
                string strEmail = txtEmail.Text.Trim();
                string strAltEmail = txtAlternateEmail.Text.Trim();
                string strDepartment = ddlDepartment.SelectedValue.Trim();
                string strDesignation = txtDesignation.Text.Trim();
                int roleID = string.IsNullOrEmpty(ddlRoleType.SelectedValue) ? 0 : Convert.ToInt32(ddlRoleType.SelectedValue);

                string strPassword = txtPassword.Text.Trim();

                bool boolIsActive = chkActive.Checked;
                string strGender = rblGender.SelectedValue;

                string strPhone = TxtPhone.Text.Trim();
                string strAltPhone = txtAlternatePhone.Text.Trim();

                using (DataSet ds = objAdminBO.UserMaster(
           "UPDATE",
           userID,
           strLoginId,
           strEmpCode,
           strUserName,
           strGender,
           strEmail,
           strAltEmail,
           strPhone,
           strAltPhone,
           strDepartment,
           strDesignation,
           roleID,
           Security.CryptTripleDES(true, strPassword),
           boolIsActive,
           intAdminUserID

       ))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                        if (msgCode == 1)
                        {
                            ShowAlert(lblErrorMsg[25], "success");

                            ClearFormFields();

                            divGrid.Visible = true;
                            divForm.Visible = false;
                            btnAddUser.Visible = true;

                            BindUserGrid();
                        }
                        else
                        {
                            ShowAlert(lblErrorMsg[0], "error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("Failed to update user", "error");
            }
        }
        private void ClearFormFields()
        {
            txtLoginId.Text = "";
            txtEmpCode.Text = "";
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtAlternateEmail.Text = "";
            TxtPhone.Text = "";
            txtAlternatePhone.Text = "";
            ddlDepartment.SelectedIndex = 0;
            txtDesignation.Text = "";
            ddlRoleType.SelectedIndex = 0;
            txtPassword.Text = "";
            chkActive.Checked = false;
            rblGender.ClearSelection();
        }

        private void ShowAlert(string message, string alertType = "error")
        {
            ScriptManager.RegisterStartupScript(
                this, GetType(), Guid.NewGuid().ToString(),
                $"$(function(){{ AlertMessage('{message.Replace("'", "\\'")}', '{alertType.ToLower()}'); }});",
                true
            );
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFormFields();
                ShowAlert(lblErrorMsg[23], "info");
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("An error occurred while clearing fields.", "error");
            }
        }
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            divGrid.Visible=false;
            divForm.Visible=true;
            btnAddUser.Visible=false;
            btnSave.Visible=true;
            btnUpdate.Visible=false;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

            divGrid.Visible=true;
            divForm.Visible=false;
            btnAddUser.Visible=true;
        }

        protected void gvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUser.PageIndex = e.NewPageIndex;
            BindUserGrid();
        }

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    string searchBy = ddlSearchBy.SelectedValue;
        //    string searchValue = txtSearchValue.Text.Trim();

        //    // Validate dropdown
        //    if (string.IsNullOrEmpty(searchBy))
        //    {
        //        ShowAlert("Please select a search option.", "warning");
        //        return;
        //    }

        //    // Validate input
        //    if (string.IsNullOrEmpty(searchValue))
        //    {
        //        ShowAlert("Please enter a search value.", "error");
        //        return;
        //    }

        //    // BookID must be numeric
        //    if (searchBy == "User Name")
        //    {
        //        // Must NOT start with a number + only letters, spaces, allowed chars
        //        if (!System.Text.RegularExpressions.Regex.IsMatch(searchBy, @"^[A-Za-z .]+$"))
        //        {
        //            ShowAlert(lblErrorMsg[6], "error");
        //            txtSearchValue.Focus();
        //            return;
        //        }

        //    }

        //    // BookTitle validation
        //    if (searchBy == "Email")
        //    {
        //        // Must NOT start with a number + allow letters, numbers, allowed chars
        //        if (!CommonFunction.CheckEmail(searchBy))
        //        {
        //            ShowAlert(lblErrorMsg[9], "error");
        //            txtSearchValue.Focus();
        //            return;
        //        }
        //    }
        //    if (searchBy == "RoleType")
        //    {
        //        int roleID = Convert.ToInt32(searchBy);
        //        if (roleID == 0)
        //        {
        //            ShowAlert(lblErrorMsg[18], "error");
        //            return;
        //        }
        //    }

        //    DataSet ds = objAdminBO.UserMaster("SEARCH", 0, "", "", "", "", "", "", "", "",
        //    "", "", 0, "", true, searchBy, searchValue);

        //    gvUser.DataSource = ds.Tables[0];
        //    gvUser.DataBind();

        //    lblRecordCount.Text = gvUser.Rows.Count + " Records found";

        //    if (gvUser.Rows.Count == 0)
        //    {
        //        ShowAlert("No records found.", "warning");
        //    }

        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchBy = ddlSearchBy.SelectedValue;
                string searchValue = txtSearchValue.Text.Trim();

                // Validate dropdown
                if (string.IsNullOrEmpty(searchBy))
                {
                    ShowAlert("Please select a search option.", "warning");
                    return;
                }

                // Validate input
                if (string.IsNullOrEmpty(searchValue))
                {
                    ShowAlert("Please enter a search value.", "error");
                    return;
                }

                // ----------- SEARCH VALIDATION ---------------

                // USER NAME
                if (searchBy == "UserName")
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(searchValue, @"^[A-Za-z .]+$"))
                    {
                        ShowAlert("Please enter a valid user name.", "error");
                        txtSearchValue.Focus();
                        return;
                    }
                }

                // EMAIL
                if (searchBy == "Email")
                {
                    if (!CommonFunction.CheckEmail(searchValue))
                    {
                        ShowAlert("Please enter a valid email.", "error");
                        txtSearchValue.Focus();
                        return;
                    }
                }

                // ROLE TYPE
                if (searchBy == "RoleType")
                {

                    if (string.IsNullOrEmpty(searchValue))
                    {
                        ShowAlert("Please enter a search value.", "error");
                        return;
                    }
                }

                // ------------ DB CALL ----------------

                DataSet ds = objAdminBO.UserMaster(
                    "SEARCH",
                    0, "", "", "", "", "", "", "", "",
                    "", "", 0, "", true, intAdminUserID,
                    searchBy,
                    searchValue
                );

                gvUser.DataSource = ds.Tables[0];
                gvUser.DataBind();

                lblRecordCount.Text = gvUser.Rows.Count + " Records found";

                if (gvUser.Rows.Count == 0)
                {
                    ShowAlert("No records found.", "warning");
                }
            }
            catch(Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("Error: " + ex.Message, "error");
            }
           
        }
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            ddlSearchBy.SelectedIndex = 0;
            txtSearchValue.Text = "";

            gvUser.PageIndex = 0;
            BindUserGrid();
        }
    }
}