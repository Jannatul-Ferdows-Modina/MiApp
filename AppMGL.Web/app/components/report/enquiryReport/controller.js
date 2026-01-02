"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "enquiryReportService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.enquiryReport;
        $scope.tabs = appUrl.enquiryReport.tabs;

        //#endregion

        //#region Private 

        var exportReportRemark = function () {

            resetFilterField();
            var reportParams = {
                Year: $scope.entity.year,
                Type: $scope.entity.type,
                EnquiryDateFrom: $scope.entity.enquiryDateFrom || "",
                EnquiryDateTo: $scope.entity.enquiryDateTo || "",
                EnquiryNo: $scope.entity.enquiryNo || "",
                CompanyName: $scope.entity.companyName || "",
                QuotationNo: $scope.entity.quotationNo || "",
                SystemRefNo: $scope.entity.systemRefNo || "",
                MiamiRefNo: $scope.entity.miamiRefNo || "",
                BookingNo: $scope.entity.bookingNo || "",
                SitId: $scope.$parent.selectedSiteId
            };
            entityService.exportReportRemark(reportParams).then(
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
        var exportReport = function () {

            resetFilterField();
            var reportParams = {
                Year: $scope.entity.year,
                Type: $scope.entity.type,
                EnquiryDateFrom: $scope.entity.enquiryDateFrom || "",
                EnquiryDateTo: $scope.entity.enquiryDateTo || "",
                EnquiryNo: $scope.entity.enquiryNo || "",
                CompanyName: $scope.entity.companyName || "",
                QuotationNo: $scope.entity.quotationNo || "",
                SystemRefNo: $scope.entity.systemRefNo || "",
                MiamiRefNo: $scope.entity.miamiRefNo || "",
                BookingNo: $scope.entity.bookingNo || "",
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

        //#endregion

        //#region Lookup 

        $scope.lookups = { years: [], types: [], filterFields: [] };

        $scope.initDropdown = function () {

            $scope.refillYears((new Date()).getFullYear());

            $scope.lookups.types = [];
            $scope.lookups.types.push({ typeId: 0, typeValue: "Select any one Type" });
            $scope.lookups.types.push({ typeId: 1, typeValue: "Pending" });
            $scope.lookups.types.push({ typeId: 2, typeValue: "Send for Quotation (Including Pending Booking)" });
            $scope.lookups.types.push({ typeId: 3, typeValue: "Enquiry Booked (Including Cancelled after Booking)" });
            $scope.lookups.types.push({ typeId: 4, typeValue: "Cancelled Enquiry" });
            $scope.lookups.types.push({ typeId: 5, typeValue: "All (Excluding Cancelled Records)" });

            $scope.entity.type = 0;

            $scope.lookups.filterFields = [];
            $scope.lookups.filterFields.push({ fieldName: "enquiryNo", fieldValue: "Enquiry No" });
            $scope.lookups.filterFields.push({ fieldName: "quotationNo", fieldValue: "Quotation No" });
            $scope.lookups.filterFields.push({ fieldName: "systemRefNo", fieldValue: "System Ref No" });
            $scope.lookups.filterFields.push({ fieldName: "miamiRefNo", fieldValue: "Miami Ref No" });
            $scope.lookups.filterFields.push({ fieldName: "bookingNo", fieldValue: "Booking No" });

            $scope.entity.filterFieldName = "enquiryNo";
        };

        //#endregion

        //#region Detail

        $scope.reportTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }")
        }, {
            counts: [],
            getData: function (params) {
                resetFilterField();
                var listParams = {
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),
                    Year: $scope.entity.year,
                    Type: $scope.entity.type,
                    EnquiryDateFrom: $scope.entity.enquiryDateFrom || "",
                    EnquiryDateTo: $scope.entity.enquiryDateTo || "",
                    EnquiryNo: $scope.entity.enquiryNo || "",
                    CompanyName: $scope.entity.companyName || "",
                    QuotationNo: $scope.entity.quotationNo || "",
                    SystemRefNo: $scope.entity.systemRefNo || "",
                    MiamiRefNo: $scope.entity.miamiRefNo || "",
                    BookingNo: $scope.entity.bookingNo || "",
                    SitId: $scope.$parent.selectedSiteId
                };
                return entityService.getEnquiryList(listParams).then(
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

        $scope.performSubAction = function (source, target) {

            var action = source.currentTarget.attributes["action"].value;

            if ($scope.entity.type == 0) {
                ngNotifier.error("First, select any one type.");
                return;
            }

            switch (action) {
                case "showReport":
                    $scope.reportTable.reload();
                    break;
                    
                case "exportReport":
                    exportReport();
                    break;
                case "exportReportRemark":
                    exportReportRemark();
                    break;
                default:
                    //TODO
                    break;
            }
        };

        $scope.refillYears = function (currentYear) {

            $scope.lookups.years = [];
            for (var k = currentYear - 5; k < currentYear + 5; k++) {
                $scope.lookups.years.push({ yearId: k, yearValue: k });
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

        (function () {
            $scope.entity.year = (new Date()).getFullYear();
        })();

    };

    controller.$inject = injectParams;

    app.register.controller("enquiryReportController", controller);

});
