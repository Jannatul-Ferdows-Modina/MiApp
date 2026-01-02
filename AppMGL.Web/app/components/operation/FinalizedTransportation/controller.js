// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "finalizedTransportationService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.finalizedTransportation;
        $scope.tabs = appUrl.finalizedTransportation.tabs;

        $scope.$parent.pageTitle = "Finalized Transportation";
        $scope.$parent.breadcrumbs = ["Shipment", "Trucking", "Finalized Transportation"];

        $scope.actionRemarksList = [];
        $scope.shipmentDocsList = [];
        $scope.isInvalidData = false;
        $scope.lookups = { siplDepartments: [] };

        $scope.initDropdown = function () {
            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
        };

        //#endregion       
        $scope.setLookups = function (source, lookup, output, index) {
            var telphArrary = new Array();            
            var faxArrary = new Array();
            if (lookup == "truckCompanyName") {
                $scope.entity.trucking_ContactID = output.data[0].contactID;
                $scope.entity.telNo1 = "";
                $scope.entity.telNo2 = "";
                $scope.entity.telNo3 = "";
                $scope.entity.fax1 = "";
                $scope.entity.fax2 = "";
                $scope.entity.fax3 = "";
                if (output.data[0].telNo != null && output.data[0].telNo != "" && output.data[0].telNo.indexOf("|") != "-1") {
                    telphArrary = output.data[0].telNo.split("|")
                    $scope.entity.telNo1 = telphArrary[0];
                    $scope.entity.telNo2 = telphArrary[1];
                    $scope.entity.telNo3 = telphArrary[2];
                }
                else {
                    $scope.entity.telNo1 = output.data[0].telNo;
                }
                if (output.data[0].fax != null && output.data[0].fax != "" && output.data[0].fax.indexOf("|") != "-1") {
                    faxArrary = output.data[0].fax.split("|")
                    $scope.entity.fax1 = faxArrary[0];
                    $scope.entity.fax2 = faxArrary[1];
                    $scope.entity.fax3 = faxArrary[2];
                }
                else {
                    $scope.entity.telNo1 = output.data[0].fax;
                }
            }
            else if (lookup == "loadingLocation") {
                var loadingAddress = "";
                $scope.entity.loading_ContactID = output.data[0].contactID;
                if (output.data[0].address != null) {
                    loadingAddress = output.data[0].address;
                } else { loadingAddress = ""; }
                if (output.data[0].cityName != null) {
                    loadingAddress = loadingAddress + "," + output.data[0].cityName;
                } else { loadingAddress = loadingAddress + ",";}
                if (output.data[0].stateName != null) {
                    loadingAddress = loadingAddress + "," + output.data[0].stateName;
                } else { loadingAddress = loadingAddress + ","; }
                if (loadingAddress == ",") { loadingAddress = "";}
                $scope.entity.loadingLocation = loadingAddress;
            }
        };

        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "truckCompanyName") {
                $scope.entity.trucking_ContactID = null;                
            }
            else if (lookup == "loadingLocation") {
                if ($scope.isInvalidData == true) {
                    $scope.entity.loading_ContactID = null;
                    $scope.entity.loadingLocation = "";
                }
            }
        }

        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {

            if (lookupModule == "SIPLContact" || lookupModule == "exporter" || lookupModule == "pickupLocation" || lookupModule == "returnLocation" || lookupModule == "consignedTo" || lookupModule == "carrierBooking" || lookupModule == "originCountry" || lookupModule == "dischargeCountry" || lookupModule == "originState" || lookupModule == "dischargeState" || lookupModule == "originCity" || lookupModule == "dischargeCity" || lookupModule == "originPort" || lookupModule == "dischargePort" || lookupModule == "RailRamp" || lookupModule == "Terminal") {

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


        $scope.searchOptions = [
                { optionValue: "", optionName: "-All-" },
                { optionValue: "SystemRefNo", optionName: "System Ref No" },               
                { optionValue: "LineBookingNo", optionName: "Line Booking No" }
        ];

        $scope.filterBookings = function () {
            var dashboardOption = localStorageService.get("dashboardOption");
            $scope.selectOption = "companyName";
            $scope.searchBox = "";
            if (dashboardOption != null) {
                if (dashboardOption == 'TPFTU') {
                    $scope.dashboardOption = "TPFTU";
                }
                if (dashboardOption == 'TPFTF') {
                    $scope.dashboardOption = "";
                }

                localStorageService.remove("dashboardOption");
            }
            else {
                $scope.dashboardOption = "";
            }

        };
        $scope.filterBookings();

        $scope.selectOption = "SystemRefNo";
        $scope.searchBox = "";
        $scope.departmentID = 0

        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            departmentID: 0,
            dashboardOption: $scope.dashboardOption
        };



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

                      var dataitems = entityService.getFinalizedTransportationList(listParams).then(
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
        $scope.performBookingSearch = function (source, selectOption, searchBox, departmentValue) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox,
                departmentID: 0,
                dashboardOption:""
            };
            $scope.bookinglistTable.reload();
        };

        $scope.performBookingAction = function (source, fromList) {

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
                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    $scope.getFinalizedTransportDetails(action, documentCommonID);
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
                    deleteShipmentDoc(enquiryID, quotationID);
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
                    $scope.getcarrierAllRates($scope.entity.enquiryID);


                    break;

            }
        };

      

        $scope.saveFinalizedTransport = function (source, fromList) {

            if ($scope.entity.fileNo == "" || $scope.entity.fileNo == null) {
                ngNotifier.error("Please enter System Ref");
                return;
            }
            if ($scope.entity.trucking_ContactID == null) {
                ngNotifier.error("Please enter Trucking Company Name");
                return;
            }
            if ($scope.entity.loading_ContactID == null) {
                ngNotifier.error("Please enter Loading Location");
                return;
            }

            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.createdBy = $scope.$parent.authentication.userId;
            $scope.entity.updatedBy = $scope.$parent.authentication.userId;
            entityService.saveFinalizedTransport($scope.entity).then(
                function (output) {
                    $scope.documentID = output.data.data;                    
                    $scope.entity = {};
                    //$scope.actionRemarksList = [];
                    //$scope.selectOption = "SystemRefNo";
                    //$scope.searchBox = "";
                    //$scope.bookinglistTable.reload();
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
            $scope.$parent.pageTitle = "Dock Receipt";
            $scope.$parent.breadcrumbs = ["Document", "Dock Receipt"];
            $location.path("/document/dockReceipt");
        };

        $scope.getFinalizedTransportDetails = function (source, documentCommonID) {
            var action = source.currentTarget.attributes["action"].value;
            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entity = {};
            $scope.entity.documentCommonID = documentCommonID;

            entityService.getFinalizedTransportDetails($scope.entity).then(
                 function (output) {
                     if (output.data.resultId == 2005) {
                         ngNotifier.showError($scope.authentication, output);
                         $scope.logOut()
                     }
                     $scope.entity = output.data.data;                     
                     $scope.actionRemarksList = [];
                     if ($scope.entity.nextActionRemarksDTOList != null) {
                         $scope.actionRemarksList = $scope.entity.nextActionRemarksDTOList;
                     }
                     //$scope.afterGetDetail(action);
                 },
                 function (output) {
                     ngNotifier.showError($scope.authentication, output);
                 }
             );
        };

        $scope.showNextActionRemarksModel = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/bookingCaptureContainer/nextActionRemarks.html",
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

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("finalizedTransportationController", controller);

});
