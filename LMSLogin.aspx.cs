using BLL;
using Library;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace LMS
{
    public partial class dummy : System.Web.UI.Page
    {
        LoginBO objLoginBo = new LoginBO();
        private string[] lblErrorMsg = new string[10];

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    // Disable caching
                    Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                    Response.Cache.SetValidUntilExpires(false);
                    Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetNoStore();
                    Response.AppendHeader("Pragma", "no-cache");

                    // Session reset
                    Session.Clear();
                    Session.RemoveAll();
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                }
                lblErrorMsg = new string[10];
                // Load error messages
                lblErrorMsg[0] = CommonFunction.GetErrorMessage("", "ERRLOG001"); // UserID empty
                lblErrorMsg[1] = CommonFunction.GetErrorMessage("", "ERRLOG002"); // Invalid format
                lblErrorMsg[2] = CommonFunction.GetErrorMessage("", "ERRLOG003"); // Password empty
                lblErrorMsg[3] = CommonFunction.GetErrorMessage("", "ERRLOG004"); // Invalid login
                lblErrorMsg[4] = CommonFunction.GetErrorMessage("", "ERRLOG005"); // Extra msg
                lblErrorMsg[5] = CommonFunction.GetErrorMessage("", "ERRLOG006"); // Success msg
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }


        protected void btnStudentLogin_Click(object sender, EventArgs e)
        {
            SignInUser(
                userType: 1,
                strUserId: txtStudentID.Text.Trim(),
                strPassword:txtStudentPwd.Text.Trim(),
                loginIdEmptyMsg: lblErrorMsg[0],
                invalidFormatMsg: lblErrorMsg[1],
                passwordEmptyMsg: lblErrorMsg[2],
                invalidLoginMsg: lblErrorMsg[3],
                validateLoginIdFormat: CommonFunction.IsAlphaNumeric
            );
        }
        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            SignInUser(
                userType:2,
                strUserId: txtAdminID.Text.Trim(),
                strPassword: txtAdminPwd.Text.Trim(),
                loginIdEmptyMsg: lblErrorMsg[0],
                invalidFormatMsg: lblErrorMsg[1],
                passwordEmptyMsg: lblErrorMsg[2],
                invalidLoginMsg: lblErrorMsg[3],
                validateLoginIdFormat: CommonFunction.IsAlphaNumeric
            );
        }

        private void SignInUser(int userType,
            string strUserId, string strPassword,
            string loginIdEmptyMsg, string invalidFormatMsg,
            string passwordEmptyMsg, string invalidLoginMsg,
            Func<string, bool> validateLoginIdFormat)
        {
            try
            {
                // 1️⃣ UserID required
                if (string.IsNullOrWhiteSpace(strUserId))
                {
                    ShowAlert(loginIdEmptyMsg, "error");
                    return;
                }

                // 2️⃣ UserID must be alphanumeric
                if (!validateLoginIdFormat(strUserId))
                {
                    ShowAlert(invalidFormatMsg, "error");
                    return;
                }

                // 3️⃣ Password required
                if (string.IsNullOrWhiteSpace(strPassword))
                {
                    ShowAlert(passwordEmptyMsg, "error");
                    return;
                }

                // 4️⃣ Validate login using stored procedure
                using (DataSet ds = objLoginBo.CheckLogin(userType,strUserId, Security.CryptTripleDES(true, strPassword)))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int count = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]);
                        if (count > 0)
                        {
                            Session["AdminUserName"] = strUserId;
                            if (userType == 1)
                            {
                                
                                Session["AdminUserID"] = ds.Tables[0].Rows[0]["StudentUserID"];
                                Session["MemberID"] = ds.Tables[0].Rows[0]["StudentID"].ToString();

                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                    1, strUserId,
                                    DateTime.Now,
                                    DateTime.Now.AddMinutes(Session.Timeout),
                                    false, "Student",
                                    FormsAuthentication.FormsCookiePath
                                );

                                string encTicket = FormsAuthentication.Encrypt(ticket);
                                Response.Cookies.Add(
                                    new HttpCookie(FormsAuthentication.FormsCookieName, encTicket)
                                );

                                Response.Redirect("~/admin/MemberDashboard.aspx", true);
                                return;
                            }

                            // 🔹 ADMIN LOGIN (CHECK RoleType)
                            if (userType == 2)
                            {
                                int roleType = Convert.ToInt32(ds.Tables[0].Rows[0]["RoleType"]);

                                Session["AdminUserID"] = ds.Tables[0].Rows[0]["UserID"];
                                Session["MemberID"] = ds.Tables[0].Rows[0]["EmployeeCode"].ToString();
                                Session["AdminUserRoleID"] = roleType;

                                bool isAdminRole = (roleType == 1 || roleType == 12);

                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                    1, strUserId,
                                    DateTime.Now,
                                    DateTime.Now.AddMinutes(Session.Timeout),
                                    false, isAdminRole ? "Admin" : "Member",
                                    FormsAuthentication.FormsCookiePath
                                );

                                string encTicket = FormsAuthentication.Encrypt(ticket);
                                Response.Cookies.Add(
                                    new HttpCookie(FormsAuthentication.FormsCookieName, encTicket)
                                );

                                // 🔀 Redirect based on RoleType
                                if (isAdminRole)
                                    Response.Redirect("~/admin/RequestDetails.aspx", true);
                                else
                                    Response.Redirect("~/admin/MemberDashboard.aspx", true);
                            }
                        }
                        else
                        {
                            ClearFields();
                            ShowAlert(invalidLoginMsg, "error");
                            return;
                        }
                    }
                    else
                    {

                        ClearFields();
                        ShowAlert(invalidLoginMsg, "error");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MyExceptionLogger.Publish(ex);
            }
        }

        private void ClearFields()
        {
            txtStudentID.Text = "";
            txtStudentPwd.Text = "";
            txtAdminID.Text="";
            txtAdminPwd.Text="";
        }

        private void ShowAlert(string message, string alertType = "error")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(),
                $"$(function(){{ AlertMessage('{message.Replace("'", "\\'")}', '{alertType.ToLower()}'); }});", true);
        }
    }
}
