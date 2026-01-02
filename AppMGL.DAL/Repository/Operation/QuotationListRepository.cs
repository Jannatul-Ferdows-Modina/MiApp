using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class QuotationListRepository : Repository<USP_GET_ENQUIRY_LIST_Result>, IQuotationListRepository, IRepository<USP_GET_ENQUIRY_LIST_Result>, IDisposable
	{
		public QuotationListRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new QuotationListQuery();
		}
	}
}
