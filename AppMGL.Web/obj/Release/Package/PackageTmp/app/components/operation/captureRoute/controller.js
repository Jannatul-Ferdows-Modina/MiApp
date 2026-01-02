// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "captureRouteService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.captureRoute;
        $scope.tabs = appUrl.captureRoute.tabs;

        $scope.$parent.pageTitle = "Route Capture";
        $scope.$parent.breadcrumbs = ["Contract", "Route Capture"];

        $scope.dispatchContainerList = [];
        $scope.actionRemarksList = [];
        $scope.isInvalidData = false;
        $scope.lookups = { siplDepartments: [] };
        $scope.searchResult = [];
        $scope.isRouteTypeDisabled = false;

        $scope.initDropdown = function () {
            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
        };
       
        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "origin") {
                if ($scope.entity.originType == 'City') {
                    $scope.entity.fkOriginID = output.data[0].cityId;
                    $scope.entity.origin = output.data[0].name;
                }
                if ($scope.entity.originType == 'Port') {
                    $scope.entity.fkOriginID = output.data[0].portId;
                    $scope.entity.origin = output.data[0].name;
                }                
                if ($scope.entity.originType == 'railRamp') {
                    $scope.entity.fkOriginID = output.data[0].railId;
                    $scope.entity.origin = output.data[0].name;
                }
            }

            if (lookup == "destination") {
                if ($scope.entity.destinationType == "City") {
                    $scope.entity.fkDestinationID = output.data[0].cityId;
                    $scope.entity.destination = output.data[0].name;
                }
                if ($scope.entity.destinationType == "Port") {
                    $scope.entity.fkDestinationID = output.data[0].portId;
                    $scope.entity.destination = output.data[0].name;
                }
                if ($scope.entity.destinationType == "railRamp") {
                    $scope.entity.fkDestinationID = output.data[0].railId;
                    $scope.entity.destination = output.data[0].name;
                }
            }
            if (lookup == "viaType1") {
                if ($scope.entity.viaType1 == "City") {
                    $scope.entity.via1 = output.data[0].cityId;
                    $scope.entity.via1Name = output.data[0].name;
                }
                if ($scope.entity.viaType1 == "Port") {
                    $scope.entity.via1 = output.data[0].portId;
                    $scope.entity.via1Name = output.data[0].name;
                }
                if ($scope.entity.viaType1 == "railRamp") {
                    $scope.entity.via1 = output.data[0].railId;
                    $scope.entity.via1Name = output.data[0].name;
                }
            }
            if (lookup == "viaType2") {
                if ($scope.entity.viaType2 == "City") {
                    $scope.entity.via2 = output.data[0].cityId;
                    $scope.entity.via2Name = output.data[0].name;
                }
                if ($scope.entity.viaType2 == "Port") {
                    $scope.entity.via2 = output.data[0].portId;
                    $scope.entity.via2Name = output.data[0].name;
                }
                if ($scope.entity.viaType2 == "railRamp") {
                    $scope.entity.via2 = output.data[0].railId;
                    $scope.entity.via2Name = output.data[0].name;
                }
            }
            
        };

        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "origin") {
                $scope.entity.fkOriginID = null;
                $scope.entity.Origin = "";
            }
            if (lookup == "destination") {
                $scope.entity.fkDestinationID = null;
                $scope.entity.destination = "";
            }
            if (lookup == "viaType1") {
                $scope.entity.via2 = null;
                $scope.entity.viaType2 = "";
            }
            if (lookup == "viaType2") {
                $scope.entity.via2 = null;
                $scope.entity.viaType2 = "";
            }

        };

        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {

            //if (lookupModule == "SIPLContact" || lookupModule == "originDoorName" || lookupModule == "destinationDoorName" || lookupModule == "BillToCompany" || lookupModule == "originCountry" || lookupModule == "dischargeCountry" || lookupModule == "originState" || lookupModule == "dischargeState" || lookupModule == "originCity" || lookupModule == "dischargeCity" || lookupModule == "originPort" || lookupModule == "dischargePort" || lookupModule == "RailRamp" || lookupModule == "Terminal") {

            //    if (lookupModule == "SIPLContact" || lookupModule == "RailRamp") {
            //        if ($scope.entity[lookupField] == null || $scope.entity[lookupField] == "") {
            //            $scope.clearLookups(source, lookupModule, lookupIndex);
            //        }
            //    }
            //    else if ($scope.entity[lookupModule] == null || $scope.entity[lookupModule] == "") {
            //        $scope.clearLookups(source, lookupModule, lookupIndex);
            //    }
            //}
            if ($scope.isInvalidData == true) {
                $scope.clearLookups(source, lookupModule, lookupIndex);
            }
        };

        $scope.searchValues = function (viewValue, selectType, searchRouteType) {
            var resultItem = {};
            var lookupModule;
            var routeType = "";
            var lookupField = "name";
            if (selectType == "" || selectType == null) {
                ngNotifier.error("Please select search options");
                return;
            }
            if (selectType == "SearchRouteName") {
                return entityService.getRouteNames(viewValue).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.routeName;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );

            }
            if (selectType == "SearchOrigin" || selectType == "SearchDestination") {
                if (searchRouteType == "" || searchRouteType == null) {
                    ngNotifier.error("Please select Route option");
                    return;
                }
                if (searchRouteType == "City") {
                    lookupModule = "LGVWCity";
                    routeType = "City";
                }
                if (searchRouteType == "Port") {
                    lookupModule = "LGVWPort";
                    routeType = "Port";
                }
                if (searchRouteType == "railRamp") {
                    lookupModule = "RailRamp";
                    routeType = "railRamp";
                    lookupField = "railRamp"
                }
            }
            if (selectType == "origin") {
                if ($scope.entity.originType == "City") {
                    lookupModule = "LGVWCity";
                    routeType = "City";
                }
                if ($scope.entity.originType == "Port") {
                    lookupModule = "LGVWPort";
                    routeType = "Port";
                }
                if ($scope.entity.originType == "railRamp") {
                    lookupModule = "RailRamp";
                    routeType = "railRamp";
                    lookupField = "railRamp"
                }
            }
            if (selectType == "destination") {
                if ($scope.entity.destinationType == "City") {
                    lookupModule = "LGVWCity";
                    routeType = "City";
                }
                if ($scope.entity.destinationType == "Port") {
                    lookupModule = "LGVWPort";
                    routeType = "Port";
                }
                if ($scope.entity.destinationType == "railRamp") {
                    lookupModule = "RailRamp";
                    routeType = "railRamp";
                    lookupField = "railRamp"
                }
            }
            if (selectType == "viaType1") {
                if ($scope.entity.viaType1 == "City") {
                    lookupModule = "LGVWCity";
                    routeType = "City";
                }
                if ($scope.entity.viaType1 == "Port") {
                    lookupModule = "LGVWPort";
                    routeType = "Port";
                }
                if ($scope.entity.viaType1 == "railRamp") {
                    lookupModule = "RailRamp";
                    routeType = "railRamp";
                    lookupField = "railRamp"
                }
            }
            if (selectType == "viaType2") {
                if ($scope.entity.viaType2 == "City") {
                    lookupModule = "LGVWCity";
                    routeType = "City";
                }
                if ($scope.entity.viaType2 == "Port") {
                    lookupModule = "LGVWPort";
                    routeType = "Port";
                }
                if ($scope.entity.viaType2 == "railRamp") {
                    lookupModule = "RailRamp";
                    routeType = "railRamp";
                    lookupField = "railRamp"
                }
            }
            return $scope.callTypeahead(viewValue, lookupModule, lookupField, null).then(
                function (output) {
                    $scope.searchResult = [];
                    output.forEach(function (o) {
                        resultItem = {}
                        if (routeType == "City") {
                            resultItem.name = o.name;
                            resultItem.cityId = o.cityId;
                            $scope.searchResult.push(resultItem)
                        }
                        if (routeType == "Port") {
                            resultItem.name = o.name;
                            resultItem.portId = o.portId;
                            $scope.searchResult.push(resultItem)
                        }
                        if (routeType == "railRamp") {
                            resultItem.name = o.railRamp;
                            resultItem.railId = o.railId;
                            $scope.searchResult.push(resultItem)
                        }
                    });
                    return $scope.searchResult;
                }
            );



        };

        $scope.routingTypes = [
                { optionValue: "Port", optionName: "Port" },
                { optionValue: "City", optionName: "City" },
                { optionValue: "railRamp", optionName: "RailRamp" }                
        ];

        $scope.searchRoutingTypes = [
                { optionValue: "", optionName: "-All-" },
                { optionValue: "Port", optionName: "Port" },
                { optionValue: "City", optionName: "City" },
                { optionValue: "railRamp", optionName: "RailRamp" }
        ];

        $scope.searchOptions = [
                { optionValue: "", optionName: "-All-" },
                { optionValue: "SearchRouteName", optionName: "Route Name" },
                { optionValue: "SearchOrigin", optionName: "Origin" },
                { optionValue: "SearchDestination", optionName: "Destination" }
        ];

        $scope.enableRouterType = function (selectOption) {
            if (selectOption == "SearchRouteName") {
                $scope.isRouteTypeDisabled = true
            }
            else {
                $scope.isRouteTypeDisabled = false;
            }

        };
      
        $scope.selectOption = "";
        $scope.searchBox = "";
       

        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox            
        };

        

        $scope.routelistTable = new NgTableParams(
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

                      var dataitems = entityService.getCaptureRouteList(listParams).then(
                          function (output) {
                              $scope.validateUser(output);
                              $scope.items = output.data.data;
                              params.total(output.data.count);
                          },
                          function (output) {
                              ngNotifier.showError($scope.authentication, output);
                          }
                      );
                  }
              });
        $scope.performRouteSearch = function (source, selectOption, searchBox) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox               
            };
            $scope.routelistTable.reload();
        };

        $scope.clearValues = function (source) {
            if(source == 'origin')
            {
                $scope.entity.origin = "";
                $scope.entity.fkOriginID = null;
                            }
            if (source == 'destination') {
                $scope.entity.destination = "";
                $scope.entity.fkDestinationID = null;
            }
            if (source == 'viaType1') {
                $scope.entity.via1 = null;
                $scope.entity.viaType1 = "";
            }
            if (source == 'viaType2') {
                $scope.entity.via1 = null;
                $scope.entity.viaType1 = "";
            }
        };

        $scope.performCaputreRouteAction = function (source, fromList) {

            var action = source.currentTarget.attributes["action"].value;

            //$scope.$broadcast("show-errors-check-validity");

            //if (action != "cancel" && $scope.form.detail != undefined && $scope.form.detail.$invalid) {
            //    if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
            //        ngNotifier.error("Required Field(s) are missing data.");
            //    }
            //    else if ($scope.form.detail.usrPwdC.$invalid) {
            //        ngNotifier.error("Password do not match with Confirm Password.");
            //    }
            //    return;
            //}

            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {

                ///$scope.showBookingDetail(action, documentCommonID);

            } else {
                initControls(action);
            }

            //switchTab("Detail", action);

            switch (action) {
                case "search":
                    filterList();
                    break;
                case "add":
                    //lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    //$scope.showPendingMovementDetails(action, documentCommonID, fileno);
                    break;
                case "copy":
                    //lastAction = action;
                    //$scope.entity.enquiryID = 0;                    
                    break;
                    //lastAction = 'copy';
                    //$scope.entityId = 0;
                    //$scope.entity = {};
                    //$("input[input-date]").each(function (index, element) { $(element).val(null); });
                    break;
                case "edit":
                    //lastAction = action;
                    break;
                case "save":
                    save(action);
                    break;
                case "saveEmail":
                    //$scope.entity.isSendEmailNow = true;
                    //save(action);
                    break;
                case "cancel":
                    //$scope.showQuotationDetail(action, enquiryID, quotationID);
                    //$scope.showEnquiryDetail('viewDetail', $scope.entity.enquiryID, $scope.entity.isComplete);
                    //lastAction = "";
                    $scope.goBack();
                    break;
                case "delete":
                    remove();
                    //lastAction = "";
                    break;
                case "deleteBatch":
                    removeBatch();
                    //lastAction = "";
                    break;
                case "verify":
                case "activate":
                case "deactivate":
                    $scope.changeStatus(action);
                    lastAction = "";
                    break;
                default:
                    //lastAction = "";
                    break;
            }

            if ($scope.afterPerformAction != undefined) {
                $scope.afterPerformAction(source, fromList);
            }
        };

        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
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

        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":

                    break;
                case "edit":
                    $scope.getcarrierAllRates($scope.entity.enquiryID);


                    break;

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
            }
            else {
                ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                    entities.forEach(function (entity) {                       
                       
                        entityService.deleteRoute(entity).then(
                            function (output) {
                                $scope.entity = {};
                                $scope.routelistTable.reload();
                                $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });
                    });
                });
            }
        };

        $scope.saveCaptureRoute = function (source, fromList) {

            if ($scope.entity.routeName == "" || $scope.entity.routeName == null) {

                ngNotifier.error("Please enter Route Name");
                return;
            }
            if ($scope.entity.origin == "" || $scope.entity.origin == null) {

                ngNotifier.error("Please enter Origin");
                return;
            }
            if ($scope.entity.destination == "" || $scope.entity.destination == null) {

                ngNotifier.error("Please enter destination");
                return;
            }
            if ($scope.entity.routeId == null)
            {
                $scope.entity.routeId = 0;
            }
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            $scope.entity.updatedBy = $scope.$parent.authentication.userId;
            entityService.saveCaptureRoute($scope.entity).then(
                function (output) {
                    //$scope.enquiryID = output.data.data;
                    $scope.entity = {};
                    
                    $scope.selectOption = "RouteName";
                    $scope.searchBox = "";
                    $scope.routelistTable.reload();
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

      
       

        var resetFilterField = function () {

            $scope.entity.miamiRefNo = ($scope.entity.filterFieldName == "miamiRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.systemRefNo = ($scope.entity.filterFieldName == "systemRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.bookingNo = ($scope.entity.filterFieldName == "bookingNo") ? $scope.entity.filterFieldValue : "";
        };

        $scope.getCaptureRouteDetails = function (source, routeId) {
            var action = source.currentTarget.attributes["action"].value;
            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entity = {};
            $scope.entity.routeId = routeId;
                     
            $scope.entity.siteId = $scope.$parent.selectedSiteId;

            entityService.getCaptureRouteDetails($scope.entity).then(
                 function (output) {
                     if (output.data.resultId == 2005) {
                         ngNotifier.showError($scope.authentication, output);
                         $scope.logOut()
                     }
                     $scope.entity = output.data.data;
                    
                 },
                 function (output) {
                     ngNotifier.showError($scope.authentication, output);
                 }
             );
        };

       

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("captureRouteController", controller);

});
