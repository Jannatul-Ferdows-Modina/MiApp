"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "lgacctcategoryService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.lgacctcategory;
        $scope.tabs = appUrl.lgacctcategory.tabs;

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.acyCreatedTs = new Date();
                $scope.entity.acy_CreatedBy = $scope.$parent.userInfo.usrId
            }
            else {
                $scope.entity.acy_UpdatedTs = new Date();
                $scope.entity.acy_UpdatedBy = $scope.$parent.userInfo.usrId
            }
        };


         


        //#endregion




        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("lgacctcategoryController", controller);

});
