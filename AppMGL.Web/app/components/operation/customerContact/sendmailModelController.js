"use strict";

define(["app"], function (app) {

   
    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "customerContactService", "requestData"];
    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal,$uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData) {
  
        $scope.ContactID = (requestData.ContactID) ? requestData.ContactID : 0;
        $scope.emailTo = (requestData.ContactEmail) ? requestData.ContactEmail : '';
        $scope.userId = (requestData.userId) ? requestData.userId : '';
        $scope.emailBody = "";
        $scope.emailSubject = "";
        $scope.Emailcc = "";
        
        entityService.getEmailDetail($scope.ContactID).then(
            function (output) {
                $scope.emailBody = output.data.data[0].emailBody;
                $scope.emailSubject = output.data.data[0].emailSubject;
                $scope.emailcc = output.data.data[0].emailcc;
               
            },
            function (output) {
                ngNotifier.showError($scope.authentication, output);
            });

        
        
        $scope.closeModel = function (action) {
            $scope.customerListQB = [];
            var outputData = {}            
            outputData.action = 'close';
            $uibModalInstance.close(outputData);
        };
        $scope.sendEmail = function (msg) {
            if ($("#emailTo").val() == undefined || $("#emailTo").val() == "") {
                ngNotifier.error("Please enter To Emailid");
                return;
            }
            if ($("#emailSubject").val() == undefined || $("#emailSubject").val() == "") {
                ngNotifier.error("Please enter Email Subject");
                return;
            }
            var emailEntity = {};
            emailEntity.emailTo = $("#emailTo").val();
            emailEntity.createdBy = $scope.userId;
            if ($("#emailcc").val() != '') {
                emailEntity.emailcc = $("#emailcc").val();
            }
            if ($("#emailBcc").val() != '') {
                emailEntity.emailBcc = $("#emailBcc").val();
            }
            emailEntity.emailSubject = $("#emailSubject").val();
            emailEntity.emailBody=CKEDITOR.instances['emailBody'].getData();
            entityService.sendEmail(emailEntity).then(
                function (output) {
                   
                    ngNotifier.show(output.data);
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });

        };
       
        angular.extend(this, new modalController($scope, $filter, $timeout, $routeParams, $uibModal,$uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData));

    };

    controller.$inject = injectParams;

    app.register.controller("sendmailModelController", controller);

});
