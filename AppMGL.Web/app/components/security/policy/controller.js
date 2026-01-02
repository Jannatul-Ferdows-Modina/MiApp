"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "policyService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.policy;
        $scope.tabs = appUrl.policy.tabs;

        //#endregion

        //#region Policy - Roles

        var openLookupModule = function (action, moduleId, moduleCaption) {

            var modalInstance = $uibModal.open({
                animation: false,
                size: "lg",
                templateUrl: "app/components/security/policyRoles/detail.html",
                controller: "policyRolesController",
                resolve: {
                    requestData: function () {
                        return {
                            action: action,
                            policyId: $scope.entity.policyId,
                            moduleId: moduleId,
                            moduleCaption: moduleCaption,
                            lastAccess: $scope.entity.lastAccess
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    ngNotifier.show(output.data);
                    if (output.resultId == 1001) {
                        $scope.getDetail();
                    }
                    if (action == "addC" || action == "editC") {
                        $scope.rolesTable.reload();
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        }

        var getModuleCaption = function (source) {
            return $(source).parent().parent().find("td:nth-child(2)").text().trim();
        };

        var remove = function (moduleId) {
            if (moduleId > 0) {
                ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                    var policyRole = {
                        policyId: $scope.entity.policyId,
                        moduleId: moduleId,
                        lastAccess: $scope.entity.lastAccess
                    };
                    entityService.deleteModule(policyRole).then(
                        function (output) {
                            ngNotifier.show(output.data);
                            if (output.data.resultId == 1001) {
                                $scope.getDetail();
                                $scope.rolesTable.reload();
                            }
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        });
                });
            }
        };

        $scope.moduleList = [];
        $scope.roles = {};

        $scope.rolesTable = new NgTableParams(
        {
            page: 1,
            count: 1000,
            sorting: $.parseJSON("{ \"ModuleId\": \"asc\" }")
        }, {
            counts: [],
            getData: function (params) {
                var listParams = {
                    OtherId: $scope.entity.policyId,
                    Sort: JSON.stringify(params.sorting()),
                    Filter: "[]"
                };
                return entityService.listModule(listParams).then(
                    function (output) {
                        $scope.moduleList = output.data.data;
                        params.total(output.data.count);
                        return output.data.data;
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });

        $scope.performSubAction = function (source, id) {

            var action = source.currentTarget.attributes["action"].value;

            $scope.$broadcast("show-errors-check-validity");

            if (action != "cancelC" && $scope.form.subdetail != undefined && $scope.form.subdetail.$invalid) {
                if ($scope.form.subdetail.$error.required != undefined && $scope.form.subdetail.$error.required.length > 0) {
                    ngNotifier.error("Required Field(s) are missing data.");
                }
                return;
            }

            switch (action) {

                case "viewDetailC":
                    openLookupModule(action, id, getModuleCaption(source.currentTarget));
                    break;

                case "addC":
                    openLookupModule(action, null, null);
                    break;

                case "editC":
                    openLookupModule(action, id, getModuleCaption(source.currentTarget));
                    break;

                case "deleteC":
                    remove(id);
                    break;
            }
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("policyController", controller);

});
