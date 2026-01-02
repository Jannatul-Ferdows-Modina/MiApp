"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/user/";
        var serviceBase1 = ngAuthSettings.apiServiceBaseUri + "api/freightforwardernetwork/";
        this.checkEmail = function (params) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "checkEmail/",
                method: "POST",
                data: params
            });
        };
        this.getCities = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase1 + "getCities/" + id,
                method: "GET"
            });
        };
        this.resetPassword = function (params) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "resendEmail/",
                method: "POST",
                data: params
            });
        };

        this.getUserRoles = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getUserRoles/" + id,
                method: "GET"
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "security"));
    };

    service.$inject = injectParams;

    app.register.service("userService", service);

});
