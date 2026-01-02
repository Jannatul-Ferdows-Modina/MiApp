using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class SIPLContactController : BaseController<SIPLContactDTO, SIPLContactRepository, SIPL_Contact>
    {
        #region Constructor

        public SIPLContactController(SIPLContactRepository context)
        {
            _context = context;
            BaseModule = EnumModule.SIPLContact;
            KeyField = "ContactID";
        }

        #endregion
    }
}