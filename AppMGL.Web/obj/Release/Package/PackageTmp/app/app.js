"use strict";

define(["routeResolver"], function () {

    var app = angular.module("AppMGL", ["ngRoute", "ngSanitize", "ngAnimate", "ngIdle", "routeResolverService", "clientLoggerService", "ui.bootstrap",
        "ui.bootstrap.tpls", "ui.mask", "ui.validate", "ui.bootstrap.showErrors", "angular-loading-bar", "infinite-scroll", "nvd3", "ui.calendar",
        "angular-cache", "noCAPTCHA", "ngTable", "ngFileUpload", "ngTagsInput", "ng-fusioncharts", "LocalStorageModule", "adminLTE", "ckeditor"]);

    app.constant("ngAuthSettings", {
        //webBaseUri: "",
        //for remote server uncomment the below line...
        apiServiceBaseUri: "https://lo-expert.miamigloballines.com:443/AppMGL/",
        //To run locally uncomment the below line...
       // apiServiceBaseUri: "http://localhost/AppMGL/",

        webBaseUri: "MGL/",
        clientId: "ngAuthApp"
    });

    var eRequire = require.config({
        context: 'config',
        urlArgs: 'v=' +new Date().toJSON(),
    });
    eRequire(['appConfig', 'app/appConstants'], function (webConfig, appConstants) {
        app.constant('webAppConfig', webConfig);
        app.constant('appConstants', appConstants);
    });
    app.config(["$routeProvider", "$locationProvider", "$controllerProvider", "$compileProvider", "$filterProvider", "$provide",
        "$httpProvider", "tagsInputConfigProvider", "routeResolverProvider", "ngAuthSettings", "KeepaliveProvider",
        "IdleProvider","webAppConfig",

        function ($routeProvider, $locationProvider, $controllerProvider, $compileProvider, $filterProvider, $provide,
            $httpProvider, tagsInputConfigProvider, routeResolverProvider, ngAuthSettings,
            KeepaliveProvider, IdleProvider, webAppConfig) {

            tagsInputConfigProvider.setDefaults("tagsInput", { placeholder: "" });

            //Change default views and controllers directory using the following:
            //routeResolverProvider.routeConfig.setBaseDirectories('/app/components', '/app/controllers', '/app/services', '/app/views');

            //Define routes - controllers will be loaded dynamically
            var route = routeResolverProvider.route;
            IdleProvider.idle(Number(webAppConfig.idleTimeOut)*60);
            IdleProvider.timeout(Number(webAppConfig.userWarningTime));
            KeepaliveProvider.interval(Number(webAppConfig.userWarningTime));
            $routeProvider
                //route.resolve() now accepts the convention to use (name of controller & view) as well as the 
                //path where the controller or view lives in the controllers or views folder if it's in a sub folder. 
                .when("/home", route.resolve({ module: "dashboard" }))
                .when("/home/dashboard", route.resolve({ module: "dashboard" }))
                .when("/uc", { templateUrl: "app/views/shared/uc.html" })
                .when("/shipment", { templateUrl: "app/views/shipment.html" })
                .when("/common", { templateUrl: "app/views/moduleMenu.html" })
                .when("/login", route.resolve({ module: "login" }))
                .when("/signup", route.resolve({ module: "signup" }))
                .when("/refresh", route.resolve({ module: "refresh" }))
                .when("/tokens", route.resolve({ module: "tokens" }))
                .when("/associate", route.resolve({ module: "associate" }))
                .when("/security/moduleType", route.resolve({ module: "moduleType" }))
                .when("/security/module", route.resolve({ module: "module" }))
                .when("/security/action", route.resolve({ module: "action" }))
                .when("/security/role", route.resolve({ module: "role" }))
                .when("/security/site", route.resolve({ module: "site" }))
                .when("/security/customerUnit", route.resolve({ module: "customerUnit" }))
                .when("/security/user", route.resolve({ module: "user" }))
                .when("/security/profile", route.resolve({ module: "profile" }))
                .when("/setup/country", route.resolve({ module: "country" }))
                .when("/setup/state", route.resolve({ module: "state" }))
                .when("/setup/department", route.resolve({ module: "department" }))
                .when("/setup/title", route.resolve({ module: "title" }))
                .when("/setup/timezone", route.resolve({ module: "timezone" }))
                .when("/setup/contact", route.resolve({ module: "contact" }))
                .when("/operation/customerContact", route.resolve({ module: "customerContact" }))
                .when("/operation/enquiry", route.resolve({ module: "enquiry" }))
                .when("/operation/enquiry/:id", route.resolve({ module: "enquiry" }))
                .when("/operation/quotation", route.resolve({ module: "quotation" }))
                .when("/operation/quotationApproval", route.resolve({ module: "quotationApproval" }))
                .when("/operation/quotationBooking", route.resolve({ module: "quotationBooking" }))
                .when("/operation/booking", route.resolve({ module: "booking" }))
                .when("/operation/bookingConfReadyToSend", route.resolve({ module: "bookingConfReadyToSend" }))
                .when("/operation/contractRate", route.resolve({ module: "contractRate" }))
                .when("/operation/rateCapture", route.resolve({ module: "rateCapture" }))
                .when("/operation/captureRoute", route.resolve({ module: "captureRoute" }))
                .when("/operation/bookingSpace", route.resolve({ module: "bookingSpace" }))
                .when("/operation/bookingCaptureExpenses", route.resolve({ module: "bookingCaptureExpenses" }))
                .when("/operation/bookingCaptureContainer", route.resolve({ module: "bookingCaptureContainer" }))
                .when("/operation/bookingDocument", route.resolve({ module: "bookingDocument" }))
                .when("/operation/bookingRollover", route.resolve({ module: "bookingRollover" }))
                .when("/operation/bookingShipperConfirmation", route.resolve({ module: "bookingShipperConfirmation" }))
                .when("/operation/bookingShippingLineRemarks", route.resolve({ module: "bookingShippingLineRemarks" }))
                .when("/operation/finalizedTransportation", route.resolve({ module: "finalizedTransportation" }))
                .when("/operation/pendingMovement", route.resolve({ module: "pendingMovement" }))
                .when("/operation/documentation1", route.resolve({ module: "documentation" }))
                .when("/operation/documentation2", route.resolve({ module: "documentation" }))
                .when("/operation/documentation3", route.resolve({ module: "documentation" }))
                .when("/operation/documentation4", route.resolve({ module: "documentation" }))
                .when("/operation/documentation5", route.resolve({ module: "documentation" }))
                .when("/operation/documentation6", route.resolve({ module: "documentation" }))
                .when("/operation/documentation7", route.resolve({ module: "documentation" }))
                .when("/operation/documentation8", route.resolve({ module: "documentation" }))
                .when("/operation/documentation9", route.resolve({ module: "documentation" }))
                .when("/datamanagement/contactcategory", route.resolve({ module: "contactcategory" }))
                .when("/operation/vendorCustomerContact", route.resolve({ module: "vendorCustomerContact" }))
                .when("/datamanagement/commodity", route.resolve({ module: "commodity" }))
                .when("/datamanagement/containertype", route.resolve({ module: "containertype" }))
                .when("/datamanagement/loadtype", route.resolve({ module: "loadtype" }))
                .when("/datamanagement/tradeservice", route.resolve({ module: "tradeservice" }))
                .when("/datamanagement/continent", route.resolve({ module: "continent" }))
                .when("/datamanagement/siplcountry", route.resolve({ module: "siplcountry" }))
                .when("/datamanagement/lgvwstate", route.resolve({ module: "lgvwstate" }))
                .when("/datamanagement/lgvwcity", route.resolve({ module: "lgvwcity" }))
                .when("/datamanagement/lgvwalias", route.resolve({ module: "lgvwalias" }))
                .when("/datamanagement/railramp", route.resolve({ module: "railramp" }))
                .when("/datamanagement/terminal", route.resolve({ module: "terminal" }))
                .when("/datamanagement/lgvwport", route.resolve({ module: "lgvwport" }))
                .when("/datamanagement/surchargegroup", route.resolve({ module: "surchargegroup" }))
                .when("/datamanagement/portgroup", route.resolve({ module: "portgroup" }))
                .when("/datamanagement/lgacctcategory", route.resolve({ module: "lgacctcategory" }))
                .when("/datamanagement/lgspfeecategory", route.resolve({ module: "lgspfeecategory" }))
                .when("/report/enquiryReport", route.resolve({ module: "enquiryReport" }))
                .when("/report/quotationReport", route.resolve({ module: "quotationReport" }))
                .when("/report/bookingReport", route.resolve({ module: "bookingReport" }))
                .when("/report/bookingStatusReport", route.resolve({ module: "bookingStatusReport" }))
                .when("/report/mcsReport", route.resolve({ module: "mcsReport" }))
                .when("/report/exportRegisterReport", route.resolve({ module: "exportRegisterReport" }))
                .when("/report/expensesReport", route.resolve({ module: "expensesReport" }))
                .when("/report/regionWiseContainerReport", route.resolve({ module: "regionWiseContainerReport" }))
                .when("/report/processInvoiceReport", route.resolve({ module: "processInvoiceReport" }))
                .when("/report/activityDueReport", route.resolve({ module: "activityDueReport" }))
                .when("/document/dockReceipt", route.resolve({ module: "dockReceipt" }))
                .when("/document/hbl", route.resolve({ module: "hbl" }))
                .when("/document/mbl", route.resolve({ module: "mbl" }))
                .when("/document/co", route.resolve({ module: "co" }))
                .otherwise({ redirectTo: "/home/dashboard" });

            //$locationProvider.html5Mode(true);

            app.register =
                {
                    controller: $controllerProvider.register,
                    directive: $compileProvider.directive,
                    filter: $filterProvider.register,
                    factory: $provide.factory,
                    service: $provide.service
                };

            $httpProvider.interceptors.push("authInterceptorService");
        }]);

    app.run(["$rootScope", "$location", "authService", "adminLTEService",
        function ($rootScope, $location, authService, adminLTE) {

            adminLTE.init();
            authService.fillAuthData();

            $rootScope.$on('$locationChangeStart', function (event, next, current) {
                if (!authService.authentication.isAuth) {
                    var returnUrl = current.indexOf("#") >= 0 ? current.substring(current.indexOf("#") + 1) : "/home/dashboard";
                    if ($location.search().logout) { returnUrl = "/home/dashboard"; }
                    $location.path("/login").search({ returnUrl: returnUrl, logout: $location.search().logout });
                }
                //if ($(".control-sidebar").hasClass("control-sidebar-open")) {
                //    $("[data-toggle='control-sidebar']").click();
                //}
            });
        }]);

    return app;

});

