using AppMGL.DAL.UDT;
using AppMGL.DAL.Models;
using AppMGL.DAL.Modules.DataManagement;


namespace AppMGL.DAL.Repository.DataManagement
{
    public class TradeServiceRepository : Repository<SIPL_TradeService>, ITradeServiceRepository
    {
        public TradeServiceRepository(Models.AppMGL context)
            : base(context)
        {
            Query = new TradeServiceQuery();
        }

    }
}