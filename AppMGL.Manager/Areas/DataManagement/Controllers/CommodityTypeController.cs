using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class CommodityTypeController : BaseController<CommodityTypeDTO, CommodityTypeRepository, SIPL_CommodityType>
    {


        public CommodityTypeController(CommodityTypeRepository context)
        {
            _context = context;
            BaseModule = EnumModule.CommodityType;
            KeyField = "CommodityTypeID";
        }
    }
}