using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class LGACCTCategoryController : BaseController<LGACCTCategoryDTO, LGACCTCategoryRepository, LG_ACCT_CATEGORY>
    {
         

        public LGACCTCategoryController(LGACCTCategoryRepository context)
        {
            _context = context;
            BaseModule = EnumModule.LGACCTCategory;
            KeyField = "AcyID";
        }
    }
}