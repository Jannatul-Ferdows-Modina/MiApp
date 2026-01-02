"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/customerContact/";

        

        this.getContactList = function () {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "contactList/",
                method: "POST",

            });
        };
        this.saveAttachment = function (filename) {
            var fd = new FormData();
            fd.append('file', filename);
            var uploadUrl = serviceBase + "contactList/";

            var request = {
                method: 'POST',
                url: serviceBase + "saveAttachment/",
                data: fd,
                headers: {
                    'Content-Type': undefined
                }
            };
            $http(request)
                    .success(function (d) {
                        alert(d);
                    })
                    .error(function () {
                    });


        };
        this.saveCustomerContact = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveCustomerContact/",
                method: "POST",
                data: entity

            });
        };
        this.getContactDetail = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getContactDetail/" + id,
                method: "GET"

            });
        };

        this.getLatestCustomerCode = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getLatestCustomerCode/",
                method: "GET"
            });
        };

        this.updateCustomerContact = function (entity, id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/customerContact" + id,
                method: "PUT",
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

        this.downloadAttachment = function (entity) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'fileName': entity.attachment,
                    'contactID': entity.contactID
                },
                url: serviceBase + "downloadAttachment/",
                responseType: "arraybuffer",
                method: "POST"
            });
        };

        this.getAccountCategories = function () {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getAccountCategories/",
                method: "GET"
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "datamanagement"));
    };

    service.$inject = injectParams;

    app.register.service("siplCompanyModalService", service);

});
