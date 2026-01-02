"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "contractRateService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.contractRate;
        $scope.tabs = appUrl.contractRate.tabs;

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
                $scope.entity.createdBy = $scope.$parent.userInfo.usrId;
                $scope.entity.siteId = $scope.$parent.selectedSiteId
            }
            else {
                $scope.entity.modifiedOn = new Date();
                $scope.entity.modifiedBy = $scope.$parent.userInfo.usrId
            }
        };

        $scope.lookups = { siplContact: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("siplContact", 28, "companyName", "siplContact", null);           

            $scope.mQCTypes = [
                { mqctype: 0, optionName: "TEU" },
                { mqctype: 1, optionName: "FEU" }
            ];
            
        };

       
       
        $scope.callContractRateModal = function (source) {

             $scope.$parent.selectedSiteId
             $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/contractRateModal/detail.html",
                controller: "contractRateModalController",
                resolve: {
                    requestData: function () {

                        return {
                            contractID: (source || 0),
                            SitId: $scope.$parent.selectedSiteId
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.contractID = output.data.contractID;

                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };




        
        //#endregion




        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("contractRateController", controller);

});
