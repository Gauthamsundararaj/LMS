using BLL;
using Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Admin
{
    public partial class AddMenu : System.Web.UI.Page
    {
        //AdminBO objAdminBO = new AdminBO();
        //protected string[] lblErrorMsg = new string[20];
        protected void Page_Load(object sender, EventArgs e)
        {
            //ErrorMessage();
            //if (!IsPostBack)
            //{
            //    CommonFunction.CheckForcefulSession();
            //    BindParentMenu();
            //    BindMenuMaster();
            //}
        }
        //private void ErrorMessage()
        //{
        //    try
        //    {
        //        lblErrorMsg[0] =CommonFunction.GetErrorMessage("", "ERRMen001");
        //        lblErrorMsg[1] =CommonFunction.GetErrorMessage("", "ERRMen002");
        //        lblErrorMsg[2] =CommonFunction.GetErrorMessage("", "ERRMen003");
        //        lblErrorMsg[3] =CommonFunction.GetErrorMessage("", "ERRMen004");
        //        lblErrorMsg[4] =CommonFunction.GetErrorMessage("", "ERRMen005");
        //        lblErrorMsg[5] =CommonFunction.GetErrorMessage("", "ERRMen006");
        //        lblErrorMsg[6] =CommonFunction.GetErrorMessage("", "ERRMen007");
        //        lblErrorMsg[7] =CommonFunction.GetErrorMessage("", "ERRMen008");
        //        lblErrorMsg[8] =CommonFunction.GetErrorMessage("", "ERRMen009");
        //        lblErrorMsg[9] =CommonFunction.GetErrorMessage("", "ERRMen010");
        //        lblErrorMsg[10] =CommonFunction.GetErrorMessage("", "ERRSTUD001");
        //        lblErrorMsg[11] =CommonFunction.GetErrorMessage("", "ERR0011");
        //        lblErrorMsg[12] =CommonFunction.GetErrorMessage("", "ERRMen011");
        //        lblErrorMsg[13] =CommonFunction.GetErrorMessage("", "ERRMen012");
        //        lblErrorMsg[14] =CommonFunction.GetErrorMessage("", "ERRMen013");
        //    }
        //    catch (Exception ex)
        //    {
        //        MyExceptionLogger.Publish(ex);
        //    }
        //}
        //private void BindParentMenu()
        //{
        //    try
        //    {
        //        ddlParentMenu.Items.Clear();
        //        using (DataSet objDataSet = objAdminBO.LoadParentMenu())
        //        {
        //            if (objDataSet != null && objDataSet.Tables.Count > 0 && objDataSet.Tables[0].Rows.Count > 0)
        //            {
        //                ddlParentMenu.DataSource = objDataSet.Tables[0];
        //                ddlParentMenu.DataTextField = "MenuName";
        //                ddlParentMenu.DataValueField = "MenuID";
        //                ddlParentMenu.DataBind();
        //            }
        //            ddlParentMenu.Items.Insert(0, new ListItem("--Select--", "0"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyExceptionLogger.Publish(ex);
        //    }
        //}
        //protected override void OnInit(EventArgs e)
        //{
        //    InitializeComponent();
        //    base.OnInit(e);
        //}
        //private void InitializeComponent()
        //{
        //    this.Page.Load += new EventHandler(this.Page_Load);
        //    this.Page.Unload += new EventHandler(this.Page_UnLoad);
        //    this.btnSave.Click += new EventHandler(btnSave_Click);
        //    this.btnclear.Click += new EventHandler(btnclear_Click);
        //    this.dgMenu.ItemCommand += new DataGridCommandEventHandler(dgMenu_ItemCommand);
        //    //this.rptMenu.ItemDataBound += new RepeaterItemEventHandler(rptMenu_ItemDataBound);
        //}
        //private void btnclear_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Clear();
        //    }
        //    catch (Exception ex)
        //    {
        //        MyExceptionLogger.Publish(ex);
        //    }
          
        //}
        //protected void dgMenu_ItemCommand(object source, DataGridCommandEventArgs e)
        //{
        //    if (e.CommandName == "Edit")
        //    {
        //        try
        //        {
        //            Clear();
        //            long editmenuid = Convert.ToInt64((e.Item.FindControl("lblMenuID") as Label).Text.Trim());
        //            ViewState["EditMenuID"] = editmenuid;
        //            PopulateEditGrid(editmenuid, "EDIT");
        //            ViewState["UdpateFlag"] = "1";
        //        }
        //        catch (Exception ex)
        //        {
        //            MyExceptionLogger.Publish(ex);
        //        }
        //    }
        //    if (e.CommandName == "Delete")
        //    {
        //        try
        //        {
        //            long editmenuid = Convert.ToInt64((e.Item.FindControl("lblMenuID") as Label).Text.Trim());
        //            ViewState["EditMenuID"] = editmenuid;
        //            PopulateEditGrid(editmenuid, "DELETE");
        //        }
        //        catch (Exception ex)
        //        {
        //            MyExceptionLogger.Publish(ex);
        //        }
        //    }
        //}
        //private void PopulateEditGrid(long editmenuid, string strType)
        //{
        //    try
        //    {
        //        using (DataSet ds = objAdminBO.EditMenuDetails(editmenuid, strType))
        //        {
        //            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                if (strType == "EDIT")
        //                {

        //                    txtMenuName.Text = Convert.ToString(ds.Tables[0].Rows[0]["MenuName"]);
        //                    string tstatus = Convert.ToString(ds.Tables[0].Rows[0]["Active"]);
        //                    if (tstatus == "True")
        //                    {
        //                        rbtnStatus.SelectedValue = "1";
        //                    }
        //                    else if (tstatus == "False")
        //                    {
        //                        rbtnStatus.SelectedValue = "0";
        //                    }
        //                    string IsChildMenu = Convert.ToString(ds.Tables[0].Rows[0]["IsChildMenu"]);
        //                    if (IsChildMenu == "True")
        //                    {
        //                        chkLeaf.Checked = true;
        //                        txtPageName.Text = Convert.ToString(ds.Tables[0].Rows[0]["PageName"]);
        //                        if (ddlParentMenu.Items.FindByValue(ds.Tables[0].Rows[0]["ParentMenuID"].ToString()) != null)
        //                            ddlParentMenu.Items.FindByValue(ds.Tables[0].Rows[0]["ParentMenuID"].ToString()).Selected = true;
        //                    }
        //                    else if (IsChildMenu == "False")
        //                    {
        //                        chkLeaf.Checked = false;
        //                    }
        //                    btnSave.Text = "Update";
        //                }
        //                else if (strType == "DELETE")
        //                {
        //                    string Errmsg = Convert.ToString(ds.Tables[0].Rows[0][0]);
        //                    Clear();
        //                    if (Errmsg == "1")
        //                    {
        //                        ShowAlert(lblErrorMsg[13], "success");
        //                        BindMenuMaster();
        //                        return;
        //                    }
        //                    else if (Errmsg == "0")
        //                    {
        //                        ShowAlert(lblErrorMsg[14], "error");
                               
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyExceptionLogger.Publish(ex);
        //    }
        //}
        //public void BindMenuMaster()
        //{
        //    try
        //    {
        //        using (DataSet dsData = objAdminBO.GetMenuDetails("SELECT"))
        //        {
        //            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
        //            {
        //                dgMenu.DataSource = dsData.Tables[0];
        //                dgMenu.DataBind();
        //                SerialNo(dgMenu);
        //                divMenudtl.Visible = true;
        //            }
        //            else
        //            {
        //                divMenudtl.Visible = false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyExceptionLogger.Publish(ex);
        //    }
        //}
        //private static void SerialNo(DataGrid Dgrid)
        //{
        //    int intSno = Dgrid.CurrentPageIndex;
        //    int intPgInd = intSno;
        //    if (intSno == 0)
        //    {
        //        foreach (DataGridItem dgi in Dgrid.Items)
        //        {
        //            intPgInd++;
        //            dgi.Cells[0].Text = intPgInd.ToString();
        //        }
        //    }
        //    else
        //    {
        //        intPgInd = Dgrid.PageSize * (intPgInd);
        //        foreach (DataGridItem dgi in Dgrid.Items)
        //        {
        //            intPgInd++;
        //            dgi.Cells[0].Text = intPgInd.ToString();
        //        }
        //    }
        //}
        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    if (!InputValidation()) { return; }
        //    try
        //    {
        //        int intIsLeaf = chkLeaf.Checked == true ? 1 : 0;
        //        int intParentmenuid;
        //        string strMenuname, strPagename = "", strActivestatus, Errnumber = "", strType = "";
        //        long intEditMenuID = 0; int intFlag = Convert.ToInt32(ViewState["UdpateFlag"]);

        //        strMenuname = Convert.ToString(txtMenuName.Text).Trim();
        //        if (intIsLeaf == 1)
        //        {
        //            strPagename = Convert.ToString(txtPageName.Text).Trim() == "" ? "" : Convert.ToString(txtPageName.Text).Trim();
        //        }
        //        intParentmenuid = Convert.ToInt32(ddlParentMenu.SelectedValue);
        //        strActivestatus = Convert.ToString(rbtnStatus.SelectedValue);
        //        if (intFlag == 0) { strType = "INSERT"; intEditMenuID = 0; }
        //        else if (intFlag == 1) { strType = "UPDATE"; intEditMenuID = Convert.ToInt32(ViewState["EditMenuID"]); }
        //        using (DataSet dsData = objAdminBO.SaveMenuDetails(intEditMenuID, intIsLeaf, strMenuname, strActivestatus, strPagename, intParentmenuid, Convert.ToInt32(Session["EmpID"]), strType))
        //        {
        //            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
        //            {
        //                Errnumber = Convert.ToString(dsData.Tables[0].Rows[0][0]);
        //                if (Errnumber == "1")
        //                {
        //                    Clear();
        //                    if (intFlag == 0)
        //                    {
        //                        ShowAlert(lblErrorMsg[5], "success");
        //                        BindParentMenu();
        //                    }
        //                    else if (intFlag == 1)
        //                        ShowAlert(lblErrorMsg[7], "Success");
        //                    BindMenuMaster();
        //                    return;
        //                }
        //                else if (Errnumber == "0")
        //                {
        //                    Clear();
        //                    if (intFlag == 0)
        //                        ShowAlert(lblErrorMsg[6], "error");
        //                    else if (intFlag == 1)
        //                        ShowAlert(lblErrorMsg[8], "error");

        //                    return;
        //                }
        //                else if (Errnumber == "2")
        //                {
        //                    ShowAlert(lblErrorMsg[9], "error");
        //                    return;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyExceptionLogger.Publish(ex);
        //    }
        //}
        //private bool InputValidation()
        //{
        //    if (txtMenuName.Text.Trim() == "")
        //    {
        //        ShowAlert(lblErrorMsg[0], "error");
        //        return false;
        //    }
        //    if (rbtnStatus.SelectedValue != "0" && rbtnStatus.SelectedValue != "1")
        //    {
        //        ShowAlert(lblErrorMsg[1], "error");
        //        return false;
        //    }
        //    if (chkLeaf.Checked)
        //    {
        //        if (Convert.ToString(txtPageName.Text).Trim() == "")
        //        {
        //            ShowAlert(lblErrorMsg[2], "error");
        //            return false;
        //        }
        //        if (Convert.ToString(ddlParentMenu.SelectedValue) == "0" || Convert.ToString(ddlParentMenu.SelectedValue) == "--Select--" || Convert.ToString(ddlParentMenu.SelectedValue) == null)
        //        {
        //            ShowAlert(lblErrorMsg[3], "error");
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        //private void Clear()
        //{
        //    ddlParentMenu.ClearSelection();
        //    txtMenuName.Text = "";
        //    txtPageName.Text = "";
        //    rbtnStatus.SelectedValue = "1";
        //    chkLeaf.Checked = false;
        //    ViewState["UdpateFlag"] = "0";
        //    ViewState["EditMenuID"] = string.Empty;
        //    btnSave.Text = "Save";
        //}
        //private void ShowAlert(string message, string alertType = "error")
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, GetType(), Guid.NewGuid().ToString(), "$(function(){AlertMessage('" + message + "','" + alertType.ToLower() + "')});", true);
        //}

        //protected void Page_UnLoad(object sender, EventArgs e)
        //{
        //    objAdminBO.ReleaseResources();
        //}

    }
}