"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";
       

        this.getStuffingList = function (listParams) {
           
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "GetStuffingList/",
                method: "POST",
                data: listParams
            });
        };
        this.getStuffingDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getStuffingDetail/",
                method: "POST",
                data: entity

            });
        };
       
        this.saveStuffing = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "SaveStuffing/",
                method: "POST",
                data: entity

            });
        };
        this.getBookedContainerList = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getBookedContainerList/" + id,
                method: "GET"
            });
        };
        this.deleteStuffing = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "deleteStuffing/",
                method: "POST",
                data: entity

            });
        };

        this.pendingContainerList = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "PendingContainerList/" + id,
                method: "GET"
            });
        };
        this.pendingQuotationList = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "PendingQuotationList/" + id,
                method: "GET"
            });
        };
        this.getContainerDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getContainerDetail/",
                method: "POST",
                data: entity

            });
        };
        this.MoveConsolidateBooking = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "MoveConsolidateBooking/" + id,
                method: "POST"

            });
        };

        this.lookup = function (name, method, listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: path + name + "/" + (method == null ? "lookup" : method) + "/",
                method: "POST",
                data: listParams
            });
        };
       

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("containerstuffingService", service);

});
