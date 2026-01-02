using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.Security.Controllers
{
    public class ModuleTypeController : BaseController<ModuleTypeDTO, ModuleTypeRepository, LG_MODULE_TYPE>
    {
        #region Constructor

        public ModuleTypeController(ModuleTypeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.ModuleType;
            KeyField = "MteId";
        }

        #endregion
    }
}
