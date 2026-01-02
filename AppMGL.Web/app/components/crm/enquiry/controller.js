"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "crmenquiryService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {
       
        $scope.viewBookingHistory = function (entity) {
            
            
            var modalInstance = $uibModal.open({
                animation: false,
                windowClass: 'custom-modal-width',
                backdrop: "static",
                keyboard: false,               
                templateUrl: "app/components/crm/enquiry/viewbooking.html",
                controller: "viewbookingController",
                resolve: {
                    requestData: function () {
                        return {
                            siteid: $scope.selectedSiteId,
                            entity:entity 

                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.quatationNo = output.finalQuotations;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });

        };
            
        



        //#region General

        $scope.page = appUrl.crmenquiry;
        $scope.tabs = appUrl.crmenquiry.tabs;
        $scope.$parent.pageTitle = "Lead Management";
        $scope.$parent.breadcrumbs = ["CRM", "Lead Management"];
        //$scope.departments = [];
        $scope.enquiryID;
        $scope.deleteRemarks = "";
        $scope.isComplete = false;
        $scope.airServiceList = [];
        $scope.breakBulkServiceList = [];
        $scope.containerServiceList = [];
        $scope.lclServiceList = [];
        $scope.roroServiceList = [];
        $scope.searchResult = [];
        $scope.actionRemarksList = [];

        $scope.isInvalidData = false;
        $scope.isWHSuplierVisible = false;
        $scope.isWHSuplierAddressVisible = false;
        $scope.isDestDoorVisible = false;
        $scope.isDestDoorAddressVisible = false;
        $scope.isOriginRailRampVisible = false;
        $scope.isDestTerminalVisible = false;
        $scope.isHaz = false;
        $scope.containerCategoryList = [];
        $scope.containerSizesList = [];
        $scope.confirmationResult = false;
        $scope.isdraft = 1;
        $scope.isdraft_cgange = function () {
            if ($('#chkisdraft')[0].checked == true) {
                $scope.isdraft = 0;
            } else {
                $scope.isdraft = 1;
            }

        };
        var lastAction = "";
       
        //#endregion

        //#region Lookup

        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "SIPLContact") {
                $scope.entity.fkCompanyID = output.data[0].contactID;
                $scope.entity.expAddress = output.data[0].address;
            }
            else if (lookup == "originDoorName") {
                $scope.entity.originDoorID = output.data[0].contactID;
                $scope.entity.originDoorAddress = output.data[0].address;
            }
            else if (lookup == "destinationDoorName") {
                $scope.entity.destinationDoorID = output.data[0].contactID;
                $scope.entity.destinationDoorAddress = output.data[0].address;
            }
            else if (lookup == "BillToCompany") {
                $scope.entity.billToCompanyId = output.data[0].contactID;
                $scope.entity.billToAddress = output.data[0].address;
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
                $scope.entity.originCountryID = output.data[0].countryId;
                $scope.entity.originCountry = output.data[0].countryName;
                $scope.entity.orignStateID = null;
                $scope.entity.originState = null;
                $scope.entity.originCityID = null;
                $scope.entity.originCity = null;
            }
            else if (lookup == "discharge") {
                $scope.entity.dischargeID = output.data[0].portId;
                $scope.entity.destinationCountryID = output.data[0].countryId;
                $scope.entity.dischargeCountry = output.data[0].countryName;
                $scope.entity.destinationStateID = null;
                $scope.entity.dischargeState = null;
                $scope.entity.dischargeCityID = null;
                $scope.entity.dischargeCity = null;
            }
            else if (lookup == "RailRamp") {
                $scope.entity.orgnRailRampId = output.data[0].railId;
            }
            else if (lookup == "Terminal") {
                $scope.entity.destnTerminalId = output.data[0].terminalId;
            }
        };

        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "SIPLContact") {
                $scope.entity.fkCompanyID = null;
                $scope.entity.address = "";
            }
            else if (lookup == "originDoorName") {
                $scope.entity.originDoorID = null;
                $scope.entity.originDoorAddress = "";
            }
            else if (lookup == "destinationDoorName") {
                $scope.entity.destinationDoorID = null;
                $scope.entity.destinationDoorAddress = "";
            }
            else if (lookup == "BillToCompany") {
                if ($scope.isInvalidData == true) {
                    $scope.entity.billToCompanyId = null;
                    $scope.entity.billToAddress = "";
                }
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
                $scope.entity.orgnRailRampId = null;
            }
            else if (lookup == "Terminal") {
                $scope.entity.destnTerminalId = null;
            }
        };

        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {

            if (lookupModule == "SIPLContact" || lookupModule == "originDoorName" || lookupModule == "destinationDoorName" || lookupModule == "BillToCompany" || lookupModule == "originCountry" || lookupModule == "dischargeCountry" || lookupModule == "originState" || lookupModule == "dischargeState" || lookupModule == "originCity" || lookupModule == "dischargeCity" || lookupModule == "originPort" || lookupModule == "dischargePort" || lookupModule == "RailRamp" || lookupModule == "Terminal") {

                if (lookupModule == "SIPLContact" || lookupModule == "RailRamp") {
                    if ($scope.entity[lookupField] == null || $scope.entity[lookupField] == "") {
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

        $scope.lookups = { siplDepartments: [], siplUsers: [], commodityTypes: [], commodities: [], containerSizes: [], containerCategories: [] };
        var today = new Date();
        var yyyy = today.getFullYear();
        $scope.startNextActionDate = Utility.getDateISO(new Date("01/01/" + yyyy + ""));
        $scope.endNextActionDate = Utility.getDateISO(new Date("12/31/" + yyyy + ""));
        $scope.initDropdown = function () {

            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
            $scope.fetchLookupData("sipluser", 0, "name", "siplUsers", null);
            $scope.fetchLookupData("commodityType", 0, "commodityType", "commodityTypes", null);
            $scope.fetchLookupData("Commodity", 0, "name", "commodities", null);
            //$scope.fetchLookupData("ContainerType", 0, "name", "containerTypeList", null);
            $scope.getContainerCategories();
            $scope.getContainerSizes();
            var today = new Date();
            var yyyy = today.getFullYear();
            $scope.entity.startNextActionDate = Utility.getDateISO(new Date("01/01/" + yyyy + ""));
            $scope.entity.endNextActionDate = Utility.getDateISO(new Date("12/31/" + yyyy + ""));
            // setTimeout(function () { $("#chkisdraft").prop('checked', true) }, 500);

        };

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
        $scope.showPickupType = function (pickupValue) {
            if (pickupValue == "0") {
                $scope.isWHSuplierVisible = true;
                $scope.isWHSuplierAddressVisible = true;
                $scope.isDestDoorVisible = false;
                $scope.isDestDoorAddressVisible = false;
                $scope.isOriginRailRampVisible = true;
                $scope.isDestTerminalVisible = false;

            }
            else if (pickupValue == "1") {
                $scope.isWHSuplierVisible = false;
                $scope.isWHSuplierAddressVisible = false;
                $scope.isDestDoorVisible = false;
                $scope.isDestDoorAddressVisible = false;
                $scope.isOriginRailRampVisible = true;
                $scope.isDestTerminalVisible = false;
            }
            else if (pickupValue == "2") {
                $scope.isWHSuplierVisible = true;
                $scope.isWHSuplierAddressVisible = true;
                $scope.isDestDoorVisible = true;
                $scope.isDestDoorAddressVisible = true;
                $scope.isOriginRailRampVisible = true;
                $scope.isDestTerminalVisible = false;
            }
            else if (pickupValue == "3") {
                $scope.isWHSuplierVisible = false;
                $scope.isWHSuplierAddressVisible = false;
                $scope.isDestDoorVisible = false;
                $scope.isDestDoorAddressVisible = false;
                $scope.isOriginRailRampVisible = false;
                $scope.isDestTerminalVisible = true;
            }
            else if (pickupValue == "4") {
                $scope.isWHSuplierVisible = false;
                $scope.isWHSuplierAddressVisible = false;
                $scope.isDestDoorVisible = false;
                $scope.isDestDoorAddressVisible = false;
                $scope.isOriginRailRampVisible = true;
                $scope.isDestTerminalVisible = false;
            }
            else if (pickupValue == "5") {
                $scope.isWHSuplierVisible = true;
                $scope.isWHSuplierAddressVisible = true;
                $scope.isDestDoorVisible = true;
                $scope.isDestDoorAddressVisible = true;
                $scope.isOriginRailRampVisible = false;
                $scope.isDestTerminalVisible = false;
            }
        };
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
                    $scope.isComplete = false;
                    $scope.entity.isHaz = false;
                    $scope.updateIsHaz($scope.entity.isHaz);
                    $scope.entity.modeOfService = "1";
                    $scope.updateContainerCategoryList($scope.entity.modeOfService);
                    $scope.entity.commodityTypeID = 2;
                    $scope.entity.pickupType = "0";
                    $scope.entity.licenseType = "1";
                    $scope.showPickupType($scope.entity.pickupType);
                    $scope.entity.enquiryDate = $scope.getCurrentDate();
                    $scope.containerServiceList = [];

                    break;
                case "edit":
                    break;


            }
        };

        $scope.afterGetDetail = function (action) {
            if ($scope.entity.isComplete == "1")
                $scope.isComplete = true;
            else
                $scope.isComplete = false;
            if (action == 'copy') {
                $scope.entity.enquiryID = 0;
                $scope.entity.enquiryNo = null;
                $scope.entity.departmentID = 0;
                $scope.entity.enquiryDate = $scope.getCurrentDate();
            }
            if ($scope.entity.hazweight != null) {
                $scope.calculateHaz('KGS')
            }
            if ($scope.entity.hazVolume != null) {
                $scope.calculateHaz('CM')
            }
        };

        $scope.getLatestEnquiryNo = function (depId) {

            var getEnquirynoValue = entityService.getEnquiryNo(depId).then(
                function (output) {
                    $scope.entity.enquiryNo = output.data.data.enquiryNo;
                    $scope.entity.lastEnquiryNo = output.data.data.lastEnquiryNo;
                    $scope.entity.enquiryControlNo = output.data.data.enquiryControlNo;

                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        $scope.updateBillType = function (billType) {
            $scope.entity.billToCompanyId = null;
            $scope.entity.billToCompanyName = "";
            $scope.entity.billToAddress = "";
            if (billType == 1) {
                $scope.entity.billToCompanyId = $scope.entity.fkCompanyID;
                $scope.entity.billToCompanyName = $scope.entity.companyName;
                $scope.entity.billToAddress = $scope.entity.expAddress;
            }
        };
        //$scope.searchResult1 = [];
        $scope.searchValues = function (viewValue, selectOption) {
            var resultItem = {};
            if (selectOption == "companyName") {
                //return $scope.callTypeahead(viewValue, 'SIPLContact', 'companyName', null).then(
                //    function (output) {
                //        $scope.searchResult = [];
                //        output.forEach(function (o) {
                //            resultItem = {}
                //            resultItem.name = o.companyName;
                //            $scope.searchResult.push(resultItem)
                //        });
                //        return $scope.searchResult;
                //    }
                //);
                var lookupField = "companyName";
                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getCompanySearchCRM(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.companyName;
                            resultItem.companyName = o.companyName;
                            resultItem.contactID = o.contactID;
                            resultItem.address = o.address;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
            var lookupModule;
            if (selectOption == "state" || selectOption == "originState" || selectOption == "dischargeState") {
                lookupModule = "LGVWState";
            }
            if (selectOption == "city" || selectOption == "originCity" || selectOption == "dischargeCity") {
                lookupModule = "LGVWCity";
            }
            if (selectOption == "origin" || selectOption == "discharge") {
                lookupModule = "SIPLCountry";
            }
            if (selectOption == "commodity") {
                lookupModule = "commodity";
            }
            if (selectOption == "state" || selectOption == "originState" || selectOption == "dischargeState" || selectOption == "city" || selectOption == "originCity" || selectOption == "dischargeCity" || selectOption == "origin" || selectOption == "discharge" || selectOption == "commodity") {
                return $scope.callTypeahead(viewValue, lookupModule, 'name', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}

                            if (selectOption == "originState") {
                                if ($scope.entity.originCountryID == o.fkCountryId) {
                                    resultItem.name = o.name;
                                    resultItem.stateId = o.stateId;
                                    $scope.searchResult.push(resultItem)
                                }
                            }
                            else if (selectOption == "dischargeState") {
                                if ($scope.entity.destinationCountryID == o.fkCountryId) {
                                    resultItem.name = o.name;
                                    resultItem.stateId = o.stateId;
                                    $scope.searchResult.push(resultItem)
                                }
                            }
                            else if (selectOption == "originCity") {
                                if ($scope.entity.orignStateID == o.fkStateId) {
                                    resultItem.name = o.name;
                                    resultItem.cityId = o.cityId;
                                    $scope.searchResult.push(resultItem)
                                }
                            }
                            else if (selectOption == "dischargeCity") {
                                if ($scope.entity.destinationStateID == o.fkStateId) {
                                    resultItem.name = o.name;
                                    resultItem.cityId = o.cityId;
                                    $scope.searchResult.push(resultItem)
                                }
                            }
                            else {
                                resultItem.name = o.name;
                                $scope.searchResult.push(resultItem)
                            }
                        });
                        return $scope.searchResult;
                    }
                );
            }
        };
        $scope.selectTypeaheadCust = function ($item, lookupModule, $label, source) {
            debugger
            var lookupModule = null;
            var lookupIndex = null;
            var target = $(source.currentTarget);


            //var lookupModule = target.attr("lookup-module");
            var lookupModule = 'SIPLContact';
            var lookupIndex = target.attr("lookup-index");

            var output = { data: [] };
            output.data.push($item);

            if ($scope.setLookups != undefined) {
                $scope.setLookups(source, lookupModule, output, lookupIndex);
            }
        };
        //#endregion

        $scope.enquiryTypes = [
            { optionValue: "0", optionName: "Select One" },
            { optionValue: "1", optionName: "By Email" },
            { optionValue: "2", optionName: "By Mail" },
            { optionValue: "3", optionName: "By Telecall" }
        ];

        $scope.billToTypes = [
            { optionValue: "1", optionName: "Same as Shipper" },
            { optionValue: "2", optionName: "Others" }
        ];

        $scope.searchOptions = [
            { optionValue: "", optionName: "-All-" },
            { optionValue: "enquiryNo", optionName: "Enquiry No" },
            { optionValue: "companyName", optionName: "Customer/Company Name" },
            { optionValue: "state", optionName: "State" },
            { optionValue: "city", optionName: "City" },
            { optionValue: "origin", optionName: "Origin Port" },
            { optionValue: "discharge", optionName: "Discharge Port" },
            { optionValue: "commodity", optionName: "Commodity" }
        ];


        $scope.filterEnquiry = function () {
            var dashboardOption = localStorageService.get("dashboardOption");
            $scope.selectOption = "companyName";
            $scope.searchBox = "";
            if (dashboardOption != null) {
                if (dashboardOption == 'EYTD') {
                    $scope.dashboardOption = "EYTD";
                }
                if (dashboardOption == 'EMTD') {
                    $scope.dashboardOption = "EMTD";
                }
                if (dashboardOption == 'EUPQU') {
                    $scope.dashboardOption = "EUPQU";
                }
                if (dashboardOption == 'EUPQF') {
                    $scope.dashboardOption = "";
                }
                localStorageService.remove("dashboardOption");
            }
            else {
                $scope.dashboardOption = "";
            }
        };
        $scope.filterEnquiry();

        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            dashboardOption: $scope.dashboardOption,
            isdraft: $scope.isdraft,
            startDate: $scope.startNextActionDate || "",
            endDate: $scope.endNextActionDate || ""
        };
        //#region Methods
        $scope.enquirylistTable = new NgTableParams(
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
                    Filter: JSON.stringify($scope.searchParam),
                    isdraft: $scope.isdraft
                };

                var dataitems = entityService.listcrmenq(listParams).then(
                    function (output) {
                        $scope.validateUser(output);
                        $scope.items = output.data.data;
                        params.total(output.data.count);
                        //document.getElementById('divAdd').hidden = "true";                        
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });
        //#endregion

        $scope.performEnquirySearch = function (source, selectOption, searchBox) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox,
                dashboardOption: "",
                isdraft: $scope.isdraft,
                startDate: $scope.entity.startNextActionDate || "",
                endDate: $scope.entity.endNextActionDate || ""
            };
            $scope.enquirylistTable.reload();
        };

        $scope.saveEnquiryIncompleteDraft = function (source, fromList) {

            //$scope.$broadcast("show-errors-check-validity");
            $scope.confirmationResult = false;

            if ($scope.entity.enquiryNo == null) {
                ngNotifier.error("Please select Department");
                return;
            }

            //if ($scope.entity.companyName == null || $scope.entity.companyName == '' || ($scope.entity.fkCompanyID == null || $scope.entity.fkCompanyID == 0)) {
            //    ngNotifier.error("Please enter Valid Company Name");
            //    return;
            //}

            if ($scope.entity.remarks == null) {
                ngNotifier.error("Please enter Remarks for Incomplete");
                return;
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
            var isInValid = false;
            isInValid = $scope.validateAutoCompleteValues();
            if (isInValid) {
                return;
            }

            //Fill commodity
            // $scope.fillEnquiryCommodityDetail();
            //Fill FCL
            var totQty = 0;
            $scope.entity.enquiryContainerServiceDTOList = [];

            if ($scope.entity.noOfContainer != null && $scope.entity.noOfContainer <= 0) {
                // ngNotifier.error("Please enter valid No of Containers");
                //  return;
            }
            if ($scope.containerServiceList.length > 0) {
                if ($scope.entity.noOfContainer == null) {
                    // ngNotifier.error("Please enter Total No of Containers");
                    // return;
                }
                else {
                    $scope.entity.enquiryContainerServiceDTOList = $scope.containerServiceList;
                    for (var i = 0; i < $scope.entity.enquiryContainerServiceDTOList.length; i++) {
                        totQty = totQty + parseInt($scope.entity.enquiryContainerServiceDTOList[i].quantity);
                    }
                    if (parseInt($scope.entity.noOfContainer) != totQty) {
                        // ngNotifier.error("Total QTY is not equal to No Of Container");
                        // return;
                    }
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

            if ($scope.containerServiceList.length == 0 &&
                ($scope.entity.enquiryAIRServiceDTOList.length > 0 ||
                    $scope.entity.enquiryBreakBulkServiceDTOList.length > 0 ||
                    $scope.entity.enquiryLCLServiceDTOList.length > 0 ||
                    $scope.entity.enquiryROROServiceDTOList > 0)
            ) {
                // ngNotifier.error("Please add Enquiry Container details");
                // return;
            }
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            $scope.entity.updatedBy = $scope.$parent.authentication.userId;
            $scope.entity.location = 'crm';
            entityService.saveEnquiryIncompleteDraft($scope.entity).then(
                function (output) {
                    $scope.enquiryID = output.data.data;
                    $scope.entity = {};
                    $scope.airServiceList = [];
                    $scope.breakBulkServiceList = [];
                    $scope.containerServiceList = [];
                    $scope.lclServiceList = [];
                    $scope.roroServiceList = [];

                    $scope.enquirylistTable.reload();
                    $scope.goBack();
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







            //var modalInstance = $uibModal.open({
            //    animation: true,
            //    size: "md",
            //    templateUrl: "app/components/operation/enquiry/confirmation.html",
            //    controller: function ($scope, $timeout, $uibModalInstance, requestData) {

            //        $scope.select = function (action) {

            //            var outputData = {}                        
            //            outputData.action = action;
            //            $uibModalInstance.close(outputData);
            //        };
            //    },
            //    resolve: {
            //        requestData: function () {
            //            return {

            //            };
            //        }
            //    }
            //});

            //modalInstance.result.then(
            //    function (output) {
            //        if (output.action == "yes") {

            //            if ($scope.entity.enquiryNo == null) {
            //                ngNotifier.error("Please select Department");
            //                return;
            //            }                       

            //            //if ($scope.entity.companyName == null || $scope.entity.companyName == '' || ($scope.entity.fkCompanyID == null || $scope.entity.fkCompanyID == 0)) {
            //            //    ngNotifier.error("Please enter Valid Company Name");
            //            //    return;
            //            //}

            //            if ($scope.entity.remarks == null)
            //            {
            //                ngNotifier.error("Please enter Remarks for Incomplete");
            //                return;
            //            }

            //            if ($scope.entity.nextActionRemarks != null || $scope.entity.nextActionDueDate != null) {
            //                if ($scope.entity.nextActionRemarks.trim() != '' && $scope.entity.nextActionDueDate == '') {
            //                    ngNotifier.error("Please select Next Action Due date");
            //                    return;
            //                }
            //                if ($scope.entity.nextActionDueDate != '' && $scope.entity.nextActionRemarks.trim() == '') {
            //                    ngNotifier.error("Please Enter Next Action Remarks");
            //                    return;
            //                }
            //            }
            //            var isInValid = false;
            //            isInValid = $scope.validateAutoCompleteValues();
            //            if (isInValid) {
            //                return;
            //            }

            //            //Fill commodity
            //           // $scope.fillEnquiryCommodityDetail();
            //            //Fill FCL
            //            var totQty = 0;
            //            $scope.entity.enquiryContainerServiceDTOList = [];

            //            if ($scope.entity.noOfContainer != null && $scope.entity.noOfContainer <= 0) {
            //               // ngNotifier.error("Please enter valid No of Containers");
            //              //  return;
            //            }
            //            if ($scope.containerServiceList.length > 0) {
            //                if ($scope.entity.noOfContainer == null) {
            //                   // ngNotifier.error("Please enter Total No of Containers");
            //                   // return;
            //                }
            //                else {
            //                    $scope.entity.enquiryContainerServiceDTOList = $scope.containerServiceList;
            //                    for (var i = 0; i < $scope.entity.enquiryContainerServiceDTOList.length; i++) {
            //                        totQty = totQty + parseInt($scope.entity.enquiryContainerServiceDTOList[i].quantity);
            //                    }
            //                    if (parseInt($scope.entity.noOfContainer) != totQty) {
            //                       // ngNotifier.error("Total QTY is not equal to No Of Container");
            //                       // return;
            //                    }
            //                }
            //            }
            //            //Fill AIR
            //            $scope.fillEnquiryAirDetail();
            //            //Fill Break Bulk
            //            $scope.fillEnquiryBreakBulkDetail();
            //            //Fill LCL
            //            $scope.fillEnquiryLCLDetail();
            //            //Fill RORO
            //            $scope.fillEnquiryRORODetail();

            //            if ($scope.containerServiceList.length == 0 &&
            //                ($scope.entity.enquiryAIRServiceDTOList.length > 0 ||
            //                $scope.entity.enquiryBreakBulkServiceDTOList.length > 0 ||
            //                $scope.entity.enquiryLCLServiceDTOList.length > 0 ||
            //                $scope.entity.enquiryROROServiceDTOList > 0)
            //                ) {
            //               // ngNotifier.error("Please add Enquiry Container details");
            //               // return;
            //            }
            //            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            //            $scope.entity.userID = $scope.$parent.authentication.userId;
            //            $scope.entity.updatedBy = $scope.$parent.authentication.userId;
            //            entityService.saveEnquiryIncompleteDraft($scope.entity).then(
            //                function (output) {
            //                    $scope.enquiryID = output.data.data;
            //                    $scope.entity = {};
            //                    $scope.airServiceList = [];
            //                    $scope.breakBulkServiceList = [];
            //                    $scope.containerServiceList = [];
            //                    $scope.lclServiceList = [];
            //                    $scope.roroServiceList = [];

            //                    $scope.enquirylistTable.reload();
            //                    $scope.goBack();
            //                    ngNotifier.show(output.data);
            //                },
            //                function (output) {
            //                    ngNotifier.showError($scope.authentication, output);
            //                    $scope.editMode = false;
            //                    $scope.disabledInsert = true;
            //                    $scope.disabledUpdate = true;
            //                    $scope.requiredInsert = false;
            //                    $scope.requiredUpdate = false;
            //                });


            //        }
            //        else if (output.action == "no") {
            //           // $scope.saveEnquiry(true, false);
            //        }
            //        else if (output == "close") {

            //        }

            //    },
            //    function (output) {
            //        ngNotifier.logError(output);
            //    });


        };

        $scope.saveEnquiry = function (source, fromList) {

            //var actionType = source.currentTarget.attributes["actionType"].value;
            //$scope.$broadcast("show-errors-check-validity");

            if ($scope.entity.enquiryNo == null) {
                ngNotifier.error("Please select Department");
                return;
            }
            if ($scope.entity.enquiryDate == null) {
                ngNotifier.error("Please enter Inquiry Date");
                return;
            }
            if ($scope.entity.companyName == null || $scope.entity.companyName == '' || ($scope.entity.fkCompanyID == null || $scope.entity.fkCompanyID == 0)) {
                ngNotifier.error("Please enter Valid Company Name");
                return;
            }
            if ($scope.entity.billToCompanyName != null && $scope.entity.billToCompanyName != '' && $scope.entity.billToCompanyId == null) {
                ngNotifier.error("Please enter valid Bill Company Name");
                return true;
            }
            if ($scope.entity.originDoorName != null && $scope.entity.originDoorName != '' && ($scope.entity.originDoorID == null || $scope.entity.originDoorID == 0)) {
                ngNotifier.error("Please enter Valid Pick up Ware House / Supplier");
                return;
            }
            if ($scope.entity.destinationDoorName != null && $scope.entity.destinationDoorName != '' && ($scope.entity.destinationDoorID == null || $scope.entity.destinationDoorID == 0)) {
                ngNotifier.error("Please enter Valid Place of Delivery / Destination Door");
                return;
            }
            if ($scope.entity.pickupType == null) {
                ngNotifier.error("Please select PickupType");
                return;
            }
            if ($scope.entity.pickupType == '0' || $scope.entity.pickupType == '1') {

                if ($scope.entity.destinationCountryID == null) {
                    ngNotifier.error("Please select Distination Country");
                    return;
                }
                //if ($scope.entity.orignStateID == null) {
                //    ngNotifier.error("Please select Origin State");
                //    return;
                //}
                //if ($scope.entity.originCityID == null) {
                //    ngNotifier.error("Please select Origin City");
                //    return;
                //}
                if ($scope.entity.dischargeID == null) {
                    ngNotifier.error("Please select Port of Discharge");
                    return;
                }

            }
            if ($scope.entity.pickupType == '2') {

                //if ($scope.entity.orignStateID == null) {
                //    ngNotifier.error("Please select Origin State");
                //    return;
                //}

                //if ($scope.entity.originCityID == null) {
                //    ngNotifier.error("Please select Origin City");
                //    return;
                //}

                if ($scope.entity.originID == null) {
                    ngNotifier.error("Please select Port of Origin");
                    return;
                }
                if ($scope.entity.dischargeID == null) {
                    ngNotifier.error("Please select Port of Discharge");
                    return;
                }

            }
            if ($scope.entity.pickupType == '3' || $scope.entity.pickupType == '4') {

                if ($scope.entity.destinationCountryID == null) {
                    ngNotifier.error("Please select Distination Country");
                    return;
                }
                //if ($scope.entity.orignStateID == null) {
                //    ngNotifier.error("Please select Origin State");
                //    return;
                //}
                //if ($scope.entity.originCityID == null) {
                //    ngNotifier.error("Please select Origin City");
                //    return;
                //}

            }
            if ($scope.entity.originCountryID == null) {
                ngNotifier.error("Please select Origin Country");
                return;
            }
            if ($scope.entity.destinationCountryID == null) {
                ngNotifier.error("Please select Distination Country");
                return;
            }
            //if ($scope.entity.orignStateID == null) {
            //    ngNotifier.error("Please select Origin State");
            //    return;
            //}
            //if ($scope.entity.originCityID == null) {
            //    ngNotifier.error("Please select Origin City");
            //    return;
            //}
            if ($scope.entity.originID == null || $scope.entity.originID == 0) {
                ngNotifier.error("Please select Port of Origin");
                return;
            }
            if ($scope.entity.dischargeID == null || $scope.entity.dischargeID == 0) {
                ngNotifier.error("Please select Port of Discharge");
                return;
            }
            if ($scope.entity.noOfContainer == null || $scope.entity.noOfContainer <= 0) {
                ngNotifier.error("Please enter valid No of Containers");
                return;
            }
            if ($scope.containerServiceList.length == 0) {
                ngNotifier.error("Please add Enquiry Container details");
                return;
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

            //if (actionType == "quotation")
            //{
            if ($scope.entity.shipmentMoveDate == null) {
                ngNotifier.error("Please select Expected Date of Shipment Move");
                return;
            }
            if ($scope.entity.originID == null) {
                ngNotifier.error("Please select Port of Origin");
                return;
            }
            if ($scope.entity.dischargeID == null) {
                ngNotifier.error("Please select Port of Discharge");
                return;
            }
            //}
            var isInValid = false;
            isInValid = $scope.validateAutoCompleteValues();
            if (isInValid) {
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

            //if (actionType == "quotation") {
            $scope.entity.isDraft = 0
            //}
            //else {
            //    $scope.entity.isDraft = 1
            //}
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            $scope.entity.updatedBy = $scope.$parent.authentication.userId;
            entityService.saveEnquiry($scope.entity).then(
                function (output) {
                    $scope.enquiryID = output.data.data;
                    $scope.entity = {};
                    $scope.airServiceList = [];
                    $scope.breakBulkServiceList = [];
                    $scope.containerServiceList = [];
                    $scope.lclServiceList = [];
                    $scope.roroServiceList = [];

                    $scope.enquirylistTable.reload();
                    $scope.goBack();
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

        $scope.validateAutoCompleteValues = function () {

            if ($scope.entity.companyName != null && $scope.entity.companyName != '' && $scope.entity.fkCompanyID == null) {
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
            if ($scope.entity.originCity != null && $scope.entity.originCity != '' && $scope.entity.originCityID == null) {
                ngNotifier.error("Please enter valid City of Origin");
                return true;
            }
            if ($scope.entity.dischargeCity != null && $scope.entity.dischargeCity != '' && $scope.entity.destinationCityID == null) {
                ngNotifier.error("Please enter valid City of Discharge");
                return true;
            }
            if ($scope.entity.originState != null && $scope.entity.originState != '' && $scope.entity.orignStateID == null) {
                ngNotifier.error("Please enter valid Origin of State");
                return true;
            }
            if ($scope.entity.dischargeState != null && $scope.entity.dischargeState != '' && $scope.entity.destinationStateID == null) {
                ngNotifier.error("Please enter valid Discharge State");
                return true;
            }
            if ($scope.entity.originCountry != null && $scope.entity.originCountry != '' && $scope.entity.originCountryID == null) {
                ngNotifier.error("Please enter valid Origin Country");
                return true;
            }
            if ($scope.entity.dischargeCountry != null && $scope.entity.dischargeCountry != '' && $scope.entity.destinationCountryID == null) {
                ngNotifier.error("Please enter valid Discharge Country");
                return true;
            }
            if ($scope.entity.originDoorName != null && $scope.entity.originDoorName != '' && $scope.entity.originDoorID == null) {
                ngNotifier.error("Please enter valid Pick up from Ware House / Supplier");
                return true;
            }
            if ($scope.entity.destinationDoorName != null && $scope.entity.destinationDoorName != '' && $scope.entity.destinationDoorID == null) {
                ngNotifier.error("Please enter valid Place of Delivery / Destination Door");
                return true;
            }
            if ($scope.entity.railRamp != null && $scope.entity.railRamp != '' && $scope.entity.orgnRailRampId == null) {
                ngNotifier.error("Please enter valid Origin RailRamp");
                return true;
            }
            if ($scope.entity.terminal != null && $scope.entity.terminal != '' && $scope.entity.destnTerminalId == null) {
                ngNotifier.error("Please enter valid Destination Terminal");
                return true;
            }

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

        $scope.fillEnquiryFCLDetail = function () {



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









        var removeBatch = function () {
            var entities = [];
            $scope.items.forEach(function (item) {
                if (item.selected) {
                    entities.push(item);
                }
            });
            if (entities.length === 0) {
                ngNotifier.info("Please, select atleast one record to perform action.");
            } else {
                ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {

                    var modalInstance = $uibModal.open({
                        animation: true,
                        size: "md",
                        templateUrl: "app/components/operation/enquiry/remarks.html",
                        controller: function ($scope, $timeout, $uibModalInstance, requestData) {

                            $scope.select = function (action) {
                                //$scope.deleteRemarks = deleteRemarks.value;
                                if (action == 'delete' && $scope.deleteRemarks == null) {
                                    ngNotifier.error("Please Enter Delete Remarks");
                                    return;
                                }
                                var outputData = {}
                                outputData.remarks = $scope.deleteRemarks;
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
                                entities.forEach(function (entity) {
                                    //entity.remarks = deleteRemarks.value;
                                    entity.remarks = output.remarks;
                                    entityService.deleteEnquiry(entity).then(
                                        function (output) {
                                            $scope.entity = {};
                                            $scope.enquirylistTable.reload();
                                            $scope.goBack();
                                            ngNotifier.show(output.data);
                                        },
                                        function (output) {
                                            ngNotifier.showError($scope.authentication, output);
                                        });
                                });
                            }
                            else if (output == "close") {

                            }
                        },
                        function (output) {
                            ngNotifier.logError(output);
                        });

                });
            }
        };

        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
        };


        // add by vikas solanki on 26 dec 2020


        $scope.viewemaillist = function (obj) {

            obj.updatedBy = $scope.$parent.authentication.userId;
            obj.location = 'Inbox';
            entityService.getEnquiryEmailDetail(obj).then(
                function (output) {
                    if (output.data.resultId == 2005) {
                        ngNotifier.showError($scope.authentication, output);
                        $scope.logOut()
                    }
                    $scope.viewemail = output.data.data;
                    openemailviewpopup(obj);
                });

        };
        $scope.viewemaildetailobj = {};
        $scope.detailobj = { to: "", from: "", cc: "", bcc: "", subject: "", body: "", createdby: "" };
        $scope.sendmessage = "";
        var openemailviewpopup = function (obj) {
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/enquiry/viewemail.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                    $scope.selectedFolder = 'Inbox';
                    $scope.selectedObj = null;
                    $scope.$watch('selectedFolder', function (newValue, oldValue) {
                        if (newValue !== oldValue) {
                            console.log('Folder changed to:', newValue);
                            // Add your logic to handle the change
                        }
                    });
                    $scope.viewemail = requestData.viewemail;
                    $scope.selectedObj = requestData.selectedObj;
                    $scope.viewemaildetail = function (obj, type) {

                        $scope.viewemaildetailobj = obj;
                        $scope.viewemaildetailobj.location = $scope.selectedFolder;
                        openemailviewpopupdetail(obj, 1, type);

                    };
                    $scope.select = function (action) {
                        var outputData = {}
                        if (action == 'update') {

                        }
                        else {
                            outputData.action = 'close';
                        }
                        $uibModalInstance.close(outputData);
                    };

                    $scope.searchemail = function () {

                        var selobj = $scope.selectedObj;
                        selobj.location = $scope.selectedFolder;
                        entityService.getEnquiryEmailDetail(obj).then(
                            function (output) {
                                if (output.data.resultId == 2005) {
                                    ngNotifier.showError($scope.authentication, output);
                                    $scope.logOut()
                                }
                                $scope.viewemail = output.data.data;
                                // openemailviewpopup(obj);
                            });

                    };


                },
                resolve: {
                    requestData: function () {
                        return {
                            viewemail: $scope.viewemail,
                            selectedObj: obj

                        };
                    }
                }
            });
        };

        var openemailviewpopupdetail = function (objemailview, status, type) {
            objemailview.createdby = $scope.$parent.authentication.userId;
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/enquiry/viewemaildetail.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {

                    $scope.detailobj = { to: "", from: "", cc: "", bcc: "", subject: "", body: "", createdby: "" };
                    $scope.viewemaildetailobj = requestData.viewemaildetailobj;

                    $scope.detailobj.to = $scope.viewemaildetailobj.from;
                    if (type == "RA") {
                        $scope.detailobj.cc = $scope.viewemaildetailobj.cc;
                    }
                    $scope.detailobj.bcc = $scope.viewemaildetailobj.bcc;

                    $scope.detailobj.subject = $scope.viewemaildetailobj.subject;
                    $scope.detailobj.body = $scope.viewemaildetailobj.body;
                    $scope.detailobj.createdby = $scope.viewemaildetailobj.createdby;
                    if (type == "F") {
                        $scope.detailobj.to = "";
                        $scope.detailobj.cc = "";
                        $scope.detailobj.bcc = "";
                    }

                    $scope.sendemail = function () {

                        entityService.sendemail_EA($scope.detailobj).then(
                            function (output) {
                                if (output.data.resultId == 2005) {
                                    ngNotifier.showError($scope.authentication, output);
                                    $scope.logOut()
                                }
                                else {
                                    $uibModalInstance.close("close");
                                    $scope.sendmessage = "Mail Send Successfully."
                                }
                            });
                    };
                    $scope.select = function (action) {
                        var outputData = {}
                        if (action == 'update') {

                        }
                        else {
                            outputData.action = 'close';
                        }
                        $uibModalInstance.close(outputData);
                        //if (status == 1) {
                        //    openemailviewpopup();
                        //}
                    };

                    if (type == "V") {
                        setTimeout(function () { $("#btnsendemail").hide(); }, 1000);

                    } else {
                        $("#btnsendemail").show();
                    }
                },
                resolve: {
                    requestData: function () {
                        return {
                            viewemaildetailobj: objemailview
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    $("#sendmessage").html('Mail Send Successfully.');
                    $('#sendmessage').delay(5000).fadeOut(400)
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.SendEMailList = function (enqueryno) {
            $scope.viewemaildetailobj = {};
            $scope.viewemaildetailobj.subject = enqueryno;
            $scope.viewemaildetailobj.createdby = $scope.$parent.authentication.userId;
            openemailviewpopupdetail($scope.viewemaildetailobj, 0, "S");
        };



        $scope.showEmailDetail = function (action, id, isComplete) {

            viewemail(action, id, isComplete);
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
                        $scope.actionRemarksList = [];
                        $scope.entity.modeOfService = "1";
                        $scope.updateContainerCategoryList($scope.entity.modeOfService)
                        $scope.entity.containerQty = "";
                        if ($scope.entity.pickupType == null) {
                            $scope.entity.pickupType = "1";
                        }
                        $scope.showPickupType($scope.entity.pickupType);
                        $scope.updateIsHaz($scope.entity.isHaz);
                        if ($scope.entity.billTo == 1) {
                            $scope.entity.billTo = "1";
                        }
                        if ($scope.entity.billTo == 2) {
                            $scope.entity.billTo = "2";
                        }
                        if ($scope.entity.licenseType == 1) {
                            $scope.entity.licenseType = "1";
                        }
                        if ($scope.entity.licenseType == 2) {
                            $scope.entity.licenseType = "2";
                        }
                        //clear container data
                        //for (var j = 1; j <= 23; j++) {
                        //    $scope['chkContainerType' + j] = false;
                        //    $scope['txtContainer' + j] = '';
                        //}
                        if ($scope.entity.nextActionRemarksDTOList != null && action != 'copy') {
                            $scope.actionRemarksList = $scope.entity.nextActionRemarksDTOList;
                        }
                        if (action == 'copy') {
                            $scope.entity.lastRemarks = '';
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

                        if ($scope.entity.enquiryContainerServiceDTOList != null) {
                            if ($scope.entity.enquiryContainerServiceDTOList.length > 0) {
                                $scope.entity.modeOfService = $scope.entity.enquiryContainerServiceDTOList[0].modeOfService;
                                $scope.updateContainerCategoryList($scope.entity.modeOfService)
                                $scope.containerServiceList = $scope.entity.enquiryContainerServiceDTOList;
                            }
                        }
                        //Fill Air grid
                        $scope.airServiceList = [];
                        if ($scope.entity.enquiryAIRServiceDTOList != null) {
                            if ($scope.entity.enquiryAIRServiceDTOList.length > 0) {
                                if ($scope.entity.enquiryContainerServiceDTOList == null) {
                                    //$scope.entity.modeOfService = "7";
                                    //$scope.updateContainerCategoryList($scope.entity.modeOfService)
                                }
                                $scope.airServiceList = $scope.entity.enquiryAIRServiceDTOList;
                                calculateAirTotal();
                            }
                        }
                        //Fill Break Bulk grid
                        $scope.breakBulkServiceList = [];
                        if ($scope.entity.enquiryBreakBulkServiceDTOList != null) {
                            if ($scope.entity.enquiryBreakBulkServiceDTOList.length > 0) {
                                if ($scope.entity.enquiryContainerServiceDTOList == null) {
                                    //$scope.entity.modeOfService = "5";
                                    //$scope.updateContainerCategoryList($scope.entity.modeOfService)
                                }

                                $scope.breakBulkServiceList = $scope.entity.enquiryBreakBulkServiceDTOList;
                                calculateBreakBulkTotal();
                            }
                        }
                        //Fill LCL grid
                        $scope.lclServiceList = [];
                        if ($scope.entity.enquiryLCLServiceDTOList != null) {
                            if ($scope.entity.enquiryLCLServiceDTOList.length > 0) {
                                if ($scope.entity.enquiryContainerServiceDTOList == null) {
                                    //$scope.entity.modeOfService = "2";
                                    //$scope.updateContainerCategoryList($scope.entity.modeOfService)
                                }

                                $scope.lclServiceList = $scope.entity.enquiryLCLServiceDTOList;
                                calculateLCLTotal();
                            }
                        }
                        //Fill RORO grid
                        $scope.roroServiceList = [];
                        if ($scope.entity.enquiryROROServiceDTOList != null) {
                            if ($scope.entity.enquiryROROServiceDTOList.length > 0) {
                                if ($scope.entity.enquiryContainerServiceDTOList == null) {
                                    //$scope.entity.modeOfService = "3";
                                    //$scope.updateContainerCategoryList($scope.entity.modeOfService)
                                }

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

        $scope.performEnquiryAction = function (source, fromList) {

            var action = source.currentTarget.attributes["action"].value;

            $scope.$broadcast("show-errors-check-validity");

            if (action != "cancel" && $scope.form.detail != undefined && $scope.form.detail.$invalid) {
                if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
                    ngNotifier.error("Required Field(s) are missing data.");
                }
                else if ($scope.form.detail.usrPwdC.$invalid) {
                    ngNotifier.error("Password do not match with Confirm Password.");
                }
                return;
            }

            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {
                if (action == "copy") {
                    var entities = [];
                    $scope.items.forEach(function (item) {
                        if (item.selected) {
                            entities.push(item);
                        }
                    });
                    if (entities.length == 0 || entities.length > 1) {
                        ngNotifier.error("Please, select one record to perform action.");
                        return false;
                    }
                    else {
                        $scope.showEnquiryDetail(action, entities[0].enquiryID, entities[0].isComplete);
                    }
                }
                else {
                    $scope.showEnquiryDetail(action, source.currentTarget.attributes["entityId"].value, source.currentTarget.attributes["isComplete"].value);
                }
            } else {
                initControls(action);
            }

            switchTab("Detail", action);

            switch (action) {
                case "search":
                    filterList();
                    break;
                case "add":
                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    $("input[input-date]").each(function (index, element) { $(element).val(null); });
                    break;
                case "copy":
                    lastAction = action;
                    //$scope.entity.enquiryID = 0;                    
                    break;
                    //lastAction = 'copy';
                    //$scope.entityId = 0;
                    //$scope.entity = {};
                    //$("input[input-date]").each(function (index, element) { $(element).val(null); });
                    break;
                case "edit":
                    lastAction = action;
                    break;
                case "save":
                    save(action);
                    break;
                case "saveEmail":
                    $scope.entity.isSendEmailNow = true;
                    save(action);
                    break;
                case "cancel":
                    $scope.showEnquiryDetail('viewDetail', $scope.entity.enquiryID, $scope.entity.isComplete);
                    lastAction = "";
                    break;
                case "delete":
                    remove();
                    lastAction = "";
                    break;
                case "deleteBatch":
                    removeBatch();
                    lastAction = "";
                    break;
                case "verify":
                case "activate":
                case "deactivate":
                    $scope.changeStatus(action);
                    lastAction = "";
                    break;
                case "viewemail":
                    viewemail('viewDetail', $scope.entity.enquiryID, $scope.entity.isComplete);
                    break;
                default:
                    lastAction = "";
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
        $scope.callCompanyModal = function (CompanyID) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId
            $scope.entity.fkCompanyID = CompanyID;
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
                            companyID: ($scope.entity.fkCompanyID || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.fkCompanyID = output.data.contactID;
                        $scope.entity.companyName = output.data.companyName;
                        $scope.entity.expAddress = output.data.address;
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
                templateUrl: "app/components/crm/enquiry/nextActionRemarks.html",
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
        //#endregion
       
        $scope.calculateHaz = function (unit) {
            var Weigthkgs = 0
            var Weigthlbs = 0
            var VolumeCM = 0
            var VolumeCF = 0
            if (unit == 'KGS') {
                if ($scope.entity.hazweight != null) {
                    Weigthkgs = $scope.entity.hazweight
                    var Rnd = Weigthkgs * 2.2046
                    $scope.entity.hazWeightLBS = Rnd
                }
            }
            if (unit == 'LBS') {
                if ($scope.entity.hazWeightLBS != null) {
                    Weigthlbs = $scope.entity.hazWeightLBS
                    var Rnd = Weigthlbs / 2.2046
                    $scope.entity.hazweight = Rnd
                }
            }
            if (unit == 'CM') {
                if ($scope.entity.hazVolume != null) {
                    VolumeCM = $scope.entity.hazVolume
                    var Rnd = VolumeCM * 35.3147
                    $scope.entity.hazVolumeCF = Rnd
                }
            }
            if (unit == 'CF') {
                if ($scope.entity.hazVolumeCF != null) {
                    VolumeCF = $scope.entity.hazVolumeCF
                    var Rnd = VolumeCF / 35.3147
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


        $scope.beforeFetchLookupData = function (moduleName, otherId, sortField, lookupKey) {
            var listParams = {
                OtherId: otherId,
                PageIndex: 1,
                PageSize: 10000,
                CwtId: $scope.userWorkTypeId,
                Sort: "{\"" + sortField + "\":\"asc\"}",
                Filter: "[]"
            };
            if (moduleName == "sipluser") {
                var filter = [];
                filter.push(Utility.createFilter("SitId", "numeric", "SitId", $scope.$parent.selectedSiteId, "contains", null));
                listParams.Filter = JSON.stringify(filter);
            }
            return listParams;
        };


        $scope.showAllRemarkDetail = function (obj) {
            $scope.entity = {};
            $scope.entityId = obj.enquiryID;
            $scope.entity.enquiryID = parseFloat(obj.enquiryID);
            $scope.entity.isComplete = parseInt(obj.isComplete);
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
                        $scope.actionRemarksList = [];
                        $scope.entity.modeOfService = "1";
                        $scope.entity.containerQty = "";
                        if ($scope.entity.nextActionRemarksDTOList != null) {
                            $scope.actionRemarksList = $scope.entity.nextActionRemarksDTOList;
                        }

                        $scope.showNextActionRemarksModel();

                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );

            }
            else {
                // $scope.goBack();
            }
        };
        $scope.downloadRemarkReport = function () {
            var listParams = {
                SiteId: $scope.$parent.selectedSiteId,
                ModuleId: $scope.page.moduleId,
                PageIndex: 0,
                PageSize: 10000,
                Sort: 'DESC',
                Filter: JSON.stringify($scope.searchParam)
            };


            entityService.exportReportRemark(listParams).then(
                function (output) {
                    var blobData = new Blob([output.data], { type: output.headers()["content-type"] });
                    var fileName = output.headers()["x-filename"];
                    saveAs(blobData, fileName);
                },
                function (output) {
                    ngNotifier.error(output);
                }
            );
        }

        $scope.isapprovedcheckboxChanged = function () {
            if ($scope.entity.isapprovedsel) {
                $scope.entity.isapproved_enq = 1;
            }
            else {
                $scope.entity.isapproved_enq = 0;
            }
        }
        
        $scope.showAllNextActionDateList = function (selectOption, searchBox) {
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/crm/enquiry/nextActionDateList.html",
                controller: "nextdateModelController",
                resolve: {
                    requestData: function () {
                        return {
                            startDate: $('[name="startNextActionDate"]').val(),
                            endDate: $('[name="endNextActionDate"]').val(),
                            selectOption: selectOption || "",
                            searchBox: searchBox || "",
                            SiteId: $scope.$parent.selectedSiteId
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {

                },
                function (output) {
                    ngNotifier.logError(output);
                });
            
        };
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("crmenquiryController", controller);

});
