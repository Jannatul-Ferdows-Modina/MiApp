using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class RailRampController : BaseController<RailRampDTO, RailRampRepository, SIPL_RailRamp>
    {


        public RailRampController(RailRampRepository context)
        {
            _context = context;
            BaseModule = EnumModule.RailRamp;
            KeyField = "railId";
        }
    }
}