"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/booking/";
        

        this.getContainerList = function (listParams) {
           
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "GetContainerList/",
                method: "POST",
                data: listParams
            });
        };
        this.getContainerDetail = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
                url: serviceBase + "GetContainerDetail/",
                method: "POST",
                data: entity

            });
        };
       
        this.saveContainer = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "SaveContainer/",
                method: "POST",
                data: entity

            });
        };

        this.deleteContainer = function (entity) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "deleteContainer/",
                method: "POST",
                data: entity

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
       

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "operation"));
    };

    service.$inject = injectParams;

    app.register.service("containermanagementService", service);

});
