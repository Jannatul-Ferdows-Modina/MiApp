"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "lgvwaliasService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.lgvwalias;
        $scope.tabs = appUrl.lgvwalias.tabs;

        //#endregion

        //#region Detail

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

        $scope.lookups = { types: [], statenames: [], citynames: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("lgvwalias", 0, "type", "types", null);
            $scope.fetchLookupData("lgvwstate", 0, "name", "statenames", null);
            $scope.fetchLookupData("lgvwcity", 0, "name", "citynames", null);
        };

        

        $scope.aliastypes = [
               { optionValue: "Select", optionName: "Select" },
               { optionValue: "City", optionName: "City" },
               { optionValue: "State", optionName: "State" }
              
        ];

        $scope.isOneTypeRequired = function () {
            return !($scope.entity.cityId || $scope.entity.fkStateId);
        };


        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("lgvwaliasController", controller);

});
