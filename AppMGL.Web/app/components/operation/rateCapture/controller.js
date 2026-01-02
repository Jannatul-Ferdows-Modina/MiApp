// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "rateCaptureService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.rateCapture;
        $scope.tabs = appUrl.rateCapture.tabs;

        $scope.$parent.pageTitle = "Rate Capture";
        $scope.$parent.breadcrumbs = ["Contract", "Rate Capture"];

        $scope.dispatchContainerList = [];
        $scope.actionRemarksList = [];
        $scope.isInvalidData = false;
        
        $scope.searchResult = [];
        $scope.isRouteTypeDisabled = false;

        $scope.containerCategoryList = [];
        $scope.containerSizesList = [];
        $scope.containerServiceList = [];
        $scope.containerChargesList = [];
        $scope.tempChargesList = [];
        $scope.isVisible = false;
        $scope.lookups = { siplContact: [], commodityTypes: [], commodities: [], carrierAllCharges: [], containerSizes: [], containerCategories: [], carrierContracts: [] };

        $scope.initDropdown = function () {
            $scope.fetchLookupData("siplContact", 28, "companyName", "siplContact", null);
            $scope.fetchLookupData("commodityType", 0, "commodityType", "commodityTypes", null);
            $scope.fetchLookupData("Commodity", 0, "name", "commodities", null);

            //$scope.getAllCarriers();
            $scope.getCarrierAllCharges();
            $scope.getContainerCategories();
            $scope.getContainerSizes();

        };

        $scope.modeOfServices = [                 
                 { optionValue: "1", optionName: "FCL" },
                 { optionValue: "2", optionName: "LCL" },
                 { optionValue: "3", optionName: "RORO" },                 
                 { optionValue: "5", optionName: "BREAK BULK" },
                 { optionValue: "7", optionName: "AIR" }
        ];
       

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
        $scope.getCarrierAllCharges = function () {
            var getCarrierCharges = entityService.getCarrierAllCharges().then(
                function (output) {
                    $scope.lookups.carrierAllCharges = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }

        $scope.getCarrierName = function (selectedCarrier) {
            $scope.selectedCarrierName = $scope.entity.selectedCarrier.companyName;
            $scope.selectedCarrierId = $scope.entity.selectedCarrier.contactID;
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
            if ($scope.entity.chargeItem == null) {
                ngNotifier.error("Please select Charge Type");
                return;
            }
            if ($scope.entity.chargeExpiryDate == null) {
                ngNotifier.error("Please select charge ExpiryDate");
                return;
            }
            if ($scope.entity.buyingAmt == '' || $scope.entity.buyingAmt <= 0) {
                ngNotifier.error("Please Enter Valid Buying Amount");
                return;
            }
            if ($scope.entity.categoryItem != null && $scope.entity.containerTypeItem != null && $scope.entity.chargeItem != null && $scope.entity.chargeExpiryDate != null && $scope.entity.buyingAmt > 0) {
                containerServiceItem.categoryID = $scope.entity.categoryItem.categoryID;
                containerServiceItem.categoryName = $scope.entity.categoryItem.categoryName;
                containerServiceItem.containerName = $scope.entity.containerTypeItem.name;
                containerServiceItem.containerTypeID = $scope.entity.containerTypeItem.containerTypeId;
                containerServiceItem.chargeId = $scope.entity.chargeItem.tradeServiceId;
                containerServiceItem.chargeName = $scope.entity.chargeItem.description;
                containerServiceItem.chargeExpiryDate = $scope.entity.chargeExpiryDate;
                containerServiceItem.buyingAmt = $scope.entity.buyingAmt;                
                $scope.containerServiceList.push(containerServiceItem);
                updateContainerTotal();
            }
            else {
                ngNotifier.error("Please select valid values");
                return;
            }

            //calculateAirTotal();
            $scope.entity.buyingAmt = '';

        };

        $scope.removeContainerServiceRow = function (rownum) {
            $scope.containerServiceList.splice(rownum, 1);
            updateContainerTotal();
            //calculateAirTotal();
        }

        var updateContainerTotal = function () {

            $scope.tempChargesList = [];
            var containerServiceItem = {};
            $scope.containerServiceList.forEach(function (containerItem) {
                if ($scope.tempChargesList.length == 0) {
                    containerServiceItem = {};
                    containerServiceItem.categoryName = containerItem.categoryName;
                    containerServiceItem.containerName = containerItem.containerName;
                    containerServiceItem.total = containerItem.buyingAmt;
                    $scope.tempChargesList.push(containerServiceItem);
                }
                else {
                    $scope.tempChargesList.forEach(function (chargeItem) {
                        if (containerItem.categoryName == chargeItem.categoryName && containerItem.containerName == chargeItem.containerName) {
                            chargeItem.total = chargeItem.total + containerItem.buyingAmt;
                        }                       
                    });
                    if (isRecExits(containerItem.categoryName, containerItem.containerName) == false) {
                        containerServiceItem = {};
                        containerServiceItem.categoryName = containerItem.categoryName;
                        containerServiceItem.containerName = containerItem.containerName;
                        containerServiceItem.total = containerItem.buyingAmt;
                        $scope.tempChargesList.push(containerServiceItem);
                    }
                }
            });
            $scope.containerChargesList = $scope.tempChargesList;

        };
        
        var isRecExits = function (category, container) {
            var itemExists = false;
            $scope.tempChargesList.forEach(function (chargeItem) {
                if (category == chargeItem.categoryName && container == chargeItem.containerName) {
                    itemExists = true;
                    return true;
                }
            });
            if (itemExists == true)
            {
                return true;
            }
            else {
                return false;
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
        $scope.searchValues = function (viewValue, selectOption) {
            var resultItem = {};
            if (selectOption == "companyName") {
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
            if (selectOption == "state" || selectOption == "originState" || selectOption == "dischargeState") {
                lookupModule = "LGVWState";
            }
            if (selectOption == "city" || selectOption == "originCity" || selectOption == "dischargeCity") {
                lookupModule = "LGVWCity";
            }
            if (selectOption == "originCountry" || selectOption == "dischargeCountry") {
                lookupModule = "SIPLCountry";
            }
            if (selectOption == "portOfOrigin" || selectOption == "portOfdischarge") {
                lookupModule = "LGVWPort";
            }
            if (selectOption == "commodity") {
                lookupModule = "commodity";
            }

            if (selectOption == "state" || selectOption == "originState" || selectOption == "dischargeState" || selectOption == "city" || selectOption == "originCity" || selectOption == "dischargeCity" || selectOption == "originCountry" || selectOption == "dischargeCountry" || selectOption == "commodity" || selectOption == "portOfOrigin" || selectOption == "portOfdischarge") {
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

        
      
        $scope.carrierName = "";
        $scope.origin = "";
        $scope.destination = "";
        $scope.originCountry = "";
        $scope.destinationCountry = "";
        $scope.originState = "";
        $scope.destinationState = "";
        $scope.originCity = "";
        $scope.destinationCity = "";
        $scope.commodity = "";
       

        $scope.searchParam = {
            carrierName: $scope.carrierName,
            origin: $scope.origin,
            destination: $scope.destination,
            originCountry: $scope.originCountry,
            destinationCountry: $scope.destinationCountry,
            originState: $scope.originState,
            destinationState: $scope.destinationState,
            originCity: $scope.originCity,
            destinationCity: $scope.destinationCity,
            commodity: $scope.commodity
        };

        

        $scope.rateCapturelistTable = new NgTableParams(
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

                      var dataitems = entityService.getRateCaptureList(listParams).then(
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
        $scope.performRouteSearch = function (source, companyName, origin, discharge, originCountry, originState,originCity,destinationCountry,destinationState, destinationCity,commodity) {

            var action = source.currentTarget.attributes["action"].value;
            if (companyName == null) {
                { companyName = "" }
            }
            if (origin == null) {
                { origin = "" }
            }
            if (discharge == null) {
                { discharge = "" }
            }
            if (originCountry == null) {
                { originCountry = "" }
            }
            if (originState == null) {
                { originState = "" }
            }
            if (originCity == null) {
                { originCity = "" }
            }
            if (destinationCountry == null) {
                { destinationCountry = "" }
            }
            if (destinationState == null) {
                { destinationState = "" }
            }
            if (destinationCity == null) {
                { destinationCity = "" }
            }
            if (commodity == null) {
                { commodity = "" }
            }
            $scope.searchParam = {
                carrierName: companyName,
                origin: origin,
                destination: discharge,
                originCountry: originCountry,
                destinationCountry: destinationCountry,
                originState: originState,
                destinationState: destinationState,
                originCity: originCity,
                destinationCity: destinationCity,
                commodity: commodity
            };
            $scope.rateCapturelistTable.reload();
        };

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
            else if (lookup == "SIPLContactAdd") {
                $scope.entity.carrierID = output.data[0].contactID;
                $scope.getCarrierContracts($scope.entity.carrierID);
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
            else if (lookup == "SIPLContactAdd") {
                $scope.entity.carrierID = null;
                
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


        $scope.performCaputreRouteAction = function (source, fromList) {

            var action = source.currentTarget.attributes["action"].value;

            

            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {

                ///$scope.showBookingDetail(action, documentCommonID);

            } else {
                initControls(action);
            }

            //switchTab("Detail", action);

            switch (action) {
                case "search":
                    filterList();
                    break;
                case "add":
                    //lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    //$scope.showPendingMovementDetails(action, documentCommonID, fileno);
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
                    //$scope.entity.isSendEmailNow = true;
                    //save(action);
                    break;
                case "cancel":
                    //$scope.showQuotationDetail(action, enquiryID, quotationID);
                    //$scope.showEnquiryDetail('viewDetail', $scope.entity.enquiryID, $scope.entity.isComplete);
                    //lastAction = "";
                    $scope.goBack();
                    break;
                case "delete":
                    remove();
                    //lastAction = "";
                    break;
                case "deleteBatch":
                    removeBatch();
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

        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
        };
        var initControls = function (action) {

            switch (action) {
                case "add":
                    $scope.editMode = true;
                    $scope.disabledInsert = false;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = true;
                    $scope.requiredUpdate = true;
                    $scope.isVisible = false;
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

        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":

                    break;
                case "edit":
                    //$scope.getcarrierAllRates($scope.entity.enquiryID);
                    $scope.getCarrierContracts($scope.entity.carrierID)


                    break;

            }
        };

        var removeBatch = function () {
            var entities = [];
            $scope.items.forEach(function (item) {
                if (item.selected) {
                    entities.push(item);
                }
            });
            if (entities.length === 0) {
                ngNotifier.info("Please, select atleast one record to perform action.");
            }
            else {
                ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                    entities.forEach(function (entity) {                       
                       
                        entityService.deleteContractRate(entity).then(
                            function (output) {
                                $scope.entity = {};
                                $scope.rateCapturelistTable.reload();
                                $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });
                    });
                });
            }
        };

        $scope.saveContractRate = function (source, fromList) {
            
            if ($scope.entity.carrierID == null) {
                ngNotifier.error("Please select Carrier");
                return;
            }           

            if ($scope.entity.contractID == null) {
                ngNotifier.error("Please select Contract No");
                return;
            }
            //Fill commodity
            $scope.fillEnquiryCommodityDetail();
            if ($scope.entity.commodityDTOList.length == 0) {
                ngNotifier.error("Please select Commodity");
                return;
            }

            if ($scope.entity.effectiveDate == null) {
                ngNotifier.error("Please select Valid EffectiveDate");
                return;
            }
            if ($scope.entity.expiryDate == null) {
                ngNotifier.error("Please select Valid ExpiryDate");
                return;
            }
            if ($scope.containerServiceList.length == 0)
            {
                ngNotifier.error("Please add Container Charges");
                return;
            }
            else
            {
                $scope.entity.containerChargesList = $scope.containerServiceList;
            }
            
            if ($scope.entity.contractRateID == null)
            {
                $scope.entity.contractRateID = 0;
            }
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.CreatedBy = $scope.$parent.authentication.userId;
            $scope.entity.updatedBy = $scope.$parent.authentication.userId;
            entityService.saveRateCapture($scope.entity).then(
                function (output) {
                    //$scope.enquiryID = output.data.data;
                    $scope.entity = {};
                    
                    //$scope.selectOption = "RouteName";
                    //$scope.searchBox = "";
                    $scope.rateCapturelistTable.reload();
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

        $scope.getCarrierContracts = function (carrierID) {
            var getcontractNos = entityService.getCarrierContracts(carrierID).then(
                function (output) {
                    if (output.data != null) {
                        $scope.lookups.carrierContracts = output.data.data;
                    }

                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        $scope.contractSelect = function () {

            var selcont = $scope.entity.contractID;
            if (selcont === 0) {
                $scope.entity.contractID = null;
                return false;
            }
            var modecontract = $scope.lookups.carrierContracts.find(x => x.contractID === selcont);
            $scope.entity.effectiveDate = setDateFormat(modecontract.startDate);
            $scope.entity.expiryDate = setDateFormat(modecontract.endDate);

            $scope.entity.scTermNo = (modecontract.scTermNo === null ? '' : modecontract.scTermNo);
            $scope.entity.tradeLane = (modecontract.tradeLane === null ? '' : modecontract.tradeLane);

        };


        function setDateFormat(ddate) {

            ddate = ddate.replace('T00:00:00', '');
            var arraydate = ddate.split('-');
            var dyear = arraydate[0];
            var dmonth = arraydate[1];
            var mdate = arraydate[2];
            return dmonth + '/' + mdate + '/'+ dyear;
        }

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

        var resetFilterField = function () {

            $scope.entity.miamiRefNo = ($scope.entity.filterFieldName == "miamiRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.systemRefNo = ($scope.entity.filterFieldName == "systemRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.bookingNo = ($scope.entity.filterFieldName == "bookingNo") ? $scope.entity.filterFieldValue : "";
        };

        $scope.getRateCaptureDetails = function (source, contractRateID) {

           // var action = source.currentTarget.attributes["action"].value;
            $scope.onClickTab($scope.tabs[0]);
            viewDetail();            
            initControls(source);
            
            
            $scope.entity = {};
            $scope.entity.contractRateID = contractRateID;
                     
            $scope.entity.siteId = $scope.$parent.selectedSiteId;

            entityService.getRateCaptureDetails($scope.entity).then(
                 function (output) {
                     if (output.data.resultId == 2005) {
                         ngNotifier.showError($scope.authentication, output);
                         $scope.logOut()
                     }
                     $scope.entity = output.data.data;
                     $scope.isVisible = true;
                     //Fill commodity details
                     var temp = new Array();
                     if ($scope.entity.commodityDTOList != null) {
                         if ($scope.entity.commodityDTOList.length > 0) {
                             $scope.entity.commodityTypeID = $scope.entity.commodityDTOList[0].commodityTypeID;
                             
                             for (var i = 0; i < $scope.entity.commodityDTOList.length; i++) {
                                 temp[i] = parseInt($scope.entity.commodityDTOList[i].commodityId);
                             }
                             $scope.entity.commodityIds = temp;
                         }
                     }
                     //Fill Container Charges
                     if ($scope.entity.containerChargesList != null) {
                         if ($scope.entity.containerChargesList.length > 0) {
                             $scope.containerServiceList = $scope.entity.containerChargesList;
                             updateContainerTotal();
                         }
                     }                     
                     
                     $scope.afterGetDetail(source);
                    
                 },
                 function (output) {
                     ngNotifier.showError($scope.authentication, output);
                 }
             );
        };

        $scope.afterGetDetail = function (action) {
            //if ($scope.entity.isComplete == "1")
            //    $scope.isComplete = true;
            //else
            //    $scope.isComplete = false;
            $scope.getCarrierContracts($scope.entity.carrierID)
        };


        $scope.callContractRateModalRate = function (source) {

            $scope.$parent.selectedSiteId
            $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/contractRateModal/detail.html",
                controller: "contractRateModalController",
                resolve: {
                    requestData: function () {

                        return {
                            contractID: (source || 0),
                            SitId: $scope.$parent.selectedSiteId,
                            isupload:0
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.contractID = output.data.contractID;

                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        $scope.getRateCapturebreakupDetails = function (typ, contractRateID) {
                     
            entityService.getRateCapturebreakupDetails(typ, contractRateID).then(
                function (output) {
                    if (output.data.resultId == 2005) {
                        ngNotifier.showError($scope.authentication, output);
                        $scope.logOut();
                    }
                    var datalist = output.data.data;
                    //Fill Container Charges
                    if (datalist.containerChargesList != null) {
                        if (datalist.containerChargesList.length > 0) {


                            var modalInstance = $uibModal.open({
                                animation: false,
                                backdrop: "static",
                                keyboard: false,
                                size: "lg",
                                templateUrl: "app/components/operation/rateCapture/ContainerChargeList.html",
                                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                                    var ContractRateID = requestData.ContractRateID;
                                    var Typ = requestData.Typ;

                                    $scope.containermodelChargesList = requestData.containerServiceList;
                                    updateContainerTotal();
                                    $scope.closeModel = function (action) {
                                        var outputData = {}

                                        $uibModalInstance.close();
                                    };


                                },
                                resolve: {
                                    requestData: function () {
                                        return {
                                            ContractRateID: contractRateID,
                                            Typ: typ,
                                            containerServiceList: datalist.containerChargesList
                                        };
                                    }
                                }
                            });

                            
                        }
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );


           

            //var modalInstance = $uibModal.open({
            //    animation: false,
            //    backdrop: "static",
            //    keyboard: false,
            //    size: "lg",
            //    templateUrl: "app/components/operation/rateCapture/ContainerChargeList.html",
            //    controller: "ratecaptureModelController",
            //    resolve: {
            //        requestData: function () {
            //            return {
            //                contractRateID: (contractRateID || 0),
            //                typ: typ
            //            };
            //        }
            //    }
            //});
            //modalInstance.result.then(
            //    function (output) {
            //        if (output.resultId == 1001) {
                      

            //        }
            //    },
            //    function (output) {
            //        ngNotifier.logError(output);
            //    });

            


           
        };


        $scope.showTooltip = function (contractRateId,typ) {            
            var obj = '#tooltipElement'+ typ+ '_' + contractRateId;
            $(obj).show();
        };
        $scope.hideTooltip = function (contractRateId,typ) {
            var obj = '#tooltipElement' + typ + '_' + contractRateId;
            $(obj).hide();
        };


        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("rateCaptureController", controller);

});
