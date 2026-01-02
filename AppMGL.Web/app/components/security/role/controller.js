"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "roleService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.role;
        $scope.tabs = appUrl.role.tabs;

        //#endregion

        //#region Lookup

        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "module") {
                $scope.roleMap.modId = output.data[0].modId;
                $scope.roleMap.modCaption = output.data[0].modCaption;
            }
            else if (lookup == "action") {
                $scope.roleMap.actId = output.data[0].actId;
                $scope.roleMap.actCaption = output.data[0].actCaption;
            }
        };

        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "module") {
                $scope.roleMap.modId = null;
                $scope.roleMap.modCaption = "";
            }
            else if (lookup == "action") {
                $scope.roleMap.actId = null;
                $scope.roleMap.actCaption = "";
            }
        };

        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {

            if (lookupModule == "module" || lookupModule == "action") {
                if ($scope.roleMap[lookupField] == null || $scope.roleMap[lookupField] == "") {
                    $scope.clearLookups(source, lookupModule, lookupIndex);
                }
            }
        };

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.rleCreatedTs = new Date();
                $scope.entity.rleCreatedBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.rleUpdatedTs = new Date();
                $scope.entity.rleUpdatedBy = $scope.$parent.userInfo.usrId;
            }
        };

        //#endregion

        //#region Mapping

        var lastActionC = "";

        var fetchAction = function (id) {
            if (id) {
                $scope.roleMap = $.extend(true, {}, $.grep($scope.roleMapTable.data, function (obj) { return obj.mrmId == id; })[0]);
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
                $scope.roleMap.rleId = $scope.entity.rleId;
                $scope.roleMap.mrmCreatedTs = new Date();
                $scope.roleMap.mrmCreatedBy = $scope.$parent.userInfo.usrId;
                entityService.insertRoleMap($scope.roleMap).then(
                    function (output) {
                        $scope.roleMap = {};
                        $scope.roleMapTable.reload();
                        ngNotifier.show(output.data);
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });
            } else if (lastActionC === "editC" && $scope.roleMap.rleId > 0) {
                $scope.roleMap.mrmUpdatedTs = new Date();
                $scope.roleMap.mrmUpdatedBy = $scope.$parent.userInfo.usrId;
                entityService.updateRoleMap($scope.roleMap, $scope.roleMap.rleId).then(
                    function (output) {
                        $scope.roleMap = {};
                        $scope.roleMapTable.reload();
                        ngNotifier.show(output.data);
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });
            }
        };

        var remove = function (id) {

            if (id == null) {
                id = $scope.roleMap.rleId;
            }
            if (id > 0) {
                fetchAction(id);
                ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                    entityService.deleteRoleMap($scope.roleMap).then(
                        function (output) {
                            $scope.roleMap = {};
                            $scope.roleMapTable.reload();
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
        $scope.roleMap = {};

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
                    $scope.roleMap = {};
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
                    $scope.roleMap = {};
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

        $scope.roleMapTable = new NgTableParams(
            {
                page: 1,
                count: 10,
                sorting: $.parseJSON("{ \"ModId\": \"asc\" }")
            }, {
                counts: [],
                getData: function (params) {
                    var param = {
                        RleId: $scope.entity.rleId
                    };
                    return entityService.listRoleMap(param).then(
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

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("roleController", controller);

});
