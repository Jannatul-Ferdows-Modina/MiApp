"use strict";

define(["app"], function (app) {

    var injectParams = ["$interpolate"];

    var directive = function ($interpolate) {

        return {
            restrict: 'A',
            priority: Number.MIN_SAFE_INTEGER, // execute last, after all other directives if any.
            link: function ($scope, $element, $attributes) {
                var expressions = $attributes.whenReady.split(';');
                var waitForInterpolation = false;
                var hasReadyCheckExpression = false;

                function evalExpressions(expressions) {
                    expressions.forEach(function (expression) {
                        $scope.$eval(expression);
                    });
                }

                if ($attributes.whenReady.trim().length === 0) { return; }

                if ($attributes.waitForInterpolation && $scope.$eval($attributes.waitForInterpolation)) {
                    waitForInterpolation = true;
                }

                if ($attributes.readyCheck) {
                    hasReadyCheckExpression = true;
                }

                if (waitForInterpolation || hasReadyCheckExpression) {
                    requestAnimationFrame(function checkIfReady() {
                        var isInterpolated = false;
                        var isReadyCheckTrue = false;

                        if (waitForInterpolation && $element.text().indexOf($interpolate.startSymbol()) >= 0) { // if the text still has {{placeholders}}
                            isInterpolated = false;
                        }
                        else {
                            isInterpolated = true;
                        }

                        if (hasReadyCheckExpression && !$scope.$eval($attributes.readyCheck)) { // if the ready check expression returns false
                            isReadyCheckTrue = false;
                        }
                        else {
                            isReadyCheckTrue = true;
                        }

                        if (isInterpolated && isReadyCheckTrue) { evalExpressions(expressions); }
                        else { requestAnimationFrame(checkIfReady); }

                    });
                }
                else {
                    evalExpressions(expressions);
                }
            }
        };
    };

    directive.$inject = injectParams;

    app.directive("whenReady", directive);

});
