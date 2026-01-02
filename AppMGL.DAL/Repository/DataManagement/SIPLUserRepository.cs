using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class SIPLUserRepository : Repository<LG_VW_SITE_CONTACT>, ISIPLUserRepository, IRepository<LG_VW_SITE_CONTACT>, IDisposable
	{
		public SIPLUserRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SIPLUserQuery();
		}
	}
}
