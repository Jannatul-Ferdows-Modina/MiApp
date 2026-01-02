using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class EnquiryDetailRepository : Repository<USP_GET_ENQUIRY_DETAIL_Result>, IEnquiryDetailRepository, IRepository<USP_GET_ENQUIRY_DETAIL_Result>, IDisposable
	{
		public EnquiryDetailRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new EnquiryDetailQuery();
		}
	}
}
