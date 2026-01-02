using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Setup;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Setup
{
	public class TitleRepository : Repository<LG_TITLE>, ITitleRepository, IRepository<LG_TITLE>, IDisposable
	{
		public TitleRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new TitleQuery();
		}
	}
}
