using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Security;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Security
{
	public class ModuleRepository : Repository<LG_MODULE>, IModuleRepository, IRepository<LG_MODULE>, IDisposable
	{
		public ModuleRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new ModuleQuery();
		}
	}
}
