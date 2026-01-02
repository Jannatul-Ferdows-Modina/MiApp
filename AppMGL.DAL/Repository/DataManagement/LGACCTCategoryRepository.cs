using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class LGACCTCategoryRepository : Repository<LG_ACCT_CATEGORY>, ILGACCTCategoryRepository, IRepository<LG_ACCT_CATEGORY>, IDisposable
	{
		public LGACCTCategoryRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new LGACCTCategoryQuery();
		}
	}
}