"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "authService", "crmenquiryService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams,ngNotifier, authService, entityService, requestData) {
       
        $scope.startDate = (requestData.startDate) ? requestData.startDate :'';
        $scope.endDate = (requestData.endDate) ? requestData.endDate : '';
        $scope.selectOption = (requestData.selectOption) ? requestData.selectOption : '';
        $scope.searchBox = (requestData.searchBox) ? requestData.searchBox : '';
        $scope.SiteId = (requestData.SiteId) ? requestData.SiteId : '0';
        $scope.closemodal = function (action) {
            
            $uibModalInstance.close();
        };
       $scope.reportTable = new NgTableParams(
                {

                    page: 1,
                    count: 10,
                    sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }"),
                    group: {
                        quotationNo: "asc"
                    }
                }, {
                counts: [],
                getData: function (params) {

                    var listParams = {

                        StartNextActionDate: $scope.startDate || "",
                        EndNextActionDate: $scope.endDate || "",
                        selectOption: $scope.selectOption,
                        searchBox: $scope.searchBox,
                        SiteId:$scope.SiteId,
                    };
                    return entityService.getNextActionDateList(listParams).then(
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
        $scope.ExportToExcelRemarks = function () {
            var listParams = {

                StartNextActionDate: $scope.startDate || "",
                EndNextActionDate: $scope.endDate || "",
                selectOption: $scope.selectOption,
                searchBox: $scope.searchBox,
                SiteId: $scope.SiteId,

            };
            entityService.exportToExcelRemarks(listParams).then(
                function (output) {
                   
                    var blob = new Blob([output.data], {
                        type: "application/octet-stream"
                    });
                    saveAs(blob, 'NextActionDateList.xlsx');
                    
                },
                function (output) {
                    $scope.uploadedFile = "";
                    alert(output.data.output.messages[0]);
                });
        };

        $scope.addremarks= function(obj)
        {
            var enqid = obj.enquiryID;
            $uibModalInstance.close();
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/crm/enquiry/addremark.html",
                controller: "addRemarkController",
                resolve: {
                    requestData: function () {
                        return {
                            startDate: $scope.startDate,
                            endDate: $scope.endDate,
                            selectOption: $scope.selectOption,
                            searchBox: $scope.searchBox,
                            SiteId: $scope.SiteId,
                            enqid: enqid
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


        }
      angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService));

    };
    controller.$inject = injectParams;
    app.register.controller("nextdateModelController", controller);
});
