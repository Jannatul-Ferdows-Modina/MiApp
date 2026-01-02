using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class SIPLBookingStatusRepository : Repository<SIPL_BookingStatus>, ISIPLBookingStatusRepository, IRepository<SIPL_BookingStatus>, IDisposable
	{
		public SIPLBookingStatusRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SIPLBookingStatusQuery();
		}
	}
}
