using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Report
{
	public class QuotationReportRepository : Repository<RPT_GET_QUOTATION_Result>, IQuotationReportRepository, IRepository<RPT_GET_QUOTATION_Result>, IDisposable
	{
		public QuotationReportRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new QuotationReportQuery();
		}
	}
}
