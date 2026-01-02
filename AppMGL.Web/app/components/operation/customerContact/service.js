"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/customerContact/";
        var serviceBaseQb = ngAuthSettings.apiServiceBaseUri + "api/MCSReport/";
        var serviceBase1 = ngAuthSettings.apiServiceBaseUri;
        this.getCities = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "getCities/" + id,
                method: "GET"
            });
        };

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
        this.CheckDuplicate = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "CheckDuplicate/",
                method: "POST",
                data: entity

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
	this.getCompanySearchSeptara = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "CompanySearchSeptara/",
                method: "POST",
                data: listfilter

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

        this.exportContactReport = function (reportParams) {
            return $http({
                cache: false,
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "exportContactReport/",
                method: "POST",
                data: reportParams,
                responseType: "arraybuffer"
            });
        };
        
        this.SearchCustomerQB = function (Custname) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'CustomerName': Custname
                },
                url: serviceBaseQb + "SearchCustomerQB/",
                method: "POST"
            });
        };

        this.LinkCustomerQB = function (contactid, qbid, qbcustomername) {
            return $http({
                headers: {
                    'Content-Type': 'application/json; charset=utf-8',
                    'CustomerId': contactid,
                    'QBId': qbid,
                    'QBCustomerName': qbcustomername
                },
                url: serviceBaseQb + "LinkCustomerQB/",
                method: "POST"
            });
        };

        this.qbRefreshTokens = function () {
            var t = serviceBase1 + "api/QuickBook/RefreshToken";
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: t,
                method: "POST",
                data: {}
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
        this.getEmailDetail = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getEmailDetail/" + id,
                method: "GET",
              
            });
        };

        this.saveCustomerContactAddress = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveCustomerContactAddress/",
                method: "POST",
                data: entity

            });
        };
        this.listAddress =function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "ContactListAddress/",
                method: "POST",
                data: listParams
            });
        };

        this.getAddressDetail = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getContactDetailAddress/" + id,
                async:true,
                method: "GET"                
            });
        };
        this.deleteAddressDetail = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "deleteContactDetailAddress/" + id,
                async: true,
                method: "POST"
            });
        };
        this.uploadImportFile = function ($file, id) {
            return Upload.upload({
                url: serviceBase + "ImportCustomer/" + id,
                method: "POST",
                responseType: "arraybuffer",
                file: $file
            });
        };
        this.createUser = function(obj)
        {
            var url = ngAuthSettings.apiServiceBaseUri + "api/user/CreateCompanyUser/"
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: url,
                data: (obj),
                method: "POST"
            });
        }

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("customerContactService", service);

});
