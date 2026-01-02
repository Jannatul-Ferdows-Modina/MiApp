"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "lgvwcityService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.lgvwcity;
        $scope.tabs = appUrl.lgvwcity.tabs;

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

        $scope.lookups = { statenames: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("lgvwstate", 0, "name", "statenames", null);
        };



        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("lgvwcityController", controller);

});
