"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/documentation/";
        var serviceBase1 = ngAuthSettings.apiServiceBaseUri + "api/booking/";
        this.getDocumentationList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase1 + "GetShiperList/",
                method: "POST",
                data: listParams
            });
        };

        this.getShiperDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase1 + "getShiperDetail/",
                method: "POST",
                data: entity

            });
        };
        this.saveShiperData = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase1 + "saveShiperData/",
                method: "POST",
                data: entity

            });
        };

        this.getOriginState = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase1 + "State/",
                method: "POST",
                data: listfilter

            });
        };
        this.getCountryofDestination = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase1 + "Country/",
                method: "POST",
                data: listfilter

            });
        };
        this.getHTSCode = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase1 + "htsCodeSearch/",
                method: "POST",
                data: listfilter

            });
        };
        this.getPortofExport = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase1 + "PortofExportSearch/",
                method: "POST",
                data: listfilter

            });
        };
        
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("documentation1Service", service);

});
