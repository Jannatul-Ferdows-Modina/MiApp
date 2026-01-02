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
    public class LGVWStateController : BaseController<LGVWStateDTO, LGVWStateRepository, LG_VW_SIPLState>
    {
         
        public LGVWStateController(LGVWStateRepository context)
        {
            _context = context;
            BaseModule = EnumModule.LGVWState;
            KeyField = "StateId";
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Insert(LGVWStateDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SIPLState_IUD @StateID,@Name,@StateCode,@IsActive,@CreatedBy,@ModifiedBy,@fkCountryID,@ActionType",
                               new SqlParameter("StateID", dto.StateId ?? Convert.DBNull),
                               new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                               new SqlParameter("StateCode", dto.StateCode ?? Convert.DBNull),
                               new SqlParameter("IsActive", dto.IsActive),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                               new SqlParameter("fkCountryID", dto.fkCountryId ?? Convert.DBNull),
                               new SqlParameter("ActionType", "insert")).ToList();
                List<int> objList = objResult.ToList();
                long vstateid = objList[0];
                var result = _context.Detail(vstateid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, LGVWStateDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SIPLState_IUD @StateID,@Name,@StateCode,@IsActive,@CreatedBy,@ModifiedBy,@fkCountryID,@ActionType",
                new SqlParameter("StateID", dto.StateId ?? Convert.DBNull),
                new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                new SqlParameter("StateCode", dto.StateCode ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("fkCountryID", dto.fkCountryId ?? Convert.DBNull),
                new SqlParameter("ActionType", "update")).ToList();
                List<int> objList = objResult.ToList();
                long vstateid = objList[0];

                var result = _context.Detail(vstateid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Update), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(LGVWStateDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_SIPLState_IUD @StateID,@Name,@StateCode,@IsActive,@CreatedBy,@ModifiedBy,@fkCountryID,@ActionType",
                new SqlParameter("StateID", dto.StateId ?? Convert.DBNull),
                new SqlParameter("Name", dto.Name ?? Convert.DBNull),
                new SqlParameter("StateCode", dto.StateCode ?? Convert.DBNull),
                new SqlParameter("IsActive", dto.IsActive),
                new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                new SqlParameter("ModifiedBy", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("fkCountryID", dto.fkCountryId ?? Convert.DBNull),
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