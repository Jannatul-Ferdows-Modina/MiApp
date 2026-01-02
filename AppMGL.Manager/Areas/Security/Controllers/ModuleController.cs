using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.Security.Controllers
{
    public class ModuleController : BaseController<ModuleDTO, ModuleRepository, LG_MODULE>
    {
        #region Constructor

        public ModuleController(ModuleRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Module;
            KeyField = "ModId";
        }

        #endregion
    }
}
