using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class DisplayRateRepository : Repository<LG_VW_DisplayRate>, IDisplayRateRepository, IRepository<LG_VW_DisplayRate>, IDisposable
	{
		public DisplayRateRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new DisplayRateQuery();
		}
	}
}
