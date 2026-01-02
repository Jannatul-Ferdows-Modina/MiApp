using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class LGSPFEECategoryController : BaseController<LGSPFEECategoryDTO, LGSPFEECategoryRepository, LG_SP_FEE_CATEGORY>
    {
         

        public LGSPFEECategoryController(LGSPFEECategoryRepository context)
        {
            _context = context;
            BaseModule = EnumModule.LGSPFEECategory;
            KeyField = "SfcID";
        }
    }
}