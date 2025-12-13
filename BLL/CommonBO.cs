using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CommonBO
    {
        private CommonDAO objCommonDAO;

        public CommonBO()
        {
            objCommonDAO = new CommonDAO();
        }
        public void ReleaseResources()
        {
            objCommonDAO.ReleaseResources();
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
        public DataSet BookIssue(string Action, int IssueID = 0, string MemberID = "", string IssueType = "",
    DateTime? IssueDate = null, DateTime? DueDate = null, string BookIDList = "",      // comma-separated book IDs
    int AdminUserID = 0, bool Active = true)
        {
            return objCommonDAO.BookIssue(
                Action,
                IssueID,
                MemberID,
                IssueType,
                IssueDate,
                DueDate,
                BookIDList,

                AdminUserID,
                Active
            );
        }

        public DataSet ValidateMember(string Action, string MemberID ="", string IssueType ="")

        {

            return objCommonDAO.ValidateMember(
                Action,
                MemberID,
                IssueType
               );
        }
    }
}
