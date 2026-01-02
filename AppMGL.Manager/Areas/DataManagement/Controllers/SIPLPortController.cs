using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class SIPLPortController : BaseController<SIPLPortDTO, SIPLPortRepository, SIPL_Port>
    {
        #region Constructor

        public SIPLPortController(SIPLPortRepository context)
        {
            _context = context;
            BaseModule = EnumModule.SIPLPort;
            KeyField = "PortId";
        }

        #endregion
    }
}