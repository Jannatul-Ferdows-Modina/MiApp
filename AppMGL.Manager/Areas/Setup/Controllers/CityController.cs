using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;


namespace AppMGL.Manager.Areas.Setup.Controllers
{
    public class CityController : BaseController<CityDTO, CityRepository, SIPL_City>
    {
        #region Constructor

        public CityController(CityRepository context)
        {
            _context = context;
            BaseModule = EnumModule.City;
            KeyField = "CityId";
        }

        #endregion
    }
}

