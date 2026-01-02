using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class LGVWSIPLContactController : BaseController<LGVWSIPLContactDTO, LGVWSIPLContactRepository, LG_VW_SIPL_CONTACT>
    {
         
        public LGVWSIPLContactController(LGVWSIPLContactRepository context)
        {
            _context = context;
            BaseModule = EnumModule.LGVWSIPLContact;
            KeyField = "ContactID";
        }

    }
}