"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/role/";

        this.listRoleMap = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/roleMap/listRoleMap/",
                method: "POST",
                data: listParams
            });
        };

        this.insertRoleMap = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/roleMap/insert/",
                method: "POST",
                data: entity
            });
        };

        this.updateRoleMap = function (entity, id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/roleMap/update/" + id,
                method: "PUT",
                data: entity
            });
        };

        this.deleteRoleMap = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/roleMap/delete/",
                method: "POST",
                data: entity
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "security"));
    };

    service.$inject = injectParams;

    app.register.service("roleService", service);

});
