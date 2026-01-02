"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings"];

    var serviceFactory = function ($http, $q, ngAuthSettings) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri,
            factory = {};

        var getRefreshTokens = function () {

            return $http.get(serviceBase + "api/refreshtokens").then(function (results) {
                return results;
            });
        };

        var deleteRefreshTokens = function (tokenid) {

            return $http.delete(serviceBase + "api/refreshtokens/?tokenid=" + tokenid).then(function (results) {
                return results;
            });
        };

        factory.deleteRefreshTokens = deleteRefreshTokens;
        factory.getRefreshTokens = getRefreshTokens;

        return factory;
    };

    serviceFactory.$inject = injectParams;

    app.factory("tokensService", serviceFactory);

});

