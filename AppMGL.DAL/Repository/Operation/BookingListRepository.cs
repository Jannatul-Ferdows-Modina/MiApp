using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class BookingListRepository : Repository<USP_GET_ENQUIRY_LIST_Result>, IBookingListRepository, IRepository<USP_GET_ENQUIRY_LIST_Result>, IDisposable
	{
		public BookingListRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new BookingListQuery();
		}
	}
}
