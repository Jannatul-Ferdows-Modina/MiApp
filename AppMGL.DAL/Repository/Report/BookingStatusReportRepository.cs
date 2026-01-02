using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Report
{
	public class BookingStatusReportRepository : Repository<RPT_GET_BOOKING_STATUS_Result>, IBookingStatusReportRepository, IRepository<RPT_GET_BOOKING_STATUS_Result>, IDisposable
	{
		public BookingStatusReportRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new BookingStatusReportQuery();
		}
	}
}
