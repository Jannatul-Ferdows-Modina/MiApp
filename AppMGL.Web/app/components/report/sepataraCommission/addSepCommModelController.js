"use strict";

define(["app"], function (app) {

   
    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "sepataraReportService", "requestData"];
    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal,$uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData) {
   // var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {
       
        var decimalPattern = /^-?\d*\.?\d*$/;       
        $scope.companyname = (requestData.companyname) ? requestData.companyname : '';
        $scope.sepatrapartnername = (requestData.sepatrapartnername) ? requestData.sepatrapartnername : '';
        $("#lblsepcomp").val($scope.sepatrapartnername);
        $("#lblcomp").val($scope.sepatrapartnername);
        $scope.docid = (requestData.docid) ? requestData.docid : 0;
        $scope.commission = $("#txtcustname").val();
        $scope.addcomm = function () {

            if ($("#txtcustname").val() && !/^(-?\d+(\.\d+)?)$/.test($("#txtcustname").val())) {
                $('#errorMsg').show();
                $("#txtcustname").addClass('error');
                return false;
            } else {
                $('#errorMsg').hide();
                $("#txtcustname").removeClass('error');
            }

            entityService.addcomm($scope.docid, $scope.commission).then(
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

       

        angular.extend(this, new modalController($scope, $filter, $timeout, $routeParams, $uibModal,$uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData));

    };

    controller.$inject = injectParams;

    app.register.controller("addSepCommModelController", controller);

});
