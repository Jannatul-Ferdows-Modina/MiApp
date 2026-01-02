"use strict";

define(["app"], function (app) {

    var injectParams = ["$q", "$injector", "$location", "localStorageService"];

    var serviceFactory = function ($q, $injector, $location, localStorageService) {

        var factory = {};

        var request = function (config) {

            config.headers = config.headers || {};

            if (config.url.slice(-4) == "html") {
                config.headers["Cache-Control"] = "no-cache";
                config.headers["Pragma"] = "no-cache";
            }

            var authData = localStorageService.get("authData");
            if (authData) {
                config.headers.Authorization = "Bearer " + authData.token;
            }

            return config;
        };

        var responseError = function (rejection) {
            if (rejection.status === 401) {
                var authService = $injector.get("authService");
                var authData = localStorageService.get("authData");
                if (authData && authData.useRefreshTokens) {
                    $location.path("/refresh");
                    return $q.reject(rejection);
                }
                authService.logOut();
                $location.path("/login");
            }
            return $q.reject(rejection);
        };

        factory.request = request;
        factory.responseError = responseError;

        return factory;
    };

    serviceFactory.$inject = injectParams;

    app.factory("authInterceptorService", serviceFactory);

});

