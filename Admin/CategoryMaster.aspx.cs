using BLL;
using Library;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class CategoryMaster : System.Web.UI.Page
    {
        MasterBO objMasterBO = new MasterBO();
       
        private string[] lblErrorMsg = new string[10];
        int intAdminUserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            intAdminUserID = Convert.ToInt32(Session["AdminUserID"]);
            if (!IsPostBack)
            {
                BindCategoryGrid();
                
            }
            lblErrorMsg = new string[30];
            lblErrorMsg[0] = CommonFunction.GetErrorMessage("", "SUSCM01");
            lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "SUSCM02");
            lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "SUSCM03");
           
            lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "ERRCM01");
            lblErrorMsg[4] = CommonFunction.GetErrorMessage("", "ERRCM02");
            lblErrorMsg[5] = CommonFunction.GetErrorMessage("", "ERRCM03");
            lblErrorMsg[6] = CommonFunction.GetErrorMessage("", "ERRCM04");
            lblErrorMsg[7] = CommonFunction.GetErrorMessage("", "ERRCM05");
            lblErrorMsg[8] = CommonFunction.GetErrorMessage("", "ERRCM06");
            lblErrorMsg[9] = CommonFunction.GetErrorMessage("", "ERRCM07");


        }
        private void BindCategoryGrid()
        {
            try
            {
                using (DataSet ds = objMasterBO.CategoryMaster("SELECT"))
                {
                    
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            gvCategory.DataSource = ds.Tables[0];
                            gvCategory.DataBind();
                      

                    }
                        else
                        {
                            gvCategory.DataSource = null;
                            gvCategory.DataBind();
                        

                    }
                    lblRecordCount.Text =ds.Tables[0].Rows.Count + " Records found";
                }
                
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("Failed to load grid", "error");
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            try
            {
                string CategoryName = txtCategoryName.Text.Trim();
                string Description = txtDescription.InnerText.Trim();
                // Category Name - Required
                if (string.IsNullOrWhiteSpace(CategoryName))
                {
                    ShowAlert(lblErrorMsg[3], "error");
                    txtCategoryName.Focus();
                    return;
                }
                // Category Name - Pattern check (alphabets, space, hyphen, apostrophe)
                if (!Regex.IsMatch(CategoryName, @"^[A-Za-z' -]+$"))
                {
                    ShowAlert(lblErrorMsg[4], "error");
                    txtCategoryName.Focus();
                    return;
                }
                // Description - Required
                if (string.IsNullOrWhiteSpace(Description))
                {
                    ShowAlert(lblErrorMsg[5], "error");
                    txtDescription.Focus();
                    return;
                }
                if(Description.Length < 5)
{
                    ShowAlert(lblErrorMsg[6], "error");
                    txtDescription.Focus();
                    return;
                }
                // Max length
                if (Description.Length > 500)
                {
                    ShowAlert(lblErrorMsg[7], "error");
                    txtDescription.Focus();
                    return;
                }
                // Allowed characters
                if (!Regex.IsMatch(Description, @"^[A-Za-z0-9 .,()'""-:;!?]+$"))
                {
                    ShowAlert(lblErrorMsg[8], "error");
                    txtDescription.Focus();
                    return;
                }
              
                using (DataSet ds = objMasterBO.CategoryMaster("INSERT", 0, CategoryName, Description, true, intAdminUserID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                        if (msgCode == 2)
                        {
                            ShowAlert(lblErrorMsg[9], "error");
                        }
                        if (msgCode == 1)
                        {
                            ShowAlert(lblErrorMsg[0], "success");
                            ClearFormFields();
                            BindCategoryGrid();
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }
        protected void gvCategory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string CategoryID = e.CommandArgument.ToString();
                int updatedBy = Convert.ToInt32(Session["UserID"]);
                if (e.CommandName == "EditCategory")
                {
                    using (DataSet ds = objMasterBO.CategoryMaster("SELECTBYID", Convert.ToInt32(CategoryID)))
                    {
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            hdnCategoryID.Value = dr["CategoryID"].ToString();
                            txtCategoryName.Text = dr["CategoryName"].ToString();
                            txtDescription.InnerText = dr["Description"].ToString();
                            chkActive.Checked = Convert.ToBoolean(dr["Active"]);
                            btnAdd.Visible = false;
                            btnUpdate.Visible = true;
                        }
                    }
                }
                if (e.CommandName == "DeleteCategory")
                {
                    using (DataSet ds = objMasterBO.CategoryMaster("DELETE", Convert.ToInt32(CategoryID), "", "", false, intAdminUserID))
                    {
                        int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                        if (msgCode == 1)
                        {
                            ShowAlert(lblErrorMsg[2], "success");
                            BindCategoryGrid();
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }
        protected void Update_Click(object sender, EventArgs e)
        {
            try
            {
                int CategoryID = Convert.ToInt32(hdnCategoryID.Value);
                string CategoryName = txtCategoryName.Text.Trim();
                string Description = txtDescription.InnerText;
                bool isUpdate = btnAdd.Text == "Save";
                bool Active = chkActive.Checked;
              

                using (DataSet ds = objMasterBO.CategoryMaster("UPDATE", Convert.ToInt32(hdnCategoryID.Value),CategoryName, Description, chkActive.Checked, intAdminUserID))
                {
                    int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                    if (msgCode == 2)
                    {
                        ShowAlert(lblErrorMsg[9], "error");
                    }
                    if (msgCode == 1)
                    {
                        ShowAlert(lblErrorMsg[1], "success");
                        ClearFormFields();
                        BindCategoryGrid();
                        btnAdd.Visible = true;
                        btnUpdate.Visible = false;
                        btnAdd.Text = "Save";
                       
                    }
                    else
                    {
                        ShowAlert(lblErrorMsg[0], "error");
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }


        private void ShowAlert(string message, string alertType = "error")
        {
            ScriptManager.RegisterStartupScript(
                this, GetType(), Guid.NewGuid().ToString(),
                $"$(function(){{ AlertMessage('{message.Replace("'", "\\'")}', '{alertType.ToLower()}'); }});",
                true
            );
        }

        private void ClearFormFields()
        {
            txtCategoryName.Text = "";
            txtDescription.InnerText = "";
        }
        protected void Clear_Click(object sender, EventArgs e)
        {
            // Clear TextBox
            ClearFormFields();
        }

        protected void gvCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategory.PageIndex = e.NewPageIndex;
            BindCategoryGrid();
         
        }
    }
}
