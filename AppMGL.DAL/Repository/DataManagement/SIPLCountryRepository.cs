using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class SIPLCountryRepository : Repository<SIPL_Country>, ISIPLCountryRepository, IRepository<SIPL_Country>, IDisposable
	{
		public SIPLCountryRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SIPLCountryQuery();
		}
	}
}
