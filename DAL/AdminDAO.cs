using Library;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AdminDAO
    {
        private SqlConnection objSqlConnection;

        public AdminDAO()
        {
            objSqlConnection = new SqlConnection(Security.CryptTripleDES(false, ConfigurationManager.AppSettings["ConnectionString"].ToString()));
        }
        public void ReleaseResources()
        {
            if (objSqlConnection.State != ConnectionState.Closed)
                objSqlConnection.Close();
            objSqlConnection.Dispose();
        }

        public DataSet UserMaster(string Action, int UserID,string LoginId, string EmpCode,string UserName,
      string Gender,string Email, string AltEmail, string Phone, string AltPhone, string Department,
      string Designation,int RoleType,string Password, bool Active, int AdminUserID, string searchBy,
      string searchValue )
        {
            SqlParameter[] objParam = new SqlParameter[]
            {
        new SqlParameter("@Action", Action),
        new SqlParameter("@UserID", UserID),
        new SqlParameter("@LoginId", (object)LoginId ?? DBNull.Value),
        new SqlParameter("@EmpCode", (object)EmpCode ?? DBNull.Value),
        new SqlParameter("@UserName", (object)UserName ?? DBNull.Value),
        new SqlParameter("@Gender", (object)Gender ?? DBNull.Value),
        new SqlParameter("@Email", (object)Email ?? DBNull.Value),
        new SqlParameter("@AltEmail", (object)AltEmail ?? DBNull.Value),
        new SqlParameter("@Phone", (object)Phone ?? DBNull.Value),
        new SqlParameter("@AltPhone", (object)AltPhone ?? DBNull.Value),
        new SqlParameter("@Department", (object)Department ?? DBNull.Value),
        new SqlParameter("@Designation", (object)Designation ?? DBNull.Value),
        new SqlParameter("@RoleType", RoleType),
        new SqlParameter("@Password", (object)Password ?? DBNull.Value),
        new SqlParameter("@Active", Active),
        new SqlParameter("@AdminUserID", AdminUserID),
        new SqlParameter("@SearchBy", (object)searchBy ?? DBNull.Value),
        new SqlParameter("@SearchValue", (object)searchValue ?? DBNull.Value)

            };

            return SqlHelper.ExecuteDataset(
                objSqlConnection,
                CommandType.StoredProcedure,
                "dbo.UserMaster",
                objParam
            );
        }


        public DataSet RoleType(string action, int roleId = 0, string strRoleName = "",
                          bool active = true)
        {
            SqlParameter[] objparam = new SqlParameter[]
            {
                new SqlParameter("@Action", action),
                new SqlParameter("@RoleID", roleId),
                new SqlParameter("@RoleName", (object)strRoleName ?? DBNull.Value),
                new SqlParameter("@Active", active),

            };

            return SqlHelper.ExecuteDataset(objSqlConnection,
                    CommandType.StoredProcedure, "dbo.getRoleMaster", objparam);
        }


    }
}