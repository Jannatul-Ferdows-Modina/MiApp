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
    public class SIPLCountryController : BaseController<SIPLCountryDTO, SIPLCountryRepository, SIPL_Country>
    {
        #region Constructor

        public SIPLCountryController(SIPLCountryRepository context)
        {
            _context = context;
            BaseModule = EnumModule.SIPLCountry;
            KeyField = "CountryId";
        }
        [System.Web.Http.HttpPut]
        public override ActionResult Detail(long id)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>
                {
                    new SqlParameter("@CountryId", id)
                };
                List<SIPLCountryDTO> PortDetail = _context.ExecuteQuery<SIPLCountryDTO>("GET_SIPl_CountryById @CountryId", param.ToArray()).ToList();
                var result = PortDetail[0];
                return AppResult(result, "");


            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }


        #endregion
    }
}