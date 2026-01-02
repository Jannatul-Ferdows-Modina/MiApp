using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class SIPLPortRepository : Repository<SIPL_Port>, ISIPLPortRepository, IRepository<SIPL_Port>, IDisposable
	{
		public SIPLPortRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SIPLPortQuery();
		}
	}
}
