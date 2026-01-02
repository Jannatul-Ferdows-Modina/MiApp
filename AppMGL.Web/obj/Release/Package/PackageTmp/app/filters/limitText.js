"use strict";

define(["app"], function (app) {

    var injectParams = [];

    var filterfactory = function () {

        return function (value, limit, ishtml, tail) {

            if (!value) {
                return "";
            }

            if (ishtml) {
                value = angular.element(value).text();
            }

            limit = parseInt(limit);

            if (!limit) {
                return value;
            }

            if (value.length <= limit) {
                return value;
            }

            value = value.substring(0, limit);

            return value + (tail || " ...");
        };
    };

    filterfactory.$inject = injectParams;

    app.filter("limitText", filterfactory);

});
