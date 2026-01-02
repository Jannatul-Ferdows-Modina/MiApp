"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "countryService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.country;
        $scope.tabs = appUrl.country.tabs;

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {

                $scope.entity.cryCreatedTs = new Date();
                $scope.entity.cryCreatedBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.cryUpdatedTs = new Date();
                $scope.entity.cryUpdatedBy = $scope.$parent.userInfo.usrId;
            }
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("countryController", controller);

});
