using AppMGL.DAL.Contract;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;
using AppMGL.DAL.UDT;
using System;

namespace AppMGL.DAL.Repository.DataManagement
{
       public class WarehouseLocationRepository : Repository<sipl_warehouselocation>, IWarehouseLocationRepository, IRepository<sipl_warehouselocation>, IDisposable
      {
        public WarehouseLocationRepository(AppMGL.DAL.Models.AppMGL context)
            : base((IQueryableUnitOfWork)context)
        {
            Query = new WarehouseLocationQuery();
        }
    }
}
