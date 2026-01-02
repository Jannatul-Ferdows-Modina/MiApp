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
    public class SurchargeGroupController : BaseController<SurchargeGroupDTO, SurchargeGroupRepository, LG_VW_SurchargeGroup>
    {
         

        public SurchargeGroupController(SurchargeGroupRepository context)
        {
            _context = context;
            BaseModule = EnumModule.SurchargeGroup;
            KeyField = "SurchargesGroupId";
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Insert(SurchargeGroupDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SurchargeGroup_IUD @SurchargeGrpID, @SurchargeGrpName, @ShortCode, @IsActive, @CreatedBy, @ActionType, @SurchargeGrpSurchargeIds",
                              new SqlParameter("SurchargeGrpID", dto.SurchargesGroupID ?? Convert.DBNull),
                               new SqlParameter("SurchargeGrpName", dto.Name ?? Convert.DBNull),
                               new SqlParameter("ShortCode", dto.ShortCode ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", "insert"),
                               new SqlParameter("SurchargeGrpSurchargeIds", dto.Sgchecked ?? Convert.DBNull)).ToList();

                List<int> objList = objResult.ToList();
                long vsgid = objList[0];
                var result = _context.Detail(vsgid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, SurchargeGroupDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SurchargeGroup_IUD @SurchargeGrpID, @SurchargeGrpName, @ShortCode, @IsActive, @CreatedBy, @ActionType, @SurchargeGrpSurchargeIds",
                              new SqlParameter("SurchargeGrpID", dto.SurchargesGroupID ?? Convert.DBNull),
                               new SqlParameter("SurchargeGrpName", dto.Name ?? Convert.DBNull),
                               new SqlParameter("ShortCode", dto.ShortCode ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive),
                               new SqlParameter("CreatedBy", dto.ModifiedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", "update"),
                               new SqlParameter("SurchargeGrpSurchargeIds", dto.Sgchecked ?? Convert.DBNull)).ToList();

                List<int> objList = objResult.ToList();
                long vsgid = objList[0];
                var result = _context.Detail(vsgid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Update), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(SurchargeGroupDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SurchargeGroup_IUD @SurchargeGrpID, @SurchargeGrpName, @ShortCode, @IsActive, @CreatedBy, @ActionType, @SurchargeGrpSurchargeIds",
                              new SqlParameter("SurchargeGrpID", dto.SurchargesGroupID ?? Convert.DBNull),
                               new SqlParameter("SurchargeGrpName", dto.Name ?? Convert.DBNull),
                               new SqlParameter("ShortCode", dto.ShortCode ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                                new SqlParameter("ActionType", "delete"),
                               new SqlParameter("SurchargeGrpSurchargeIds", dto.Sgchecked ?? Convert.DBNull)).ToList();

                List<int> objList = objResult.ToList();
                long vsgid = objList[0];
                var result = _context.Detail(vsgid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

    }
}