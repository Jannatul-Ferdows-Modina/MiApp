"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";
        var sitebaseurl = ngAuthSettings.apiServiceBaseUri + "api/site/";
        this.getBookingDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getBookingDetail/",
                method: "POST",
                data: entity

            });
        };

        this.GetConsolidateBookingList = function (listParams) {

            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "GetConsolidateBookingList/",
                method: "POST",
                data: listParams
            });
        };
        //this.saveDocumentDetail = function (entity) {
        //    return $http({
        //        headers: { 'Content-Type': "application/json" },
        //        url: serviceBase + "saveDocumentDetail/",
        //        method: "POST",
        //        data: entity

        //    });
        //};
        this.SaveConsolidateBooking = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "SaveConsolidateBooking/",
                method: "POST",
                data: entity

            });
        };
        this.getConsolidateBookingDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getConsolidateBookingDetail/",
                method: "POST",
                data: entity

            });
        };
        this.deleteConsolidateBooking = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "deleteConsolidateBooking/",
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
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getEnquiryDepartments/",
                method: "GET"
            });
        };

        this.getQuotationNo = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getQuotationNo/",
                method: "GET"
            });
        };
        this.getCurrencies = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getCurrencies/",
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
        this.getAllRemarks = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getAllRemarks/",
                method: "GET"
            });
        };
        this.getcarrierAllRates = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getCarrierAllRates/" + id,
                method: "GET"
            });
        };
        this.getSelectedCarrierRates = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getCarrierRates/" + id,
                method: "GET"
            });
        };
        this.getEmailData = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getEmailData/" + id,
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
        this.getDocumentationStatus = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getDocumentationStatus/",
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
        this.getAllSite = function () {
            debugger;
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: sitebaseurl + "GetAllSiteList/",
                method: "GET"
            });
        };
        this.changeSite = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "ChangeSite/",
                method: "POST",
                data: entity

            });
        };

        this.getBookingDetail_BYQuationID = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getBookingDetail_BYQuationID/",
                method: "POST",
                data: entity

            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("containerbookingService", service);

});
