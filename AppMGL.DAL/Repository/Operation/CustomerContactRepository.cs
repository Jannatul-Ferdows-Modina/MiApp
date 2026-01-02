using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Operation;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class CustomerContactRepository : Repository<USP_GET_CUSTOMERCONTACT_LIST_Result>, ICustomerContactRepository, IRepository<USP_GET_CUSTOMERCONTACT_LIST_Result>, IDisposable
	{
		public CustomerContactRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new CustomerContactQuery();
		}
	}
}
