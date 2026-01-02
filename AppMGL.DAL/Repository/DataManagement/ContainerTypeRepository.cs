using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class ContainerTypeRepository : Repository<SIPL_ContainerType>, IContainerTypeRepository, IRepository<SIPL_ContainerType>, IDisposable
	{
		public ContainerTypeRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new CommodityQuery();
		}
	}
}