using AppMGL.DAL.UDT;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;

namespace AppMGL.DAL.Repository.DataManagement
{
    public class CompanyGradationRepository : Repository<SIPL_CompanyGradation>, ICompanyGradationRepository
    {
        #region Constructor

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="context">Associated context</param>
        public CompanyGradationRepository(Models.AppMGL context)
            : base(context)
        {
            Query = new CompanyGradationQuery();
        }

        #endregion
    }
}
