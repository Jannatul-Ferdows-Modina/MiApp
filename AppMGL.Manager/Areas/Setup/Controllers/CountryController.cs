using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.Setup.Controllers
{
    public class CountryController : BaseController<CountryDTO, CountryRepository, LG_COUNTRY>
    {
        #region Constructor

        public CountryController(CountryRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Country;
            KeyField = "CountryId";
        }

        #endregion
    }
}
