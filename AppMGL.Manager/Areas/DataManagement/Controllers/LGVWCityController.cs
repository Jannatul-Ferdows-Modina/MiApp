using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class LGVWCityController : BaseController<LGVWCityDTO, LGVWChtscodeRepository, LG_VW_SIPLCity>
    {
        

        public LGVWCityController(LGVWChtscodeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.LGVWCity;
            KeyField = "CityId";
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(LGVWCityDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SIPLCity_IUD @CityID,@Name,@IsActive,@CreatedBy,@ModifiedBy,@fkStateID,@ActionType",
                              new SqlParameter("CityID", dto.CityId ?? Convert.DBNull),
                               new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                               new SqlParameter("fkStateID", dto.fkStateId ?? Convert.DBNull),
                               new SqlParameter("ActionType", "insert")).ToList();
                List<int> objList = objResult.ToList();
                long vcityid = objList[0];
                var result = _context.Detail(vcityid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, LGVWCityDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SIPLCity_IUD @CityID,@Name,@IsActive,@CreatedBy,@ModifiedBy,@fkStateID,@ActionType",
                new SqlParameter("CityID", dto.CityId ?? Convert.DBNull),
                new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("fkStateID", dto.fkStateId ?? Convert.DBNull),
                new SqlParameter("ActionType", "update")).ToList();
                List<int> objList = objResult.ToList();
                long vcityid = objList[0];

                var result = _context.Detail(vcityid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Update), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(LGVWCityDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SIPLCity_IUD @CityID,@Name,@IsActive,@CreatedBy,@ModifiedBy,@fkStateID,@ActionType",
                new SqlParameter("CityID", dto.CityId ?? Convert.DBNull),
                new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("fkStateID", dto.fkStateId ?? Convert.DBNull),
                new SqlParameter("ActionType", "delete")).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }



    }
}