using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
using Newtonsoft.Json;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class LGVWlienseexemptionController : BaseController<LGVWLienseexemptionDTO, LGVWChtscodeRepository, LG_VW_SIPLCity>
    {
        

        public LGVWlienseexemptionController(LGVWChtscodeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.LGVWlienseexemption;
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
                condition = condition.Replace("Name", "LicenseCode");
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<LGVWLienseexemptionDTO> list = _context.ExecuteQuery<LGVWLienseexemptionDTO>("EXEC dbo.LG_LienseexemptionList_SP @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@Condition", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key.Replace("Name", "LicenseCode")),
                   new SqlParameter("SORTORDER", source.First().Value),
                   new SqlParameter("Condition", condition)
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
                               
                               new SqlParameter("ID",id ),
                               new SqlParameter("LicenseCode", ""),
                               new SqlParameter("description", ""),
                               new SqlParameter("licval", ""),
                               new SqlParameter("isDel", ""),
                               new SqlParameter("CreatedBy", ""),
                               new SqlParameter("ActionType", "detail")
                };
                List<LGVWLienseexemptionDTO> Detail = _context.ExecuteQuery<LGVWLienseexemptionDTO>("USP_LG_LienseExemption @Id,@LicenseCode,@description,@licval,@isDel,@CreatedBY,@ActionType", param.ToArray()).ToList();
                var result = Detail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(LGVWLienseexemptionDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_LienseExemption @Id,@LicenseCode,@description,@licval,@isDel,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("LicenseCode", dto.LicenseCode ?? Convert.DBNull),
                               new SqlParameter("description", dto.description ?? Convert.DBNull),
                               new SqlParameter("licval", dto.licval ?? Convert.DBNull),
                               new SqlParameter("isDel", dto.isDel == true ? 1 : 0),
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
        public override ActionResult Update(long id, LGVWLienseexemptionDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_LienseExemption @Id,@LicenseCode,@description,@licval,@isDel,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("LicenseCode", dto.LicenseCode ?? Convert.DBNull),
                               new SqlParameter("description", dto.description ?? Convert.DBNull),
                               new SqlParameter("licval", dto.licval ?? Convert.DBNull),
                               new SqlParameter("isDel", dto.isDel == true ? 1 : 0),
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
        public override ActionResult Delete(LGVWLienseexemptionDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_LienseExemption @Id,@LicenseCode,@description,@licval,@isDel,@CreatedBY,@ActionType",
                                new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                                new SqlParameter("LicenseCode", dto.LicenseCode ?? Convert.DBNull),
                                new SqlParameter("description", dto.description ?? Convert.DBNull),
                                new SqlParameter("licval", dto.licval ?? Convert.DBNull),
                                new SqlParameter("isDel", dto.isDel == true ? 1 : 0),
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
    }
}