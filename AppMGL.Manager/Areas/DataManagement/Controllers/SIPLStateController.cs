using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class SIPLStateController : BaseController<SIPLStateDTO, SIPLStateRepository, SIPL_State>
    {
        #region Constructor

        public SIPLStateController(SIPLStateRepository context)
        {
            _context = context;
            BaseModule = EnumModule.SIPLState;
            KeyField = "StateId";
        }

        #endregion
    }
}