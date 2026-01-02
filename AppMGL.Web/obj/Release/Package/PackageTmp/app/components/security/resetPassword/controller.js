"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "resetPasswordService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {

        //#region Detail Method

        $scope.gRecaptchaResponse = "";

        var resetPassword = function () {

            var params = {
                Email: $scope.entity.email
            };

            if ($scope.gRecaptchaResponse == "") {
                ngNotifier.error("Please verify, I'm not a robot.");
                return;
            }
            else {
                entityService.resetPassword(params).then(
                    function (output) {
                        if (output.data.resultId == 1001) {
                            $scope.responseData.data = output.data;
                            $scope.responseData.resultId = 1001;
                            $uibModalInstance.close($scope.responseData);
                        }
                        else {
                            ngNotifier.showError($scope.authentication, output);
                        }
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });
            }
        };

        var resetPasswordA = function () {

            var params = {
                AspNetUserId: requestData.aspNetUserId,
                CurrentPassword: $scope.entity.curPwd,
                NewPassword: $scope.entity.usrPwd
            };

            entityService.resetPasswordA(params).then(
                function (output) {
                    if (output.data.resultId == 1001) {
                        $scope.responseData.data = output.data;
                        $scope.responseData.resultId = 1001;
                        $uibModalInstance.close($scope.responseData);
                    }
                    else {
                        ngNotifier.showError($scope.authentication, output);
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });
        };

        $scope.entityId = (requestData.userId) ? requestData.userId : 0;

        $scope.afterPerformAction = function (source) {

            var action = source.currentTarget.attributes["action"].value;

            switch (action) {
                case "resetPassword":
                    resetPassword();
                    break;
                case "resetPasswordA":
                    resetPasswordA();
                    break;
            }
        };

        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("resetPasswordController", controller);

});
