using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class ContinentController : BaseController<ContinentDTO, ContinentRepository, SIPL_Continent>
    {


        public ContinentController(ContinentRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Continent;
            KeyField = "ContinentId";
        }
    }
}