using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Operation;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class ContractRateRepository : Repository<SIPL_Contract>, IContractRateRepository, IRepository<SIPL_Contract>, IDisposable
	{
		public ContractRateRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new ContractRateQuery();
		}
	}
}
