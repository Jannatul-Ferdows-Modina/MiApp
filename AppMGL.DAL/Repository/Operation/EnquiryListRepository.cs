using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class EnquiryListRepository : Repository<USP_GET_ENQUIRY_LIST_Result>, IEnquiryListRepository, IRepository<USP_GET_ENQUIRY_LIST_Result>, IDisposable
	{
		public EnquiryListRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new EnquiryListQuery();
		}
	}
}
