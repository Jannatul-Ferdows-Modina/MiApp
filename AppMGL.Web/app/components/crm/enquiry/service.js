"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/enquiry/";
        var sitebaseurl = ngAuthSettings.apiServiceBaseUri + "api/site/";
        var contactbaseurl = ngAuthSettings.apiServiceBaseUri + "api/CustomerContact/";

        this.sendemail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "SendEmail/",
                method: "POST",
                data: entity

            });
        };



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
        this.getEnquiryEmailDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "GetEnquiryEmailDetail/",
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
        this.getCompanySearch = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "CompanySearch/",
                method: "POST",
                data: listfilter

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
        this.exportReportRemark = function (reportParams) {
            return $http({
                cache: false,
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "ExportReportRemark_CRM/",
                method: "POST",
                data: reportParams,
                responseType: "arraybuffer"
            });
        };

        this.listcrmenq = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "listcrm/",
                method: "POST",
                data: listParams
            });
        };
        this.getCompanySearchCRM = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: contactbaseurl + "CompanySearchCRM/",
                method: "POST",
                data: listfilter

            });
        };

        this.sendemail_EA = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "SendEmail_EA/",
                method: "POST",
                data: entity

            });
        };
        this.getNextActionDateList = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "NextActionDateList/",
                method: "POST",
                data: entity

            });
        };
        this.exportToExcelRemarks = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "ExportToExcelRemarks/",
                method: "POST",
                data: entity,
                responseType: "arraybuffer"

            });
        };

        this.getBookingHistory = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "GetBookingHistory/",
                method: "POST",
                data: entity              

            });
        };

        this.getBookingDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: ngAuthSettings.apiServiceBaseUri + "api/booking/" + "getBookingDetail_CRM/",
                method: "POST",
                data: entity

            });
        };
        this.getContainerCategories = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: ngAuthSettings.apiServiceBaseUri + "api/booking/" + "getContainerCategories/",
                method: "GET"
            });
        };
        this.getContainerSizes = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: ngAuthSettings.apiServiceBaseUri + "api/booking/" + "getContainerSizes/",
                method: "GET"
            });
        };
        this.getDocumentationStatus = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: ngAuthSettings.apiServiceBaseUri + "api/booking/" + "getDocumentationStatus/",
                method: "GET"
            });
        };

        this.getAllCarriers = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: ngAuthSettings.apiServiceBaseUri + "api/booking/" + "getAllCarriers/",
                method: "GET"
            });
        };
        this.getAllSite = function () {
            
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: ngAuthSettings.apiServiceBaseUri + "api/site/" + "GetAllSiteList/",
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
        this.addenqremark = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: ngAuthSettings.apiServiceBaseUri + "api/enquiry/" + "AddEnquiryRemark/",
                method: "POST",
                data: entity
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("crmenquiryService", service);

});
