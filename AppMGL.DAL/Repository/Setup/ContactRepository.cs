using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Setup
{
	public class ContactRepository : Repository<LG_CONTACT>, IContactRepository, IRepository<LG_CONTACT>, IDisposable
	{
		public ContactRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new ContactQuery();
		}
	}
}
