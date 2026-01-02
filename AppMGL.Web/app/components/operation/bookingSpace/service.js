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

        this.getDocumentDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getDocumentDetail/",
                method: "POST",
                data: entity

            });
        };

        this.saveDocumentDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveDocumentDetail/",
                method: "POST",
                data: entity

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
        this.rollOverBooking = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "rollOverBooking/",
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
        
        this.getEnquiryDepartments = function () {
            return $http({
                headers: { 'Content-Type': "application/json"},
                url: serviceBase + "getEnquiryDepartments/",
                method: "GET"
            });
        };
        
        this.getLatestBookingNo = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getLatestBookingNo/",
                method: "POST",
                data: entity
            });
        };
        this.validateBookingFileNo = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "validateBookingFileNo/" + id,
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

        this.getContainerCategories = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getContainerCategories/",
                method: "GET"
            });
        };
        this.getDocumentationStatus = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getDocumentationStatus/",
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

        this.getAllCarriers = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getAllCarriers/",
                method: "GET"
            });
        };

        this.CopyEnquiry = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "copyEnquiry/",
                method: "POST",
                data: entity

            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("bookingSpaceService", service);

});
