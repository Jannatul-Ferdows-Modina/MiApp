
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class ContactCategoryController : BaseController<AppMGL.DTO.DataManagement.ContactCategoryDTO, ContactCategoryRepository, AppMGL.DAL.Models.LG_VW_CONTACTCATEGORY>
    {
        

        public ContactCategoryController(ContactCategoryRepository context)
        {
            _context = context;
            BaseModule = EnumModule.ContactCategory;
            KeyField = "ContactCategoryId";
        }


        [System.Web.Http.HttpPost]
        public override ActionResult Insert(ContactCategoryDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_ContactCategory_IUD @ContactCategoryID,@Name,@CreatedBy,@ModifiedBy,@IsActive,@IsVendor,@ActionType,@CSIDCommodity,@CSIDOrigin,@CSIDDestination",

                new SqlParameter("ContactCategoryID", dto.ContactCategoryId ?? Convert.DBNull),
                new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("IsVendor", dto.IsVendor),
                new SqlParameter("ActionType", "insert"),
                new SqlParameter("CSIDCommodity", dto.Commodity ?? Convert.DBNull),
                new SqlParameter("CSIDOrigin", dto.Origin ?? Convert.DBNull),
                new SqlParameter("CSIDDestination", dto.Destination ?? Convert.DBNull)).ToList();
                List<int> objList = objResult.ToList();
                long vcontactcategoryid = objList[0];
                var result = _context.Detail(vcontactcategoryid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, ContactCategoryDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_ContactCategory_IUD @ContactCategoryID,@Name,@CreatedBy,@ModifiedBy,@IsActive,@IsVendor,@ActionType,@CSIDCommodity,@CSIDOrigin,@CSIDDestination",
                new SqlParameter("ContactCategoryID", dto.ContactCategoryId ?? Convert.DBNull),
                new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("IsVendor", dto.IsVendor),
                new SqlParameter("ActionType", "update"),
                new SqlParameter("CSIDCommodity", dto.Commodity ?? Convert.DBNull),
                new SqlParameter("CSIDOrigin", dto.Origin ?? Convert.DBNull),
                new SqlParameter("CSIDDestination", dto.Destination ?? Convert.DBNull)).ToList();
                List<int> objList = objResult.ToList();
                long vcontactcategoryid = objList[0];

                var result = _context.Detail(vcontactcategoryid);
                
               return AppResult(result, 1, PrepareMessage(EnumAction.Update), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(ContactCategoryDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_ContactCategory_IUD @ContactCategoryID,@Name,@CreatedBy,@ModifiedBy,@IsActive,@IsVendor,@ActionType,@CSIDCommodity,@CSIDOrigin,@CSIDDestination",

                new SqlParameter("ContactCategoryID", dto.ContactCategoryId ?? Convert.DBNull),
                new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("IsVendor", dto.IsVendor),
                new SqlParameter("ActionType", "delete"),
                new SqlParameter("CSIDCommodity", dto.Commodity ?? Convert.DBNull),
                new SqlParameter("CSIDOrigin", dto.Origin ?? Convert.DBNull),
                new SqlParameter("CSIDDestination", dto.Destination ?? Convert.DBNull)).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult List(AppMGL.DAL.Helper.ListParams listParams)
        {
            try
            {
                int count;
                var result = _context.List(listParams, out count).ToList();
               // var dtoResult = Mapper.Map<List<T>>(result);
                return AppResult(result, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
    }
}