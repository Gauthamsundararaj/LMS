using BLL;
using Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        AdminBO objAdminBO = new AdminBO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadDashboard();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        /* ===============================
           LOAD DASHBOARD SUMMARY
        =============================== */
        private void LoadDashboard()
        {
            DateTime fromDate;
            DateTime toDate;
            if (!DateTime.TryParse(txtFromDate.Text, out fromDate)) return;
            if (!DateTime.TryParse(txtToDate.Text, out toDate)) return;

            using (DataSet ds = objAdminBO.GetDashboardData(fromDate, toDate))
            {
                if (ds == null || ds.Tables.Count < 5) return;

                lblTotalBooks.Text = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["TotalBooks"].ToString() : "0";
                lblTotalIssued.Text =ds.Tables[1].Rows.Count > 0 ? ds.Tables[1].Rows[0]["TotalIssued"].ToString() : "0";
                lblDueBooks.Text = ds.Tables[1].Rows.Count > 0 ? ds.Tables[1].Rows[0]["DueBooks"].ToString() : "0";
                lblReturnedBooks.Text = ds.Tables[1].Rows.Count > 0 ? ds.Tables[1].Rows[0]["ReturnedBooks"].ToString() : "0";

                int studentIssued = 0, studentReturned = 0;
                int staffIssued = 0, staffReturned = 0;

                foreach (DataRow row in ds.Tables[3].Rows)
                {
                    bool isStudent = row["IssueType"].ToString().Equals("Student", StringComparison.OrdinalIgnoreCase);
                    bool isIssued = row["Status"].ToString().Equals("Issued", StringComparison.OrdinalIgnoreCase);
                    int total = Convert.ToInt32(row["Total"]);

                    if (isStudent)
                    {
                        if (isIssued) studentIssued = total;
                        else studentReturned = total;
                    }
                    else
                    {
                        if (isIssued) staffIssued = total;
                        else staffReturned = total;
                    }
                }

                hfStudentIssued.Value = studentIssued.ToString();
                hfStudentReturned.Value = studentReturned.ToString();
                hfStaffIssued.Value = staffIssued.ToString();
                hfStaffReturned.Value = staffReturned.ToString();

                int stuIssuedDD = 0, stuReturnedDD = 0;
                int staffIssuedDD = 0, staffReturnedDD = 0;

                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    string issueType = row["IssueType"].ToString().Trim();

                    if (issueType == "Student")
                    {
                        stuIssuedDD   = Convert.ToInt32(row["IssuedCount"]);
                        stuReturnedDD = Convert.ToInt32(row["ReturnedCount"]);
                    }
                    else if (issueType == "Staff")
                    {
                        staffIssuedDD   = Convert.ToInt32(row["IssuedCount"]);
                        staffReturnedDD = Convert.ToInt32(row["ReturnedCount"]);
                    }
                }

                hfStudentIssuedDD.Value   = stuIssuedDD.ToString();
                hfStudentReturnedDD.Value = stuReturnedDD.ToString();
                hfStaffIssuedDD.Value     = staffIssuedDD.ToString();
                hfStaffReturnedDD.Value   = staffReturnedDD.ToString();


                // =====================
                // DAY-WISE BAR CHART
                // =====================

                DataTable dtDayWise = ds.Tables[4];

                int totalDays = (toDate - fromDate).Days + 1;

                // Decide grouping
                int barCount = totalDays <= 7 ? totalDays : 6;
                int daysPerBar = (int)Math.Ceiling((double)totalDays / barCount);

                List<string> labels = new List<string>();
                List<int> issuedList = new List<int>();
                List<int> returnedList = new List<int>();

                DateTime current = fromDate;

                while (current <= toDate)
                {
                    DateTime end = current.AddDays(daysPerBar - 1);
                    if (end > toDate) end = toDate;

                    labels.Add($"{current:dd MMM} - {end:dd MMM}");

                    int issued = dtDayWise.AsEnumerable()
                    .Where(r => r.Field<string>("Status") == "Issued"
                   && r.Field<DateTime>("ActionDate") >= current
                   && r.Field<DateTime>("ActionDate") <= end)
                    .Sum(r => r.Field<int>("Total"));

                    int returned = dtDayWise.AsEnumerable()
                        .Where(r => r.Field<string>("Status") == "Returned"
                                 && r.Field<DateTime>("ActionDate") >= current
                                 && r.Field<DateTime>("ActionDate") <= end)
                        .Sum(r => r.Field<int>("Total"));

                    issuedList.Add(issued);
                    returnedList.Add(returned);

                    current = end.AddDays(1);
                }


                // Store for Chart.js
                hfDayLabels.Value   = string.Join(",", labels);
                hfDayIssued.Value   = string.Join(",", issuedList);
                hfDayReturned.Value = string.Join(",", returnedList);

            }
        }

        /* ===============================
           CARD CLICK → LOAD GRID
        =============================== */

        protected void Card_Click(object sender, CommandEventArgs e)
        {

            DateTime fromDate, toDate;
            if (!DateTime.TryParse(txtFromDate.Text, out fromDate)) return;
            if (!DateTime.TryParse(txtToDate.Text, out toDate)) return;

            string actionType = e.CommandArgument.ToString();
            using (DataSet ds = objAdminBO.GetDashboardGrid(actionType, fromDate, toDate))
            {
                DataTable dt = (ds != null && ds.Tables.Count > 0)
                                ? ds.Tables[0]
                                : new DataTable();

                lblModalTitle.Text = $"{actionType} Books";

                gvData.PageIndex = 0;
                gvData.DataSource = dt;
                gvData.DataBind();
                lblRecordCount.Text = "No. of Records: " + dt.Rows.Count;
                ViewState["GridData"] = dt;
                ViewState["ReportName"] = actionType;

                BuildPager(gvData.PageCount, gvData.PageIndex);
                ShowModal();
            }
        }

        /* ===============================
           PAGER CLICK
        =============================== */
        protected void rptPager_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Page") return;

            DataTable dt = ViewState["GridData"] as DataTable;
            if (dt == null) return;

            int newIndex = Convert.ToInt32(e.CommandArgument);

            // ✅ VALIDATE PAGE INDEX
            if (newIndex < 0) newIndex = 0;
            if (newIndex >= gvData.PageCount) newIndex = gvData.PageCount - 1;

            gvData.PageIndex = newIndex;
            gvData.DataSource = dt;
            gvData.DataBind();

            if (dt.Rows.Count > gvData.PageSize)
            {
                BuildPager(gvData.PageCount, gvData.PageIndex);
            }
            else
            {
                rptPager.DataSource = null;
                rptPager.DataBind();
            }
            ShowModal();
        }

        private void BuildPager(int totalPages, int currentPage)
        {
            // ❌ NO PAGINATION WHEN 0 OR 1 PAGE
            if (totalPages <= 1)
            {
                rptPager.DataSource = null;
                rptPager.DataBind();
                return;
            }

            var pages = new List<object>();
            int maxPagesToShow = 3;

            pages.Add(new
            {
                PageIndex = currentPage - 1,
                Text = "« Previous",
                Command = "Page",
                Enabled = currentPage > 0,
                IsActive = false
            });

            int startPage = Math.Max(0, currentPage - 1);
            int endPage = Math.Min(totalPages - 1, startPage + maxPagesToShow - 1);

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

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ GET DATA FROM VIEWSTATE
                DataTable dt = ViewState["GridData"] as DataTable;
                string reportName = ViewState["ReportName"]?.ToString() ?? "Report";

                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowAlert("No data available for export.", "warning");
                    return;
                }

                // ✅ GENERATE CSV CONTENT
                StringBuilder sb =
                    CommonFunction.CSVFileGenerationWithoutHeader(dt, reportName);

                // ✅ WRITE RESPONSE
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader(
                    "Content-Disposition",
                    $"attachment;filename={reportName}.csv"
                );
                Response.ContentType = "text/csv";
                Response.Write(sb.ToString());

                // ✅ SAFE REQUEST TERMINATION
                Response.Flush();
                Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();

                // ✅ CORRECT CLEANUP
                ViewState.Remove("GridData");   // release reference
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("Failed to download CSV.", "error");
            }
        }



        private void ShowModal()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowModal", "$(document).ready(function(){ $('#dataModal').modal('show');initPagination();  });", true);
        }

        private void ShowAlert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(
                this, GetType(), Guid.NewGuid().ToString(),
                $"AlertMessage('{msg}','{type}');", true);
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                if (objAdminBO != null)
                {
                    objAdminBO.ReleaseResources();
                    objAdminBO = null; 
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }

    }
}
