using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class TerminalRepository : Repository<Sipl_Terminal>, ITerminalRepository, IRepository<Sipl_Terminal>, IDisposable
	{
		public TerminalRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new TerminalQuery();
		}
	}
}