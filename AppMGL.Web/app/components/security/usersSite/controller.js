"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "authService", "usersSiteService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService, requestData) {

        //#region Private Methods

        var items = [];

        var save = function () {

            $scope.$broadcast("show-errors-check-validity");

            if ($scope.form.subdetail != undefined && $scope.form.subdetail.$invalid) {
                if ($scope.form.subdetail.$error.required != undefined && $scope.form.subdetail.$error.required.length > 0) {
                    ngNotifier.error("Required Field(s) are missing data.");
                }
                return;
            }

            var entities = [];
            items.forEach(function (item) {
                if (item.selected) {
                    item.userId = requestData.userId;
                    item.siteId = $scope.roles.siteId;
                    item.isDefault = (requestData.siteCount == 0) ? true : $scope.roles.isDefault;
                    entities.push(item);
                }
            });

            if (entities.length == 0) {
                ngNotifier.error("Please, select atleast one record to perform action.");
            }
            else {
                if (requestData.action == "addC") {
                    entityService.batchInsert(entities).then(
                        function (output) {
                            $scope.responseData.resultId = output.data.resultId;
                            $scope.responseData.data = output.data;
                            $uibModalInstance.close($scope.responseData);
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        });
                }
                else {
                    entityService.batchUpdate(entities).then(
                    function (output) {
                        $scope.responseData.resultId = output.data.resultId;
                        $scope.responseData.data = output.data;
                        $uibModalInstance.close($scope.responseData);
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });
                }
            }
        };

        //#endregion

        //#region Lookup Methods

        $scope.page = { moduleId: "300700" };
        $scope.roles = {};
        $scope.editMode = (requestData.action == "addC" || requestData.action == "editC");
        $scope.isSelected = false;
        $scope.disabledInput = !(requestData.action == "addC");
        $scope.requiredInput = (requestData.action == "addC");
        $scope.otherId = requestData.userId;

        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "site") {
                $scope.roles.siteId = output.data[0].siteId;
                $scope.roles.siteCode = output.data[0].siteCode;
                $scope.roles.siteName = output.data[0].siteName;
                $scope.groupsTable.reload();
                $scope.isSelected = true;
            }
        };

        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "site") {
                $scope.roles.siteId = "";
                $scope.roles.siteCode = "";
                $scope.roles.siteName = "";
                $scope.isSelected = false;
                items = [];
            }
        };

        $scope.groupsTable = new NgTableParams(
        {
            page: 1,
            count: 1000,
            sorting: $.parseJSON("{ \"ActionId\": \"asc\" }")
        }, {
            counts: [],
            getData: function (params) {
                if (requestData.siteId && $scope.roles.siteId == null) {
                    $scope.roles.siteId = requestData.siteId;
                    $scope.roles.siteCode = requestData.siteInfo.siteCode;
                    $scope.roles.siteName = requestData.siteInfo.siteName;
                    $scope.roles.isDefault = requestData.siteInfo.isDefault;
                }
                if ($scope.roles.siteId) {
                    var listParams = {
                        UserId: requestData.userId,
                        SiteId: $scope.roles.siteId,
                        Sort: JSON.stringify(params.sorting()),
                        Filter: "[]"
                    };
                    return entityService.listGroup(listParams).then(
                        function (output) {
                            items = output.data.data;
                            params.total(output.data.count);
                            $timeout(function () {
                                $scope.isSelected = true;
                            }, 100);
                            return output.data.data;
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        });
                }
            }
        });

        $scope.responseData = {
            data: null,
            resultId: 2001
        };

        $scope.performAction = function (source, id) {

            var action = source.currentTarget.attributes["action"].value;

            if (action == "cancel" && requestData.action == "addC") {
                action = "close";
            }

            switch (action) {

                case "edit":
                    $scope.editMode = true;
                    break;

                case "save":
                    save();
                    break;

                case "cancel":
                    $scope.editMode = false;
                    break;

                case "close":
                    $uibModalInstance.close($scope.responseData);
                    break;
            }
        };

        $scope.selectAll = function () {

            var selectedAll = !$scope.getSelectedAll();
            items.forEach(function (item) {
                item.selected = selectedAll;
            });
        };

        $scope.getSelectedAll = function () {

            var selected = 0;
            items.forEach(function (item) {
                selected += (item.selected) || 0;
            });
            return (selected === items.length);
        };

        //#endregion

        angular.extend(this, new childController($scope, $timeout, $uibModal, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("usersSiteController", controller);

});
