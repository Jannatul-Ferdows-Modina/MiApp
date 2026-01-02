"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "siplPortModalService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {

        //#region Lookup 

        $scope.lookups = { countrynames: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("siplcountry", 0, "name", "countrynames", null);
        };


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

    app.register.controller("siplPortModalController", controller);

});
