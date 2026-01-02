"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "quotationService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {

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

        var lastAction = "";        

        //#region Lookup 

        $scope.lookups = { siplDepartments: [], siplUsers: [], commodityTypes: [], commodities: [], containerSizes: [], containerCategories: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
            $scope.fetchLookupData("sipluser", 0, "name", "siplUsers", null);
            $scope.fetchLookupData("commodityType", 0, "commodityType", "commodityTypes", null);
            $scope.fetchLookupData("Commodity", 0, "name", "commodities", null);
            //$scope.fetchLookupData("ContainerType", 0, "name", "containerTypeList", null);
            //$scope.getContainerCategories();
            //$scope.getContainerSizes();
        };

        //#endregion

        //#region Detail Method

        $scope.enquiryEntityId = (requestData.enquiryId) ? requestData.enquiryId : 0;


        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
            }
            else {
                $scope.entity.modifiedOn = new Date();
            }
        };

        $scope.closeEnquiryModel = function (action) {
            var outputData = {}

            $uibModalInstance.close();
        };
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
        $scope.updateIsHaz = function (isHazValue) {
            if (isHazValue) {
                $scope.isHaz = true;
            }
            else {
                $scope.isHaz = false;
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
        $scope.showEnquiryDetail = function (enquiryId) {
            
            $scope.entity.enquiryID = parseFloat(enquiryId);
            $scope.entity.isComplete = 1;
            $scope.entity.siteId = $scope.$$prevSibling.selectedSiteId;
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
                     if ($scope.entity.nextActionRemarksDTOList != null) {
                         $scope.actionRemarksList = $scope.entity.nextActionRemarksDTOList;
                     }
                     //Fill commodity details
                     var temp = new Array();
                     if ($scope.entity.commodityDTOList != null) {
                         if ($scope.entity.commodityDTOList.length > 0) {
                             $scope.entity.commodityTypeID = $scope.entity.commodityDTOList[0].commodityTypeID
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

                     //$scope.afterGetDetail(action);
                     $scope.disabledUpdate = true;
                 },
                 function (output) {
                     ngNotifier.showError($scope.authentication, output);
                 }
             );

        };

        //#endregion
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


        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("enquiryModelController", controller);

});
