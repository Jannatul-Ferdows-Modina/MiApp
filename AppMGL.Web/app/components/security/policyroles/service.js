"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/policyRoles/";

        this.listAction = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "listAction/",
                method: "POST",
                data: listParams
            });
        };

        this.batchInsert = function (batchData) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "batchInsert/",
                method: "POST",
                data: batchData
            });
        };

        this.batchUpdate = function (batchData) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "batchUpdate/",
                method: "POST",
                data: batchData
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "security"));
    };

    service.$inject = injectParams;

    app.register.service("policyRolesService", service);

});
