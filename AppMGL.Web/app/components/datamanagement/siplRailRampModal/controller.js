"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "siplRailRampModalService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {

        //#region Lookup 

        $scope.lookups = { continents: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("continent", 0, "ContinentId", "continents", null);
        };

        //#endregion

        //#region Detail Method

        $scope.entityId = (requestData.orgnRailRampId) ? requestData.orgnRailRampId : 0;

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
            }
            else {
                $scope.entity.modifiedOn = new Date();
            }
        };

        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("siplRailRampModalController", controller);

});
