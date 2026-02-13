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
    public partial class AuthorMaster : System.Web.UI.Page
    {
        MasterBO objMasterBO = new MasterBO();
        private string[] lblErrorMsg = new string[10];
        int intAdminUserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            intAdminUserID = Convert.ToInt32(Session["AdminUserID"]);
            if (!IsPostBack)
            {
                BindAuthorGrid();

            }
            lblErrorMsg = new string[30];
            lblErrorMsg[0] = CommonFunction.GetErrorMessage("", "ERRAM01");
            lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "SUSAM01");
            lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "SUSAM02");
            lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "SUSAM03");
            lblErrorMsg[4] = CommonFunction.GetErrorMessage("", "ERRAM03");
            lblErrorMsg[5] = CommonFunction.GetErrorMessage("", "ERRAM05");
            lblErrorMsg[6] = CommonFunction.GetErrorMessage("", "ERRAM06");
            lblErrorMsg[7] = CommonFunction.GetErrorMessage("", "ERRAM07");
        }

        private void BindAuthorGrid()
        {
            try
            {
                using (DataSet ds = objMasterBO.AuthorMaster("SELECT"))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        gvAuthor.DataSource = ds.Tables[0];
                        gvAuthor.DataBind();
                    }
                    else
                    {
                        gvAuthor.DataSource = null;
                        gvAuthor.DataBind();
                    }
                    lblRecordCount.Text =  "No. of Records: " + ds.Tables[0].Rows.Count;
                    if (ds.Tables[0].Rows.Count > gvAuthor.PageSize)
                    {
                        BuildPager(gvAuthor.PageCount, gvAuthor.PageIndex);
                        rptPager.Visible = true;
                    }
                    else
                    {
                        rptPager.Visible = false;
                    }
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
                string authorName = txtAuthorName.Text.Trim();
                string authorType = ddlAuthorType.SelectedValue;

                Regex namePattern = new Regex(@"^[A-Za-z'. -]+$");

                if (string.IsNullOrWhiteSpace(authorName))
                {
                    ShowAlert(lblErrorMsg[4], "error");
                    txtAuthorName.Focus();
                    return;
                }

                if (!namePattern.IsMatch(authorName))
                {
                    ShowAlert(lblErrorMsg[1], "error");
                    txtAuthorName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(authorType))
                {
                    ShowAlert(lblErrorMsg[5], "error");
                    ddlAuthorType.Focus();
                    return;
                }
                using (DataSet ds = objMasterBO.AuthorMaster("INSERT", 0, authorName, authorType, true, intAdminUserID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                        if (msgCode == 1)
                        {
                            ShowAlert(lblErrorMsg[1], "success");
                            ClearFormFields();
                            BindAuthorGrid();

                        }
                        if (msgCode == 2)
                        {
                            ShowAlert(lblErrorMsg[6], "error");
                        }
                        if (msgCode == 3)
                        {
                            ShowAlert(lblErrorMsg[7], "error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }
        protected void gvAuthor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string authorID = e.CommandArgument.ToString();
                if (e.CommandName == "EditAuthor")
                {
                    using (DataSet ds = objMasterBO.AuthorMaster("SELECTBYID", Convert.ToInt32(authorID)))
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];

                            hdnAuthorID.Value = dr["AuthorID"].ToString();
                            txtAuthorName.Text = dr["AuthorName"].ToString();
                            ddlAuthorType.SelectedValue = dr["AuthorType"].ToString();
                            chkActive.Checked = Convert.ToBoolean(dr["Active"]);

                            btnAdd.Visible = false;
                            btnUpdate.Visible = true;
                        }
                    }
                }
                if (e.CommandName == "DeleteAuthor")
                {

                    using (DataSet ds = objMasterBO.AuthorMaster("DELETE", Convert.ToInt32(authorID), "", "", false, intAdminUserID))
                    {
                        int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                        if (msgCode == 1)
                        {
                            ShowAlert(lblErrorMsg[3], "success");
                            BindAuthorGrid();
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
                int authorID = Convert.ToInt32(hdnAuthorID.Value);
                string authorName = txtAuthorName.Text.Trim();
                string authorType = ddlAuthorType.SelectedValue;
                bool isUpdate = btnAdd.Text == "Save";
                bool Active = chkActive.Checked;

                using (DataSet ds = objMasterBO.AuthorMaster("UPDATE", Convert.ToInt32(hdnAuthorID.Value), authorName, authorType, chkActive.Checked, intAdminUserID))
                {
                    int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                    if (msgCode == 1)
                    {
                        ShowAlert(lblErrorMsg[2], "success");
                        ClearFormFields();
                        BindAuthorGrid();
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
            txtAuthorName.Text = "";
            ddlAuthorType.SelectedIndex = 0;

        }
        protected void Clear_Click(object sender, EventArgs e)
        {
            chkActive.Checked=true;
            ClearFormFields();
            btnAdd.Visible=true;
            btnUpdate.Visible= false;
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
                IsActive = false  
            });

            for (int i = startPage; i <= endPage; i++)
            {
                pages.Add(new
                {
                    PageIndex = i,
                    Text = (i + 1).ToString(),
                    Command = "Page",
                    Enabled = true,
                    IsActive = (i == currentPage) 
                });
            }

            pages.Add(new
            {
                PageIndex = currentPage + 1,
                Text = "Next »",
                Command = "Page",
                Enabled = currentPage < totalPages - 1,
                IsActive = false   
            });

            rptPager.DataSource = pages;
            rptPager.DataBind();
        }

        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int newIndex = Convert.ToInt32(e.CommandArgument);

            gvAuthor.PageIndex = newIndex;
            BindAuthorGrid();
        }

        protected void gvAuthor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAuthor.PageIndex = e.NewPageIndex;
            BindAuthorGrid();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                if (objMasterBO != null)
                {
                    objMasterBO.ReleaseResources();
                    objMasterBO = null;
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }
    }
}
