"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/dashboard/";

    this.getDashBoardData = function (id) {
        return $http({
            headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
            url: serviceBase + "getDashBoardData/" + id,
            method: "GET"
        });
    };
    this.getTotalActivity = function (id) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: serviceBase + "getTotalActivity/" + id,
            method: "GET"
        });
    };
    this.getSiteLocations = function () {
        return $http({
            headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
            url: serviceBase + "getSiteLocations/",
            method: "GET"
        });
    };
    
    angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "home"));

    };

    service.$inject = injectParams;

    app.register.service("dashboardService", service);

});
