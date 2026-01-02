"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "bookingSpaceService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.bookingSpace;
        $scope.tabs = appUrl.bookingSpace.tabs;
        $scope.$parent.pageTitle = "Edit/Rollover";
        $scope.$parent.breadcrumbs = ["Shipment", "Booking", "Edit/Rollover"];
        //$scope.departments = [];
        $scope.enquiryID;
        $scope.deleteRemarks = "";
        $scope.nextActionDueDate = "";
        $scope.isComplete = false;
        $scope.airServiceList = [];
        $scope.breakBulkServiceList = [];
        $scope.containerServiceList = [];
        $scope.lclServiceList = [];
        $scope.roroServiceList = [];
        $scope.searchResult = [];
        
        $scope.isHaz = false;
        $scope.containerCategoryList = [];
        $scope.containerSizesList = [];
        $scope.actionRemarksList = [];
        $scope.isInvalidData = false;
        $scope.isCarrierDisabled = true;
        //$scope.isValidFileNo = true;
        var lastAction = "";
        $scope.isGalBookingStatusVisible = true;
        $scope.documentationStatusList = [];
        $scope.carrierChargesList = [];
        $scope.isCarrierChargesVisible = true;
        //#endregion

        //#region Lookup

        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "SIPLContact") {
                $scope.entity.customerID = output.data[0].contactID;
                $scope.entity.customerAddress = output.data[0].address;
            }
            else if (lookup == "exporter") {
                $scope.entity.exporterID = output.data[0].contactID;
                $scope.entity.exporterAddress = output.data[0].address;
            }
            else if (lookup == "pickupLocation") {
                var pickupaddress="";
                $scope.entity.pickupLocationId = output.data[0].contactID;
                if(output.data[0].cityName !=null){pickupaddress = output.data[0].cityName;
                }else{pickupaddress ="";}
                if(output.data[0].stateName !=null){ pickupaddress = pickupaddress + "," + output.data[0].stateName;
                }else{pickupaddress = pickupaddress+","; }
                $scope.entity.pickupLocationAddress = pickupaddress;
            }
            else if (lookup == "returnLocation") {
                var returnaddress = "";
                $scope.entity.returnLocationId = output.data[0].contactID;
                if (output.data[0].cityName != null) {
                    returnaddress = output.data[0].cityName;
                } else { returnaddress = ""; }
                if (output.data[0].stateName != null) {
                    returnaddress = returnaddress + "," + output.data[0].stateName;
                } else { returnaddress = returnaddress + ","; }
                $scope.entity.returnLocationAddress = returnaddress;
            }
            else if (lookup == "consignedTo") {
                $scope.entity.consignedTo = output.data[0].contactID;
                $scope.entity.contactPerson = output.data[0].contactPerson;
            }
            else if (lookup == "truckCompanyName") {
                $scope.entity.truckCompanyName = output.data[0].companyName;
            }
            else if (lookup == "carrierBooking") {
                $scope.entity.carrierBooking = output.data[0].companyName;                
            }
            else if (lookup == "originCountry") {
                $scope.entity.originCountryID = output.data[0].countryId;
            }
            else if (lookup == "dischargeCountry") {
                $scope.entity.destinationCountryID = output.data[0].countryId;
            }
            else if (lookup == "originState") {
                $scope.entity.orignStateID = output.data[0].stateId;
                if ($scope.entity.originCountryID == null) {
                    $scope.entity.originCountryID = output.data[0].countryId;
                    $scope.entity.originCountry = output.data[0].countryName;
                }
            }
            else if (lookup == "dischargeState") {
                $scope.entity.destinationStateID = output.data[0].stateId;
                if ($scope.entity.destinationCountryID == null) {
                    $scope.entity.destinationCountryID = output.data[0].countryId;
                    $scope.entity.destinationCountry = output.data[0].countryName;
                }
            }
            else if (lookup == "originCity") {
                $scope.entity.originCityID = output.data[0].cityId;
            }
            else if (lookup == "dischargeCity") {
                $scope.entity.destinationCityID = output.data[0].cityId;
            }
            else if (lookup == "origin") {
                $scope.entity.originID = output.data[0].portId;
            }
            else if (lookup == "discharge") {
                $scope.entity.dischargeID = output.data[0].portId;
            }
            else if (lookup == "RailRamp") {
                $scope.entity.orgnRailRampId = output.data[0].railID;
            }
            else if (lookup == "Terminal") {
                $scope.entity.destnTerminalId = output.data[0].terminalid;
            }
        };

        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "SIPLContact") {
                $scope.entity.customerID = null;
                $scope.entity.address = "";
            }
            else if (lookup == "exporter") {
                $scope.entity.exporterID = null;
                $scope.entity.exporterAddress = "";
            }
            else if (lookup == "consignedTo") {
                $scope.entity.consignedTo = null;
                $scope.entity.contactPerson = "";
            }
            else if (lookup == "pickupLocation") {
                if ($scope.isInvalidData == true) {
                    $scope.entity.pickupLocationId = null;
                    $scope.entity.pickupLocationAddress = "";
                }
            }
            else if (lookup == "returnLocation") {
                if ($scope.isInvalidData == true) {
                    $scope.entity.returnLocationId = output.data[0].contactID;
                    $scope.entity.returnLocationAddress = output.data[0].cityName + "," + output.data[0].stateName;
                }
            }
            else if (lookup == "truckCompanyName") {
                $scope.entity.truckCompanyName = null;                
            }
            else if (lookup == "carrierBooking") {
                $scope.entity.carrierBooking = null;
            }
            else if (lookup == "originCountry") {
                $scope.entity.originCountryID = null;
            }
            else if (lookup == "dischargeCountry") {
                $scope.entity.destinationCountryID = null;
            }
            else if (lookup == "originState") {
                $scope.entity.orignStateID = null;
            }
            else if (lookup == "dischargeState") {
                $scope.entity.destinationStateID = null;
            }
            else if (lookup == "originCity") {
                $scope.entity.originCityID = null;
            }
            else if (lookup == "dischargeCity") {
                $scope.entity.destinationCityID = null;
            }
            else if (lookup == "originPort") {
                $scope.entity.originID = null;
            }
            else if (lookup == "dischargePort") {
                $scope.entity.dischargeID = null;
            }
            else if (lookup == "RailRamp") {
                $scope.entity.OrgnRailRampId = null;
            }
            else if (lookup == "Terminal") {
                $scope.entity.destnTerminalId = null;
            }
        };

        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {

            if (lookupModule == "SIPLContact" || lookupModule == "exporter" || lookupModule == "pickupLocation" || lookupModule == "returnLocation" ||lookupModule == "consignedTo" || lookupModule == "carrierBooking" || lookupModule == "originCountry" || lookupModule == "dischargeCountry" || lookupModule == "originState" || lookupModule == "dischargeState" || lookupModule == "originCity" || lookupModule == "dischargeCity" || lookupModule == "originPort" || lookupModule == "dischargePort" || lookupModule == "RailRamp" || lookupModule == "Terminal") {

                if (lookupModule == "SIPLContact" || lookupModule == "exporter" || lookupModule == "consignedTo" || lookupModule == "carrierBooking" || lookupModule == "RailRamp") {
                    if ($scope.entity[lookupField] == null || $scope.entity[lookupField] == "" || $scope.entity[lookupField] == undefined) {
                        $scope.clearLookups(source, lookupModule, lookupIndex);
                    }
                }
                else if ($scope.entity[lookupModule] == null || $scope.entity[lookupModule] == "") {
                    $scope.clearLookups(source, lookupModule, lookupIndex);
                }
            }
            if ($scope.isInvalidData == true) {
                $scope.clearLookups(source, lookupModule, lookupIndex);
            }
        };

        $scope.lookups = { siplDepartments: [], carriers: [], siplUsers: [], commodityTypes: [], commodities: [], miamiBookingStatus: [], LGVWPorts: [], containerSizes: [], containerCategories: [],documentationStatus: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
            $scope.fetchLookupData("siplUser", 0, "name", "siplUsers", null);
            $scope.fetchLookupData("commodityType", 0, "commodityType", "commodityTypes", null);
            $scope.fetchLookupData("Commodity", 0, "name", "commodities", null);
            $scope.fetchLookupData("siplBookingStatus", 0, "Status", "miamiBookingStatus", null);
            $scope.fetchLookupData("siplContact", 0, "companyName", "siplContact", null);
            $scope.fetchLookupData("LGVWPort", 0, "name", "lgvwPorts", null);
            $scope.getContainerCategories();
            $scope.getDocumentationStatus();
            $scope.getContainerSizes();
            $scope.getAllCarriers();
            
        };

        $scope.getDocumentationStatus = function () {
            var getdocumentstatus = entityService.getDocumentationStatus().then(
                function (output) {
                    $scope.lookups.documentationStatus = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }
        $scope.getContainerCategories = function () {
            var getCategories = entityService.getContainerCategories().then(
                function (output) {
                    $scope.lookups.containerCategories = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }

        $scope.getContainerSizes = function () {
            var getCategorySizes = entityService.getContainerSizes().then(
                function (output) {
                    $scope.lookups.containerSizes = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }

        $scope.updateIsHaz = function (isHazValue) {
            if (isHazValue) {
                $scope.isHaz = true;
                $scope.entity.commodityTypeID = 1;
                $scope.entity.commodityIds = [];
            }
            else {
                $scope.isHaz = false;
                $scope.entity.commodityTypeID = 2;
                $scope.entity.commodityIds = [];
            }
        };
        $scope.updateContainerCategoryList = function (modeOfService) {
            $scope.containerServiceList = [];
            $scope.containerCategoryList = [];
            $scope.containerSizesList = [];
            $scope.lookups.containerCategories.forEach(function (item) {
                if (item.modeOfServiceID == modeOfService) {
                    $scope.containerCategoryList.push(item);
                }
            });
        };
        $scope.UpdateContainerSizes = function (selectedcategoryItem) {
            $scope.containerSizesList = [];
            $scope.lookups.containerSizes.forEach(function (item) {
                if (item.containerCategoryID == selectedcategoryItem.categoryID) {
                    $scope.containerSizesList.push(item);
                }
            });
        };

        $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey == "miamiBookingStatus") { $scope.lookups.miamiBookingStatus.unshift({ "statusID": -1, "status": "-Select-" }); } //
        }
        $scope.getCurrentDate = function () {
            //fill enquiry default date
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            var today = mm + '/' + dd + '/' + yyyy;
            return today;
        }
        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":
                    $scope.entity.documentCommonID = 0;
                    $scope.entity.quotationID = 0;
                    $scope.entity.quotationNo = "Direct";
                    $scope.entity.enquiryID = 0;
                    $scope.entity.enquiryNo = "Direct";
                    $scope.entity.bookingCategory = "1";
                    $scope.entity.modeOfService = "2";
                    $scope.updateContainerCategoryList($scope.entity.modeOfService)
                    $scope.entity.pickupCategory = "0";
                    $scope.entity.galBookingType = "1";
                    $scope.entity.commodityTypeID = 2;
                    $scope.entity.isHaz = false;
                    $scope.updateIsHaz($scope.entity.isHaz);
                    $scope.isCarrierDisabled = false;
                    var currentDate = $scope.getCurrentDate();
                    $scope.entity.bookingReqtRecivedDate = currentDate;
                    $scope.entity.bookingDate = currentDate;
                    $scope.entity.etSailing = currentDate;
                    $scope.entity.eta = currentDate;
                    $scope.entity.ets = currentDate;
                    $scope.entity.erd = currentDate;
                    $scope.containerServiceList = [];
                    $scope.carrierChargesList = [];
                    $scope.isCarrierChargesVisible = false;
                    //$scope.getLatestBookingNo();
                    break;
                case "edit":
                    $scope.isCarrierDisabled = true;
                    $scope.isCarrierChargesVisible = true;
                    break;


            }
        };

        $scope.afterGetDetail = function (action) {
            if ($scope.entity.documentCommonID == 0) {
                $scope.entity.bookingCategory = "0";
                var currentDate = $scope.getCurrentDate();
                $scope.entity.bookingDate = currentDate;
                $scope.entity.etSailing = currentDate;
                $scope.entity.eta = currentDate;
                $scope.entity.ets = currentDate;
                $scope.entity.erd = currentDate;
            }
            if ($scope.entity.bookingCategory == 0)
            {
                $scope.entity.bookingCategory = "0";
            }
            else if ($scope.entity.bookingCategory == 1)
            {
                $scope.entity.bookingCategory = "1";
                $scope.entity.quotationID = 0;
                $scope.entity.quotationNo = "Direct";
                $scope.entity.enquiryID = 0;
                $scope.entity.enquiryNo = "Direct";
            }
            
            $scope.documentationStatusList = [];
            if ($scope.entity.dos_Id == 1) {
                $scope.lookups.documentationStatus.forEach(function (item) {
                    if (item.dos_Id == 1) {
                        $scope.documentationStatusList.push(item);
                    }
                })                
            }
            else {
                $scope.lookups.documentationStatus.forEach(function (item) {
                    if (item.dos_Id != 1) {
                        $scope.documentationStatusList.push(item);
                    }
                })
            }         
            

            $scope.calculateWeight('hazweight');
            $scope.calculateVolume('hazVolume');
        };

        $scope.getLatestBookingNo = function (depId) {
            var entites = {};
            entites.siteId = $scope.$parent.selectedSiteId;
            entites.departmentID = depId;

            var getBookingNo = entityService.getLatestBookingNo(entites).then(
                function (output) {
                    $scope.entity.fileNo = output.data.data.fileNo;

                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };
        $scope.validateBookingFileNo= function(fileNo){
            var getBookingNo = entityService.validateBookingFileNo(fileNo).then(
                function (output) {
                    if (output.data.count > 0)
                    {
                        
                        if (output.data.data[0] != undefined && output.data.data[0] != $scope.entity.documentCommonID) {
                            //$scope.isValidFileNo = false;
                            ngNotifier.error("System No is already exits, please enter unique system No");
                            return;
                        }
                        else {
                            //$scope.isValidFileNo = true;
                            return;
                        }
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }

        $scope.searchValues = function (viewValue, selectOption) {
            var resultItem = {};
            if (selectOption == "customer") {
                return $scope.callTypeahead(viewValue, 'SIPLContact', 'companyName', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.companyName;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
            var lookupModule;
            if (selectOption == "state") {
                lookupModule = "LGVWState";
            }
            if (selectOption == "city") {
                lookupModule = "LGVWCity";
            }
            if (selectOption == "origin" || selectOption == "discharge") {
                lookupModule = "SIPLCountry";
            }
            if (selectOption == "commodity") {
                lookupModule = "commodity";
            }
            if (selectOption == "state" || selectOption == "city" || selectOption == "origin" || selectOption == "discharge" || selectOption == "commodity") {
                return $scope.callTypeahead(viewValue, lookupModule, 'name', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.name;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
        };

        //#endregion
        $scope.enquiryTypes = [
                { optionValue: "0", optionName: "Select One" },
                { optionValue: "1", optionName: "By Email" },
                { optionValue: "2", optionName: "By Mail" },
                { optionValue: "3", optionName: "By Telecall" }
        ];
        $scope.pickupTypes = [
                { optionValue: "1", optionName: "SD/Port" },
                { optionValue: "2", optionName: "Port/Port" },
                { optionValue: "3", optionName: "Door/Door" },
                { optionValue: "4", optionName: "CFS/CFS" },
                { optionValue: "5", optionName: "RailRamp/Port" },
                { optionValue: "6", optionName: "CY/CY" },
                { optionValue: "7", optionName: "SD/CY" }
        ];
        $scope.sipl_MailMode = [
                { optionValue: 1, optionName: "Telephonic" },
                { optionValue: 2, optionName: "eMail" },
                { optionValue: 3, optionName: "Fax " }
        ];
        $scope.sipl_ContractBooking = [
                { optionValue: 1, optionName: "NotKnown" },
                { optionValue: 2, optionName: "Booked  as per contract" },
                { optionValue: 3, optionName: "Booked out side contract " }
        ];
        $scope.sipl_InvoiceStatus = [
                { optionValue: "1", optionName: "NOT READY FOR  INVOICING" },
                { optionValue: "2", optionName: "INVOICED" },
                { optionValue: "4", optionName: "VOID" },
                { optionValue: "5", optionName: "RECORD CANCELLED" },
                { optionValue: "6", optionName: "RECORD ROLLED OVER" },
                { optionValue: "7", optionName: "READY FOR INVOICING" }
        ];
        $scope.sipl_LoadType = [
                { optionValue: "1", optionName: "FCL" },
                { optionValue: "2", optionName: "LCL" },
                { optionValue: "3", optionName: "RORO" },
                { optionValue: "4", optionName: "TOTO" },
                { optionValue: "5", optionName: "BREAK BULK" },
                { optionValue: "7", optionName: "AIR" },
                { optionValue: "13", optionName: "SPECIAL CARGO" },
                { optionValue: "16", optionName: "Candle Consignment" }
        ];

        //$scope.entity.enquiryTypeID = 0;

        $scope.searchOptions = [
                { optionValue: "", optionName: "-All-" },
                { optionValue: "customer", optionName: "Customer" },
                { optionValue: "SystemRefNo", optionName: "System Ref No" },                
                { optionValue: "LineBookingNo", optionName: "Line Booking No" }
        ];

        $scope.filterBookings = function () {
            var dashboardOption = localStorageService.get("dashboardOption");
            $scope.selectOption = "companyName";
            $scope.searchBox = "";
            if (dashboardOption != null) {
                if (dashboardOption == 'BYTD') {
                    $scope.dashboardOption = "BYTD";
                }
                if (dashboardOption == 'BMTD') {
                    $scope.dashboardOption = "BMTD";
                }               

                localStorageService.remove("dashboardOption");
            }
            else {
                $scope.dashboardOption = "";
            }

        };

        $scope.filterBookings();

        $scope.selectOption = "companyName";
        $scope.searchBox = "";
        $scope.departmentID = 0;
        $scope.galBookingStatusID = 0;

        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            departmentID: 0,
            galBookingStatusID: $scope.galBookingStatusID,
            dashboardOption: $scope.dashboardOption
        };
        //#region Methods
        $scope.bookinglistTable = new NgTableParams(
       {
           page: 1,
           count: 10,
           sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")
       }, {
           getData: function (params) {
               var listParams = {
                   SiteId: $scope.$parent.selectedSiteId,
                   ModuleId: $scope.page.moduleId,
                   PageIndex: params.page(),
                   PageSize: params.count(),
                   Sort: JSON.stringify(params.sorting()),
                   Filter: JSON.stringify($scope.searchParam)
               };

               var dataitems = entityService.getDocumentList(listParams).then(
                   function (output) {
                       $scope.validateUser(output);
                       $scope.items = output.data.data;
                       params.total(output.data.count);
                   },
                   function (output) {
                       ngNotifier.showError($scope.authentication, output);
                   }
               );
           }
       });
        //#endregion
        $scope.getAllCarriers = function () {
            var getCurrencies = entityService.getAllCarriers().then(
                function (output) {
                    $scope.lookups.carriers = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }

        $scope.performBookingSearch = function (source, selectOption, searchBox, departmentValue) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox,
                departmentID: departmentValue,
                galBookingStatusID: 0,
                dashboardOption: ""
            };
            $scope.bookinglistTable.reload();
        };
              

        $scope.saveDocumentDetail = function (source, fromList) {

            //var actionType = source.currentTarget.attributes["actionType"].value;
            if ($scope.entity.departmentID == null) {
                ngNotifier.error("Please select Department");
                return;
            }
            if ($scope.entity.fileNo == null) {
                ngNotifier.error("Booking No. is Required");
                return;
            }            

            if ($scope.entity.pickupCategory == null ) {
                ngNotifier.error("Please select PickupType");
                return;
            }
            if ($scope.entity.galBookingType == null) {
                ngNotifier.error("Please select Miami BookingType");
                return;
            }
            if ($scope.entity.galBookingStatusID == null || $scope.entity.galBookingStatusID <= 0) {
                ngNotifier.error("Please select Miami Booking Status");
                return;
            }
            if ($scope.entity.companyName != null && $scope.entity.companyName != '' && $scope.entity.customerID == null) {
                ngNotifier.error("Please enter valid Company Name");
                return true;
            }

            if ($scope.entity.origin != null && $scope.entity.origin != '' && $scope.entity.originID == null) {
                ngNotifier.error("Please enter valid Port of Origin");
                return true;
            }
            if ($scope.entity.discharge != null && $scope.entity.discharge != '' && $scope.entity.dischargeID == null) {
                ngNotifier.error("Please enter valid Port of Discharge");
                return true;
            }
            if ($scope.entity.nextActionRemarks != null || $scope.entity.nextActionDueDate != null) {
                if ($scope.entity.nextActionRemarks.trim() != '' && $scope.entity.nextActionDueDate == '') {
                    ngNotifier.error("Please select Next Action Due date");
                    return;
                }
                if ($scope.entity.nextActionDueDate != '' && $scope.entity.nextActionRemarks.trim() == '') {
                    ngNotifier.error("Please Enter Next Action Remarks");
                    return;
                }
            }
            if ($scope.entity.pickupLocationAddress != null && $scope.entity.pickupLocationAddress != '' && $scope.entity.pickupLocationId == null) {
                ngNotifier.error("Please enter valid Pickup Location address");
                return true;
            }
            if ($scope.entity.returnLocationAddress != null && $scope.entity.returnLocationAddress != '' && $scope.entity.returnLocationId == null) {
                ngNotifier.error("Please enter valid Return Location address");
                return true;
            }
            if ($scope.entity.ets == null) {
                ngNotifier.error("Please select Estimated Time of Sailing");
                return true;
            }
            //if ($scope.entity.bookingDate == null) {
            //    ngNotifier.error("Please select Booking date");
            //    return;
            //}
            //if ($scope.entity.contractBookingID == null) {
            //    ngNotifier.error("NAAA Contract Type required");
            //    return;
            //}
            //if ($scope.entity.galBookingStatusID == '9' && $scope.entity.truckCompanyName == '') {
            //    ngNotifier.error("Inlad - Trucking Company is Required");
            //    return;
            //}
            //if ($scope.entity.galBookingStatusID == '9' && $scope.entity.pickupDate == '') {
            //    ngNotifier.error("Inland - Picked Up Date is Required");
            //    return;
            //}

            if ($scope.entity.noOfContainer == null) {
                ngNotifier.error("Please select No of Containers");
                return;
            }

            //Fill commodity
            $scope.fillEnquiryCommodityDetail();
            if ($scope.entity.commodityDTOList.length == 0) {
                ngNotifier.error("Please select Commodity");
                return;
            }
            //Fill FCL
            var totQty = 0;
            $scope.entity.enquiryContainerServiceDTOList = [];
            if ($scope.containerServiceList.length == 0) {
                ngNotifier.error("Please add Enquiry Container details");
                return;
            }
            else {
                $scope.entity.enquiryContainerServiceDTOList = $scope.containerServiceList;
                for (var i = 0; i < $scope.entity.enquiryContainerServiceDTOList.length; i++) {
                    totQty = totQty + parseInt($scope.entity.enquiryContainerServiceDTOList[i].quantity);
                }
                if (parseInt($scope.entity.noOfContainer) != totQty) {
                    ngNotifier.error("Total QTY is not equal to No Of Container");
                    return;
                }

            }

            //Fill AIR
            $scope.fillEnquiryAirDetail();
            //Fill Break Bulk
            $scope.fillEnquiryBreakBulkDetail();
            //Fill LCL
            $scope.fillEnquiryLCLDetail();
            //Fill RORO
            $scope.fillEnquiryRORODetail();
            if ($scope.entity.documentCommonID == null || $scope.entity.documentCommonID == 0) {
                $scope.entity.galBookingStatusID = 7;  //Pending for Confirmation from line
            }
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            $scope.entity.updatedBy = $scope.$parent.authentication.userId;
            entityService.saveDocumentDetail($scope.entity).then(
                function (output) {
                    if (output.data.messages[0] =="") {
                        $scope.entity = {};
                        $scope.airServiceList = [];
                        $scope.breakBulkServiceList = [];

                        $scope.lclServiceList = [];
                        $scope.roroServiceList = [];
                        $scope.containerServiceList = [];
                        $scope.bookinglistTable.reload();
                        $scope.goBack();
                    }
                    ngNotifier.show(output.data);
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                    $scope.editMode = false;
                    $scope.disabledInsert = true;
                    $scope.disabledUpdate = true;
                    $scope.requiredInsert = false;
                    $scope.requiredUpdate = false;
                });
        };

        $scope.fillEnquiryCommodityDetail = function () {
            var commodityItem = {};
            $scope.entity.commodityDTOList = [];

            if ($scope.entity.commodityIds != null) {
                for (var i = 0; i < $scope.entity.commodityIds.length; i++) {
                    commodityItem = {};
                    commodityItem.commodityId = $scope.entity.commodityIds[i];
                    $scope.entity.commodityDTOList.push(commodityItem);
                }
            }
        }

        $scope.fillEnquiryAirDetail = function () {
            $scope.entity.enquiryAIRServiceDTOList = [];
            if ($scope.entity.modeOfService == '7') {
                if ($scope.airServiceList.length > 0) {
                    $scope.entity.enquiryAIRServiceDTOList = $scope.airServiceList;
                }
            }
        }

        $scope.fillEnquiryBreakBulkDetail = function () {
            var breakBulkServiceItem = {};
            $scope.entity.enquiryBreakBulkServiceDTOList = [];
            if ($scope.entity.modeOfService == '5') {
                if ($scope.breakBulkServiceList.length > 0) {
                    $scope.entity.enquiryBreakBulkServiceDTOList = $scope.breakBulkServiceList;
                }
            }
        }

        $scope.fillEnquiryLCLDetail = function () {
            var lclServiceItem = {};
            $scope.entity.enquiryLCLServiceDTOList = [];
            if ($scope.entity.modeOfService == '2') {
                if ($scope.lclServiceList.length > 0) {
                    $scope.entity.enquiryLCLServiceDTOList = $scope.lclServiceList;
                }
            }
        }

        $scope.fillEnquiryRORODetail = function () {
            var roroServiceItem = {};
            $scope.entity.enquiryROROServiceDTOList = [];
            if ($scope.entity.modeOfService == '3') {
                if ($scope.roroServiceList.length > 0) {
                    $scope.entity.enquiryROROServiceDTOList = $scope.roroServiceList;
                }
            }
        }

        var initControls = function (action) {

            switch (action) {
                case "add":
                    $scope.editMode = true;
                    $scope.disabledInsert = false;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = true;
                    $scope.requiredUpdate = true;
                    break;
                case "copy":
                    $scope.editMode = true;
                    $scope.disabledInsert = false;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = true;
                    $scope.requiredUpdate = true;
                    break;
                case "edit":
                    $scope.editMode = true;
                    $scope.disabledInsert = true;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = false;
                    $scope.requiredUpdate = true;
                    break;
                case "rollOver":
                    $scope.editMode = true;
                    $scope.disabledInsert = true;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = false;
                    $scope.requiredUpdate = true;
                    break;
                default:
                    $scope.editMode = false;
                    $scope.disabledInsert = true;
                    $scope.disabledUpdate = true;
                    $scope.requiredInsert = false;
                    $scope.requiredUpdate = false;
                    break;
            }
        };

        var switchTab = function (title, action) {

            $scope.tabs.forEach(function (tab) {
                tab.active = false;
                tab.disabled = ((action === "add" || action === "copy" || action === "edit") && tab !== $scope.tabs[0]);
                if (tab.title === title) {
                    tab.active = true;
                }
            });
        };

        var copyEnquiry = function () {

            return true;
        }
        var deleteDocument = function (documentCommonID) {
            var entity = {};
            entity.documentCommonID = documentCommonID;            
            ngNotifier.confirm("Are you sure you want to DELETE Booking?", null, function () {                
                entityService.deleteBooking(entity).then(
                function (output) {
                    $scope.entity = {};
                    $scope.bookinglistTable.reload();
                    //$scope.goBack();
                    ngNotifier.show(output.data);
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });                       
            });

        };

        var cancelDocument = function (documentCommonID) {
            var entity = {};
            entity.documentCommonID = documentCommonID;
            ngNotifier.confirm("Are you sure you want to Cancel Booking?", null, function () {
                var modalInstance = $uibModal.open({
                    animation: false,
                    backdrop: "static",
                    keyboard: false,
                    size: "lg",
                    templateUrl: "app/components/operation/bookingSpace/cancelRemarks.html",
                    controller: function ($scope, $timeout, $uibModalInstance, requestData) {

                        $scope.select = function (action) {
                            //$scope.deleteRemarks = deleteRemarks.value;
                            if (action == 'delete' && ($scope.deleteRemarks == null || $scope.nextActionDueDate == null)) {
                                ngNotifier.error("Please Enter Delete Remarks & Next action date");
                                return;
                            }
                            var outputData = {}
                            outputData.remarks = $scope.deleteRemarks;
                            outputData.nextActionDueDate = $scope.nextActionDueDate;
                            outputData.action = action;
                            $uibModalInstance.close(outputData);
                        };
                    },
                    resolve: {
                        requestData: function () {
                            return {
                                deleteRemarks: $scope.deleteRemarks
                            };
                        }
                    }
                });

                modalInstance.result.then(
                    function (output) {
                        if (output.action == "delete") {
                            entity.remarks = output.remarks;
                            entity.nextActionDueDate = output.nextActionDueDate;
                            entityService.cancelBooking(entity).then(
                            function (output) {
                                $scope.entity = {};
                                $scope.bookinglistTable.reload();
                                $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });

                        }
                        else if (output == "close") {

                        }
                    },
                    function (output) {
                        ngNotifier.logError(output);
                    });

            });

        };

        var rollOverDocument = function (documentCommonID) {
            var entity = {};
            entity.documentCommonID = documentCommonID;
            ngNotifier.confirm("Are you sure you want to rollOver Booking?", null, function () {
                var modalInstance = $uibModal.open({
                    animation: false,
                    backdrop: "static",
                    keyboard: false,
                    size: "lg",
                    templateUrl: "app/components/operation/bookingSpace/cancelRemarks.html",
                    controller: function ($scope, $timeout, $uibModalInstance, requestData) {

                        $scope.select = function (action) {
                            //$scope.deleteRemarks = deleteRemarks.value;
                            if (action == 'delete' && ($scope.deleteRemarks == null || $scope.nextActionDueDate == null)) {
                                ngNotifier.error("Please Enter Delete Remarks & Next action date");
                                return;
                            }
                            var outputData = {}
                            outputData.remarks = $scope.deleteRemarks;
                            outputData.nextActionDueDate = $scope.nextActionDueDate;
                            outputData.action = action;
                            $uibModalInstance.close(outputData);
                        };
                    },
                    resolve: {
                        requestData: function () {
                            return {
                                deleteRemarks: $scope.deleteRemarks
                            };
                        }
                    }
                });

                modalInstance.result.then(
                    function (output) {
                        if (output.action == "delete") {
                            entity.remarks = output.remarks;
                            entity.nextActionDueDate = output.nextActionDueDate;
                            entityService.rollOverBooking(entity).then(
                            function (output) {                                
                                $scope.bookinglistTable.reload();                                
                                ngNotifier.show(output.data);
                                $scope.entity = {};
                                
                                
                                $scope.entity = output.data.data;
                               
                                initControls("edit");
                                $scope.airServiceList = [];
                                $scope.breakBulkServiceList = [];
                                $scope.containerServiceList = [];
                                $scope.lclServiceList = [];
                                $scope.roroServiceList = [];
                                $scope.entity.modeOfService = "2";
                                $scope.updateContainerCategoryList($scope.entity.modeOfService)
                                $scope.entity.containerQty = "";
                                $scope.updateIsHaz($scope.entity.isHaz);
                                
                                //
                                if ($scope.entity.nextActionRemarksDTOList != null) {
                                    $scope.actionRemarksList = $scope.entity.nextActionRemarksDTOList;
                                }

                                //Fill commodity details
                                var temp = new Array();
                                if ($scope.entity.commodityDTOList != null) {
                                    if ($scope.entity.commodityDTOList.length > 0) {
                                        $scope.entity.commodityTypeID = $scope.entity.commodityDTOList[0].commodityTypeID;
                                        if ($scope.entity.commodityTypeID == 1)
                                        {
                                            $scope.entity.isHaz = true;
                                            $scope.updateIsHaz($scope.entity.isHaz);
                                        }
                                        else {
                                            $scope.entity.isHaz = false;
                                            $scope.updateIsHaz($scope.entity.isHaz);
                                        }
                                        for (var i = 0; i < $scope.entity.commodityDTOList.length; i++) {
                                            temp[i] = parseInt($scope.entity.commodityDTOList[i].commodityId);
                                        }
                                        $scope.entity.commodityIds = temp;
                                    }
                                }
                                //$scope.entity.modeOfService = "1";
                                //Fill Container details
                                var total = 0;
                                if ($scope.entity.enquiryContainerServiceDTOList != null) {
                                    if ($scope.entity.enquiryContainerServiceDTOList.length > 0) {
                                        $scope.entity.modeOfService = $scope.entity.enquiryContainerServiceDTOList[0].modeOfService;
                                        $scope.updateContainerCategoryList($scope.entity.modeOfService)
                                        $scope.containerServiceList = $scope.entity.enquiryContainerServiceDTOList;
                                        $scope.containerServiceList.forEach(function (o) {
                                            total = total + o.quantity;
                                        });
                                        $scope.entity.noOfContainer = total;
                                    }
                                }
                               
                                //Fill Air grid
                                $scope.airServiceList = [];
                                if ($scope.entity.enquiryAIRServiceDTOList != null) {
                                    if ($scope.entity.enquiryAIRServiceDTOList.length > 0) {
                                        //$scope.entity.modeOfService = "7";
                                        $scope.airServiceList = $scope.entity.enquiryAIRServiceDTOList;
                                        calculateAirTotal();
                                    }
                                }
                                //Fill Break Bulk grid
                                $scope.breakBulkServiceList = [];
                                if ($scope.entity.enquiryBreakBulkServiceDTOList != null) {
                                    if ($scope.entity.enquiryBreakBulkServiceDTOList.length > 0) {
                                        //$scope.entity.modeOfService = "5";
                                        $scope.breakBulkServiceList = $scope.entity.enquiryBreakBulkServiceDTOList;
                                        calculateBreakBulkTotal();
                                    }
                                }
                                //Fill LCL grid
                                $scope.lclServiceList = [];
                                if ($scope.entity.enquiryLCLServiceDTOList != null) {
                                    if ($scope.entity.enquiryLCLServiceDTOList.length > 0) {
                                        //$scope.entity.modeOfService = "2";
                                        $scope.lclServiceList = $scope.entity.enquiryLCLServiceDTOList;
                                        calculateLCLTotal();
                                    }
                                }
                                //Fill RORO grid
                                $scope.roroServiceList = [];
                                if ($scope.entity.enquiryROROServiceDTOList != null) {
                                    if ($scope.entity.enquiryROROServiceDTOList.length > 0) {
                                        //$scope.entity.modeOfService = "3";
                                        $scope.roroServiceList = $scope.entity.enquiryROROServiceDTOList;
                                        calculateROROTotal();
                                    }
                                }                               
                                $scope.entity.documentCommonID = 0;
                                //$scope.entity.FileNo = "1" + _newDocumentCommonID.ToString();
                                $scope.entity.quotationID = 0;
                                $scope.entity.quotationNo = "Direct";
                                $scope.entity.enquiryID = 0;
                                $scope.entity.enquiryNo = "Direct";
                                $scope.entity.bookingCategory = "1";
                                $scope.entity.galBookingStatusID = 1;
                                $scope.entity.invoiceStatusID = "1";                                

                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });

                        }
                        else if (output.action == "close") {
                            $scope.bookinglistTable.reload();
                            $scope.goBack();
                        }                       
                        
                        
                        if (output.action != "close") {
                            $scope.onClickTab($scope.tabs[0]);
                            viewDetail();
                        }
                    },
                    function (output) {
                        ngNotifier.logError(output);
                    });

            });
          
            
        };

        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
        };

        $scope.showDocumentDetail = function (action, documentCommonID) {

            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entity.documentCommonID = documentCommonID;
            $scope.entity.siteId = $scope.$parent.selectedSiteId;

            entityService.getDocumentDetail($scope.entity).then(
                 function (output) {
                     if (output.data.resultId == 2005) {
                         ngNotifier.showError($scope.authentication, output);
                         $scope.logOut()
                     }
                     $scope.entity = output.data.data;
                     $scope.airServiceList = [];
                     $scope.breakBulkServiceList = [];
                     $scope.containerServiceList = [];
                     $scope.lclServiceList = [];
                     $scope.roroServiceList = [];
                     $scope.isDepartmentDisabled = true;
                     $scope.entity.modeOfService = "2";
                     $scope.updateContainerCategoryList($scope.entity.modeOfService)
                     $scope.entity.containerQty = "";
                     $scope.updateIsHaz($scope.entity.isHaz);
                     if ($scope.entity.pickupCategory == null) {
                         $scope.entity.pickupCategory = "1";
                     }
                     if ($scope.entity.galBookingType == null) {
                         $scope.entity.galBookingType = "1";
                     }
                     
                     if ($scope.entity.carrierChargesDTOList != null)
                     {
                         $scope.carrierChargesList = $scope.entity.carrierChargesDTOList;
                     }
                     //
                     if ($scope.entity.nextActionRemarksDTOList != null) {
                         $scope.actionRemarksList = $scope.entity.nextActionRemarksDTOList;
                     }

                     //Fill commodity details
                     var temp = new Array();
                     if ($scope.entity.commodityDTOList != null) {
                         if ($scope.entity.commodityDTOList.length > 0) {
                             $scope.entity.commodityTypeID = $scope.entity.commodityDTOList[0].commodityTypeID;
                             if ($scope.entity.commodityTypeID == 1) {
                                 $scope.entity.isHaz = true;
                                 $scope.updateIsHaz($scope.entity.isHaz);
                             }
                             else {
                                 $scope.entity.isHaz = false;
                                 $scope.updateIsHaz($scope.entity.isHaz);
                             }
                             for (var i = 0; i < $scope.entity.commodityDTOList.length; i++) {
                                 temp[i] = parseInt($scope.entity.commodityDTOList[i].commodityId);
                             }
                             $scope.entity.commodityIds = temp;
                         }
                     }
                     //Fill Container details
                     var total = 0;
                     if ($scope.entity.enquiryContainerServiceDTOList != null) {
                         if ($scope.entity.enquiryContainerServiceDTOList.length > 0) {
                             $scope.entity.modeOfService = $scope.entity.enquiryContainerServiceDTOList[0].modeOfService;
                             $scope.updateContainerCategoryList($scope.entity.modeOfService)
                             $scope.containerServiceList = $scope.entity.enquiryContainerServiceDTOList;
                             $scope.containerServiceList.forEach(function (o) {
                                 total = total + o.quantity;
                             });
                             $scope.entity.noOfContainer = total;
                         }
                     }
                     //Fill Air grid
                     $scope.airServiceList = [];
                     if ($scope.entity.enquiryAIRServiceDTOList != null) {
                         if ($scope.entity.enquiryAIRServiceDTOList.length > 0) {
                             $scope.entity.modeOfService = "7";
                             $scope.airServiceList = $scope.entity.enquiryAIRServiceDTOList;
                             calculateAirTotal();
                         }
                     }
                     //Fill Break Bulk grid
                     $scope.breakBulkServiceList = [];
                     if ($scope.entity.enquiryBreakBulkServiceDTOList != null) {
                         if ($scope.entity.enquiryBreakBulkServiceDTOList.length > 0) {
                             $scope.entity.modeOfService = "5";
                             $scope.breakBulkServiceList = $scope.entity.enquiryBreakBulkServiceDTOList;
                             calculateBreakBulkTotal();
                         }
                     }
                     //Fill LCL grid
                     $scope.lclServiceList = [];
                     if ($scope.entity.enquiryLCLServiceDTOList != null) {
                         if ($scope.entity.enquiryLCLServiceDTOList.length > 0) {
                             $scope.entity.modeOfService = "2";
                             $scope.lclServiceList = $scope.entity.enquiryLCLServiceDTOList;
                             calculateLCLTotal();
                         }
                     }
                     //Fill RORO grid
                     $scope.roroServiceList = [];
                     if ($scope.entity.enquiryROROServiceDTOList != null) {
                         if ($scope.entity.enquiryROROServiceDTOList.length > 0) {
                             $scope.entity.modeOfService = "3";
                             $scope.roroServiceList = $scope.entity.enquiryROROServiceDTOList;
                             calculateROROTotal();
                         }
                     }

                     $scope.afterGetDetail(action);

                 },
                 function (output) {
                     ngNotifier.showError($scope.authentication, output);
                 }
             );


        };
        $scope.showEnquiryDetail = function (action, id, isComplete) {

            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entityId = id;
            $scope.entity.enquiryID = parseFloat(id);
            $scope.entity.isComplete = parseInt(isComplete);
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            if ($scope.entityId > 0) {
                entityService.getEnquiryDetail($scope.entity).then(
                     function (output) {
                         if (output.data.resultId == 2005) {
                             ngNotifier.showError($scope.authentication, output);
                             $scope.logOut()
                         }
                         $scope.entity = output.data.data;
                         $scope.airServiceList = [];
                         $scope.breakBulkServiceList = [];
                         $scope.containerServiceList = [];
                         $scope.lclServiceList = [];
                         $scope.roroServiceList = [];
                         
                         $scope.entity.modeOfService = "2";
                         $scope.updateContainerCategoryList($scope.entity.modeOfService)
                         $scope.entity.containerQty = "";
                         $scope.updateIsHaz($scope.entity.isHaz);

                         //Fill commodity details
                         var temp = new Array();
                         if ($scope.entity.commodityDTOList != null) {
                             if ($scope.entity.commodityDTOList.length > 0) {
                                 $scope.entity.commodityTypeID = $scope.entity.commodityDTOList[0].commodityTypeID;
                                 if ($scope.entity.commodityTypeID == 1) {
                                     $scope.entity.isHaz = true;
                                     $scope.updateIsHaz($scope.entity.isHaz);
                                 }
                                 else {
                                     $scope.entity.isHaz = false;
                                     $scope.updateIsHaz($scope.entity.isHaz);
                                 }

                                 for (var i = 0; i < $scope.entity.commodityDTOList.length; i++) {
                                     temp[i] = parseInt($scope.entity.commodityDTOList[i].commodityId);
                                 }
                                 $scope.entity.commodityIds = temp;
                             }
                         }
                         //Fill Container details
                         var total = 0;
                         if ($scope.entity.enquiryContainerServiceDTOList != null) {
                             if ($scope.entity.enquiryContainerServiceDTOList.length > 0) {
                                 $scope.entity.modeOfService = $scope.entity.enquiryContainerServiceDTOList[0].modeOfService;
                                 $scope.updateContainerCategoryList($scope.entity.modeOfService)
                                 $scope.containerServiceList = $scope.entity.enquiryContainerServiceDTOList;
                                 $scope.containerServiceList.forEach(function (o) {
                                     total = total + o.quantity;
                                 });
                                 $scope.entity.noOfContainer = total;
                             }
                         }
                         //Fill Air grid
                         $scope.airServiceList = [];
                         if ($scope.entity.enquiryAIRServiceDTOList != null) {
                             if ($scope.entity.enquiryAIRServiceDTOList.length > 0) {
                                 //$scope.entity.modeOfService = "7";
                                 $scope.airServiceList = $scope.entity.enquiryAIRServiceDTOList;
                                 calculateAirTotal();
                             }
                         }
                         //Fill Break Bulk grid
                         $scope.breakBulkServiceList = [];
                         if ($scope.entity.enquiryBreakBulkServiceDTOList != null) {
                             if ($scope.entity.enquiryBreakBulkServiceDTOList.length > 0) {
                                 //$scope.entity.modeOfService = "5";
                                 $scope.breakBulkServiceList = $scope.entity.enquiryBreakBulkServiceDTOList;
                                 calculateBreakBulkTotal();
                             }
                         }
                         //Fill LCL grid
                         $scope.lclServiceList = [];
                         if ($scope.entity.enquiryLCLServiceDTOList != null) {
                             if ($scope.entity.enquiryLCLServiceDTOList.length > 0) {
                                 //$scope.entity.modeOfService = "2";
                                 $scope.lclServiceList = $scope.entity.enquiryLCLServiceDTOList;
                                 calculateLCLTotal();
                             }
                         }
                         //Fill RORO grid
                         $scope.roroServiceList = [];
                         if ($scope.entity.enquiryROROServiceDTOList != null) {
                             if ($scope.entity.enquiryROROServiceDTOList.length > 0) {
                                 //$scope.entity.modeOfService = "3";
                                 $scope.roroServiceList = $scope.entity.enquiryROROServiceDTOList;
                                 calculateROROTotal();
                             }
                         }

                         $scope.afterGetDetail(action);

                     },
                     function (output) {
                         ngNotifier.showError($scope.authentication, output);
                     }
                 );

            }
            else {
                $scope.goBack();
            }
        };

        $scope.performBookingAction = function (source, fromList, documentCommonID) {

            var action = source.currentTarget.attributes["action"].value;

            //$scope.$broadcast("show-errors-check-validity");

            //if (action != "cancel" && $scope.form.detail != undefined && $scope.form.detail.$invalid) {
            //    if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
            //        ngNotifier.error("Required Field(s) are missing data.");
            //    }
            //    else if ($scope.form.detail.usrPwdC.$invalid) {
            //        ngNotifier.error("Password do not match with Confirm Password.");
            //    }
            //    return;
            //}

            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {

                $scope.showDocumentDetail(action, documentCommonID);

            } else {
                $scope.onClickTab($scope.tabs[0]);
                viewDetail();
                //initControls(action);
                initControls(action);
                switchTab("Detail", action);
            }

            //switchTab("Detail", action);

            switch (action) {
                case "search":
                    filterList();
                    break;
                case "add":
                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                   
                    
                    //$("input[input-date]").each(function (index, element) { $(element).val(null); });
                    break;
                case "copy":
                    //lastAction = action;
                    //$scope.entity.enquiryID = 0;                    
                    break;
                    //lastAction = 'copy';
                    //$scope.entityId = 0;
                    //$scope.entity = {};
                    //$("input[input-date]").each(function (index, element) { $(element).val(null); });
                    break;
                case "edit":
                    //lastAction = action;
                    break;
                case "save":
                    save(action);
                    break;
                case "saveEmail":
                    $scope.entity.isSendEmailNow = true;
                    save(action);
                    break;
                case "cancel":
                    cancelDocument(documentCommonID);
                    //$scope.showEnquiryDetail('viewDetail', $scope.entity.enquiryID, $scope.entity.isComplete);
                    //lastAction = "";
                    break;
                case "delete":
                    deleteDocument(documentCommonID);
                    //lastAction = "";
                    break;
                case "rollOver":
                    rollOverDocument(documentCommonID);
                    //lastAction = "";
                    break;
                case "deleteBatch":
                    //removeBatch(documentCommonID);
                    //lastAction = "";
                    break;
                case "verify":
                case "activate":
                case "deactivate":
                    $scope.changeStatus(action);
                    lastAction = "";
                    break;
                default:
                    //lastAction = "";
                    break;
            }

            if ($scope.afterPerformAction != undefined) {
                $scope.afterPerformAction(source, fromList);
            }
        };


        //#region Modal Methods
        $scope.callCountryModal = function (source) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId
            var sourceId;
            if (source == 'originCountry') {
                if ($scope.entity.originCountryID != null)
                    sourceId = $scope.entity.originCountryID;
            }
            else {
                if ($scope.entity.destinationCountryID != null) {
                    sourceId = $scope.entity.destinationCountryID;
                }
            }
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplcountryModal/detail.html",
                controller: "siplcountryModalController",
                resolve: {
                    requestData: function () {
                        return {
                            countryId: (sourceId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {

                        if (source == 'originCountry') {
                            if (output.data.countryId != null) {
                                $scope.entity.originCountryID = output.data.countryId;
                            }
                            if (output.data.name != null) {
                                $scope.entity.originCountry = output.data.name;
                            }
                        }
                        else {
                            if (output.data.countryId != null) {
                                $scope.entity.destinationCountryID = output.data.countryId;
                            }
                            if (output.data.name != null) {
                                $scope.entity.dischargeCountry = output.data.name;
                            }
                        }
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        $scope.callStateModal = function (source) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId
            var sourceId;
            if (source == 'originState') {
                if ($scope.entity.originStateID != null)
                    sourceId = $scope.entity.originStateID;
            }
            else {
                if ($scope.entity.dischargeStateCityID != null) {
                    sourceId = $scope.entity.dischargeStateCityID;
                }
            }
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplStateModal/detail.html",
                controller: "siplStateModalController",
                resolve: {
                    requestData: function () {
                        return {
                            orignStateID: (sourceId || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        if (source == 'originState') {
                            if (output.data.stateId != null) {
                                $scope.entity.orignStateID = output.data.stateId;
                            }
                            if (output.data.name != null) {
                                $scope.entity.originState = output.data.name;
                            }
                        }
                        else {
                            if (output.data.stateId != null) {
                                $scope.entity.destinationStateID = output.data.countryId;
                            }
                            if (output.data.name != null) {
                                $scope.entity.dischargeState = output.data.name;
                            }
                        }
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        $scope.callCityModal = function (source) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId
            var sourceId;
            if (source == 'originCity') {
                if ($scope.entity.originCityID != null)
                    sourceId = $scope.entity.originCityID;
            }
            else {
                if ($scope.entity.dischargeCityID != null) {
                    sourceId = $scope.entity.dischargeCityID;
                }
            }

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCityModal/detail.html",
                controller: "siplCityModalController",
                resolve: {
                    requestData: function () {
                        return {
                            cityId: (sourceId || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
               function (output) {
                   if (output.resultId == 1001) {
                       if (source == 'originCity') {
                           if (output.data.cityId != null) {
                               $scope.entity.originCityID = output.data.cityId;
                           }
                           if (output.data.name != null) {
                               $scope.entity.originCity = output.data.name;
                           }
                       }
                       else {
                           if (output.data.cityId != null) {
                               $scope.entity.dischargeCityID = output.data.cityId;
                           }
                           if (output.data.name != null) {
                               $scope.entity.dischargeCity = output.data.name;
                           }
                       }

                   }
               },
               function (output) {
                   ngNotifier.logError(output);
               });
        };
        $scope.callPortModal = function (source) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId
            var sourceId;
            if (source == 'origin') {
                if ($scope.entity.originID != null)
                    sourceId = $scope.entity.originID;
            }
            else {
                if ($scope.entity.dischargeID != null) {
                    sourceId = $scope.entity.dischargeID;
                }
            }
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplPortModal/detail.html",
                controller: "siplPortModalController",
                resolve: {
                    requestData: function () {
                        return {
                            originID: (sourceId || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
               function (output) {
                   if (output.resultId == 1001) {
                       $scope.entity.originID = output.data[0].portId;
                       $scope.entity.origin = output.data.name;

                       if (source == 'origin') {
                           if (output.data.portId != null) {
                               $scope.entity.originID = output.data.portId;
                           }
                           if (output.data.name != null) {
                               $scope.entity.origin = output.data.name;
                           }
                       }
                       else {
                           if (output.data.portId != null) {
                               $scope.entity.dischargeID = output.data.cityId;
                           }
                           if (output.data.name != null) {
                               $scope.entity.discharge = output.data.name;
                           }
                       }
                   }
               },
               function (output) {
                   ngNotifier.logError(output);
               });
        }
        $scope.callTerminalModal = function () {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplTerminalModal/detail.html",
                controller: "siplTerminalModalController",
                resolve: {
                    requestData: function () {
                        return {
                            destnTerminalId: ($scope.entity.destnTerminalId || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.destnTerminalId = output.data.terminalid;
                        $scope.entity.terminal = output.data.name;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        $scope.callRailRampModal = function (source) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplRailRampModal/detail.html",
                controller: "siplRailRampModalController",
                resolve: {
                    requestData: function () {
                        return {
                            orgnRailRampId: ($scope.entity.orgnRailRampId || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.orgnRailRampId = output.data.railID;
                        $scope.entity.railRamp = output.data.name;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        $scope.callCommodityModal = function (source) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/commodityModal/detail.html",
                controller: "commodityModalController",
                resolve: {
                    requestData: function () {
                        return {
                            commodityId: ($scope.entity.commodityId || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.commodityId = output.data.commodityId;
                        $scope.entity.commodity = output.data.name;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        $scope.callCompanyModal = function (source) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCompanyModal/detail.html",
                controller: "siplCompanyModalController",
                resolve: {
                    requestData: function () {

                        return {
                            companyID: ($scope.entity.customerID || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.customerID = output.data.contactID;
                        $scope.entity.customerAddress = output.data.address;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.showNextActionRemarksModel = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/booking/nextActionRemarks.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                    $scope.actionRemarks = requestData.actionRemarks
                    $scope.select = function (action) {
                        var outputData = {}
                        if (action == 'update') {

                        }
                        else {
                            outputData.action = 'close';
                        }
                        $uibModalInstance.close(outputData);
                    };
                },
                resolve: {
                    requestData: function () {
                        return {
                            actionRemarks: $scope.actionRemarksList
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.action == "update") {

                    }
                    else if (output == "close") {

                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.showCarrierChargesModel = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/booking/carrierCharges.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                    $scope.carrierChargesList = requestData.carrierChargesList
                    $scope.select = function (action) {
                        var outputData = {}
                        if (action == 'update') {

                        }
                        else {
                            outputData.action = 'close';
                        }
                        $uibModalInstance.close(outputData);
                    };
                },
                resolve: {
                    requestData: function () {
                        return {
                            carrierChargesList: $scope.carrierChargesList
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.action == "update") {

                    }
                    else if (output == "close") {

                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };


        //#endregion

        $scope.calculateWeight= function (control) {
            var Weigthkgs = 0
            var Weigthlbs = 0
            var value = 0

            if (control == 'hazweight') {
                if ($scope.entity.hazweight != null) {
                    Weigthkgs = $scope.entity.hazweight
                    var Rnd = Weigthkgs * 2.2046
                    $scope.entity.hazWeightLBS = Rnd
                }
            }
            if (control == 'txtBwtpcLbs') {
                if ($scope.entity.txtBwtpcLbs != null) {
                    Weigthlbs = $scope.entity.txtBwtpcLbs
                    var Rnd = Weigthlbs / 2.2046
                    $scope.entity.txtBwtpcKg = Rnd
                }
            }
        }
        $scope.calculateVolume = function (control) {
            var volumnCM = 0
            var volumnCF = 0
            var value = 0

            if (control == 'hazVolume') {
                if ($scope.entity.hazVolume != null) {
                    volumnCM = $scope.entity.hazVolume
                    var Rnd = volumnCM * 35.3147
                    $scope.entity.hazVolumeCF = Rnd
                }
            }
            if (control == 'hazVolumeCF') {
                if ($scope.entity.hazVolumeCF != null) {
                    volumnCF = $scope.entity.hazVolumeCF
                    var Rnd = volumnCF / 35.3147
                    $scope.entity.hazVolume = Rnd
                }
            }
        }
        var calculateAirTotal = function () {
            var totalWt = 0;
            var totalVol = 0;
            if ($scope.airServiceList.length > 0) {
                for (var i = 0; i < $scope.airServiceList.length; i++) {
                    totalWt += $scope.airServiceList[i].totalWt;
                    totalVol += $scope.airServiceList[i].volume;
                }
                $scope.entity.txtActualWeightkg = totalWt;
                $scope.entity.txtActualWeightLbs = totalWt * 2.2046;

                $scope.entity.txtVolumeWeight = totalVol * 0.02540; // Convert in meter

                if (totalWt > (totalVol * 0.02540)) {
                    $scope.entity.txtChargeableWeight = totalWt;
                }
                else {
                    $scope.entity.txtChargeableWeight = totalVol * 0.02540;
                }
            }
            else {
                $scope.entity.txtActualWeightkg = '';
                $scope.entity.txtActualWeightLbs = '';
                $scope.entity.txtVolumeWeight = '';
                $scope.entity.txtChargeableWeight = '';
            }

        }
        $scope.calculateTotalwtAir = function () {

            var Piece = 0
            var Weigthkgs = 0
            var Weigthlbs = 0
            var Len = 0
            var LenCM = 0
            var Wid = 0
            var WidCM = 0
            var Hei = 0
            var HeiCM = 0
            var Rnd = 0; // round the value
            if ($scope.entity.txtPieceCount != null) {
                Piece = $scope.entity.txtPieceCount
            }
            if ($scope.entity.txtwtpckg != null) {
                Weigthkgs = $scope.entity.txtwtpckg
            }
            if ($scope.entity.txtwtpclbs != null) {
                Weigthlbs = $scope.entity.txtwtpclbs
            }
            if ($scope.entity.txtLengthInch != null) {
                Len = $scope.entity.txtLengthInch
            }
            if ($scope.entity.txtLengthCM != null) {
                LenCM = $scope.entity.txtLengthCM
            }
            if ($scope.entity.txtwidthInch != null) {
                Wid = $scope.entity.txtwidthInch
            }
            if ($scope.entity.txtwidthCM != null) {
                WidCM = $scope.entity.txtwidthCM
            }
            if ($scope.entity.txtHightInch != null) {
                Hei = $scope.entity.txtHightInch
            }
            if ($scope.entity.txtHightCM != null) {
                HeiCM = $scope.entity.txtHightCM
            }
            if (Piece != 0 && Weigthkgs != 0) {
                Rnd = Piece * Weigthkgs
                $scope.entity.txtTotalWtkg = Rnd
            }
            if (Piece != 0 && Weigthlbs != 0) {
                Rnd = Piece * Weigthlbs
                $scope.entity.txtTotalWtlbs = Rnd
            }
            if (Len != 0 && Wid != 0 && Hei != 0 && Piece != 0) {
                Rnd = (Len * Wid * Hei) / 366
                $scope.entity.airVolume = Rnd
            }

        }
        $scope.calculatewtAir = function (control) {
            var Weigthkgs = 0
            var Weigthlbs = 0
            var value = 0

            if (control == 'txtwtpckg') {
                if ($scope.entity.txtwtpckg != null) {
                    Weigthkgs = $scope.entity.txtwtpckg
                    var Rnd = Weigthkgs * 2.2046
                    $scope.entity.txtwtpclbs = Rnd
                }
            }
            if (control == 'txtwtpclbs') {
                if ($scope.entity.txtwtpclbs != null) {
                    Weigthlbs = $scope.entity.txtwtpclbs
                    var Rnd = Weigthlbs / 2.2046
                    $scope.entity.txtwtpckg = Rnd
                }
            }

            //Conversion of length from inch to cm
            if (control == 'txtLengthInch') {
                if ($scope.entity.txtLengthInch != null) {
                    value = $scope.entity.txtLengthInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtLengthCM = Rnd
                }
            }

            //Conversion of length from cm  to inch
            if (control == 'txtLengthCM') {
                if ($scope.entity.txtLengthCM != null) {
                    value = $scope.entity.txtLengthCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtLengthInch = Rnd
                }
            }
            //Conversion of width from inch to cm
            if (control == 'txtwidthInch') {
                if ($scope.entity.txtwidthInch != null) {
                    value = $scope.entity.txtwidthInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtwidthCM = Rnd
                }
            }
            //Conversion of width from cm  to inch
            if (control == 'txtwidthCM') {
                if ($scope.entity.txtwidthCM != null) {
                    value = $scope.entity.txtwidthCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtwidthInch = Rnd
                }
            }

            //Conversion of Height from inch to cm
            if (control == 'txtHightInch') {
                if ($scope.entity.txtHightInch != null) {
                    value = $scope.entity.txtHightInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtHightCM = Rnd
                }
            }

            //Conversion of Height from cm  to inch
            if (control == 'txtHightCM') {
                if ($scope.entity.txtHightCM != null) {
                    value = $scope.entity.txtHightCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtHightInch = Rnd
                }
            }
        }
        $scope.addAirRow = function () {
            var airServiceItem = {};
            if ($scope.entity.txtPieceCount != null && $scope.entity.txtPieceCount != 0) {
                airServiceItem.pieceCount = $scope.entity.txtPieceCount;
            }
            else {
                ngNotifier.error("Please enter Piece Count");
                return;
            }
            if ($scope.entity.txtwtpckg != null && $scope.entity.txtwtpckg != 0) {
                airServiceItem.weight = $scope.entity.txtwtpckg;
            } else {
                ngNotifier.error("Please enter Weight");
                return;
            }
            if ($scope.entity.txtTotalWtkg != null && $scope.entity.txtTotalWtkg != 0) {
                airServiceItem.totalWt = $scope.entity.txtTotalWtkg;
            } else { airServiceItem.totalWt = 0; }
            if ($scope.entity.txtLengthInch != null && $scope.entity.txtLengthInch != 0) {
                airServiceItem.length = $scope.entity.txtLengthInch;
            } else {
                ngNotifier.error("Please enter Length");
                return;
            }
            if ($scope.entity.txtwidthInch != null && $scope.entity.txtwidthInch != 0) {
                airServiceItem.width = $scope.entity.txtwidthInch;
            } else {
                ngNotifier.error("Please enter Width");
                return;
            }
            if ($scope.entity.txtHightInch != null && $scope.entity.txtHightInch != 0) {
                airServiceItem.height = $scope.entity.txtHightInch;
            } else {
                ngNotifier.error("Please enter Hight");
                return;
            }
            if ($scope.entity.airVolume != null && $scope.entity.airVolume != 0) {
                airServiceItem.volume = $scope.entity.airVolume;
            } else {
                ngNotifier.error("Volume can't be empty, please enter length/width/hight");
                return;
            }
            $scope.airServiceList.push(airServiceItem);
            calculateAirTotal();
            $scope.entity.txtPieceCount = '';
            $scope.entity.txtwtpckg = '';
            $scope.entity.txtwtpclbs = '';
            $scope.entity.Weight = '';
            $scope.entity.txtTotalWtkg = '';
            $scope.entity.txtTotalWtlbs = '';
            $scope.entity.txtLengthInch = '';
            $scope.entity.txtLengthCM = '';
            $scope.entity.txtwidthInch = '';
            $scope.entity.txtwidthCM = '';
            $scope.entity.txtHightInch = '';
            $scope.entity.txtHightCM = '';
            $scope.entity.volume = '';
        };
        $scope.removeAirServiceRow = function (rownum) {
            $scope.airServiceList.splice(rownum, 1);
            calculateAirTotal();
        }
        $scope.editAirServiceRow = function (rownum) {
            $scope.entity.txtPieceCount = $scope.airServiceList[rownum].pieceCount;
            $scope.entity.txtwtpckg = $scope.airServiceList[rownum].weight;
            $scope.entity.txtTotalWtkg = $scope.airServiceList[rownum].totalWt;
            $scope.entity.txtLengthInch = $scope.airServiceList[rownum].length;
            $scope.entity.txtwidthInch = $scope.airServiceList[rownum].width;
            $scope.entity.txtHightInch = $scope.airServiceList[rownum].height;
            $scope.entity.airVolume = '';
            $scope.airServiceList.splice(rownum, 1);
            calculateAirTotal();
        }

        var calculateBreakBulkTotal = function () {
            var totalWt = 0;
            var totalVol = 0;
            if ($scope.breakBulkServiceList.length > 0) {
                for (var i = 0; i < $scope.breakBulkServiceList.length; i++) {
                    totalWt += $scope.breakBulkServiceList[i].totalWtkgs;
                    totalVol += $scope.breakBulkServiceList[i].volume;
                }
                $scope.entity.txtBVolume = totalVol * 0.0000164;
                $scope.entity.txtBCubMt = totalVol * 0.000579;

                $scope.entity.txtBWeight = totalWt;
                $scope.entity.txtBWeightLbs = totalWt * 2.2046;
            }
            else {
                $scope.entity.txtBVolume = '';
                $scope.entity.txtBCubMt = '';
                $scope.entity.txtBWeight = '';
                $scope.entity.txtBWeightLbs = '';
            }

        }
        $scope.calculateTotalWtBB = function () {
            var Piece = 0
            var Hei = 0
            var HeiCM = 0
            var Weigthkgs = 0
            var Weigthlbs = 0
            var Len = 0
            var LenCM = 0
            var Wid = 0
            var WidCM = 0
            var Rnd = 0; // round the value

            if ($scope.entity.txtBQty != null) {
                Piece = $scope.entity.txtBQty;
            }
            if ($scope.entity.txtBwtpcKg != null) {
                Weigthkgs = $scope.entity.txtBwtpcKg;
            }
            if ($scope.entity.txtBwtpcLbs != null) {
                Weigthlbs = $scope.entity.txtBwtpcLbs;
            }
            if ($scope.entity.txtBLengthInch != null) {
                Len = $scope.entity.txtBLengthInch;
            }
            if ($scope.entity.txtBwidthInch != null) {
                Wid = $scope.entity.txtBwidthInch;
            }
            if ($scope.entity.txtBHightInch != null) {
                Hei = $scope.entity.txtBHightInch;
            }
            if ($scope.entity.txtBLengthCM != null) {
                LenCM = $scope.entity.txtBLengthCM;
            }
            if ($scope.entity.txtBwidthCM != null) {
                WidCM = $scope.entity.txtBwidthCM;
            }
            if ($scope.entity.txtBHightCM != null) {
                HeiCM = $scope.entity.txtBHightCM;
            }
            if (Piece != 0 && Weigthkgs != 0) {
                Rnd = Piece * Weigthkgs;
                $scope.entity.txtBTotalWtkgs = Rnd;
            }
            if (Piece != 0 && Weigthlbs != 0) {
                Rnd = Piece * Weigthlbs;
                $scope.entity.txtBTotalWtlbs = Rnd;
            }
            if (Len != 0 && Wid != 0 && Hei != 0 && Piece != 0) {
                Rnd = (Len * Wid * Hei);
                $scope.entity.bbVolume = Rnd;
            }
            else { $scope.entity.bbVolume = 0; }
        }
        $scope.calculateWtBB = function (control) {
            var Weigthkgs = 0
            var Weigthlbs = 0
            var value = 0

            if (control == 'txtBwtpcKg') {
                if ($scope.entity.txtBwtpcKg != null) {
                    Weigthkgs = $scope.entity.txtBwtpcKg
                    var Rnd = Weigthkgs * 2.2046
                    $scope.entity.txtBwtpcLbs = Rnd
                }
            }
            if (control == 'txtBwtpcLbs') {
                if ($scope.entity.txtBwtpcLbs != null) {
                    Weigthlbs = $scope.entity.txtBwtpcLbs
                    var Rnd = Weigthlbs / 2.2046
                    $scope.entity.txtBwtpcKg = Rnd
                }
            }
            //Conversion of length from inch to cm
            if (control == 'txtBLengthInch') {
                if ($scope.entity.txtBLengthInch != null) {
                    value = $scope.entity.txtBLengthInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtBLengthCM = Rnd
                }
            }
            //Conversion of length from cm  to inch
            if (control == 'txtBLengthCM') {
                if ($scope.entity.txtBLengthCM != null) {
                    value = $scope.entity.txtBLengthCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtBLengthInch = Rnd
                }
            }
            //Conversion of width from inch to cm
            if (control == 'txtBwidthInch') {
                if ($scope.entity.txtBwidthInch != null) {
                    value = $scope.entity.txtBwidthInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtBwidthCM = Rnd
                }
            }
            //Conversion of width from cm  to inch
            if (control == 'txtBwidthCM') {
                if ($scope.entity.txtBwidthCM != null) {
                    value = $scope.entity.txtBwidthCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtBwidthInch = Rnd
                }
            }
            //Conversion of Height from inch to cm
            if (control == 'txtBHightInch') {
                if ($scope.entity.txtBHightInch != null) {
                    value = $scope.entity.txtBHightInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtBHightCM = Rnd
                }
            }
            //Conversion of Height from cm  to inch
            if (control == 'txtBHightCM') {
                if ($scope.entity.txtBHightCM != null) {
                    value = $scope.entity.txtBHightCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtBHightInch = Rnd
                }
            }
        }
        $scope.addBreakBulkRow = function () {
            var breakBulkServiceItem = {};
            if ($scope.entity.txtDescription != null) {
                breakBulkServiceItem.description = $scope.entity.txtDescription;
            }
            else {
                ngNotifier.error("Please enter Description");
                return;
            }
            if ($scope.entity.txtBQty != null && $scope.entity.txtBQty != 0) {
                breakBulkServiceItem.qty = $scope.entity.txtBQty;
            } else {
                ngNotifier.error("Please enter Qty");
                return;
            }
            if ($scope.entity.txtBwtpcKg != null && $scope.entity.txtBwtpcKg != 0) {
                breakBulkServiceItem.weight = $scope.entity.txtBwtpcKg;
            } else {
                ngNotifier.error("Please enter Wt/Pcs");
                return;
            }
            breakBulkServiceItem.totalWtkgs = $scope.entity.txtBTotalWtkgs
            if ($scope.entity.txtBLengthInch != null && $scope.entity.txtBLengthInch != 0) {
                breakBulkServiceItem.length = $scope.entity.txtBLengthInch;
            } else {
                ngNotifier.error("Please enter Length");
                return;
            }
            if ($scope.entity.txtBwidthInch != null && $scope.entity.txtBwidthInch != 0) {
                breakBulkServiceItem.width = $scope.entity.txtBwidthInch;
            } else {
                ngNotifier.error("Please enter width");
                return;
            }
            if ($scope.entity.txtBHightInch != null && $scope.entity.txtBHightInch != 0) {
                breakBulkServiceItem.height = $scope.entity.txtBHightInch;
            } else {
                ngNotifier.error("Please enter Hight");
                return;
            }
            if ($scope.entity.bbVolume != null && $scope.entity.bbVolume != 0) {
                breakBulkServiceItem.volume = $scope.entity.bbVolume;
            } else {
                ngNotifier.error("Volume can't be empty, please enter length/width/hight");
                return;
            }

            $scope.breakBulkServiceList.push(breakBulkServiceItem);
            calculateBreakBulkTotal();
            $scope.entity.txtDescription = '';
            $scope.entity.txtBQty = '';
            $scope.entity.txtBwtpcKg = '';
            $scope.entity.txtBwtpcLbs = '';
            $scope.entity.txtBTotalWtkgs = '';
            $scope.entity.txtBTotalWtlbs = '';
            $scope.entity.txtBLengthInch = '';
            $scope.entity.txtBLengthCM = '';
            $scope.entity.txtBwidthInch = '';
            $scope.entity.txtBwidthCM = '';
            $scope.entity.txtBHightInch = '';
            $scope.entity.txtBHightCM = '';

        };
        $scope.removeBreakBulkServiceRow = function (rownum) {
            $scope.breakBulkServiceList.splice(rownum, 1);
            calculateBreakBulkTotal();
        }
        $scope.editBreakBulkServiceRow = function (rownum) {
            $scope.entity.txtDescription = $scope.breakBulkServiceList[rownum].description;
            $scope.entity.txtBQty = $scope.breakBulkServiceList[rownum].qty;
            $scope.entity.txtBwtpcKg = $scope.breakBulkServiceList[rownum].weight;
            $scope.entity.txtBTotalWtkgs = $scope.breakBulkServiceList[rownum].totalWtkgs;
            $scope.entity.txtBLengthInch = $scope.breakBulkServiceList[rownum].length;
            $scope.entity.txtBwidthInch = $scope.breakBulkServiceList[rownum].width;
            $scope.entity.txtBHightInch = $scope.breakBulkServiceList[rownum].height;
            $scope.entity.bbVolume = '';
            $scope.breakBulkServiceList.splice(rownum, 1);
            calculateBreakBulkTotal();
        }

        var calculateLCLTotal = function () {
            var totalWt = 0;
            var totalVol = 0;
            if ($scope.lclServiceList.length > 0) {
                for (var i = 0; i < $scope.lclServiceList.length; i++) {
                    totalWt += $scope.lclServiceList[i].totalWt;
                    totalVol += $scope.lclServiceList[i].volume;
                }
                $scope.entity.txtLCLActualWeightkg = totalWt;
                $scope.entity.txtLCLActualWeightLbs = totalWt * 2.2046;

                $scope.entity.txtLCLVolumeWeight = totalVol * 0.02540; // Convert in meter

                if (totalWt > totalVol * 0.02540) {
                    $scope.entity.txtLCLChargeableWeight = totalWt;
                }
                else {
                    $scope.entity.txtLCLChargeableWeight = totalVol * 0.02540;
                }
            }
            else {
                $scope.entity.txtLCLActualWeightkg = '';
                $scope.entity.txtLCLActualWeightLbs = '';
                $scope.entity.txtLCLVolumeWeight = '';
                $scope.entity.txtLCLChargeableWeight = '';
            }

        }
        $scope.calculateTotalwtLCL = function () {

            var Piece = 0
            var Weigthkgs = 0
            var Weigthlbs = 0
            var Len = 0
            var LenCM = 0
            var Wid = 0
            var WidCM = 0
            var Hei = 0
            var HeiCM = 0
            var Rnd = 0; // round the value
            if ($scope.entity.txtLCLPieceCount != null) {
                Piece = $scope.entity.txtLCLPieceCount
            }
            if ($scope.entity.txtLCLwtpckg != null) {
                Weigthkgs = $scope.entity.txtLCLwtpckg
            }
            if ($scope.entity.txtLCLwtpclbs != null) {
                Weigthlbs = $scope.entity.txtLCLwtpclbs
            }
            if ($scope.entity.txtLCLLengthInch != null) {
                Len = $scope.entity.txtLCLLengthInch
            }
            if ($scope.entity.txtLCLLengthCM != null) {
                LenCM = $scope.entity.txtLCLLengthCM
            }
            if ($scope.entity.txtLCLwidthInch != null) {
                Wid = $scope.entity.txtLCLwidthInch
            }
            if ($scope.entity.txtLCLwidthCM != null) {
                WidCM = $scope.entity.txtLCLwidthCM
            }
            if ($scope.entity.txtLCLHightInch != null) {
                Hei = $scope.entity.txtLCLHightInch
            }
            if ($scope.entity.txtLCLHightCM != null) {
                HeiCM = $scope.entity.txtLCLHightCM
            }
            if (Piece != 0 && Weigthkgs != 0) {
                Rnd = Piece * Weigthkgs
                $scope.entity.txtLCLTotalWtkg = Rnd
            }
            if (Piece != 0 && Weigthlbs != 0) {
                Rnd = Piece * Weigthlbs
                $scope.entity.txtLCLTotalWtlbs = Rnd
            }
            if (Len != 0 && Wid != 0 && Hei != 0 && Piece != 0) {
                Rnd = (Len * Wid * Hei) / 366
                $scope.entity.lclvolume = Rnd
            }
            else { $scope.entity.lclvolume = 0; }

        }
        $scope.calculatewtLCL = function (control) {
            var Weigthkgs = 0
            var Weigthlbs = 0
            var value = 0

            if (control == 'txtLCLwtpckg') {
                if ($scope.entity.txtLCLwtpckg != null) {
                    Weigthkgs = $scope.entity.txtLCLwtpckg
                    var Rnd = Weigthkgs * 2.2046
                    $scope.entity.txtLCLwtpclbs = Rnd
                }
            }
            if (control == 'txtLCLwtpclbs') {
                if ($scope.entity.txtLCLwtpclbs != null) {
                    Weigthlbs = $scope.entity.txtLCLwtpclbs
                    var Rnd = Weigthlbs / 2.2046
                    $scope.entity.txtLCLwtpckg = Rnd
                }
            }

            //Conversion of length from inch to cm
            if (control == 'txtLCLLengthInch') {
                if ($scope.entity.txtLCLLengthInch != null) {
                    value = $scope.entity.txtLCLLengthInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtLCLLengthCM = Rnd
                }
            }

            //Conversion of length from cm  to inch
            if (control == 'txtLCLLengthCM') {
                if ($scope.entity.txtLCLLengthCM != null) {
                    value = $scope.entity.txtLCLLengthCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtLCLLengthInch = Rnd
                }
            }
            //Conversion of width from inch to cm
            if (control == 'txtLCLwidthInch') {
                if ($scope.entity.txtLCLwidthInch != null) {
                    value = $scope.entity.txtLCLwidthInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtLCLwidthCM = Rnd
                }
            }
            //Conversion of width from cm  to inch
            if (control == 'txtLCLwidthCM') {
                if ($scope.entity.txtLCLwidthCM != null) {
                    value = $scope.entity.txtLCLwidthCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtLCLwidthInch = Rnd
                }
            }

            //Conversion of Height from inch to cm
            if (control == 'txtLCLHightInch') {
                if ($scope.entity.txtLCLHightInch != null) {
                    value = $scope.entity.txtLCLHightInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtLCLHightCM = Rnd
                }
            }

            //Conversion of Height from cm  to inch
            if (control == 'txtLCLHightCM') {
                if ($scope.entity.txtLCLHightCM != null) {
                    value = $scope.entity.txtLCLHightCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtLCLHightInch = Rnd
                }
            }
        }
        $scope.addLCLRow = function () {
            var lclServiceItem = {};
            if ($scope.entity.txtLCLPieceCount != null && $scope.entity.txtLCLPieceCount != 0) {
                lclServiceItem.pieceCount = $scope.entity.txtLCLPieceCount;
            }
            else {
                ngNotifier.error("Please enter Piece Count");
                return;
            }
            if ($scope.entity.txtLCLwtpckg != null && $scope.entity.txtLCLwtpckg != 0) {
                lclServiceItem.weight = $scope.entity.txtLCLwtpckg;
            } else {
                ngNotifier.error("Please enter Weight");
                return;
            }
            if ($scope.entity.txtLCLTotalWtkg != null && $scope.entity.txtLCLTotalWtkg != 0) {
                lclServiceItem.totalWt = $scope.entity.txtLCLTotalWtkg;
            } else { lclServiceItem.totalWt = 0; }
            if ($scope.entity.txtLCLLengthInch != null && $scope.entity.txtLCLLengthInch != 0) {
                lclServiceItem.length = $scope.entity.txtLCLLengthInch;
            } else {
                ngNotifier.error("Please enter Length");
                return;
            }
            if ($scope.entity.txtLCLwidthInch != null && $scope.entity.txtLCLwidthInch != 0) {
                lclServiceItem.width = $scope.entity.txtLCLwidthInch;
            } else {
                ngNotifier.error("Please enter Width");
                return;
            }
            if ($scope.entity.txtLCLwidthInch != null && $scope.entity.txtLCLwidthInch != 0) {
                lclServiceItem.height = $scope.entity.txtLCLHightInch;
            } else {
                ngNotifier.error("Please enter Hight");
                return;
            }
            if ($scope.entity.lclvolume != null && $scope.entity.lclvolume != 0) {
                lclServiceItem.volume = $scope.entity.lclvolume;
            } else {
                ngNotifier.error("Volume can't be empty, please enter length/width/hight");
                return;
            }
            if (lclServiceItem.pieceCount != 0 && lclServiceItem.weight != 0 && lclServiceItem.length && lclServiceItem.width && lclServiceItem.height != 0) {
                $scope.lclServiceList.push(lclServiceItem);
            }
            calculateLCLTotal();
            $scope.entity.txtLCLPieceCount = '';
            $scope.entity.txtLCLwtpckg = '';
            $scope.entity.txtLCLwtpclbs = '';
            $scope.entity.txtLCLTotalWtkg = '';
            $scope.entity.txtLCLTotalWtlbs = '';
            $scope.entity.txtLCLLengthInch = '';
            $scope.entity.txtLCLLengthCM = '';
            $scope.entity.txtLCLwidthInch = '';
            $scope.entity.txtLCLwidthCM = '';
            $scope.entity.txtLCLHightInch = '';
            $scope.entity.txtLCLHightCM = '';
            $scope.entity.lclvolume = '';
        };
        $scope.removeLCLServiceRow = function (rownum) {
            $scope.lclServiceList.splice(rownum, 1);
            calculateLCLTotal();
        }
        $scope.editLCLServiceRow = function (rownum) {
            $scope.entity.txtLCLPieceCount = $scope.lclServiceList[rownum].pieceCount;
            $scope.entity.txtLCLwtpckg = $scope.lclServiceList[rownum].weight;
            $scope.entity.txtLCLTotalWtkg = $scope.lclServiceList[rownum].totalWt;
            $scope.entity.txtLCLLengthInch = $scope.lclServiceList[rownum].length;
            $scope.entity.txtLCLwidthInch = $scope.lclServiceList[rownum].width;
            $scope.entity.txtLCLHightInch = $scope.lclServiceList[rownum].height;
            $scope.entity.lclvolume = '';
            $scope.lclServiceList.splice(rownum, 1);
            calculateLCLTotal();
        }

        var calculateROROTotal = function () {
            var totalWt = 0;
            var totalVol = 0;
            if ($scope.roroServiceList.length > 0) {
                for (var i = 0; i < $scope.roroServiceList.length; i++) {
                    totalWt += $scope.roroServiceList[i].totalwt;
                    totalVol += $scope.roroServiceList[i].volume;
                }
                $scope.entity.txtROROVolume = totalVol * 0.0000164;
                $scope.entity.txtROROCubMt = totalVol * 0.000579;

                $scope.entity.txtROROWeight = totalWt;
                $scope.entity.txtROROWeightLbs = totalWt * 2.2046;

            }
            else {
                $scope.entity.txtROROVolume = '';
                $scope.entity.txtROROCubMt = '';
                $scope.entity.txtROROWeight = '';
                $scope.entity.txtROROWeightLbs = '';

            }

        }
        $scope.calculateTotalWtRORO = function () {
            var Piece = 0
            var Hei = 0
            var HeiCM = 0
            var Weigthkgs = 0
            var Weigthlbs = 0
            var Len = 0
            var LenCM = 0
            var Wid = 0
            var WidCM = 0
            var Rnd = 0; // round the value

            if ($scope.entity.txtROROQty != null) {
                Piece = $scope.entity.txtROROQty;
            }
            if ($scope.entity.txtROROwtpcKg != null) {
                Weigthkgs = $scope.entity.txtROROwtpcKg;
            }
            if ($scope.entity.txtROROwtpcLbs != null) {
                Weigthlbs = $scope.entity.txtROROwtpcLbs;
            }
            if ($scope.entity.txtROROLengthInch != null) {
                Len = $scope.entity.txtROROLengthInch;
            }
            if ($scope.entity.txtROROwidthInch != null) {
                Wid = $scope.entity.txtROROwidthInch;
            }
            if ($scope.entity.txtROROHightInch != null) {
                Hei = $scope.entity.txtROROHightInch;
            }
            if ($scope.entity.txtROROLengthCM != null) {
                LenCM = $scope.entity.txtROROLengthCM;
            }
            if ($scope.entity.txtROROwidthCM != null) {
                WidCM = $scope.entity.txtROROwidthCM;
            }
            if ($scope.entity.txtROROHightCM != null) {
                HeiCM = $scope.entity.txtROROHightCM;
            }
            if (Piece != 0 && Weigthkgs != 0) {
                Rnd = Piece * Weigthkgs
                $scope.entity.txtROROTotalWtkgs = Rnd;
            }
            if (Piece != 0 && Weigthlbs != 0) {
                Rnd = Piece * Weigthlbs
                $scope.entity.txtROROTotalWtlbs = Rnd
            }
            if (Len != 0 && Wid != 0 && Hei != 0 && Piece != 0) {
                Rnd = (Len * Wid * Hei)
                $scope.entity.rorovolume = Rnd
            }
            else { $scope.entity.rorovolume = 0; }

        }
        $scope.calculateWtRORO = function (control) {
            var Weigthkgs = 0
            var Weigthlbs = 0
            var value = 0

            if (control == 'txtROROwtpcKg') {
                if ($scope.entity.txtROROwtpcKg != null) {
                    Weigthkgs = $scope.entity.txtROROwtpcKg
                    var Rnd = Weigthkgs * 2.2046
                    $scope.entity.txtROROwtpcLbs = Rnd
                }
            }
            if (control == 'txtROROwtpcLbs') {
                if ($scope.entity.txtROROwtpcLbs != null) {
                    Weigthlbs = $scope.entity.txtROROwtpcLbs
                    var Rnd = Weigthlbs / 2.2046
                    $scope.entity.txtROROwtpcKg = Rnd
                }
            }
            //Conversion of length from inch to cm
            if (control == 'txtROROLengthInch') {
                if ($scope.entity.txtROROLengthInch != null) {
                    value = $scope.entity.txtROROLengthInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtROROLengthCM = Rnd
                }
            }
            //Conversion of length from cm  to inch
            if (control == 'txtROROLengthCM') {
                if ($scope.entity.txtROROLengthCM != null) {
                    value = $scope.entity.txtROROLengthCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtROROLengthInch = Rnd
                }
            }
            //Conversion of width from inch to cm
            if (control == 'txtROROwidthInch') {
                if ($scope.entity.txtROROwidthInch != null) {
                    value = $scope.entity.txtROROwidthInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtROROwidthCM = Rnd
                }
            }
            //Conversion of width from cm  to inch
            if (control == 'txtROROwidthCM') {
                if ($scope.entity.txtROROwidthCM != null) {
                    value = $scope.entity.txtROROwidthCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtROROwidthInch = Rnd
                }
            }
            //Conversion of Height from inch to cm
            if (control == 'txtROROHightInch') {
                if ($scope.entity.txtROROHightInch != null) {
                    value = $scope.entity.txtROROHightInch
                    var Rnd = value * 2.54000
                    $scope.entity.txtROROHightCM = Rnd
                }
            }
            //Conversion of Height from cm  to inch
            if (control == 'txtROROHightCM') {
                if ($scope.entity.txtROROHightCM != null) {
                    value = $scope.entity.txtROROHightCM
                    var Rnd = value / 2.54000
                    $scope.entity.txtROROHightInch = Rnd
                }
            }
        }
        $scope.addRORORow = function () {
            var roroServiceItem = {};
            if ($scope.entity.txtROROQty != 0 && $scope.entity.txtROROQty != null) {
                roroServiceItem.qty = $scope.entity.txtROROQty;
            }
            else {
                ngNotifier.error("Please enter Qty");
                return;
            }
            if ($scope.entity.txtROROwtpcKg != 0 && $scope.entity.txtROROwtpcKg != null) {
                roroServiceItem.weight = $scope.entity.txtROROwtpcKg;
            } else {
                ngNotifier.error("Please enter Wt/Pcs");
                return;
            }
            roroServiceItem.totalwt = $scope.entity.txtROROTotalWtkgs;
            if ($scope.entity.txtROROLengthInch != 0 && $scope.entity.txtROROLengthInch != null) {
                roroServiceItem.length = $scope.entity.txtROROLengthInch;
            } else {
                ngNotifier.error("Please enter Length");
                return;
            }
            if ($scope.entity.txtROROwidthInch != 0 && $scope.entity.txtROROwidthInch != null) {
                roroServiceItem.width = $scope.entity.txtROROwidthInch;
            } else {
                ngNotifier.error("Please enter Width");
                return;
            }
            if ($scope.entity.txtROROHightInch != 0 && $scope.entity.txtROROwidthInch != null) {
                roroServiceItem.height = $scope.entity.txtROROHightInch;
            } else {
                ngNotifier.error("Please enter Hight");
                return;
            }
            if ($scope.entity.txtNoofCranes != null && $scope.entity.txtNoofCranes != 0) {
                roroServiceItem.noofCrain = $scope.entity.txtNoofCranes;
            } else {
                ngNotifier.error("Please enter No of Units");
                return;
            }
            if ($scope.entity.rorovolume != 0 && $scope.entity.rorovolume != null) {
                roroServiceItem.volume = $scope.entity.rorovolume;
            } else {
                ngNotifier.error("Volume can't be empty, please enter length/width/hight");
                return;
            }

            $scope.roroServiceList.push(roroServiceItem);
            calculateROROTotal();
            $scope.entity.txtROROQty = '';
            $scope.entity.txtROROwtpcKg = '';
            $scope.entity.txtROROwtpcLbs = '';
            $scope.entity.txtROROTotalWtkgs = '';
            $scope.entity.txtROROTotalWtlbs = '';
            $scope.entity.txtROROLengthInch = '';
            $scope.entity.txtROROLengthCM = '';
            $scope.entity.txtROROwidthInch = '';
            $scope.entity.txtROROwidthCM = '';
            $scope.entity.txtROROHightInch = '';
            $scope.entity.txtROROHightCM = '';
        };
        $scope.removeROROServiceRow = function (rownum) {
            $scope.roroServiceList.splice(rownum, 1);
            calculateROROTotal();
        }
        $scope.editROROServiceRow = function (rownum) {

            $scope.entity.txtROROQty = $scope.roroServiceList[rownum].qty;
            $scope.entity.txtROROwtpcKg = $scope.roroServiceList[rownum].weight;
            $scope.entity.txtROROTotalWtkgs = $scope.roroServiceList[rownum].totalwt;
            $scope.entity.txtROROLengthInch = $scope.roroServiceList[rownum].length;
            $scope.entity.txtROROwidthInch = $scope.roroServiceList[rownum].width;
            $scope.entity.txtROROHightInch = $scope.roroServiceList[rownum].height;
            $scope.entity.noofCrain = $scope.roroServiceList[rownum].txtNoofCranes;
            $scope.entity.rorovolume = '';
            $scope.roroServiceList.splice(rownum, 1);
            calculateROROTotal();
        }

        $scope.addContainerRow = function () {
            var containerServiceItem = {};

            if ($scope.entity.categoryItem == null) {
                ngNotifier.error("Please select Container Category");
                return;
            }
            if ($scope.entity.containerTypeItem == null) {
                ngNotifier.error("Please select Container Type");
                return;
            }
            if ($scope.entity.containerQty == '' || $scope.entity.containerQty <= 0) {
                ngNotifier.error("Please Enter Valid Service QTY");
                return;
            }
            if ($scope.entity.categoryItem != null && $scope.entity.containerTypeItem != null && $scope.entity.containerQty > 0) {
                containerServiceItem.categoryName = $scope.entity.categoryItem.categoryName;
                containerServiceItem.containerName = $scope.entity.containerTypeItem.name;
                containerServiceItem.containerTypeID = $scope.entity.containerTypeItem.containerTypeId;
                containerServiceItem.quantity = $scope.entity.containerQty;
                containerServiceItem.noOfContainer = $scope.entity.noOfContainer;
                $scope.containerServiceList.push(containerServiceItem);
            }
            else {
                ngNotifier.error("Please select valid values");
                return;
            }

            //calculateAirTotal();
            $scope.entity.containerQty = '';

        };
        $scope.removeContainerServiceRow = function (rownum) {
            $scope.containerServiceList.splice(rownum, 1);
            //calculateAirTotal();
        }

        $scope.beforeFetchLookupData = function (moduleName, otherId, sortField, lookupKey) {
            var listParams = {
                OtherId: otherId,
                PageIndex: 1,
                PageSize: 10000,
                CwtId: $scope.userWorkTypeId,
                Sort: "{\"" + sortField + "\":\"asc\"}",
                Filter: "[]"
            };
            if (moduleName == "siplUser") {
                var filter = [];
                filter.push(Utility.createFilter("SitId", "numeric", "SitId", $scope.$parent.selectedSiteId, "contains", null));
                listParams.Filter = JSON.stringify(filter);
            }
            return listParams;
        };

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("bookingSpaceController", controller);

});
