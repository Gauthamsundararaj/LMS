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
        //public DataSet AuthorMasterSelect(string action)
        //{
        //    SqlParameter[] objSqlParam = new SqlParameter[1];
        //    objSqlParam[0] = new SqlParameter("@Action", action);
        //    return SqlHelper.ExecuteDataset(objSqlConnection,CommandType.StoredProcedure,"dbo.AuthorMaster", objSqlParam );
        //}
        //public DataSet AuthorMasterInsert(string action,string authorName,string authorType,bool active,int adminuserID)
        //{
        //    SqlParameter[] objSqlParam = new SqlParameter[6];
        //    objSqlParam[0] = new SqlParameter("@Action", action);
        //    objSqlParam[1] = new SqlParameter("@AuthorName", (object)authorName ?? DBNull.Value);
        //    objSqlParam[2] = new SqlParameter("@AuthorType", (object)authorType ?? DBNull.Value);
        //    objSqlParam[3] = new SqlParameter("@Active", active);
        //    objSqlParam[4] = new SqlParameter("@AdminUserID", adminuserID);
        //    return SqlHelper.ExecuteDataset(objSqlConnection, CommandType.StoredProcedure, "dbo.AuthorMaster", objSqlParam);
        //}
        //public DataSet AuthorMasterUpdate(string action,int authorID, string authorName, string authorType, bool active, int adminuserID)
        //{
        //    SqlParameter[] objSqlParam = new SqlParameter[7];
        //    objSqlParam[0] = new SqlParameter("@Action", action);
        //    objSqlParam[1] = new SqlParameter("@AuthorID", authorID);
        //    objSqlParam[2] = new SqlParameter("@AuthorName", (object)authorName ?? DBNull.Value);
        //    objSqlParam[3] = new SqlParameter("@AuthorType", (object)authorType ?? DBNull.Value);
        //    objSqlParam[4] = new SqlParameter("@Active", active);
        //    objSqlParam[5] = new SqlParameter("@AdminUserID", adminuserID);
        //    return SqlHelper.ExecuteDataset(objSqlConnection, CommandType.StoredProcedure, "dbo.AuthorMaster", objSqlParam);
        //}
        //public DataSet AuthorMasterDelete(string action, int authorID)
        //{
        //    SqlParameter[] objSqlParam = new SqlParameter[2];
        //    objSqlParam[0] = new SqlParameter("@Action", action);
        //    objSqlParam[1] = new SqlParameter("@AuthorID",authorID);
        //    return SqlHelper.ExecuteDataset(objSqlConnection, CommandType.StoredProcedure, "dbo.AuthorMaster", objSqlParam);
        //}

        //public DataSet AuthorMaster(string action,int authorID )
        //{
        //    SqlParameter[] objSqlParam = new SqlParameter[2];
        //    objSqlParam[0] = new SqlParameter("@Action", action);
        //    objSqlParam[1] = new SqlParameter("@AuthorID", (object)authorID ?? DBNull.Value);

        //    return SqlHelper.ExecuteDataset(
        //        objSqlConnection,
        //        CommandType.StoredProcedure,
        //        "dbo.AuthorMaster",
        //        objSqlParam
        //    );
        //}

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

        //public DataSet CategoryMaster(string action,
        //    int? categoryID,
        //    string categoryName,
        //    string description,
        //    bool active = true,
        //    int createdBy = 0,
        //    int updatedBy = 0,
        //    bool isUpdate = false)
        //{
        //    SqlParameter[] objSqlParam = new SqlParameter[8];

        //    objSqlParam[0] = new SqlParameter("@Action", action);
        //    objSqlParam[1] = new SqlParameter("@CategoryID", (object)categoryID ?? DBNull.Value);
        //    objSqlParam[2] = new SqlParameter("@CategoryName", (object)categoryName ?? DBNull.Value);

        //    objSqlParam[3] = new SqlParameter("@Description", SqlDbType.VarChar, 500);
        //    objSqlParam[3].Value = (object)description ?? DBNull.Value;

        //    objSqlParam[4] = new SqlParameter("@Active", active);
        //    objSqlParam[5] = new SqlParameter("@CreatedBy", createdBy);
        //    objSqlParam[6] = new SqlParameter("@UpdatedBy", updatedBy);

        //    // IMPORTANT FIX
        //    objSqlParam[7] = new SqlParameter("@Update", isUpdate);

        //    return SqlHelper.ExecuteDataset(
        //        objSqlConnection,
        //        CommandType.StoredProcedure,
        //        "dbo.CategoryMaster",
        //        objSqlParam
        //    );
        //}


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
                        CommandType.StoredProcedure, "dbo.spBookMaster", objParam);
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