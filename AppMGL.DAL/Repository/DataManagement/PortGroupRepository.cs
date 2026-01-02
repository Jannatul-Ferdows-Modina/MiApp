using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class PortGroupRepository : Repository<SIPL_PortGroup>, IPortGroupRepository, IRepository<SIPL_PortGroup>, IDisposable
	{
		public PortGroupRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new PortGroupQuery();
		}
	}
}
