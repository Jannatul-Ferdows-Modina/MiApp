"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "NgTableParams", "ngNotifier", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, NgTableParams, ngNotifier, requestData) {

        //#region Private Methods

        var criteria = requestData.lookupCriteria;
        var items = [];
        var selectedItems = [];

        var openFilter = function (action) {

            var modalInstance = $uibModal.open({
                animation: false,
                templateUrl: requestData.filter,
                controller: "filterController",
                resolve: {
                    requestData: function () {
                        return {
                            title: $scope.lookupTitle,
                            criteria: requestData.criteria,
                            currentCriteria: JSON.parse(criteria)
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    criteria = JSON.stringify(output.criteria);
                    $scope.lookupTable.reload();
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        var getSelectedItems = function () {

            items.forEach(function (o) {
                var k = Utility.inArray(selectedItems, requestData.lookupKeyField, o[requestData.lookupKeyField]);
                if (o.selected && k == -1) {
                    selectedItems.push(o);
                }
                else if (!o.selected && k != -1) {
                    selectedItems.splice(k, 1);
                }
            });

            return selectedItems;
        };

        //#endregion

        //#region Lookup Methods

        $scope.responseData = {
            data: null,
            resultId: 2001
        };

        $scope.isMultiSelect = requestData.isMultiSelect;

        $scope.performAction = function (source, id) {

            var action = source.currentTarget.attributes["action"].value;

            switch (action) {

                case "select":
                    if ($scope.isMultiSelect) {
                        $scope.responseData.data = getSelectedItems();
                    }
                    else {
                        $scope.responseData.data = $.extend(true, {}, $.grep(items, function (obj) { return obj[requestData.lookupField] == id; }));
                    }
                    $scope.responseData.resultId = 1001;
                    $uibModalInstance.close($scope.responseData);
                    break;

                case "multiSelect":
                    $scope.responseData.data = getSelectedItems();
                    $scope.responseData.resultId = 1001;
                    $uibModalInstance.close($scope.responseData);
                    break;

                case "clear":
                    $scope.responseData.data = [];
                    $scope.responseData.resultId = 1001;
                    $uibModalInstance.close($scope.responseData);
                    break;

                case "close":
                    $uibModalInstance.close($scope.responseData);
                    break;

                case "filter":
                    openFilter(action);
                    break;

                case "refresh":
                    criteria = "[]";
                    $scope.lookupTable.reload();
                    break;
            }
        };

        $scope.lookupTable = new NgTableParams(
        {
            page: 1,
            count: 5,
            sorting: $.parseJSON("{ \"" + requestData.lookupSortField + "\": \"" + requestData.lookupSortType + "\" }")
        }, {
            getData: function (params) {
                var listParams = {
                    SiteId: requestData.siteId,
                    CwtId: requestData.cwtId,
                    UserId: requestData.userId,
                    OtherId: requestData.otherId,
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),
                    Filter: criteria
                };
                return requestData.fetch(requestData.lookup, requestData.lookupMethod, listParams).then(
                    function (output) {
                        if ($scope.isMultiSelect) {
                            getSelectedItems();
                            items = $.map(output.data.data, function (o) {
                                var k = Utility.inArray(selectedItems, requestData.lookupKeyField, o[requestData.lookupKeyField]);
                                if (k != -1) {
                                    o.selected = true;
                                }
                                return o;
                            });
                        }
                        else {
                            items = output.data.data;
                        }
                        params.total(output.data.count);
                        return output.data.data;
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });

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

        (function () {
            if ($scope.isMultiSelect) {
                selectedItems = requestData.selectedItems;
            }
        })();

        //#endregion
    };

    controller.$inject = injectParams;

    app.register.controller("lookupController", controller);

});
