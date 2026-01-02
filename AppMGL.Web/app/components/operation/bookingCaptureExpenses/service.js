"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";
       
        this.getDocumentList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getDocumentList/",
                method: "POST",
                data: listParams
            });
        };

        this.showDocumnetExpenseDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "showDocumnetExpenseDetail/",
                method: "POST",
                data: entity

            });
        };

        this.saveBookingExpenses = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveBookingExpenses/",
                method: "POST",
                data: entity

            });
        };
        
        this.deleteBooking = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "deleteBooking/",
                method: "POST",
                data: entity

            });
        };

        this.cancelBooking = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "cancelBooking/",
                method: "POST",
                data: entity

            });
        };
       
        this.getExpenseHeads = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getExpenseHeads/",
                method: "GET"
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

    app.register.service("bookingCaptureExpensesService", service);

});
