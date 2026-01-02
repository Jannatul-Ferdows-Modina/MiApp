using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class LoadTypeRepository : Repository<SIPL_LoadType>, ILoadTypeRepository, IRepository<SIPL_LoadType>, IDisposable
	{
		public LoadTypeRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new LoadTypeQuery();
		}
	}
}