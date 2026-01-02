using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.Manager.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace AppMGL.Manager.Areas.Security.Controllers
{
    public class ContactWorkTypeController : BaseController<ContactWorkTypeDTO, ContactWorkTypeRepository, LG_CONTACT_WORK_TYPE>
    {
        #region Constructor

        public ContactWorkTypeController(ContactWorkTypeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.ContactWorkType;
            KeyField = "CwtId";
        }

        #endregion

        [System.Web.Http.HttpPost]
        public override ActionResult Lookup(ListParams listParams)
        {
            try
            {
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;
                int CwtID = (int)listParams.CwtId;

                List<ContactWorkTypeDTO> result = _context.ExecuteQuery<ContactWorkTypeDTO>("EXEC dbo.LG_GET_CWT_TYP @PAGENO, @PAGESIZE,@USER_WORKTYPE_ID,@SORTCOLUMN,@SORTORDER",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("USER_WORKTYPE_ID", CwtID),
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
    }
}
