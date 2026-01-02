"use strict";

define(["app"], function (app) {


    var injectParams = [ "$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "warehousemappingService", "requestData"];
    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData) {
        $scope.lookups = { contact: [] };
        $scope.items = [];
        $scope.selectedQuotations = [];
        $scope.finalQuotations = '';
        $scope.selquo = function()
        {
            
            $('.selquo').each(function () {
                var quotationNo = $(this).closest('tr').find('td:eq(3)').text().trim();
                if (this.checked)
                    $scope.selectedQuotations.push(quotationNo);
                else
                    $scope.selectedQuotations.pop(quotationNo);
            });

            $scope.finalQuotations = $scope.selectedQuotations.toString();
        }

        $scope.callTypeaheadQuo = function (viewValue, lookupModule, lookupField1, lookupMethod, fieldType, lookupField2, valueType) {

            var criteria = [];

            if ($scope.setLookupCriteria != undefined) {
                criteria = $scope.setLookupCriteria(lookupModule);
            }

            fieldType = (fieldType || "string");

            if (valueType != null) {
                criteria.push(Utility.createFilter(lookupField1, fieldType, lookupField1, viewValue, "startWith"));
                criteria.push(Utility.createFilter(lookupField2, fieldType, lookupField2, valueType, "equalTo"));
            } else {
                criteria.push(Utility.createFilter(lookupField1, fieldType, lookupField1, viewValue, "startWith", null));
            }


            var listParams = {
                SiteId: $scope.siteid,
                CwtId: 1,
                ModuleId: 1,
                PageIndex: 1,
                PageSize: 25,
                Sort: "{ \"" + lookupField1 + "\": \"asc\" }",
                Filter: JSON.stringify(criteria)
            };

            return entityService.getCompanySearch(listParams).then(
                function (output) {                    
                    return output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        $scope.performQuotationSearch = function (source, selectOption, searchBox) {

            var action ='';
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox,
                dashboardOption: "",
                isdraft: 1
            };
            $scope.quotationlistTable.reload();
        };

        $scope.selectOption = "companyName";
        $scope.searchBox = "";

        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            dashboardOption: $scope.dashboardOption,
            isdraft: $scope.isdraft

        };
        //#region Methods
        $scope.quotationlistTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"" + 'EnquiryId' + "\": \"" + 'desc' + "\" }")
        }, {
            getData: function (params) {
                var listParams = {
                    SiteId: $scope.siteid,
                    ModuleId: 1,
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),
                    Filter: JSON.stringify($scope.searchParam)
                };

                var dataitems = entityService.searchquotationlist(listParams).then(
                    function (output) {
                        
                        $scope.items = output.data.data;
                        params.total(output.data.count);
                       // $scope.$parent.pageTitle = "Create Quotation";
                        //document.getElementById('divAdd').hidden = "true";                        
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });






        $scope.searchValues = function (viewValue, selectOption) {
            var resultItem = {};
            if (selectOption == "customer") {
                return $scope.callTypeaheadQuo(viewValue, 'SIPLContact', 'companyName', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.companyName;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
        }
        $scope.searchOptions = [
 { optionValue: "", optionName: "-All-" },
 { optionValue: "customer", optionName: "Customer" },
 { optionValue: "quotationNo", optionName: "Quotation No" },
 { optionValue: "enquiryNo", optionName: "Enquiry No" }
        ];
        $scope.selectOption = 'customer';
        $scope.siteid = requestData.siteid;
        
        if (requestData.finalQuotations != undefined) {
            $scope.finalQuotations = requestData.finalQuotations;
            $scope.selectedQuotations = requestData.finalQuotations.split(',');
        }

       


        $scope.msg = "s";
        $scope.SearchQuotationResult = [];
        $scope.SearchQuotationResult = function () {




            var t = $("#txtcustname").val();
            entityService.SearchQuotation(t).then(
                function (output) {
                    $scope.SearchQuotationResult = output.data.data;
                },
                function (ex) {

                    // alert("error");
                    ngNotifier.logError(output);
                }
            );
        };
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


        angular.extend(this, new modalController( $scope, $filter, $timeout, $routeParams, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData));

    };

    controller.$inject = injectParams;

    app.register.controller("searchQuotationController", controller);

});
