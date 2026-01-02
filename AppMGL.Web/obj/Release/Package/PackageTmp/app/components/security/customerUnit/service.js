"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/customerContact/";

        this.getContactsUnitList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/customerContact/getContactsUnitList/",
                method: "POST",
                data: listParams
            });
        };
        this.updateCustomerUnit = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/customerContact/updateCustomerUnit/",
                method: "POST",
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
        this.getFeeCategories = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getFeeCategories/",
                method: "GET"
            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "security"));
    };

    service.$inject = injectParams;

    app.register.service("customerUnitService", service);

});
