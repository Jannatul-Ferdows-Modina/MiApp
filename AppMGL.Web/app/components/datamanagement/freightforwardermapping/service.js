"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/freightforwardermapping/";
        var serviceBase1 = ngAuthSettings.apiServiceBaseUri + "api/booking/";

       
        this.delete = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "delete/",
                method: "POST",
                data: entity
            });
        };
        
        this.saveForwarderMapping = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveForwarderMapping/",
                method: "POST",
                data: entity

            });
        };
        this.getForwarderMapping = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getForwarderMapping/" + id,
                method: "GET"

            });
        };

        this.getLatestMappingCode = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getLatestMappingCode/",
                method: "GET"
            });
        };


        this.lookup = function (name, method, listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: path + name + "/" + (method == null ? "lookup" : method) + "/",
                method: "POST",
                data: listParams
            });
        };
        this.getCompanySearch = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase1 + "CompanySearch/",
                method: "POST",
                data: listfilter

            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "freightforwardernetwork"));
    };

    service.$inject = injectParams;

    app.register.service("freightforwardermappingService", service);

});
