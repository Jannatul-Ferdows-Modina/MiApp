using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class LGSPFEECategoryRepository : Repository<LG_SP_FEE_CATEGORY>, ILGSPFEECategoryRepository, IRepository<LG_SP_FEE_CATEGORY>, IDisposable
	{
		public LGSPFEECategoryRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new LGSPFEECategoryQuery();
		}
	}
}