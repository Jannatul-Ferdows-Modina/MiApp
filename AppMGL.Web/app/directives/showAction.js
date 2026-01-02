"use strict";

define(["app"], function (app) {

    var injectParams = ["localStorageService"];

    var directive = function (localStorageService) {

        return {
            restrict: "A",
            scope: {
                showAction: "@",
                statusId: "@",
                modId: "="
            },
            link: function (scope, element, attributes) {

                scope.$watch(function () { return scope.statusId; }, function (newValue, oldValue) {

                    var actions = scope.showAction.split(",");
                    var roles = [];

                    var userRoles = localStorageService.get("userRoles");
                    if (userRoles) {
                        userRoles.forEach(function (r) {
                            actions.some(function (a) {
                                if (scope.$parent.page) {
                                    if (r.modId == scope.$parent.page.modId && r.actId == a) {
                                        roles.push(r);
                                    }
                                }
                                else if (scope.modId) {
                                    if (scope.modId.indexOf(",") != -1) {
                                        if (scope.modId.split(",").indexOf(r.modId.toString()) != -1 && r.actId == a) {
                                            roles.push(r);
                                        }
                                    }
                                    else if (r.modId == scope.modId && r.actId == a) {
                                        roles.push(r);
                                    }
                                    else if (scope.modId == "0" && a == "40") {
                                        roles.push({ modId: "0", actId: "40" });
                                    }
                                }
                            });
                        });
                    }

                    var flag = (roles.length > 0);
                    if (flag && scope.$parent.entity && scope.$parent.entity.statusId) {
                        flag = roles.some(function (r) {
                            if (r.applicableStatus && r.applicableStatus != "") {
                                var statuses = r.applicableStatus.split(",");
                                return statuses.some(function (s) {
                                    return (s == scope.$parent.entity.statusId);
                                });
                            }
                            else {
                                return true;
                            }
                        });
                    }

                    if (element.is("button")) {
                        element.prop("disabled", !flag);
                    }
                    else {
                        element.css("display", (flag ? "" : "none"));
                    }
                }, true);
            }
        };
    };

    directive.$inject = injectParams;

    app.directive("showAction", directive);

});
