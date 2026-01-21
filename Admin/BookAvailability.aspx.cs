using BLL;
using Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class BookAvailability : System.Web.UI.Page
    {
        private string[] lblErrorMsg = new string[20];
        MasterBO objMasterBO = new MasterBO();
        CommonBO objCommonBO = new CommonBO();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ErrorMessages();
                hfIsFirstTime.Value = "0";
                if (!IsPostBack)
                {
                    hfIsFirstTime.Value = "1";
                    LoadCategories();
                    BindBookAvailability(false);
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[5], "error");
            }
        }

        private void ErrorMessages()
        {
            lblErrorMsg[0] = CommonFunction.GetErrorMessage("", "ERRBA0001");   //Please select at least one category
            lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "ERRBA0002");   //Please select a Search By option.
            lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "ERRBA0003");   //Please enter a search value.
            lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "ERRBA0004");   //Search value must contain at least 2 characters.
            lblErrorMsg[4] = CommonFunction.GetErrorMessage("", "ERRBA0005");   //Error occurred while loading book availability.
            lblErrorMsg[5] = CommonFunction.GetErrorMessage("", "ERRBA0006");   //Error occurred while searching books.
            lblErrorMsg[6] = CommonFunction.GetErrorMessage("", "ERRBA0007");   //No records found.
            lblErrorMsg[7] = CommonFunction.GetErrorMessage("", "ERRBA0008");   //No books available for the selected category.
            lblErrorMsg[8] = CommonFunction.GetErrorMessage("", "ERRBA0009");   //No data available to download.
            lblErrorMsg[9] = CommonFunction.GetErrorMessage("", "ERRBA0010");   //Books loaded successfully.
            lblErrorMsg[10]= CommonFunction.GetErrorMessage("", "ERRBA0011");   //Book availability exported successfully.
        }
        private void LoadCategories()
        {
            try
            {
                using (DataSet ds = objMasterBO.CategoryMaster("SELECT"))
                {
                    ddlCategory.Items.Clear();

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlCategory.DataSource = ds.Tables[0];
                        ddlCategory.DataTextField = "CategoryName";
                        ddlCategory.DataValueField = "CategoryID";
                        ddlCategory.DataBind();
                    }
                    foreach (ListItem item in ddlCategory.Items)
                    {
                        item.Selected = true;
                    }
                    BindBookAvailability(false);
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            var selectedCategories = ddlCategory.Items
                .Cast<ListItem>()
                .Where(i => i.Selected)
                .ToList();


            if (selectedCategories.Count == 0)
            {
                gvbookA.Visible = false;
                gvbookA.DataSource = null;
                gvbookA.DataBind();
                lblRecordCount.Text = string.Empty;
                pnlGrid.Visible=false;
                ShowToastr(lblErrorMsg[0], "error");
                return;
            }

            BindBookAvailability(false);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ViewState["ISBN"] = null;
            ViewState["BookTitle"] = null;
            ViewState["Author"] = null;
            ViewState["Year"] = null;
            ViewState["Publisher"] = null;

            gvbookA.PageIndex = 0;

            foreach (ListItem item in ddlCategory.Items)
            {
                item.Selected = true;
            }
            BindBookAvailability(false);
        }

        protected void BindBookAvailability(bool showMessage = true)
        {
            try
            {
                if (!IsPostBack && !showMessage)
                {
                    showMessage = false;
                }

                if (!IsRefresh)
                {
                    SaveGridFilters();
                }

                var selectedCategories = ddlCategory.Items.Cast<ListItem>()
                    .Where(i => i.Selected)
                    .Select(i => i.Text)
                    .ToList();

                if (showMessage && selectedCategories.Count == 0)
                {
                    ShowToastr(lblErrorMsg[0], "error");
                    return;
                }

                string categoryNames = string.Join(",", selectedCategories);

                string isbn = ViewState["ISBN"]?.ToString();
                string bookTitle = ViewState["BookTitle"]?.ToString();
                string authorName = ViewState["Author"]?.ToString();
                string publisherName = ViewState["Publisher"]?.ToString();

                int? yearPublished = null;
                int y;

                if (int.TryParse(Convert.ToString(ViewState["Year"]), out y))
                {
                    yearPublished = y;
                }

                DataSet ds = objCommonBO.GetBookAvailability(
                    categoryNames, isbn, bookTitle, authorName, yearPublished, publisherName);

                pnlGrid.Visible = true;

                gvbookA.DataSource = ds?.Tables[0];
                gvbookA.DataBind();
                int totalRecords = ds.Tables[0].Rows.Count;
                int pageSize = gvbookA.PageSize;
                int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                if (ds.Tables[0].Rows.Count > gvbookA.PageSize)
                {
                    BuildPager(totalPages, gvbookA.PageIndex);
                    rptPager.Visible = true;
                }
                else
                {
                    rptPager.Visible = false;
                }
                
                RestoreGridFilters();

                int recordCount = ds?.Tables[0].Rows.Count ?? 0;

                lblRecordCount.Text = "No of Records : " + recordCount;

                ViewState["BookAvailabilityDS"] = recordCount > 0 ? ds : null;

                if (!showMessage) return;

                if (recordCount > 0)
                {
                    ShowToastr(lblErrorMsg[9], "success"); // Books loaded successfully
                }
                else
                {
                    ShowToastr(lblErrorMsg[7], "warning"); // No books available for selected category
                }
                BuildPager(gvbookA.PageCount, gvbookA.PageIndex);
            }
            catch (Exception ex)
            {
                pnlGrid.Visible = false;
                ClearGrid();
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[4], "error");
            }
        }

        private void SaveGridFilters()
        {
            if (gvbookA.HeaderRow == null) return;

            ViewState["ISBN"] =
                (gvbookA.HeaderRow.FindControl("txtFilterISBN") as TextBox)?.Text?.Trim();

            ViewState["BookTitle"] =
                (gvbookA.HeaderRow.FindControl("txtFilterBookTitle") as TextBox)?.Text?.Trim();

            ViewState["Author"] =
                (gvbookA.HeaderRow.FindControl("txtFilterAuthor") as TextBox)?.Text?.Trim();

            ViewState["Year"] =
                (gvbookA.HeaderRow.FindControl("txtFilterYear") as TextBox)?.Text?.Trim();

            ViewState["Publisher"] =
                (gvbookA.HeaderRow.FindControl("txtFilterPublisher") as TextBox)?.Text?.Trim();
        }

        private void RestoreGridFilters()
        {
            if (gvbookA.HeaderRow == null) return;

            (gvbookA.HeaderRow.FindControl("txtFilterISBN") as TextBox).Text =
                ViewState["ISBN"]?.ToString();

            (gvbookA.HeaderRow.FindControl("txtFilterBookTitle") as TextBox).Text =
                ViewState["BookTitle"]?.ToString();

            (gvbookA.HeaderRow.FindControl("txtFilterAuthor") as TextBox).Text =
                ViewState["Author"]?.ToString();

            (gvbookA.HeaderRow.FindControl("txtFilterYear") as TextBox).Text =
                ViewState["Year"]?.ToString();

            (gvbookA.HeaderRow.FindControl("txtFilterPublisher") as TextBox).Text =
                ViewState["Publisher"]?.ToString();
        }

        protected void btnDownloadCSV_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedCategories = ddlCategory.Items.Cast<ListItem>()
                    .Where(i => i.Selected)
                    .Select(i => i.Text)
                    .ToList();

                if (selectedCategories.Count == 0)
                {
                    ShowToastr(lblErrorMsg[0], "error");
                    return;
                }

                string categoryNames = string.Join(",", selectedCategories);

                string isbn = ViewState["ISBN"]?.ToString();
                string bookTitle = ViewState["BookTitle"]?.ToString();
                string authorName = ViewState["Author"]?.ToString();
                string publisherName = ViewState["Publisher"]?.ToString();

                int? yearPublished = null;
                int y;
                if (int.TryParse(Convert.ToString(ViewState["Year"]), out y))
                {
                    yearPublished = y;
                }

                DataSet ds = objCommonBO.GetBookAvailability(
                    categoryNames, isbn, bookTitle, authorName, yearPublished, publisherName);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    ShowToastr(lblErrorMsg[8], "warning");
                    return;
                }

                DataTable sourceTable = ds.Tables[0];

                if (sourceTable.Columns.Contains("ISBN"))
                {
                    foreach (DataRow row in sourceTable.Rows)
                    {
                        row["ISBN"] = "\t" + row["ISBN"];
                    }
                }

                string removeColumns = hfRemoveColumnsCSV.Value;
                if (!string.IsNullOrWhiteSpace(removeColumns))
                {
                    foreach (string col in removeColumns.Split(','))
                    {
                        if (sourceTable.Columns.Contains(col))
                            sourceTable.Columns.Remove(col);
                    }
                }

                StringBuilder sb = CommonFunction.CSVFileGenerationWithoutHeader(sourceTable, "BookAvailability");

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=BookAvailability.csv");
                Response.Charset = "";
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


        protected void gvbookA_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvbookA.PageIndex = e.NewPageIndex;

            if (ViewState["BookAvailabilityDS"] != null)
            {
                DataSet ds = (DataSet)ViewState["BookAvailabilityDS"];
                gvbookA.DataSource = ds.Tables[0];
                gvbookA.DataBind();
                RestoreGridFilters();
                lblRecordCount.Text = "No of Records : " + ds.Tables[0].Rows.Count;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedCategories = ddlCategory.Items.Cast<ListItem>()
                    .Where(i => i.Selected)
                    .ToList();

                if (selectedCategories.Count == 0)
                {
                    ShowToastr(lblErrorMsg[0], "error");
                    return;
                }

                if (!HasAnyGridFilter())
                {
                    ShowToastr("Please enter at least one filter value", "warning");
                    return;
                }
                TextBox txtISBN = gvbookA.HeaderRow?.FindControl("txtFilterISBN") as TextBox;
                TextBox txtYear = gvbookA.HeaderRow?.FindControl("txtFilterYear") as TextBox;

                string isbn = txtISBN?.Text.Trim();
                string year = txtYear?.Text.Trim();

                if (!string.IsNullOrEmpty(isbn) && !isbn.All(char.IsDigit))
                {
                    ShowToastr("ISBN must contain numbers only.", "warning");
                    return;
                }

                if (!string.IsNullOrEmpty(year))
                {
                    if (!year.All(char.IsDigit))
                    {
                        ShowToastr("Year must contain numbers only.", "warning");
                        return;
                    }

                    if (year.Length != 4)
                    {
                        ShowToastr("Year must be a 4-digit value.", "warning");
                        return;
                    }
                }

                hfIsFirstTime.Value = "0";
                gvbookA.PageIndex = 0;
                BindBookAvailability(false);
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowToastr(lblErrorMsg[5], "error");
            }
        }

        private bool HasAnyGridFilter()
        {
            if (gvbookA.HeaderRow == null)
                return false;

            TextBox txtISBN = gvbookA.HeaderRow.FindControl("txtFilterISBN") as TextBox;
            TextBox txtBookTitle = gvbookA.HeaderRow.FindControl("txtFilterBookTitle") as TextBox;
            TextBox txtAuthor = gvbookA.HeaderRow.FindControl("txtFilterAuthor") as TextBox;
            TextBox txtYear = gvbookA.HeaderRow.FindControl("txtFilterYear") as TextBox;
            TextBox txtPublisher = gvbookA.HeaderRow.FindControl("txtFilterPublisher") as TextBox;

            bool hasText =
                !string.IsNullOrWhiteSpace(txtISBN?.Text) ||
                !string.IsNullOrWhiteSpace(txtBookTitle?.Text) ||
                !string.IsNullOrWhiteSpace(txtAuthor?.Text) ||
                !string.IsNullOrWhiteSpace(txtPublisher?.Text)||
                !string.IsNullOrWhiteSpace(txtYear?.Text);

            return hasText;
        }
        private bool IsRefresh
        {
            get { return ViewState["IsRefresh"] != null && (bool)ViewState["IsRefresh"]; }
            set { ViewState["IsRefresh"] = value; }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            IsRefresh = true;

            ViewState["ISBN"] = null;
            ViewState["BookTitle"] = null;
            ViewState["Author"] = null;
            ViewState["Year"] = null;
            ViewState["Publisher"] = null;

            foreach (ListItem item in ddlCategory.Items)
            {
                item.Selected = true;
            }

            gvbookA.PageIndex = 0;
            BindBookAvailability(false);
            IsRefresh = false;
        }

        private void ClearGrid()
        {
            gvbookA.DataSource = null;
            gvbookA.DataBind();
        }

        private void ShowToastr(string message, string alertType = "error")
        {
            ScriptManager.RegisterStartupScript(
                this, GetType(), Guid.NewGuid().ToString(),
                $"$(function(){{ AlertMessage('{message.Replace("'", "\\'")}', '{alertType.ToLower()}'); }});",
                true
            );
        }
        private void BuildPager(int totalPages, int currentPage)
        {
            var pages = new List<object>();
            int maxPagesToShow = 3;

            int startPage = Math.Max(0, currentPage - 1);
            int endPage = Math.Min(totalPages - 1, startPage + maxPagesToShow - 1);

            if (endPage - startPage < maxPagesToShow - 1)
                startPage = Math.Max(0, endPage - maxPagesToShow + 1);

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

            gvbookA.PageIndex = newIndex;
            BindBookAvailability(false);
        }
    }
}
