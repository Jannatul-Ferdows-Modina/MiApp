"use strict";

var baseService = function ($http, $q, ngAuthSettings, serviceBase, Upload, moduleType) {

    //#region Private Properties

    var path = ngAuthSettings.apiServiceBaseUri + "api/";
    var factory = {};

    factory.location = ngAuthSettings.apiServiceBaseUri;

    //#endregion

    //#region Non-Transaction Methods
    
    factory.list = function (listParams) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: serviceBase + "list/",
            method: "POST",
            data: listParams
        });
    };

    factory.detail = function (id) {
        return $http({
            headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache" },
            url: serviceBase + "detail/" + id,
            method: "GET"
        });
    };

    factory.lookup = function (name, method, listParams) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: path + name + "/" + (method == null ? "lookup" : method) + "/",
            method: "POST",
            data: listParams
        });
    };

    factory.log = function (listParams) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: path + moduleType + "Log/list/",
            method: "POST",
            data: listParams
        });
    };

    factory.listData = function (name, method, listParams) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: path + name + "/" + method + "/",
            method: "POST",
            data: listParams
        });
    };

    //#endregion

    //#region Transaction Methods

    factory.insert = function (entity) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: serviceBase + "insert/",
            method: "POST",
            data: entity
        });
    };

    factory.update = function (entity, id) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: serviceBase + "update/" + id,
            method: "PUT",
            data: entity
        });
    };

    factory.delete = function (entity) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: serviceBase + "delete/",
            method: "POST",
            data: entity
        });
    };

    factory.changeStatus = function (entity, action) {
        return $http({
            headers: { 'Content-Type': "application/json" },
            url: serviceBase + action + "/",
            method: "POST",
            data: entity
        });
    };

    //#endregion

    return factory;
};
