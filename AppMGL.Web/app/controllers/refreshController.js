"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$location", "authService"];

    var controller = function ($scope, $timeout, $location, authService) {

        $scope.authentication = authService.authentication;
        $scope.tokenRefreshed = false;
        $scope.tokenResponse = null;

        $scope.refreshToken = function () {

            authService.refreshToken().then(function (response) {
                $scope.tokenRefreshed = true;
                $scope.tokenResponse = response;
            },
            function (err) {
                $location.path("/login");
            });
        };
    };

    controller.$inject = injectParams;

    app.controller("refreshController", controller);

});
