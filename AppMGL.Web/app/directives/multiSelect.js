"use strict";

define(["app"], function (app) {

    var injectParams = [];

    var directive = function () {

        return {
            restrict: "A",
            scope: {
                multiSelect: "@",
                optionData: "=",
                optionValue: "@",
                optionText: "@",
                selectDisabled: "="
            },
            link: function ($scope, element, attributes) {

                var options = [];

                if ($scope.optionData) {
                    $scope.optionData.forEach(function (object) {
                        options.push({ label: object[$scope.optionText], value: object[$scope.optionValue] });
                    });
                }

                var buttonClass = "multiselect dropdown-toggle btn btn-default text-left";
                var includeSelectAllOption = true;

                if ($scope.multiSelect == "false") {
                    //buttonClass = "hide";
                    includeSelectAllOption = false;
                }

                element.attr("multiple", "multiple");

                element.multiselect({
                    maxHeight: "200",
                    buttonClass: buttonClass,
                    buttonWidth: "100%",
                    numberDisplayed: 2,
                    includeSelectAllOption: includeSelectAllOption
                });

                element.multiselect("dataprovider", options);

                $scope.$watch("selectDisabled", function (newValue, oldValue) {
                    element.multiselect(($scope.selectDisabled ? "disable" : "enable"));
                }, true);
           }
        };
    };

    directive.$inject = injectParams;

    app.directive("multiSelect", directive);

});
