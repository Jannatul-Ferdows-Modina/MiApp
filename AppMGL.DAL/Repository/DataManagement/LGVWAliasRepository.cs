using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class LGVWAliasRepository : Repository<LG_VW_Alias>, ILGVWAliasRepository, IRepository<LG_VW_Alias>, IDisposable
	{
		public LGVWAliasRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new LGVWAliasQuery();
		}
	}
}
