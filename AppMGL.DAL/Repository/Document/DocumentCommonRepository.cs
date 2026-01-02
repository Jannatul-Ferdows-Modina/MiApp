using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Document
{
	public class DocumentCommonRepository : Repository<SIPL_DocumentCommon>, IDocumentCommonRepository, IRepository<SIPL_DocumentCommon>, IDisposable
	{
		public DocumentCommonRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new DocumentCommonQuery();
		}
	}
}
