namespace AppMGL.DAL.UDT
{
    public enum EnumModule
    {
        // Security (101 - 199)
        ModuleType = 101,
        Module = 102,
        Action = 103,
        Role = 105,
        RoleMap = 104,
        Site = 106,
        SiteRole = 107,
        User = 108,

        // Setup (201 - 289)
        Country = 201,
        State = 202,
        Department = 203,
        Title = 204,
        Timezone = 205,
        Contact = 206,
        Location = 207,
        CustomerContact = 208,
        Dashboard = 209,        
        CompanyGradation = 211,
        UserJobRole = 526,
        // Operation
        Enquiry = 210,
        Quotation = 212,
        Booking = 213,
        Documentation = 214,
        QuickBook = 215,
        ContractRate = 288,
        DisplayRate = 250,
        ContractRateVW = 288,
        ContractRateModal = 289,
        // Operation


        // DataManagement(290-350)
        ContactCategory = 290,
        Commodity = 291,
        ContainerType = 292,
        LoadType = 293,
        TradeService = 294,
        CommodityType = 295,
        Continent = 296,
        SIPLCountry = 297,
        LGVWState = 298,
        LGVWCity = 299,
        RailRamp = 300,
        Terminal = 301,
        LGVWPort = 302,
        SurchargeGroup = 303,
        PortGroup = 304,
        LGVWAlias = 305,
        LGACCTCategory = 306,
        LGSPFEECategory = 307,
        LGVWSIPLContact = 308,
        LGVWHtsCode = 518,
        LGVWlienseexemption=519,
        GVWModeofTransport = 520,
        FreightForwarderNetwork=522,
        FreightForwarderMapping=523,
        DDTCITAR = 524,
        DDTCUSML = 525,
        // No Factory
        SIPLContact = 351,
        SIPLUser = 352,
        SIPLPort = 353,
        SIPLDepartment = 354,
        SIPLState = 355,
        SIPLCity = 356,
        ContactWorkType = 357,
        SIPLBookingStatus = 358,

        // Report
        EnquiryReport = 401,
        QuotationReport = 402,
        BookingReport = 403,
        BookingStatusReport = 405,
        MCSReport = 406,

        // Document
        DocumentCommon = 500,
        DockReceipt = 501,
        HBL = 502,
        MBL = 503,
        CO = 504,
        WarehouseLocation=535
    };
}
