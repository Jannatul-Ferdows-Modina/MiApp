"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/user/";

        this.resetPassword = function (params) {

            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "resetPassword/",
                method: "POST",
                data: params
            });
        };

        this.resetPasswordA = function (params) {

            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "resetPasswordA/",
                method: "POST",
                data: params
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "security"));
    };

    service.$inject = injectParams;

    app.register.service("resetPasswordService", service);

});
