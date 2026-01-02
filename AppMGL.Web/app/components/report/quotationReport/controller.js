"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "quotationReportService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.quotationReport;
        $scope.tabs = appUrl.quotationReport.tabs;

        //#endregion

        //#region Private 

        var exportReport = function () {

            resetFilterField();
            var reportParams = {
                Type: $scope.entity.type,
                EnquiryDateFrom: $scope.entity.enquiryDateFrom || "",
                EnquiryDateTo: $scope.entity.enquiryDateTo || "",
                OptionValue: $scope.entity.filterFieldName || " ",
                SearchValue: $scope.entity.filterFieldValue || " ",
                CompanyName: $scope.entity.companyName || " ",
                SitId: $scope.$parent.selectedSiteId
            };
            entityService.exportReport(reportParams).then(
                function (output) {
                    var blobData = new Blob([output.data], { type: output.headers()["content-type"] });
                    var fileName = output.headers()["x-filename"];
                    saveAs(blobData, fileName);
                },
                function (output) {
                    ngNotifier.error(output);
                }
            );
        };

        var resetFilterField = function () {

            $scope.entity.enquiryNo = ($scope.entity.filterFieldName == "enquiryNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.quotationNo = ($scope.entity.filterFieldName == "quotationNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.systemRefNo = ($scope.entity.filterFieldName == "systemRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.miamiRefNo = ($scope.entity.filterFieldName == "miamiRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.bookingNo = ($scope.entity.filterFieldName == "bookingNo") ? $scope.entity.filterFieldValue : "";
        };

        var viewCharges = function (quotationId) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/report/viewCharges/index.html",
                controller: "viewChargesController",
                resolve: {
                    requestData: function () {
                        return {
                            quotationId: quotationId
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    //if (output.data && output.resultId == 1001) {
                    //    ngNotifier.show(output.data);
                    //}
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        //#endregion

        //#region Lookup 

        $scope.lookups = { types: [], filterFields: [] };

        $scope.initDropdown = function () {

            $scope.lookups.types = [];
            $scope.lookups.types.push({ typeId: 0, typeValue: "Select any one Type" });
            $scope.lookups.types.push({ typeId: 1, typeValue: "All (Excluding Cancelled Records)" });
            $scope.lookups.types.push({ typeId: 2, typeValue: "Cancelled Quotation" });
            $scope.lookups.types.push({ typeId: 3, typeValue: "Pending Quotations" });
            $scope.lookups.types.push({ typeId: 4, typeValue: "Approval Awaited" });
            $scope.lookups.types.push({ typeId: 5, typeValue: "Approved Quotations to be sent for Booking" });
            $scope.lookups.types.push({ typeId: 6, typeValue: "Quotations sent for booking" });
            $scope.lookups.types.push({ typeId: 7, typeValue: "Booked Quotations" });

            $scope.entity.type = 0;

            $scope.lookups.filterFields = [];
            $scope.lookups.filterFields.push({ fieldName: "enquiryNo", fieldValue: "Enquiry No" });
            $scope.lookups.filterFields.push({ fieldName: "quotationNo", fieldValue: "Quotation No" });
            $scope.lookups.filterFields.push({ fieldName: "systemRefNo", fieldValue: "System Ref No" });            
            $scope.lookups.filterFields.push({ fieldName: "bookingNo", fieldValue: "Booking No" });

            $scope.entity.filterFieldName = "enquiryNo";
        };

        //#endregion

        //#region Detail
        $scope.Type = 0;
        $scope.enquiryDateFrom = "",
        $scope.enquiryDateTo = "",        
        $scope.filterFieldName = "";        
        $scope.filterFieldValue = "";
        $scope.companyName = ""
        

        $scope.searchParam = {
            Type: $scope.Type,
            EnquiryDateFrom: $scope.enquiryDateFrom,
            EnquiryDateTo: $scope.enquiryDateTo || '',
            OptionValue: $scope.filterFieldName || '',
            SeachValue: $scope.filterFieldValue || '',
            CompanyName: $scope.companyName || '',
        };

        $scope.reportTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")
        }, {
            
            getData: function (params) {
                //resetFilterField();
                var listParams = {
                    SiteId: $scope.$parent.selectedSiteId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),
                    Filter: JSON.stringify($scope.searchParam)
                };
                return entityService.getQuotationList(listParams).then(
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

        $scope.performQuotationSearch = function (source, valu1) {
            
            $scope.searchParam = {
                Type: $scope.entity.type,
                EnquiryDateFrom: $scope.entity.enquiryDateFrom || '',
                EnquiryDateTo: $scope.entity.enquiryDateTo || '',
                OptionValue: $scope.entity.filterFieldName || '',
                SeachValue: $scope.entity.filterFieldValue || '',
                CompanyName: $scope.entity.companyName || '',
            };

            $scope.reportTable.reload();
        };

        $scope.performSubAction = function (source, target) {

            var action = source.currentTarget.attributes["action"].value;

            //if ($scope.entity.type == 0) {
            //    ngNotifier.error("First, select any one type.");
            //    return;
            //}

            switch (action) {
                case "showReport":
                    $scope.reportTable.reload();
                    break;
                case "exportReport":
                    exportReport();
                    break;
                case "viewCharges":
                    viewCharges(source.currentTarget.attributes["entityid"].value);
                    break;
                default:
                    //TODO
                    break;
            }
        };

        $scope.disabledFilters = false;

        $scope.disableOtherFilters = function (flag) {

            if (!flag && ($scope.entity.filterFieldValue || "") != "") {
                return;
            }

            $scope.disabledFilters = flag;
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("quotationReportController", controller);

});
