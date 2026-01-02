"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "changePasswordService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {

        //#region Detail Method
        $scope.regexPassword = /^(?=.*[A-Z])(?=.*[0-9])(?=.*[@#!])(?=.{8,})/;
        //var mediumRegex = new RegExp("^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})");
        //var strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
        var changePassword = function () {
            if (!$scope.regexPassword.test($scope.entity.usrPwd)) {
                ngNotifier.error("Please enter valid Password");
                return;
            }

            var params = {
                AspNetUserId: requestData.aspNetUserId,
                CurrentPassword: $scope.entity.curPwd,
                NewPassword: $scope.entity.usrPwd
            };

            entityService.changePassword(params).then(
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
                case "changePassword":
                    changePassword();
                    break;
            }
        };

        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.controller("changePasswordController", controller);

});
