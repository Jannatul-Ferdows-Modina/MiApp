"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "bookingStatusReportService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.bookingStatusReport;
        $scope.tabs = appUrl.bookingStatusReport.tabs;

        //#endregion

        //#region Private 

        var exportReport = function () {

            resetFilterField();
            var reportParams = {
                BookingStatusId: $scope.entity.bookingStatusId,
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

        $scope.lookups = { bookingStatuses: [], filterFields: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("siplbookingstatus", 0, "status", "bookingStatuses", null);

            $scope.lookups.filterFields = [];
            $scope.lookups.filterFields.push({ fieldName: "enquiryNo", fieldValue: "Enquiry No" });
            $scope.lookups.filterFields.push({ fieldName: "quotationNo", fieldValue: "Quotation No" });
            $scope.lookups.filterFields.push({ fieldName: "systemRefNo", fieldValue: "System Ref No" });
            $scope.lookups.filterFields.push({ fieldName: "miamiRefNo", fieldValue: "Miami Ref No" });
            $scope.lookups.filterFields.push({ fieldName: "bookingNo", fieldValue: "Booking No" });

            $scope.entity.filterFieldName = "enquiryNo";
        };

        $scope.afterFetchLookupData = function (lookupKey) {

            if (lookupKey == "bookingStatuses") {
                $scope.lookups[lookupKey].splice(0, 0, { statusID: 0, status: "Select any one Booking Status", isActive: true });
                $scope.entity.bookingStatusId = 0;
            }
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
                    BookingStatusId: $scope.entity.bookingStatusId,
                    EnquiryNo: $scope.entity.enquiryNo || "",
                    CompanyName: $scope.entity.companyName || "",
                    QuotationNo: $scope.entity.quotationNo || "",
                    SystemRefNo: $scope.entity.systemRefNo || "",
                    MiamiRefNo: $scope.entity.miamiRefNo || "",
                    BookingNo: $scope.entity.bookingNo || "",
                    SitId: $scope.$parent.selectedSiteId
                };
                return entityService.getBookingStatusList(listParams).then(
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

            if ($scope.entity.bookingStatusId == 0) {
                ngNotifier.error("First, select any one booking status.");
                return;
            }

            switch (action) {
                case "showReport":
                    $scope.reportTable.reload();
                    break;
                case "exportReport":
                    exportReport();
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

    app.register.controller("bookingStatusReportController", controller);

});
