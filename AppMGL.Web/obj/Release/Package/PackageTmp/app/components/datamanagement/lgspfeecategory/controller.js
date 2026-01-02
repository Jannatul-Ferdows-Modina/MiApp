"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "lgspfeecategoryService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.lgspfeecategory;
        $scope.tabs = appUrl.lgspfeecategory.tabs;

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.sfcCreatedTs = new Date();
                $scope.entity.sfc_CreatedBy = $scope.$parent.userInfo.usrId
            }
            else {
                $scope.entity.sfc_UpdatedTs = new Date();
                $scope.entity.sfc_UpdatedBy = $scope.$parent.userInfo.usrId
            }
        };


         


        //#endregion




        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("lgspfeecategoryController", controller);

});
