using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Setup;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Setup
{
	public class CountryRepository : Repository<LG_COUNTRY>, ICountryRepository, IRepository<LG_COUNTRY>, IDisposable
	{
		public CountryRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new CountryQuery();
		}
	}
}
