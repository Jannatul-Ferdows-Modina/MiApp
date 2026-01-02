"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "departmentService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.department;
        $scope.tabs = appUrl.department.tabs;

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.dptCreatedTs = new Date(); 
                $scope.entity.dptCreatedBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.dptUpdatedTs = new Date(); 
                $scope.entity.dptUpdatedBy = $scope.$parent.userInfo.usrId;
            }
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("departmentController", controller);

});
