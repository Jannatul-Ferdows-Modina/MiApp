using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Home;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Home
{
	public class DashboardRepository : Repository<DashboardData_Job>, IDashboardRepository, IRepository<DashboardData_Job>, IDisposable
	{
		public DashboardRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new DashboardQuery();
		}
	}
}
