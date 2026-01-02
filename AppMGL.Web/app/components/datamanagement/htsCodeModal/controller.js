"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "htsCodeModalService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {

        $scope.htsid = (requestData.htsid) ? requestData.htsid : '';
        $scope.responseData = {
            data: null,
            resultId: 2001
        };
       
        $scope.lookups = { siplContinents:[], countries: [], origincountries: [], states: [], lgvwstates: [], commoditys: [], categories: [], cities: [], companyGradations: [], users: [], accountCategories: [] };

        $scope.initDropdown = function () {
           
            $scope.fetchLookupData("Booking", '0', "name", "uomList", "uomList");
            
        };

        $scope.beforeSave = function (action, lastAction) {
            
            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
            }
            else {
                $scope.entity.modifiedOn = new Date();
            }
            $scope.$broadcast("show-errors-check-validity");

            var finalhtsNumber = "";
            if ($scope.entity.htsfirst != "" && $scope.entity.htsfirst != null)
                finalhtsNumber = $scope.entity.htsfirst;
            if ($scope.entity.htssecond != "" && $scope.entity.htssecond != null)
                finalhtsNumber += '.' + $scope.entity.htssecond;
            if ($scope.entity.htsthird != "" && $scope.entity.htsthird != null)
                finalhtsNumber += '.' + $scope.entity.htsthird;
            $scope.entity.htsNumber = finalhtsNumber;
            $scope.entity.createdBy = $scope.$$prevSibling.authentication.userId;
            if ($scope.entity.htsNumber == null || $scope.entity.htsNumber == "") {
                ngNotifier.error("Please enter HTS Code");
                return true;
            }

        };
        $scope.getDetail = function (id) {
            $scope.entityId = id;
            if ($scope.entityId > 0) {
                entityService.detail($scope.entityId).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        $scope.entity = output.data.data;
                        if ($scope.entity.secondUOM == "")
                            $scope.entity.secondUOM = null;
                        if ($scope.entity.firstUOM == "")
                            $scope.entity.firstUOM = null;
                        
                        if ($scope.afterGetDetail != undefined) {
                            $scope.afterGetDetail();
                        }

                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            } else {
                $scope.goBack();
            }
        };

        if ($scope.htsid != undefined && $scope.htsid != '' && $scope.htsid != null) {

            $scope.getDetail($scope.htsid);
        }
       
        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));
    };

    controller.$inject = injectParams;
    app.register.controller("htsCodeModalController", controller);

});
