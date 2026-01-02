"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "lgvwmodeoftransportService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        $scope.page = appUrl.lgvwmodeoftransport;
        $scope.tabs = appUrl.lgvwmodeoftransport.tabs;
           
          $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
                $scope.entity.createdBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.modifiedOn = new Date();
                $scope.entity.modifiedBy = $scope.$parent.userInfo.usrId;
            }
        };

        $scope.lookups = { statenames: [] };

       
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("lgvwmodeoftransportController", controller);

});
