using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.Document
{
	public class LGShipMentDocRepository : Repository<LG_SHIPMENT_DOCS>, ILGShipMentDocRepository, IRepository<LG_SHIPMENT_DOCS>, IDisposable
	{
		public LGShipMentDocRepository(AppMGL.DAL.Models.AppMGL contextLGShipDoc)
			: base((IQueryableUnitOfWork)contextLGShipDoc)
		{
			Query = new DocumentCommonQuery();
		}
	}
}
