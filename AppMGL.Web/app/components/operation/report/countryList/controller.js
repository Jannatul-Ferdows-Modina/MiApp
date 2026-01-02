"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "countryListService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.countryList;
        $scope.tabs = appUrl.countryList.tabs;

        //#endregion

        //#region Detail

        //$scope.reportServiceUrl = 'http://js.syncfusion.com/ejservices/api/ReportViewer';
        //$scope.remoteMode = ej.ReportViewer.ProcessingMode.Remote;
        //$scope.rdlReportPath = 'GroupingAgg.rdl';

        $scope.ServerUrl = "http://192.168.10.51:55220";
        $scope.reportServiceUrl = "http://js.syncfusion.com/ejservices/api/ReportViewer";
        $scope.remoteMode = ej.ReportViewer.ProcessingMode.Remote;
        $scope.ssrsReportPath = "/Sample Reports/Country List";

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("countryListController", controller);

});
