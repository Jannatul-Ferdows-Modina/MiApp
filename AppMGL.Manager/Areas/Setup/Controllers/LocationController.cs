using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.Setup.Controllers
{
    public class LocationController : BaseController<LocationDTO, LocationRepository, LG_LOCATION>
    {
        #region Constructor

        public LocationController(LocationRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Location;
            KeyField = "LocationId";
        }

        #endregion
    }
}
