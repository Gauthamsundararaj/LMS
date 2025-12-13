//using Library;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//namespace LMS
//{
//    public partial class dummy : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
           
//        }

//        protected void btnLogin_Click(object sender, EventArgs e)
//        {
//            // Fetch messages from XML
//            string msg = "";

//            // Login ID
//            if (string.IsNullOrWhiteSpace(txtLoginId.Text))
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR001");
//                ShowToastr("error", msg);
//                return;
//            }
//            if (!Regex.IsMatch(txtLoginId.Text, @"^[A-Za-z0-9]+$"))
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR002");
//                ShowToastr("error", msg);
//                return;
//            }

//            // Employee Code
//            if (string.IsNullOrWhiteSpace(txtEmpCode.Text))
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR004");
//                ShowToastr("error", msg);
//                return;
//            }

//            // User Name
//            if (string.IsNullOrWhiteSpace(txtUsername.Text))
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR006");
//                ShowToastr("error", msg);
//                return;
//            }

//            // Gender
//            if (!rbMale.Checked && !rbFemale.Checked && !rbOther.Checked)
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR008");
//                ShowToastr("error", msg);
//                return;
//            }

//            // Email
//            if (string.IsNullOrWhiteSpace(txtEmail.Text))
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR009");
//                ShowToastr("error", msg);
//                return;
//            }
//            if (!Regex.IsMatch(txtEmail.Text, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR010");
//                ShowToastr("error", msg);
//                return;
//            }

//            // Phone Number
//            if (string.IsNullOrWhiteSpace(TxtPhone.Text))
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR012");
//                ShowToastr("error", msg);
//                return;
//            }
//            if (!Regex.IsMatch(TxtPhone.Text, @"^\d{10}$"))
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR013");
//                ShowToastr("error", msg);
//                return;
//            }

//            // Department
//            if (ddlDepartment.SelectedValue == "")
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR017");
//                ShowToastr("error", msg);
//                return;
//            }

//            // Designation
//            if (ddlDesignation.SelectedValue == "")
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR018");
//                ShowToastr("error", msg);
//                return;
//            }

//            // Role Type
//            if (ddlRoleType.SelectedValue == "")
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR019");
//                ShowToastr("error", msg);
//                return;
//            }

//            // Password
//            if (string.IsNullOrWhiteSpace(txtPassword.Text))
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR020");
//                ShowToastr("error", msg);
//                return;
//            }
//            if (txtPassword.Text.Length < 6)
//            {
//                msg = CommonFunction.GetErrorMessage("", "ERRUSR021");
//                ShowToastr("error", msg);
//                return;
//            }

//            // ✅ If all validations pass
//            msg = CommonFunction.GetErrorMessage("", "SUCUSR001");
//            ShowToastr("success", msg);

//            // TODO: Save to database logic here
//        }
//            catch (Exception ex)
//            {
//                ShowToastr("error", "An unexpected error occurred: " + ex.Message);
//    }
//}
//private void ShowToastr(string type, string message)
//{
//    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_message",
//        $"toastr.{type}('{message}');", true);
//}
//        }
//    }
//}