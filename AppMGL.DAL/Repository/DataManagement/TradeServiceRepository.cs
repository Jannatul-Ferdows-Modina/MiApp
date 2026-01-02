using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class TradeServiceRepository : Repository<SIPL_TradeService>, ITradeServiceRepository, IRepository<SIPL_TradeService>, IDisposable
	{
		public TradeServiceRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new TradeServiceQuery();
		}
	}
}
