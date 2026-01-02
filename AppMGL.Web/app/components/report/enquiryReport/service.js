"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/enquiryReport/";

        this.getEnquiryList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getEnquiryList/",
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
        this.exportReportRemark = function (reportParams) {
            return $http({
                cache: false,
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "exportReportRemark/",
                method: "POST",
                data: reportParams,
                responseType: "arraybuffer"
            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "report"));
    };

    service.$inject = injectParams;

    app.register.service("enquiryReportService", service);

});
