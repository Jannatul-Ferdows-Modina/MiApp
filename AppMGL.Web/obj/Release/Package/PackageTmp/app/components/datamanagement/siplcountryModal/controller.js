"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "siplcountryModalService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {

        //#region Lookup 

        $scope.lookups = { continentnames: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("continent", 0, "name", "continentnames", null);
        };
        $scope.entity = {};
        //#endregion

        //#region Detail Method

        $scope.entityId = (requestData.countryId) ? requestData.countryId : 0;
        $scope.entity.continentID = (requestData.continentID) ? requestData.continentID : 0;

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
            }
            else {
                $scope.entity.modifiedOn = new Date();
            }
        };
        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":
                    $scope.entity.continentID = 2;
                    break;
            }

        }
        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("siplcountryModalController", controller);

});
