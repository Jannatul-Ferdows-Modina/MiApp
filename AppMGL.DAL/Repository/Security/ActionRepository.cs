using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Security;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Security
{
	public class ActionRepository : Repository<LG_ACTION>, IActionRepository, IRepository<LG_ACTION>, IDisposable
	{
		public ActionRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new ActionQuery();
		}
	}
}
