"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/quotationReport/";

        this.getQuotationEnquiry = function (params) {

            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getQuotationEnquiry/",
                method: "POST",
                data: params
            });
        };

        this.getQuotationCharges = function (params) {

            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getQuotationCharges/",
                method: "POST",
                data: params
            });
        };

        this.getQuotaionChargesList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getQuotaionChargesList/",
                method: "POST",
                data: listParams
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "report"));
    };

    service.$inject = injectParams;

    app.register.service("viewChargesService", service);

});
