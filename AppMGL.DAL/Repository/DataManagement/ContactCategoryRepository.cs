using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class ContactCategoryRepository : Repository<LG_VW_CONTACTCATEGORY>, IContactCategoryRepository, IRepository<LG_VW_CONTACTCATEGORY>, IDisposable
	{
		public ContactCategoryRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new ContactCategoryQuery();
		}
	}
}
