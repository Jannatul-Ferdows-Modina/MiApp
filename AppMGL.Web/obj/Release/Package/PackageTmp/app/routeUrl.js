var appUrl = (function () {

    var factory = {};

    //#region Private Properties & Methods

    var basePath = {

        filter: "app/controllers/filterController.js",
        lookup: "app/controllers/lookupController.js",

        controllers: "app/controllers/",
        services: "app/services/",
        views: "app/views/",

        sec: "app/components/security/",
        set: "app/components/setup/",
        opr: "app/components/operation/",
        home: "app/components/home/",
        dmt: "app/components/datamanagement/",
        rpt: "app/components/report/",
        doc: "app/components/document/"

    };

    var getPageUrls = function (type, name, logFilter) {
        var pages = {};
        pages.index = basePath[type] + name + "/index.html";
        pages.list = basePath[type] + name + "/list.html";
        pages.listFilter = basePath[type] + name + "/listFilter.html";
        pages.filter = basePath[type] + name + "/filter.html";
        pages.container = "";
        pages.info = basePath[type] + name + "/info.html";
        pages.lookup = basePath[type] + name + "/lookup.html";
        pages.lookupFilter = basePath[type] + name + "/filter.html";
        pages.logFilter = logFilter;
        return pages;
    };

    var getDependencies = function (type, name, other) {
        var depend = [];
        depend.push(basePath.filter);
        depend.push(basePath.lookup);
        depend.push(basePath[type] + name + "/controller.js");
        depend.push(basePath[type] + name + "/service.js");
        other.forEach(function (d) { depend.push(d); });
        return depend;
    };

    //#endregion

    //#region Base Modules

    factory.home = {
        title: "Home",
        urls: { index: basePath.views + "home.html" },
        depend: [
            basePath.controllers + "homeController.js",
            basePath.controllers + "popupController.js"
        ]
    };

    factory.login = {
        title: "Login",
        urls: { index: basePath.views + "login.html" },
        depend: [
            basePath.controllers + "loginController.js",
            basePath.sec + "resetPassword/controller.js",
            basePath.sec + "resetPassword/service.js"
        ]
    };

    factory.signup = {
        title: "SignUp",
        urls: { index: basePath.views + "signup.html" },
        depend: [basePath.controllers + "signupController.js"]
    };

    factory.refresh = {
        title: "Refresh",
        urls: { index: basePath.views + "refresh.html" },
        depend: [basePath.controllers + "refreshController.js"]
    };

    factory.tokens = {
        title: "Tokens",
        urls: { index: basePath.views + "tokens.html" },
        depend: [basePath.controllers + "tokensController.js"]
    };

    factory.associate = {
        title: "Associate",
        urls: { index: basePath.views + "associate.html" },
        depend: [basePath.controllers + "associateController.js"]
    };

    //#endregion

    //#region Security Modules

    factory.moduleType = {

        modId: "101",
        title: "Module Type",
        keyField: "MteId",
        sortField: "MteId",
        sortType: "asc",

        urls: getPageUrls("sec", "moduleType", null),

        tabs: [
            { title: "Detail", url: basePath.sec + "moduleType/detail.html" }
        ],
        criteria: [
            Utility.createFilter("MteId", "long"),
            Utility.createFilter("MteName", "string"),
            Utility.createFilter("MteCaption", "string")
        ],
        logCriteria: null,

        depend: getDependencies("sec", "moduleType", [])
    };

    factory.module = {

        modId: "102",
        title: "Module",
        keyField: "ModId",
        sortField: "ModId",
        sortType: "asc",

        urls: getPageUrls("sec", "module", null),

        tabs: [
            { title: "Detail", url: basePath.sec + "module/detail.html" }
        ],
        criteria: [
            Utility.createFilter("ModId", "long"),
            Utility.createFilter("ModName", "string"),
            Utility.createFilter("ModCaption", "string")
        ],
        logCriteria: null,

        depend: getDependencies("sec", "module", [])
    };

    factory.action = {

        modId: "103",
        title: "Action",
        keyField: "ActId",
        sortField: "ActId",
        sortType: "asc",

        urls: getPageUrls("sec", "action", null),

        tabs: [
            { title: "Detail", url: basePath.sec + "action/detail.html" }
        ],
        criteria: [
            Utility.createFilter("ActId", "string"),
            Utility.createFilter("ActName", "string"),
            Utility.createFilter("ActCaption", "string")
        ],
        logCriteria: null,

        depend: getDependencies("sec", "action", [])
    };

    factory.role = {

        modId: "104",
        title: "Role",
        keyField: "RleId",
        sortField: "RleId",
        sortType: "asc",

        urls: getPageUrls("sec", "role", null),

        tabs: [
            { title: "Detail", url: basePath.sec + "role/detail.html" },
            { title: "Mapping", url: basePath.sec + "role/roleMap.html" }
        ],
        criteria: [
            Utility.createFilter("RleId", "string"),
            Utility.createFilter("RleName", "string"),
            Utility.createFilter("RleStatus", "string")
        ],
        logCriteria: null,

        depend: getDependencies("sec", "role", [])
    };

    factory.site = {

        modId: "106",
        title: "Site",
        keyField: "SitId",
        sortField: "SitCode",
        sortType: "asc",

        urls: getPageUrls("sec", "site", null),

        tabs: [
            { title: "Detail", url: basePath.sec + "site/detail.html" },
            { title: "Role", url: basePath.sec + "site/role.html" }
        ],
        criteria: [
            Utility.createFilter("SitCode", "string"),
            Utility.createFilter("SitName", "string"),
            Utility.createFilter("SitStatus", "string")
        ],
        logCriteria: null,

        depend: getDependencies("sec", "site", [])
    };

    factory.customerUnit = {

        modId: "110",
        title: "CustomerUnit",
        keyField: "ContactId",
        sortField: "CompanyName",
        sortType: "asc",

        urls: getPageUrls("sec", "customerUnit", null),

        tabs: [
            { title: "Detail", url: basePath.sec + "customerUnit/detail.html" },
            { title: "UpdateUnit", url: basePath.sec + "customerUnit/updateUnit.html" }
        ],
        criteria: [
            Utility.createFilter("companyName", "string"),
            Utility.createFilter("customerCode", "string"),
            Utility.createFilter("unitId", "string"),
            Utility.createFilter("refUnitId", "string")
        ],
        logCriteria: null,

        depend: getDependencies("sec", "customerUnit", [])
    };

    factory.user = {

        modId: "108",
        title: "User",
        keyField: "UsrId",
        sortField: "UsrId",
        sortType: "asc",

        urls: getPageUrls("sec", "user", null),

        tabs: [
            { title: "Detail", url: basePath.sec + "user/detail.html" }
        ],
        criteria: [
            Utility.createFilter("CntFirstName", "string", "LG_CONTACT.CntFirstName"),
            Utility.createFilter("CntLastName", "string", "LG_CONTACT.CntLastName"),
            Utility.createFilter("DptName", "string", "LG_CONTACT.LG_DEPARTMENT.DptName")
        ],
        logCriteria: null,

        depend: getDependencies("sec", "user", [
            basePath.sec + "resetPassword/controller.js",
            basePath.sec + "resetPassword/service.js"
        ])
    };

    factory.profile = {

        modId: "109",
        title: "My Profile",
        keyField: "UsrId",
        sortField: "UsrId",
        sortType: "asc",

        urls: getPageUrls("sec", "profile", null),

        tabs: [
            { title: "Detail", url: basePath.sec + "profile/detail.html" }
        ],
        criteria: [
            Utility.createFilter("CntFirstName", "string", "LG_CONTACT.CntFirstName"),
            Utility.createFilter("CntLastName", "string", "LG_CONTACT.CntLastName"),
            Utility.createFilter("DptName", "string", "LG_CONTACT.LG_DEPARTMENT.DptName")
        ],
        logCriteria: null,

        depend: getDependencies("sec", "profile", [
            basePath.sec + "changePassword/controller.js",
            basePath.sec + "changePassword/service.js"
        ])
    };

    //#endregion

    //#region Setup Modules

    factory.country = {

        modId: "201",
        title: "Country",
        keyField: "CryId",
        sortField: "CryName",
        sortType: "asc",

        urls: getPageUrls("set", "country", null),

        tabs: [
            { title: "Detail", url: basePath.set + "country/detail.html" }
        ],
        criteria: [
            Utility.createFilter("CryName", "string"),
            Utility.createFilter("CryAbbrev", "string"),
            Utility.createFilter("CryPhoneCode", "numeric")
        ],
        logCriteria: null,

        depend: getDependencies("set", "country", [])
    };

    factory.state = {

        modId: "202",
        title: "State",
        keyField: "UstId",
        sortField: "UstName",
        sortType: "asc",

        urls: getPageUrls("set", "state", null),

        tabs: [
            { title: "Detail", url: basePath.set + "state/detail.html" }
        ],
        criteria: [
            Utility.createFilter("UstName", "string"),
            Utility.createFilter("UstAbbrev", "string"),
            Utility.createFilter("UstStatus", "string")
        ],
        logCriteria: null,

        depend: getDependencies("set", "state", [])
    };

    factory.department = {

        modId: "203",
        title: "Department",
        keyField: "DptId",
        sortField: "DptId",
        sortType: "asc",

        urls: getPageUrls("set", "department", null),

        tabs: [
            { title: "Detail", url: basePath.set + "department/detail.html" }
        ],
        criteria: [
            Utility.createFilter("DptId", "string"),
            Utility.createFilter("DptName", "string"),
            Utility.createFilter("DptStatus", "string")
        ],
        logCriteria: null,

        depend: getDependencies("set", "department", [])
    };

    factory.title = {

        modId: "204",
        title: "Title",
        keyField: "TtlId",
        sortField: "TtlId",
        sortType: "asc",

        urls: getPageUrls("set", "title", null),

        tabs: [
            { title: "Detail", url: basePath.set + "title/detail.html" }
        ],
        criteria: [
            Utility.createFilter("TtlId", "string"),
            Utility.createFilter("TtlName", "string"),
            Utility.createFilter("TtlStatus", "string")
        ],
        logCriteria: null,

        depend: getDependencies("set", "title", [])
    };

    factory.timezone = {

        modId: "205",
        title: "Timezone",
        keyField: "TmzId",
        sortField: "TmzName",
        sortType: "asc",

        urls: getPageUrls("set", "timezone", null),

        tabs: [
            { title: "Detail", url: basePath.set + "timezone/detail.html" }
        ],
        criteria: [
            Utility.createFilter("TmzName", "string"),
            Utility.createFilter("TmzOffset", "string"),
            Utility.createFilter("TmzStatus", "string")
        ],
        logCriteria: null,

        depend: getDependencies("set", "timezone", [])
    };

    factory.contact = {

        modId: "206",
        title: "Contact",
        keyField: "CntId",
        sortField: "CntFirstName",
        sortType: "asc",

        urls: getPageUrls("set", "contact", null),

        tabs: [
            { title: "Detail", url: basePath.set + "contact/detail.html" }
        ],
        criteria: [
            Utility.createFilter("CntFirstName", "string"),
            Utility.createFilter("CntLastName", "string")
        ],
        logCriteria: null,

        depend: getDependencies("set", "contact", [])
    };

    factory.customerContact = {

        modId: "208",
        title: "CustomerContact",
        keyField: "ContactID",
        sortField: "CompanyName",
        sortType: "asc",

        urls: getPageUrls("opr", "customerContact", null),

        tabs: [
            { title: "Detail", url: basePath.opr + "customerContact/detail.html" }
        ],
        criteria: [
            Utility.createFilter("CompanyName", "string"),
            Utility.createFilter("CustomerCode", "string"),
            Utility.createFilter("CompanyName", "string"),
            Utility.createFilter("galRepresentative", "string"),
            Utility.createFilter("contactCategoryID", "string"),
            Utility.createFilter("companyGradation", "string"),
            Utility.createFilter("OriginCountry", "string"),
            Utility.createFilter("continent", "string"),
            Utility.createFilter("cryName", "string"),
            Utility.createFilter("state", "string"),
            Utility.createFilter("city", "string")
            //Utility.createFilter("ContactCategoryID", "string"),
            //Utility.createFilter(" CompanyGradation", "string")
        ],
        logCriteria: null,

        depend: getDependencies("opr", "customerContact", [
            basePath.dmt + "siplcountryModal/controller.js",
            basePath.dmt + "siplcountryModal/service.js",
            basePath.dmt + "siplStateModal/controller.js",
            basePath.dmt + "siplStateModal/service.js",
            basePath.dmt + "siplCityModal/controller.js",
            basePath.dmt + "siplCityModal/service.js",
            basePath.dmt + "continentModal/controller.js",
            basePath.dmt + "continentModal/service.js"

        ])


    };

    factory.vendorCustomerContact = {

        modId: "208",
        title: "Vendor",
        keyField: "ContactID",
        sortField: "CompanyName",
        sortType: "asc",

        urls: getPageUrls("opr", "vendorCustomerContact", null),

        tabs: [
            { title: "Detail", url: basePath.opr + "vendorCustomerContact/detail.html" }
        ],
        criteria: [
            Utility.createFilter("CompanyName", "string"),
            Utility.createFilter("CustomerCode", "string"),
            Utility.createFilter("CompanyName", "string"),
            Utility.createFilter("galRepresentative", "string"),
            Utility.createFilter("contactCategoryID", "string"),
            Utility.createFilter("companyGradation", "string"),
            Utility.createFilter("OriginCountry", "string"),
            Utility.createFilter("continent", "string"),
            Utility.createFilter("cryName", "string"),
            Utility.createFilter("state", "string"),
            Utility.createFilter("city", "string")
            //Utility.createFilter("ContactCategoryID", "string"),
            //Utility.createFilter(" CompanyGradation", "string")
        ],
        logCriteria: null,

        depend: getDependencies("opr", "vendorCustomerContact", [
            basePath.dmt + "siplcountryModal/controller.js",
            basePath.dmt + "siplcountryModal/service.js",
            basePath.dmt + "siplStateModal/controller.js",
            basePath.dmt + "siplStateModal/service.js",
            basePath.dmt + "siplCityModal/controller.js",
            basePath.dmt + "siplCityModal/service.js",
            basePath.dmt + "continentModal/controller.js",
            basePath.dmt + "continentModal/service.js"

        ])


    };

    factory.dashboard = {

        modId: "209",
        title: "Dashboard",
        keyField: "Id",
        sortField: "Id",
        sortType: "asc",

        urls: getPageUrls("home", "dashboard", null),

        tabs: [
            { title: "Detail", url: basePath.home + "dashboard/detail.html" }
        ],

        logCriteria: null,

        depend: getDependencies("home", "dashboard", [])
    };

    factory.enquiry = {

        modId: "210",
        title: "Enquiry",
        keyField: "EnquiryDate",
        sortField: "EnquiryDate",
        sortType: "DESC",

        urls: getPageUrls("opr", "enquiry", null),

        tabs: [
            { title: "Detail", url: basePath.opr + "enquiry/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("opr", "enquiry", [
            basePath.dmt + "siplcountryModal/controller.js",
            basePath.dmt + "siplcountryModal/service.js",
            basePath.dmt + "siplStateModal/controller.js",
            basePath.dmt + "siplStateModal/service.js",
            basePath.dmt + "siplCityModal/controller.js",
            basePath.dmt + "siplCityModal/service.js",
            basePath.dmt + "continentModal/controller.js",
            basePath.dmt + "continentModal/service.js",
            basePath.dmt + "siplPortModal/controller.js",
            basePath.dmt + "siplPortModal/service.js",
            basePath.dmt + "siplPortModal/service.js",
            basePath.dmt + "siplCompanyModal/controller.js",
            basePath.dmt + "siplCompanyModal/service.js",
            basePath.dmt + "commodityModal/controller.js",
            basePath.dmt + "commodityModal/service.js",
            basePath.dmt + "siplRailRampModal/controller.js",
            basePath.dmt + "siplRailRampModal/service.js",
            basePath.dmt + "siplTerminalModal/controller.js",
            basePath.dmt + "siplTerminalModal/service.js"

        ])
    };

    factory.quotation = {

        modId: "211",
        title: "quotation",
        keyField: "EnquiryNo",
        sortField: "EnquiryNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "quotation", null),

        tabs: [
            { title: "Quotation", url: basePath.opr + "quotation/detail.html" },
            { title: "Quotation Email", url: basePath.opr + "quotation/generateEmail.html" },
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,


        depend: getDependencies("opr", "quotation", [
            basePath.opr + "quotation/enquiryModelController.js"

        ])
    };

    factory.quotationApproval = {

        modId: "212",
        title: "QuotationApproval",
        keyField: "EnquiryNo",
        sortField: "EnquiryNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "quotationApproval", null),

        tabs: [
            { title: "QuotationApporval", url: basePath.opr + "quotationApproval/detail.html" }

        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,


        depend: getDependencies("opr", "quotationApproval", [
            basePath.opr + "quotationApproval/enquiryModelController.js"

        ])
    };

    factory.quotationBooking = {

        modId: "213",
        title: "quotationBooking",
        keyField: "EnquiryNo",
        sortField: "EnquiryNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "quotationBooking", null),

        tabs: [
            { title: "quotationBooking", url: basePath.opr + "quotationBooking/detail.html" }

        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,


        depend: getDependencies("opr", "quotationBooking", [
            basePath.opr + "quotationBooking/enquiryModelController.js"

        ])
    };

    factory.booking = {

        modId: "214",
        title: "booking",
        keyField: "FileNo",
        sortField: "FileNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "booking", null),

        tabs: [
            { title: "Booking", url: basePath.opr + "booking/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,


        depend: getDependencies("opr", "booking", [
            basePath.dmt + "siplcountryModal/controller.js",
            basePath.dmt + "siplcountryModal/service.js",
            basePath.dmt + "siplStateModal/controller.js",
            basePath.dmt + "siplStateModal/service.js",
            basePath.dmt + "siplCityModal/controller.js",
            basePath.dmt + "siplCityModal/service.js",
            basePath.dmt + "siplPortModal/controller.js",
            basePath.dmt + "siplPortModal/service.js",
            basePath.dmt + "siplPortModal/service.js",
            basePath.dmt + "siplCompanyModal/controller.js",
            basePath.dmt + "siplCompanyModal/service.js",
            basePath.dmt + "commodityModal/controller.js",
            basePath.dmt + "commodityModal/service.js",
            basePath.dmt + "siplRailRampModal/controller.js",
            basePath.dmt + "siplRailRampModal/service.js",
            basePath.dmt + "siplTerminalModal/controller.js",
            basePath.dmt + "siplTerminalModal/service.js"
        ])
    };

    factory.bookingConfReadyToSend = {

        modId: "218",
        title: "Booking Conf Ready To Send",
        keyField: "SystemRefNo",
        sortField: "FileNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "bookingConfReadyToSend", null),

        tabs: [
            { title: "bookingConfReadyToSend", url: basePath.opr + "bookingConfReadyToSend/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("opr", "bookingConfReadyToSend", [])
    };

    factory.bookingSpace = {

        modId: "215",
        title: "Space Booking",
        keyField: "EnquiryNo",
        sortField: "FileNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "bookingSpace", null),

        tabs: [
            { title: "Booking", url: basePath.opr + "bookingSpace/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,


        depend: getDependencies("opr", "bookingSpace", [
            basePath.dmt + "siplcountryModal/controller.js",
            basePath.dmt + "siplcountryModal/service.js",
            basePath.dmt + "siplStateModal/controller.js",
            basePath.dmt + "siplStateModal/service.js",
            basePath.dmt + "siplCityModal/controller.js",
            basePath.dmt + "siplCityModal/service.js",
            basePath.dmt + "siplPortModal/controller.js",
            basePath.dmt + "siplPortModal/service.js",
            basePath.dmt + "siplPortModal/service.js",
            basePath.dmt + "siplCompanyModal/controller.js",
            basePath.dmt + "siplCompanyModal/service.js",
            basePath.dmt + "commodityModal/controller.js",
            basePath.dmt + "commodityModal/service.js",
            basePath.dmt + "siplRailRampModal/controller.js",
            basePath.dmt + "siplRailRampModal/service.js",
            basePath.dmt + "siplTerminalModal/controller.js",
            basePath.dmt + "siplTerminalModal/service.js"
        ])
    };

    factory.bookingCaptureExpenses = {

        modId: "216",
        title: "Capture Expenses",
        keyField: "EnquiryNo",
        sortField: "FileNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "bookingCaptureExpenses", null),

        tabs: [
            { title: "Capture Expenses", url: basePath.opr + "bookingCaptureExpenses/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,


        depend: getDependencies("opr", "bookingCaptureExpenses", [])
    };
    factory.bookingCaptureContainer = {

        modId: "217",
        title: "Capture Container",
        keyField: "EnquiryNo",
        sortField: "FileNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "bookingCaptureContainer", null),

        tabs: [
            { title: "Capture Container", url: basePath.opr + "bookingCaptureContainer/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,


        depend: getDependencies("opr", "bookingCaptureContainer", [])
    };

    factory.bookingDocument = {

        modId: "218",
        title: "Document Attachment",
        keyField: "SystemRefNo",
        sortField: "LastRemarkDate",
        sortType: "ASC",

        urls: getPageUrls("opr", "bookingDocument", null),

        tabs: [
            { title: "Waiting for Line Confirmation", url: basePath.opr + "bookingDocument/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("opr", "bookingDocument", [])
    };    

    factory.bookingRollover = {

        modId: "219",
        title: "Rollover Records",
        keyField: "SystemRefNo",
        sortField: "SystemRefNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "bookingRollover", null),

        tabs: [
            { title: "Rollover Records", url: basePath.opr + "bookingRollover/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("opr", "bookingRollover", [])
    };

    factory.bookingShipperConfirmation = {

        modId: "220",
        title: "Shipper Confirmation",
        keyField: "SystemRefNo",
        sortField: "SystemRefNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "bookingShipperConfirmation", null),

        tabs: [
            { title: "Shipper Confirmation", url: basePath.opr + "bookingShipperConfirmation/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("opr", "bookingShipperConfirmation", [])
    };

    factory.bookingShippingLineRemarks = {

        modId: "221",
        title: "ShippingLine Remarks",
        keyField: "SystemRefNo",
        sortField: "SystemRefNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "bookingShippingLineRemarks", null),

        tabs: [
            { title: "ShippingLine Remarks", url: basePath.opr + "bookingShippingLineRemarks/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("opr", "bookingShippingLineRemarks", [])
    };

    factory.finalizedTransportation = {

        modId: "222",
        title: "Finalize Transportation",
        keyField: "SystemRefNo",
        sortField: "SystemRefNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "finalizedTransportation", null),

        tabs: [
            { title: "Finalize Transportation", url: basePath.opr + "finalizedTransportation/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("opr", "finalizedTransportation", [])
    };

    factory.pendingMovement = {

        modId: "223",
        title: "Pending Movement",
        keyField: "SystemRefNo",
        sortField: "SystemRefNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "pendingMovement", null),

        tabs: [
            { title: "Pending Movement", url: basePath.opr + "pendingMovement/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("opr", "pendingMovement", [])
    };

    factory.documentation = {

        modId: "220",
        title: "documentation",
        keyField: "SystemRefNo",
        sortField: "SystemRefNo",
        sortType: "DESC",

        urls: getPageUrls("opr", "documentation", null),

        tabs: [
            { title: "documentation", url: basePath.opr + "documentation/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("opr", "documentation", [])
    };
    

    factory.contractRate = {

        modId: "208",
        title: "Contract Capture",
        keyField: "ContractID",
        sortField: "ContractID",
        sortType: "DESC",

        urls: getPageUrls("opr", "contractRate", null),

        tabs: [
            { title: "Contract Capture", url: basePath.opr + "contractRate/detail.html" }

        ],
        criteria: [
            Utility.createFilter("Carrier", "string"),
            Utility.createFilter("ContractNo", "string"),
            Utility.createFilter("StartDate", "string"),
            Utility.createFilter("EndDate", "string")
        ],
        logCriteria: null,

        depend: getDependencies("opr", "contractRate", [
             basePath.opr + "contractRateModal/controller.js",
            basePath.opr + "contractRateModal/service.js"
        ])

    };

    factory.rateCapture = {

        modId: "208",
        title: "Rate Capture",
        keyField: "CCID",
        sortField: "CCID",
        sortType: "DESC",

        urls: getPageUrls("opr", "rateCapture", null),

        tabs: [
            { title: "Rate Capture", url: basePath.opr + "rateCapture/detail.html" }

        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")
            

        ],
        logCriteria: null,

        depend: getDependencies("opr", "rateCapture", [])

    };

    factory.captureRoute = {

        modId: "208",
        title: "Route Capture",
        keyField: "CCID",
        sortField: "CCID",
        sortType: "DESC",

        urls: getPageUrls("opr", "captureRoute", null),

        tabs: [
            { title: "Route Capture", url: basePath.opr + "captureRoute/detail.html" }

        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")


        ],
        logCriteria: null,

        depend: getDependencies("opr", "captureRoute", [])

    };

    factory.contactcategory = {

        modId: "290",
        title: "ContactCategory",
        keyField: "ContactCategoryId",
        sortField: "Name",
        sortType: "asc",

        urls: getPageUrls("dmt", "contactcategory", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "contactcategory/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string")
        ],
        logCriteria: null,

        depend: getDependencies("dmt", "contactcategory", [])
    };

    factory.commodity = {

        modId: "291",
        title: "Commodity",
        keyField: "CommodityId",
        sortField: "Name",
        sortType: "asc",

        urls: getPageUrls("dmt", "commodity", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "commodity/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("Description", "string"),
            Utility.createFilter("CommodityType", "string", "SIPL_CommodityType.CommodityType")
        ],
        logCriteria: null,

        depend: getDependencies("dmt", "commodity", [])
    };

    factory.containertype = {

        modId: "292",
        title: "ContainerType",
        keyField: "ContainerTypeId",
        sortField: "Name",
        sortType: "asc",

        urls: getPageUrls("dmt", "containertype", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "containertype/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("Description", "string")
        ],
        logCriteria: null,

        depend: getDependencies("dmt", "containertype", [])
    };

    factory.loadtype = {

        modId: "293",
        title: "LoadType",
        keyField: "LoadTypeId",
        sortField: "Name",
        sortType: "asc",

        urls: getPageUrls("dmt", "loadtype", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "loadtype/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("Description", "string")
        ],
        logCriteria: null,

        depend: getDependencies("dmt", "loadtype", [])
    };

    factory.tradeservice = {
        modId: "294",
        title: "TradeService",
        keyField: "TradeServiceId",
        sortField: "Name",
        sortType: "asc",

        urls: getPageUrls("dmt", "tradeservice", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "tradeservice/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("Description", "string")
        ],
        logCriteria: null,

        depend: getDependencies("dmt", "tradeservice", [])
    };

    factory.continent = {

        modId: "296",
        title: "Continent",
        keyField: "ContinentId",
        sortField: "Name",
        sortType: "asc",

        urls: getPageUrls("dmt", "continent", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "continent/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("Description", "string")
        ],
        logCriteria: null,

        depend: getDependencies("dmt", "continent", [])
    };

    factory.siplcountry = {

        modId: "297",
        title: "SIPLCountry",
        keyField: "CountryId",
        sortField: "Name",
        sortType: "asc",

        urls: getPageUrls("dmt", "siplcountry", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "siplcountry/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("ContinentName", "string", "SIPL_Continent.Name")

        ],
        logCriteria: null,

        depend: getDependencies("dmt", "siplcountry", [])
    };

    factory.lgvwstate = {

        modId: "298",
        title: "LGVWState",
        keyField: "StateId",
        sortField: "Name",
        sortType: "asc",


        urls: getPageUrls("dmt", "lgvwstate", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "lgvwstate/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("CountryName", "string")

        ],
        logCriteria: null,

        depend: getDependencies("dmt", "lgvwstate", [])
    };

    factory.lgvwcity = {

        modId: "299",
        title: "LGVWCity",
        keyField: "CityId",
        sortField: "Name",
        sortType: "asc",


        urls: getPageUrls("dmt", "lgvwcity", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "lgvwcity/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("StateName", "string")

        ],
        logCriteria: null,

        depend: getDependencies("dmt", "lgvwcity", [])
    };

    factory.railramp = {

        modId: "300",
        title: "RailRamp",
        keyField: "railId",
        sortField: "railRamp",
        sortType: "asc",


        urls: getPageUrls("dmt", "railramp", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "railramp/detail.html" }
        ],
        criteria: [
            Utility.createFilter("railRamp", "string")


        ],
        logCriteria: null,

        depend: getDependencies("dmt", "railramp", [])
    };

    factory.terminal = {

        modId: "301",
        title: "Terminal",
        keyField: "terminalId",
        sortField: "terminalName",
        sortType: "asc",


        urls: getPageUrls("dmt", "terminal", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "terminal/detail.html" }
        ],
        criteria: [
            Utility.createFilter("terminalName", "string")


        ],
        logCriteria: null,

        depend: getDependencies("dmt", "terminal", [])
    };

    factory.lgvwport = {

        modId: "302",
        title: "LGVWPort",
        keyField: "PortId",
        sortField: "Name",
        sortType: "asc",


        urls: getPageUrls("dmt", "lgvwport", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "lgvwport/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("CountryName", "string")

        ],
        logCriteria: null,

        depend: getDependencies("dmt", "lgvwport", [])
    };

    factory.surchargegroup = {

        modId: "303",
        title: "SurchargeGroup",
        keyField: "SurchargesGroupId",
        sortField: "Name",
        sortType: "asc",


        urls: getPageUrls("dmt", "surchargegroup", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "surchargegroup/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("ShortCode", "string")


        ],
        logCriteria: null,

        depend: getDependencies("dmt", "surchargegroup", [])
    };

    factory.portgroup = {

        modId: "304",
        title: "PortGroup",
        keyField: "PortGroupId",
        sortField: "Name",
        sortType: "asc",


        urls: getPageUrls("dmt", "portgroup", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "portgroup/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("ShortCode", "string")


        ],
        logCriteria: null,

        depend: getDependencies("dmt", "portgroup", [])
    };

    factory.lgvwalias = {

        modId: "305",
        title: "LGVWAlias",
        keyField: "AliasID",
        sortField: "Alias",
        sortType: "asc",


        urls: getPageUrls("dmt", "lgvwalias", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "lgvwalias/detail.html" }
        ],
        criteria: [
            Utility.createFilter("Name", "string"),
            Utility.createFilter("Alias", "string")


        ],
        logCriteria: null,

        depend: getDependencies("dmt", "lgvwalias", [])
    };

    factory.lgacctcategory = {

        modId: "306",
        title: "Account Category",
        keyField: "AcyID",
        sortField: "AcyNAME",
        sortType: "asc",


        urls: getPageUrls("dmt", "lgacctcategory", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "lgacctcategory/detail.html" }
        ],
        criteria: [
            Utility.createFilter("AcyNAME", "string")



        ],
        logCriteria: null,

        depend: getDependencies("dmt", "lgacctcategory", [])
    };

    factory.lgspfeecategory = {

        modId: "307",
        title: "SP FEE Category",
        keyField: "SfcID",
        sortField: "SfcName",
        sortType: "asc",


        urls: getPageUrls("dmt", "lgspfeecategory", null),

        tabs: [
            { title: "Detail", url: basePath.dmt + "lgspfeecategory/detail.html" }
        ],
        criteria: [
            Utility.createFilter("SfcName", "string")



        ],
        logCriteria: null,

        depend: getDependencies("dmt", "lgspfeecategory", [])
    };

    //#endregion

    //#region Report Modules


    factory.enquiryReport = {

        modId: "401",
        title: "Enquiry Report",
        keyField: "EnquiryId",
        sortField: "SerialNo",
        sortType: "asc",

        urls: getPageUrls("rpt", "enquiryReport", null),

        tabs: [
            { title: "Detail", url: basePath.rpt + "enquiryReport/index.html" }
        ],
        criteria: [
            Utility.createFilter("Year", "string"),
            Utility.createFilter("Type", "string"),
            Utility.createFilter("EnquiryDateFrom", "string"),
            Utility.createFilter("EnquiryDateTo", "string"),
            Utility.createFilter("EnquiryNo", "string"),
            Utility.createFilter("CompanyName", "string")
        ],
        logCriteria: null,

        depend: getDependencies("rpt", "enquiryReport", [])
    };

    factory.quotationReport = {

        modId: "402",
        title: "Quotation Report",
        keyField: "QuotatationId",
        sortField: "SerialNo",
        sortType: "asc",

        urls: getPageUrls("rpt", "quotationReport", null),

        tabs: [
            { title: "Detail", url: basePath.rpt + "quotationReport/index.html" }
        ],
        criteria: [
            Utility.createFilter("Type", "string"),
            Utility.createFilter("EnquiryDateFrom", "string"),
            Utility.createFilter("EnquiryDateTo", "string"),
            Utility.createFilter("EnquiryNo", "string"),
            Utility.createFilter("CompanyName", "string"),
            Utility.createFilter("QuotationNo", "string"),
            Utility.createFilter("SystemRefNo", "string"),
            Utility.createFilter("MiamiRefNo", "string"),
            Utility.createFilter("BookingNo", "string")
        ],
        logCriteria: null,

        depend: getDependencies("rpt", "quotationReport", [
            basePath.rpt + "viewCharges/controller.js",
            basePath.rpt + "viewCharges/service.js"
        ])
    };

    factory.bookingReport = {

        modId: "403",
        title: "Booking Report",
        keyField: "SerialNo",
        sortField: "SerialNo",
        sortType: "asc",

        urls: getPageUrls("rpt", "bookingReport", null),

        tabs: [
            { title: "Detail", url: basePath.rpt + "bookingReport/index.html" }
        ],
        criteria: [
            Utility.createFilter("DepartmentId", "string"),
            Utility.createFilter("EnquiryNo", "string"),
            Utility.createFilter("CompanyName", "string"),
            Utility.createFilter("QuotationNo", "string"),
            Utility.createFilter("SystemRefNo", "string"),
            Utility.createFilter("MiamiRefNo", "string"),
            Utility.createFilter("BookingNo", "string")
        ],
        logCriteria: null,

        depend: getDependencies("rpt", "bookingReport", [])
    };

    factory.bookingStatusReport = {

        modId: "405",
        title: "Booking Status Report",
        keyField: "SerialNo",
        sortField: "SerialNo",
        sortType: "asc",

        urls: getPageUrls("rpt", "bookingStatusReport", null),

        tabs: [
            { title: "Detail", url: basePath.rpt + "bookingStatusReport/index.html" }
        ],
        criteria: [
            Utility.createFilter("BookingStatusId", "string"),
            Utility.createFilter("EnquiryNo", "string"),
            Utility.createFilter("CompanyName", "string"),
            Utility.createFilter("QuotationNo", "string"),
            Utility.createFilter("SystemRefNo", "string"),
            Utility.createFilter("MiamiRefNo", "string"),
            Utility.createFilter("BookingNo", "string")
        ],
        logCriteria: null,

        depend: getDependencies("rpt", "bookingStatusReport", [])
    };

    factory.mcsReport = {

        modId: "406",
        title: "MCS Report",
        keyField: "ExportRef",
        sortField: "ExportRef",
        sortType: "asc",

        urls: getPageUrls("rpt", "mcsReport", null),

        tabs: [
            { title: "Detail", url: basePath.rpt + "mcsReport/index.html" }
        ],
        criteria: [
            Utility.createFilter("SiteId", "string"),
            Utility.createFilter("StartBookingDate", "string"),
            Utility.createFilter("EndBookingDate", "string"),
            Utility.createFilter("DeptId", "string")
        ],
        logCriteria: null,

        depend: getDependencies("rpt", "mcsReport", [])
    };

    factory.exportRegisterReport = {

        modId: "407",
        title: "Export Register Report",
        keyField: "SystemRefNo",
        sortField: "LastRemarkDate",
        sortType: "ASC",

        urls: getPageUrls("rpt", "exportRegisterReport", null),

        tabs: [
            { title: "Export Register Report", url: basePath.rpt + "exportRegisterReport/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("rpt", "exportRegisterReport", [
                            basePath.rpt + "documentUploadModel/controller.js",
                            basePath.rpt + "documentUploadModel/service.js"
        ])



    };

    factory.processInvoiceReport = {

        modId: "407",
        title: "Process Invoice Report",
        keyField: "SystemRefNo",
        sortField: "LastRemarkDate",
        sortType: "ASC",

        urls: getPageUrls("rpt", "processInvoiceReport", null),

        tabs: [
            { title: "Process Invoice Report", url: basePath.rpt + "processInvoiceReport/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("rpt", "processInvoiceReport", [
                            basePath.rpt + "documentUploadModel/controller.js",
                            basePath.rpt + "documentUploadModel/service.js",
                            basePath.rpt + "viewCharges/controller.js",
                            basePath.rpt + "viewCharges/service.js"
        ])        

    };

    factory.expensesReport = {

        modId: "407",
        title: "expenses Report",
        keyField: "SystemRefNo",
        sortField: "documentCommonID",
        sortType: "ASC",

        urls: getPageUrls("rpt", "expensesReport", null),

        tabs: [
            { title: "Expenses Report", url: basePath.rpt + "expensesReport/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("rpt", "expensesReport", [])



    };

    factory.regionWiseContainerReport = {

        modId: "407",
        title: "Region Wise Container Report",
        keyField: "regionName",
        sortField: "regionName",
        sortType: "ASC",

        urls: getPageUrls("rpt", "regionWiseContainerReport", null),

        tabs: [
            { title: "Region Wise Container Report", url: basePath.rpt + "regionWiseContainerReport/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("rpt", "regionWiseContainerReport", [])



    };

    factory.activityDueReport = {

        modId: "407",
        title: "Activity Due Report",
        keyField: "SystemRefNo",
        sortField: "documentCommonID",
        sortType: "ASC",

        urls: getPageUrls("rpt", "activityDueReport", null),

        tabs: [
            { title: "Activity Due Report", url: basePath.rpt + "activityDueReport/detail.html" }
        ],
        criteria: [
            Utility.createFilter("searchOptionValue", "string"),
            Utility.createFilter("searchBox", "string")

        ],
        logCriteria: null,

        depend: getDependencies("rpt", "activityDueReport", [])



    };
    //#endregion

    //#region Document Modules

    factory.dockReceipt = {

        modId: "501",
        title: "Dock Receipt",
        keyField: "DocumentCommonId",
        sortField: "DocumentCommonId",
        sortType: "asc",

        urls: getPageUrls("doc", "dockReceipt", null),

        tabs: [
            { title: "Detail", url: basePath.doc + "dockReceipt/index.html" }
        ],
        criteria: [
            Utility.createFilter("MiamiRefNo", "string"),
            Utility.createFilter("SystemRefNo", "string"),
            Utility.createFilter("BookingNo", "string")
        ],
        logCriteria: null,

        depend: getDependencies("doc", "dockReceipt", [
            basePath.dmt + "siplCompanyModal/controller.js",
            basePath.dmt + "siplCompanyModal/service.js",
            basePath.dmt + "siplPortModal/controller.js",
            basePath.dmt + "siplPortModal/service.js",
            basePath.doc + "viewDocumentList/controller.js",
            basePath.doc + "viewDocumentList/service.js"
        ])
    };

    factory.hbl = {

        modId: "502",
        title: "HBL",
        keyField: "DocumentCommonId",
        sortField: "DocumentCommonId",
        sortType: "asc",

        urls: getPageUrls("doc", "hbl", null),

        tabs: [
            { title: "Detail", url: basePath.doc + "hbl/index.html" }
        ],
        criteria: [
            Utility.createFilter("MiamiRefNo", "string"),
            Utility.createFilter("SystemRefNo", "string"),
            Utility.createFilter("BookingNo", "string")
        ],
        logCriteria: null,

        depend: getDependencies("doc", "hbl", [
            basePath.dmt + "siplCompanyModal/controller.js",
            basePath.dmt + "siplCompanyModal/service.js",
            basePath.dmt + "siplPortModal/controller.js",
            basePath.dmt + "siplPortModal/service.js",
            basePath.doc + "viewDocumentList/controller.js",
            basePath.doc + "viewDocumentList/service.js"
        ])
    };

    factory.mbl = {

        modId: "503",
        title: "MBL",
        keyField: "DocumentCommonId",
        sortField: "DocumentCommonId",
        sortType: "asc",

        urls: getPageUrls("doc", "mbl", null),

        tabs: [
            { title: "Detail", url: basePath.doc + "mbl/index.html" }
        ],
        criteria: [
            Utility.createFilter("MiamiRefNo", "string"),
            Utility.createFilter("SystemRefNo", "string"),
            Utility.createFilter("BookingNo", "string")
        ],
        logCriteria: null,

        depend: getDependencies("doc", "mbl", [
            basePath.dmt + "siplCompanyModal/controller.js",
            basePath.dmt + "siplCompanyModal/service.js",
            basePath.dmt + "siplPortModal/controller.js",
            basePath.dmt + "siplPortModal/service.js",
            basePath.doc + "viewDocumentList/controller.js",
            basePath.doc + "viewDocumentList/service.js"
        ])
    };

    factory.co = {

        modId: "504",
        title: "CO",
        keyField: "DocumentCommonId",
        sortField: "DocumentCommonId",
        sortType: "asc",

        urls: getPageUrls("doc", "co", null),

        tabs: [
            { title: "Detail", url: basePath.doc + "co/index.html" }
        ],
        criteria: [
            Utility.createFilter("MiamiRefNo", "string"),
            Utility.createFilter("SystemRefNo", "string"),
            Utility.createFilter("BookingNo", "string")
        ],
        logCriteria: null,

        depend: getDependencies("doc", "co", [
            basePath.dmt + "siplCompanyModal/controller.js",
            basePath.dmt + "siplCompanyModal/service.js",
            basePath.dmt + "siplPortModal/controller.js",
            basePath.dmt + "siplPortModal/service.js",
            basePath.doc + "viewDocumentList/controller.js",
            basePath.doc + "viewDocumentList/service.js"
        ])
    };

    //#endregion

    return factory;
})();
