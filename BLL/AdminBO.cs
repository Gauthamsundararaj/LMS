using DAL;
using Library;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using static System.Net.Mime.MediaTypeNames;
namespace BLL
{
    public class AdminBO
    {
        private AdminDAO objAdminDAO;

        public AdminBO()
        {
            objAdminDAO = new AdminDAO();
        }
        public void ReleaseResources()
        {
            objAdminDAO.ReleaseResources();
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }



        // 

        public DataSet UserMaster(string Action, int UserID = 0, string LoginId = "", string EmpCode = "",
          string UserName = "", string Gender = "", string Email = "", string AltEmail = "",
          string Phone = "", string AltPhone = "", string Department = "", string Designation = "",
          int RoleType = 0, string Password = "", bool Active = true, int AdminUserID = 0, string searchBy = "",
          string searchValue = "")

        {
            return objAdminDAO.UserMaster(Action, UserID, LoginId, EmpCode, UserName, Gender,
                Email, AltEmail, Phone, AltPhone, Department, Designation, RoleType, Password,
                Active, AdminUserID, searchBy, searchValue
            );
        }


        public DataSet RoleType(string action, int roleId = 0, string strRoleName = "",
                          bool active = true)
        {
            return objAdminDAO.RoleType(action, roleId, strRoleName, active);
        }
    }


}

