using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.Setup.Controllers
{
    public class TimezoneController : BaseController<TimezoneDTO, TimezoneRepository, LG_TIMEZONE>
    {
        #region Constructor

        public TimezoneController(TimezoneRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Timezone;
            KeyField = "TimezoneId";
        }

        #endregion
    }
}
