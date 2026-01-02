using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Operation;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class CustomerContactDetailRepository : Repository<USP_GET_CUSTOMERCONTACT_DETAIL_Result>, ICustomerConcatDetailRepository, IRepository<USP_GET_CUSTOMERCONTACT_DETAIL_Result>, IDisposable
	{
		public CustomerContactDetailRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new EnquiryDetailQuery();
		}
	}
}
