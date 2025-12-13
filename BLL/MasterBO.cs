using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DAL;
using Library;

namespace BLL
{
    public class MasterBO
    {
        private MasterDAO objMasterDAO;
        public MasterBO()
        {
            objMasterDAO = new MasterDAO();
        }
        public void ReleaseResources()
        {
            objMasterDAO.ReleaseResources();
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
       
        public DataSet AuthorMaster(string action, int authorID = 0, string authorName = "",
                            string authorType = "", bool active = true, int adminUserID = 0)
        {
            return objMasterDAO.AuthorMaster(action, authorID, authorName, authorType, active, adminUserID);
        }
        public DataSet CategoryMaster(string action, int categoryID = 0, string categoryName = "",
                           string description = "", bool active = true, int adminUserID = 0)
        {
            return objMasterDAO.CategoryMaster(action, categoryID, categoryName, description, active, adminUserID);
        }

        //public DataSet CategoryMaster(string action, int? categoryID, string categoryName, string description, bool Active=true, int createdBy = 0, int updatedBy = 0, bool isUpdate =false)
        //{
        //    return objMasterDAO.CategoryMaster(action, categoryID, categoryName, description, Active, createdBy, updatedBy, isUpdate);
        //}

    public DataSet BookMaster(
    string Action,
    int BookID = 0,
    string ISBN = "",
    int CategoryID = 0,
    string BookTitle = "",
    string Language = "",
    string PublisherName = "",
    int YearPublished = 0,
    string Edition = "",
    decimal Price = 0,
    int TotalCopies = 0,
    string ShelfLocation = "",
    bool Active = true,
    string AuthorIDs = "",
    int AdminUserID = 0,
     string searchBy= "",
    string searchValue = ""
)

        {
            return objMasterDAO.BookMaster(Action, BookID, ISBN, CategoryID, BookTitle, Language,
                PublisherName, YearPublished, Edition, Price, TotalCopies,
                ShelfLocation, Active, AuthorIDs, AdminUserID , searchBy, searchValue);
        }

        
        public DataSet BookAuthor(string action, int bookId, int? authorId = null)
        {
            return objMasterDAO.BookAuthor(action, bookId, authorId);
        }

       

    }


}

