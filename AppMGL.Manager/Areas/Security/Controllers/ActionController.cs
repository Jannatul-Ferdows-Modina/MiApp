using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.Security.Controllers
{
    public class ActionController : BaseController<ActionDTO, ActionRepository, LG_ACTION>
    {
        #region Constructor

        public ActionController(ActionRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Action;
            KeyField = "ActId";
        }

        #endregion
    }
}
