using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.Manager.Infrastructure;
using AutoMapper;

namespace AppMGL.Manager.Areas.Security.Controllers
{
    public class SiteRoleController : BaseController<SiteRoleDTO, SiteRoleRepository, LG_SITE_CONTACT_ROLE>
    {
        #region Constructor

        public SiteRoleController(SiteRoleRepository context)
        {
            _context = context;
            BaseModule = EnumModule.SiteRole;
            KeyField = "PolicyRolesId";
        }

        #endregion

        #region Public Method

        [System.Web.Http.HttpPost]
        public virtual ActionResult ListRole(Dictionary<string, string> param)
        {
            try
            {
                var result = _context.ListRole(param).ToList();
                int count = result.Count;
                var dtoResult = Mapper.Map<List<SiteRoleDTO>>(result);
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
