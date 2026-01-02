"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/ddtcusml/";
        //var serviceBase1 = ngAuthSettings.apiServiceBaseUri + "api/booking/";
        this.getHTSCode = function (listfilter) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase1 + "htsCodeSearch/",
                method: "POST",
                data: listfilter

            });
        };

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "datamanagement"));
    };

    service.$inject = injectParams;

    app.register.service("ddtcusmlService", service);

});
