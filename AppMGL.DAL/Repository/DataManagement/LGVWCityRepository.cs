using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class LGVWChtscodeRepository : Repository<LG_VW_SIPLCity>, ILGVWCityRepository, IRepository<LG_VW_SIPLCity>, IDisposable
	{
		public LGVWChtscodeRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new LGVWCityQuery();
		}
	}
}
