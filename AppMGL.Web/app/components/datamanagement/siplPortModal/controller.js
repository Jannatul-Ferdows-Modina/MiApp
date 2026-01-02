"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "siplPortModalService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {
       
        $scope.originID = (requestData.originID) ? requestData.originID : '';
       
        $scope.lookups = { countrynames: [] };
        $scope.initDropdown = function () {

            $scope.fetchLookupData("siplcountry", 0, "name", "countrynames", null);
        };

        $scope.getDetail = function (id) {
            $scope.entityId = id;
            if ($scope.entityId > 0) {
                entityService.detail($scope.entityId).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        $scope.entity = output.data.data;
                        if ($scope.afterGetDetail != undefined) {
                            $scope.afterGetDetail();
                        }
                        
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            } else {
                $scope.goBack();
            }
        };

        if ($scope.originID != undefined && $scope.originID != '' && $scope.originID != null) {
           
            $scope.getDetail($scope.originID);
        }
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
