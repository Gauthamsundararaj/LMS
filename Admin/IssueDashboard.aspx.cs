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
    public partial class IssueDashboard : System.Web.UI.Page

    {
        CommonBO objCommonBO = new CommonBO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboard();
            }
        }

        /* ================= LOAD COUNTS ================= */
        private void LoadDashboard()
        {
            try
            {
                using (DataSet ds = objCommonBO.GetBookIssueDashboard("TOTAL_COUNT"))
                {

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];

                        lblTotalBooks.Text    = dt.Rows[0]["TotalBooks"].ToString();
                        lblIssuedBooks.Text   = GetCount("ISSUED_COUNT");
                        lblDueBooks.Text      = GetCount("DUE_COUNT");
                        lblReturnedBooks.Text = GetCount("RETURNED_COUNT");

                        BindGrid("TOTAL_GRID", true);
                    }
                    else
                    {
                        ClearGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }

        private string GetCount(string action)
        {
            using (DataSet ds = objCommonBO.GetBookIssueDashboard(action))
            {
                return (ds != null && ds.Tables[0].Rows.Count > 0)
                       ? ds.Tables[0].Rows[0][0].ToString()
                       : "0";
            }
        }

        private void BindGrid(string type, bool resetPageIndex = false)
        {
            divPager.Visible = lnkDownloadCSV.Visible = false;
            CurrentGridType = type;
            SetCSVHiddenColumns(type);

            if (!IsRefresh)
                SaveGridFilters();
            DataSet ds = objCommonBO.GetBookIssueDashboard(type);

            using (ds)
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ApplyFiltersUsingViewState(ref ds);
                    ViewState["GridData"] = ds.Tables[0].DefaultView.ToTable();
                    gvBooks.DataSource = ds.Tables[0].DefaultView;
                    gvBooks.DataBind();
                    if (resetPageIndex)
                        gvBooks.PageIndex = 0;

                    int intRowCount = ds.Tables[0].Rows.Count;

                    // ✅ hide pager + CSV when empty
                    divPager.Visible = lnkDownloadCSV.Visible = true;

                    // ✅ custom pager
                    if (intRowCount > gvBooks.PageSize)
                    {
                        BuildPager(gvBooks.PageCount, gvBooks.PageIndex);
                        rptPager.Visible = true;
                    }
                    else
                        rptPager.Visible = false;
                    if (type.ToUpper() == "TOTAL_GRID")
                    {
                        lblGridTitle.InnerText = "Book Details";
                        gvBooks.Columns[9].Visible = true;
                        gvBooks.Columns[10].Visible = true;
                        gvBooks.Columns[11].Visible = true;
                    }
                    else if (type.ToUpper() == "ISSUED_GRID")
                    {
                        lblGridTitle.InnerText = "Issued Book Details";
                        gvBooks.Columns[9].Visible = false;
                        gvBooks.Columns[10].Visible = false;
                        gvBooks.Columns[11].Visible = false;
                    }
                    else if (type.ToUpper() == "DUE_GRID")
                    {
                        lblGridTitle.InnerText = "Due Book Details";
                        gvBooks.Columns[9].Visible = false;
                        gvBooks.Columns[10].Visible = false;
                        gvBooks.Columns[11].Visible = false;
                    }
                    else if (type.ToUpper() == "RETURNED_GRID")
                    {
                        lblGridTitle.InnerText = "Returned Book Details";
                        gvBooks.Columns[9].Visible = false;
                        gvBooks.Columns[10].Visible = true;
                        gvBooks.Columns[11].Visible = false;
                    }

                    RestoreGridFilters();
                }

            }
        }
            /*

            DataSet ds = null;

            switch (type)
            {
                case "TOTAL":
                    ds = objCommonBO.GetBookIssueDashboard("TOTAL_GRID");
                    lblGridTitle.InnerText = "Book Details";
                    lblGridTitle.InnerText = "Book Details"; 
                    gvBooks.Columns[9].Visible = true; 
                    gvBooks.Columns[10].Visible = true; 
                    gvBooks.Columns[11].Visible = true; 
                    break;

                case "ISSUED":
                    ds = objCommonBO.GetBookIssueDashboard("ISSUED_GRID");
                    lblGridTitle.InnerText = "Issued Book Details";
                    gvBooks.Columns[9].Visible = false; 
                    gvBooks.Columns[10].Visible = false; 
                    gvBooks.Columns[11].Visible = false; 
                    break;

                case "DUE":
                    ds = objCommonBO.GetBookIssueDashboard("DUE_GRID");
                    lblGridTitle.InnerText = "Due Book Details";
                    gvBooks.Columns[9].Visible = false; 
                    gvBooks.Columns[10].Visible = false; 
                    gvBooks.Columns[11].Visible = false; 
                    break;

                case "RETURNED":
                    ds = objCommonBO.GetBookIssueDashboard("RETURNED_GRID");
                    lblGridTitle.InnerText = "Returned Book Details";
                    gvBooks.Columns[9].Visible = false; 
                    gvBooks.Columns[10].Visible = true; 
                    gvBooks.Columns[11].Visible = false; 
                    break;
            }

            if (resetPageIndex)
                gvBooks.PageIndex = 0;

            ApplyFiltersUsingViewState(ref ds);

            // store filtered data for CSV
            ViewState["GridData"] = ds.Tables[0].DefaultView.ToTable();

            gvBooks.DataSource = ds.Tables[0].DefaultView;
            gvBooks.DataBind();
            gvBooks.DataSource = ds.Tables[0].DefaultView;
            gvBooks.DataBind();

            int filteredCount = ds.Tables[0].DefaultView.Count;

            // ✅ hide pager + CSV when empty
            divPager.Visible = filteredCount > 0;
            lnkDownloadCSV.Visible = filteredCount > 0;

            // ✅ custom pager
            if (filteredCount > gvBooks.PageSize)
            {
                BuildPager(gvBooks.PageCount, gvBooks.PageIndex);
                rptPager.Visible = true;
            }
            else
            {
                rptPager.Visible = false;
            }
            RestoreGridFilters();
            */


        protected void CardTotalBooks_Click(object sender, EventArgs e)
        {
            CurrentGridType = "TOTAL";
            BindGrid("TOTAL_GRID", true);
        }

        protected void CardIssuedBooks_Click(object sender, EventArgs e)
        {
            CurrentGridType = "ISSUED";
            BindGrid("ISSUED_GRID", true);
        }

        protected void CardDueBooks_Click(object sender, EventArgs e)
        {
            CurrentGridType = "DUE";
            BindGrid("DUE_GRID", true);
        }

        protected void CardReturnedBooks_Click(object sender, EventArgs e)
        {
            CurrentGridType = "RETURNED";
            BindGrid("RETURNED_GRID", true);
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

        private string FilterMemberID
        {
            get { return ViewState["MemberID"]?.ToString(); }
            set { ViewState["MemberID"] = value; }
        }

        private string FilterMemberType
        {
            get { return ViewState["MemberType"]?.ToString(); }
            set { ViewState["MemberType"] = value; }

        }

        private void SaveGridFilters()
        {
            if (gvBooks.HeaderRow == null) return;

            FilterISBN = (gvBooks.HeaderRow.FindControl("txtFilterISBN") as TextBox)?.Text.Trim();
            FilterBookTitle = (gvBooks.HeaderRow.FindControl("txtFilterBookTitle") as TextBox)?.Text.Trim();
            FilterMemberID = (gvBooks.HeaderRow.FindControl("txtFilterMemberID") as TextBox)?.Text.Trim();
            FilterMemberType = (gvBooks.HeaderRow.FindControl("txtFilterMemberType") as TextBox)?.Text.Trim();
        }
        private void RestoreGridFilters()
        {
            if (gvBooks.HeaderRow == null) return;

            TextBox txtISBN = gvBooks.HeaderRow.FindControl("txtFilterISBN") as TextBox;
            if (txtISBN != null)
                txtISBN.Text = FilterISBN ?? string.Empty;

            TextBox txtTitle = gvBooks.HeaderRow.FindControl("txtFilterBookTitle") as TextBox;
            if (txtTitle != null)
                txtTitle.Text = FilterBookTitle ?? string.Empty;

            TextBox txtMemberID = gvBooks.HeaderRow.FindControl("txtFilterMemberID") as TextBox;
            if (txtMemberID != null)
                txtMemberID.Text = FilterMemberID ?? string.Empty;

            TextBox txtMemberType = gvBooks.HeaderRow.FindControl("txtFilterMemberType") as TextBox;
            if (txtMemberType != null)
                txtMemberType.Text = FilterMemberType ?? string.Empty;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvBooks.PageIndex = 0;
            BindGrid(CurrentGridType, false);
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            // 🔥 FORCE CLEAR FILTERS FIRST
            IsRefresh = true;
            ClearAllFilters();

            // 🔥 RESET PAGE
            gvBooks.PageIndex = 0;

            // 🔥 REBIND GRID WITHOUT FILTERS
            BindGrid(CurrentGridType, true);

            IsRefresh = false;
        }

        private void ClearAllFilters()
        {
            FilterISBN = string.Empty;
            FilterBookTitle = string.Empty;
            FilterMemberID = string.Empty;
            FilterMemberType = string.Empty;
        }


        private string Safe(string value)
        {
            return string.IsNullOrEmpty(value) ? value : value.Replace("'", "''");
        }

        private void ApplyFiltersUsingViewState(ref DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return;

            // 🚫 NO FILTERS → SHOW ALL
            if (string.IsNullOrWhiteSpace(FilterISBN) &&
                string.IsNullOrWhiteSpace(FilterBookTitle) &&
                string.IsNullOrWhiteSpace(FilterMemberID) &&
                string.IsNullOrWhiteSpace(FilterMemberType))
            {
                ds.Tables[0].DefaultView.RowFilter = string.Empty;
                return;
            }

            string filter = "1=1";

            if (!string.IsNullOrWhiteSpace(FilterISBN))
                filter += $" AND ISBN LIKE '%{Safe(FilterISBN)}%'";

            if (!string.IsNullOrWhiteSpace(FilterBookTitle))
                filter += $" AND [Book Title] LIKE '%{Safe(FilterBookTitle)}%'";

            if (!string.IsNullOrWhiteSpace(FilterMemberID))
                filter += $" AND MemberID LIKE '%{Safe(FilterMemberID)}%'";

            if (!string.IsNullOrWhiteSpace(FilterMemberType))
                filter += $" AND [Member Type] LIKE '%{Safe(FilterMemberType)}%'";

            ds.Tables[0].DefaultView.RowFilter = filter;
        }

        protected string GetStatusCssClass(object dataItem)
        {
            string status = DataBinder.Eval(dataItem, "Status").ToString();
            DateTime dueDate = Convert.ToDateTime(DataBinder.Eval(dataItem, "Due Date"));

            if (status == "Returned")
                return "badge bg-success";

            if (dueDate < DateTime.Today)
                return "badge bg-danger";

            return "badge bg-warning";
        }
        protected void Filter_TextChanged(object sender, EventArgs e)
        {
            gvBooks.PageIndex = 0;
            BindGrid(CurrentGridType);
        }


        private string CurrentGridType
        {
            get { return ViewState["CurrentGridType"]?.ToString() ?? "TOTAL"; }
            set { ViewState["CurrentGridType"] = value; }
        }
        private bool IsRefresh
        {
            get
            {
                return ViewState["IsRefresh"] != null && (bool)ViewState["IsRefresh"];
            }
            set
            {
                ViewState["IsRefresh"] = value;
            }
        }

        private void SetCSVHiddenColumns(string gridType)
        {
            switch (gridType)
            {
                case "TOTAL":
                    hfRemoveColumnsCSV.Value =
                        "IssueID,BookID";
                    break;

                case "ISSUED":
                    hfRemoveColumnsCSV.Value =
                        "IssueID,BookID,Returned Date,Status";
                    break;
                case "DUE":
                    hfRemoveColumnsCSV.Value =
                        "IssueID,BookID,Returned Date,Status";
                    break;

                case "RETURNED":
                    hfRemoveColumnsCSV.Value =
                        "IssueID,BookID,Status,RenewalCount";
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
                    ShowToastr("No data available for export.", "warning");
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
                StringBuilder sb = CommonFunction.CSVFileGeneration(csvTable, "");

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=BookDetails.csv");
                Response.ContentType = "text/csv";
                Response.Write(sb.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr("Failed to download CSV.", "error");
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

            gvBooks.PageIndex = newIndex;
            string type = ViewState["CurrentGridType"]?.ToString() ?? "TOTAL";
            BindGrid(CurrentGridType, false);

        }

        private void ClearGrid()
        {
            gvBooks.DataSource = null;
            gvBooks.DataBind();

            lblTotalBooks.Text = "0";
            lblIssuedBooks.Text = "0";
            lblDueBooks.Text = "0";
            lblReturnedBooks.Text = "0";

            lblGridTitle.InnerText = "Books";
        }

        private void ShowToastr(string message, string alertType = "error")
        {
            ScriptManager.RegisterStartupScript(
                this, GetType(), Guid.NewGuid().ToString(),
                $"$(function(){{ AlertMessage('{message.Replace("'", "\\'")}', '{alertType.ToLower()}'); }});",
                true
            );
        }
    }
}
