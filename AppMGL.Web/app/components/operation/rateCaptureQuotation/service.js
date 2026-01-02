"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/displayRate/";
  
    

    this.getAllCarriers = function () {
        return $http({
            headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
            url: serviceBase + "getAllCarriers/",
            method: "GET"
        });
    };
    this.getCarrierAllCharges = function () {
        return $http({
            headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
            url: serviceBase + "getCarrierAllCharges/",
            method: "GET"
        });
    };

    this.getContainerCategories = function () {
        return $http({
            headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
            url: serviceBase + "getContainerCategories/",
            method: "GET"
        });
    };
    this.getContainerSizes = function () {
        return $http({
            headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
            url: serviceBase + "getContainerSizes/",
            method: "GET"
        });
    };







    this.getRateCaptureList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getRateCaptureList/",
                method: "POST",
                data: listParams
            });
        };

    this.getRateCaptureDetails = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getRateCaptureDetails/",
                method: "POST",
                data: entity

            });
        };
    this.saveRateCapture = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveRateCapture/",
                method: "POST",
                data: entity

            });
    };

    this.deleteContractRate = function (entity) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: serviceBase + "deleteContractRate/",
            method: "POST",
            data: entity

        });
    };

    this.getRouteNames = function (id) {
        return $http({
            headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
            url: serviceBase + "getRouteNames/" + id,
            method: "GET"
        });
    };

    this.getCarrierContracts = function (id) {
        return $http({
            headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
            url: serviceBase + "getCarrierContracts/" + id,
            method: "GET"
        });
    };


        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("rateCaptureService", service);

});
