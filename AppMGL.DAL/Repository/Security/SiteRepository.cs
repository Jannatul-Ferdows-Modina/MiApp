using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Security;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Security
{
	public class SiteRepository : Repository<LG_SITE>, ISiteRepository, IRepository<LG_SITE>, IDisposable
	{
		public SiteRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SiteQuery();
		}

		public override LG_SITE Update(LG_SITE item)
		{
			if (item != null)
			{
				_unitOfWork.SetModified(item);
				_unitOfWork.SetModified(item.LG_LOCATION);
			}
			return null;
		}
	}
}
