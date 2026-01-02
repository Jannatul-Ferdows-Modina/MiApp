"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "quickbooksyncService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {
        $scope.entity = {};
        $scope.qbid = (requestData.qbid) ? requestData.qbid : 0;
        $scope.contactId = (requestData.contactId) ? requestData.contactId : 0;
        $scope.lookups = { siplContinents: [], countries: [], origincountries: [], states: [], lgvwstates: [], commoditys: [], categories: [], cities: [], companyGradations: [], users: [], accountCategories: [] };
        
        $scope.initDropdown = function () {
           
            $scope.fetchLookupData("continent", 0, "name", "siplContinents", null);
            $scope.fetchLookupData("siplcountry", 0, "name", "siplcountries", null);
            $scope.fetchLookupData("lgvwstate", 0, "name", "lgvwstates", null);
            $scope.fetchLookupData("lgvwcity", 0, "name", "cities", null);
            
        };

        $scope.afterFetchLookupData = function (lookupKey) {

            if (lookupKey == "countries") {
                if ($scope.lookups.countries != null) {

                    $scope.lookups.countries.unshift({ "cryId": "", "cryName": "" });
                } else {
                    alert('countries null');
                }
            } 

            if (lookupKey == "origincountries") { $scope.lookups.origincountries.unshift({ "cryId": "", "cryName": "" }); } 
            if (lookupKey == "states") { $scope.lookups.states.unshift({ "ustId": "", "ustName": "" }); }
            if (lookupKey == "cities") { $scope.lookups.cities.unshift({ "cityId": "", "name": "" }); }
           
        };

        $scope.setOptionValue = function (source) {
            if (source == "branchRegion") {
                $scope.entity.branchRegion = null;
                $scope.entity.branchCountry = null;
                $scope.entity.branchCountryID = null;
                $scope.entity.branchState = null;
                $scope.entity.branchStateID = null;
                $scope.entity.branchCity = null;
                $scope.entity.branchCityID = null;
                $scope.lookups.siplContinents.forEach(function (o) {
                    if (o.continentId == $scope.entity.branchRegionID) {
                        $scope.entity.branchRegion = o.name;
                        return;
                    }
                });
            }
            else if (source == "branchCountry") {
                $scope.entity.branchCountry = null;
                $scope.entity.branchState = null;
                $scope.entity.branchStateID = null;
                $scope.entity.branchCity = null;
                $scope.entity.branchCityID = null;
                $scope.lookups.siplcountries.forEach(function (o) {
                    if (o.countryId == $scope.entity.branchCountryID) {
                        $scope.entity.branchCountry = o.name;
                        return;
                    }
                });
            }
            else if (source == "branchState") {
                $scope.entity.branchState = null;
                $scope.entity.branchCity = null;
                $scope.entity.branchCityID = null;
                $scope.lookups.lgvwstates.forEach(function (o) {
                    if (o.stateId == $scope.entity.branchStateID) {
                        $scope.entity.branchState = o.name;
                        return;
                    }
                });
            }
            else if (source == "branchCity") {
                $scope.entity.branchCity = null;
                $scope.lookups.cities.forEach(function (o) {
                    if (o.cityId == $scope.entity.branchCityID) {
                        $scope.entity.branchCity = o.name;
                        return;
                    }
                });
            }
        }
        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "continent") {
                $scope.entity.continentId = output.data[0].continentId;
            }
            else if (lookup == "countryName") {
                $scope.entity.countryId = output.data[0].countryId;
            }
            else if (lookup == "stateName") {
                $scope.entity.stateId = output.data[0].stateId;
            }
            else if (lookup == "city") {
                $scope.entity.cityId = output.data[0].cityId;
            }
           
        }
        $scope.clearLookups = function (source, lookup, index) {
            if (lookup == "continent") {
                $scope.entity.continentId = null;
            }
            else if (lookup == "countryName") {
                $scope.entity.countryId = null;
            }
            else if (lookup == "stateName") {
                $scope.entity.stateId = null;
            }
            else if (lookup == "city") {
                $scope.entity.cityId = null;
            }
            
        }
       
        
        $scope.beforeSave = function (action, lastAction) {
            
            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
            }
            else {
                $scope.entity.modifiedOn = new Date();
            }
        };
        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
           
            switch (action) {
                case "add":
                    $scope.entity.continentID = 2;
                    break;
            }

        }
        //#endregion
       
        $scope.getDetailData = function () {

            if ($scope.contactId > 0) {
                entityService.getDetailData($scope.contactId).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            $scope.entity = output.data.data;
                            ngNotifier.showError1(output.data.messages[0]);
                            $scope.logOut()
                        }
                        $scope.entity = output.data.data;
                        if ($scope.afterGetDetail != undefined) {
                            $scope.afterGetDetail();
                        }
                        
                    },
                    function (output) {
                        $scope.entity = output.data.data;
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            } else {
                $scope.goBack();
            }
        };
        if ($scope.contactId != undefined && $scope.contactId != '' && $scope.contactId != null) {

            $scope.getDetailData($scope.entityId);
        }
        $scope.SaveCompairData = function (type) {
             if (type =="App")
                 $scope.entity.isquickbook = 1;
             else
                $scope.entity.isquickbook = 0;
            if (type == "QuickBook") {
                if ($scope.entity.quickCompanyName == "" || $scope.entity.quickCompanyName == null) {
                    alert("Please enter customer name.");
                    return false;
                }
                if ($scope.entity.quickContactPerson == "" || $scope.entity.quickContactPerson == null) {
                    alert("Please enter Contact Person.");
                    return false;
                }
                if ($scope.entity.quickContactNumber == "" || $scope.entity.quickContactNumber == null) {
                    alert("Please enter Contact number.");
                    return false;
                }
                if ($scope.entity.qbid == "" || $scope.entity.qbid == null || $scope.entity.qbid == "0") {
                    alert("Quick book data not fatched, please try again by compair data.");
                    return false;
                }
            }
            if (type == "App") {
                if ($scope.entity.companyName == "" || $scope.entity.companyName == null) {
                    alert("Please enter customer name");
                    return false;
                }
                if ($scope.entity.contactPerson == "" || $scope.entity.contactPerson == null) {
                    alert("Please enter Contact Person");
                    return false;
                }
                if ($scope.entity.telNo == "" || $scope.entity.telNo == null) {
                    alert("Please enter Contact number");
                    return false;
                }
            }

            if ($scope.contactId > 0) {
                entityService.saveCompairData($scope.entity).then(
                    function (output) {
                        alert("data update successfully.");
                        $uibModalInstance.close($scope.responseData);
                    },
                    function (output) {
                        ngNotifier.error(output.data.output.messages);
                    }
                )
            }
        };
        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("quickbooksyncController", controller);

});
