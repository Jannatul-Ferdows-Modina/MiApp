using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;
namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class SIPLCityController : BaseController<SIPLCityDTO, SIPLCityRepository, SIPL_City>
    {
        #region Constructor

        public SIPLCityController(SIPLCityRepository context)
        {
            _context = context;
            BaseModule = EnumModule.SIPLCity;
            KeyField = "CityId";
        }

        #endregion
    }
}