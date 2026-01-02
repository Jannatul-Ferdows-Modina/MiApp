using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Operation;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class ContractRateVWRepository : Repository<LG_VW_Contract>, IContractRateVWRepository, IRepository<LG_VW_Contract>, IDisposable
	{
		public ContractRateVWRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new ContractRateQuery();
		}
	}
}
