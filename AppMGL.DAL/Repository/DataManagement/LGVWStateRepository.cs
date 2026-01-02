using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class LGVWStateRepository : Repository<LG_VW_SIPLState>, ILGVWStateRepository, IRepository<LG_VW_SIPLState>, IDisposable
	{
		public LGVWStateRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new LGVWStateQuery();
		}
	}
}
