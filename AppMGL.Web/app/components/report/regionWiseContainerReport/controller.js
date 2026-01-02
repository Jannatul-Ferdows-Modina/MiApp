// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "regionWiseContainerReportService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.regionWiseContainerReport;
        $scope.tabs = appUrl.regionWiseContainerReport.tabs;

        $scope.actionRemarksList = [];
        $scope.shipmentDocsList = [];
        $scope.searchOptions2 = [];
        $scope.searchOptions3 = [];
        $scope.isOption2disabled = true;
        $scope.isOption3disabled = true;
        $scope.lookups = { siplDepartments: [], miamiBookingStatus: [] };

        $scope.initDropdown = function () {
            //$scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
            //$scope.fetchLookupData("siplBookingStatus", 0, "Status", "miamiBookingStatus", null);
            //$scope.entity.startBookingDate = Utility.getDateISO(new Date("01/01/2018"));
            //$scope.entity.endBookingDate = Utility.getDateISO(new Date("12/31/2018"));
            //$scope.entity.departmentId = 2;
        };
        $scope.afterFetchLookupData = function (lookupKey) {
            //if (lookupKey == "siplDepartments") { $scope.lookups.siplDepartments.unshift({ "departmentID": 0, "department": "-Select-" }); } //
            //if (lookupKey == "miamiBookingStatus") { $scope.lookups.miamiBookingStatus.unshift({ "statusID": 0, "status": "-Select-" }); } //

        };
        //#endregion       

        $scope.searchOptions = [
                { optionValue: "", optionName: "-All-" },
                { optionValue: "SystemRefNo", optionName: "System Ref No" },
                { optionValue: "Customer", optionName: "Customer" },                
                { optionValue: "LineBookingNo", optionName: "Line Booking No" }
        ];

        $scope.seachDateFilters = [
                { optionValue: "", optionName: "-Select-" },                
                { optionValue: "ETA", optionName: "ETA" },
                { optionValue: "ETS", optionName: "ETS" },
                { optionValue: "BookingDate", optionName: "Booking Date" }                
        ];

        

        $scope.selectDateOption = "-Select-";
        $scope.fromDate = "",
        $scope.toDate = "",
        $scope.selectOption = "SystemRefNo";
        $scope.searchBox = "";

        $scope.searchParam = {
            optionDateValue: $scope.selectDateOption,
            fromDate: $scope.fromDate,
            toDate: $scope.toDate          
        };

        $scope.clearDates = function (dateoption) {
            if (dateoption == "") {
                $scope.fromDate = null;
                //angular.element("#toDate")[0].value = null;
                $scope.toDate = null
            }
        };

        $scope.reportTable = new NgTableParams(
       {
           page: 1,
           count: 10,
           sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }"),
           group: {
               regionName: "asc"
           }
       }, {
           counts: [],
           getData: function (params) {
               var listParams = {
                   SiteId: $scope.$parent.selectedSiteId,
                   ModuleId: $scope.page.moduleId,
                   PageIndex: params.page(),
                   PageSize: params.count(),
                   Sort: JSON.stringify(params.sorting()),
                   Filter: JSON.stringify($scope.searchParam)
               };
               return entityService.getRegionWiseContainerList(listParams).then(
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

        $scope.sum = function (data, field) {
            return Utility.sumArray(data, field);
        }
        
        $scope.searchValues = function (viewValue, selectOption) {
            var resultItem = {};
            if (selectOption == "companyName") {
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
            
        };
        $scope.performBookingSearch = function (source, selectDateOption, fromDate, toDate) {

            var action = source.currentTarget.attributes["action"].value;
            if (selectDateOption == null) {
                selectDateOption = "-Select-";
            }
            if (selectDateOption != null && selectDateOption != "-Select-" && selectDateOption != "") {

                if (fromDate == null || fromDate == '' || toDate == null || toDate == '') {
                    ngNotifier.error("Please enter valid From Date and To Date");
                    return;
                }
            }          
            
            $scope.searchParam = {
                optionDateValue: selectDateOption,
                fromDate: fromDate,
                toDate: toDate                
            };
            $scope.reportTable.reload();
        };

        $scope.exportExpensesReport = function (source, selectDateOption, fromDate, toDate) {

            var action = source.currentTarget.attributes["action"].value;
            var optionValue;
            var seachValue;
            var optionDateValue;
           

            var action = source.currentTarget.attributes["action"].value;
            if (selectDateOption == null) {
                selectDateOption = "-Select-";
            }
            if (selectDateOption != null && selectDateOption != "-Select-" && selectDateOption != "") {

                if (fromDate == null || fromDate == '' || toDate == null || toDate == '') {
                    ngNotifier.error("Please enter valid From Date and To Date");
                    return;
                }
            }
           
           

            if (fromDate == '') { fromDate = '1900-01-01' };
            if (toDate == '') { toDate = '1900-01-01' };
            
            
            var reportParams = {
                optionDateValue: selectDateOption,
                fromDate: fromDate,
                toDate: toDate,               
                SitId: $scope.$parent.selectedSiteId
            };
            entityService.exportRegionWiseContainerReport(reportParams).then(
                function (output) {
                    var blobData = new Blob([output.data], { type: output.headers()["content-type"] });
                    var fileName = output.headers()["x-filename"];
                    saveAs(blobData, fileName);
                },
                function (output) {
                    ngNotifier.error(output);
                }
            );
        }

        $scope.UpdateSearch2 = function (selectOption1) {
            $scope.searchOptions2 = [];
            $scope.searchOptions3 = [];
            $scope.isOption2disabled = false;
            //$scope.isOption3disabled = true;
            $scope.searchOptions.forEach(function (item) {
                if (item.optionValue != selectOption1) {
                    $scope.searchOptions2.push(item);
                }
            });
        };
        $scope.UpdateSearch3 = function (selectOption1, selectOption2) {
            $scope.searchOptions3 = [];
            $scope.isOption3disabled = false;
            $scope.searchOptions.forEach(function (item) {
                if (item.optionValue != selectOption1 && item.optionValue != selectOption2) {
                    $scope.searchOptions3.push(item);
                }
            });
        };

        $scope.performBookingAction = function (source, fromList) {

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
                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    $scope.getDocumentAttachmentDetail(action, documentCommonID);
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
                    $scope.entity.isSendEmailNow = true;
                    save(action);
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
                    deleteShipmentDoc(enquiryID, quotationID);
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
                    //$scope.getcarrierAllRates($scope.entity.enquiryID);


                    break;

            }
        };



        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("regionWiseContainerReportController", controller);

});
