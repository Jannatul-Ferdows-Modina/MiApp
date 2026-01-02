using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class CompanyGradationController : BaseController<CompanyGradationDTO, CompanyGradationRepository, SIPL_CompanyGradation>
    {
        #region Constructor

        public CompanyGradationController(CompanyGradationRepository context)
        {
            _context = context;
            BaseModule = EnumModule.CompanyGradation;
            KeyField = "CompanyGradationId";
        }

        #endregion
    }
}

