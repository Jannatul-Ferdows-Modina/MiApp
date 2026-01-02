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
    public class LGVWhtscodeController : BaseController<LGVWHtsCodeDTO, LGVWChtscodeRepository, LG_VW_SIPLCity>
    {
        

        public LGVWhtscodeController(LGVWChtscodeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.LGVWHtsCode;
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
                condition = condition.Replace("Name", "HTSNumber");
                Dictionary<string, string> source = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                List<LGVWHtsCodeDTO> list = _context.ExecuteQuery<LGVWHtsCodeDTO>("EXEC dbo.LG_HTSCodeList_SP @PAGENO, @PAGESIZE,@SORTCOLUMN,@SORTORDER,@HTSNumber", new object[5]
                {
                   new SqlParameter("PAGENO", listParams.PageIndex),
                   new SqlParameter("PAGESIZE", listParams.PageSize),
                   new SqlParameter("SORTCOLUMN", source.First().Key.Replace("Name", "HTSNumber")),
                   new SqlParameter("SORTORDER", source.First().Value),
                   new SqlParameter("HTSNumber", condition)
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
                               new SqlParameter("htscode", ""),
                               new SqlParameter("description", ""),
                               new SqlParameter("unit", ""),
                               new SqlParameter("unit1", ""),
                               new SqlParameter("indent", ""),
                               new SqlParameter("isvin", ""),
                               new SqlParameter("isDel", ""),
                               new SqlParameter("CreatedBy", ""),
                               new SqlParameter("ActionType", "detail")
                };
                List<LGVWHtsCodeDTO> Detail = _context.ExecuteQuery<LGVWHtsCodeDTO>("USP_LG_HtsCode @Id,@htscode,@description,@unit,@unit1,@indent,@isvin,@isDel,@CreatedBY,@ActionType", param.ToArray()).ToList();
                if (Detail[0].HTSNumber != null && Detail[0].HTSNumber != "")
                {
                    string htsn = Detail[0].HTSNumber.ToString();
                    string[] htsn1  = htsn.Split('.');
                    if(htsn1.Length>0)
                    Detail[0].htsfirst = htsn1[0]==null?"":htsn1[0].ToString();
                    if (htsn1.Length > 1)
                        Detail[0].htssecond = htsn1[1] == null ? "" : htsn1[1].ToString();
                    if (htsn1.Length > 2)
                        Detail[0].htsthird = htsn1[2] == null ? "" : htsn1[2].ToString();
                    //if (htsn1.Length > 3)
                    //    Detail[0].htsfourth = htsn1[3] == null ? "" : htsn1[3].ToString();
                }
                
                var result = Detail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(LGVWHtsCodeDTO dto)
         {
            try
            {
                HtsCode data = null;
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_HtsCode @Id,@htscode,@description,@unit,@unit1,@indent,@isvin,@isDel,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("htscode", dto.HTSNumber ?? Convert.DBNull),
                               new SqlParameter("description", dto.description ?? Convert.DBNull),
                               new SqlParameter("unit", dto.FirstUOM ?? Convert.DBNull),
                               new SqlParameter("unit1", dto.SecondUOM ?? Convert.DBNull),
                               new SqlParameter("indent", dto.indent ?? Convert.DBNull),
                               new SqlParameter("isvin", dto.isvin == true ? 1 : 0),
                               new SqlParameter("isDel", dto.isDel == true ? 1 : 0),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", dto.Id == null || dto.Id == 0 ? "insert" : "update")).ToList();
               
                List<int> objList = objResult.ToList();
                int num = objList[0];
                if (num > 0)
                {
                    List<HtsCode> list2 = _context.ExecuteQuery<HtsCode>("EXEC dbo.USP_LG_GET_HtsCode @Id", new object[1]
                   {
                        new SqlParameter("Id", num)
                   }).ToList();
                    data = list2[0];
                }
                return AppResult(data, 1L, "", EnumResult.Success);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, LGVWHtsCodeDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_HtsCode @Id,@htscode,@description,@unit,@unit1,@indent,@isvin,@isDel,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("htscode", dto.HTSNumber ?? Convert.DBNull),
                               new SqlParameter("description", dto.description ?? Convert.DBNull),
                               new SqlParameter("unit", dto.FirstUOM ?? Convert.DBNull),
                               new SqlParameter("unit1", dto.SecondUOM ?? Convert.DBNull),
                               new SqlParameter("indent", dto.indent ?? Convert.DBNull),
                               new SqlParameter("isvin", dto.isvin==true?1:0 ),
                               new SqlParameter("isDel", dto.isDel == true ? 1 : 0),
                               new SqlParameter("CreatedBy", dto.CreatedBy ?? Convert.DBNull),
                               new SqlParameter("ActionType", "update")
                               ).ToList();
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
        public override ActionResult Delete(LGVWHtsCodeDTO dto)
        {
            try
            {
                IEnumerable<int> objResult = _context.ExecuteQuery<int>("EXEC dbo.USP_LG_HtsCode @Id,@htscode,@description,@unit,@unit1,@indent,@isvin,@IsDel,@CreatedBY,@ActionType",
                               new SqlParameter("ID", dto.Id ?? Convert.DBNull),
                               new SqlParameter("htscode", dto.HTSNumber ?? Convert.DBNull),
                               new SqlParameter("description", dto.description ?? Convert.DBNull),
                               new SqlParameter("unit", dto.FirstUOM ?? Convert.DBNull),
                               new SqlParameter("unit1", dto.SecondUOM ?? Convert.DBNull),
                               new SqlParameter("indent", dto.indent ?? Convert.DBNull),
                               new SqlParameter("isvin", dto.isvin == true ? 1 : 0),
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
        [System.Web.Http.HttpPost]
        public ActionResult htsCodeSearch(ListParams listParams)
        {
            try
            {

                // int count;
                var result = _context.ExecuteQuery<HtsCode>("EXEC dbo.SP_HTSCode_Search @SEARCHVALUE",
                    new SqlParameter("SEARCHVALUE", listParams.Filter)).ToList();
                return AppResult(result, result.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
    }
}