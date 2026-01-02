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

        this.getExpensesReportDetails = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getExpensesReportDetails/",
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

    app.register.service("processInvoiceReportService", service);

});
