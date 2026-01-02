"use strict";

var baseController = function ($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

    //#region Properties

    $scope.viewList = true;
    $scope.editMode = false;
    $scope.isTab0 = true;
    $scope.disabledInsert = true;
    $scope.disabledUpdate = true;
    $scope.requiredInsert = false;
    $scope.requiredUpdate = false;
    $scope.entity = {};
    $scope.form = {};
    $scope.currentTab = $scope.tabs[0];
    $scope.criteria = ($scope.criteria) ? $scope.criteria : [];
    $scope.entityId = 0;
    $scope.routeEntityId = $routeParams.id;
    $scope.items = [];
    $scope.hasError = false;
    $scope.regexEmail = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    //#endregion

    //#region Private Methods

    var dataChanged = false;
    var lastAction = "";
    var logs = [];
    var logCriteria = [];
    
    var save = function (action) {

        if ($scope.beforeSave != undefined) {
            $scope.beforeSave(action, lastAction);
            if ($scope.hasError){                
                initControls(lastAction);
                return;
            }
        }

        if (lastAction === "add") {
            entityService.insert($scope.entity).then(
                function (output) {
                    if (output.data.resultId == 1001) {
                        dataChanged = true;
                        $scope.entity = output.data.data;
                        $scope.entityId = $scope.entity[Utility.lowerFirstChar($scope.page.keyField)];
                        ngNotifier.show(output.data);
                        lastAction = "";
                        if ($scope.afterSave != undefined) {
                            $scope.afterSave(lastAction);
                        }
                    }
                    else {
                        ngNotifier.showError($scope.authentication, output);
                        initControls(lastAction);
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                    initControls(lastAction);
                });
        }
        else if (lastAction === "edit" && $scope.entityId > 0) {
            entityService.update($scope.entity, $scope.entityId).then(
                function (output) {
                    if (output.data.resultId == 1001) {
                        dataChanged = true;
                        $scope.entity = output.data.data;
                        ngNotifier.show(output.data);
                        lastAction = "";
                        if ($scope.afterSave != undefined) {
                            $scope.afterSave(lastAction);
                        }
                    }
                    else {
                        ngNotifier.showError($scope.authentication, output);
                        initControls(lastAction);
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                    initControls(lastAction);
                });
        }
    };

    var remove = function () {

        if ($scope.entityId > 0) {
            ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                entityService.delete($scope.entity).then(
                    function (output) {
                        dataChanged = true;
                        ngNotifier.show(output.data);
                        $scope.goBack();
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });
            }, function (output) {
                ngNotifier.showError($scope.authentication, output);
            });
        }
    };

    var removeBatch = function () {
        var entities = [];
        $scope.items.forEach(function (item) {
            if (item.selected) {
                entities.push(item);
            }
        });
        if (entities.length === 0) {
            ngNotifier.info("Please, select atleast one record to perform action.");
        } else {
            ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                entities.forEach(function (entity) {
                    entityService.delete(entity).then(
                        function (output) {
                            dataChanged = true;
                            ngNotifier.show(output.data);
                            $scope.goBack();
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        });
                });
            });
        }
    };

    var switchTab = function (title, action) {

        $scope.tabs.forEach(function (tab) {
            tab.active = false;
            tab.disabled = ((action === "add" || action === "edit") && tab !== $scope.tabs[0]);
            if (tab.title === title) {
                tab.active = true;
            }
        });
    };

    var initControls = function (action) {

        switch (action) {
            case "add":
                $scope.editMode = true;
                $scope.disabledInsert = false;
                $scope.disabledUpdate = false;
                $scope.requiredInsert = true;
                $scope.requiredUpdate = true;
                break;
            case "edit":
                $scope.editMode = true;
                $scope.disabledInsert = true;
                $scope.disabledUpdate = false;
                $scope.requiredInsert = false;
                $scope.requiredUpdate = true;
                break;
            default:
                $scope.editMode = false;
                $scope.disabledInsert = true;
                $scope.disabledUpdate = true;
                $scope.requiredInsert = false;
                $scope.requiredUpdate = false;
                break;
        }
    };

    var viewDetail = function () {
        $scope.viewList = false;
        $scope.page.urls.container = "app/views/shared/container.html";
        $scope.entity = {};
    };

    var reloadList = function () {
        if ($.inArray($scope.page.moduleId, ["600100", "600200", "600300", "700400", "700500", "700600"]) != -1) {
            $scope.groupListTable.reload();
        }
        else {
            $scope.listTable.reload();
        }
    }

    var openFilter = function (action) {

        var modalInstance = $uibModal.open({
            animation: false,
            templateUrl: (action == "filterList" ? $scope.page.urls.filter : action == "filterLog" ? $scope.page.urls.logFilter : ""),
            controller: "filterController",
            resolve: {
                requestData: function () {
                    return {
                        title: (action == "filterList" ? $scope.page.title : action == "filterLog" ? $scope.page.title + " Log" : ""),
                        criteria: (action == "filterList" ? $scope.page.criteria : action == "filterLog" ? $scope.page.logCriteria : []),
                        currentCriteria: (action == "filterList" ? $scope.criteria : action == "filterLog" ? logCriteria : [])
                    };
                }
            }
        });

        modalInstance.result.then(
            function (output) {
                if (output.resultId == 1001) {
                    if (action == "filterList") {
                        $scope.criteria = output.criteria;
                        reloadList();
                    }
                    else if (action == "filterLog") {
                        logCriteria = output.criteria;
                        $scope.logTable.reload();
                    }
                }
            },
            function (output) {
                ngNotifier.logError(output);
            });
    };

    var filterList = function () {

        var _criteria = [];

        $scope.page.criteria.forEach(function (filter) {
            var fieldValue = $("input[name='" + filter.name + "F']").val();
            if (fieldValue && fieldValue != "") {
                filter.value = fieldValue;
                filter.operator = "contains";
                _criteria.push(filter);
            }
        });

        $scope.criteria = _criteria;
        reloadList();
    };

    //#endregion

    //#region List Methods

    $scope.validateUser = function (output) {
        if (output.data.resultId == 2005) {
            ngNotifier.showError($scope.authentication, output);
            $scope.logOut();
        }
    };

    $scope.requestLoad = function () {
        var overlay = $('<div class="overlay"><i class="fa fa-refresh fa-spin"></i></div>');
        if ($("#mainBoxWidget").find("div.overlay").length == 0) {
            $("#mainBoxWidget").append(overlay);
        }
    }

    $scope.requestCompleted = function () {
        $("#mainBoxWidget").find("div.overlay").remove();
    }

    $scope.listTable = new NgTableParams(
    {
        page: 1,
        count: 10,
        sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")
    }, {
        getData: function (params) {
            var listParams = {
                SiteId: $scope.selectedSite.siteId,
                UserId: $scope.authentication.userId,
                UserWorkTypeId: $scope.$parent.userWorkTypeId,
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

    $scope.logTable = new NgTableParams(
    {
        page: 1,
        count: 10,
        sorting: $.parseJSON("{ \"ActivityDate\": \"desc\" }")
    }, {
        getData: function (params) {
            var listParams = {
                ModuleId: $scope.page.moduleId,
                OtherId: $scope.entityId,
                PageIndex: params.page(),
                PageSize: params.count(),
                Sort: JSON.stringify(params.sorting()),
                Filter: JSON.stringify(logCriteria)
            };
            return entityService.log(listParams).then(
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

    $scope.selectAll = function () {

        var selectedAll = !$scope.getSelectedAll();
        $scope.items.forEach(function (item) {
            item.selected = selectedAll;
        });
    };

    $scope.getSelectedAll = function () {

        var selected = 0;
        if ($scope.items) {
            $scope.items.forEach(function (item) {
                selected += (item.selected) || 0;
            });
            return (selected === $scope.items.length);
        }
        return false;
    };

    $scope.actionList = function (source) {

        var action = source.currentTarget.attributes["action"].value;

        switch (action) {

            case "filterList":
            case "filterLog":
                openFilter(action);
                break;

            case "refreshList":
                $scope.criteria = [];
                reloadList();
                break;

            case "refreshLog":
                logCriteria = [];
                $scope.logTable.reload();
                break;
        }
    }

    //#endregion

    //#region Detail Methods

    $scope.onClickTab = function (tab) {

        if (tab.disabled) {
            return;
        }

        $.each($scope.tabs, function (index, tab) { tab.active = false; });

        $scope.currentTab = tab;
        $scope.currentTab.active = true;
        $scope.isTab0 = ($scope.tabs[0] == tab);

        if ($scope.afterClickTab != undefined) {
            $scope.afterClickTab(tab);
        }
    }

    $scope.goBack = function () {

        $scope.entityId = 0;
        $scope.viewList = true;
        $scope.page.urls.container = "";

        if (dataChanged) {
            reloadList();
        }

        if ($scope.afterGoBack != undefined) {
            $scope.afterGoBack();
        }
    };

    $scope.showDetail = function (action, id) {

        $scope.onClickTab($scope.tabs[0]);

        $scope.entityId = id;

        viewDetail();

        initControls(action);

        if ($scope.entityId > 0) {
            $scope.getDetail();
        }
    };

    $scope.getDetail = function () {

        if ($scope.entityId > 0) {
            entityService.detail($scope.entityId).then(
                function (output) {
                    //$timeout(function () {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        $scope.entity = output.data.data;
                        if ($scope.afterGetDetail != undefined) {
                            $scope.afterGetDetail();
                        }
                    //}, 100);
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        } else {
            $scope.goBack();
        }
    };

    $scope.performAction = function (source, fromList) {

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

        if (action == "save" && $scope.validateAction != undefined) {
            if (!$scope.validateAction(source)) {
                return;
            }
        }

        if (fromList) {
            $scope.showDetail(action, source.currentTarget.attributes["entityId"].value);
        } else {
            initControls(action);
        }

        switchTab("Detail", action);

        switch (action) {
            case "search":
                filterList();
                break;
            case "add":
                lastAction = action;
                $scope.entityId = 0;
                $scope.entity = {};
                $("input[input-date]").each(function (index, element) { $(element).val(null); });
                break;
            case "edit":
                lastAction = action;
                break;
            case "save":
                save(action);
                break;
            case "saveEmail":
                $scope.entity.isSendEmailNow = true;
                save(action);
                break;
            case "cancel":
                $scope.getDetail();
                lastAction = "";
                break;
            case "delete":
                remove();
                lastAction = "";
                break;
            case "deleteBatch":
                removeBatch();
                lastAction = "";
                break;
            case "verify":
            case "activate":
            case "deactivate":
                $scope.changeStatus(action);
                lastAction = "";
                break;
            default:
                lastAction = "";
                break;
        }

        if ($scope.afterPerformAction != undefined) {
            $scope.afterPerformAction(source, fromList);
        }
    };

    $scope.changeStatus = function (action) {
        entityService.changeStatus($scope.entity, action).then(
            function (output) {
                dataChanged = true;
                $scope.entity = output.data.data;
                ngNotifier.show(output.data);
                if ($scope.afterChangeStatus != undefined) {
                    $scope.afterChangeStatus(action);
                }
            },
            function (output) {
                ngNotifier.showError($scope.authentication, output);
            });
        lastAction = "";
    };

    $scope.callTypeahead = function (viewValue, lookupModule, lookupField1, lookupMethod, fieldType,lookupField2, valueType) {
       
        var criteria = [];

        if ($scope.setLookupCriteria != undefined) {
            criteria = $scope.setLookupCriteria(lookupModule);
        }

        fieldType = (fieldType || "string");

        if (valueType != null) {
            criteria.push(Utility.createFilter(lookupField1, fieldType, lookupField1, viewValue, "startWith"));
            criteria.push(Utility.createFilter(lookupField2, fieldType, lookupField2, valueType, "equalTo"));
        } else {
            criteria.push(Utility.createFilter(lookupField1, fieldType, lookupField1, viewValue, "startWith", null));
        }
        

        var listParams = {
            SiteId: $scope.selectedSite.siteId,
            CwtId: $scope.userWorkTypeId,
            ModuleId: $scope.page.moduleId,
            PageIndex: 1,
            PageSize: 15,
            Sort: "{ \"" + lookupField1 + "\": \"asc\" }",
            Filter: JSON.stringify(criteria)
        };

        return entityService.lookup(lookupModule, lookupMethod, listParams).then(
            function (output) {

                if ($scope.isInvalidData != undefined) {
                    if (output.data.data.length == 0) { $scope.isInvalidData = true; }
                    else { $scope.isInvalidData = false; }
                } 
                return output.data.data;
            },
            function (output) {
                ngNotifier.showError($scope.authentication, output);
            }
        );
    };

    $scope.selectTypeahead = function ($item, $model, $label, source) {

        var lookupModule = null;
        var lookupIndex = null;
        var target = $(source.currentTarget);

        if (source.type == "click") {
            target = $(source.currentTarget).parent().parent().find("input");
        }

        var lookupModule = target.attr("lookup-module");
        var lookupIndex = target.attr("lookup-index");

        var output = { data: [] };
        output.data.push($item);

        if ($scope.setLookups != undefined) {
            $scope.setLookups(source, lookupModule, output, lookupIndex);
        }
    };

    $scope.validateTypeahead = function (source, lookup, index) {

        var lookupModule = source.currentTarget.attributes["lookup-module"].value;

        var lookupField = source.currentTarget.attributes["lookup-field"].value;
        if (source.currentTarget.attributes["entity-field"]) {
            lookupField = source.currentTarget.attributes["entity-field"].value;
        }

        var lookupKeyField = lookupField;
        if (source.currentTarget.attributes["entity-key-field"]) {
            lookupKeyField = source.currentTarget.attributes["entity-key-field"].value;
        }

        var lookupIndex = null;
        if (source.currentTarget.attributes["lookup-index"]) {
            lookupIndex = source.currentTarget.attributes["lookup-index"].value;
        }

        if (source.currentTarget.attributes["custom-clear"]) {
            if ($scope.customClearLookups != undefined) {
                $scope.customClearLookups(source, lookupModule, lookupIndex, lookupField);
            }
        }
        else {
            if ($scope.entity[lookupKeyField] == null || $scope.entity[lookupField] == null || $scope.entity[lookupField] == "") {
                $scope.clearLookups(source, lookupModule, lookupIndex);
            }
        }
    };

    $scope.callLookup = function (source) {

        var lookupModule = source.currentTarget.attributes["lookup-module"].value;
        var lookupField = source.currentTarget.attributes["lookup-field"].value;

        var lookupKeyField = lookupField;
        if (source.currentTarget.attributes["lookup-key-field"]) {
            lookupKeyField = source.currentTarget.attributes["lookup-key-field"].value;
        }

        var lookupMethod = null;
        if (source.currentTarget.attributes["lookup-method"]) {
            lookupMethod = source.currentTarget.attributes["lookup-method"].value;
        }

        var lookupIndex = null;
        if (source.currentTarget.attributes["lookup-index"]) {
            lookupIndex = source.currentTarget.attributes["lookup-index"].value;
        }

        var isMultiSelect = false;
        if (source.currentTarget.attributes["is-multi"]) {
            isMultiSelect = (source.currentTarget.attributes["is-multi"].value == "true");
        }

        var selectedItems = [];
        if (source.currentTarget.attributes["selected-items"]) {
            selectedItems = eval(source.currentTarget.attributes["selected-items"].value);
        }

        var lookupUrl = appUrl[lookupModule].urls.lookup;
        if (lookupMethod) {
            lookupUrl = lookupUrl.replace("lookup", lookupMethod);
        }

        var lookupCriteria = "[]";
        if ($scope.setLookupCriteria != undefined) {
            lookupCriteria = JSON.stringify($scope.setLookupCriteria(lookupModule));
        }

        var modalInstance = $uibModal.open({
            animation: false,
            templateUrl: lookupUrl,
            controller: "lookupController",
            resolve: {
                requestData: function () {
                    return {
                        title: appUrl[lookupModule].title,
                        siteId: $scope.selectedSite.siteId,
                        cwtId: $scope.userWorkTypeId,
                        userId: $scope.authentication.userId,
                        lookup: lookupModule,
                        lookupField: lookupField,
                        lookupKeyField: lookupKeyField,
                        lookupMethod: lookupMethod,
                        lookupSortField: appUrl[lookupModule].sortField,
                        lookupSortType: appUrl[lookupModule].sortType,
                        filter: appUrl[lookupModule].urls.filter,
                        criteria: appUrl[lookupModule].criteria,
                        lookupCriteria: lookupCriteria,
                        fetch: entityService.lookup,
                        isMultiSelect: isMultiSelect,
                        selectedItems: selectedItems
                    };
                }
            }
        });

        modalInstance.result.then(
            function (output) {
                $scope.setLookups(source, lookupModule, output, lookupIndex);
            },
            function (output) {
                $scope.clearLookups(source, lookupModule, lookupIndex);
                ngNotifier.logError(output);
            });
    }

    $scope.callLookupTags = function (query, lookup, lookupField, lookupMethod, index) {

        if (query == "") return;

        var fieldName = lookupField;
        appUrl[lookup].criteria.some(function (item) {
            if (Utility.lowerFirstChar(item.name) == lookupField) {
                fieldName = item.fieldName;
                return true;
            }
        });
        var criteria = JSON.stringify([{
            name: lookupField,
            type: "string",
            fieldName: fieldName,
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
        if ($scope.routeEntityId) {
            if ($scope.routeEntityId == 0) {
                $scope.$broadcast("show-errors-check-validity");
                $scope.showDetail("add", 0);
                switchTab("Detail", "add");
                lastAction = "add";
                $scope.entityId = 0;
                $scope.entity = {};
                $("input[input-date]").each(function (index, element) { $(element).val(null); });
                if ($scope.defaultBehaviorOnAdd != undefined) {
                    $timeout(function () {
                        $scope.defaultBehaviorOnAdd();
                    }, 1000);
                }
            }
            else {
                $scope.showDetail('viewDetail', $scope.routeEntityId);
            }
        }
    })();
    
    var skipFixedHeaderPages = ['HBL', 'MBL', 'CO','Dock Receipt'];
    // Code added for fixed header   
    $timeout(function () {
        if ($('table').length && skipFixedHeaderPages.indexOf($scope.pageTitle) == -1) {
            $('table').tableHeadFixer();
            var _parent = $('table').parent();
            $(_parent[0]).addClass('fixed-table-container-400');
            var table = $(_parent[0]).children()[1];
            $(table).insertBefore($(_parent[0]).children()[0]);
            //$(table).remove();
            //$(_parent[0]).prepend(table);
        }
    },500);
    //#endregion

};
