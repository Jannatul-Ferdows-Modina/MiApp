using AppMGL.DAL.UDT;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.Setup;

namespace AppMGL.DAL.Repository.Setup
{
    public class LocationRepository : Repository<LG_LOCATION>, ILocationRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="context">Associated context</param>
        public LocationRepository(Models.AppMGL context)
            : base(context)
        {
            Query = new LocationQuery();
        }

        #endregion
    }
}
