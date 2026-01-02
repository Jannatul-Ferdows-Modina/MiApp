using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AppMGL.DAL.Models;
using AppMGL.DAL.Repository.Security;
using AppMGL.DAL.Repository.Setup;
using AppMGL.DAL.UDT;
using AppMGL.DTO.Security;
using AppMGL.DTO.Setup;
using AppMGL.Manager.Infrastructure;
using AutoMapper;

using AppMGL.DAL.Helper;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Linq;
using AppMGL.DAL.Repository.Operation;
using AppMGL.DTO.Operation;

namespace AppMGL.Manager.Areas.Operation.Controllers
{
    public class ContractRateController : BaseController<ContractRateDTO, ContractRateRepository, SIPL_Contract>
    {


        public ContractRateController(ContractRateRepository context)
        {
            _context = context;
            BaseModule = EnumModule.ContractRate;
            KeyField = "ContractID";


        }
        [System.Web.Http.HttpPost]
        public override ActionResult Insert(ContractRateDTO dto)
        {
            try
            {
                var entity = Mapper.Map<SIPL_Contract>(dto);
                _context.Insert(entity);

                _context.UnitOfWork.Commit();

                var result = _context.Detail(GetPrimaryKeyValue(entity));
                var dtoResult = Mapper.Map<ContractRateDTO>(result);

                // dtoResult.Location = Mapper.Map<LocationDTO>(result.LG_LOCATION);

                return AppResult(dtoResult, PrepareMessage(EnumAction.Insert));
            }
            catch (Exception ex)
            {
                return AppResult(ex);
            }
        }

    }
}