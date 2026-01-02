using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.DataManagement;
using AppMGL.DAL.UDT;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure;

namespace AppMGL.Manager.Areas.DataManagement.Controllers
{
    public class TerminalController : BaseController<TerminalDTO, TerminalRepository, Sipl_Terminal>
    {


        public TerminalController(TerminalRepository context)
        {
            _context = context;
            BaseModule = EnumModule.Terminal;
            KeyField = "terminalId";
        }
    }
}