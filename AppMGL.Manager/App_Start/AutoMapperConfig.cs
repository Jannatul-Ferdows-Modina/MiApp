using AppMGL.DAL.Models;
using AppMGL.DTO.Operation;
using AppMGL.DTO.Security;
using AppMGL.DTO.Setup;
using AppMGL.DTO.Home;
using AppMGL.DTO.DataManagement;
using AppMGL.Manager.Infrastructure.Tasks;
using AutoMapper;

namespace AppMGL.Manager
{
	public class AutoMapperConfig : IRunAtInit
	{
		public void Execute()
		{
            Mapper.Initialize(cfg => {
                cfg.AddProfile<AppProfile>();
            });
        }
	}

    //public class AppProfile : Profile
    //{
    //    public AppProfile()
    //    {
    //        CreateMap<LG_MODULE_TYPE, ModuleTypeDTO>().ReverseMap();
    //        CreateMap<LG_MODULE, ModuleDTO>().ReverseMap();
    //        CreateMap<LG_ACTION, ActionDTO>().ReverseMap();
    //        CreateMap<LG_ROLE, RoleDTO>().ReverseMap();
            
    //        CreateMap<LG_MAM_ROL_MAP, RoleMapDTO>()
    //            .ForMember(d => d.ModCaption, x => x.MapFrom(s => s.LG_MODULE.ModCaption))
    //            .ForMember(d => d.ActCaption, x => x.MapFrom(s => s.LG_ACTION.ActCaption));
    //        CreateMap<RoleMapDTO, LG_MAM_ROL_MAP>();

    //        CreateMap<LG_SITE, SiteDTO>().ReverseMap();
            
    //        //CreateMap<LG_SITE_CONTACT_ROLE, SiteRoleDTO>()
    //        //    .ForMember(d => d.FullName, x => x.MapFrom(s => s.LG_CONTACT.CntFirstName + " " + s.LG_CONTACT.CntLastName))
    //        //    .ForMember(d => d.RleName, x => x.MapFrom(s => s.LG_ROLE.RleName));
    //        //CreateMap<SiteRoleDTO, LG_SITE_CONTACT_ROLE>();

    //        //CreateMap<LG_USER, UserDTO>()
    //        //    .ForMember(d => d.DptName, x => x.MapFrom(s => s.LG_CONTACT.LG_DEPARTMENT.DptName))
    //        //    .ForMember(d => d.FullName, x => x.MapFrom(s => s.LG_CONTACT.CntFirstName + " " + s.LG_CONTACT.CntLastName));
    //        CreateMap<UserDTO, LG_USER>();

    //        CreateMap<LG_COUNTRY, CountryDTO>().ReverseMap();
    //        CreateMap<LG_USSTATE, StateDTO>().ReverseMap();
    //        CreateMap<LG_DEPARTMENT, DepartmentDTO>().ReverseMap();
    //        CreateMap<LG_TITLE, TitleDTO>().ReverseMap();
    //        CreateMap<LG_TIMEZONE, TimezoneDTO>().ReverseMap();

    //        CreateMap<LG_CONTACT, ContactDTO>()
    //            .ForMember(d => d.FullName, x => x.MapFrom(s => s.CntFirstName + " " + s.CntLastName));
    //        CreateMap<ContactDTO, LG_CONTACT>();
    //        CreateMap<LG_CONTACT_WORK_TYPE, ContactWorkTypeDTO>().ReverseMap();

            


    //        CreateMap<LG_LOCATION, LocationDTO>().ReverseMap();

    //        //CreateMap<USP_GET_CUSTOMERCONTACT_DATA_Result, CustomerContactDTO>().ReverseMap();
    //        //CreateMap<USP_GET_ENQUIRY_DATA_Result, EnquiryListDTO>().ReverseMap();
    //        CreateMap<DashboardData_Job, DashboardDTO>().ReverseMap();

    //        CreateMap<AppMGL.DAL.Repository.DataManagement.LG_VW_CONTACTCATEGORY, ContactCategoryDTO>().ReverseMap();

    //        CreateMap<SIPL_Commodity, CommodityDTO>()
    //             .ForMember(d => d.CommodityTypeName, x => x.MapFrom(s => s.SIPL_CommodityType.CommodityType));
    //        CreateMap<CommodityDTO, SIPL_Commodity>();

    //        CreateMap<SIPL_CommodityType, CommodityTypeDTO>().ReverseMap();
    //        CreateMap<SIPL_ContainerType, ContainerTypeDTO>().ReverseMap();
    //        CreateMap<SIPL_LoadType, LoadTypeDTO>().ReverseMap();
    //        CreateMap<SIPL_TradeService, TradeServiceDTO>().ReverseMap();
    //        CreateMap<SIPL_Continent, ContinentDTO>().ReverseMap();
    //        //CreateMap<SIPL_Country, SIPLCountryDTO>()
    //        //.ForMember(d => d.ContinentName, x => x.MapFrom(s => s.SIPL_Continent.Name));
    //        CreateMap<SIPLCountryDTO, SIPL_Country>();

    //        CreateMap<LG_VW_SIPLState, LGVWStateDTO>().ReverseMap();
    //        CreateMap<LG_VW_SIPLCity, LGVWCityDTO>().ReverseMap();
    //        CreateMap<LG_VW_Port, LGVWPortDTO>().ReverseMap();
    //        //CreateMap<LG_VW_Alias, LGVWAliasDTO>().ReverseMap();
    //        CreateMap<SIPL_RailRamp, RailRampDTO>().ReverseMap();
    //        CreateMap<Sipl_Terminal , TerminalDTO>().ReverseMap();
    //        //CreateMap<LG_VW_SurchargeGroup, SurchargeGroupDTO>().ReverseMap();
    //        CreateMap<SIPL_PortGroup, PortGroupDTO>().ReverseMap();
    //        CreateMap<LG_ACCT_CATEGORY, LGACCTCategoryDTO>().ReverseMap();
    //        CreateMap<LG_SP_FEE_CATEGORY, LGSPFEECategoryDTO>().ReverseMap();



    //        CreateMap<SIPL_Port, SIPLPortDTO>().ReverseMap();
    //        CreateMap<SIPL_State, SIPLStateDTO>().ReverseMap();
    //        CreateMap<SIPL_City, SIPLCityDTO>().ReverseMap();
    //        CreateMap<SIPL_Department, SIPLDepartmentDTO>().ReverseMap();
    //        CreateMap<SIPL_Contact, SIPLContactDTO>().ReverseMap();
    //        CreateMap<LG_VW_SITE_CONTACT, SIPLUserDTO>().ReverseMap();
    //        CreateMap<SIPL_CompanyGradation, CompanyGradationDTO>().ReverseMap();
    //        CreateMap<SIPL_BookingStatus, SIPLBookingStatusDTO>().ReverseMap();

    //        CreateMap<LG_VW_Contract, LGVWContractRateDTO>().ReverseMap();
    //        CreateMap<LG_VW_SIPL_CONTACT, LGVWSIPLContactDTO >().ReverseMap();
    //        CreateMap<LG_VW_DisplayRate, LGVWDisplayRateDTO>().ReverseMap();

    //    }
    //}
    public class AppProfile : Profile
    {

    public AppProfile()
		{
			CreateMap<LG_MODULE_TYPE, ModuleTypeDTO>().ReverseMap();
			CreateMap<LG_MODULE, ModuleDTO>().ReverseMap();
			CreateMap<LG_ACTION, ActionDTO>().ReverseMap();
			CreateMap<LG_ROLE, RoleDTO>().ReverseMap();
			CreateMap<LG_MAM_ROL_MAP, RoleMapDTO>().ForMember((RoleMapDTO d) => d.ModCaption, delegate(IMemberConfigurationExpression<LG_MAM_ROL_MAP, RoleMapDTO, string> x)
			{
				x.MapFrom((LG_MAM_ROL_MAP s) => s.LG_MODULE.ModCaption);
			}).ForMember((RoleMapDTO d) => d.ActCaption, delegate(IMemberConfigurationExpression<LG_MAM_ROL_MAP, RoleMapDTO, string> x)
			{
				x.MapFrom((LG_MAM_ROL_MAP s) => s.LG_ACTION.ActCaption);
			});
			CreateMap<RoleMapDTO, LG_MAM_ROL_MAP>();
			CreateMap<LG_SITE, SiteDTO>().ReverseMap();
			CreateMap<LG_SITE_CONTACT_ROLE, SiteRoleDTO>().ForMember((SiteRoleDTO d) => d.FullName, delegate(IMemberConfigurationExpression<LG_SITE_CONTACT_ROLE, SiteRoleDTO, string> x)
			{
				x.MapFrom((LG_SITE_CONTACT_ROLE s) => s.LG_CONTACT.CntFirstName + " " + s.LG_CONTACT.CntLastName);
			}).ForMember((SiteRoleDTO d) => d.RleName, delegate(IMemberConfigurationExpression<LG_SITE_CONTACT_ROLE, SiteRoleDTO, string> x)
			{
				x.MapFrom((LG_SITE_CONTACT_ROLE s) => s.LG_ROLE.RleName);
			});
			CreateMap<SiteRoleDTO, LG_SITE_CONTACT_ROLE>();
			CreateMap<LG_USER, UserDTO>().ForMember((UserDTO d) => d.DptName, delegate(IMemberConfigurationExpression<LG_USER, UserDTO, string> x)
			{
				x.MapFrom((LG_USER s) => s.LG_CONTACT.LG_DEPARTMENT.DptName);
			}).ForMember((UserDTO d) => d.FullName, delegate(IMemberConfigurationExpression<LG_USER, UserDTO, string> x)
			{
				x.MapFrom((LG_USER s) => s.LG_CONTACT.CntFirstName + " " + s.LG_CONTACT.CntLastName);
			});
			CreateMap<UserDTO, LG_USER>();
			CreateMap<LG_COUNTRY, CountryDTO>().ReverseMap();
			CreateMap<LG_USSTATE, StateDTO>().ReverseMap();
			CreateMap<LG_DEPARTMENT, DepartmentDTO>().ReverseMap();
			CreateMap<LG_TITLE, TitleDTO>().ReverseMap();
			CreateMap<LG_TIMEZONE, TimezoneDTO>().ReverseMap();
			CreateMap<LG_CONTACT, ContactDTO>().ForMember((ContactDTO d) => d.FullName, delegate(IMemberConfigurationExpression<LG_CONTACT, ContactDTO, string> x)
			{
				x.MapFrom((LG_CONTACT s) => s.CntFirstName + " " + s.CntLastName);
			});
			CreateMap<ContactDTO, LG_CONTACT>();
			CreateMap<LG_CONTACT_WORK_TYPE, ContactWorkTypeDTO>().ReverseMap();
			CreateMap<LG_LOCATION, LocationDTO>().ReverseMap();
          //  CreateMap<USP_GET_CUSTOMERCONTACT_DATA_Result, CustomerContactDTO>().ReverseMap();
            //CreateMap<USP_GET_ENQUIRY_DATA_Result, EnquiryListDTO>().ReverseMap();
			CreateMap<DashboardData_Job, DashboardDTO>().ReverseMap();
			//CreateMap<LG_VW_CONTACTCATEGORY, ContactCategoryDTO>().ReverseMap();
			CreateMap<SIPL_Commodity, CommodityDTO>().ForMember((CommodityDTO d) => d.CommodityTypeName, delegate(IMemberConfigurationExpression<SIPL_Commodity, CommodityDTO, string> x)
			{
				x.MapFrom((SIPL_Commodity s) => s.SIPL_CommodityType.CommodityType);
			});
			CreateMap<CommodityDTO, SIPL_Commodity>();
			CreateMap<SIPL_CommodityType, CommodityTypeDTO>().ReverseMap();
			CreateMap<SIPL_ContainerType, ContainerTypeDTO>().ReverseMap();
			CreateMap<SIPL_LoadType, LoadTypeDTO>().ReverseMap();
			CreateMap<SIPL_TradeService, TradeServiceDTO>().ReverseMap();
			CreateMap<SIPL_Continent, ContinentDTO>().ReverseMap();
			CreateMap<SIPL_Country, SIPLCountryDTO>().ForMember((SIPLCountryDTO d) => d.ContinentName, delegate(IMemberConfigurationExpression<SIPL_Country, SIPLCountryDTO, string> x)
			{
				x.MapFrom((SIPL_Country s) => s.SIPL_Continent.Name);
			});
			CreateMap<SIPLCountryDTO, SIPL_Country>();
			CreateMap<LG_VW_SIPLState, LGVWStateDTO>().ReverseMap();
			CreateMap<LG_VW_SIPLCity, LGVWCityDTO>().ReverseMap();
			CreateMap<LG_VW_Port, LGVWPortDTO>().ReverseMap();
			CreateMap<LG_VW_Alias, LGVWAliasDTO>().ReverseMap();
			CreateMap<SIPL_RailRamp, RailRampDTO>().ReverseMap();
			CreateMap<Sipl_Terminal, TerminalDTO>().ReverseMap();
			CreateMap<LG_VW_SurchargeGroup, SurchargeGroupDTO>().ReverseMap();
			CreateMap<SIPL_PortGroup, PortGroupDTO>().ReverseMap();
			CreateMap<LG_ACCT_CATEGORY, LGACCTCategoryDTO>().ReverseMap();
			CreateMap<LG_SP_FEE_CATEGORY, LGSPFEECategoryDTO>().ReverseMap();
			CreateMap<SIPL_Port, SIPLPortDTO>().ReverseMap();
			CreateMap<SIPL_State, SIPLStateDTO>().ReverseMap();
			CreateMap<SIPL_City, SIPLCityDTO>().ReverseMap();
			CreateMap<SIPL_Department, SIPLDepartmentDTO>().ReverseMap();
			CreateMap<SIPL_Contact, SIPLContactDTO>().ReverseMap();
			CreateMap<LG_VW_SITE_CONTACT, SIPLUserDTO>().ReverseMap();
			CreateMap<SIPL_CompanyGradation, CompanyGradationDTO>().ReverseMap();
			CreateMap<SIPL_BookingStatus, SIPLBookingStatusDTO>().ReverseMap();
			CreateMap<LG_VW_Contract, LGVWContractRateDTO>().ReverseMap();
			CreateMap<LG_VW_SIPL_CONTACT, LGVWSIPLContactDTO>().ReverseMap();
			CreateMap<LG_VW_DisplayRate, LGVWDisplayRateDTO>().ReverseMap();
		}
    }
}