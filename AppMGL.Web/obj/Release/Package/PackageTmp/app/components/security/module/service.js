"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/module/";

        this.listAction = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/moduleAction/list/",
                method: "POST",
                data: listParams
            });
        };

        this.insertAction = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/moduleAction/insert/",
                method: "POST",
                data: entity
            });
        };

        this.updateAction = function (entity, id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/moduleAction/update/" + id,
                method: "PUT",
                data: entity
            });
        };

        this.deleteAction = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/moduleAction/delete/",
                method: "POST",
                data: entity
            });
        };

        this.lookupPolicyModule = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "lookupPolicyModule/",
                method: "POST",
                data: listParams
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "security"));
    };

    service.$inject = injectParams;

    app.register.service("moduleService", service);

});
