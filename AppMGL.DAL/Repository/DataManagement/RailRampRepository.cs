using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class RailRampRepository : Repository<SIPL_RailRamp>, IRailRampRepository, IRepository<SIPL_RailRamp>, IDisposable
	{
		public RailRampRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new RailRampQuery();
		}
	}
}