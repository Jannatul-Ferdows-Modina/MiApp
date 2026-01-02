using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Setup;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Setup
{
	public class CityRepository : Repository<SIPL_City>, ICityRepository, IRepository<SIPL_City>, IDisposable
	{
		public CityRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new CityQuery();
		}
	}
}
