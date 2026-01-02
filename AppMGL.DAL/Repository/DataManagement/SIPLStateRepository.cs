using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class SIPLStateRepository : Repository<SIPL_State>, ISIPLStateRepository, IRepository<SIPL_State>, IDisposable
	{
		public SIPLStateRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SIPLStateQuery();
		}
	}
}
