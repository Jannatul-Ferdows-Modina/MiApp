using AppMGL.DAL.Models;
using AppMGL.DAL.UDT;


namespace AppMGL.DAL.Repository.Document
{
    public class LGShipMentDocRepository : Repository<LG_SHIPMENT_DOCS>, ILGShipMentDocRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="context">Associated context</param>
        public LGShipMentDocRepository(Models.AppMGL contextLGShipDoc)
            : base(contextLGShipDoc)
        {
            Query = new DocumentCommonQuery();
        }

        #endregion
    }
}
