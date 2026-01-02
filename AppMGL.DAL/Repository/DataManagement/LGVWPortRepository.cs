using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class LGVWPortRepository : Repository<LG_VW_Port>, ILGVWPortRepository, IRepository<LG_VW_Port>, IDisposable
	{
		public LGVWPortRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new LGVWPortQuery();
		}
	}
}