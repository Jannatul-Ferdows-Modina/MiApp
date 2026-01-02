"use strict";

define(["app"], function (app) {

    var injectParams = ["$timeout"];

    var directive = function ($timeout) {

        return {
            restrict: "A",
            link: function (scope, element, attributes) {

                $timeout(function () {
                    element.find("thead").sticky({ topSpacing: 50 });
                }, 0);
            }
        };
    };

    directive.$inject = injectParams;

    app.directive("fixedHeader", directive);

});
