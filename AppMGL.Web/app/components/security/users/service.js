"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/user/";

        this.uploadIcon = function (entity, $file) {
            return Upload.upload({
                url: serviceBase + "uploadIcon/",
                method: "POST",
                data: entity,
                file: $file
            });
        };

        this.deleteIcon = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "deleteIcon/",
                method: "POST",
                data: entity
            });
        };

        this.listSite = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/usersSite/listSite/",
                method: "POST",
                data: listParams
            });
        };

        this.deleteSite = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/usersSite/delete/",
                method: "POST",
                data: entity
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "security"));
    };

    service.$inject = injectParams;

    app.register.service("usersService", service);

});
