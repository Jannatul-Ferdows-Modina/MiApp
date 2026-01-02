"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";

        this.getDispatchContainerList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getDispatchContainerList/",
                method: "POST",
                data: listParams
            });
        };
        this.getDispatchContainerDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getDispatchContainerDetail/",
                method: "POST",
                data: entity

            });
        };
        this.saveDispatchContainerData = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveDispatchContainerData/",
                method: "POST",
                data: entity

            });
        };
        this.approveQuotation = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "approveQuotation/",
                method: "POST",
                data: entity

            });
        };
        this.getEnquiryDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: ngAuthSettings.apiServiceBaseUri + "api/enquiry/" + "getEnquiryDetail/",
                method: "POST",
                data: entity

            });
        };

        
        this.getcarrierAllRates = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getCarrierAllRates/" + id,
                method: "GET"
            });
        };

        this.deleteQuotation = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "deleteQuotation/",
                method: "POST",
                data: entity

            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("bookingCaptureContainerService", service);

});
