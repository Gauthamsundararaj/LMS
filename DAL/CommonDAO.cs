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
    public class CommonDAO
    {
        private SqlConnection objSqlConnection;

        public CommonDAO()
        {
            objSqlConnection = new SqlConnection(Security.CryptTripleDES(false, ConfigurationManager.AppSettings["ConnectionString"].ToString()));
        }
        public void ReleaseResources()
        {
            if (objSqlConnection.State != ConnectionState.Closed)
                objSqlConnection.Close();
            objSqlConnection.Dispose();
        }

        public DataSet BookIssue(string Action, int IssueID, string MemberID, string IssueType,
             DateTime? IssueDate, DateTime? DueDate, string BookIDList, int AdminUserID, bool Active)
        {
            SqlParameter[] objParam = new SqlParameter[]
            {
        new SqlParameter("@Action", Action),
        new SqlParameter("@IssueID", IssueID),
        new SqlParameter("@MemberID", MemberID),
        new SqlParameter("@IssueType", (object)IssueType ?? DBNull.Value),
        new SqlParameter("@IssueDate", (object)IssueDate ?? DBNull.Value),
        new SqlParameter("@DueDate", (object)DueDate ?? DBNull.Value),
        
        // list of multiple book ids
        new SqlParameter("@BookIDList", (object)BookIDList ?? DBNull.Value),
        
        new SqlParameter("@AdminUserID", AdminUserID),
        new SqlParameter("@Active", Active)
            };

            return SqlHelper.ExecuteDataset(
                        objSqlConnection,
                        CommandType.StoredProcedure,
                        "Issuebook",
                        objParam
                   );
        }

        public DataSet ValidateMember(string Action, string MemberID, string IssueType)
        {
            SqlParameter[] objParam = new SqlParameter[]
            {
            new SqlParameter("@Action", Action),
            new SqlParameter("@MemberID", MemberID),
            new SqlParameter("@IssueType", (object)IssueType ?? DBNull.Value),
           
            };

            return SqlHelper.ExecuteDataset(
                objSqlConnection,
                CommandType.StoredProcedure,
                "ValidateMember",
                objParam
            );
        }
    }
}
