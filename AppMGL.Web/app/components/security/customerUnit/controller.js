"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "customerUnitService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.customerUnit;
        $scope.tabs = appUrl.customerUnit.tabs;
        $scope.modalData = [];
        $scope.lookups = { units: [], categories: [], feeCategories: [], spFeeCategory: [] };
        $scope.initDropdown = function () {
            $scope.lookups.units = $scope.$parent.authentication.userSite;
            $scope.fetchLookupData("contactCategory", 0, "name", "categories", null);
            $scope.fetchLookupData("lgSPFEECategory", 0, "sfcName", "feeCategories", null);
            $scope.fetchLookupData("lgSPFEECategory", 0, "sfcName", "spFeeCategory", null);
            //$scope.getFeeCategories()
        };
        $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey != "feeCategories" && lookupKey != "categories") { $scope.lookups.units.unshift({ "SitId": "", "SitName": "" }) };
            if (lookupKey == "feeCategories") {
                $scope.lookups.feeCategories.unshift({ "sfcID": 1, "sfcName": "Unassigned" });
                $scope.lookups.feeCategories.unshift({ "sfcID": 0, "sfcName": "" });                
            };
            if (lookupKey == "spFeeCategory") {
                $scope.lookups.spFeeCategory.unshift({ "sfcID": "", "sfcName": "" });
            }
            if (lookupKey == "categories") { $scope.lookups.categories.unshift({ "contactCategoryId": "", "name": "" }); }
        }
        $scope.searchParam = {
            companyName: $scope.companyName,
            customerCode: $scope.customerCode,
            contactCategoryID: ($scope.contactCategoryId) ? $scope.contactCategoryId.join() : '',
            unitId: $scope.refUnitId,
            refUnitId: $scope.refUnitId,
            sfcID: $scope.sfcID
        };

        $scope.searchValues = function (viewValue, selectOption) {
            var resultItem = {};
            var lookupField = "companyName";

            if (selectOption == "companyName") {

                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getCompanySearch(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}

                            resultItem.companyName = o.companyName;
                            resultItem.contactID = o.contactID;
                            resultItem.address = o.address;

                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }

        };
       //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.rleCreatedTs = new Date();
                $scope.entity.rleCreatedBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.rleUpdatedTs = new Date();
                $scope.entity.rleUpdatedBy = $scope.$parent.userInfo.usrId;
            }
        };

        //$scope.getFeeCategories = function () {
        //    var getFeeCategory = entityService.getFeeCategories().then(
        //        function (output) {
        //            $scope.lookups.feeCategories = output.data.data;
        //        },
        //        function (output) {
        //            ngNotifier.showError($scope.authentication, output);
        //        }
        //    );
        //}

       
        
        $scope.performCustomerUnitSearch = function (source, companyName, customerCode, contactCategoryId, unitId, refUnitId, sfcID) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                companyName: companyName,
                customerCode: customerCode,
                contactCategoryID: (contactCategoryId) ? contactCategoryId.join() : '',
                unitId: unitId,
                refUnitId: refUnitId,
                sfcID: sfcID
            };
            $scope.customerUnitListTable.reload();
        };

        $scope.customerUnitListTable = new NgTableParams(
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

                    var dataitems = entityService.getContactsUnitList(listParams).then(
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

        $scope.updateCustomerUnit = function () {
            var entities = [];
            $scope.items.forEach(function (item) {
                if (item.selected) {
                    entities.push(item);
                }
            });
            if (entities.length === 0) {
                ngNotifier.error("Please, select atleast one record to perform action.");
            } else {
                ngNotifier.confirm("Are you sure do you want to Update the data?", null, function () {

                    var modalInstance = $uibModal.open({
                        animation: true,
                        size: "md",
                        templateUrl: "app/components/security/customerUnit/updateUnit.html",
                        controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                            $scope.units = requestData.modalData;
                            $scope.feeCategories = requestData.feeCategories;
                            $scope.select = function (action) {
                                ////$scope.deleteRemarks = deleteRemarks.value;
                                if (action == 'update' && ($scope.unitId == null ||$scope.unitId =='') ) {
                                    ngNotifier.error("Please select Unit");
                                    return;
                                }
                                if (action == 'update' && ($scope.refUnitId == null || $scope.refUnitId =='')) {
                                    ngNotifier.error("Please select Referred By Unit");
                                    return;
                                }
                                var outputData = {}
                                if (action == 'update') {
                                    outputData.unitId = $scope.unitId;
                                    outputData.refUnitId = $scope.refUnitId;
                                    outputData.sfcID = $scope.sfcID;
                                    outputData.action = 'update';
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
                                    modalData: $scope.$parent.authentication.userSite,
                                    feeCategories: $scope.lookups.spFeeCategory
                                };
                            }
                        }
                    });

                    modalInstance.result.then(
                        function (output) {
                            if (output.action == "update") {
                                entities.forEach(function (item) {
                                    item.sitId = output.unitId
                                    item.referredBySitId = output.refUnitId
                                    if (output.sfcID == "")
                                    {
                                        item.sfcID = null;
                                    }
                                    else { item.sfcID = output.sfcID }
                                    item.modifiedBy = $scope.$parent.userInfo.usrId;
                                });
                                entityService.updateCustomerUnit(entities).then(
                                        function (output) {
                                            $scope.entity = {};
                                            $scope.customerUnitListTable.reload();
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
            }
        };
        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("customerUnitController", controller);

});
