"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";
       
        this.getPendingStuffingList = function (listParams) {

            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "GetPendingStuffingList/",
                method: "POST",
                data: listParams
            });
        };
        this.getBookedContainerList = function (listParams) {
           
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getContainerBookingList/",
                method: "POST",
                data: listParams
            });
        };
        this.saveBookedContainer = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveContainerBooked/",
                method: "POST",
                data: entity

            });
        };
        this.getBookedContainerDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getContainerBookedDetail/",
                method: "POST",
                data: entity

            });
        };
       
       

        this.deleteBookedContainer = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "deleteContainerBooked/",
                method: "POST",
                data: entity

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
       
        this.searchContainer = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "SearchContainer/",
                method: "POST",
                data: listfilter

            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("containerbookingService", service);

});
