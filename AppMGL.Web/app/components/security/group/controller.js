"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "groupService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.group;
        $scope.tabs = appUrl.group.tabs;

        //#endregion

        //#region Lookup 

        $scope.lookups = { groupTypes: [] };

        $scope.initDropdown = function () {
            var listParams = {
                OtherId: 2000,
                PageIndex: 1,
                PageSize: 100,
                Sort: "{\"EnumValueId\":\"asc\"}",
                Filter: "[]"
            };
            entityService.lookup("enumValue", null, listParams).then(
                function (output) {
                    $scope.lookups.groupTypes = output.data.data;
                    $scope.entity.groupTypeId = $scope.lookups.groupTypes[0].enumValueId;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "policy") {
                $scope.entity.policyId = output.data[0].policyId;
                $scope.entity.policyCode = output.data[0].policyCode;
                $scope.entity.policyName = output.data[0].policyName;
            }
            else if (lookup == "site") {
                $scope.entity.siteId = output.data[0].siteId;
                $scope.entity.siteCode = output.data[0].siteCode;
                $scope.entity.siteName = output.data[0].siteName;
                $scope.entity.policyId = output.data[0].policyId;
            }
        };

        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "policy") {
                $scope.entity.policyId = "";
                $scope.entity.policyCode = "";
                $scope.entity.policyName = "";
            }
            else if (lookup == "site") {
                $scope.entity.siteId = "";
                $scope.entity.siteCode = "";
                $scope.entity.siteName = "";
                $scope.entity.policyId = "";
            }
        };

        //#endregion

        //#region Detail

        $scope.isGlobalGroup = true;
        $scope.requiredUpdateG = false;
        $scope.requiredUpdateL = false;

        $scope.changeGroupType = function (groupTypeId) {

            $scope.isGlobalGroup = (groupTypeId == 2001);
            $scope.requiredUpdateG = (groupTypeId == 2001);
            $scope.requiredUpdateL = (groupTypeId == 2002);
            $scope.clearLookups("policy");
            $scope.clearLookups("site");
        };

        $scope.afterGetDetail = function () {

            $scope.isGlobalGroup = ($scope.entity.groupTypeId == 2001);
        };

        $scope.afterPerformAction = function (source, fromList) {

            var action = source.currentTarget.attributes["action"].value;

            switch (action) {
                case "add":
                    $scope.isGlobalGroup = true;
                    $scope.requiredUpdateG = true;
                    $scope.requiredUpdateL = false;
                    break;
                case "edit":
                    $scope.requiredUpdateG = ($scope.entity.groupTypeId == 2001);
                    $scope.requiredUpdateL = ($scope.entity.groupTypeId == 2002);
                    break;
                default:
                    $scope.requiredUpdateG = false;
                    $scope.requiredUpdateL = false;
                    break;
            }
        };

        //#endregion

        //#region Group - Roles

        var openLookupModule = function (action, moduleId, moduleCaption) {

            var modalInstance = $uibModal.open({
                animation: false,
                size: "lg",
                templateUrl: "app/components/security/groupRoles/detail.html",
                controller: "groupRolesController",
                resolve: {
                    requestData: function () {
                        return {
                            action: action,
                            groupId: $scope.entity.groupId,
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
                    var groupRole = {
                        groupId: $scope.entity.groupId,
                        moduleId: moduleId,
                        lastAccess: $scope.entity.lastAccess
                    };
                    entityService.deleteModule(groupRole).then(
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
                    OtherId: $scope.entity.groupId,
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

    app.register.controller("groupController", controller);

});
