"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "authService", "sendEmailService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService, requestData) {

        //#region Detail Method

        
        $scope.detail = function (source, target) {
            debugger;
            var action = source.currentTarget.attributes["action"].value;
            var fileNo = source.currentTarget.attributes["entityid"].value;
            var docCommId = source.currentTarget.attributes["doccommid"].value;

            switch (action) {
                case "selectDocument":
                    $scope.responseData.data = { exportRef: fileNo };
                    $scope.responseData.docCommId = docCommId;
                    $scope.responseData.resultId = 1001;
                    $uibModalInstance.close($scope.responseData);
                    break;
            }
        };

        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("sendEmailController", controller);

});
