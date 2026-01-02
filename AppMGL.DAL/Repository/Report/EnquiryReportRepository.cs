using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Report
{
	public class EnquiryReportRepository : Repository<RPT_GET_ENQUIRY_Result>, IEnquiryReportRepository, IRepository<RPT_GET_ENQUIRY_Result>, IDisposable
	{
		public EnquiryReportRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new EnquiryReportQuery();
		}
	}
}
