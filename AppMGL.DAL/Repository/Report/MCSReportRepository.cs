using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Report
{
	public class MCSReportRepository : Repository<SIPL_DocumentCommon>, IMCSRepository, IRepository<SIPL_DocumentCommon>, IDisposable
	{
		public MCSReportRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new MCSReportQuery();
		}
	}
}
