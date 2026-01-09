using BLL;
using Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class MemberDashboard : System.Web.UI.Page
    {
        CommonBO objCommonBO = new CommonBO();
        private string[] lblErrorMsg = new string[10];
        //int intAdminUserID;
        protected void Page_Load(object sender, EventArgs e)
        {
            //intAdminUserID = Convert.ToInt32(Session["AdminUserID"]);

            if (Session["MemberID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }
            if (!IsPostBack)
            {
                BindSummary();

            }
            lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "ERRMEM01");
            lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "ERRMEM02");
            lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "SUSMEM01");
            lblErrorMsg[4] = CommonFunction.GetErrorMessage("", "ERRMEM03");
            lblErrorMsg[5] = CommonFunction.GetErrorMessage("", "ERRMEM04");

        }

        private void BindSummary()
        {
            try
            {
                string memberID = Session["MemberID"].ToString();

                using (DataSet ds = objCommonBO.GetMemberDashboard(memberID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        // 🔹 SUMMARY COUNTS
                        DataRow dr = ds.Tables[0].Rows[0];

                        lblBorrowedCount.Text = dr["BorrowedBooksCount"].ToString();
                        lblReturnedCount.Text = dr["ReturnedBooksCount"].ToString();
                        lblDueCount.Text = dr["DueBooksCount"].ToString();

                        int overdueCount = Convert.ToInt32(dr["OverDueBooksCount"]);
                        lblOverDueCount.Text = overdueCount.ToString();
                        divOverDueAlert.Visible = overdueCount > 0;

                        // 🔹 STORE BOOK DATA
                        Session["MemberBooks"] = ds.Tables[0];

                        // 🔹 DEFAULT GRID
                        BindGrid("BORROWED");
                    }
                    else
                    {
                        ClearGrids();
                        rptPager.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }



        private void BindGrid(string type, bool resetPageIndex = false)
        {
            DataTable dt = Session["MemberBooks"] as DataTable;

            if (dt == null)
            {
                gvBooks.DataSource = null;
                gvBooks.DataBind();
                rptPager.Visible = false;
                return;
            }

            DataView dv = dt.DefaultView;

            switch (type)
            {
                case "BORROWED":
                    dv.RowFilter = "";
                    ConfigureGrid("BORROWED");
                    lblGridTitle.InnerText = "Borrowed Books";
                    break;

                case "RETURNED":
                    dv.RowFilter = "ReturnDate IS NOT NULL";
                    ConfigureGrid("RETURNED");
                    lblGridTitle.InnerText = "Returned Books";
                    break;

                case "DUE":
                    dv.RowFilter = "ReturnDate IS NULL";
                    ConfigureGrid("DUE");
                    lblGridTitle.InnerText = "Due Books";
                    break;
            }

            DataTable filteredTable = dv.ToTable();

            // ✅ Reset page index ONLY when required
            if (resetPageIndex)
                gvBooks.PageIndex = 0;

            gvBooks.DataSource = filteredTable;
            gvBooks.DataBind();

            // ✅ Pagination logic
            if (filteredTable.Rows.Count > gvBooks.PageSize)
            {
                BuildPager(gvBooks.PageCount, gvBooks.PageIndex);
                rptPager.Visible = true;
            }
            else
            {
                rptPager.Visible = false;
            }
        }

        private void ConfigureGrid(string mode)
        {
            int issueDateCol = 7;
            int dueDateCol = 8;
            int returnDateCol = 9;
            int renewalCol = 10;
            int renewalRequestCol = 11;
            int status = 12;
            int rejectReason = 13;

            // Hide optional columns first
            gvBooks.Columns[issueDateCol].Visible = false;
            gvBooks.Columns[dueDateCol].Visible = false;
            gvBooks.Columns[returnDateCol].Visible = false;
            gvBooks.Columns[renewalCol].Visible = false;
            gvBooks.Columns[renewalRequestCol].Visible = false;
            gvBooks.Columns[status].Visible = false;
            gvBooks.Columns[rejectReason].Visible = false;



            if (mode == "BORROWED") // Default
            {
                gvBooks.Columns[issueDateCol].Visible = true;
                gvBooks.Columns[dueDateCol].Visible = true;
                gvBooks.Columns[returnDateCol].Visible = true;
                gvBooks.Columns[renewalCol].Visible = true;
                gvBooks.Columns[status].Visible = true;
                gvBooks.Columns[rejectReason].Visible = true;
            }
            else if (mode == "RETURNED")
            {
                gvBooks.Columns[returnDateCol].Visible = true;
                gvBooks.Columns[status].Visible = true;
               
            }
            else if (mode == "DUE")
            {
                gvBooks.Columns[issueDateCol].Visible = true;
                gvBooks.Columns[dueDateCol].Visible = true;
                gvBooks.Columns[renewalCol].Visible = true;
                gvBooks.Columns[renewalRequestCol].Visible = true;
                gvBooks.Columns[status].Visible = true;
                gvBooks.Columns[rejectReason].Visible = true;

            }
        }


        protected void Borrowed_Click(object sender, EventArgs e)
        {
            BindGrid("BORROWED");
        }

        protected void Returned_Click(object sender, EventArgs e)
        {
            BindGrid("RETURNED");
        }

        protected void Due_Click(object sender, EventArgs e)
        {
            BindGrid("DUE");
        }
        private void ClearGrids()
        {
            // Clear GridView data
            gvBooks.DataSource = null;
            gvBooks.DataBind();

            // Reset summary counts
            lblBorrowedCount.Text = "0";
            lblReturnedCount.Text = "0";
            lblDueCount.Text = "0";
            lblOverDueCount.Text = "0";

            // Hide overdue alert
            divOverDueAlert.Visible = false;

            // Clear session data
            Session["MemberBooks"] = null;

            // Reset grid title
            lblGridTitle.InnerText = "Books";
        }

        protected void gvBooks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBooks.PageIndex = e.NewPageIndex;
            BindSummary();
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

            gvBooks.PageIndex = newIndex;
            BindSummary();

        }

        protected void gvBooks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RenewalRequest")
            {
                // 1️⃣ Get BookIssueID from CommandArgument
                int bookIssueId;
                if (!int.TryParse(e.CommandArgument.ToString(), out bookIssueId))
                {
                    ShowAlert(lblErrorMsg[5], "error");
                    return;
                }

                // 2️⃣ Get data from session
                DataTable dt = Session["MemberBooks"] as DataTable;
                if (dt == null) return;

                // 3️⃣ Find row by BookIssueID
                DataRow row = dt.AsEnumerable()
                                .FirstOrDefault(r => Convert.ToInt32(r["BookIssueID"]) == bookIssueId);

                if (row != null)
                {
                    lblISBN.Text = row["ISBN"].ToString();
                    lblBookTitle.Text = row["BookTitle"].ToString();
                    lblCategory.Text = row["CategoryName"].ToString();
                    lblAuthors.Text = row["AuthorNames"].ToString();
                    lblDueDate.Text = Convert.ToDateTime(row["DueDate"])
                                        .ToString("dd-MMM-yyyy");

                    // Store BookID in hidden field for submission
                    hdnBookID.Value = row["BookID"].ToString();
                    hdnBookIssueID.Value = row["BookIssueID"].ToString();

                    txtRenewDays.Text = "";
                }

                // 4️⃣ Show modal
                ScriptManager.RegisterStartupScript(
                    Page,
                    Page.GetType(),
                    "ShowModal",
                    "$(document).ready(function(){ $('#renewalModal').modal('show'); });",
                    true
                );
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
        protected void btnSubmitRenewal_Click(object sender, EventArgs e)
        {
            try
            {
                string memberID = Session["MemberID"].ToString();
                int bookIssueId = Convert.ToInt32(hdnBookIssueID.Value);

                int noOfDays;
                if (!int.TryParse(txtRenewDays.Text.Trim(), out noOfDays) || noOfDays <= 0)
                {
                    ShowAlert(lblErrorMsg[4], "error");
                    return;
                }

                using (DataSet ds = objCommonBO.BookRenewalRequest(
                    "INSERT",
                    memberID,
                    bookIssueId,
                    noOfDays
                ))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);

                        // ❌ Invalid issue / already returned
                        if (msgCode == -1)
                        {
                            ShowAlert(lblErrorMsg[1], "error");
                        }
                        // ❌ Duplicate renewal request
                        else if (msgCode == 2)
                        {
                            ShowAlert(lblErrorMsg[2], "error");
                        }
                        // ✅ Renewal request success
                        else if (msgCode == 1)
                        {
                            ShowAlert(lblErrorMsg[3], "success");

                            BindSummary();

                            ScriptManager.RegisterStartupScript(
                                Page,
                                Page.GetType(),
                                "HideModal",
                                "$('#renewalModal').modal('hide');",
                                true
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }

        }
    }
}