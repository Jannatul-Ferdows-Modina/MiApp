using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Security;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Security
{
	public class ModuleTypeRepository : Repository<LG_MODULE_TYPE>, IModuleTypeRepository, IRepository<LG_MODULE_TYPE>, IDisposable
	{
		public ModuleTypeRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new ModuleTypeQuery();
		}
	}
}
