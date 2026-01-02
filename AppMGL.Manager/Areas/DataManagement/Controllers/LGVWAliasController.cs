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
    public class LGVWAliasController : BaseController<LGVWAliasDTO, LGVWAliasRepository, LG_VW_Alias>
    {
         

        public LGVWAliasController(LGVWAliasRepository context)
        {
            _context = context;
            BaseModule = EnumModule.LGVWAlias;
            KeyField = "AliasID";
        }



        [System.Web.Http.HttpPost]
        public override ActionResult Insert(LGVWAliasDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_CreateAlias_IUD @userid,@Alias,@Type,@AliasReferenceID,@aliasid,@ActionType",
              new SqlParameter("userid", dto.CreatedBy ?? Convert.DBNull),
              new SqlParameter("Alias", dto.Alias ?? Convert.DBNull),
              new SqlParameter("Type", dto.Type ?? Convert.DBNull),
              new SqlParameter("AliasReferenceID", dto.AliasReferenceID ?? Convert.DBNull),
              new SqlParameter("aliasid", dto.AliasID ?? Convert.DBNull),
              new SqlParameter("ActionType", "insert")).ToList();
                List<int> objList = objResult.ToList();
                long valiasid = objList[0];
                var result = _context.Detail(valiasid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, LGVWAliasDTO dto)
        {
            try
            {

                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_CreateAlias_IUD @userid,@Alias,@Type,@AliasReferenceID,@aliasid,@ActionType",
              new SqlParameter("userid", dto.ModifiedBy ?? Convert.DBNull),
              new SqlParameter("Alias", dto.Alias ?? Convert.DBNull),
              new SqlParameter("Type", dto.Type ?? Convert.DBNull),
              new SqlParameter("AliasReferenceID", dto.AliasReferenceID ?? Convert.DBNull),
              new SqlParameter("aliasid", dto.AliasID ?? Convert.DBNull),
              new SqlParameter("ActionType", "update")).ToList();
                List<int> objList = objResult.ToList();
                long valiasid = objList[0];
                var result = _context.Detail(valiasid);
                
                return AppResult(result, 1, PrepareMessage(EnumAction.Update), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(LGVWAliasDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.LG_CreateAlias_IUD @userid,@Alias,@Type,@AliasReferenceID,@aliasid,@ActionType",
                new SqlParameter("userid", dto.ModifiedBy ?? Convert.DBNull),
                new SqlParameter("Alias", dto.Alias ?? Convert.DBNull),
                new SqlParameter("Type", dto.Type ?? Convert.DBNull),
                new SqlParameter("AliasReferenceID", dto.AliasReferenceID ?? Convert.DBNull),
                new SqlParameter("aliasid", dto.AliasID ?? Convert.DBNull),
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