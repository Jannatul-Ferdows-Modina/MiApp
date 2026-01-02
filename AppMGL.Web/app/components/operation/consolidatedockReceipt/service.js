   "use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

       // var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/documentCommon/";
        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";

        this.getDockReceipt = function (params) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "GetConsolidateDockReceipt/",
                method: "POST",
                data: params
            });
        };

        this.SaveConsolidateDockReceipt = function (document) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "SaveConsolidateDockReceipt/",
                method: "POST",
                data: document
            });
        };

        this.saveDockRecPdf = function (document) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveDockRecPdf/",
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

        this.getEmailIds = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getEmailIds/" + id,
                method: "GET"
            });
        };
        this.getConsolidateDockReceiptList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "GetConsolidateDockReceiptList/",
                method: "POST",
                data: listParams
            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "report"));
    };

    service.$inject = injectParams;

    app.register.service("consolidatedockReceiptService", service);

});
