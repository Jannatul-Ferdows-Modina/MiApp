"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/warehouselocation/";
        var serviceBaseContact = ngAuthSettings.apiServiceBaseUri + "api/customerContact/";

        var serviceBaseQuo = ngAuthSettings.apiServiceBaseUri + "api/Quotation/";

        this.getCompanySearch = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: ngAuthSettings.apiServiceBaseUri + "api/booking/" + "CompanySearch/",
                method: "POST",
                data: listfilter

            });
        };


        this.wareHouseList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "WareHouseList/",
                method: "POST",
                data: listParams
            });
        };
        this.insertWarehouse = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "InsertUpdateWareHouse/",
                method: "POST",
                data: listParams
            });
        };
        this.deletewarehouse = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "DeleteWareHouse/" + id,
                method: "POST"
              
            });
        };


        
        this.getdetailwarehousemapping = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "WarehoseMappingDetail/" + id,
                method: "GET"
                
            });
        };

        this.getblocknumberlist = function (id) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "Searchblocknumberlist/" + id,
                method: "GET"

            });
        };
        this.SearchQuotation = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "SearchQuotation/",
                method: "POST",
                data: listfilter

            });
        };

        this.SearchWarehouselocation = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "SearchWarehouselocation/",
                method: "POST",
                data: listfilter

            });
        };

        this.getCompanySearch = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBaseContact + "CompanySearch/",
                method: "POST",
                data: listfilter

            });
        };
        this.searchquotationlist = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBaseQuo + "List/",
                method: "POST",
                data: listfilter

            });
        };

        this.warehouseinwardno = function () {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "WarehouseinwardNo/",
                method: "GET"
            });
        };



        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "datamanagement"));
    };

    service.$inject = injectParams;

    app.register.service("warehousemappingService", service);

});
