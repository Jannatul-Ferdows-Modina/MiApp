"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "lgvwhtscodeService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        $scope.page = appUrl.lgvwhtscode;
        $scope.tabs = appUrl.lgvwhtscode.tabs;
        $scope.initDropdown = function () {

            $scope.fetchLookupData("Booking", 0, "name", "uomList", "uomList");

        };
      
        $scope.beforeSave = function (action, lastAction) {
            var finalhtsNumber = "";
            if ($scope.entity.htsfirst != "" && $scope.entity.htsfirst != null)
                finalhtsNumber = $scope.entity.htsfirst;
            if ($scope.entity.htssecond != "" && $scope.entity.htssecond != null)
                finalhtsNumber += '.'+$scope.entity.htssecond;
            if ($scope.entity.htsthird != "" && $scope.entity.htsthird != null)
                finalhtsNumber += '.' +$scope.entity.htsthird;
            

            $scope.entity.htsNumber = finalhtsNumber;

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
                $scope.entity.createdBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.modifiedOn = new Date();
                $scope.entity.modifiedBy = $scope.$parent.userInfo.usrId;
            }
        };

        $scope.lookups = { statenames: [] };
        $scope.searchValues = function (viewValue, selectType, searchRouteType) {
            var resultItem = {};
            var lookupModule;
            var routeType = "";
            var lookupField = "name";
            if (selectType == "htsCode" || selectType == "htsCode") {
                lookupModule = "Booking";
                lookupField = "htsCode";

                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getHTSCode(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}

                            resultItem.name = o.name.split("-")[0];
                            resultItem.id = o.id;
                            resultItem.unit = o.unit;
                            resultItem.unit1 = o.unit1;
                            resultItem.description = o.description;
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }
            
        };
        $scope.getDetail = function (id) {
            $scope.entityId = id;
            if ($scope.entityId > 0) {
                entityService.detail($scope.entityId).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        $scope.entity = output.data.data;
                        if ($scope.entity.secondUOM == "")
                            $scope.entity.secondUOM = null;
                        if ($scope.entity.firstUOM == "")
                            $scope.entity.firstUOM = null;

                        if ($scope.afterGetDetail != undefined) {
                            $scope.afterGetDetail();
                        }

                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            } else {
                $scope.goBack();
            }
        };
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("lgvwhtscodeController", controller);

});
