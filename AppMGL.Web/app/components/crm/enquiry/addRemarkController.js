"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "authService", "crmenquiryService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService, requestData) {

        $scope.startDate = (requestData.startDate) ? requestData.startDate : '';
        $scope.endDate = (requestData.endDate) ? requestData.endDate : '';
        $scope.selectOption = (requestData.selectOption) ? requestData.selectOption : '';
        $scope.searchBox = (requestData.searchBox) ? requestData.searchBox : '';
        $scope.SiteId = (requestData.SiteId) ? requestData.SiteId : '0';
        $scope.enqid = (requestData.enqid)


        $scope.closemodal = function (action)
        {
            $uibModalInstance.close();
        }
        $scope.select = function (action) {
            if (action == 'add') {

                if ($.trim($scope.addRemark) == '')
                {
                    ngNotifier.error("Please enter Note");
                    return;
                }
                if ($.trim($scope.DateRemarks) == '') {
                    ngNotifier.error("Please enter Next Action Date");
                    return;
                }
                var listParams = {

                    remark: $scope.addRemark || "",
                    remarkdate: $scope.DateRemarks || "",
                    EnquiryId: $scope.enqid
                };
                entityService.addenqremark(listParams).then(
                    function (output) {
                        ngNotifier.success("Successfully added note");
                        closepopup();

                    },
                    function (output) {
                        $scope.uploadedFile = "";
                        alert(output.data.output.messages[0]);
                    });
            }
            else {
                closepopup();

            }
        };
        
       function  closepopup(){
            $uibModalInstance.close();

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/crm/enquiry/nextActionDateList.html",
                controller: "nextdateModelController",
                resolve: {
                    requestData: function () {
                        return {
                            startDate: $scope.startDate,
                            endDate: $scope.endDate,
                            selectOption: $scope.selectOption,
                            searchBox: $scope.searchBox,
                            SiteId: $scope.SiteId
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {

                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

       
        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService));

    };
    controller.$inject = injectParams;
    app.register.controller("addRemarkController", controller);
});
