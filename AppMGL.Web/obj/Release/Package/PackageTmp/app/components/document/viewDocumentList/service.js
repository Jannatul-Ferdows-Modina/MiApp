"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/documentCommon/";

        this.getDocumentList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "getDocumentList/",
                method: "POST",
                data: listParams
            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "report"));
    };

    service.$inject = injectParams;

    app.register.service("viewDocumentListService", service);

});
