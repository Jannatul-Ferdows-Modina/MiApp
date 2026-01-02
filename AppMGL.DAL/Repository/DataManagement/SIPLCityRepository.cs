using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class SIPLCityRepository : Repository<SIPL_City>, ISIPLCityRepository, IRepository<SIPL_City>, IDisposable
	{
		public SIPLCityRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SIPLCityQuery();
		}
	}
}
