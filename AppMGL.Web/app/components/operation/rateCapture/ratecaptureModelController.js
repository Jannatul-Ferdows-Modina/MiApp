"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "rateCaptureService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService, requestData) {
        debugger;

        $scope.closeModel = function (action) {
            var outputData = {}

            $uibModalInstance.close();
        };
        var updateContainerTotal = function () {

            $scope.tempChargesList = [];
            var containerServiceItem = {};
            $scope.containerServiceList.forEach(function (containerItem) {
                if ($scope.tempChargesList.length == 0) {
                    containerServiceItem = {};
                    containerServiceItem.categoryName = containerItem.categoryName;
                    containerServiceItem.containerName = containerItem.containerName;
                    containerServiceItem.total = containerItem.buyingAmt;
                    $scope.tempChargesList.push(containerServiceItem);
                }
                else {
                    $scope.tempChargesList.forEach(function (chargeItem) {
                        if (containerItem.categoryName == chargeItem.categoryName && containerItem.containerName == chargeItem.containerName) {
                            chargeItem.total = chargeItem.total + containerItem.buyingAmt;
                        }
                    });
                    if (isRecExits(containerItem.categoryName, containerItem.containerName) == false) {
                        containerServiceItem = {};
                        containerServiceItem.categoryName = containerItem.categoryName;
                        containerServiceItem.containerName = containerItem.containerName;
                        containerServiceItem.total = containerItem.buyingAmt;
                        $scope.tempChargesList.push(containerServiceItem);
                    }
                }
            });
        }

        var contractRateID = (requestData.contractRateID) ? requestData.contractRateID : 0;
        var typ = (requestData.typ) ? requestData.typ : 0;

            $scope.ShowDetail = function (typ, contractRateID) {

                entityService.getRateCapturebreakupDetails(typ, contractRateID).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut();
                        }
                        $scope.entity = output.data.data;
                        //Fill Container Charges
                        if ($scope.entity.containerChargesList != null) {
                            if ($scope.entity.containerChargesList.length > 0) {
                                $scope.containerServiceList = $scope.entity.containerChargesList;
                                updateContainerTotal();
                            }
                        }
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            };
            $scope.ShowDetail(typ, contractRateID);


        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("ratecaptureModelController", controller);

});