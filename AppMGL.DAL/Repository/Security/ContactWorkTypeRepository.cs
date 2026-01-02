using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Security
{
	public class ContactWorkTypeRepository : Repository<LG_CONTACT_WORK_TYPE>, IContactWorkTypeRepository, IRepository<LG_CONTACT_WORK_TYPE>, IDisposable
	{
		public ContactWorkTypeRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new ContactWorkTypeQuery();
		}
	}
}
