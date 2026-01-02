"use strict";

define(["app"], function (app) {

    var injectParams = [];

    var directive = function () {

        var selectors = {
            //Remove button selector
            remove: '[box-widget="remove"]',
            //Collapse button selector
            collapse: '[box-widget="collapse"]'
        };
        var icons = {
            //Collapse icon
            collapse: 'fa-minus',
            //Open icon
            open: 'fa-plus',
            //Remove icon
            remove: 'fa-times'
        };
        var animationSpeed = 500;

        return {
            restrict: "A",
            scope: {
                boxWidget: "@"
            },
            link: function ($scope, element, attributes) {

                element.on("click", function () {
                    //Find the box parent
                    var box = element.parents(".box").first();
                    //Listen for collapse event triggers
                    if ($scope.boxWidget == "collapse") {
                        var _this = this;
                        var box_content = box.find("> .box-body, > .box-footer, > form  >.box-body, > form > .box-footer");
                        if (!box.hasClass("collapsed-box")) {
                            element.find("span").text("Show");
                            //Convert minus into plus
                            element.children(":first")
                                .removeClass(icons.collapse)
                                .addClass(icons.open);
                            //Hide the content
                            box_content.slideUp(animationSpeed, function () {
                                box.addClass("collapsed-box");
                            });
                        } else {
                            element.find("span").text("Hide");
                            //Convert plus into minus
                            element.children(":first")
                                .removeClass(icons.open)
                                .addClass(icons.collapse);
                            //Show the content
                            box_content.slideDown(animationSpeed, function () {
                                box.removeClass("collapsed-box");
                            });
                        }
                    }
                    //Listen for remove event triggers
                    else if ($scope.boxWidget == "remove") {
                        box.slideUp(animationSpeed);
                    }
                });
            }
        };
    };

    directive.$inject = injectParams;

    app.directive("boxWidget", directive);

});
