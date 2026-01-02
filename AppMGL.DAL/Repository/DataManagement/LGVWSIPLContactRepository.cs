using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class LGVWSIPLContactRepository : Repository<LG_VW_SIPL_CONTACT>, ILG_VW_SIPL_CONTACT, IRepository<LG_VW_SIPL_CONTACT>, IDisposable
	{
		public LGVWSIPLContactRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new LGVWSIPLContactQuery();
		}
	}
}