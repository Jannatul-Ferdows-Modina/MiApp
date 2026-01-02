using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Operation;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class ContractRateModalRepository : Repository<SIPL_Contract>, IContractRateModalRepository, IRepository<SIPL_Contract>, IDisposable
	{
		public ContractRateModalRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new ContractRateModalQuery();
		}
	}
}
