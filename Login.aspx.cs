using BLL;
using Library;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LMS
{
    public partial class Login : System.Web.UI.Page
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

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SignInUser(
                strUserId: txtUserId.Text.Trim(),
                strPassword: txtPassword.Text.Trim(),
                loginIdEmptyMsg: lblErrorMsg[0],
                invalidFormatMsg: lblErrorMsg[1],
                passwordEmptyMsg: lblErrorMsg[2],
                invalidLoginMsg: lblErrorMsg[3],
                validateLoginIdFormat: CommonFunction.IsAlphaNumeric
            );
        }

        private void SignInUser(
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
                using (DataSet ds = objLoginBo.CheckAdminLogin(strUserId, Security.CryptTripleDES(true, strPassword)))
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int count = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]);
                        if (count > 0)
                        {

                            Session["AdminUserName"] = strUserId;
                            Session["AdminUserID"] = ds?.Tables[0].Rows[0]["userid"];
                            Session["AdminUserRoleID"] = ds?.Tables[0].Rows[0]["RoleType"];
                            // Forms Authentication
                            FormsAuthentication.Initialize();
                            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                1, strUserId,
                                DateTime.Now,
                                DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout),
                                false,
                                FormsAuthentication.FormsCookiePath

                            );

                            string encTicket = FormsAuthentication.Encrypt(ticket);
                            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                            Response.Cookies.Add(authCookie);

                            // Redirect to dashboard
                            Response.Redirect("~/admin/AuthorMaster.aspx", true);
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
            txtUserId.Text = "";
            txtPassword.Text = "";
        }

        private void ShowAlert(string message, string alertType = "error")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(),
                $"$(function(){{ AlertMessage('{message.Replace("'", "\\'")}', '{alertType.ToLower()}'); }});", true);
        }
    }
}
