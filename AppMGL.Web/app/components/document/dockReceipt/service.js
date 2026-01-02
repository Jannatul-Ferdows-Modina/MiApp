   "use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/documentCommon/";
        var serviceBase1 = ngAuthSettings.apiServiceBaseUri + "api/booking/";
        this.getDockReceipt = function (params) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getDockReceipt/",
                method: "POST",
                data: params
            });
        };

        this.saveDockReceipt = function (document) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveDockReceipt/",
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
        this.getDocumentList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getDocumentList/",
                method: "POST",
                data: listParams
            });
        };
        this.getDockReceiptSearch = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "DockReceiptSearch/",
                method: "POST",
                data: listfilter

            });
        };
        this.getState = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase1 + "State/",
                method: "POST",
                data: listfilter

            });
        };
        this.getCountry = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase1 + "Country/",
                method: "POST",
                data: listfilter

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
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "report"));
    };

    service.$inject = injectParams;

    app.register.service("dockReceiptService", service);

});
