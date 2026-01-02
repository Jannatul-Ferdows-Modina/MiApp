"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "lgvwportService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.lgvwport;
        $scope.tabs = appUrl.lgvwport.tabs;

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
                $scope.entity.createdBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.modifiedOn = new Date();
                $scope.entity.modifiedBy = $scope.$parent.userInfo.usrId;
            }
        };

        $scope.lookups = { countrynames: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("siplcountry", 0, "name", "countrynames", null);
        };
        $scope.callTypeaheadp = function (viewValue, lookupModule, lookupField1, lookupMethod, fieldType, lookupField2, valueType) {

            var criteria = [];

            if ($scope.setLookupCriteria != undefined) {
                criteria = $scope.setLookupCriteria(lookupModule);
            }

            fieldType = (fieldType || "string");
            criteria.push(Utility.createFilter(lookupField1, fieldType, lookupField1, viewValue, "contains", null));
           


            var listParams = {
                SiteId: $scope.selectedSite.siteId,
                CwtId: $scope.userWorkTypeId,
                ModuleId: $scope.page.moduleId,
                PageIndex: 1,
                PageSize: 25,
                Sort: "{ \"" + lookupField1 + "\": \"asc\" }",
                Filter: JSON.stringify(criteria)
            };

            return entityService.lookup(lookupModule, lookupMethod, listParams).then(
                function (output) {

                    if ($scope.isInvalidData != undefined) {
                        if (output.data.data.length == 0) { $scope.isInvalidData = true; }
                        else { $scope.isInvalidData = false; }
                    }
                    return output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };
        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("lgvwportController", controller);

});
