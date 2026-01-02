"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "exporttargetService"];
    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {
        $scope.page = appUrl.exporttarget;
        $scope.tabs = appUrl.exporttarget.tabs;
        $scope.sitelist = [];
        $scope.initDropdown = function () {
            $scope.fetchLookupData("CustomerContact", 0, "SitName", "sitelist", "SiteList");
        };
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

        $scope.lookups = { statenames: [] };
        $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey == "sitelist") {
                if ($scope.lookups.sitelist != undefined) {
                    if ($scope.lookups.sitelist.length > 0) {
                        $scope.lookups.sitelist.forEach(function (o) {

                            $scope.sitelist.push(o);

                        });
                    };
                };
            };
        };
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;
    app.register.controller("exporttargetController", controller);

});
