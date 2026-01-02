"use strict";

define(["app"], function (app) {


    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "crmenquiryService", "requestData"];
    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData) {
        $scope.entity = {};
        $scope.entity.modeOfService = '1';
        $scope.enquiryID;
        $scope.deleteRemarks = "";
        $scope.isComplete = false;
        $scope.airServiceList = [];
        $scope.breakBulkServiceList = [];
        $scope.containerServiceList = [];
        $scope.containerServiceListDDL = [];
        //$scope.fclServiceList = [];
        $scope.lclServiceList = [];
        $scope.roroServiceList = [];
        $scope.searchResult = [];

        $scope.actionRemarksList = [];
        $scope.isHaz = false;
        $scope.containerCategoryList = [];
        $scope.containerSizesList = [];
        $scope.isCarrierDisabled = true;
        $scope.isDepartmentDisabled = true;
        $scope.isInvalidData = false;
        $scope.isGalBookingStatusVisible = false;
        $scope.isCarrierChargesVisible = true;
        $scope.documentationStatusList = [];
        $scope.carrierChargesList = [];
        $scope.allsite = [];
        var lastAction = "";
        $scope.siteid = requestData.siteid;
        $scope.enqentity = requestData.entity;
        $scope.fdata = [];

        $scope.lookups = { siplDepartments: [], carriers: [], siplUsers: [], commodityTypes: [], commodities: [], miamiBookingStatus: [], LGVWPorts: [], containerSizes: [], containerCategories: [], documentationStatus: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupDataBooking("sipldepartment", 0, "displayOrder", "siplDepartments", null);
            $scope.fetchLookupDataBooking("siplUser", 0, "name", "siplUsers", null);
            $scope.fetchLookupDataBooking("commodityType", 0, "commodityType", "commodityTypes", null);
            $scope.fetchLookupDataBooking("Commodity", 0, "name", "commodities", null);
            $scope.fetchLookupDataBooking("siplBookingStatus", 0, "Status", "miamiBookingStatus", null);
            $scope.fetchLookupDataBooking("siplContact", 0, "companyName", "siplContact", null);
            $scope.fetchLookupDataBooking("LGVWPort", 0, "name", "lgvwPorts", null);
            $scope.getContainerCategories();
            $scope.getContainerSizes();
            $scope.getAllCarriers();
            $scope.getDocumentationStatus();
            $scope.getAllSite();
            $scope.entity = {};
            $scope.showBookingDetail('view', requestData.enquiryID, requestData.quotationID, requestData.documentCommonID);
            
        };



        $scope.fetchLookupDataBooking = function (moduleName, otherId, sortField, lookupKey, lookupMethod) {
            var listParams = {
                OtherId: otherId,
                PageIndex: 1,
                PageSize: 60000,
                CwtId: $scope.userWorkTypeId,
                Sort: "{\"" + sortField + "\":\"asc\"}",
                Filter: "[]"
            };
            if ($scope.beforeFetchLookupData != undefined) {
                listParams = $scope.beforeFetchLookupData(moduleName, otherId, sortField, lookupKey);
            }
            entityService.lookup(moduleName, lookupMethod, listParams).then(
                function (output) {
                    $scope.lookups[lookupKey] = output.data.data;
                    if ($scope.afterFetchLookupData != undefined) {
                        $scope.afterFetchLookupData(lookupKey);
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
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
        };
        $scope.getAllCarriers = function () {
            var getCurrencies = entityService.getAllCarriers().then(
                function (output) {
                    $scope.lookups.carriers = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

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

        $scope.getAllSite = function () {
            var getsite = entityService.getAllSite().then(
                function (output) {
                    $scope.allsite = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };


       

        $scope.showBookingDetail = function (action, enquiryID, quotationID, documentCommonID) {

          
           
            $scope.entity.enquiryID = enquiryID;
            $scope.entity.quotationID = quotationID;
            $scope.entity.documentCommonID = documentCommonID;
            $scope.quotationID = quotationID;
            $scope.entity.siteId = $scope.siteid;
            if ($scope.entity.enquiryID > 0 && $scope.entity.quotationID > 0) {
                entityService.getBookingDetail($scope.entity).then(
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
                         $scope.entity.modeOfService = "1";
                         $scope.updateContainerCategoryList($scope.entity.modeOfService)
                         $scope.entity.containerQty = "";
                         $scope.updateIsHaz($scope.entity.isHaz);
                         if ($scope.entity.pickupCategory == null) {
                             $scope.entity.pickupCategory = "1";
                         }
                         //
                         if ($scope.entity.carrierChargesDTOList != null) {
                             $scope.carrierChargesList = $scope.entity.carrierChargesDTOList;
                         }

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
                         //$scope.entity.modeOfService = "1";
                         //Fill Container details
                         var total = 0
                         
                         if ($scope.entity.enquiryContainerServiceDTOList != null) {
                             if ($scope.entity.enquiryContainerServiceDTOList.length > 0) {
                                 $scope.entity.modeOfService = $scope.entity.enquiryContainerServiceDTOList[0].modeOfService;
                                 $scope.updateContainerCategoryList($scope.entity.modeOfService);
                                 $scope.containerServiceList = $scope.entity.enquiryContainerServiceDTOList;
                                 $scope.entity.enquiryContainerServiceDTOList.forEach(function (o) {
                                     $scope.containerServiceListDDL.push(o);

                                 });


                                 $scope.containerServiceList.forEach(function (o) {
                                     total = total + o.quantity
                                 });
                                 $scope.entity.noOfContainer = total;
                                 $scope.updateContainerCategoryList($scope.entity.modeOfService);
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


        $scope.updateContainerCategoryList = function (modeOfService) {
            debugger;
            // $scope.containerServiceList = [];
            $scope.containerCategoryList = [];
            $scope.containerSizesList = [];
            if (modeOfService == '1') {

                $scope.lookups.containerCategories.forEach(function (item) {
                    $scope.containerServiceListDDL.forEach(function (item1) {
                        {
                            if (item.modeOfServiceID == modeOfService && item1.categoryName == item.categoryName) {
                                $scope.containerCategoryList.push(item);
                            }
                        }
                    });
                });
            } else {
                $scope.lookups.containerCategories.forEach(function (item) {
                    if (item.modeOfServiceID == modeOfService) {
                        $scope.containerCategoryList.push(item);
                    }
                });
            }
        };



       





        


        $scope.closeModel = function (action) {
            $scope.SearchQuotationResult = [];
            var outputData = {}
            outputData.action = 'close';
            outputData.resultId = 1001;
            outputData.finalQuotations = $scope.finalQuotations;
            $uibModalInstance.close(outputData);
        };

        $scope.select = function (action) {

            if (action == 'cancel') {
                $scope.cancel = 0;
                return;
            }


            $uibModalInstance.close();
        };


        angular.extend(this, new modalController($scope, $filter, $timeout, $routeParams, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData));

    };

    controller.$inject = injectParams;

    app.register.controller("bookingdetailController", controller);

});