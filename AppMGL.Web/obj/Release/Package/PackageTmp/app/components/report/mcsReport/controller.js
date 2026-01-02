"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "mcsReportService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.mcsReport;
        $scope.tabs = appUrl.mcsReport.tabs;

        //#endregion

        //#region Private 

        var exportReport = function () {

            var reportParams = {
                SiteId: $scope.entity.siteId,
                StartBookingDate: $scope.entity.startBookingDate || "",
                EndBookingDate: $scope.entity.endBookingDate || "",
                DeptId: $scope.entity.departmentId
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

        //#endregion

        //#region Lookup 

        $scope.lookups = { departments: [], sites: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "departments", null);

            var sitAll = $.map($scope.authentication.userSite, function (o) { return o.SitId; }).join(",");
            $scope.lookups.sites = angular.copy($scope.authentication.userSite);
            $scope.lookups.sites.unshift({ SitId: sitAll, SitName: "All" });
            $scope.entity.siteId = sitAll;


            $scope.entity.startBookingDate = Utility.getDateISO(new Date("01/01/2018"));
            $scope.entity.endBookingDate = Utility.getDateISO(new Date("12/31/2018"));
            $scope.entity.departmentId = 2;
        };

        $scope.afterFetchLookupData = function (lookupKey) {

            if (lookupKey == "departments") {
                $scope.lookups[lookupKey].splice(0, 0, { departmentID: 0, department: "Select any one Department" });
                $scope.entity.departmentId = 2;
            }
        };

        //#endregion

        //#region Detail

        $scope.reportTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }"),
            group: {
                sitId: "asc"
            }
        }, {
            counts: [],
            getData: function (params) {
                var listParams = {
                    SiteId: $scope.entity.siteId,
                    StartBookingDate: $scope.entity.startBookingDate || "",
                    EndBookingDate: $scope.entity.endBookingDate || "",
                    DeptId: $scope.entity.departmentId
                };
                return entityService.getMCSList(listParams).then(
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

        $scope.performSubAction = function (source, target) {

            var action = source.currentTarget.attributes["action"].value;

            $scope.$broadcast("show-errors-check-validity");
            if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
                ngNotifier.error("Required Field(s) are missing data.");
                return;
            }

            if ($scope.entity.departmentId == 0) {
                ngNotifier.error("First, select any one department.");
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

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("mcsReportController", controller);

});
