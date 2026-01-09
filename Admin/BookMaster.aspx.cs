using BLL;
using Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class BookMaster : System.Web.UI.Page
    {
        MasterBO objMasterBO = new MasterBO();
        private string[] lblErrorMsg = new string[30];
        int intAdminUserID;

        protected void Page_Load(object sender, EventArgs e)

        {
            intAdminUserID = Convert.ToInt32(Session["AdminUserID"]);

            if (!IsPostBack)
            {
                BindCategory();
                BindAuthorList();
                BindBookGrid();


                // If query string contains BookID -> edit mode
                if (!string.IsNullOrEmpty(Request.QueryString["BookID"]))
                {
                    int bookId;
                    if (int.TryParse(Request.QueryString["BookID"], out bookId))
                    {
                        LoadBookForEdit(bookId);
                        btnSave.Visible = false;
                        btnUpdate.Visible = true;
                    }
                }

            }
            //lblErrorMsg = new string[50];
            lblErrorMsg[0] = CommonFunction.GetErrorMessage("", "ERRCOM"); //NO records Found
            lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "BKM001"); // ISBN required
            lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "BKM002"); // Invalid ISBN
            lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "BKM003"); // ISBN < 10
            lblErrorMsg[4] = CommonFunction.GetErrorMessage("", "BKM004"); // ISBN > 13
            lblErrorMsg[5] = CommonFunction.GetErrorMessage("", "BKM005"); // Title required
            lblErrorMsg[6] = CommonFunction.GetErrorMessage("", "BKM006"); // Invalid title
            lblErrorMsg[7] = CommonFunction.GetErrorMessage("", "BKM007"); // Category
            lblErrorMsg[8] = CommonFunction.GetErrorMessage("", "BKM008"); // Author
            lblErrorMsg[9] = CommonFunction.GetErrorMessage("", "BKM009"); // Language
            lblErrorMsg[10] = CommonFunction.GetErrorMessage("", "BKM010"); // Publisher required
            lblErrorMsg[11] = CommonFunction.GetErrorMessage("", "BKM011"); // Invalid publisher
            lblErrorMsg[12] = CommonFunction.GetErrorMessage("", "BKM012"); // Year required
            lblErrorMsg[13] = CommonFunction.GetErrorMessage("", "BKM013"); // 4-digit rule
            lblErrorMsg[14] = CommonFunction.GetErrorMessage("", "BKM014"); // Year > now
            lblErrorMsg[15] = CommonFunction.GetErrorMessage("", "BKM015"); // Year < 1900
            lblErrorMsg[16] = CommonFunction.GetErrorMessage("", "BKM016"); // Edition
            lblErrorMsg[17] = CommonFunction.GetErrorMessage("", "BKM017"); // Price
            lblErrorMsg[18] = CommonFunction.GetErrorMessage("", "BKM018"); // Total copies
            lblErrorMsg[19] = CommonFunction.GetErrorMessage("", "BKM019"); // Available copies
            lblErrorMsg[20] = CommonFunction.GetErrorMessage("", "SUSBOOK01");//insert success
            lblErrorMsg[21] = CommonFunction.GetErrorMessage("", "SUSBOOK02");//update success
            lblErrorMsg[22] = CommonFunction.GetErrorMessage("", "SUSBOOK03");//delete success
            lblErrorMsg[23] = CommonFunction.GetErrorMessage("", "WARBOOK01");//ISBN WARNING
        }
        private void BindCategory()
        {
            try
            {
                using (DataSet ds = objMasterBO.CategoryMaster("SELECT"))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlCategory.DataSource = ds.Tables[0];
                        ddlCategory.DataTextField = "CategoryName";
                        ddlCategory.DataValueField = "CategoryID";
                        ddlCategory.DataBind();
                    }
                    ddlCategory.Items.Insert(0, new ListItem("Select Category", ""));
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);

            }
        }

        private void BindAuthorList()
        {
            try
            {
                using (DataSet ds = objMasterBO.AuthorMaster("SELECT"))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        lstAuthor.DataSource = ds.Tables[0];
                        lstAuthor.DataTextField = "AuthorName";
                        lstAuthor.DataValueField = "AuthorID";
                        lstAuthor.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }

        private void BindBookGrid()
        {
            try
            {
                using (DataSet ds = objMasterBO.BookMaster("SELECT"))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        gvBookMaster.DataSource = dt;
                        gvBookMaster.DataBind();
                        divBookGrid.Visible = true;

                        int totalRecords = dt.Rows.Count;
                        int pageSize = gvBookMaster.PageSize;
                        int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                        BuildPager(totalPages, gvBookMaster.PageIndex);


                    }
                    else
                    {
                        divBookGrid.Visible = false;
                    }
                    lblRecordCount.Text = "Total Books: "+ ds.Tables[0].Rows.Count;
                    BuildPager(gvBookMaster.PageCount, gvBookMaster.PageIndex);
                }

            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }


        }
        private List<string> GetSelectedAuthors()
        {
            List<string> list = new List<string>();

            foreach (ListItem item in lstAuthor.Items)
            {
                if (item.Selected)
                    list.Add(item.Value);
            }

            return list;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string selectedValues = string.Join(",", GetSelectedAuthors());

            //bool isUpdate = btnSave.Text == "Update";
            try
            {
                // Collect fields
                string title = txtBookTitle.Text.Trim();
                string isbn = txtISBN.Text.Trim();
                int categoryId = string.IsNullOrEmpty(ddlCategory.SelectedValue) ? 0 : Convert.ToInt32(ddlCategory.SelectedValue);
                string language = ddlLanguage.SelectedValue;
                string publisher = txtPublisher.Text.Trim();
                string yearText = txtYearPublished.Text.Trim();
                string edition = txtEdition.Text.Trim();
                string priceText = txtPrice.Text.Trim();
                string totalCopiesText = txtTotalCopies.Text.Trim();

                string shelfLocation = txtShelfLocation.Text.Trim();
                bool Active = chkActive.Checked;

                // Authors selected -> build CSV of IDs
                string authorIdsCsv = "";
                foreach (ListItem li in lstAuthor.Items)
                {
                    if (li.Selected)
                    {
                        if (!string.IsNullOrEmpty(authorIdsCsv)) authorIdsCsv += ",";
                        authorIdsCsv += li.Value;
                    }
                }


                if (string.IsNullOrEmpty(title))
                {
                    ShowAlert(lblErrorMsg[5], "error");
                    txtBookTitle.Focus();
                    return;
                }

                // Only letters, numbers, space, and basic punctuation allowed
                string titlePattern = @"^[A-Za-z0-9 ,.:'\-]+$";
                if (!Regex.IsMatch(title, titlePattern))
                {
                    ShowAlert(lblErrorMsg[6], "error");
                    txtBookTitle.Focus();
                    return;
                }
                // Client-like validations on server side (same rules you used for AuthorMaster)
                if (string.IsNullOrWhiteSpace(isbn))
                {
                    ShowAlert(lblErrorMsg[1], "error");
                    txtISBN.Focus();
                    return;
                }
                // ISBN pattern: allow letters, numbers, hyphen
                string pattern = @"^(?:\d[\- ]?){9}[\dX]$|^(?:\d[\- ]?){13}$";
                if (!Regex.IsMatch(isbn, pattern))
                {
                    ShowAlert(lblErrorMsg[2], "error");
                    txtISBN.Focus();
                    return;
                }
                if (isbn.Length < 10)
                {
                    ShowAlert(lblErrorMsg[3], "error");
                    txtISBN.Focus();
                    return;
                }
                // Optional: Maximum length check (ISBN-13)
                if (isbn.Length > 13)
                {
                    ShowAlert(lblErrorMsg[4], "error");
                    txtISBN.Focus();
                    return;
                }



                if (categoryId == 0)
                {
                    ShowAlert(lblErrorMsg[7], "error");
                    ddlCategory.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(authorIdsCsv))
                {
                    ShowAlert(lblErrorMsg[8], "error");
                    return;
                }
                if (ddlLanguage.SelectedIndex == 0 || ddlLanguage.SelectedValue == "0")
                {
                    ShowAlert(lblErrorMsg[9], "error");
                    ddlLanguage.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(publisher))
                {
                    ShowAlert(lblErrorMsg[10], "error");
                    txtPublisher.Focus();
                    return;
                }

                // Allowed: letters, numbers, spaces, dot, comma, hyphen, ampersand
                string publisherpattern = @"^[A-Za-z0-9 .,&\-]+$";
                if (!Regex.IsMatch(publisher, publisherpattern))
                {
                    ShowAlert(lblErrorMsg[11], "error");
                    txtPublisher.Focus();
                    return;
                }
                // 2️⃣ Year Published
                // 1️⃣ Year Published

                if (string.IsNullOrEmpty(yearText))
                {
                    ShowAlert(lblErrorMsg[12], "error");
                    txtYearPublished.Focus();
                    return;
                }
                // Must be 4 digits
                if (!Regex.IsMatch(yearText, @"^\d{4}$"))
                {
                    ShowAlert(lblErrorMsg[13], "error");
                    txtYearPublished.Focus();
                    return;
                }
                // Convert to integer
                int publishedYear = int.Parse(yearText);
                // Year cannot be in the future
                if (publishedYear > DateTime.Now.Year)
                {
                    ShowAlert(lblErrorMsg[14], "error");
                    txtYearPublished.Focus();
                    return;
                }
                // Optional: Minimum year (example: 1900)
                if (publishedYear < 1900)
                {
                    ShowAlert(lblErrorMsg[15], "error");
                    txtYearPublished.Focus();
                    return;
                }
                string editonpattern = @"^[A-Za-z0-9 .\-]+$";
                if (string.IsNullOrWhiteSpace(edition) || !Regex.IsMatch(edition, editonpattern))
                {
                    ShowAlert(lblErrorMsg[16], "error");
                    txtEdition.Focus();
                    return;
                }
                decimal price;
                if (string.IsNullOrWhiteSpace(priceText) || !decimal.TryParse(priceText, out price) || price <= 0 ||
                  decimal.Round(price, 2) != price)
                {
                    ShowAlert("Enter a valid decimal price (up to 2 digits).", "error");
                    txtPrice.Focus();
                    return;
                }

                // 3️⃣ Total Copies
                int totalCopies = 0;
                if (!int.TryParse(totalCopiesText, out totalCopies) || totalCopies <= 0)
                {
                    ShowAlert(lblErrorMsg[18], "error");
                    txtTotalCopies.Focus();
                    return;
                }





                // Call BLL -> DAL via MasterBO (INSERT)
                // Note: param order below should match your MasterBO.BookMaster signature
                using (DataSet ds = objMasterBO.BookMaster("INSERT", 0, isbn, categoryId, title, language, publisher, publishedYear, edition, price,
                    totalCopies, shelfLocation, Active, authorIdsCsv, intAdminUserID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                        if (msgCode == 2)
                        {
                            ShowAlert(lblErrorMsg[23], "error");
                        }

                        if (msgCode == 1)
                        {
                            ShowAlert(lblErrorMsg[20], "success"); // insert success
                            ClearFormFields();
                            divBookGrid.Visible = true;
                            divForm.Visible = false;
                            btnAddBooks.Visible = true;
                            BindBookGrid();


                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("Failed to save book", "error");
            }
        }

        protected void gvBookMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                // Ignore paging commands
                if (e.CommandName == "Page")
                    return;

                string command = e.CommandName;

                // Safely convert only when it is numeric
                int bookId;
                if (!int.TryParse(e.CommandArgument.ToString(), out bookId))
                {
                    ShowAlert("Invalid Book ID.", "error");
                    return;
                }

                if (command == "EditBook")
                {
                    LoadBookForEdit(bookId);
                    divForm.Visible = true;
                    divBookGrid.Visible = false;
                    btnSave.Visible = false;
                    btnUpdate.Visible = true;
                }
                else if (command == "DeleteBook")
                {
                    using (DataSet ds = objMasterBO.BookMaster("DELETE", bookId))
                    {
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                            if (msgCode == 1)
                            {
                                ShowAlert(lblErrorMsg[22], "success");
                                BindBookGrid();
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

        protected void Update_Click(object sender, EventArgs e)
        {
            try
            {
                int bookID = Convert.ToInt32(hdnBookID.Value);
                string isbn = txtISBN.Text.Trim();
                string title = txtBookTitle.Text.Trim();
                int categoryId = string.IsNullOrEmpty(ddlCategory.SelectedValue) ? 0 : Convert.ToInt32(ddlCategory.SelectedValue);
                string language = ddlLanguage.SelectedValue;
                string publisher = txtPublisher.Text.Trim();
                string yearText = txtYearPublished.Text.Trim();
                string edition = txtEdition.Text.Trim();
                string priceText = txtPrice.Text.Trim();
                string totalCopiesText = txtTotalCopies.Text.Trim();
                string shelfLocation = txtShelfLocation.Text.Trim();
                bool Active = chkActive.Checked;

                // Authors CSV
                string authorIdsCsv = "";
                foreach (ListItem li in lstAuthor.Items)
                {
                    if (li.Selected)
                    {
                        if (!string.IsNullOrEmpty(authorIdsCsv)) authorIdsCsv += ",";
                        authorIdsCsv += li.Value;
                    }
                }



                // Update via BLL
                using (DataSet ds = objMasterBO.BookMaster(
                    "UPDATE",
                    bookID,
                    isbn,
                    categoryId,
                    title,
                    language,
                    publisher,
                    Convert.ToInt32(yearText),
                    edition,
                    Convert.ToDecimal(priceText),
                    Convert.ToInt32(totalCopiesText),

                    shelfLocation,
                    Active,
                    authorIdsCsv,
                    intAdminUserID))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);
                        if (msgCode == 2)
                        {
                            ShowAlert(lblErrorMsg[23], "error");
                        }
                        if (msgCode == 1)
                        {
                            ShowAlert(lblErrorMsg[21], "success");
                            ClearFormFields();
                            divBookGrid.Visible = true;
                            divForm.Visible = false;
                            btnAddBooks.Visible = true;
                            BindBookGrid();
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
                ShowAlert("Failed to update book", "error");
            }
        }



        #region Edit helpers

        private DataTable GetBookById(int bookId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (DataSet ds = objMasterBO.BookMaster("SELECTBYID", bookId))
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

        private void LoadBookForEdit(int bookId)
        {
            var dt = GetBookById(bookId);
            if (dt.Rows.Count == 0) return;
            var dr = dt.Rows[0];
            hdnBookID.Value = dr["BookID"].ToString();
            txtISBN.Text = dr["ISBN"].ToString();
            txtBookTitle.Text = dr["BookTitle"].ToString();
            ddlCategory.SelectedValue = dr["CategoryID"].ToString();
            ddlLanguage.SelectedValue = dr["Language"].ToString();
            txtPublisher.Text = dr["PublisherName"].ToString();
            txtYearPublished.Text = dr["YearPublished"].ToString();
            txtEdition.Text = dr["Edition"].ToString();
            txtPrice.Text = dr["Price"].ToString();
            txtTotalCopies.Text = dr["TotalCopies"].ToString();

            txtShelfLocation.Text = dr["ShelfLocation"].ToString();
            chkActive.Checked = Convert.ToBoolean(dr["Active"]);

            // Load selected authors for this book (BookAuthor table select)
            LoadAuthorsForBook(bookId);
        }

        private void LoadAuthorsForBook(int bookId)
        {
            try
            {
                DataTable dt = new DataTable();
                using (DataSet ds = objMasterBO.BookAuthor("SELECTBYBOOK", bookId))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }

                foreach (ListItem li in lstAuthor.Items) li.Selected = false;

                foreach (DataRow r in dt.Rows)
                {
                    string aid = r["AuthorID"].ToString();
                    var li = lstAuthor.Items.FindByValue(aid);
                    if (li != null) li.Selected = true;
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }

        #endregion

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
            txtISBN.Text = "";
            txtBookTitle.Text = "";
            ddlCategory.SelectedIndex = 0;
            foreach (ListItem li in lstAuthor.Items) li.Selected = false;
            ddlLanguage.SelectedIndex = 0;
            txtPublisher.Text = "";
            txtYearPublished.Text = "";
            txtEdition.Text = "";
            txtPrice.Text = "";
            txtTotalCopies.Text = "";
            txtShelfLocation.Text = "";
            chkActive.Checked = true;
            hdnBookID.Value = "0";
        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            ClearFormFields();
        }

        protected void btnAddBooks_Click(object sender, EventArgs e)
        {
            ClearFormFields();
            divBookGrid.Visible=false;
            divForm.Visible=true;
            btnAddBooks.Visible=false;
            btnSave.Visible=true;
            btnUpdate.Visible=false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            divBookGrid.Visible=true;
            divForm.Visible=false;
            btnAddBooks.Visible=true;
        }

        protected void gvBookMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBookMaster.PageIndex = e.NewPageIndex;
            BindBookGrid();
        }

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

                // BookID must be numeric
                if (searchBy == "Category")
                {
                    // Must NOT start with a number + only letters, spaces, allowed chars
                    if (!Regex.IsMatch(searchValue, @"^[A-Za-z][A-Za-z\s\.,:'\-]*$"))
                    {
                        ShowAlert("Category name must start with a letter and contain only valid characters.", "error");
                        txtSearchValue.Focus();
                        return;
                    }
                }

                // BookTitle validation
                if (searchBy == "BookTitle")
                {
                    // Must NOT start with a number + allow letters, numbers, allowed chars
                    if (!Regex.IsMatch(searchValue, @"^[A-Za-z][A-Za-z0-9\s\.,:'\-]*$"))
                    {
                        ShowAlert("Book title must start with a letter and contain only valid characters.", "error");
                        txtSearchValue.Focus();
                        return;
                    }
                }

                // Search call (correct BAL object name)
                DataSet ds = objMasterBO.BookMaster("SEARCH", 0, "", 0, "", "", "", 0, "", 0, 0, "", true,
                "", intAdminUserID, searchBy, searchValue);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvBookMaster.DataSource = ds.Tables[0];

                    gvBookMaster.DataBind();
                    //lblRecordCount.Text ="Total Books:" + gvBookMaster.Rows.Count;
                }
                else
                {
                    ShowAlert("No records found.", "warning");
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("Error: " + ex.Message, "error");
            }
        }


        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            ddlSearchBy.SelectedIndex = 0;
            txtSearchValue.Text = "";

            gvBookMaster.PageIndex = 0;
            BindBookGrid();
        }
        protected void btnDownloadCSV_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = objMasterBO.BookMaster("SELECT");

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    ShowAlert("No data available for export.", "warning");
                    return;
                }

                DataTable sourceTable = ds.Tables[0];
                if (sourceTable.Columns.Contains("ISBN"))
                {
                    foreach (DataRow row in sourceTable.Rows)
                    {
                        row["ISBN"] = "\t" + row["ISBN"].ToString();
                    }
                }
                string removeColumns = hfRemoveColumnsCSV.Value;

                if (!string.IsNullOrWhiteSpace(removeColumns))
                {
                    string[] columnsToRemove = removeColumns.Split(',');

                    foreach (string col in columnsToRemove)
                    {
                        if (ds.Tables[0].Columns.Contains(col))
                            ds.Tables[0].Columns.Remove(col);
                    }
                }
                StringBuilder sb = CommonFunction.CSVFileGeneration(sourceTable, "BookMaster");

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=BookMaster.csv");
                Response.Charset = "";
                Response.ContentType = "text/csv";
                Response.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                ShowAlert("Failed to download CSV.", "error");
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

            gvBookMaster.PageIndex = newIndex;
            BindBookGrid();
        }

    }
}
