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
	public class SiteRoleRepository : Repository<LG_SITE_CONTACT_ROLE>, ISiteRoleRepository, IRepository<LG_SITE_CONTACT_ROLE>, IDisposable
	{
		public SiteRoleRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SiteRoleQuery();
		}

		public IQueryable<LG_SITE_CONTACT_ROLE> ListRole(Dictionary<string, string> param)
		{
			long sitId = Convert.ToInt64(param["SitId"]);
			return (from x in GetSet()
			where x.SitId == (decimal?)(decimal)sitId
			select x).OrderBy("CntId ASC, RleId ASC");
		}
	}
}
