"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "moduleService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.module;
        $scope.tabs = appUrl.module.tabs;

        //#endregion

        //#region Lookup 

        $scope.lookups = { moduleTypes: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("moduleType", 0, "MteId", "moduleTypes", null);
        };

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.modCreatedTs = new Date();
                $scope.entity.modCreatedBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.modUpdatedTs = new Date();
                $scope.entity.modUpdatedBy = $scope.$parent.userInfo.usrId;
            }
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("moduleController", controller);

});
