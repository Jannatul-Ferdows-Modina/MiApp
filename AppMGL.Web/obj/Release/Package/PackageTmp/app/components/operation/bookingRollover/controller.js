// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "bookingRolloverService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.bookingRollover;
        $scope.tabs = appUrl.bookingRollover.tabs;
        
        $scope.actionRemarksList = [];
        $scope.shipmentDocsList = [];

        $scope.lookups = { siplDepartments: [] };

        $scope.initDropdown = function () {
            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
        };
        $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey == "siplDepartments") { $scope.lookups.siplDepartments.unshift({ "departmentID": 0, "department": "" }); } //
        }
        //#endregion       

        $scope.searchOptions = [
                { optionValue: "", optionName: "-All-" },                
                { optionValue: "SystemRefNo", optionName: "System Ref No" },                
                { optionValue: "LineBookingNo", optionName: "Line Booking No" }
        ];
        $scope.selectOption = "SystemRefNo";
        $scope.searchBox = "";
        $scope.departmentID = 0;

        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            departmentID: 0
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

                      var dataitems = entityService.getRolledOverList(listParams).then(
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
                departmentID: departmentValue
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
                    $scope.getDocumentAttachmentDetail(action, documentCommonID);
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

        $scope.cancelRollOver = function (documentCommonID) {
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;            
            ngNotifier.confirm("Are you sure you want to ReInstate this record?", null, function () {                
                entityService.cancelRollOver($scope.entities).then(
                            function (output) {                                
                                //$scope.entity = {};
                                $scope.bookinglistTable.reload();
                                //$scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });


            });

        };    



        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("bookingRolloverController", controller);

});
