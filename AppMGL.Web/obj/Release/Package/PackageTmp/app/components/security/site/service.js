"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/site/";

        this.listRole = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/siteRole/listRole/",
                method: "POST",
                data: listParams
            });
        };

        this.insertRole = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/siteRole/insert/",
                method: "POST",
                data: entity
            });
        };

        this.updateRole = function (entity, id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/siteRole/update/" + id,
                method: "PUT",
                data: entity
            });
        };

        this.deleteRole = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/siteRole/delete/",
                method: "POST",
                data: entity
            });
        };
        this.getLoginUserUnits = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getLoginUserUnits/" + id,
                method: "GET"
            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "security"));
    };

    service.$inject = injectParams;

    app.register.service("siteService", service);

});
