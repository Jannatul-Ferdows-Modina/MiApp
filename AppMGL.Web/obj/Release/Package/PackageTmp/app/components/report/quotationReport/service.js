"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/quotationReport/";

        this.getQuotationList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getQuotationList/",
                method: "POST",
                data: listParams
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

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "report"));
    };

    service.$inject = injectParams;

    app.register.service("quotationReportService", service);

});
