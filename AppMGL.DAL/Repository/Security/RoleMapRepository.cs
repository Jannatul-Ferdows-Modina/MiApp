using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Security;
using AppMGL.DAL.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace AppMGL.DAL.Repository.Security
{
	public class RoleMapRepository : Repository<LG_MAM_ROL_MAP>, IRoleMapRepository, IRepository<LG_MAM_ROL_MAP>, IDisposable
	{
		public RoleMapRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new RoleMapQuery();
		}

		public IQueryable<LG_MAM_ROL_MAP> ListRoleMap(Dictionary<string, string> param)
		{
			long rleId = Convert.ToInt64(param["RleId"]);
			return (from x in GetSet()
			where x.RleId == (decimal)rleId
			select x).OrderBy("ModId ASC, ActId ASC");
		}
	}
}
