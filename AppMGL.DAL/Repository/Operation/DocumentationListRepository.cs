using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Operation
{
	public class DocumentationListRepository : Repository<USP_GET_ENQUIRY_LIST_Result>, IDocumentationListRepository, IRepository<USP_GET_ENQUIRY_LIST_Result>, IDisposable
	{
		public DocumentationListRepository(AppMGL.DAL.Models.AppMGL context)
			: base((IQueryableUnitOfWork)context)
		{
			Query = new DocumentationListQuery();
		}
	}
}
