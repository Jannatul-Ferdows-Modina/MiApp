using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class CommodityRepository : Repository<SIPL_Commodity>, ICommodityRepository, IRepository<SIPL_Commodity>, IDisposable
	{
		public CommodityRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new CommodityQuery();
		}
	}
}