using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.Setup.Controllers
{
    public class StateController : BaseController<StateDTO, StateRepository, LG_USSTATE>
    {
        #region Constructor

        public StateController(StateRepository context)
        {
            _context = context;
            BaseModule = EnumModule.State;
            KeyField = "StateId";
        }

        #endregion
    }
}
