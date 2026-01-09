using Library;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MasterDAO
    {
        private SqlConnection objSqlConnection;

        public MasterDAO()
        {
            objSqlConnection = new SqlConnection(Security.CryptTripleDES(false, ConfigurationManager.AppSettings["ConnectionString"].ToString()));
        }
        public void ReleaseResources()
        {
            if (objSqlConnection.State != ConnectionState.Closed)
                objSqlConnection.Close();
            objSqlConnection.Dispose();
        }
       

        public DataSet AuthorMaster(string action, int authorID = 0, string authorName = "",
                            string authorType = "", bool active = true, int adminUserID = 0)
        {
            SqlParameter[] objparam = new SqlParameter[]
            {
                new SqlParameter("@Action", action),
                new SqlParameter("@AuthorID", authorID),
                new SqlParameter("@AuthorName", (object)authorName ?? DBNull.Value),
                new SqlParameter("@AuthorType", (object)authorType ?? DBNull.Value),
                new SqlParameter("@Active", active),
                new SqlParameter("@AdminUserID", adminUserID),

            };

            return SqlHelper.ExecuteDataset(objSqlConnection,
                    CommandType.StoredProcedure, "dbo.AuthorMaster", objparam);
        }

        public DataSet CategoryMaster(string action, int categoryID = 0, string categoryName = "",
                           string description = "", bool active = true, int adminUserID = 0)
        {
            SqlParameter[] objparam = new SqlParameter[]
            {
                new SqlParameter("@Action", action),
                new SqlParameter("@CategoryID", categoryID),
                new SqlParameter("@CategoryName", (object)categoryName ?? DBNull.Value),
                new SqlParameter("@Description", (object)description ?? DBNull.Value),
                new SqlParameter("@Active", active),
                new SqlParameter("@AdminUserID", adminUserID),

            };

            return SqlHelper.ExecuteDataset(objSqlConnection,
                    CommandType.StoredProcedure, "dbo.CategoryMaster", objparam);
        }

        
        public DataSet BookMaster(
           string Action,
           int BookID,
           string ISBN,
           int CategoryID,
           string BookTitle,
           string Language,
           string PublisherName,
           int YearPublished,
           string Edition,
           decimal Price,
           int TotalCopies,

           string ShelfLocation,
           bool Active,
           string AuthorIDs,
           int AdminUserID,
           string searchBy,
           string searchValue
       )
        {
            SqlParameter[] objParam = new SqlParameter[]
            {
        new SqlParameter("@Action", Action),
        new SqlParameter("@BookID", BookID),
        new SqlParameter("@ISBN", (object)ISBN ?? DBNull.Value),
        new SqlParameter("@CategoryID", CategoryID),
        new SqlParameter("@BookTitle", (object)BookTitle ?? DBNull.Value),
        new SqlParameter("@Language", (object)Language ?? DBNull.Value),
        new SqlParameter("@PublisherName", (object)PublisherName ?? DBNull.Value),
        new SqlParameter("@YearPublished", YearPublished),
        new SqlParameter("@Edition", (object)Edition ?? DBNull.Value),
        new SqlParameter("@Price", Price),
        new SqlParameter("@TotalCopies", TotalCopies),

        new SqlParameter("@ShelfLocation", (object)ShelfLocation ?? DBNull.Value),
        new SqlParameter("@Active", Active),
        new SqlParameter("@AuthorIDs", (object)AuthorIDs ?? DBNull.Value),
        new SqlParameter("@AdminUserID", AdminUserID),
        new SqlParameter("@SearchBy", (object)searchBy ?? DBNull.Value),
        new SqlParameter("@SearchValue", (object)searchValue ?? DBNull.Value)
            };

            return SqlHelper.ExecuteDataset(objSqlConnection,
                        CommandType.StoredProcedure, "dbo.BookMasterPage", objParam);
        }

        public DataSet BookAuthor(string action, int bookId, int? authorId = null)
        {
            SqlParameter[] objParam = new SqlParameter[]
            {
                    new SqlParameter("@Action", action),
                    new SqlParameter("@BookID", bookId),
                    new SqlParameter("@AuthorID", authorId),

            };
            return SqlHelper.ExecuteDataset(objSqlConnection,
                       CommandType.StoredProcedure, "dbo.BookAuthorMapping", objParam);
        }




    }
}