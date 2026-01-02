using AppMGL.DAL.Helper;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace AppMGL.Manager.Areas.Setup.Controllers
{
    public class ContactController : BaseController<ContactDTO, ContactRepository, LG_CONTACT>
    {
        #region Constructor

        public ContactController(ContactRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Contact;
            KeyField = "ContactId";
        }

        #endregion

        [System.Web.Http.HttpPost]
        public override ActionResult Lookup(ListParams listParams)
        {
            try
            {
                var sortOrder = JsonConvert.DeserializeObject<Dictionary<string, string>>(listParams.Sort);
                int TotalRows = 0;

              int   CwtID = (int)listParams.CwtId;
              int   UserID = (int)listParams.UserId;

                List<ContactDTO> result = _context.ExecuteQuery<ContactDTO>("EXEC dbo.LG_GET_CONTACT_CWT @PAGENO, @PAGESIZE,@USER_WORKTYPE_ID,@SORTCOLUMN,@SORTORDER,@USER_ID",
                        new SqlParameter("PAGENO", listParams.PageIndex),
                        new SqlParameter("PAGESIZE", listParams.PageSize),
                        new SqlParameter("USER_WORKTYPE_ID", CwtID),
                        new SqlParameter("SORTCOLUMN", sortOrder.First().Key),
                        new SqlParameter("SORTORDER", sortOrder.First().Value),
                        new SqlParameter("@USER_ID", UserID)
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
