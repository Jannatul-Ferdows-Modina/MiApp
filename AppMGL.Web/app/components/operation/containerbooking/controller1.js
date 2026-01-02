"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "containerbookingService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.containerbooking;
        $scope.tabs = appUrl.containerbooking.tabs;
        $scope.$parent.pageTitle = "Create Booking";
        $scope.$parent.breadcrumbs = ["Shipment", " Consolidate -Create Booking"];
        $scope.bookedId;
        var lastAction = "";
        $scope.containerList = [];
        
        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":
                    $scope.PendingStuffingList();
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
        $scope.PendingStuffingList = function () {
            $scope.pendingStuffingListTable = new NgTableParams(
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

                    //entityService.getBookedContainerList(listParams).then(
                        entityService.getPendingStuffingList(listParams).then(
                        function (output) {
                            $scope.items = output.data.data;
                            params.total(output.data.count);
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        }
                    );
                }
            })
        };
        $scope.containerBookedlistTable = new NgTableParams(
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

                entityService.getBookedContainerList(listParams).then(
                    function (output) {
                        $scope.itemss = output.data.data;
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
            $scope.containerBookedlistTable.reload();
        };

        $scope.saveBookedContainer = function () {
            $scope.$broadcast("show-errors-check-validity");

            if ($scope.entity.bookedNo == null) {
                ngNotifier.error("Please enter booking No");
                return;
            }
            
            if ($scope.entity.bookingDescription == null) {
                ngNotifier.error("Please enter description");
                return;
            }
           
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            entityService.saveBookedContainer($scope.entity).then(
                function (output) {
                    $scope.bookedId = output.data.data;
                    $scope.entity = {};
                    $scope.containerBookedlistTable.reload();
                    $scope.editMode = false;
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
        $scope.showBookedContainerDetail = function (action, id) {

            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entityId = id;
            $scope.entity.bookedId = parseFloat(id);
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            if ($scope.entityId > 0) {
                entityService.getBookedContainerDetail($scope.entity).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        
                        $scope.entity = output.data.data;
                        $scope.containerList = $scope.entity.containerDetail;
                        $scope.hideshow('StuffingDiv');
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

        $scope.deleteBookedContainer = function (id) {
            $scope.entityId = id;
            $scope.entity.bookedId = parseFloat(id);
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            if ($scope.entityId > 0) {
                entityService.deleteBookedContainer($scope.entity).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        $scope.containerBookedlistTable.reload();

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
        $scope.performBookedContainerAction = function (source, fromList) {

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
              $scope.showBookedContainerDetail(action, source.currentTarget.attributes["entityId"].value);
               
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
                    $scope.showBookedContainerDetail('viewDetail', $scope.entity.containerId);
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
        $scope.addContainer = function () {
            if ($scope.entity.containerId != "" && $scope.entity.containerId != undefined && $scope.entity.containerId != null) {

                var qids = $scope.entity.containerId.split(',');
                const index = qids.indexOf($scope.entity.contId);
                if (index > -1) {
                    alert("This container already exist.");
                    return false;
                }
            }
            $scope.containerList.push({
                containerNo: $scope.entity.containerNo, containerId: $scope.entity.contId
            });
            $scope.entity.containerId = ($scope.entity.containerId == "undefined" || $scope.entity.containerId == null) ? $scope.entity.contId : $scope.entity.containerId + ',' + $scope.entity.contId;
        };
        $scope.deletecontainer = function (rownum, containerId) {

            var qids = $scope.entity.containerId.split(',');
            const index = qids.indexOf(containerId);
            if (index > -1) {
                qids.splice(index, 1);
            }
            $scope.entity.containerId = qids.join(',');
            $scope.containerList.splice(rownum, 1);
        };
        $scope.searchValues = function (viewValue, selectType, searchRouteType) {
            var resultItem = {};
            var lookupModule;
            var lookupField = "name";
            if (lookupModule == "containerNo" || selectType == "containerNo") {
                lookupField = "containerNo";
                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
               
                return entityService.searchContainer(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}
                            resultItem.containerNo = o.name;
                            resultItem.containerId = o.value;
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }

        };
        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "containerNo") {

                $scope.entity.containerNo = output.data[0].containerNo;
                $scope.entity.contId = output.data[0].containerId;

            }


        };
        $scope.processBooking = function (id) {

        };
        $scope.ProcessQuotation = function () {
            $scope.items.forEach(function (item) {
                if (item.selected) {
                    $scope.containerList.push(item);
                    $scope.entity.containerId = ($scope.entity.containerId == "undefined" || $scope.entity.containerId == null) ? item.containerId : $scope.entity.containerId + ',' + item.containerId;
                }
            });
            $scope.entity.bookedId = "0";
            $scope.hideshow('StuffingDiv');
           
        };
        $scope.hideshow = function (name) {
            if (name == "StuffingDiv") {

                $("#StuffingDiv").show();
                $("#tblPending").hide();
                $("#Stuffing_tab").addClass('active');
                $("#Pending_tab").removeClass('active');
                $("#Pending_tab").addClass('disableli');
            }
            else {
                $("#StuffingDiv").hide();
                $("#tblPending").show();
                $("#Pending_tab").addClass('active');
                $("#Stuffing_tab").removeClass('active');
                $("#Stuffing_tab").addClass('disableli');
            }
        };
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("containerbookingController", controller);

});
