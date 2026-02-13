using BLL;
using Library;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Admin
{
    public partial class BookIssue : System.Web.UI.Page
    {
        MasterBO objMasterBO = new MasterBO();
        CommonBO objCommonBO = new CommonBO();
        int intAdminUserID;
        private string[] lblErrorMsg = new string[30];

        protected void Page_Load(object sender, EventArgs e)
        {
            intAdminUserID = Convert.ToInt32(Session["AdminUserID"]);
            if (!IsPostBack)
            {
                BindBookList();
            }
            lblErrorMsg[1]  = CommonFunction.GetErrorMessage("", "ERRBI01"); // Student ID is required
            lblErrorMsg[2]  = CommonFunction.GetErrorMessage("", "ERRBI02"); // Invalid Student ID
            lblErrorMsg[3]  = CommonFunction.GetErrorMessage("", "ERRBI03"); // Student ID not found
            lblErrorMsg[4]  = CommonFunction.GetErrorMessage("", "ERRBI04"); // Staff ID is required
            lblErrorMsg[5]  = CommonFunction.GetErrorMessage("", "ERRBI05"); // Invalid Staff ID
            lblErrorMsg[6]  = CommonFunction.GetErrorMessage("", "ERRBI06"); // Staff ID not found
            lblErrorMsg[7]  = CommonFunction.GetErrorMessage("", "ERRBI07"); // Select at least one ISBN
            lblErrorMsg[8]  = CommonFunction.GetErrorMessage("", "ERRBI08"); // Invalid Issue Date
            lblErrorMsg[9]  = CommonFunction.GetErrorMessage("", "ERRBI09"); // Invalid Due Date
            lblErrorMsg[10] = CommonFunction.GetErrorMessage("", "ERRBI10"); // Due Date must be greater than Issue Date
            lblErrorMsg[11] = CommonFunction.GetErrorMessage("", "ERRBI11"); // Session expired
            lblErrorMsg[12] = CommonFunction.GetErrorMessage("", "ERRBI12"); // Failed to insert Book Issue

            // Success message
            lblErrorMsg[13] = CommonFunction.GetErrorMessage("", "SUSBI01"); // Book Issue inserted successfully

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
                    //pnlConfirm.Visible = true;
                }
                else
                {
                    ShowAlert(lblErrorMsg[0], "error");
                    //pnlConfirm.Visible = false;
                }
            }

            //pnlConfirm.Visible = true;
        }

        protected void btnSave_Click1(object sender, EventArgs e)
        {
            try
            {
                string issueType = rblIssueType.SelectedValue;
                string studentId = txtStudentID.Text.Trim();
                string staffId = txtStaffID.Text.Trim();
                string issueDateText = issueDate.Text.Trim();
                string dueDateText = dueDate.Text.Trim();
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
               
                if (issueType == "Student")
                {
                    if (string.IsNullOrWhiteSpace(studentId))
                    {
                        ShowAlert(lblErrorMsg[1], "error");
                        return;
                    }
                    if (!Regex.IsMatch(studentId, @"^[A-Za-z0-9\-]+$"))
                    {
                        ShowAlert(lblErrorMsg[2], "error");
                        txtStudentID.Focus();
                        return;
                    }

                    MemberID = ValidateMemberFromDB(action, studentId, "Student");

                    if (MemberID == null)
                    {
                        ShowAlert(lblErrorMsg[3], "error");
                        return;
                    }
                }
                else if (issueType == "Staff")
                {
                    if (string.IsNullOrWhiteSpace(staffId))
                    {
                        ShowAlert(lblErrorMsg[4],"error");
                        return;
                    }
                    if (!Regex.IsMatch(staffId, @"^[A-Za-z0-9\-]+$"))
                    {
                        ShowAlert(lblErrorMsg[5], "error");
                        txtStaffID.Focus();
                        return;
                    }

                    MemberID = ValidateMemberFromDB(action, staffId, "Staff");
                    if (MemberID == null)
                    {
                        ShowAlert(lblErrorMsg[6], "error");
                        return;
                    }
                }

                // 3️⃣ ISBN Validate
                if (string.IsNullOrWhiteSpace(bookIdsCsv))
                {
                    ShowAlert(lblErrorMsg[7], "warning");
                    return;
                }
                DateTime issueDt;
                if (string.IsNullOrWhiteSpace(issueDate.Text) ||
                    !DateTime.TryParseExact(
                        issueDate.Text.Trim(),
                        "dd-MMM-yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out issueDt))
                {
                    ShowAlert(lblErrorMsg[8], "error"); // Invalid Issue Date
                    return;
                }

                DateTime dueDt;
                if (string.IsNullOrWhiteSpace(dueDate.Text) ||
                    !DateTime.TryParseExact(
                        dueDate.Text.Trim(),
                        "dd-MMM-yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out dueDt))
                {
                    ShowAlert(lblErrorMsg[9], "error"); // Invalid Due Date
                    return;
                }

                //DateTime issueDt;
                //if (string.IsNullOrWhiteSpace(issueDate.Value)
                //    || !DateTime.TryParse(issueDate.Value, out issueDt))
                //{
                //    ShowAlert(lblErrorMsg[8], "error");
                //    return;
                //}


                //DateTime dueDt;
                //if (string.IsNullOrWhiteSpace(dueDate.Value)
                //    || !DateTime.TryParse(dueDate.Value, out dueDt))
                //{
                //    ShowAlert(lblErrorMsg[9], "error");
                //    return;
                //}

                if (dueDt <= issueDt)
                {
                    ShowAlert(lblErrorMsg[10], "error");
                    return;
                }

                ViewState["MemberID"] = MemberID;
                ViewState["IssueType"] = issueType;
                ViewState["IssueDate"] = issueDt;
                ViewState["DueDate"] = dueDt;
                ViewState["bookIdsCsv"] = bookIdsCsv;

                BindSelectedBooksGrid();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ShowModal",
                    "$(document).ready(function()" +
                    "{ $('#selectedBooksModal').modal('show')  });", true);
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
                    ShowAlert(lblErrorMsg[11], "warning");
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
                            ShowAlert(lblErrorMsg[13], "success");
                            ClearFormFields();
                        }
                        else
                        {
                            ShowAlert(lblErrorMsg[12], "error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }

           
            btnSave.Visible = true;
            btnClear.Visible = true;
            btnCancel.Visible = true;
        }

        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            gvSelectedBooks.DataSource = null;
            gvSelectedBooks.DataBind();
            //pnlConfirm.Visible = false;
            //divGrid.Visible=false;
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

            issueDate.Text = "";
            dueDate.Text= "";
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

        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                if (objMasterBO != null & objCommonBO!=null)
                {
                    objMasterBO.ReleaseResources();
                    objMasterBO = null;
                    objCommonBO.ReleaseResources();
                    objCommonBO = null;
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }
    }
}
