"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$location", "authService", "ngNotifier"];

    var controller = function ($scope, $timeout, $location, authService, ngNotifier) {

        //#region Properties

        $scope.savedSuccessfully = false;
        $scope.message = "";

        $scope.registration = {
            userName: "",
            password: "",
            confirmPassword: ""
        };

        //#endregion

        //#region Private Methods

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path("/login");
            }, 2000);
        }

        //#endregion

        //#region Signup Methods

        $scope.signUp = function () {

            authService.saveRegistration($scope.registration).then(
                function (response) {
                    ngNotifier.success("User has been registered successfully, you will be redicted to login page in 2 seconds.");
                    startTimer();
                },
                function (response) {
                    var errors = [];
                    for (var key in response.data.modelState) {
                        if (response.data.modelState.hasOwnProperty(key)) {
                            for (var i = 0; i < response.data.modelState[key].length; i++) {
                                errors.push(response.data.modelState[key][i]);
                            }
                        }
                    }
                    ngNotifier.error("Failed to register user due to:" + errors.join(" "));
                }
            );
        };

        //#endregion
    };

    controller.$inject = injectParams;

    app.controller("signupController", controller);

});
