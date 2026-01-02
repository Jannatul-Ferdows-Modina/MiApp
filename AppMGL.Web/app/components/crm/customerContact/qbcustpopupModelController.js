"use strict";

define(["app"], function (app) {

   
    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "customerContactService", "requestData"];
    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal,$uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData) {
   // var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {
        $scope.customerListQB = [];
        $scope.selCustomerName = '';
        $scope.ContactID = (requestData.ContactID) ? requestData.ContactID : 0;
        $scope.selCustomerName =$scope.CustomerName = (requestData.CustomerName) ? requestData.CustomerName : "";
        $scope.QBSynCustomer = function (qbid, qbcustomername) {
          /*  if ($scope.selCustomerName.toLowerCase() != qbcustomername.toLowerCase())
            {
                ngNotifier.error("Quick book Company name & MGL company name are not same.");
                return false;
            } */
            entityService.LinkCustomerQB($scope.ContactID, qbid,qbcustomername).then(
                 function (output) {
                     var outputData = {}
                     outputData.action = 'reload';
                     $uibModalInstance.close(outputData);

                 },
                 function (ex) {
                     debugger;
                     alert("error");
                      ngNotifier.logError(output);
                 }
             );
        };
        $("#txtcustname").val($scope.CustomerName);
        $scope.closeModel = function (action) {
            $scope.customerListQB = [];
            var outputData = {}            
            outputData.action = 'close';
            $uibModalInstance.close(outputData);
        };

        $scope.SearchCustomerQB = function () {
            var t = $("#txtcustname").val();
            entityService.SearchCustomerQB(t).then(
                function (output) {
                    $scope.customerListQB = output.data.data;

                },
                function (ex) {
                    debugger;
                   // alert("error");
                     ngNotifier.logError(output);
                }
            );
        };

        angular.extend(this, new modalController($scope, $filter, $timeout, $routeParams, $uibModal,$uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData));

    };

    controller.$inject = injectParams;

    app.register.controller("crmqbcustpopupModelController", controller);

});
