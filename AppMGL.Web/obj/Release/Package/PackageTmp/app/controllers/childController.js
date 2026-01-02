"use strict";

var childController = function ($scope, $timeout, $uibModal, localStorageService, ngNotifier, authService, entityService) {

    //#region Private Methods

    $scope.regexEmail = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    var openLookup = function (source, lookup, lookupField, lookupMethod, index, criteria) {

        var modalInstance = $uibModal.open({
            animation: false,
            templateUrl: appUrl[lookup].urls.lookup,
            controller: "lookupController",
            resolve: {
                requestData: function () {
                    return {
                        title: appUrl[lookup].title,
                        lookup: lookup,
                        lookupField: lookupField,
                        lookupMethod: lookupMethod,
                        lookupSortField: appUrl[lookup].sortField,
                        lookupSortType: appUrl[lookup].sortType,
                        filter: appUrl[lookup].urls.filter,
                        criteria: appUrl[lookup].criteria,
                        lookupCriteria: criteria,
                        otherId: $scope.otherId,
                        fetch: entityService.lookup
                    };
                }
            }
        });

        modalInstance.result.then(
            function (output) {
                $scope.setLookups(source, lookup, output, index);
            },
            function (output) {
                $scope.clearLookups(source, lookup, index);
                ngNotifier.logError(output);
            });
    }

    //#endregion

    //#region Detail Methods

    $scope.authentication = authService.authentication;

    $scope.callLookup = function (source, lookup, lookupField, lookupMethod, index) {

        openLookup(source, lookup, lookupField, lookupMethod, index, "[]");
    }

    $scope.callLookupTags = function (query, lookup, lookupField, lookupMethod, index) {

        if (query == "") return;

        var criteria = JSON.stringify([{
            name: lookupField,
            type: "string",
            fieldName: lookupField,
            value: query,
            operator: "contains",
            valueT: ""
        }]);

        var listParams = {
            PageIndex: 1,
            PageSize: 10,
            CwtId: $scope.userWorkTypeId,
            Sort: "{ \"" + lookupField + "\": \"asc\" }",
            Filter: criteria
        };

        var tagsData = entityService.lookup(lookup, lookupMethod, listParams).then(
            function (output) {
                output.data.data.forEach(function (object) {
                    object.isTag = true;
                });
                return output.data.data;
            },
            function (output) {
                ngNotifier.showError($scope.authentication, output);
            }
        );

        return tagsData;
    };

    $scope.validateTags = function ($tag) {

        if ($tag.isTag) {
            return true;
        }
        return false;
    };

    $scope.validateEmailTags = function ($tag) {

        if ($tag.isTag) {
            return true;
        }
        else if ($tag.emailAddress && $scope.regexEmail.test($tag.emailAddress)) {
            return true;
        }
        return false;
    };

    $scope.fetchLookupData = function (moduleName, otherId, sortField, lookupKey, lookupMethod) {
        var listParams = {
            OtherId: otherId,
            PageIndex: 1,
            PageSize: 300,
            CwtId: $scope.userWorkTypeId,
            Sort: "{\"" + sortField + "\":\"asc\"}",
            Filter: "[]"
        };
        if ($scope.beforeFetchLookupData != undefined) {
            listParams = $scope.beforeFetchLookupData(moduleName, otherId, sortField, lookupKey);
        }
        entityService.lookup(moduleName, lookupMethod, listParams).then(
            function (output) {
                $scope.lookups[lookupKey] = output.data.data;
                if ($scope.afterFetchLookupData != undefined) {
                    $scope.afterFetchLookupData(lookupKey);
                }
            },
            function (output) {
                ngNotifier.showError($scope.authentication, output);
            }
        );
    };

    $scope.fetchListData = function (moduleName, lookupKey, lookupMethod, listParams) {
        entityService.listData(moduleName, lookupMethod, listParams).then(
            function (output) {
                $scope.lookups[lookupKey] = output.data.data;
            },
            function (output) {
                ngNotifier.showError($scope.authentication, output);
            }
        );
    };

    //#endregion

};
