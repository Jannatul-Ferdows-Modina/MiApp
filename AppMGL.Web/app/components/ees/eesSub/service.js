"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";

        this.getExportRegisterList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getExportRegisterListEES/",
                method: "POST",
                data: listParams
            });
        };
        this.saveEES = function (objees) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "Save_EES_Submit/",
                method: "POST",
                data: objees
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
                url: serviceBase + "ExportRegisterReport_AES/",
                method: "POST",
                data: reportParams,
                responseType: "arraybuffer"
            });
        };

        this.getExportRegisterDetailEES = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getExportRegisterDetailEES/",
                method: "POST",
                data: entity

            });
        };
        this.addAesSubmission = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "Save_EES_Submit/",
                method: "POST",
                data: entity

            });
        };

        this.getHTSCode = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "htsCodeSearch/",
                method: "POST",
                data: listfilter

            });
        };
        this.getPortofExport = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "PortofExportSearch/",
                method: "POST",
                data: listfilter

            });
        };

        this.getCountryofDestination = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "CountrySearch/",
                method: "POST",
                data: listfilter

            });
        };

        this.getOriginState = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "OriginState/",
                method: "POST",
                data: listfilter

            });
        };
        this.UploadEssFile = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "UploadEssFile/",
                method: "POST",
                data: listParams

            });
        };
        
        this.DownloadEssFile = function (documentCommonID, aesFileName) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'aesFileName': aesFileName,
                    'documentCommonID': documentCommonID
                },
                url: serviceBase + "DownloadEssFile/",
                responseType: "arraybuffer",
                method: "POST"
            });
        };
        this.ViewFile = function (documentCommonID, aesFileName) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'aesFileName': aesFileName,
                    'documentCommonID': documentCommonID
                },
                url: serviceBase + "ViewFile/",
                method: "POST"
            });
        };
        this.ViewInputFile = function (aesFileName) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'aesFileName': aesFileName
                },
                url: serviceBase + "ViewInputFile/",
                method: "POST"
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
        this.UpdateStatus = function (type, dcommonid, aesid, itanno) {
            return $http({
                headers: {
                    'Content-Type': 'application/json',
                    'statustype': type,
                    'documetcommanid': dcommonid,
                    'aesid': aesid,
                    'itanno': itanno
                },
                url: serviceBase + "UpdateStatus/",
                method: "POST"
            });
        };
        this.GetEssFileList = function (documentCommonID) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'documentCommonID': documentCommonID
                },
                url: serviceBase + "GetEssFileList/",
                method: "POST"

            });
        };

        this.pdfreportdownload = function (reportParams) {
            return $http({
                cache: false,
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "AESReport/",
                method: "POST",
                data: reportParams,
                responseType: "arraybuffer"
            });
        };
        this.UploadSLIForm = function (entity, $file) {
            return Upload.upload({
                url: serviceBase + "UploadSLIForm/",
                method: "POST",
                data: entity,
                file: $file
            });
        };
        //this.GetTemplateList = function (siteId) {
        //    return $http({
        //        headers: {
        //            'Content-Type': 'application/json; charset=utf-8',

        //            'siteId': siteId
        //        },
        //        url: serviceBase + "GetTemplateList/",
        //        method: "POST"

        //    });
        //};
        this.GetTemplateList = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "GetTemplateList/",
                method: "POST",
                data: listfilter

            });
        };
        
        this.GetPartTemplateList = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "GetPartTemplateList/",
                method: "POST",
                data: listfilter

            });
        };
        
        this.getAesTemplateDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getAesTemplateDetail/",
                method: "POST",
                data: entity

            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("eessubService", service);

});
