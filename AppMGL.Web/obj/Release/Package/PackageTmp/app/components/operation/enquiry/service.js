"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/enquiry/";
        
        this.getEnquiryList = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "enquiryList/",
                method: "POST",
                
            });
        };
        this.getEnquiryDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getEnquiryDetail/",
                method: "POST",
                data: entity

            });
        };
        this.saveEnquiryIncompleteDraft = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveEnquiryAsIncompleteDraft/",
                method: "POST",
                data: entity
               
            });
        };

        this.saveEnquiry = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveEnquiry/",
                method: "POST",
                data: entity

            });
        };

        this.deleteEnquiry = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "deleteEnquiry/",
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
        
        this.getEnquiryNo = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getEnquiryNo/" + id,
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
        this.getContainerSizes = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getContainerSizes/",
                method: "GET"
            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("enquiryService", service);

});
