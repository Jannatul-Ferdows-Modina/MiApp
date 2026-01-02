"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "siplCityModalService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {

        //#region Lookup 

        $scope.lookups = { statenames: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("lgvwstate", 0, "name", "statenames", null);
        };
        //#endregion

        //#region Detail Method

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

        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("siplCityModalController", controller);

});
