"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "localStorageService", "ngAuthSettings"];

    var serviceFactory = function ($http, $q, localStorageService, ngAuthSettings) {

        //#region Properties

        var serviceBase = ngAuthSettings.apiServiceBaseUri;
        var factory = {};
        var authentication = {
            isAuth: false,
            userId: 0,
            userName: "",
            useRefreshTokens: false,
            selectedSiteId: 0
        };

        //#endregion

        //#region Private Methods

        var logOut = function () {

            localStorageService.remove("authData");
            localStorageService.remove("userRoles");
            sessionStorage.removeItem("token");
            authentication.isAuth = false;
            authentication.userId = 0;
            authentication.userName = "";
            authentication.useRefreshTokens = false;
        };

        var initAuthData = function (response, useRefreshTokens) {

            var authData = {
                token: response.access_token,
                userId: response.userId,
                userName: response.userName,
                refreshToken: (useRefreshTokens ? response.refresh_token : ""),
                useRefreshTokens: useRefreshTokens
            };
            sessionStorage.setItem('token', response.access_token);
            return authData;
        };

        //#endregion

        //#region Public Methods

        factory.authentication = authentication;
        factory.location = ngAuthSettings.apiServiceBaseUri;
        factory.logOut = logOut;

        factory.saveRegistration = function (registration) {

            logOut();

            return $http.post(serviceBase + "api/userAccount/register", registration).then(function (response) {
                return response;
            });
        };

        factory.login = function (loginData) {

            var data = "grant_type=password&userid=&username=" + loginData.userName + "&password=" + loginData.password;

            if (loginData.useRefreshTokens) {
                data = data + "&client_id=" + ngAuthSettings.clientId;
            }

            return $http({
                headers: { 'Content-Type': "application/x-www-form-urlencoded" },
                url: serviceBase + "token",
                method: "POST",
                data: data
            });
        };

        factory.fillAuthData = function () {

            var authData = localStorageService.get("authData");
            var sessionData = sessionStorage.getItem('token');
            if (!sessionData && authData) {
                logOut();
                authData = null;
            }
            if (authData) {
                authentication.isAuth = true;
                authentication.userId = authData.userId;
                authentication.userName = authData.userName;
                authentication.useRefreshTokens = authData.useRefreshTokens;
            }
        };

        factory.refreshToken = function () {

            var deferred = $q.defer();
            var authData = localStorageService.get("authData");

            if (authData) {
                if (authData.useRefreshTokens) {
                    var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;
                    localStorageService.remove("authData");
                    $http({
                        headers: { 'Content-Type': "application/x-www-form-urlencoded" },
                        url: serviceBase + "token",
                        method: "POST",
                        data: data
                    })
                        .then(function (response) {
                            localStorageService.set("authData", initAuthData(response, true));
                            deferred.resolve(response);
                        })
                        .catch(function (response) {
                            logOut();
                            deferred.reject(response);
                        });
                }
            }

            return deferred.promise;
        };

        factory.obtainAccessToken = function (externalData) {

            var deferred = $q.defer();

            $http({
                url: serviceBase + "api/userAccount/ObtainLocalAccessToken",
                method: "GET",
                params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken }
            })
                .then(function (response) {
                    localStorageService.set("authData", initAuthData(response, false));

                    authentication.isAuth = true;
                    authentication.userId = response.userId;
                    authentication.userName = response.userName;
                    authentication.useRefreshTokens = false;

                    deferred.resolve(response);
                })
                .catch(function (response) {
                    logOut();
                    deferred.reject(response);
                });

            return deferred.promise;
        };

        factory.externalAuthData = {
            provider: "",
            userId: 0,
            userName: "",
            externalAccessToken: ""
        };;

        factory.registerExternal = function (registerExternalData) {

            var deferred = $q.defer();

            $http({
                url: serviceBase + "api/userAccount/registerexternal",
                method: "POST",
                data: registerExternalData
            })
                .then(function (response) {
                    localStorageService.set("authData", initAuthData(response, false));

                    authentication.isAuth = true;
                    authentication.userId = response.userId;
                    authentication.userName = response.userName;
                    authentication.useRefreshTokens = false;

                    deferred.resolve(response);
                })
                .catch(function (response) {
                    logOut();
                    deferred.reject(response);
                });

            return deferred.promise;
        };

        //#endregion

        //#region Dashboard Methods

        factory.validateAccessToken = function () {

            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "api/user/validateAccessToken/",
                method: "POST"
            });
        };

        factory.getUserInfo = function (params) {

            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "api/user/info/",
                method: "POST",
                data: params
            });
        };

        factory.getUserRoles = function (params) {

            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "api/user/roles/",
                method: "POST",
                data: params
            });
        };

        factory.downloadUserImage = function (params) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'fileName': 'male_user_icon',
                    'userId': params.UsrId
                },
                url: serviceBase + "api/user/getUserImage/",
                responseType: "arraybuffer",
                method: "POST"
            });
        };
        factory.webConfig;
        factory.getConfigDetails = function (externalData) {
            //var deferred = $q.defer();
            if (factory.webConfig) {
                return $q.when(factory.webConfig);
            }
            return $http({
                url: "appConfig.json",
                method: "GET"
            }).then(function (config) {
                factory.webConfig = config.data;
                return config.data
            });
            //return deferred.promise;
        };
        //#endregion

        return factory;
    };

    serviceFactory.$inject = injectParams;

    app.factory("authService", serviceFactory);
});
