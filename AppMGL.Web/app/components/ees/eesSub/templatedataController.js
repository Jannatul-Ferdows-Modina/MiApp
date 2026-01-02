"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "authService", "eessubService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService, requestData) {

        $scope.siteId = (requestData.siteId) ? requestData.siteId : 0;
        //$scope.TemplateList = (requestData.TemplateList) ? requestData.TemplateList : '';
        $scope.dcommonid = (requestData.dcommonid) ? requestData.dcommonid : '';
        $scope.templateType = (requestData.templateType) ? requestData.templateType : '';
        $scope.items = [];
        $scope.closemodal = function (action) {
            $uibModalInstance.close();
        };
        $scope.SelectTemplate = function (documentCommonId,id) {
            var outputdata = {};
            outputdata.dcommonid = documentCommonId;
            outputdata.id = id;
            $uibModalInstance.close(outputdata);
            
        };
        $scope.searchOptions1 = [
            { optionValue: "", optionName: "-All-" },
            { optionValue: "USPPICompanyName", optionName: "USPPI Company Name" },
            { optionValue: "PortofExport", optionName: "Port of Export" },
            { optionValue: "PortofUnloading", optionName: "Port of Unloading" },
            { optionValue: "OriginState", optionName: "Origin State" }
        ];
        $scope.selectOption1 = "USPPICompanyName";
        $scope.searchBox1 = "";
        $scope.searchParam1 = {
            optionValue: $scope.selectOption1,
            seachValue: $scope.searchBox1
        };
        $scope.searchOptions = [
            { optionValue: "", optionName: "-All-" },
            { optionValue: "companyName", optionName: "Company Name" },
            { optionValue: "FirstName", optionName: "First Name" },
            { optionValue: "LastName", optionName: "Last Name" }
        ];
      
        $scope.selectOption = "companyName";
        $scope.searchBox = "";
        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            templateType: $scope.templateType
        };
        $scope.partyTemplateSearch = function (source, selectOption, searchBox) {
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox,
                templateType: $scope.templateType
               
            };
            $scope.partyTemplatelistTable.reload();
        };
        $scope.partyTemplatelistTable = new NgTableParams(
            {
                page: 1,
                count: 10,
                sorting: $.parseJSON("{ \"" + 'EnquiryId' + "\": \"" + 'desc' + "\" }")
            }, {
            getData: function (params) {
                var listParams = {
                    SiteId: $scope.siteId,
                    ModuleId: 1,
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),
                    Filter: JSON.stringify($scope.searchParam)
                };

                    var dataitems = entityService.GetPartTemplateList(listParams).then(
                    function (output) {

                        $scope.items = output.data.data;
                        params.total(output.data.count);
                                             
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });

        $scope.getAesTemplateDetail = function (id) {
           
            $scope.entity = {};
            $scope.entity.templateId = id;
            entityService.getAesTemplateDetail($scope.entity).then(
                function (output) {
                    if (output.data.resultId == 2005) {
                        ngNotifier.showError($scope.authentication, output);
                        $scope.logOut()
                    }

                    if (output.data.data != null) {
                        $uibModalInstance.close(output.data.data);
                    }


                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        $scope.templatelistTable = new NgTableParams(
            {
                page: 1,
                count: 10,
                sorting: $.parseJSON("{ \"" + 'EnquiryId' + "\": \"" + 'desc' + "\" }")
            }, {
            getData: function (params) {
                var listParams = {
                    SiteId: $scope.siteId,
                    ModuleId: 1,
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),
                    Filter: JSON.stringify($scope.searchParam1)
                };

                    var dataitems = entityService.GetTemplateList(listParams).then(
                    function (output) {

                        $scope.items = output.data.data;
                        params.total(output.data.count);

                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });

        $scope.templateSearch = function (source, selectOption1, searchBox1) {
            $scope.searchParam1 = {
                optionValue: selectOption1,
                seachValue: searchBox1
            };
            $scope.templatelistTable.reload();
        };
        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService));

    };
    controller.$inject = injectParams;
    app.register.controller("templatedataController", controller);
});
