"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/contractRateVW/";

        this.getContractList = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getContractList/",
                method: "POST",
                data: entity

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

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("contractRateService", service);

});
