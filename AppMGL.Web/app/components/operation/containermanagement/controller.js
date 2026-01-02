"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "containermanagementService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, containerService) {

        //#region General
        $scope.page = appUrl.containermanagement;
        $scope.tabs = appUrl.containermanagement.tabs;
        $scope.$parent.pageTitle = "Container Management";
        $scope.$parent.breadcrumbs = ["Shipment", " Consolidate -Booking Container Management"];
        $scope.bookedId;
        var lastAction = "";
        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":
                    break;
                case "edit":
                    break;
            }
        };
        $scope.selectOption = "";
        $scope.searchBox = "";
        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox
        };
        $scope.containerlistTable = new NgTableParams(
            {
                page: 1,
                count: 10,
                sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")
            }, {
            getData: function (params) {
                var listParams = {
                    SiteId: $scope.$parent.selectedSiteId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),
                    Filter: JSON.stringify($scope.searchParam)
                     };

                    containerService.getContainerList(listParams).then(
                    function (output) {
                        $scope.items = output.data.data;
                        params.total(output.data.count);
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });
        $scope.performContainerSearch = function (source, selectOption, searchBox) {
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox
            };
            $scope.containerlistTable.reload();
        };

        $scope.saveContainer = function () {
            $scope.$broadcast("show-errors-check-validity");

            //if ($scope.entity.containerNo == null) {
            //    ngNotifier.error("Please enter Continer No");
            //    return;
            //}
            //if ($scope.entity.containerLineNo == null) {
            //    ngNotifier.error("Please enter Continer No");
            //    return;
            //}
            //if ($scope.entity.containerDescription == null) {
            //    ngNotifier.error("Please enter description");
            //    return;
            //}
           
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            containerService.saveContainer($scope.entity).then(
                function (output) {
                    $scope.containerId = output.data.data;
                    $scope.entity = {};
                    $scope.editMode = false;
                    $scope.containerlistTable.reload();
                    $scope.goBack();
                    ngNotifier.show(output.data);
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                    $scope.editMode = false;
                    $scope.disabledInsert = true;
                    $scope.disabledUpdate = true;
                    $scope.requiredInsert = false;
                    $scope.requiredUpdate = false;
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
        var switchTab = function (title, action) {

            $scope.tabs.forEach(function (tab) {
                tab.active = false;
                tab.disabled = ((action === "add" || action === "edit") && tab !== $scope.tabs[0]);
                if (tab.title === title) {
                    tab.active = true;
                }
            });
        };
        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
        };
        $scope.showContainerDetail = function (action, id) {

            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entityId = id;
            $scope.entity.bookedId = parseFloat(id);
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            if ($scope.entityId > 0) {
                containerService.getContainerDetail($scope.entity).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        $scope.entity = output.data.data;
                        $scope.dispatchContainerList = [];
                        if ($scope.entity.dispatchContainerList != null) {
                            $scope.dispatchContainerList = $scope.entity.dispatchContainerList;
                        }
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );

            }
            else {
                $scope.goBack();
            }
        };

        $scope.deleteContainer = function (id) {
            $scope.entityId = id;
            $scope.entity.bookedId = parseFloat(id);
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            if ($scope.entityId > 0) {
                containerService.deleteContainer($scope.entity).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        if (output.data.resultId == 1001) {
                            $scope.goBack();
                            ngNotifier.show(output.data);
                        }
                        $scope.containerlistTable.reload();

                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );

            }
            else {
                $scope.goBack();
            }
        };
        $scope.performContainerAction = function (source, fromList) {

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
              $scope.showContainerDetail(action, source.currentTarget.attributes["entityId"].value);
               
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
                case "cancel":
                    $scope.showContainerDetail('viewDetail', $scope.entity.containerId);
                    lastAction = "";
                    break;
                case "delete":
                    remove();
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

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, containerService));

    };

    controller.$inject = injectParams;

    app.register.controller("containermanagementController", controller);

});
