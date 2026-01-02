"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/quotation/";
        
        this.getEnquiryList = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "quotationList/",
                method: "POST",
                
            });
        };
        this.getQuotaionDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getQuotaionDetail/",
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
        this.saveQuotation = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveQuotation/",
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
        this.sendEmail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "sendEmail/",
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
        this.getCarrierAllCharges = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getCarrierAllCharges/",
                method: "GET"
            });
        };
        this.getEnquiryContainers = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getEnquiryContainers/" + id,
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
        this.getEmailIds = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getEmailIds/" + id,
                method: "GET"
            });
        };
        this.ExportQuptationTempData = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "ExportQuotationData/",
                method: "GET",
                responseType: "arraybuffer"
               
            });
        };
        //this.uploadFileToUrl = function (fd) {
        //    //var fd = new FormData();
        //    //  fd.append('file', file);
        //    return $http({
        //        //headers: { 'Content-Type': undefined }
        //        headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
        //        url: serviceBase + "QuotationExcelUpload/" + fd,
        //        method: "POST",
        //       transformRequest: angular.identity,
                
        //    });
                
        //};
        this.uploadQuotationFinalFile = function (entity, $file) {
            return Upload.upload({
                url: serviceBase + "uploadQuotationFinalFile/",
                method: "POST",
                data: entity,
                file: $file
            });
        };
        this.downloadQuotationFinal = function (siteid) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'FileName': 'Tracking',
                    'SiteId': siteid
                },
                url: serviceBase + "downloadQuotationFinal/",
                responseType: "arraybuffer",
                method: "POST"
            });
        };
        this.deleteTempQuotation = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "TempDeleteQuotation/" + id,
                method: "DELETE"
            });
        };
        this.exportReport = function (reportParams) {
            return $http({
                cache: false,
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "TrackingExportReport/",
                method: "POST",
                data: reportParams,
                responseType: "arraybuffer"
            });           

        };
        this.copyQuotaion = function (entity) {
            var httppath = ngAuthSettings.apiServiceBaseUri + 'api/booking/'
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: httppath + "CopyQuotation/",
                method: "POST",
                data: entity

            });
        };
        this.getQuotationRemark= function(id)
        {
            var httppath = ngAuthSettings.apiServiceBaseUri + 'api/booking/'
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "GetQuotationRemark/" + id,
                method: "GET"              

            });
        }
        this.exportReportRemark = function (reportParams) {
            return $http({
                cache: false,
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "ExportReportRemarkQuotation/",
                method: "POST",
                data: reportParams,
                responseType: "arraybuffer"
            });
        };

        this.SaveQuationRemarks = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "SaveQuationRemarks/",
                method: "POST",
                data: entity

            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

   

    service.$inject = injectParams;

    app.register.service("quotationService", service);

});
