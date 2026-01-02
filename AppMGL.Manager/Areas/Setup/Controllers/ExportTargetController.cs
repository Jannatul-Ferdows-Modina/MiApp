using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;
using Newtonsoft.Json;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class ExportTargetController : BaseController<ExportTargetDTO, LGVWChtscodeRepository, LG_VW_SIPLCity>
    {
        

        public ExportTargetController(LGVWChtscodeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.UserJobRole;
            KeyField = "ID";
        }
        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                int count = 0;
                string condition = "";
                condition= Utility.GetWhere1(listParams.Filter);
                condition = condition.Replace("Name", "TargetYear");
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<ExportTargetListDTO> list = _context.ExecuteQuery<ExportTargetListDTO>("EXEC dbo.USP_LG_ExportTargetList @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@TargetYear", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key.Replace("Name", "TargetYear")),
                   new SqlParameter("SORTORDER", source.First().Value),
                   new SqlParameter("TargetYear", condition)
                }).ToList();
                if (list.Count > 0)
                {
                    count = list[0].TotalCount;
                }
                return AppResult(list, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPut]
        public override ActionResult Detail(long id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                               
                               
                               new SqlParameter("TargetId", id),
                               new SqlParameter("FranchiseId", ""),
                               new SqlParameter("TargetYear", ""),
                               new SqlParameter("TargetJan",""),
                               new SqlParameter("TargetFeb", ""),
                               new SqlParameter("TargetMar", ""),
                               new SqlParameter("TargetApr", ""),
                               new SqlParameter("TargetMay", ""),
                               new SqlParameter("TargetJun", ""),
                               new SqlParameter("TargetJul", ""),
                               new SqlParameter("TargetAug", ""),
                               new SqlParameter("TargetSep", ""),
                               new SqlParameter("TargetOct", ""),
                               new SqlParameter("TargetNov", ""),
                               new SqlParameter("TargetDec", ""),
                               new SqlParameter("IsDel", ""),
                               new SqlParameter("CreatedBy", ""),
                               new SqlParameter("ActionType", "detail")
                };
                
                List <ExportTargetDTO> Detail = _context.ExecuteQuery<ExportTargetDTO>("dbo.USP_LG_ExportTarget @TargetId,@FranchiseId,@TargetYear,@TargetJan,@TargetFeb,@TargetMar,@TargetApr,@TargetMay,@TargetJun,@TargetJul,@TargetAug,@TargetSep,@TargetOct,@TargetNov,@TargetDec,@IsDel,@CreatedBY,@ActionType", param.ToArray()).ToList();
                Detail[0].TargetYear = Detail[0].TargetYear.ToString();
                var result = Detail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(ExportTargetDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ExportTarget @TargetId,@FranchiseId,@TargetYear,@TargetJan,@TargetFeb,@TargetMar,@TargetApr,@TargetMay,@TargetJun,@TargetJul,@TargetAug,@TargetSep,@TargetOct,@TargetNov,@TargetDec,@IsDel,@CreatedBY,@ActionType",
                               new SqlParameter("TargetId", dto.TargetId ?? Convert.DBNull),
                               new SqlParameter("FranchiseId", dto.FranchiseId ?? Convert.DBNull),
                               new SqlParameter("TargetYear", dto.TargetYear ?? Convert.DBNull),
                               new SqlParameter("TargetJan", dto.TargetJan ?? Convert.DBNull),
                               new SqlParameter("TargetFeb", dto.TargetFeb ?? Convert.DBNull),
                               new SqlParameter("TargetMar", dto.TargetMar ?? Convert.DBNull),
                               new SqlParameter("TargetApr", dto.TargetApr ?? Convert.DBNull),
                               new SqlParameter("TargetMay", dto.TargetMay ?? Convert.DBNull),
                               new SqlParameter("TargetJun", dto.TargetJun ?? Convert.DBNull),
                               new SqlParameter("TargetJul", dto.TargetJul ?? Convert.DBNull),
                               new SqlParameter("TargetAug", dto.TargetAug ?? Convert.DBNull),
                               new SqlParameter("TargetSep", dto.TargetSep ?? Convert.DBNull),
                               new SqlParameter("TargetOct", dto.TargetOct ?? Convert.DBNull),
                               new SqlParameter("TargetNov", dto.TargetNov ?? Convert.DBNull),
                               new SqlParameter("TargetDec", dto.TargetDec ?? Convert.DBNull),
                               new SqlParameter("IsDel", dto.IsDel == true ? 1 : 0),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", "insert")).ToList();
                List<int> objList = objResult.ToList();
                long htsid = objList[0];
                var result = _context.Detail(htsid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Insert), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, ExportTargetDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ExportTarget @TargetId,@FranchiseId,@TargetYear,@TargetJan,@TargetFeb,@TargetMar,@TargetApr,@TargetMay,@TargetJun,@TargetJul,@TargetAug,@TargetSep,@TargetOct,@TargetNov,@TargetDec,@IsDel,@CreatedBY,@ActionType",
                                new SqlParameter("TargetId", dto.TargetId ?? Convert.DBNull),
                               new SqlParameter("FranchiseId", dto.FranchiseId ?? Convert.DBNull),
                               new SqlParameter("TargetYear", dto.TargetYear ?? Convert.DBNull),
                               new SqlParameter("TargetJan", dto.TargetJan ?? Convert.DBNull),
                               new SqlParameter("TargetFeb", dto.TargetFeb ?? Convert.DBNull),
                               new SqlParameter("TargetMar", dto.TargetMar ?? Convert.DBNull),
                               new SqlParameter("TargetApr", dto.TargetApr ?? Convert.DBNull),
                               new SqlParameter("TargetMay", dto.TargetMay ?? Convert.DBNull),
                               new SqlParameter("TargetJun", dto.TargetJun ?? Convert.DBNull),
                               new SqlParameter("TargetJul", dto.TargetJul ?? Convert.DBNull),
                               new SqlParameter("TargetAug", dto.TargetAug ?? Convert.DBNull),
                               new SqlParameter("TargetSep", dto.TargetSep ?? Convert.DBNull),
                               new SqlParameter("TargetOct", dto.TargetOct ?? Convert.DBNull),
                               new SqlParameter("TargetNov", dto.TargetNov ?? Convert.DBNull),
                               new SqlParameter("TargetDec", dto.TargetDec ?? Convert.DBNull),
                               new SqlParameter("IsDel", dto.IsDel == true ? 1 : 0),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", "update")).ToList();
                List<int> objList = objResult.ToList();
                long htsid = objList[0];

                var result = _context.Detail(htsid);

                return AppResult(result, 1, PrepareMessage(EnumAction.Update), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Delete(ExportTargetDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_ExportTarget @TargetId,@FranchiseId,@TargetYear,@TargetJan,@TargetFeb,@TargetMar,@TargetApr,@TargetMay,@TargetJun,@TargetJul,@TargetAug,@TargetSep,@TargetOct,@TargetNov,@TargetDec,@IsDel,@CreatedBY,@ActionType",
                                new SqlParameter("TargetId", dto.TargetId ?? Convert.DBNull),
                               new SqlParameter("FranchiseId", dto.FranchiseId ?? Convert.DBNull),
                               new SqlParameter("TargetYear", dto.TargetYear ?? Convert.DBNull),
                               new SqlParameter("TargetJan", dto.TargetJan ?? Convert.DBNull),
                               new SqlParameter("TargetFeb", dto.TargetFeb ?? Convert.DBNull),
                               new SqlParameter("TargetMar", dto.TargetMar ?? Convert.DBNull),
                               new SqlParameter("TargetApr", dto.TargetApr ?? Convert.DBNull),
                               new SqlParameter("TargetMay", dto.TargetMay ?? Convert.DBNull),
                               new SqlParameter("TargetJun", dto.TargetJun ?? Convert.DBNull),
                               new SqlParameter("TargetJul", dto.TargetJul ?? Convert.DBNull),
                               new SqlParameter("TargetAug", dto.TargetAug ?? Convert.DBNull),
                               new SqlParameter("TargetSep", dto.TargetSep ?? Convert.DBNull),
                               new SqlParameter("TargetOct", dto.TargetOct ?? Convert.DBNull),
                               new SqlParameter("TargetNov", dto.TargetNov ?? Convert.DBNull),
                               new SqlParameter("TargetDec", dto.TargetDec ?? Convert.DBNull),
                               new SqlParameter("IsDel", dto.IsDel == true ? 1 : 0),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", "delete")).ToList();
                List<int> objList = objResult.ToList();
                return AppResult(objList, 1, PrepareMessage(EnumAction.Delete), EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        public override ActionResult Lookup(ListParams listParams)
        {
            try
            {
                var result = _context.ExecuteQuery<UserJobRoleDTO>("EXEC dbo.SP_UserJobRole_Search").ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
    }
}