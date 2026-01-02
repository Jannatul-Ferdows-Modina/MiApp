using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class LoadTypeController : BaseController<LoadTypeDTO, LoadTypeRepository, SIPL_LoadType>
    {


        public LoadTypeController(LoadTypeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.LoadType;
            KeyField = "LoadTypeId";
        }
    }
}