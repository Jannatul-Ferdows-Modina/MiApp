using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class SIPLContactRepository : Repository<SIPL_Contact>, ISIPLContactRepository, IRepository<SIPL_Contact>, IDisposable
	{
		public SIPLContactRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SIPLContactQuery();
		}
	}
}
