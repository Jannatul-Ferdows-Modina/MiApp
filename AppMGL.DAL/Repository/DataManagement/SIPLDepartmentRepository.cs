using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
	public class SIPLDepartmentRepository : Repository<SIPL_Department>, ISIPLDepartmentRepository, IRepository<SIPL_Department>, IDisposable
	{
		public SIPLDepartmentRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new SIPLDepartment();
		}
	}
}
