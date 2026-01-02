using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Setup;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Setup
{
	public class StateRepository : Repository<LG_USSTATE>, IStateRepository, IRepository<LG_USSTATE>, IDisposable
	{
		public StateRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new StateQuery();
		}
	}
}
