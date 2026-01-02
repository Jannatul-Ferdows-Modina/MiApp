using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class PortGroupController : BaseController<PortGroupDTO, PortGroupRepository, SIPL_PortGroup>
    {
        #region Constructor

        public PortGroupController(PortGroupRepository context)
        {
            _context = context;
            BaseModule = EnumModule.PortGroup;
            KeyField = "PortGroupId";
        }

        #endregion
    }
}