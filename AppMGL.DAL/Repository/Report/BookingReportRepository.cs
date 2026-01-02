using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Report
{
	public class BookingReportRepository : Repository<RPT_GET_BOOKING_Result>, IBookingReportRepository, IRepository<RPT_GET_BOOKING_Result>, IDisposable
	{
		public BookingReportRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new BookingReportQuery();
		}
	}
}
