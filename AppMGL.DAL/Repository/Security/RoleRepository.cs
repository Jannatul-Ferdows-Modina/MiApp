using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Security;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Security
{
	public class RoleRepository : Repository<LG_ROLE>, IRoleRepository, IRepository<LG_ROLE>, IDisposable
	{
		public RoleRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new RoleQuery();
		}
	}
}
