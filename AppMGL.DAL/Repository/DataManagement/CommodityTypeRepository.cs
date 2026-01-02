using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class CommodityTypeRepository : Repository<SIPL_CommodityType>, ICommodityTypeRepository, IRepository<SIPL_CommodityType>, IDisposable
	{
		public CommodityTypeRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new CommodityTypeQuery();
		}
	}
}