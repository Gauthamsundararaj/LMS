using Admin;
using BLL;
using Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class BookIssue : System.Web.UI.Page
    {
        MasterBO objMasterBO = new MasterBO();
        CommonBO objCommonBO = new CommonBO();
        int intAdminUserID;

        protected void Page_Load(object sender, EventArgs e)
        {
            intAdminUserID = Convert.ToInt32(Session["AdminUserID"]);
            if (!IsPostBack)
            {
                BindBookList();
            }
        }

        protected void rblIssueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            divStudent.Visible = false;
            divStaff.Visible = false;
            divIssueDetails.Visible = false;

            if (rblIssueType.SelectedValue == "Student")
            {
                divStudent.Visible = true;
                divIssueDetails.Visible = true;
                btnSave.Visible = true;
                btnClear.Visible = true;
                btnCancel.Visible = true;
            }
            else if (rblIssueType.SelectedValue == "Staff")
            {
                divStaff.Visible = true;
                divIssueDetails.Visible = true;
                btnSave.Visible = true;
                btnClear.Visible = true;
                btnCancel.Visible = true;
            }
        }

        private void BindBookList()
        {
            try
            {
                using (DataSet ds = objMasterBO.BookMaster("SELECTISBN"))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        lstIsbn.DataSource = ds.Tables[0];
                        lstIsbn.DataTextField = "ISBN";
                        lstIsbn.DataValueField = "BookID";
                        lstIsbn.DataBind();
                    }
                    else
                    {
                        lstIsbn.Items.Clear();
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

        // NEW METHOD → MEMBER VALIDATION FROM DB
        private string ValidateMemberFromDB(string action, string MemberID, string IssueType)
        {
            try
            {
                using (DataSet ds = objCommonBO.ValidateMember("CheckMember", MemberID, IssueType))
                {

                    if (ds == null || ds.Tables.Count == 0)
                        return null;

                    DataTable dt = ds.Tables[0];

                    if (dt.Rows.Count == 0)
                        return null;
                    //bool isActive = Convert.ToBoolean(dt.Rows[0]["Active"]);
                    //if (!isActive)
                    //    return null;
                    return dt.Rows[0]["MemberID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
                return null;   // safe exit
            }
        }



        private void BindSelectedBooksGrid()
        {
            string ISBNValues = "";

            foreach (ListItem li in lstIsbn.Items)
            {
                if (li.Selected)
                {
                    ISBNValues += $"{li.Text},";
                }
            }

            if (!string.IsNullOrWhiteSpace(ISBNValues))
            {
                ISBNValues = ISBNValues.TrimEnd(',');
            }

            using (DataSet ds = objMasterBO.BookMaster("SEARCH", 0, "", 0, "", "", "", 0, "", 0, 0, "", true, "",
                    intAdminUserID, "ISBN", ISBNValues))
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gvSelectedBooks.DataSource = ds.Tables[0];
                    gvSelectedBooks.DataBind();
                    pnlConfirm.Visible = true;
                }
                else
                {
                    ShowAlert("No records found.", "warning");
                    pnlConfirm.Visible = false;
                }
            }

            pnlConfirm.Visible = true;
        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {
            try
            {
                string issueType = rblIssueType.SelectedValue;
                string studentId = txtStudentID.Text.Trim();
                string staffId = txtStaffID.Text.Trim();
                string issueDateText = issueDate.Value;
                string dueDateText = dueDate.Value;
                string action = "";
                string bookIdsCsv = "";
                string MemberID = "";

                // Collect Selected Books
                foreach (ListItem li in lstIsbn.Items)
                {
                    if (li.Selected)
                    {
                        if (bookIdsCsv != "")
                            bookIdsCsv += ",";
                        bookIdsCsv += li.Value;
                    }
                }
                // 1️⃣ Issue Type
                if (string.IsNullOrWhiteSpace(issueType))
                {
                    ShowAlert("Please select Issue Type.");
                    return;
                }
                if (issueType == "Student")
                {
                    if (string.IsNullOrWhiteSpace(studentId))
                    {
                        ShowAlert("Student ID is required.");
                        return;
                    }
                    if (!Regex.IsMatch(studentId, @"^[A-Za-z0-9\-]+$"))
                    {
                        ShowAlert("Invalid Student ID.", "error");
                        txtStudentID.Focus();
                        return;
                    }

                    MemberID = ValidateMemberFromDB(action, studentId, "Student");

                    if (MemberID == null)
                    {
                        ShowAlert("Student ID not found or inactive.");
                        return;
                    }
                }
                else if (issueType == "Staff")
                {
                    if (string.IsNullOrWhiteSpace(staffId))
                    {
                        ShowAlert("Staff ID is required.");
                        return;
                    }
                    if (!Regex.IsMatch(staffId, @"^[A-Za-z0-9\-]+$"))
                    {
                        ShowAlert("Invalid Staff ID.", "error");
                        txtStaffID.Focus();
                        return;
                    }

                    MemberID = ValidateMemberFromDB(action, staffId, "Staff");
                    if (MemberID == null)
                    {
                        ShowAlert("Staff ID is Not found or inactive.", "warning");
                        return;
                    }
                }

                // 3️⃣ ISBN Validate
                if (string.IsNullOrWhiteSpace(bookIdsCsv))
                {
                    ShowAlert("Select at least one ISBN.", "warning");
                    return;
                }
                DateTime issueDt;
                if (string.IsNullOrWhiteSpace(issueDate.Value)
                    || !DateTime.TryParse(issueDate.Value, out issueDt))
                {
                    ShowAlert("Invalid Issue Date.");
                    return;
                }


                DateTime dueDt;
                if (string.IsNullOrWhiteSpace(dueDate.Value)
                    || !DateTime.TryParse(dueDate.Value, out dueDt))
                {
                    ShowAlert("Invalid Due Date.");
                    return;
                }

                if (dueDt <= issueDt)
                {
                    ShowAlert("Due Date must be greater than Issue Date.");
                    return;
                }

                ViewState["MemberID"] = MemberID;
                ViewState["IssueType"] = issueType;
                ViewState["IssueDate"] = issueDt;
                ViewState["DueDate"] = dueDt;
                ViewState["bookIdsCsv"] = bookIdsCsv;

                BindSelectedBooksGrid();
                divGrid.Visible=true;
                btnSave.Visible = false;
                btnClear.Visible = false;
                btnCancel.Visible = false;
            }


            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);

            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {

                if (ViewState["MemberID"] == null ||
                   ViewState["IssueType"] == null ||
                   ViewState["IssueDate"] == null ||
                    ViewState["DueDate"] == null||
                        ViewState["bookIdsCsv"]==null)
                {
                    ShowAlert("Session expired. Please click Save again.");
                    return;
                }
                string MemberID = ViewState["MemberID"].ToString();
                string issueType = ViewState["IssueType"].ToString();
                DateTime issueDt = (DateTime)ViewState["IssueDate"];
                DateTime dueDt = (DateTime)ViewState["DueDate"];
                string bookIdsCsv = ViewState["bookIdsCsv"].ToString(); ;



                using (DataSet ds = objCommonBO.BookIssue( "INSERT",0,MemberID,issueType, issueDt,  dueDt, bookIdsCsv, intAdminUserID, true))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int msgCode = Convert.ToInt32(ds.Tables[0].Rows[0]["MsgCode"]);

                        if (msgCode == 1)
                        {
                            ShowAlert("Book Issue inserted successfully.", "success");
                            ClearFormFields();
                        }
                        else
                        {
                            ShowAlert("Failed to insert Book Issue.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }

            divGrid.Visible=false;
            btnSave.Visible = true;
            btnClear.Visible = true;
            btnCancel.Visible = true;
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            gvSelectedBooks.DataSource = null;
            gvSelectedBooks.DataBind();
            pnlConfirm.Visible = false;
            divGrid.Visible=false;
            ClearFormFields();
            btnSave.Visible = true;
            btnClear.Visible = true;
            btnCancel.Visible = true;
        }


        private void ClearFormFields()
        {
            //rblIssueType.ClearSelection();
            if (rblIssueType.SelectedValue=="Student")
            {
                txtStudentID.Text = "";
                divStudent.Visible =true;
            }
            else
            {
                txtStaffID.Text = "";
                divStaff.Visible = true;
            }
            lstIsbn.ClearSelection();

            issueDate.Value = "";
            dueDate.Value = "";
            divIssueDetails.Visible = true;

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFormFields();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            ClearFormFields();
            rblIssueType.ClearSelection();
            rblIssueType.Visible=true;
            divStudent.Visible =false;
            divStaff.Visible = false;
            divIssueDetails.Visible = false;
            btnSave.Visible = false;
            btnClear.Visible = false;
            btnCancel.Visible = false;

        }
    }
}





































