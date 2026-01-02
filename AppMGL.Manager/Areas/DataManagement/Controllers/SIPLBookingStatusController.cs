using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class SIPLBookingStatusController : BaseController<SIPLBookingStatusDTO, SIPLBookingStatusRepository, SIPL_BookingStatus>
    {
        #region Constructor

        public SIPLBookingStatusController(SIPLBookingStatusRepository context)
        {
            _context = context;
            BaseModule = EnumModule.SIPLBookingStatus;
            KeyField = "StatusId";
        }

        #endregion
    }
}