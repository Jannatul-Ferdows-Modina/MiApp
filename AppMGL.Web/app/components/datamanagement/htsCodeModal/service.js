"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

       // var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/Booking/";
        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/lgvwhtscode/";
        this.SaveHtsCode = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "saveHtsCode/",
                method: "POST",
                data: entity

            });
        };
        
        this.detail = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "detail/" + id,
                method: "GET"
            });
        };
        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "datamanagement"));
    };

    service.$inject = injectParams;

    app.register.service("htsCodeModalService", service);

});
