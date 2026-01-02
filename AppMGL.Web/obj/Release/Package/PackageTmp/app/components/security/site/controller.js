"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "siteService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.site;
        $scope.tabs = appUrl.site.tabs;

        //#endregion

        //#region Lookup

        $scope.lookups = { siteTypes: [], countries: [], states: [] };

        $scope.initDropdown = function () {

            $scope.lookups.siteTypes = [
                { sitTypeId: 0, sitTypeName: "Franchise" },
                { sitTypeId: 1, sitTypeName: "Own" }
            ];

            $scope.fetchLookupData("country", 0, "CryName", "countries", null);
            $scope.fetchLookupData("state", 0, "UstName", "states", null);
        };

        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "contact") {
                $scope.role.cntId = output.data[0].cntId;
                $scope.role.fullName = output.data[0].fullName;
            }
            else if (lookup == "role") {
                $scope.role.rleId = output.data[0].rleId;
                $scope.role.rleName = output.data[0].rleName;
            }
        };

        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "contact") {
                $scope.role.cntId = null;
                $scope.role.fullName = "";
            }
            else if (lookup == "role") {
                $scope.role.rleId = null;
                $scope.role.rleName = "";
            }
        };

        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {

            if (lookupModule == "contact" || lookupModule == "role") {
                if ($scope.role[lookupField] == null || $scope.role[lookupField] == "") {
                    $scope.clearLookups(source, lookupModule, lookupIndex);
                }
            }
        };

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.sitCreatedTs = new Date();
                $scope.entity.sitCreatedBy = $scope.$parent.userInfo.usrId;
                $scope.entity.location.lcnCreatedTs = new Date();
                $scope.entity.location.lcnCreatedBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.sitUpdatedTs = new Date();
                $scope.entity.sitUpdatedBy = $scope.$parent.userInfo.usrId;
                $scope.entity.location.lcnUpdatedTs = new Date();
                $scope.entity.location.lcnUpdatedBy = $scope.$parent.userInfo.usrId;
            }
        };

        //#endregion

        //#region Mapping

        var lastActionC = "";

        var fetchAction = function (id) {
            if (id) {
                $scope.role = $.extend(true, {}, $.grep($scope.roleTable.data, function (obj) { return obj.scrId == id; })[0]);
            }
        };

        var disableControls = function (flag, action) {
            $("div > button[action='editC'] , div > button[action='deleteC']").prop("disabled", flag);
            $scope.disabledInput = flag;
            $scope.requiredInput = !flag;
            if (action == "viewDetailC") {
                $scope.disabledInput = true;
                $scope.requiredInput = false;
            }
        };

        var save = function () {

            if (lastActionC === "addC") {
                $scope.role.sitId = $scope.entity.sitId;
                $scope.role.scrCreatedTs = new Date();
                $scope.role.scrCreatedBy = $scope.$parent.userInfo.usrId;
                entityService.insertRole($scope.role).then(
                    function (output) {
                        $scope.role = {};
                        $scope.roleTable.reload();
                        ngNotifier.show(output.data);
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });
            } else if (lastActionC === "editC" && $scope.role.scrId > 0) {
                $scope.role.scrUpdatedTs = new Date();
                $scope.role.scrUpdatedBy = $scope.$parent.userInfo.usrId;
                entityService.updateRole($scope.role, $scope.role.scrId).then(
                    function (output) {
                        $scope.role = {};
                        $scope.roleTable.reload();
                        ngNotifier.show(output.data);
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });
            }
        };

        var remove = function (id) {

            if (id == null) {
                id = $scope.role.scrId;
            }
            if (id > 0) {
                fetchAction(id);
                ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                    entityService.deleteRole($scope.role).then(
                        function (output) {
                            $scope.role = {};
                            $scope.roleTable.reload();
                            ngNotifier.show(output.data);
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        });
                });
            }
        };

        $scope.editModeSub = false;
        $scope.disabledInput = true;
        $scope.requiredInput = false;
        $scope.role = {};

        $scope.performSubAction = function (source, id) {

            var action = source.currentTarget.attributes["action"].value;

            $scope.$broadcast("show-errors-check-validity");

            if (action != "cancelC" && $scope.form.subdetail != undefined && $scope.form.subdetail.$invalid) {
                if ($scope.form.subdetail.$error.required != undefined && $scope.form.subdetail.$error.required.length > 0) {
                    ngNotifier.error($scope.resource["ErrRequiredFields"]);
                }
                return;
            }

            switch (action) {

                case "viewDetailC":
                    lastActionC = "";
                    $scope.editModeSub = false;
                    fetchAction(id);
                    disableControls(false, action);
                    break;

                case "addC":
                    lastActionC = action;
                    $scope.editModeSub = true;
                    $scope.role = {};
                    disableControls(false, action);
                    break;

                case "editC":
                    lastActionC = action;
                    $scope.editModeSub = true;
                    fetchAction(id);
                    disableControls(false, action);
                    break;

                case "saveC":
                    save();
                    lastActionC = "";
                    $scope.editModeSub = false;
                    disableControls(true, action);
                    break;

                case "cancelC":
                    lastActionC = "";
                    $scope.editModeSub = false;
                    $scope.role = {};
                    disableControls(true, action);
                    break;

                case "deleteC":
                    remove(id);
                    lastActionC = "";
                    $scope.editModeSub = false;
                    disableControls(true, action);
                    break;
            }
        };

        $scope.roleTable = new NgTableParams(
            {
                page: 1,
                count: 10,
                sorting: $.parseJSON("{ \"CntId\": \"asc\" }")
            }, {
                counts: [],
                getData: function (params) {
                    var param = {
                        SitId: $scope.entity.sitId
                    };
                    return entityService.listRole(param).then(
                        function (output) {
                           
                            params.total(output.data.count);
                            return output.data.data;
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        }
                    );
                }
            });

        $scope.siteListTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")
        }, {
            getData: function (params) {
            var listParams = {
                UserId: $scope.authentication.userId,
                UserWorkTypeId : $scope.$parent.userWorkTypeId,
                SiteId: $scope.selectedSite.siteId,
                ModuleId: $scope.page.moduleId,
                PageIndex: params.page(),
                PageSize: params.count(),
                Sort: JSON.stringify(params.sorting()),
                Filter: JSON.stringify($scope.criteria)
            };
            return entityService.list(listParams).then(
                function (output) {
                    $scope.validateUser(output);
                    $scope.items = output.data.data;
                    params.total(output.data.count);
                    return output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }
    });
        //#endregion
        
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("siteController", controller);

});
