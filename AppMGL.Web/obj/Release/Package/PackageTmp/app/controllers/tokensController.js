"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "tokensService"];

    var controller = function ($scope, $timeout, tokensService) {

        $scope.refreshTokens = [];

        tokensService.getRefreshTokens().then(function (results) {

            $scope.refreshTokens = results.data;

        }, function (error) {
            alert(error.data.message);
        });

        $scope.deleteRefreshTokens = function (index, tokenid) {

            tokenid = window.encodeURIComponent(tokenid);

            tokensService.deleteRefreshTokens(tokenid).then(function (results) {

                $scope.refreshTokens.splice(index, 1);

            }, function (error) {
                alert(error.data.message);
            });
        }
    };

    controller.$inject = injectParams;

    app.controller("tokensController", controller);

});
