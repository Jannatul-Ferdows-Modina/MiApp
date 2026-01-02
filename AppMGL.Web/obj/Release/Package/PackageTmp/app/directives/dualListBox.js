"use strict";

define(["app"], function (app) {

    var injectParams = [];

    var directive = function () {

        return {
            restrict: "A",
            scope: {
                dualListBox: "@",
                optionData: "=",
                optionValue: "@",
                optionText: "@",
                disabled: "=ngDisabled"
            },
            link: function ($scope, element, attributes) {

                element.attr("multiple", "multiple");

                $scope.$watch("optionData", function () {

                    if ($scope.optionData.length > 0) {

                        element.find("option").remove();

                        $scope.optionData.forEach(function (option) {
                            element.append($("<option>", {
                                value: option[$scope.optionValue],
                                text: option[$scope.optionText],
                                selected: option.selected
                            }));
                        });

                        var lb = element.bootstrapDualListbox({
                            infoText: "",
                            infoTextFiltered: "",
                            infoTextEmpty: "",
                        });

                        element.bootstrapDualListbox("refresh");
                    }
                });

                $scope.$watch("disabled", function (newValue, oldValue) {
                    element.parent().find("button,input,select").prop("disabled", newValue);
                    element.bootstrapDualListbox("refresh");
                }, true);
            }
        };
    };

    directive.$inject = injectParams;

    app.directive("dualListBox", directive);

});
