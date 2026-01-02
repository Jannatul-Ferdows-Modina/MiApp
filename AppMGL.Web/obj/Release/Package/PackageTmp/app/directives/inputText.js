"use strict";

define(["app"], function (app) {

    var injectParams = [];

    var directive = function () {

        return {
            restrict: "A",
            require: '?ngModel',
            scope: {
                model: "=ngModel",
                inputText: "@"
            },
            link: function ($scope, element, attributes, model) {

                if (!model) return;

                model.$parsers.unshift(function (inputValue) {

                    var output = inputValue;

                    if ($scope.inputText == "CODE") {
                        output = inputValue.replace(/[^a-zA-Z0-9]/g, "").toUpperCase();
                    }

                    model.$setViewValue(output);
                    model.$render();

                    return output;
                });
            }
        };
    };

    directive.$inject = injectParams;

    app.directive("inputText", directive);

});
