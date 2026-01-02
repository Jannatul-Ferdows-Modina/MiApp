"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$window", "$route", "$timeout", "$location", "$uibModal", "localStorageService",
        "NgTableParams", "ngNotifier", "authService", "Idle", "webAppConfig", "$rootScope", "appConstants"];

    var controller = function ($scope, $window, $route, $timeout, $location, $uibModal, localStorageService,
        NgTableParams, ngNotifier, authService, Idle, webAppConfig, $rootScope, appConstants) {

        //#region Properties

        $scope.appTitle = "MGL";
        $scope.navbarExpanded = false;
        $scope.pageTitle = "Dashboard";
        $scope.breadcrumbs = ["Dashboard"];
        $scope.userSites = [];
        $scope.selectedSite = {};
        $scope.userInfo = {};
        $scope.menus = [];
        $scope.authentication = authService.authentication;
        $scope.accounts = [];
        $scope.randomId = 0;
        $scope.selectedSiteId = 0;
        $scope.userWorkTypeId = 0;
        $scope.userImage = [];

        $rootScope.webConfig = $scope.config = webAppConfig;
        //#endregion

        //#region Private Methods
        var getConfigDetails = function () {
            authService.getConfigDetails().then(function (details) {
                $scope.config = details;
            });
        };
        //getConfigDetails();
        var initMainPage = function () {

            $("div#splash").hide();
            $("div.wrapper").show();

            authService.validateAccessToken().then(
                function (output) {
                    if (output.status == 200) {
                        var authData = localStorageService.get("authData");
                        $scope.authentication.userSite = authData.userSite;
                        $scope.selectedSiteId = authData.selectedSiteId;
                        var params = {
                            UsrId: $scope.authentication.userId,
                            SitId: $scope.selectedSiteId
                        };
                        authService.getUserInfo(params).then(
                            function (output) {
                                $scope.userInfo = output.data.data;
                                $scope.userWorkTypeId = output.data.data.contact.cwtId;
                                if (output.data.data.contact.cntImageName != null) {
                                    $scope.userImage = authService.location + 'UserProfile/' + output.data.data.contact.cntImageName;
                                }
                                else {
                                    $scope.userImage = 'Images/Icons/male_user_icon.png';
                                }
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });

                        authService.getUserRoles(params).then(
                            function (output) {
                                localStorageService.set("userRoles", output.data.data);
                                fetchingMenu();
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });
                        //authService.downloadUserImage(params).then(
                        //    function (output) {
                        //        //$scope.userImage = $scope.arrayBufferToBase64(output.data);
                        //        $scope.userImage = authService.location + 'CustomerFiles/percent_icon.png';
                        //    },
                        //    function (output) {
                        //        ngNotifier.showError($scope.authentication, output);
                        //    });
                    }
                    else {
                        $scope.logOut();
                    }
                },
                function (output) {
                    if (output.status != 400 && output.status != 401) {
                        ngNotifier.error(output.statusText);
                    }
                });
        };

        var fetchingMenu = function () {

            $scope.menus = [
                {
                    "menuId": 1,
                    "text": "Dashboard",
                    "iconClass": "fa fa-home",
                    "breadcrumbs": "Dashboard",
                    "href": "#!/home/dashboard",
                    "parentMenuId": null,
                    "roles": "0",
                    "menus": []
                },
                {
                    "menuId": 2,
                    "text": "Contact",
                    "iconClass": "fa fa-laptop",
                    "breadcrumbs": "Contact",
                    "href": "#!/operation/customerContact",
                    "parentMenuId": null,
                    "roles": "0,208",
                    "menus": []
                },
                {
                    "menuId": 3,
                    "text": "Contract",
                    "iconClass": "fa fa-pencil-square-o",
                    "breadcrumbs": "Contract",
                    "href": null,
                    "parentMenuId": null,
                    "roles": "0,208",
                    "menus": [
                        {
                            "menuId": 31,
                            "text": "Contract Capture",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Contract - Contract Capture",
                            "href": "#!/operation/contractRate",
                            "parentMenuId": 3,
                            "roles": "0,208",
                            "menus": []
                        },
                        {
                            "menuId": 32,
                            "text": "Rate Capture",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Contract - Rate Capture",
                            "href": "#!/operation/rateCapture",
                            "parentMenuId": 3,
                            "roles": "0,208",
                            "menus": []
                        }
                        
                    ]
                },
                {
                    "menuId": 4,
                    "text": "Shipment",
                    "iconClass": "fa fa-dashboard",
                    "breadcrumbs": "Shipment",
                    "href": "#!/shipment",
                    "parentMenuId": null,
                    "roles": "210,211,212,213,214,215,216,217,218,219,220,221",
                    "menus": [
                        {
                            "menuId": 41,
                            "text": "Inquiry",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Shipment - Inquiry",
                            "href": "#!/operation/enquiry",
                            "parentMenuId": 4,
                            "roles": "210",
                            "menus": []
                        },
                        {
                            "menuId": 42,
                            "text": "Quotation",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Shipment - Quotation",
                            "href": null,
                            "parentMenuId": 4,
                            "roles": "211,212,213",
                            "menus": [
                                {
                                    "menuId": 421,
                                    "text": "Create Quotation",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Quotation - Create Quotation",
                                    "href": "#!/operation/quotation",
                                    "parentMenuId": 42,
                                    "roles": "211",
                                    "menus": []
                                },
                                {
                                    "menuId": 422,
                                    "text": "Pending Shipper Approval",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Quotation - Pending Shipper Approval",
                                    "href": "#!/operation/quotationApproval",
                                    "parentMenuId": 42,
                                    "roles": "212",
                                    "menus": []
                                },
                                {
                                    "menuId": 423,
                                    "text": "Sent for Booking",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Quotation - Sent for Booking",
                                    "href": "#!/operation/quotationBooking",
                                    "parentMenuId": 42,
                                    "roles": "213",
                                    "menus": []
                                }

                            ]
                        },
                        {
                            "menuId": 43,
                            "text": "Booking",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Shipment - Booking",
                            "href": null,
                            "parentMenuId": 4,
                            "roles": "214,215,216,217,218,219,220,221",
                            "menus": [
                               {
                                   "menuId": 431,
                                   "text": "Booking To Do",
                                   "iconClass": "fa fa-circle-o",
                                   "breadcrumbs": "Shipment - Booking - Booking To Do",
                                   "href": "#!/operation/booking",
                                   "parentMenuId": 43,
                                   "roles": "214",
                                   "menus": []
                               },
                               {
                                   "menuId": 432,
                                   "text": "Waiting for Line Confirmation",
                                   "iconClass": "fa fa-circle-o",
                                   "breadcrumbs": "Shipment - Booking - Waiting for Line Confirmation",
                                   "href": "#!/operation/bookingDocument",
                                   "parentMenuId": 43,
                                   "roles": "218",
                                   "menus": []
                               },
                               {
                                   "menuId": 433,
                                   "text": "Booking Conf Ready To Send",
                                   "iconClass": "fa fa-circle-o",
                                   "breadcrumbs": "Shipment - Booking - Booking Conf Ready To Send",
                                   "href": "#!/operation/bookingConfReadyToSend",
                                   "parentMenuId": 43,
                                   "roles": "215",
                                   "menus": []
                               },
                               {
                                   "menuId": 434,
                                   "text": "Awaited Shipper Confirmation",
                                   "iconClass": "fa fa-circle-o",
                                   "breadcrumbs": "Shipment - Booking - Awaited Shipper Confirmation",
                                   "href": "#!/operation/bookingShipperConfirmation",
                                   "parentMenuId": 43,
                                   "roles": "220",
                                   "menus": []
                               },
                               {
                                   "menuId": 435,
                                   "text": "Edit/Rollover",
                                   "iconClass": "fa fa-circle-o",
                                   "breadcrumbs": "Shipment - Booking - Edit/Rollover",
                                   "href": "#!/operation/bookingSpace",
                                   "parentMenuId": 43,
                                   "roles": "215",
                                   "menus": []
                               },
                               {
                                   "menuId": 436,
                                   "text": "Capture Expenses",
                                   "iconClass": "fa fa-circle-o",
                                   "breadcrumbs": "Shipment - Booking - Capture Expenses",
                                   "href": "#!/operation/bookingCaptureExpenses",
                                   "parentMenuId": 43,
                                   "roles": "216",
                                   "menus": []
                               },
                               
                               
                               {
                                   "menuId": 437,
                                   "text": "Rollover Records",
                                   "iconClass": "fa fa-circle-o",
                                   "breadcrumbs": "Shipment - Booking - Rollover Records",
                                   "href": "#!/operation/bookingRollover",
                                   "parentMenuId": 43,
                                   "roles": "219",
                                   "menus": []
                               }
                            ]
                        },
                        {
                            "menuId": 44,
                            "text": "Trucking",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Shipment - Trucking",
                            "href": null,
                            "parentMenuId": 4,
                            "roles": "210",
                            "menus": [
                                {
                                    "menuId": 441,
                                    "text": "Finalize Transportation",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Trucking - Finalize Transportation",
                                    "href": "#!/operation/finalizedTransportation",
                                    "parentMenuId": 44,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 443,
                                    "text": "Dock Receipt",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Trucking - Dock Receipt",
                                    "href": "#!/document/dockReceipt",
                                    "parentMenuId": 44,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 442,
                                    "text": "Pending Movement",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Trucking - Pending Movement",
                                    "href": "#!/operation/pendingMovement",
                                    "parentMenuId": 44,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 443,
                                    "text": "Capture Container & Seal No",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Trucking - Capture Container & Seal No",
                                    "href": "#!/operation/bookingCaptureContainer",
                                    "parentMenuId": 44,
                                    "roles": "217",
                                    "menus": []
                                }
                            ]
                        },
                        {
                            "menuId": 45,
                            "text": "Documentation",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Shipment - Documentation",
                            "href": null,
                            "parentMenuId": 4,
                            "roles": "210",
                            "menus": [
                                {
                                    "menuId": 451,
                                    "text": "Pending Shipping Instruction",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Documentation - Pending Shipping Instruction",
                                    "href": "#!/operation/documentation1",
                                    "parentMenuId": 45,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 452,
                                    "text": "Pending filing of SED",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Documentation - Pending filing of SED",
                                    "href": "#!/operation/documentation2",
                                    "parentMenuId": 45,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 453,
                                    "text": "Pending B/L instruction Customer",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Documentation - Pending B/L instruction Customer",
                                    "href": "#!/operation/documentation3",
                                    "parentMenuId": 45,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 454,
                                    "text": "Pending B/L instruction Line",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Documentation - Pending B/L instruction Line",
                                    "href": "#!/operation/documentation4",
                                    "parentMenuId": 45,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 455,
                                    "text": "Pending Draft B/L Line",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Documentation - Pending Draft B/L Line",
                                    "href": "#!/operation/documentation5",
                                    "parentMenuId": 45,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 456,
                                    "text": "Pending Send Draft B/L Customer",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Documentation - Pending Send Draft B/L Customer",
                                    "href": "#!/operation/documentation6",
                                    "parentMenuId": 45,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 457,
                                    "text": "Pending Draft B/L Approval From Customer",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Documentation - Pending Draft B/L Approval From Customer",
                                    "href": "#!/operation/documentation7",
                                    "parentMenuId": 45,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 458,
                                    "text": "Pending to Send Approved Draft B/L to Line",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Documentation - Pending to Send Approved Draft B/L to Line",
                                    "href": "#!/operation/documentation8",
                                    "parentMenuId": 45,
                                    "roles": "217",
                                    "menus": []
                                },
                                {
                                    "menuId": 459,
                                    "text": "B/L Release Awaited From Line",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Shipment - Documentation - B/L Release Awaited From Line",
                                    "href": "#!/operation/documentation9",
                                    "parentMenuId": 45,
                                    "roles": "217",
                                    "menus": []
                                },
                            ]
                        }
                    ]
                },
                {
                    "menuId": 5,
                    "text": "Document",
                    "iconClass": "fa fa-pencil-square-o",
                    "breadcrumbs": "Document",
                    "href": null,
                    "parentMenuId": null,
                    "roles": "501,502,503,504",
                    "menus": [
                        
                        {
                            "menuId": 52,
                            "text": "HBL",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Document - HBL",
                            "href": "#!/document/hbl",
                            "parentMenuId": 5,
                            "roles": "502",
                            "menus": []
                        },
                        {
                            "menuId": 53,
                            "text": "MBL",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Document - MBL",
                            "href": "#!/document/mbl",
                            "parentMenuId": 5,
                            "roles": "503",
                            "menus": []
                        },
                        {
                            "menuId": 54,
                            "text": "CO",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Document - CO",
                            "href": "#!/document/co",
                            "parentMenuId": 5,
                            "roles": "504",
                            "menus": []
                        }
                    ]
                },
                {
                    "menuId": 6,
                    "text": "Data Management",
                    "iconClass": "fa fa-files-o",
                    "breadcrumbs": "Data Management",
                    "href": null,
                    "parentMenuId": null,
                    "roles": "296,297,298,304,299,300,301,302,303,290,291,292,293,294,305,306,307",
                    "menus": [
                        {
                            "menuId": 61,
                            "text": "Geographical",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Data Management - Geographical",
                            "href": null,
                            "parentMenuId": 6,
                            "roles": "296,297,298,304,299,300,301,302,303",
                            "menus": [
                                {
                                    "menuId": 611,
                                    "text": "Region/Continent",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Geographical - Region/Continent",
                                    "href": "#!/datamanagement/continent",
                                    "parentMenuId": 61,
                                    "roles": "296",
                                    "menus": []
                                },
                                {
                                    "menuId": 612,
                                    "text": "Country",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Geographical - Country",
                                    "href": "#!/datamanagement/siplcountry",
                                    "parentMenuId": 61,
                                    "roles": "297",
                                    "menus": []
                                },
                                {
                                    "menuId": 613,
                                    "text": "State",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Geographical - State",
                                    "href": "#!/datamanagement/lgvwstate",
                                    "parentMenuId": 61,
                                    "roles": "298",
                                    "menus": []
                                },
                                {
                                    "menuId": 614,
                                    "text": "Port Group",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Geographical - Port Group",
                                    "href": "#!/datamanagement/portgroup",
                                    "parentMenuId": 61,
                                    "roles": "304",
                                    "menus": []
                                },
                                {
                                    "menuId": 615,
                                    "text": "City",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Geographical - City",
                                    "href": "#!/datamanagement/lgvwcity",
                                    "parentMenuId": 61,
                                    "roles": "299",
                                    "menus": []
                                },
                                {
                                    "menuId": 616,
                                    "text": "Rail Ramp",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Geographical - Rail Ramp",
                                    "href": "#!/datamanagement/railramp",
                                    "parentMenuId": 61,
                                    "roles": "300",
                                    "menus": []
                                },
                                {
                                    "menuId": 617,
                                    "text": "Destination Terminal",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Geographical - Destination Terminal",
                                    "href": "#!/datamanagement/terminal",
                                    "parentMenuId": 61,
                                    "roles": "301",
                                    "menus": []
                                },
                                {
                                    "menuId": 618,
                                    "text": "SurchargeGroup",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Geographical - SurchargeGroup",
                                    "href": "#!/datamanagement/surchargegroup",
                                    "parentMenuId": 61,
                                    "roles": "303",
                                    "menus": []
                                },
                                {
                                    "menuId": 619,
                                    "text": "Port",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Geographical - Port",
                                    "href": "#!/datamanagement/lgvwport",
                                    "parentMenuId": 61,
                                    "roles": "302",
                                    "menus": []
                                }
                            ]
                        },
                        {
                            "menuId": 62,
                            "text": "Configurator",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Data Management - Configurator",
                            "href": null,
                            "parentMenuId": 6,
                            "roles": "290,291,292,293,294,305,306,307",
                            "menus": [
                                {
                                    "menuId": 621,
                                    "text": "Contact Category",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Configurator - Contact Category",
                                    "href": "#!/datamanagement/contactcategory",
                                    "parentMenuId": 62,
                                    "roles": "290",
                                    "menus": []
                                },                               
                                {
                                    "menuId": 622,
                                    "text": "Commodity",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Configurator - Commodity",
                                    "href": "#!/datamanagement/commodity",
                                    "parentMenuId": 62,
                                    "roles": "291",
                                    "menus": []
                                },
                                {
                                    "menuId": 623,
                                    "text": "Container Type",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Configurator - Container Type",
                                    "href": "#!/datamanagement/containertype",
                                    "parentMenuId": 62,
                                    "roles": "292",
                                    "menus": []
                                },
                                {
                                    "menuId": 624,
                                    "text": "Load Type",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Configurator - Load Type",
                                    "href": "#!/datamanagement/loadtype",
                                    "parentMenuId": 62,
                                    "roles": "293",
                                    "menus": []
                                },
                                {
                                    "menuId": 625,
                                    "text": "Trade Service",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Data Management - Configurator - Trade Service",
                                    "href": "#!/datamanagement/tradeservice",
                                    "parentMenuId": 62,
                                    "roles": "294",
                                    "menus": []
                                },
                                 {
                                     "menuId": 626,
                                     "text": "Create Alias",
                                     "iconClass": "fa fa-circle-o",
                                     "breadcrumbs": "Data Management - Configurator - Create Alias",
                                     "href": "#!/datamanagement/lgvwalias",
                                     "parentMenuId": 62,
                                     "roles": "305",
                                     "menus": []
                                 },
                                 {
                                     "menuId": 627,
                                     "text": "Account Category",
                                     "iconClass": "fa fa-circle-o",
                                     "breadcrumbs": "Data Management - Configurator - Account Category",
                                     "href": "#!/datamanagement/lgacctcategory",
                                     "parentMenuId": 62,
                                     "roles": "306",
                                     "menus": []
                                 },
                                 {
                                     "menuId": 628,
                                     "text": "SP FEE Category",
                                     "iconClass": "fa fa-circle-o",
                                     "breadcrumbs": "Data Management - Configurator - SP FEE Category",
                                     "href": "#!/datamanagement/lgspfeecategory",
                                     "parentMenuId": 62,
                                     "roles": "307",
                                     "menus": []
                                 }
                                 
                            ]
                        }
                    ]
                },
                {
                    "menuId": 7,
                    "text": "Administration",
                    "iconClass": "fa fa-th",
                    "breadcrumbs": "Administration",
                    "href": null,
                    "parentMenuId": null,
                    "roles": "201,202,203,204,205,101,102,103,104",
                    "menus": [
                        {
                            "menuId": 71,
                            "text": "System Entities",
                            "iconClass": "fa fa-circle",
                            "breadcrumbs": "Administration - System Entities",
                            "href": null,
                            "parentMenuId": 7,
                            "roles": "201,202,203,204,205",
                            "menus": [
                                {
                                    "menuId": 711,
                                    "text": "Country",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Administration - System Entities - Country",
                                    "href": "#!/setup/country",
                                    "parentMenuId": 71,
                                    "roles": "201",
                                    "menus": []
                                },
                                {
                                    "menuId": 712,
                                    "text": "State",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Administration - System Entities - State",
                                    "href": "#!/setup/state",
                                    "parentMenuId": 71,
                                    "roles": "202",
                                    "menus": []
                                },
                                {
                                    "menuId": 713,
                                    "text": "Department",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Administration - System Entities - Department",
                                    "href": "#!/setup/department",
                                    "parentMenuId": 71,
                                    "roles": "203",
                                    "menus": []
                                },
                                {
                                    "menuId": 714,
                                    "text": "Title",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Administration - System Entities - Title",
                                    "href": "#!/setup/title",
                                    "parentMenuId": 71,
                                    "roles": "204",
                                    "menus": []
                                },
                                {
                                    "menuId": 715,
                                    "text": "Timezone",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Administration - System Entities - Timezone",
                                    "href": "#!/setup/timezone",
                                    "parentMenuId": 71,
                                    "roles": "205",
                                    "menus": []
                                }
                            ]
                        },
                        {
                            "menuId": 72,
                            "text": "Security Entities",
                            "iconClass": "fa fa-circle",
                            "breadcrumbs": "Administration - Security Entities",
                            "href": null,
                            "parentMenuId": 7,
                            "roles": "101,102,103,104",
                            "menus": [
                                {
                                    "menuId": 721,
                                    "text": "Module Type",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Administration - Security Entities - Module Type",
                                    "href": "#!/security/moduleType",
                                    "parentMenuId": 72,
                                    "roles": "101",
                                    "menus": []
                                },
                                {
                                    "menuId": 722,
                                    "text": "Module",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Administration - Security Entities - Module",
                                    "href": "#!/security/module",
                                    "parentMenuId": 72,
                                    "roles": "102",
                                    "menus": []
                                },
                                {
                                    "menuId": 723,
                                    "text": "Action",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Administration - Security Entities - Action",
                                    "href": "#!/security/action",
                                    "parentMenuId": 72,
                                    "roles": "103",
                                    "menus": []
                                },
                                {
                                    "menuId": 724,
                                    "text": "Role",
                                    "iconClass": "fa fa-circle-o",
                                    "breadcrumbs": "Administration - Security Entities - Role",
                                    "href": "#!/security/role",
                                    "parentMenuId": 72,
                                    "roles": "104",
                                    "menus": []
                                }
                            ]
                        },
                        {
                            "menuId": 73,
                            "text": "Unit",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Administration - Unit",
                            "href": "#!/security/site",
                            "parentMenuId": 7,
                            "roles": "106",
                            "menus": []
                        },
                        {
                            "menuId": 76,
                            "text": "Customer Unit",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Administration - Security Entities - Customer Unit",
                            "href": "#!/security/customerUnit",
                            "parentMenuId": 7,
                            "roles": "110",
                            "menus": []
                        },
                        {
                            "menuId": 74,
                            "text": "User",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Administration - User",
                            "href": "#!/security/user",
                            "parentMenuId": 7,
                            "roles": "108",
                            "menus": []
                        },
                        {
                             "menuId": 76,
                             "text": "Vendor",
                             "iconClass": "fa fa-circle-o",
                             "breadcrumbs": "Data Management - Configurator - Vendor",
                             "href": "#!/operation/vendorCustomerContact",
                             "parentMenuId": 7,
                             "roles": "101",
                             "menus": []
                         },
                    ]
                },
                {
                    "menuId": 5,
                    "text": "Report",
                    "iconClass": "fa fa-pie-chart",
                    "breadcrumbs": "Report",
                    "href": null,
                    "parentMenuId": null,
                    "roles": "401,402,403,405,406",
                    "menus": [
                        {
                            "menuId": 51,
                            "text": "Enquiry",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Report - Enquiry",
                            "href": "#!/report/enquiryReport",
                            "parentMenuId": 5,
                            "roles": "401",
                            "menus": []
                        },
                        {
                            "menuId": 52,
                            "text": "Quotation",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Report - Quotation",
                            "href": "#!/report/quotationReport",
                            "parentMenuId": 5,
                            "roles": "402",
                            "menus": []
                        },
                        {
                            "menuId": 57,
                            "text": "Export Register Report",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Report - Export Register Report",
                            "href": "#!/report/exportRegisterReport",
                            "parentMenuId": 5,
                            "roles": "401",
                            "menus": []
                        },
                        {
                            "menuId": 58,
                            "text": "Expenses Report",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Report - Expenses Report",
                            "href": "#!/report/expensesReport",
                            "parentMenuId": 5,
                            "roles": "401",
                            "menus": []
                        },
                        {
                            "menuId": 61,
                            "text": "Region Wise Container Report",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Report - Region Wise Container Report",
                            "href": "#!/report/regionWiseContainerReport",
                            "parentMenuId": 5,
                            "roles": "401",
                            "menus": []
                        },
                        {
                            "menuId": 59,
                            "text": "Process Invoice Report",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Report - Process Invoice Report",
                            "href": "#!/report/processInvoiceReport",
                            "parentMenuId": 5,
                            "roles": "401",
                            "menus": []
                        },
                        {
                            "menuId": 60,
                            "text": "Activity Due Report",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Report - Activity Due Report",
                            "href": "#!/report/activityDueReport",
                            "parentMenuId": 5,
                            "roles": "401",
                            "menus": []
                        },
                        {
                            "menuId": 56,
                            "text": "SP MCS",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Report - SP MCS",
                            "href": "#!/report/mcsReport",
                            "parentMenuId": 5,
                            "roles": "406",
                            "menus": []
                        },
                        {
                            "menuId": 53,
                            "text": "Booking",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Report - Booking",
                            "href": "#!/report/bookingReport",
                            "parentMenuId": 5,
                            "roles": "403",
                            "menus": []
                        },
                        {
                            "menuId": 55,
                            "text": "Miami Booking Status",
                            "iconClass": "fa fa-circle-o",
                            "breadcrumbs": "Report - Miami Booking Status",
                            "href": "#!/report/bookingStatusReport",
                            "parentMenuId": 5,
                            "roles": "405",
                            "menus": []
                        }
                        
                        
                    ]
                }
            ];
        };

        //#endregion

        //#region Dashboard Methods

        $scope.performSubAction = function (source, entity) {

            var action = source.currentTarget.attributes["action"].value;
            var moduleId = source.currentTarget.attributes["module-id"].value;
            var entityId = source.currentTarget.attributes["entity-id"].value;
            var module = "dashboard";

            switch (action) {
                case "redirectTo":
                    $scope.redirectTo(module, entityId);
                    break;
                default:
                    //TODO
                    break;
            }
        };

        //#endregion

        //#region Public Methods

        var proceedLogout = function () {
            Idle.unwatch();
            authService.logOut();
            $location.path("/login").search({ logout: true });;
            $scope.userSites = [];
            $scope.selectedSite = {};
            $scope.userInfo = {};
            $scope.menus = [];
        };

        $scope.logOut = function (onClick) {
            if(onClick){
                $uibModal.open({
                    animation: false,
                    backdrop: "static",
                    keyboard: false,
                    templateUrl: 'app/views/shared/confirm.html',
                    controller: ['$scope', '$uibModalInstance', 'data', function ($scope, $uibModalInstance, data) {                        
                        $scope.title = data.title;
                        $scope.message = data.message;
                        $scope.ok = function () {
                            $uibModalInstance.close(true);
                        };
                        $scope.close = function () {
                            $uibModalInstance.dismiss();
                        };
                     }],
                     size: "md",
                     resolve: {
                        data: function () {
                            return {
                                "title": "Warning",
                                "message" : "Are you sure you want to logout the application?"
                            };
                        }
                     }
                }).result.then(function () {
                    proceedLogout();
                }, function () {
                    
                })
            }else{
                proceedLogout();
            }
        };
        $scope.customMenu = {
            selectedMenu: '',
            showChildMenu : false
        };
        $scope.customMenu.menuObj = appConstants.mainMenus;
        $scope.mainMenuClick = function (mainMenu, menuID, showChildMenu) {
            $scope.customMenu.selectedMenu = mainMenu;
            $scope.customMenu.showChildMenu = showChildMenu;
            $scope.breadcrumbs = [];
            for (var i = 0; i <$scope.menus.length; i++) {
                if (menuID === $scope.menus[i].menuId) {
                    $scope.menus[i].isOpen = true;
                    $scope.breadcrumbs.push($scope.menus[i].text);
                    for (var j = 0; j < $scope.menus[i].menus.length; j++) {
                        if ($scope.menus[i].menus[j].text.toLowerCase() === mainMenu.toLowerCase()) {
                            $scope.menus[i].menus[j].isOpen = true;
                            $scope.breadcrumbs.push($scope.menus[i].menus[j].text);
                        } else {
                            $scope.menus[i].menus[j].isOpen = false;
                        }
                    }
                } else {
                    $scope.menus[i].isOpen = false;
                }
            }
        };
        $scope.setHeader = function (menu) {
            if (menu && menu.href) {
                if ($scope.pageTitle === menu.text) {
                    $route.reload();
                } else {
                    $scope.pageTitle = menu.text;
                    $scope.breadcrumbs = menu.breadcrumbs.split("-");
                }
            } else if(menu && !menu.menus.length){
                $scope.pageTitle = "Dashboard";
                $scope.breadcrumbs = ["Dashboard"];
            } else if (menu && !menu.href && (menu.text.toLowerCase() == 'contract' || menu.text.toLowerCase() == 'document' || menu.text.toLowerCase() == 'report' || menu.text.toLowerCase() == 'administration' || menu.text.toLowerCase() == 'data management')) {
                $scope.breadcrumbs = [menu.text];
                $scope.pageTitle = menu.text;
                $location.path("/common");
            } else if (menu && menu.menus.length) {
                $scope.breadcrumbs.push(menu.text);
            }
            $scope.customMenu.showChildMenu = (menu.parentMenuId && menu.menus && menu.menus.length > 0);
            $scope.customMenu.selectedMenu = menu.text.toLowerCase();
        };

        $scope.changeSite = function (site) {

            $scope.selectedSite = site;
            $scope.redirectTo("dashboard");

            fetchingMenu($scope.selectedSite.siteId);
        };

        $scope.redirectTo = function (target, id, parentID, firstChild, index) {

            //if (target == "home") {
            //    $scope.pageTitle = "Home";
            //    $scope.breadcrumbs = ["Home"];
            //    $location.path("/home");
            //}
            if (id === true) {
                for (var i = 0; i < $scope.menus.length; i++) {
                    if (parentID === $scope.menus[i].menuId) {
                        //$scope.breadcrumbs.push($scope.menus[i].menu);
                        for (var j = 0; j < $scope.menus[i].menus.length; j++) {
                            $scope.menus[i].menus[j].isOpen = ($scope.menus[i].menus[j].text.toLowerCase() === firstChild || firstChild === j);
                            if ($scope.menus[i].menus[j].isOpen) {
                                $scope.breadcrumbs.push($scope.menus[i].menus[j].menu);
                                for (var k = 0; k < $scope.menus[i].menus[j].menus.length; k++) {
                                    $scope.menus[i].menus[j].menus[k].isOpen = (k == index);
                                }
                            }
                        }
                    } 
                }
                $location.path(target);
                return;
            }
            if (target == "dashboard") {
                $scope.pageTitle = "Dashboard";
                $scope.breadcrumbs = ["Dashboard"];
                $location.path("/home/dashboard");
            }
            else if (target == "profile") {
                $scope.pageTitle = "My Profile";
                $scope.breadcrumbs = ["My Profile"];
                $location.path("/security/profile");
            }
            else if (target == "enquiry") {
                $scope.pageTitle = "Enquiry";
                $scope.breadcrumbs = ["Shipment", "Enquiry"];
                $location.path("/operation/enquiry");
            }
            else if (target == "quotation") {
                $scope.pageTitle = "Create Quotation";
                $scope.breadcrumbs = ["Shipment", "Quotation", "Create Quotation"];
                $location.path("/operation/quotation");
            }
            else if (target == "booking") {
                $scope.pageTitle = "Booking To Do";
                $scope.breadcrumbs = ["Shipment", "Booking", "Booking To Do"];
                $location.path("/operation/booking");
            }
            else if (target == "dockReceipt") {
                $scope.pageTitle = "Dock Receipt";
                $scope.breadcrumbs = ["Document", "Dock Receipt"];
                $location.path("/document/dockReceipt");
            }
        };
        
        $scope.refreshSession = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: 'refreshSession.tpl.html',
                controller: function ($scope, $uibModalInstance, requestData) {
                    $scope.performAction = function (action) {
                        $uibModalInstance.close(action);
                    };
                },
                size: "md",
                resolve: {
                    requestData: function () {
                        return {
                            userInfo: $scope.userInfo
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output == "refresh") {
                        authService.refreshToken().then(
                            function (output) {
                                $scope.redirectTo("dashboard");
                                //$route.reload();
                                $window.location.reload();
                            },
                            function (output) {
                                ngNotifier.logError(output);
                            });
                    }
                    else if (output == "logout") {
                        $scope.logOut();
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.changeSite = function (siteId) {

            $scope.selectedSiteId = siteId;

            var authData = localStorageService.get("authData");
            authData.selectedSiteId = $scope.selectedSiteId;
            localStorageService.set("authData", authData);

            //$route.reload();
            $scope.$emit("initMainPage", null);
            $scope.pageTitle = "Dashboard";
            $scope.breadcrumbs = ["Dashboard"];
            $location.path("/home/dashboard");
            $route.reload();
        };

        $scope.arrayBufferToBase64 = function (buffer) {
            var binary = '';
            var bytes = new Uint8Array(buffer);
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            return window.btoa(binary);
        }

        //#endregion

        //#region Init Methods

        var isRefresh = false;
        document.onkeydown = function (e) {
            var keycode;
            if (window.event) {
                keycode = window.event.keyCode;
            }
            else if (e) {
                keycode = e.which;
            }
            if (keycode == 116) {
                isRefresh = true;
            }
        };

        $window.onbeforeunload = function (event) {
            if ($location.$$path !== '/login') {
                return "Are you sure want to exit the application.";
                //todo on click ok this method to trigger.
                if (!isRefresh) {
                    $scope.logOut();
                }
            }
        };

        $scope.$on('initMainPage', function (event, args) {
            initMainPage();
            Idle.watch();
        });

        (function () {
            initMainPage();
        })();

        function closeModals() {
            if ($scope.warning) {
                $scope.warning.close();
                $scope.warning = null;
            }

            if ($scope.timedout) {
                $scope.timedout.close();
                $scope.timedout = null;
            }
        }

        $scope.$on('IdleStart', function () {
            closeModals();
            $scope.warning = $uibModal.open({
                templateUrl: 'warning-dialog.html'
            });
        });

        $scope.$on('IdleEnd', function () {
            closeModals();
        });

        $scope.$on('IdleTimeout', function () {
            closeModals();
            Idle.unwatch();
            $scope.logOut();
        });
        //#endregion
    };

    controller.$inject = injectParams;

    app.controller("indexController", controller);

});
