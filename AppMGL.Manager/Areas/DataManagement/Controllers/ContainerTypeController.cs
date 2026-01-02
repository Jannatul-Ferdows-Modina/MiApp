using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class ContainerTypeController : BaseController<ContainerTypeDTO, ContainerTypeRepository, SIPL_ContainerType>
    {
        

        public ContainerTypeController(ContainerTypeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.ContainerType;
            KeyField = "ContainerTypeId";
        }
    }
}