using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.Manager.Infrastructure;
using AutoMapper;

namespace AppMGL.Manager.Areas.Security.Controllers
{
    public class RoleMapController : BaseController<RoleMapDTO, RoleMapRepository, LG_MAM_ROL_MAP>
    {
        #region Constructor

        public RoleMapController(RoleMapRepository context)
        {
            _context = context;
            BaseModule = EnumModule.RoleMap;
            KeyField = "MrmId";
        }

        #endregion

        #region Public Method

        [System.Web.Http.HttpPost]
        public virtual ActionResult ListRoleMap(Dictionary<string, string> param)
        {
            try
            {
                var result = _context.ListRoleMap(param).ToList();
                int count = result.Count;
                var dtoResult = Mapper.Map<List<RoleMapDTO>>(result);
                return AppResult(dtoResult, count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        #endregion
    }
}
