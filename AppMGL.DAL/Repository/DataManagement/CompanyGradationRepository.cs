using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class CompanyGradationRepository : Repository<SIPL_CompanyGradation>, ICompanyGradationRepository, IRepository<SIPL_CompanyGradation>, IDisposable
	{
		public CompanyGradationRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new CompanyGradationQuery();
		}
	}
}
