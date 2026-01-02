"use strict";

define(["app"], function (app) {


    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "customerContactService", "requestData"];
    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData) {
        
        $scope.selectedCountries = [];
        $scope.page = {};
        $scope.page.sortField = 'ID';
        $scope.page.sortType = 'ASC';
        $scope.page.moduleId = 1;
       
        $("#btnaddupate").html('Add');
        $scope.createdBy = requestData.createdBy;
        $scope.entity = {};
        $scope.entity.ID = 0;
        $scope.citiesList = [];
        $scope.lookups = { siplContinents: [], countries: [], origincountries: [], states: [], lgvwstates: [], commoditys: [], contactCategories: [], forwarderNetwork: [], cities: [], companyGradations: [], users: [], accountCategories: [], sites: [], idNumberType: [] };
        $scope.lookups.siplcountries = requestData.country;
        $scope.lookups.lgvwstates = requestData.state;
        $scope.lookups.siplContinents = requestData.siplContinents;
        $scope.selCustomerName = '';
        $scope.entity.contactID = $scope.ContactID = (requestData.ContactID) ? requestData.ContactID : 0;
        $scope.selCustomerName = $scope.CustomerName = (requestData.CustomerName) ? requestData.CustomerName : "";
        $scope.entity.CustomerName = $scope.CustomerName;
        $scope.UserId = (requestData.UserId) ? requestData.UserId : 0;
        $scope.searchParam = {
            address: '',
            contactid: $scope.entity.contactID
        };

        $scope.progress = function () {
            $scope.dynamic = $scope.dynamic + 10;
        };
        $scope.customerContactlistTable = new NgTableParams(
           {
               page: 1,
               count: 10,
               sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")
           }, {
               getData: function (params) {
                   var listParams = {
                       ModuleId: $scope.page.moduleId,
                       PageIndex: params.page(),
                       PageSize: params.count(),
                       Sort: JSON.stringify(params.sorting()),
                       Filter: JSON.stringify($scope.searchParam)
                   };

                   var dataitems = entityService.listAddress(listParams).then(
                       function (output) {
                           //  $scope.validateUser(output);
                           $scope.items = output.data.data;
                           params.total(output.data.count);

                           //return output.data.data;
                       },
                       function (output) {
                           ngNotifier.showError($scope.authentication, output);
                       }
                   );

               }
           });

        $scope.getCities = function (stateid) {

            var getCitiesvalues = entityService.getCities(stateid).then(
                function (output) {
                    $scope.citiesList = [];
                    //$scope.citiesList = output.data.data;  
                    if (output.data.data != null) {
                        output.data.data.forEach(function (item) {
                            $scope.citiesList.push(item);
                        });
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };
        $scope.closeModel = function (action) {
            $scope.customerListQB = [];
            var outputData = {}
            outputData.action = 'close';
            $uibModalInstance.close(outputData);
        };

        $scope.deleteAddressDetail = function (id) {

            entityService.deleteAddressDetail(id).then(
                function (output) {
                    $scope.customerContactlistTable.reload();                    
                    ngNotifier.show(output.data);

                },
                function (ex) {


                   
                    // alert("error");
                    ngNotifier.logError(output);
                }
            );
        };
        $scope.getAddressDetail = function (id) {
           
            entityService.getAddressDetail(id).then(
                function (output) {
                    $("#btnaddupate").html('Update');

                    $scope.entity = output.data.data;
                    $scope.entity.ID = id;
                    var telphArrary = new Array();
                    var celphArrary = new Array();
                    var faxArrary = new Array();
                    if ($scope.entity.telNo != null && $scope.entity.telNo != "" && $scope.entity.telNo.indexOf("|") != "-1") {
                        telphArrary = $scope.entity.telNo.split("|")
                        $scope.entity.telNo1 = telphArrary[0];
                        $scope.entity.telNo2 = telphArrary[1];
                        $scope.entity.telNo3 = telphArrary[2];
                    }
                    else {
                        $scope.entity.telNo1 = $scope.entity.telNo;
                    }
                    if ($scope.entity.cellNo != null && $scope.entity.cellNo != "" && $scope.entity.cellNo.indexOf("|") != "-1") {
                        celphArrary = $scope.entity.cellNo.split("|")
                        $scope.entity.cellNo1 = celphArrary[0];
                        $scope.entity.cellNo2 = celphArrary[1];
                        $scope.entity.cellNo3 = celphArrary[2];
                    }
                    else {
                        $scope.entity.cellNo1 = $scope.entity.cellNo;
                    }
                    if ($scope.entity.fax != null && $scope.entity.fax != "" && $scope.entity.fax.indexOf("|") != "-1") {
                        faxArrary = $scope.entity.fax.split("|")
                        $scope.entity.fax1 = faxArrary[0];
                        $scope.entity.fax2 = faxArrary[1];
                        $scope.entity.fax3 = faxArrary[2];
                    }
                    else {
                        $scope.entity.fax1 = $scope.entity.fax;
                    }
                    $scope.selectedCountries = [];
                    $scope.entity.stateId = output.data.data.stateId;
                    $scope.entity.countryId = output.data.data.countryId;
                    $scope.entity.cityId = output.data.data.cityId;

                },
                function (ex) {


                   
                    // alert("error");
                    ngNotifier.logError(output);
                }
            );
        };

        $scope.saveCustomerContactAddrees = function (source, fromList) {
            $scope.$broadcast("show-errors-check-validity");
          
            //fill additional contact detail
            //var contactDetailItem = {};

           

            $scope.entity.additionalContactDTOList = [];            
            $scope.entity.contactBranchDetailDTOList = [];
           
           
            $scope.entity.contactCommodityDTOList = [];        

          
            var originItem = {};
            $scope.entity.contactOrigionDTOList = [];
            var selectedCountries = $("select[name=countryIds]").val();
            selectedCountries.forEach(function (o) {
                originItem = {};
                originItem.origionID = o;
                $scope.entity.contactOrigionDTOList.push(originItem);
            });
            if ($scope.entity.telNo1 != null) { $scope.entity.telNo = $scope.entity.telNo1 + "|"; }
            else { $scope.entity.telNo = "|"; }
            if ($scope.entity.telNo2 != null) { $scope.entity.telNo += $scope.entity.telNo2 + "|"; }
            else { $scope.entity.telNo += "|"; }
            if ($scope.entity.telNo3 != null) { $scope.entity.telNo += $scope.entity.telNo3; }


            if ($scope.entity.cellNo1 != null) { $scope.entity.cellNo = $scope.entity.cellNo1 + "|"; }
            else { $scope.entity.cellNo = "|"; }
            if ($scope.entity.cellNo2 != null) { $scope.entity.cellNo += $scope.entity.cellNo2 + "|"; }
            else { $scope.entity.cellNo += "|"; }
            if ($scope.entity.cellNo3 != null) { $scope.entity.cellNo += $scope.entity.cellNo3; }


            if ($scope.entity.fax1 != null) { $scope.entity.fax = $scope.entity.fax1 + "|"; }
            else { $scope.entity.fax = "|"; }
            if ($scope.entity.fax2 != null) { $scope.entity.fax += $scope.entity.fax2 + "|"; }
            else { $scope.entity.fax += "|"; }
            if ($scope.entity.fax3 != null) { $scope.entity.fax += $scope.entity.fax3; }


            $scope.entity.siteId = 0;
            $scope.entity.createdBy = $scope.createdBy;
            $scope.entity.ModifiedBy = $scope.createdBy;
            $scope.entity.contactID = (requestData.ContactID);
           
                        entityService.saveCustomerContactAddress($scope.entity).then(
                            function (output) {
                                $scope.contactID = output.data.data.contactID;
                                
                                $scope.entity = {};
                                $scope.contactList = [];
                                $scope.branchList = [];
                                $scope.selectedCommodities = [];
                                $scope.selectedCountries = [];
                                $("#btnaddupate").html('Add');                               
                                $scope.entity.ID = 0;
                                $scope.customerContactlistTable.reload();
                              //  $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                                $scope.editMode = false;
                                $scope.disabledInsert = true;
                                $scope.disabledUpdate = true;
                                $scope.requiredInsert = false;
                                $scope.requiredUpdate = false;
                            });
                    
        };
        
        $scope.uploadedFile = "";
        $scope.uploadExcel = function (file) {
            $scope.uploadedFile = file;
        };
        $scope.importContact = function () {
                if (!$scope.uploadedFile) {
                    alert('Please upload an Excel file first.');
                    return;
            }
           
            entityService.uploadImportFile($scope.uploadedFile, $scope.UserId ).then(
                function (output) {
                    $scope.uploadedFile = "";
                    alert("Record updated successfully.");
                    var blob = new Blob([output.data], {
                        type: "application/octet-stream"
                    });
                    saveAs(blob, 'import.xlsx');
                    $scope.isProgessBarVisible = false;
                },
                function (output) {
                    $scope.uploadedFile = "";
                  alert(output.data.output.messages[0]);
            });

        };
        $scope.closeModelContact = function (action) {
          
            $uibModalInstance.close();
        };
        angular.extend(this, new modalController($scope, $filter, $timeout, $routeParams, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, entityService, requestData));

    };

    controller.$inject = injectParams;

    app.register.controller("addcustaddresspopupModelController", controller);

});
