"use strict";

define(["app"], function (app) {

    var injectParams = ["$filter"];

    var directive = function ($filter) {

        return {
            restrict: "A",
            require: '?ngModel',
            scope: {
                model: "=ngModel",
                inputNumber: "=",
                allowDecimal: "=",
                allowNegative: "=",
                maxDigit: "=",
                maxDecimal: "=",
                showSpin: "="
            },
            link: function ($scope, element, attributes, model) {

                if (!model) return;

                model.$formatters.unshift(function (inputValue) {
                    if ($scope.inputNumber) {
                        return $filter("number")(model.$modelValue);
                    }
                    return model.$modelValue;
                });

                model.$parsers.unshift(function (inputValue) {
                    var decimalFound = false;
                    var output = inputValue.split("").filter(function (s, i) {
                        var valid = (!isNaN(s) && s != " ");
                        if (!valid && $scope.allowDecimal) {
                            if (!decimalFound && s == ".") {
                                decimalFound = true;
                                valid = true;
                            }
                        }
                        if (!valid && $scope.allowNegative) {
                            valid = (s == "-" && i == 0);
                        }
                        return valid;
                    }).join("");

                    var data = output.split(".");

                    if ($scope.maxDigit) {
                        if (data[0].length > $scope.maxDigit) {
                            data[0] = data[0].substring(0, $scope.maxDigit);
                        }
                    }

                    if ($scope.inputNumber) {
                        data[0] = data[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }

                    if (data.length == 2 && $scope.maxDecimal) {
                        if (data[1].length > $scope.maxDecimal) {
                            data[1] = data[1].substring(0, $scope.maxDecimal);
                        }
                    }

                    output = data.join(".");

                    //model.$viewValue = output;
                    model.$setViewValue(output);
                    model.$render();

                    return output;
                });

                element.bind("keypress", function (event) {

                    //if (event.keyCode != 8 && event.keyCode != 46 && String.fromCharCode(event.keyCode).replace(/[^0-9.,-]/g, "") == "") {
                    //    event.preventDefault();
                    //}
                    if (event.keyCode === 32) {
                        event.preventDefault();
                    }
                });

                if (!element.hasClass("text-left")) {
                    element.addClass("text-right");
                }

                if ($scope.showSpin) {
                    element.TouchSpin({
                        min: parseInt(element.attr("min")),
                        max: parseInt(element.attr("max")),
                        decimals: $scope.maxDecimal || 0,
                        step: parseFloat(element.attr("step"))
                    });
                }
            }
        };
    };

    directive.$inject = injectParams;

    app.directive("inputNumber", directive);

});
