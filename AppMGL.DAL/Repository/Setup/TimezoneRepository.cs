using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Setup;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Setup
{
	public class TimezoneRepository : Repository<LG_TIMEZONE>, ITimezoneRepository, IRepository<LG_TIMEZONE>, IDisposable
	{
		public TimezoneRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new TimezoneQuery();
		}
	}
}
