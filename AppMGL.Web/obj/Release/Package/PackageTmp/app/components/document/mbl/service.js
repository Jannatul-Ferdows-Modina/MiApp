   "use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/mbl/";

        this.getMBL = function (params) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getMBL/",
                method: "POST",
                data: params
            });
        };

        this.saveMBL = function (document) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveMBL/",
                method: "POST",
                data: document
            });
        };

        this.saveMBLPdf = function (document) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveMBLPdf/",
                method: "POST",
                data: document
            });
        };

        this.exportPdf = function (reportParams) {
            return $http({
                cache: false,
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "exportPdf/",
                method: "POST",
                data: reportParams,
                responseType: "arraybuffer"
            });
        };

        this.sendEmail = function (params) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "sendEmail/",
                method: "POST",
                data: params
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "report"));
    };

    service.$inject = injectParams;

    app.register.service("mblService", service);

});
