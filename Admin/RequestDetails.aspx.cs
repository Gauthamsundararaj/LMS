using BLL;
using Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class RequestDetials : System.Web.UI.Page
    {
        CommonBO objCommonBO = new CommonBO();
        int intAdminUserID;
        private string[] lblErrorMsg = new string[30];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRenewals();
                //ApplySelectedCardStyle();
            }

            lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "ERRRR01");
            lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "ERRRR02");
            lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "SUSRR01");
            lblErrorMsg[4] = CommonFunction.GetErrorMessage("", "SUSRR02");
        }
        private void LoadRenewals()
        {
            try
            {
                using (DataSet ds = objCommonBO.BookRenewalRequest("SELECT", "", 0, 0))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];

                        // 🔹 CARD COUNTS
                        lblRenewalsCount.Text = dt.Rows[0]["TotalRenewals"].ToString();
                        lblRequestCount.Text  = dt.Rows[0]["Requests"].ToString();
                        lblApproveCount.Text  = dt.Rows[0]["ProcessedRequests"].ToString();

                        // 🔹 STORE DATA IN SESSION
                        Session["RenewalData"] = dt;

                        // 🔹 DEFAULT GRID → NEW REQUESTS
                        BindRenewalGrid("NEW");
                    }
                    else
                    {
                        ClearRenewalGrid();
                        rptPager.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }

        private void BindRenewalGrid(string type, bool resetPageIndex = false)
        {
            ViewState["CurrentGridType"] = type;
            SetCSVHiddenColumns(type);
            DataTable dt = Session["RenewalData"] as DataTable;
            if (dt == null) return;

            DataView dv = new DataView(dt);

            // STATUS FILTER
            switch (type)
            {
                case "NEW":
                    dv.RowFilter = "RequestStatus = 1";
                    lblGridTitle.InnerText = "Yet To Be Process";
                    ConfigureRenewalGrid("NEW");
                    break;

                case "PROCESSED":
                    dv.RowFilter = "RequestStatus IN (2,3)";
                    lblGridTitle.InnerText = "Processed Renewal Requests";
                    ConfigureRenewalGrid("PROCESSED");
                    break;

                case "ALL":
                    dv.RowFilter = "";
                    lblGridTitle.InnerText = "Total Renewal Requests";
                    ConfigureRenewalGrid("ALL");
                    break;
            }

            // APPLY HEADER FILTERS
            ApplyFiltersUsingViewState(ref dv);

            if (resetPageIndex)
                gvRenewals.PageIndex = 0;

            // STORE FILTERED DATA FOR CSV
            ViewState["GridData"] = dv.ToTable();
            DataTable filteredTable = dv.ToTable();
            gvRenewals.DataSource = filteredTable;
            gvRenewals.DataBind();
            lblRecordCount.Text = "No. of Records: " + filteredTable.Rows.Count;
            // RESTORE FILTER UI
            RestoreRenewalGridFilters();
            if (filteredTable.Rows.Count > gvRenewals.PageSize) 
            {
                
                BuildPager(gvRenewals.PageCount, gvRenewals.PageIndex);
                rptPager.Visible = true;
            } 
            else 
            {
                rptPager.Visible = false;
            }
            ApplySelectedCardStyle();
        }

        private string SelectedCard
        {
            get { return ViewState["SelectedCard"]?.ToString() ?? "NEW"; }
            set { ViewState["SelectedCard"] = value; }
        }
        private void ApplySelectedCardStyle()
        {
            cardAll.Attributes["class"] =
                cardAll.Attributes["class"].Replace(" selected-card", "");
            cardNew.Attributes["class"] =
                cardNew.Attributes["class"].Replace(" selected-card", "");
            cardProcessed.Attributes["class"] =
                cardProcessed.Attributes["class"].Replace(" selected-card", "");

            switch (SelectedCard)
            {
                case "ALL":
                    cardAll.Attributes["class"] += " selected-card";
                    break;

                case "NEW":
                    cardNew.Attributes["class"] += " selected-card";
                    break;

                case "PROCESSED":
                    cardProcessed.Attributes["class"] += " selected-card";
                    break;
            }
        }


        private void ConfigureRenewalGrid(string mode)
        {
            int approvedDueCol = 8;
            int approvedDueDateCol = 12;
            int actionCol = 13;
            int rejectReason = 14;

            // Hide first
            gvRenewals.Columns[approvedDueCol].Visible = false;
            gvRenewals.Columns[approvedDueDateCol].Visible = false;
            gvRenewals.Columns[actionCol].Visible = false;
            gvRenewals.Columns[rejectReason].Visible = false;


            if (mode == "NEW")
            {
                gvRenewals.Columns[approvedDueCol].Visible = false;
                gvRenewals.Columns[actionCol].Visible = true;
                gvRenewals.Columns[approvedDueDateCol].Visible = true;
                gvRenewals.Columns[rejectReason].Visible = false;
            }
            else if (mode == "PROCESSED")
            {
                gvRenewals.Columns[approvedDueCol].Visible = true;
                gvRenewals.Columns[approvedDueDateCol].Visible = false;
                gvRenewals.Columns[actionCol].Visible = false;
                gvRenewals.Columns[rejectReason].Visible = true;
            }
            else if (mode == "ALL")
            {
                gvRenewals.Columns[approvedDueCol].Visible = true;
                gvRenewals.Columns[approvedDueDateCol].Visible = false;
                gvRenewals.Columns[actionCol].Visible = false;
                gvRenewals.Columns[rejectReason].Visible = true;
            }
        }
        protected void CardNewRequests_Click(object sender, EventArgs e)
        {
            SelectedCard = "NEW";
            BindRenewalGrid("NEW", true);
        }

        protected void CardApproved_Click(object sender, EventArgs e)
        {
            SelectedCard = "PROCESSED";
            BindRenewalGrid("PROCESSED", true);
        }

        protected void CardAllRenewals_Click(object sender, EventArgs e)
        {
            SelectedCard = "ALL";
            BindRenewalGrid("ALL", true);
        }
        private void ClearRenewalGrid()
        {
            gvRenewals.DataSource = null;
            gvRenewals.DataBind();

            lblRenewalsCount.Text = "0";
            lblRequestCount.Text = "0";
            lblApproveCount.Text = "0";

            lblGridTitle.InnerText = "Renewal Requests";

            Session["RenewalData"] = null;
        }

        private string FilterRequestedType
        {
            get { return ViewState["RequestedType"]?.ToString(); }
            set { ViewState["RequestedType"] = value; }
        }

        private string FilterISBN
        {
            get { return ViewState["ISBN"]?.ToString(); }
            set { ViewState["ISBN"] = value; }
        }

        private string FilterBookTitle
        {
            get { return ViewState["BookTitle"]?.ToString(); }
            set { ViewState["BookTitle"] = value; }
        }

        private string FilterRequestedMember
        {
            get { return ViewState["RequestedMember"]?.ToString(); }
            set { ViewState["RequestedMember"] = value; }
        }

        protected void gvRenewals_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int renewalRequestId = Convert.ToInt32(e.CommandArgument);
                intAdminUserID = Convert.ToInt32(Session["AdminUserID"]);
                if (e.CommandName == "Approve")
                {
                    GridViewRow row = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
                    TextBox txtDate = row.FindControl("txtApprovedDueDate") as TextBox;

                    if (txtDate == null || string.IsNullOrEmpty(txtDate.Text))
                    {
                        ShowAlert(lblErrorMsg[1], "error");
                        return;
                    }

                    DateTime approvedDate = Convert.ToDateTime(txtDate.Text);

                    objCommonBO.RenewalRequestAction(
                       "APPROVE",
                        renewalRequestId,
                        approvedDate,
                        null,
                        intAdminUserID
                    );

                    ShowAlert(lblErrorMsg[3], "success");
                    LoadRenewals();
                }

                else if (e.CommandName == "Reject")
                {
                    hdnRejectRequestID.Value = renewalRequestId.ToString();
                    ddlRejectReason.SelectedValue = "";

                    ScriptManager.RegisterStartupScript(
                    Page,
                    Page.GetType(),
                    "ShowModal",
                    "$(document).ready(function(){ $('#rejectModal').modal('show'); });",
                    true
                );
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }
        protected void btnConfirmReject_Click(object sender, EventArgs e)
        {
            try
            {
                int renewalRequestId = Convert.ToInt32(hdnRejectRequestID.Value);
                intAdminUserID = Convert.ToInt32(Session["AdminUserID"]);

                if (string.IsNullOrWhiteSpace(ddlRejectReason.SelectedValue))
                {
                    ShowAlert(lblErrorMsg[2], "error");
                    return;
                }
                string rejectReason = ddlRejectReason.SelectedValue.Trim();
                objCommonBO.RenewalRequestAction(
                    "REJECT",
                    renewalRequestId,
                    null,
                    rejectReason,
                    intAdminUserID
                );

                ShowAlert(lblErrorMsg[4], "success");

                ScriptManager.RegisterStartupScript(
                   Page,
                   Page.GetType(),
                   "HideModal",
                   "$(document).ready(function(){ $('#rejectModal').modal('hide'); });",
                   true

              
                );

                LoadRenewals();
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }
        protected void gvRenewals_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRenewals.PageIndex = e.NewPageIndex;

            string type = ViewState["CurrentGridType"]?.ToString() ?? "NEW";
            BindRenewalGrid(type);
        }
        private void ShowAlert(string message, string alertType = "error")
        {
            ScriptManager.RegisterStartupScript(
                this, GetType(), Guid.NewGuid().ToString(),
                $"$(function(){{ AlertMessage('{message.Replace("'", "\\'")}', '{alertType.ToLower()}'); }});",
                true
            );
        }
        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int newIndex = Convert.ToInt32(e.CommandArgument);
            gvRenewals.PageIndex = newIndex;
            string type = ViewState["CurrentGridType"]?.ToString() ?? "NEW";
            BindRenewalGrid(type);
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
        private void ApplyFiltersUsingViewState(ref DataView dv)
        {
            List<string> filters = new List<string>();

            if (!string.IsNullOrEmpty(FilterRequestedType))
                filters.Add($"[Requested Type] LIKE '%{Safe(FilterRequestedType)}%'");

            if (!string.IsNullOrEmpty(FilterISBN))
                filters.Add($"ISBN LIKE '%{Safe(FilterISBN)}%'");

            if (!string.IsNullOrEmpty(FilterBookTitle))
                filters.Add($"[Book Title] LIKE '%{Safe(FilterBookTitle)}%'");

            if (!string.IsNullOrEmpty(FilterRequestedMember))
                filters.Add($"[Requested Member] LIKE '%{Safe(FilterRequestedMember)}%'");

            if (filters.Count > 0)
                dv.RowFilter += (dv.RowFilter == "" ? "" : " AND ") + string.Join(" AND ", filters);
        }

        private string Safe(string value)
        {
            return value?.Replace("'", "''");
        }

        private void SaveRenewalGridFilters()
        {
            if (gvRenewals.HeaderRow == null) return;

            FilterRequestedType =
                (gvRenewals.HeaderRow.FindControl("txtFilterRequestedType") as TextBox)?.Text.Trim();

            FilterISBN =
                (gvRenewals.HeaderRow.FindControl("txtFilterISBN") as TextBox)?.Text.Trim();

            FilterBookTitle =
                (gvRenewals.HeaderRow.FindControl("txtFilterBookTitle") as TextBox)?.Text.Trim();

            FilterRequestedMember =
                (gvRenewals.HeaderRow.FindControl("txtFilterRequestedMember") as TextBox)?.Text.Trim();
        }


        private void RestoreRenewalGridFilters()
        {
            if (gvRenewals.HeaderRow == null) return;

            (gvRenewals.HeaderRow.FindControl("txtFilterRequestedType") as TextBox).Text = FilterRequestedType;
            (gvRenewals.HeaderRow.FindControl("txtFilterISBN") as TextBox).Text = FilterISBN;
            (gvRenewals.HeaderRow.FindControl("txtFilterBookTitle") as TextBox).Text = FilterBookTitle;
            (gvRenewals.HeaderRow.FindControl("txtFilterRequestedMember") as TextBox).Text = FilterRequestedMember;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SaveRenewalGridFilters();
            BindRenewalGrid(
                ViewState["CurrentGridType"]?.ToString() ?? "NEW",
                true
            );
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            

            FilterRequestedType = null;
            FilterISBN = null;
            FilterBookTitle = null;
            FilterRequestedMember = null;

            gvRenewals.PageIndex = 0;
            BindRenewalGrid(
                ViewState["CurrentGridType"]?.ToString() ?? "NEW",
                true
            );

            //IsRefresh = false;
        }

        private void SetCSVHiddenColumns(string gridType)
        {
            switch (gridType)
            {
                case "NEW":
                    hfRemoveColumnsCSV.Value =
                        "TotalRenewals,Requests,ProcessedRequests,RenewalRequestID,BookIssueID,ActiveStatus,Approved Due Date,Rejected Reason";
                    break;

                case "PROCESSED":
                case "ALL":
                    hfRemoveColumnsCSV.Value =
                        "TotalRenewals,Requests,ProcessedRequests,RenewalRequestID,BookIssueID,ActiveStatus";
                    break;
            }
        }

        protected void btnDownloadCSV_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable gridData = ViewState["GridData"] as DataTable;

                if (gridData == null || gridData.Rows.Count == 0)
                {
                    ShowAlert("No data available for export.", "warning");
                    return;
                }

                // ✅ CLONE STRUCTURE + DATA
                DataTable csvTable = gridData.Copy();
                DataColumn snoCol = new DataColumn("S.No", typeof(int));
                csvTable.Columns.Add(snoCol);
                snoCol.SetOrdinal(0); // Make it first column

                // ✅ FILL S.NO VALUES
                int index = 1;
                foreach (DataRow row in csvTable.Rows)
                {
                    row["S.No"] = index++;
                }
                if (csvTable.Columns.Contains("ISBN"))
                {
                    foreach (DataRow row in csvTable.Rows)
                    {
                        row["ISBN"] = "\t" + row["ISBN"].ToString();
                    }
                }
                // ✅ REMOVE UNWANTED COLUMNS USING DATATABLE
                string removeColumns = hfRemoveColumnsCSV.Value;

                if (!string.IsNullOrWhiteSpace(removeColumns))
                {
                    string[] cols = removeColumns
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string col in cols)
                    {
                        string colName = col.Trim();

                        if (csvTable.Columns.Contains(colName))
                        {
                            csvTable.Columns.Remove(colName);
                        }
                    }
                }

                // ✅ GENERATE CSV
                StringBuilder sb = CommonFunction.CSVFileGenerationWithoutHeader(csvTable, "Renewals");

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=Renewals.csv");
                Response.ContentType = "text/csv";
                Response.Write(sb.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("Failed to download CSV.", "error");
            }
        }



    }
}
    