"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "containerstuffingService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, containerstuffingService) {

        //#region General
        $scope.page = appUrl.containerstuffing;
        $scope.tabs = appUrl.containerstuffing.tabs;
        $scope.$parent.pageTitle = "Container Stuffing";
        $scope.$parent.breadcrumbs = ["Shipment", " Consolidate -Booking Container Stuffing"];
       // $scope.bookedId;
        var lastAction = "";
        $scope.pendinglistitem;
       // $scope.lookups = {  containerList: []};
        $scope.containerList = [];
        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":
                    break;
                case "edit":
                    break;
            }
        };
        $scope.quotationId = "";
        $scope.quotationList = [];
        $scope.selectOption = "";
        $scope.searchBox = "";
        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox
        };
        $scope.stuffinglistTable = new NgTableParams(
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

                    containerstuffingService.getStuffingList(listParams).then(
                    function (output) {
                       // $scope.validateUser(output);
                        $scope.items = output.data.data;
                        params.total(output.data.count);
                        //document.getElementById('divAdd').hidden = "true";                        
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });
        $scope.performStuffingSearch = function (source, selectOption, searchBox) {
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox
            };
            $scope.stuffinglistTable.reload();
        };

        $scope.saveStuffing = function () {
            $scope.$broadcast("show-errors-check-validity");

            //if ($scope.entity.stuffingNo == null) {
            //    ngNotifier.error("Please enter stuffing No");
            //    return;
            //}
            //if ($scope.entity.stuffingDescription == null) {
            //    ngNotifier.error("Please enter stuffing description");
            //    return;
            //}
            //if ($scope.entity.quotationId == "" || $scope.entity.quotationId == null) {
            //    ngNotifier.error("Please add at least one quotation");
            //    return;
            //}
            if ($scope.quotationList.length == 0) {
                alert("Please add atleast one quotation.");
                return true;
            }
            $scope.entity.quotationDetail = $scope.quotationList;
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            containerstuffingService.saveStuffing($scope.entity).then(
                function (output) {
                    $scope.bookedId = output.data.data;
                    $scope.entity = {};
                    $scope.editMode = false;
                    $scope.stuffinglistTable.reload();
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
                case "copy":
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
                tab.disabled = ((action === "add" || action === "copy" || action === "edit") && tab !== $scope.tabs[0]);
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
        $scope.showStuffingDetail = function (action, id, isStuffed) {
            $scope.getBookedContainerList(id);
            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
           // $scope.bookedId = id;
            $scope.entityId = id;
            $scope.entity.bookedId = parseFloat(id);
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            //if (stuffingId != "" && stuffingId != null && stuffingId != undefined) {
            if ($scope.entityId != "") {
                containerstuffingService.getStuffingDetail($scope.entity).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        else {
                            $scope.entity = output.data.data;
                            $scope.quotationList = $scope.entity.quotationDetail;
                            // $scope.entity.containerNo = "-- Select --";
                            $scope.entity.containerNo = "";
                            $scope.entity.quotationDetail.forEach(function (item) {
                                $scope.quotationId = $scope.quotationId = '' ? item.quotationId : $scope.quotationId + ","+item.quotationId;
                            }); 
                            if (isStuffed == "0" || isStuffed == "") {
                                $scope.hideshow("");
                                
                            }
                            else
                                $scope.hideshow('StuffingDiv');
                            
                           
                        }
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
               
            }
            $scope.PendingContainerList();
        };
        $scope.deleteStuffing = function (id) {

           
            $scope.entityId = id;
            $scope.entity.bookedId = parseFloat(id);
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            if (id > 0) {
                containerstuffingService.deleteStuffing($scope.entity).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        if (output.data.resultId == 1001) {
                            $scope.goBack();
                            ngNotifier.show(output.data);
                        }
                        $scope.stuffinglistTable.reload();

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
        $scope.performStuffingAction = function (source, fromList) {

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
                $scope.showStuffingDetail(action, source.currentTarget.attributes["entityId"].value, source.currentTarget.attributes["isStuffed"].value);
               
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
                    $scope.showStuffingDetail('viewDetail', $scope.entity.bookedId);
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
        $scope.hideshow = function (name) {
            if (name == "StuffingDiv") {
                
                    $("#StuffingDiv").show();
                    $("#tblPending").hide();
                    $("#Stuffing_tab").addClass('active');
                    $("#Pending_tab").removeClass('active');
                    // $("#Pending_tab").addClass('disableli');
            }
            else {
                $("#StuffingDiv").hide();
                $("#tblPending").show();
                $("#Pending_tab").addClass('active');
                $("#Stuffing_tab").removeClass('active');
               // $("#Stuffing_tab").addClass('disableli');
                
               
                $("#containerNo").val("");
            }
        };
        $scope.PendingContainerList = function () {
           
            //containerstuffingService.pendingContainerList($scope.$parent.selectedSiteId).then(
            //    function (output) {
            //        $scope.pendinglistitem = output.data.data;
            //        $scope.hideshow("");
            //    },
            //    function (output) {
                   
            //    });
            containerstuffingService.pendingQuotationList($scope.$parent.selectedSiteId).then(
                function (output) {
                    $scope.pendinglistitem = output.data.data;
                   // $scope.hideshow("");
                },
                function (output) {

                });
        };
        //$scope.processContainer = function (id) {
        //    $scope.entity.siteId = $scope.$parent.selectedSiteId;
        //    $scope.entity.containerId = parseFloat(id);
        //    if (id > 0) {
        //        containerstuffingService.getContainerDetail($scope.entity).then(
        //            function (output) {
        //                if (output.data.resultId == 2005) {
        //                    ngNotifier.showError($scope.authentication, output);
        //                    $scope.logOut()
        //                }
        //                else {
        //                    $scope.entity = output.data.data;
        //                    //$scope.entity.stuffingId = "0";
        //                    $scope.hideshow('StuffingDiv');
        //                }
                        
        //            },
        //            function (output) {
        //                ngNotifier.showError($scope.authentication, output);
        //            }
        //        );

        //    }
        //    else {
        //        $scope.goBack();
        //    }
        //};
        $scope.ProcessQuotation = function () {

            var ContainerCount = 0;
            if ($("#containerNo").val() == "" || $("#containerNo").val() == "-- Select --")
            {
                alert("Please Select Container Number.");
                return true;
            }

            $scope.pendinglistitem.forEach(function (item) {
                if (item.selected) {
                    ContainerCount = ContainerCount + 1;
                    const exists = $scope.quotationList.some(i => i.quotationNo === item.quotationNo);
                    if (exists == false) {
                    item.containerNo = $scope.entity.containerNo;// $("#containerno").val();
                        $scope.quotationList.push(item);

                    $scope.quotationId = ($scope.quotationId == "undefined" || $scope.quotationId == null || $scope.quotationId == "") ? item.quotationID : $scope.quotationId + ',' + item.quotationID;
                    }
                }
            });
            if (ContainerCount == 0) {
                alert("Please Select Atleast one quotation.");
                return true;
            }
            //$scope.entity.stuffingId = "0";
            $scope.hideshow('StuffingDiv');
           
        };
        //$scope.searchValues = function (viewValue, selectType, searchRouteType) {
        //    var resultItem = {};
        //    var lookupModule;
        //    var lookupField = "name";
        //    if (lookupModule == "quotationNo" || selectType == "quotationNo") {
        //        lookupField = "quotationNo";
        //    }
        //    if (lookupModule == "containerNo" || selectType == "containerNo") {
        //        lookupField = "containerNo";
        //    }
        //    var listParams = {
        //        SiteId: $scope.selectedSite.siteId,
        //        CwtId: $scope.userWorkTypeId,
        //        ModuleId: $scope.page.moduleId,
        //        PageIndex: 1,
        //        PageSize: 25,
        //        Sort: "{ \"" + lookupField + "\": \"asc\" }",
        //        Filter: viewValue
        //    };
        //        if (lookupModule == "quotationNo" || selectType == "quotationNo") {
        //            return containerstuffingService.searchQuotation(listParams).then(
        //                function (output) {
        //                    $scope.searchResult = [];
        //                    output.data.data.forEach(function (o) {
        //                        resultItem = {}
        //                        resultItem.quotationNo = o.name;
        //                        resultItem.quotationId = o.value;
        //                        $scope.searchResult.push(resultItem)

        //                    });
        //                    return $scope.searchResult;

        //                }
        //            )
        //        };
        //        if (lookupModule == "containerNo" || selectType == "containerNo") {
        //            return containerstuffingService.searchContainer(listParams).then(
        //            function (output) {
        //                $scope.searchResult = [];
        //                output.data.data.forEach(function (o) {
        //                    resultItem = {}
        //                    resultItem.containerNo = o.containerNo;
        //                    resultItem.containerId = o.containerId;
        //                    resultItem.containerDescription = o.containerDescription;
        //                    $scope.searchResult.push(resultItem)

        //                });
        //                return $scope.searchResult;

        //            }
        //            )
        //        };
            
           
        //};
        //$scope.setLookups = function (source, lookup, output, index) {

        //    if (lookup == "quotationNo") {
              
        //        $scope.entity.quotationNo = output.data[0].quotationNo;
        //        $scope.entity.quotId = output.data[0].quotationId;

        //    }
        //    if (lookup == "containerNo") {

        //        $scope.entity.containerNo = output.data[0].containerNo;
        //        $scope.entity.containerId = output.data[0].containerId;
        //        $scope.entity.containerDescription = output.data[0].containerDescription;

        //    }
           

        //};
        //$scope.addQuotation = function ()
        //{
        //    if ($scope.entity.quotationId != "" && $scope.entity.quotationId != undefined && $scope.entity.quotationId != null) {
 
        //    var qids = $scope.entity.quotationId.split(',');
        //        const index = qids.indexOf($scope.entity.quotId);
        //        if (index > -1) {
        //            alert("This quotation already exist.");
        //            return false;
        //    }
        //    }
        //   $scope.quotationList.push({
        //       quotationNo: $scope.entity.quotationNo, quotationId: $scope.entity.quotId
        //   });
        //    $scope.entity.quotationId = ($scope.entity.quotationId == "undefined" || $scope.entity.quotationId == null) ? $scope.entity.quotId : $scope.entity.quotationId+ ','+$scope.entity.quotId;
        //};
        $scope.deletequotation = function (rownum, quotationId) {

            var qid = quotationId.toString();
            var qids = "";
            if ($scope.quotationId!="")
                 qids = $scope.quotationId.toString().split(',');
            const index = qids.indexOf(qid);
            if (index > -1) { 
                qids.splice(index, 1); 
            }
            $scope.quotationId=qids.join(',');
            $scope.quotationList.splice(rownum, 1);
        };
        $scope.getBookedContainerList = function (id) {
            containerstuffingService.getBookedContainerList(id).then(
                function (output) {
                    $scope.containerList = output.data.data;
                   // $scope.entity.containerNo = "-- Select --";
                   // $scope.entity.containerNo = "";
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }
        $scope.moveConsolidateBooking = function (BookedId) {
            containerstuffingService.MoveConsolidateBooking(BookedId).then(
                function (output) {
                  
                    $scope.stuffinglistTable.reload();
                    $scope.goBack();
                    ngNotifier.show(output.data);

                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
           
        };
   angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, containerstuffingService));

    };
    controller.$inject = injectParams;
    app.register.controller("containerstuffingController", controller);

});
