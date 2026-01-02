"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "authService", "viewDocumentListService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService, requestData) {

        //#region Detail Method

        $scope.documentTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }")
        }, {
            counts: [],
            getData: function (params) {
                var listParams = {
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),
                    SitId: requestData.SitId,
                    DocumentStatus: requestData.documentStatus
                };
                return entityService.getDocumentList(listParams).then(
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

        $scope.performSubAction = function (source, target) {
            debugger;
            var action = source.currentTarget.attributes["action"].value;
            var fileNo = source.currentTarget.attributes["entityid"].value;
            var docCommId = source.currentTarget.attributes["doccommid"].value;

            switch (action) {
                case "selectDocument":
                    $scope.responseData.data = { exportRef: fileNo };
                    $scope.responseData.docCommId =  docCommId;
                    $scope.responseData.resultId = 1001;
                    $uibModalInstance.close($scope.responseData);
                    break;
            }
        };

        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("viewDocumentListController", controller);

});
