"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "timezoneService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.timezone;
        $scope.tabs = appUrl.timezone.tabs;

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.tmzCreatedTs = new Date();
                $scope.entity.tmzCreatedBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.tmzUpdatedTs = new Date();
                $scope.entity.tmzUpdatedBy = $scope.$parent.userInfo.usrId;
            }
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("timezoneController", controller);

});
