   "use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/documentCommon/";
        var serviceBase1 = ngAuthSettings.apiServiceBaseUri + "api/booking/";
        var serviceBase2 = ngAuthSettings.apiServiceBaseUri + "api/customerContact/";
        this.getDockReceipt = function (params) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "GetAddDockReceipt/",
                method: "POST",
                data: params
            });
        };

        this.saveDockReceipt = function (document) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "AddDockReceipt/",
                method: "POST",
                data: document
            });
        };
        this.getDocumentList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "GetAddDockReceiptList/",
                method: "POST",
                data: listParams
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
        this.getCities = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase2 + "getCities/" + id,
                method: "GET"
            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "report"));
    };

    service.$inject = injectParams;

    app.register.service("dockReceiptDirectService", service);

});
