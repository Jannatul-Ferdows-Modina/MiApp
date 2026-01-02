"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";

        this.getExportRegisterList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getExportRegisterList/",
                method: "POST",
                data: listParams
            });
        };
        this.getEmailDocumentDetail = function (EnqId) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache", "EnqId": "" + EnqId + "" },
                url: serviceBase + "GetEmailDocumentDetail/",
                method: "GET",
                data:{}
            });
        };
        this.prealert = function (EnqId, obj) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache", "EnqId": "" + EnqId + "" },
                url: serviceBase + "PreAlert/",
                method: "POST",
                data:  obj 
            });
        };
        this.downloadAttachmentPreAlert = function (entity) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'fileName': entity.attachFile,
                    'documentCommonID': entity.documentCommonID,
                    'documentType': entity.documentType,
                    'isSystemGenerated': entity.isSystemGenerated
                },
                url: serviceBase + "downloadAttachment/",
                responseType: "arraybuffer",
                method: "POST"
            });
        };

        this.saveCustomerDocAttachementsPreAlert = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveCustomerDocAttachements/",
                method: "POST",
                data: entity

            });
        };

        this.prealertsendemail = function (QuotationEmailData) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "SendEmailPreAlert/",
                method: "POST",
                data: QuotationEmailData
            });
        };

        this.getDocumentAttachmentDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getDocumentAttachmentDetail/",
                method: "POST",
                data: entity

            });
        };

        this.GetAllActionRemarks = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "GetAllActionRemarks/",
                method: "POST",
                data: entity

            });
        };

        this.getCustomerDocumentDetails = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getCustomerDocumentDetails/",
                method: "POST",
                data: entity

            });
        };

        this.saveDocumentAttachement = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveDocumentAttachement/",
                method: "POST",
                data: entity

            });
        };

        this.downloadAttachment = function (entity) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'fileName': entity.attachFile,
                    'documentCommonID': entity.documentCommonID
                },
                url: serviceBase + "downloadAttachment/",
                responseType: "arraybuffer",
                method: "POST"
            });
        };
        this.deleteShipmentDoc = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "deleteShipmentDoc/",
                method: "POST",
                data: entity

            });
        };
        this.uploadFile = function (entity, $file) {
            return Upload.upload({
                url: serviceBase + "upload/",
                method: "POST",
                data: entity,
                file: $file
            });
        };
        this.exportRegisterReport = function (reportParams) {
            return $http({
                cache: false,
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "exportRegisterReport/",
                method: "POST",
                data: reportParams,
                responseType: "arraybuffer"
            });
        };

        this.getExportRegisterDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getExportRegisterDetail/",
                method: "POST",
                data: entity

            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("exportRegisterReportCorporateService", service);

});
