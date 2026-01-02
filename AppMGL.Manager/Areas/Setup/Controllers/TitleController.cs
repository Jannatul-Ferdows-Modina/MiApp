using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.Setup.Controllers
{
    public class TitleController : BaseController<TitleDTO, TitleRepository, LG_TITLE>
    {
        #region Constructor

        public TitleController(TitleRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Title;
            KeyField = "TitleId";
        }

        #endregion
    }
}
