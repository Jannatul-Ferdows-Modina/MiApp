using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Setup
{
	public class DepartmentRepository : Repository<LG_DEPARTMENT>, IDepartmentRepository, IRepository<LG_DEPARTMENT>, IDisposable
	{
		public DepartmentRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new DepartmentQuery();
		}
	}
}
