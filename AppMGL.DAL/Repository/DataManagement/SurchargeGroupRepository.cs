using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class SurchargeGroupRepository : Repository<LG_VW_SurchargeGroup>, ISurchargeGroupRepository, IRepository<LG_VW_SurchargeGroup>, IDisposable
	{
		public SurchargeGroupRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SurchargeGroupQuery();
		}
	}
}