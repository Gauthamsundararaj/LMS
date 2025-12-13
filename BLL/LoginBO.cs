using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Library;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
namespace BLL
{
    public class LoginBO
    {
        private LoginDAO _objLoginDAO;

        public LoginBO()
        {
            _objLoginDAO = new LoginDAO();
        }

        public void ReleaseResources()
        {
            _objLoginDAO.ReleaseResources();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public DataSet CheckAdminLogin(string username, string Password)
        {
            return _objLoginDAO.CheckAdminLogin(username, Password);
        }

      

    }
}
