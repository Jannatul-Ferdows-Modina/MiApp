"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/displayRate/";
  
        
    this.getCaptureRouteList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getCaptureRouteList/",
                method: "POST",
                data: listParams
            });
        };

    this.getCaptureRouteDetails = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getCaptureRouteDetails/",
                method: "POST",
                data: entity

            });
        };
    this.saveCaptureRoute = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveCaptureRoute/",
                method: "POST",
                data: entity

            });
    };

    this.deleteRoute = function (entity) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: serviceBase + "deleteRoute/",
            method: "POST",
            data: entity

        });
    };

    this.getRouteNames = function (id) {
        return $http({
            headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
            url: serviceBase + "getRouteNames/" + id,
            method: "GET"
        });
    };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("captureRouteService", service);

});
