"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "commodityModalService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {

        //#region Lookup 

        $scope.lookups = { commoditytypes: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("commoditytype", 0, "CommodityType", "commoditytypes", null);
        };

        //#endregion

        //#region Detail Method

        $scope.entityId = (requestData.commodityId) ? requestData.commodityId : 0;

        $scope.beforeSave = function (action, lastAction) {
            debugger;

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
                // $scope.entity.createdBy = $scope.$parent.userInfo.usrId;
                $scope.entity.createdBy =  $scope.$$prevSibling.authentication.userId;
            }
            else {
                $scope.entity.modifiedOn = new Date();
                //  $scope.entity.modifiedBy = $scope.$parent.userInfo.usrId;
                $scope.entity.modifiedBy = $scope.$$prevSibling.authentication.userId;
            }
        };

        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("commodityModalController", controller);

});
