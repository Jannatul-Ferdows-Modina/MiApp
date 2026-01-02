using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;
using AutoMapper;

using AppMGL.DAL.Helper;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Linq;
namespace AppMGL.Manager.Areas.Security.Controllers
{
    public class SiteController : BaseController<SiteDTO, SiteRepository, LG_SITE>
    {
        #region Constructor

        public LocationRepository locationRepository;
        public SiteRoleRepository siteRoleRepository;

        public SiteController(SiteRepository context, LocationRepository locationRepository, SiteRoleRepository siteRoleRepository)
        {
            _context = context;
            BaseModule = EnumModule.Site;
            KeyField = "SiteId";

            this.locationRepository = locationRepository;
            this.siteRoleRepository = siteRoleRepository;
        }

        #endregion
        [System.Web.Http.HttpGet]
        public virtual ActionResult GetLoginUserUnits(int id)
        {
            try
            {
                List<LG_SITE> userSite;
                var repo = new AuthRepository();
                userSite = repo.FindSite(id.ToString());
                var dtoResult = Mapper.Map<List<SiteDTO>>(userSite);
                //int TotalCount;                
                //var result = _context.ExecuteQuery<LG_LOCATION>("EXEC dbo.USP_LG_GET_SITE_LOCATION").ToList();
                return AppResult(dtoResult, dtoResult.Count);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }
        #region Override Methods
        [System.Web.Http.HttpPost]
        public override ActionResult List(ListParams listParams)
        {
            try
            {
                var userId = 0;                
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                if(listParams.UserWorkTypeId == 2)
                {
                    userId = (int)(listParams.UserId);
                }
                List<SiteDTO> result = _context.ExecuteQuery<SiteDTO>("EXEC dbo.USP_LG_USER_GET_SITES @PAGENO, @PAGESIZE,@USER_ID,@SORTCOLUMN,@SORTORDER",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("USER_ID", userId),                       
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value)
                        
                        ).ToList();
                if (result.Count > 0)
                {
                    TotalRows = result[0].TotalCount.Value;
                }
                return AppResult(result, TotalRows);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpGet]
        public override ActionResult Detail(long id)
        {
            try
            {
                var result = _context.Detail(id);
                var dtoResult = Mapper.Map<SiteDTO>(result);

                dtoResult.Location = Mapper.Map<LocationDTO>(result.LG_LOCATION);

                return AppResult(dtoResult, "");
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Insert(SiteDTO dto)
        {
            try
            {
                var entity = Mapper.Map<LG_SITE>(dto);
                _context.Insert(entity);

                _context.UnitOfWork.Commit();

                var result = _context.Detail(GetPrimaryKeyValue(entity));
                var dtoResult = Mapper.Map<SiteDTO>(result);

                dtoResult.Location = Mapper.Map<LocationDTO>(result.LG_LOCATION);

                return AppResult(dtoResult, PrepareMessage(EnumAction.Insert));
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPut]
        public override ActionResult Update(long id, SiteDTO dto)
        {
            try
            {
                var entity = Mapper.Map<LG_SITE>(dto);
                _context.Update(entity);

                _context.UnitOfWork.Commit();

                var result = _context.Detail(GetPrimaryKeyValue(entity));
                var dtoResult = Mapper.Map<SiteDTO>(result);

                dtoResult.Location = Mapper.Map<LocationDTO>(result.LG_LOCATION);

                return AppResult(dtoResult, PrepareMessage(EnumAction.Update));
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        [System.Web.Http.HttpPost]
        public override ActionResult Delete(SiteDTO dto)
        {
            try
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("SitId", dto.SitId.ToString());

                var siteRoles = siteRoleRepository.ListRole(param);
                foreach (var sr in siteRoles)
                {
                    siteRoleRepository.Delete(sr);
                }

                siteRoleRepository.UnitOfWork.Commit();

                var entity = Mapper.Map<LG_SITE>(dto);
                var result = _context.Delete(entity);
                _context.UnitOfWork.Commit();

                var location = locationRepository.Detail(Convert.ToInt64(dto.LcnId));
                locationRepository.Delete(location);
                locationRepository.UnitOfWork.Commit();

                var dtoResult = Mapper.Map<SiteDTO>(result);
                return AppResult(dtoResult, PrepareMessage(EnumAction.Delete));
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        [System.Web.Http.HttpGet]
        public  ActionResult GetAllSiteList()
        {
            try
            {               
                List<SiteDTO> result = _context.ExecuteQuery<SiteDTO>("EXEC dbo.USP_LG_USER_GET_All_SITES").ToList();
                if (result.Count > 0)
                {
                   
                }
                return AppResult(result, 0);
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

        #endregion
    }
}
