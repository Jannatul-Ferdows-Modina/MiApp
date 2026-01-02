"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";

        this.getShippingConfirmationList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getShippingConfirmationList/",
                method: "POST",
                data: listParams
            });
        };

        this.getShipperConfirmationDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getShipperConfirmationDetail/",
                method: "POST",
                data: entity

            });
        };
        this.saveShipperConfirmation = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveShipperConfirmation/",
                method: "POST",
                data: entity

            });
        };

        this.uploadFile = function (entity, $file) {
            return Upload.upload({
                url: serviceBase + "UploadShipperAttachment/",
                method: "POST",
                data: entity,
                file: $file
            });
        };

        this.downloadShipperAttachment = function (entity) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'fileName': entity.attachFile,
                    'documentCommonID': entity.documentCommonID
                },
                url: serviceBase + "downloadShipperAttachment/",
                responseType: "arraybuffer",
                method: "POST"
            });
        };
        
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("bookingShipperConfirmationService", service);

});
