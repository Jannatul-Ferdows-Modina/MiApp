using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class SIPLUserController : BaseController<SIPLUserDTO, SIPLUserRepository, LG_VW_SITE_CONTACT>
    {
        #region Constructor

        public SIPLUserController(SIPLUserRepository context)
        {
            _context = context;
            BaseModule = EnumModule.SIPLUser;
            KeyField = "UsrId";
        }

        #endregion
        [System.Web.Http.HttpPost]
        public override ActionResult Lookup(ListParams listParams)
        {
            try
            {
               
                int count;
                var result = _context.List(listParams, out count)
                    .GroupBy(x => x.UsrId) // Group by unique property (e.g., Id)
                     .Select(g => g.First()) // Select the first unique item from each group
                     .ToList();
                var dtoResult = Mapper.Map<List<SIPLUserDTO>>(result);
                return AppResult(dtoResult, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        [System.Web.Http.HttpPost]
        public  ActionResult GetUserWithCorporate(ListParams listParams)
        {
            try
            {
                var sid = listParams.SiteId;
                 var  objsipluser = _context.ExecuteQuery<SIPLUserDTOList>("EXEC dbo.GetUserListWithCorporate @SiteId",
                   new SqlParameter("SiteId", sid)).ToList();
                return AppResult(objsipluser, objsipluser.Count());
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

    }
}