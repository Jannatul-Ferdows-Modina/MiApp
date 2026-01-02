"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

       // var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/siplcountry/";
        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";
        this.getState = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "State/",
                method: "POST",
                data: listfilter

            });
        };
        this.getCountry = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "Country/",
                method: "POST",
                data: listfilter

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
        this.saveAesTemplate = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "SaveAesTemplate/",
                method: "POST",
                data: entity

            });
        };

        this.getAesTemplateList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "GetAesTemplateList/",
                method: "POST",
                data: listParams
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
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "datamanagement"));
    };

    service.$inject = injectParams;

    app.register.service("aestemplateService", service);

});
