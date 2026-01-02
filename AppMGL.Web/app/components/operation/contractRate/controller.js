"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "contractRateService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
       // $scope.page.moduleId = 2080;
        $scope.page = appUrl.contractRate;
        $scope.tabs = appUrl.contractRate.tabs;

        $scope.CarrierF = "";
        $scope.ContractNoF = "",
        $scope.StartDateF = "",
        $scope.EndDateF = "",
       $scope.searchParam = {
           Carrier: $scope.CarrierF,
           fromDate: $scope.StartDateF,
           toDate: $scope.EndDateF,
           ContractNo: $scope.ContractNoF
       };
        $scope.criteria = {
            Carrier: $scope.CarrierF,
            fromDate: $scope.StartDateF,
            toDate: $scope.EndDateF,
            ContractNo: $scope.ContractNoF
        };

        $scope.setLookups = function (source, lookup, output, index) {
            
            debugger;
            if (lookup == "SIPLContactAdd") {
                $scope.entity.carrierID = output.data[0].contactID;
            }
        };

        $scope.clearLookups = function (source, lookup, index) {

            
            if (lookup == "SIPLContactAdd") {
                $scope.entity.carrierID = null;

            }
        };



        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
                $scope.entity.createdBy = $scope.$parent.userInfo.usrId;
                $scope.entity.siteId = $scope.$parent.selectedSiteId
            }
            else {
                $scope.entity.modifiedOn = new Date();
                $scope.entity.modifiedBy = $scope.$parent.userInfo.usrId
            }
        };

        $scope.lookups = { siplContact: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("siplContact", 28, "companyName", "siplContact", null);           

            $scope.mQCTypes = [
                { mqctype: 0, optionName: "TEU" },
                { mqctype: 1, optionName: "FEU" }
            ];
            
        };

       
       
        $scope.callContractRateModal = function (source) {

             $scope.$parent.selectedSiteId
             $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/contractRateModal/detail.html",
                controller: "contractRateModalController",
                resolve: {
                    requestData: function () {

                        return {
                            contractID: (source || 0),
                            SitId: $scope.$parent.selectedSiteId
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.contractID = output.data.contractID;

                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        
      


     
      
        

        $scope.performActionSearch = function (source, CarrierF, ContractNoF, StartDateF, EndDateF) {

            var action = source.currentTarget.attributes["action"].value;
           $scope.CarrierF = CarrierF;
            $scope.StartDateF = StartDateF;
            $scope.EndDateF = EndDateF;
            $scope.ContractNoF = ContractNoF;
            $scope.searchParam = {
                Carrier: CarrierF,
                fromDate: StartDateF,
                toDate: EndDateF,
                ContractNo: ContractNoF               
            };

            $scope.criteria = {
                Carrier: CarrierF,
                fromDate: StartDateF,
                toDate: EndDateF,
                ContractNo: ContractNoF
            };          
            $scope.performAction(source);
            
        };

        var exportReport = function (CarrierF, ContractNoF, StartDateF, EndDateF) {

            $scope.searchParam = {
                Carrier: CarrierF,
                fromDate: StartDateF,
                toDate: EndDateF,
                ContractNo: ContractNoF
            };
            var reportParams = {
                SiteId: $scope.$parent.selectedSiteId,
                ModuleId: $scope.page.moduleId,
                PageIndex: 1,
                PageSize: 10000,
                Sort: JSON.stringify($.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")),

                //Carrier:$scope.CarrierF,
                //fromDate: $scope.StartDateF,
                //toDate: $scope.EndDateF,
                //ContractNo: $scope.ContractNoF
                Filter: JSON.stringify($scope.searchParam)
            };
            entityService.exportReport(reportParams).then(
                function (output) {
                    var blobData = new Blob([output.data], { type: output.headers()["content-type"] });
                    var fileName = output.headers()["x-filename"];
                    saveAs(blobData, fileName);
                },
                function (output) {
                    ngNotifier.error(output);
                }
            );
        };

        $scope.performSubAction = function (source, target, CarrierF, ContractNoF, StartDateF, EndDateF) {

            var action = source.currentTarget.attributes["action"].value;

            if ($scope.entity.type == 0) {
                ngNotifier.error("First, select any one type.");
                return;
            }

            switch (action) {
                case "showReport":
                    $scope.reportTable.reload();
                    break;
                case "exportReport":
                    exportReport(CarrierF, ContractNoF, StartDateF, EndDateF);
                    break;
                default:
                    //TODO
                    break;
            }
        };
        
        //#endregion

       


     angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("contractRateController", controller);

});
