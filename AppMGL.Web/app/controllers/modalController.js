"use strict";

var modalController = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService) {

    //#region Private Methods

    $scope.regexEmail = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    var initPage = function () {
        if ($scope.entityId > 0) {
            entityService.detail($scope.entityId).then(
                function (output) {
                    $scope.entity = output.data.data;
                    $scope.isNew = false;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }
        if ($scope.enquiryEntityId != undefined) {
            if ($scope.enquiryEntityId > 0)
            {
                $scope.showEnquiryDetail($scope.enquiryEntityId);
            }
        }
    };

    var save = function () {

        if ($scope.beforeSave != undefined) {
            $scope.beforeSave("save", ($scope.isNew ? "add" : "edit"));
        }

        if ($scope.entityId == 0) {
            entityService.insert($scope.entity).then(
                function (output) {
                    if (output.data.resultId == 1001) {
                        $scope.entity = output.data.data;
                        $scope.responseData.data = output.data.data;
                        $scope.responseData.resultId = 1001;
                        $uibModalInstance.close($scope.responseData);
                    }
                    else {
                        ngNotifier.showError($scope.authentication, output);
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });
        }
        else if ($scope.entityId > 0) {
            entityService.update($scope.entity, $scope.entityId).then(
                function (output) {
                    if (output.data.resultId == 1001) {
                        $scope.entity = output.data.data;
                        $scope.responseData.data = output.data.data;
                        $scope.responseData.resultId = 1001;
                        $uibModalInstance.close($scope.responseData);
                    }
                    else {
                        ngNotifier.showError($scope.authentication, output);
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });
        }
    };

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

    $scope.entityId = 0;
    $scope.entity = {};
    $scope.requiredUpdate = true;
    $scope.disabledUpdate = false;
    $scope.isNew = true;

    $scope.responseData = {
        data: null,
        resultId: 2001
    };

    $scope.performAction = function (source) {

        var action = source.currentTarget.attributes["action"].value;

        $scope.$broadcast("show-errors-check-validity");

        if (action != "cancel" && $scope.form.detail != undefined && $scope.form.detail.$invalid) {
            if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
                ngNotifier.error("Required Field(s) are missing data.");
            }
            else if ($scope.form.detail.usrPwdC.$invalid) {
                ngNotifier.error("Password do not match with Confirm Password.");
            }
            return;
        }

        switch (action) {
            case "save":
                save(action);
                break;
            case "cancel":
                $uibModalInstance.close($scope.responseData);
                break;
        }

        if ($scope.afterPerformAction != undefined) {
            $scope.afterPerformAction(source);
        }
    };

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
            PageSize: 60000,
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

    (function () {
        if ($scope.initDropdown != undefined) {
            $scope.initDropdown();
        }
        initPage();
    })();

    //#endregion

};
