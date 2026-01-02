using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class ContinentRepository : Repository<SIPL_Continent>, IContinentRepository, IRepository<SIPL_Continent>, IDisposable
	{
		public ContinentRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new ContinentQuery();
		}
	}
}