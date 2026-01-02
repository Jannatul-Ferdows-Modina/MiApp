"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "displayRateService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.displayRate;
        $scope.tabs = appUrl.displayRate.tabs;
       // $('table').stickyTableHeaders();
        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
                $scope.entity.createdBy = $scope.$parent.userInfo.usrId;
                $scope.entity.siteId = $scope.$parent.selectedSiteId;


            }
            else {
                $scope.entity.modifiedOn = new Date();
                $scope.entity.modifiedBy = $scope.$parent.userInfo.usrId
            }
        };

        

       
        $scope.lookups = { siplContact: [], commodityTypes: [], commodities: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("siplContact", 28, "companyName", "siplContact", null);
            $scope.fetchLookupData("Commodity", 0, "name", "commodities", null);

            $scope.mQCTypes = [
                { mqctype: 0, optionName: "TEU" },
                { mqctype: 1, optionName: "FEU" }
            ];

        };

        $scope.searchResult1 = [];
        $scope.searchValues = function (viewValue, selectOption) {
            var resultItem = {};
            if (selectOption == "carrier") {
                return $scope.callTypeahead(viewValue, 'SIPLContact', 'companyName', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.companyName;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
            if (selectOption == "originPort" || selectOption == "dischargePort") {
                return $scope.callTypeahead(viewValue, 'LGVWPort', 'name', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.name + '(' + o.countryName + ')';
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
            if (selectOption == "originRailramp" || selectOption == "dischargeRailramp") {
                return $scope.callTypeahead(viewValue, 'railramp', 'railRamp', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.railRamp;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
            if (selectOption == "generalCargo") {
                return $scope.callTypeahead(viewValue, 'commodity', 'name', null, null, 'commoditytypeid', 2).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.name;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
            if (selectOption == "hazardous") {
                return $scope.callTypeahead(viewValue, 'commodity', 'name', null, null, 'commoditytypeid', 1).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.name;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
            var lookupModule;

            if (selectOption == "city" || selectOption == "originCity" || selectOption == "dischargeCity") {
                lookupModule = "LGVWCity";
            }
            if (selectOption == "originCity" || selectOption == "dischargeCity" || selectOption == "origin" || selectOption == "discharge" || selectOption == "RailRamp") {
                return $scope.callTypeahead(viewValue, lookupModule, 'name', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.name;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
        };

        $scope.searchOptions = [
               { optionValue: "carrier", optionName: "Carrier" },
               { optionValue: "contractNo", optionName: "Contract No" }
        ];
        $scope.selectOption = "carrier";
        $scope.searchBox = "";

        $scope.searchOptionsOrigin = [
               { optionValue: "originPort", optionName: "Origin Port" },
               { optionValue: "originCity", optionName: "Origin City" },
               { optionValue: "originRailramp", optionName: "Origin RailRamp" }
        ];
        $scope.selectOptionOrigin = "originPort";
        $scope.searchBoxorigin = "";

        $scope.searchOptionsDisch = [
             { optionValue: "dischargePort", optionName: "Discharge Port" },
             { optionValue: "dischargeCity", optionName: "Discharge City" },
             { optionValue: "dischargeRailramp", optionName: "Discharge RailRamp" }
        ];
        $scope.selectOptionDisch = "dischargePort";
        $scope.searchBoxorigin = "";

        $scope.searchOptionsComm = [
             { optionValue: "generalCargo", optionName: "General Cargo" },
             { optionValue: "hazardous", optionName: "Hazardous" }
        ];
        $scope.selectOptionComm = "generalCargo";
        $scope.searchBoxorigin = "";

        $scope.selectOption = "";
        $scope.searchBox = "";
        $scope.startDate = "";
       // $scope.startDateValue = "";
        $scope.endDate = "";
       // $scope.endDateValue = "";
        $scope.selectOptionOrigin = "";
        $scope.searchBoxorigin = "";
        $scope.selectOptionDisch = "";
        $scope.searchBoxdisch = "";
        $scope.selectOptionComm = "";
        $scope.searchBoxcomm = "";
        $scope.route = "";
        //$scope.routeValue = "";
        $scope.via1 = "";
       // $scope.via1Value = "";
        $scope.via2 = "";
       // $scope.via2Value = "";

        $scope.searchParam = {
            selectOption: $scope.selectOption,
            searchBox: $scope.searchBox,
            startDate: $scope.startDate,
           // startDateValue: $scope.startDateValue,
            endDate: $scope.endDate,
           // endDateValue: $scope.endDateValue,
            selectOptionOrigin: $scope.selectOptionOrigin,
            searchBoxorigin: $scope.searchBoxorigin,
            selectOptionDisch: $scope.selectOptionDisch,
            searchBoxdisch: $scope.searchBoxdisch,
            selectOptionComm: $scope.selectOptionComm,
            searchBoxcomm: $scope.searchBoxcomm,
            route: $scope.route,
           // routeValue: $scope.routeValue,
            via1: $scope.via1,
           // via1Value: $scope.via1Value,
            via2: $scope.via2,
           // via2Value: $scope.via2Value
        };
        // $scope.displayRatelistTable.reload();
        $scope.displayRatelistTable = new NgTableParams(
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

                     var dataitems = entityService.getDisplayRateList(listParams).then(
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

        $scope.performDisplayRateSearch = function (source, selectOption, searchBox, startDate, endDate, selectOptionOrigin, searchBoxorigin, selectOptionDisch, searchBoxdisch, selectOptionComm, searchBoxcomm, route, via1, via2) {

            var action = source.currentTarget.attributes["action"].value;
            //if (searchBox == null) {
            //    searchBox = "";
            //}
            //if (startDate != null || startDate != undefined) {

            //    startDate = "StartDate";
            //}
            //if (endDate != null || endDate != undefined) {

            //    endDate = "EndDate";
            //}             
            
            $scope.searchParam = {
                selectOption: selectOption,
                searchBox: searchBox,
                startDate: startDate,
                //startDateValue: startDateValue,
                endDate: endDate,
               // endDateValue: endDateValue,
                selectOptionOrigin: selectOptionOrigin,
                searchBoxorigin: searchBoxorigin,
                selectOptionDisch: selectOptionDisch,
                searchBoxdisch: searchBoxdisch,
                selectOptionComm: selectOptionComm,
                searchBoxcomm: searchBoxcomm,
                route: route,
                //routeValue: routeValue,
                via1: via1,
               // via1Value: via1Value,
                via2: via2,
               // via2Value: via2Value
            };
            $scope.displayRatelistTable.reload();
        };
        //$scope.callContractRateModal = function (source) {

        //    $scope.$parent.selectedSiteId
        //    $scope.$parent.authentication.userId

        //    var modalInstance = $uibModal.open({
        //        animation: false,
        //        backdrop: "static",
        //        keyboard: false,
        //        size: "lg",
        //        templateUrl: "app/components/operation/contractRateModal/detail.html",
        //        controller: "contractRateModalController",
        //        resolve: {
        //            requestData: function () {

        //                return {
        //                    contractID: (source || 0),
        //                    SitId: $scope.$parent.selectedSiteId
        //                };
        //            }
        //        }
        //    });
        //    modalInstance.result.then(
        //        function (output) {
        //            if (output.resultId == 1001) {
        //                $scope.entity.contractID = output.data.contractID;

        //            }
        //        },
        //        function (output) {
        //            ngNotifier.logError(output);
        //        });
        //};



        $scope.callContractRateModal = function (source) {
            $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                templateUrl: 'app/views/shared/dummy.html',
                controller: ['$scope', '$uibModalInstance', 'data', function ($scope, $uibModalInstance, data) {
                    $scope.title = data.data;
                    $scope.data = {
                        details: [{ destination: $scope.title.destination, origin: $scope.title.origin, refNo: "121212121", bookNo: "322323", quoteNo: "5454543", truck: "Ajdskdjsk dfsd", other: "Yuueroerioei", mode: "Truck" },
                { destination: $scope.title.destination, origin: $scope.title.origin, refNo: "43434212", bookNo: "5656534", quoteNo: "5454543", truck: "Ajdskdjsk dfsd", other: "Yuueroerioei", mode: "Truck" },
                        { destination: $scope.title.destination, origin: $scope.title.origin, refNo: "67674545", bookNo: "765645", quoteNo: "5454543", truck: "Ajdskdjsk dfsd", other: "Yuueroerioei", mode: "Truck" },
                        { destination: $scope.title.destination, origin: $scope.title.origin, refNo: "89676767", bookNo: "434e34", quoteNo: "5454543", truck: "Ajdskdjsk dfsd", other: "Yuueroerioei", mode: "Truck" },
                { destination: $scope.title.destination, origin: $scope.title.origin, refNo: "3454545", bookNo: "676756", quoteNo: "5454543", truck: "Ajdskdjsk dfsd", other: "Yuueroerioei", mode: "Truck" },
                        { destination: $scope.title.destination, origin: $scope.title.origin, refNo: "6756454", bookNo: "786756", quoteNo: "5454543", truck: "Ajdskdjsk dfsd", other: "Yuueroerioei", mode: "Truck" }]

                    };
                    $scope.data.secDetails = [
                        { bookingNo: "5656534", name: "Alex Carry", address: "123 Asdsdnmnmnsd", date: "12-Nov-2018" },
                        { bookingNo: "22323232", name: "Alex Stewart", address: "123 Asdsdnmnmnsd", date: "15-Nov-2018" },
                {
                    bookingNo: "6767565645", name: "John Carry", address: "123 Asdsdnmnmnsd", date: "18-Nov-2018"
                },
                    ]; $scope.data.acc = [
                        { selected: true, title: "Details" },
                        { selected: false, title: "Additional Information" },
                        { selected: false, title: "More Information" },
                    ]
                    $scope.data.showSec = false;
                    $scope.data.showPri = true;
                    $scope.data.showlast = false;
                    
                    $scope.clickHandler = function (type, item) {
                        if (type === 'primary') {
                            $scope.data.showSec = true;
                            $scope.data.showPri = false;
                            $scope.data.priSelData = item;
                        } else {
                            $scope.data.showPri = false;
                            $scope.data.showSec = false;
                            $scope.data.showlast = true;
                            $scope.data.secSelData = item;
                        }
                    };
                    $scope.ok = function () {
                        $uibModalInstance.close(true);
                    };
                    $scope.close = function () {
                        $uibModalInstance.dismiss();
                    };
                }],
                size: "lg",
                resolve: {
                    data: function () {
                        return {
                            data : source
                        };
                    }
                }
            });

        };

        //#endregion




        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("displayRateController", controller);

});
