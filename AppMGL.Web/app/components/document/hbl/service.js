   "use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/hbl/";
        this.getDocumentList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getDocumentList/",
                method: "POST",
                data: listParams
            });
        };
        this.getHBL = function (params) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getHBL/",
                method: "POST",
                data: params
            });
        };

        this.saveHBL = function (document) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveHBL/",
                method: "POST",
                data: document
            });
        };

        this.saveHBLPdf = function (document) {
            debugger;
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveHBLPdf/",
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
        this.getHBLRefrenceSearch = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "HBLRefrenceSearch/",
                method: "POST",
                data: listfilter

            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "report"));
    };

    service.$inject = injectParams;

    app.register.service("hblService", service);

});
