"use strict";

define(["app"], function (app) {


    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "crmenquiryService", "requestData"];
    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData) {
        $scope.siteid = requestData.siteid;
        $scope.enqentity = requestData.entity;
        $scope.fdata = [];
        $scope.searchParam = {
            companyid: $scope.enqentity.companyid,
            enqeryid: $scope.enqentity.enqueryid   

        };
        //#region Methods
        $scope.bookinglistTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"" + 'BookingNo' + "\": \"" + 'desc' + "\" }"),
            group: {
                sitId: "asc"
            }
        }, {
            getData: function (params) {
                var listParams = {
                    SiteId: $scope.siteid,                   
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),                    
                    EnquiryId: requestData.entity.enquiryID,
                    CompanyId: requestData.entity.companyId
                };

                return  entityService.getBookingHistory(listParams).then(
                    function (output) {

                        
                        params.total(output.data.count);
                        return output.data.data;
                        // $scope.$parent.pageTitle = "Create Quotation";
                        //document.getElementById('divAdd').hidden = "true";                        
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });
        $scope.performBookingAction = function (source, fromList, enquiryID, quotationID, documentCommonID) {

         
           // $scope.showBookingDetail(action, enquiryID, quotationID, documentCommonID);
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/crm/enquiry/bookingdetail.html",
                controller: "bookingdetailController",
                resolve: {
                    requestData: function () {
                        return {
                            siteid: $scope.siteid,
                            enquiryID:enquiryID,
                            quotationID: quotationID,
                            documentCommonID: documentCommonID
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.quatationNo = output.finalQuotations;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        
        $scope.msg = "s";
      
       
        $scope.closeModel = function (action) {
            $scope.SearchQuotationResult = [];
            var outputData = {}
            outputData.action = 'close';
            outputData.resultId = 1001;
            outputData.finalQuotations = $scope.finalQuotations;
            $uibModalInstance.close(outputData);
        };

        $scope.select = function (action) {

            if (action == 'cancel') {
                $scope.cancel = 0;
                return;
            }


            $uibModalInstance.close();
        };


        angular.extend(this, new modalController($scope, $filter, $timeout, $routeParams, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData));

    };

    controller.$inject = injectParams;

    app.register.controller("viewbookingController", controller);

});
