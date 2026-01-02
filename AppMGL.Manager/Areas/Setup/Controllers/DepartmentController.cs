using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.Setup.Controllers
{
    public class DepartmentController : BaseController<DepartmentDTO, DepartmentRepository, LG_DEPARTMENT>
    {
        #region Constructor

        public DepartmentController(DepartmentRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Department;
            KeyField = "DepartmentId";
        }

        #endregion
    }
}
