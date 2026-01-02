using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class TradeServiceController : BaseController<TradeServiceDTO, TradeServiceRepository, SIPL_TradeService>
    {


        public TradeServiceController(TradeServiceRepository context)
        {
            _context = context;
            BaseModule = EnumModule.TradeService;
            KeyField = "TradeServiceId";
        }
    }
}