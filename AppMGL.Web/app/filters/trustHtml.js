"use strict";

define(["app"], function (app) {

    var injectParams = ["$sce"];

    var filterfactory = function ($sce) {

        return function (value) {

            return $sce.trustAsHtml(value);
        };
    };

    filterfactory.$inject = injectParams;

    app.filter("trustHtml", filterfactory);

});
