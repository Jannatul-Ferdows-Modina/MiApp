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
        
        this.getDashBoardTargetData = function (TargetFilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "ListTargetData/",
                method: "POST",
                data: TargetFilter

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

    this.exportReport = function (reportParams) {
        return $http({
            cache: false,
            headers: { 'Content-Type': "application/json" },
            url: serviceBase + "exportReport/",
            method: "POST",
            data: reportParams,
            responseType: "arraybuffer"
        });
    };
    
    angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "home"));

    };

    service.$inject = injectParams;

    app.register.service("corporatedashboardService", service);

});
