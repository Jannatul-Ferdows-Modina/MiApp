using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class SIPLDepartmentController : BaseController<SIPLDepartmentDTO, SIPLDepartmentRepository, SIPL_Department>
    {
        #region Constructor

        public SIPLDepartmentController(SIPLDepartmentRepository context)
        {
            _context = context;
            BaseModule = EnumModule.SIPLDepartment;
            KeyField = "DepartmentID";
        }

        #endregion
    }
}