"use strict";

define(["app"], function (app) {

    var injectParams = [];

    var directive = function () {

        return {
            restrict: "A",
            scope: {
                customSortable: "@",
                tableName: "="
            },
            link: function ($scope, element, attributes) {

                element.append(angular.element("<span class=\"sort-indicator\"></span>"));

                if ($scope.tableName.isSortBy($scope.customSortable, "asc")) {
                    element.addClass("sort-asc");
                }
                else if ($scope.tableName.isSortBy($scope.customSortable, "desc")) {
                    element.addClass("sort-desc");
                }

                element.on("click", function () {

                    element.parent().find("th").each(function (index, columnHeader) {
                        if ($(columnHeader).hasClass("sort-asc")) {
                            $(columnHeader).removeClass("sort-asc");
                        }
                        if ($(columnHeader).hasClass("sort-desc")) {
                            $(columnHeader).removeClass("sort-desc");
                        }
                    });

                    $scope.$apply(function () {
                        var sort = $scope.tableName.isSortBy($scope.customSortable, "asc") ? "desc" : "asc";
                        $scope.tableName.sorting($.parseJSON("{ \"" + $scope.customSortable + "\": \"" + sort + "\" }"));
                    });

                    if ($scope.tableName.isSortBy($scope.customSortable, "asc")) {
                        element.addClass("sort-asc");
                    }
                    else if ($scope.tableName.isSortBy($scope.customSortable, "desc")) {
                        element.addClass("sort-desc");
                    }
                });
            }
        };
    };

    directive.$inject = injectParams;

    app.directive("customSortable", directive);

});
