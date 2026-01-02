"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/country/";

        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "setup"));
    };

    service.$inject = injectParams;

    app.register.service("countryService", service);

});
