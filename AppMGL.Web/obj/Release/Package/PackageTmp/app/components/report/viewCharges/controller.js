"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "authService", "viewChargesService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService, requestData) {

        //#region Detail Method

        var getQuotationEnquiry = function () {

            var params = { QuotationId: requestData.quotationId };
            entityService.getQuotationEnquiry(params).then(
                function (output) {
                    $scope.entity = output.data.data[0];
                    getQuotationCharges();
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        var getQuotationCharges = function () {

            var params = {
                QuotationId: requestData.quotationId

            };
            entityService.GetQuotaionDetail(params).then(
                function (output) {
                    $scope.charges = output.data.data;
                    $scope.chargesTable.reload();
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        $scope.entityId = (requestData.quotationId) ? requestData.quotationId : 0;

        $scope.charges = [];

        $scope.chargesTable = new NgTableParams(
            {
                page: 1,
                count: 10,
                sorting: $.parseJSON("{ \"carrierName\": \"asc\" }"),
                group: {
                    carrierName: "asc"
                }
            }, {
                counts: [],
                getData: function (params) {
                    var listParams = {
                        QuotationId: requestData.quotationId
                    };
                    return entityService.getQuotaionChargesList(listParams).then(
                        function (output) {
                            params.total(output.data.count);
                            return output.data.data;
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        }
                    );
                }
            });

        $scope.sum = function (data, field) {
            return Utility.sumArray(data, field);
        }

        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

        (function () {
            getQuotationEnquiry();
        })();

    };

    controller.$inject = injectParams;

    app.register.controller("viewChargesController", controller);

});
