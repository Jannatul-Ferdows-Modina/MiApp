"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "customerContactService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.customerContact;
        $scope.tabs = appUrl.customerContact.tabs;
        $scope.contactID;
        $scope.contactList = [];
        $scope.branchList = [];
        $scope.citiesList = [];
        $scope.branchCitiesList = [];
        $scope.searchCitiesList = [];
        $scope.categories = [];
        //#endregion
        var lastAction = "";
        $scope.selectedStateId = 0;
        //#region Lookup

        //var box = $(".btn .moveall .btn-default")
        //box.prop("disabled", true);
        //$(".bootstrap-duallistbox-container").find(".moveall").prop('disabled', true);
        //$(".bootstrap-duallistbox-container").find(".removeall").prop('disabled', true);
        //disabledUpdate
        //lb.parent().find('.moveall').prop('disabled', false);
        //lb.parent().find('.removeall').prop('disabled', false);
        //boxes.

        $scope.lookups = { siplContinents: [], countries: [], origincountries: [], states: [], lgvwstates: [], commoditys: [], contactCategories: [], cities: [], companyGradations: [], users: [], accountCategories: [], sites: [] };

        $scope.initDropdown = function () {
            $scope.fetchLookupData("country", 0, "CryName", "countries", null);
            $scope.fetchLookupData("country", 0, "CryName", "origincountries", null);
            $scope.fetchLookupData("state", 0, "ustName", "states", null);
            $scope.fetchLookupData("commodity", 0, "name", "commoditys", null);
            $scope.fetchLookupData("contactCategory", 0, "name", "contactCategories", null);
            $scope.fetchLookupData("continent", 0, "name", "siplContinents", null);
            $scope.fetchLookupData("siplcountry", 0, "name", "siplcountries", null);
            $scope.fetchLookupData("lgvwstate", 0, "name", "lgvwstates", null);
            //$scope.fetchLookupData("lgvwcity", 0, "name", "cities", null);
            $scope.fetchLookupData("companyGradation", 0, "gradationID", "companyGradations", null);
            $scope.fetchLookupData("sipluser", 0, "name", "users", null);

            var sitAll = $.map($scope.authentication.userSite, function (o) { return o.SitId; }).join(",");
            $scope.lookups.sites = angular.copy($scope.authentication.userSite);
            $scope.lookups.sites.unshift({ SitId: sitAll, SitName: "All" });
            $scope.siteId = sitAll;

            $scope.getAccountCategories();
        };

        $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey == "countries") { $scope.lookups.countries.unshift({ "cryId": "", "cryName": "" }); } //
            if (lookupKey == "origincountries") { $scope.lookups.origincountries.unshift({ "cryId": "", "cryName": "" }); } //
            if (lookupKey == "states") { $scope.lookups.states.unshift({ "ustId": "", "ustName": "" }); }
            if (lookupKey == "commoditys") { $scope.lookups.commoditys.unshift({ "commodityId": "", "name": "" }); }
            //if (lookupKey == "categories") { $scope.lookups.categories.unshift({ "contactCategoryId": "", "name": "" }); }
            if (lookupKey == "cities") { $scope.lookups.cities.unshift({ "cityId": "", "name": "" }); }
            if (lookupKey == "users") { $scope.lookups.users.unshift({ "userId": "", "name": "" }); }  //
            if (lookupKey == "companyGradations") { $scope.lookups.companyGradations.unshift({ "gradationId": "", "companyGradation": "" }); }
            if (lookupKey == "contactCategories") {
                if ($scope.lookups.contactCategories.length > 0) {                    
                    $scope.lookups.contactCategories.forEach(function (o) {
                        if (o.isVendor == false) {
                            $scope.categories.push(o);
                        }
                    });
                };
            };
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
                var getCitiesvalues = entityService.getCities($scope.entity.branchStateID).then(
                function (output) {
                    $scope.branchCitiesList = [];
                    //$scope.citiesList = output.data.data;  
                    if (output.data.data != null) {
                        output.data.data.forEach(function (item) {
                            $scope.branchCitiesList.push(item);
                        });
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
            }
            else if (source == "branchCity") {
                $scope.entity.branchCity = null;
                
                $scope.branchCitiesList.forEach(function (o) {
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
            else if (lookup == "branchState") {
                $scope.entity.branchStateID = output.data[0].stateId;
            }
            else if (lookup == "branchCity") {
                $scope.entity.branchCityID = output.data[0].cityId;
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
            else if (lookup == "branchState") {
                $scope.entity.branchStateID = null;
            }
            else if (lookup == "branchCity") {
                $scope.entity.branchCityID = null;
            }
        }
        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {
            if (lookupModule == "continent" || lookupModule == "countryName" || lookupModule == "stateName" || lookupModule == "city" || lookupModule == "branchState" || lookupModule == "branchCity") {

                if ($scope.entity[lookupModule] == null || $scope.entity[lookupModule] == "") {
                    $scope.clearLookups(source, lookupModule, lookupIndex);
                }
            }
        }
        //#endregion
        $scope.selectedCommodities = [];
        $scope.selectedCountries = [];

        $scope.updateCommodities = function () {
            $scope.selectedCommodities = [];
            if ($scope.entity.commodityIds != null) {
                for (var i = 0; i < $scope.entity.commodityIds.length; i++) {
                    if ($scope.lookups.commoditys != null) {
                        for (var j = 0; j < $scope.lookups.commoditys.length; j++) {
                            if ($scope.lookups.commoditys[j].commodityId == $scope.entity.commodityIds[i]) {
                                $scope.selectedCommodities.push(
                                    {
                                        commodityId: $scope.lookups.commoditys[j].commodityId,
                                        commodityName: $scope.lookups.commoditys[j].name
                                    });
                                break;
                            }
                        }
                    }
                }
            }
        }

        $scope.updateCountries = function () {
            $scope.selectedCountries = [];
            if ($scope.entity.countryIds != null) {
                for (var i = 0; i < $scope.entity.countryIds.length; i++) {
                    if ($scope.lookups.origincountries != null) {
                        for (var j = 0; j < $scope.lookups.origincountries.length; j++) {
                            if ($scope.lookups.origincountries[j].cryId == $scope.entity.countryIds[i]) {
                                $scope.selectedCountries.push(
                                    {
                                        countryId: $scope.lookups.origincountries[j].cryId,
                                        countryName: $scope.lookups.origincountries[j].cryName
                                    });
                                break;
                            }
                        }
                    }
                }
            }

        }

        $scope.getSearchCities = function(stateID)
        {
            $scope.searchCitiesList = [];
            if (stateID != null) {
                var getCitiesvalues = entityService.getCities(stateID).then(
                    function (output) {
                        $scope.searchCitiesList = [];
                        //$scope.citiesList = output.data.data;  
                        if (output.data.data != null) {
                            output.data.data.forEach(function (item) {
                                $scope.searchCitiesList.push(item);
                            });
                        }
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        }
        $scope.customerContactTypes = [
            { optionValue: "", optionName: "Select One" }
        ];
        $scope.searchOptions = [
            { optionValue: "", optionName: "-All-" },
            { optionValue: "CompanyName", optionName: "Company Name" },
            { optionValue: "CustomerCode", optionName: "Customer Code" }//,
        ];
        //$scope.selectOption = "";
        //$scope.searchBox = "";


        $scope.companyNameF = "";
        $scope.customerCodeF = "";
        $scope.galRepresentativeF = "";
        $scope.contactCategoryIDF = "";
        $scope.companyGradationF = "";
        $scope.OriginCountryF = "";
        $scope.commodityF = "";
        $scope.continentF = "";
        $scope.cryNameF = "";
        $scope.stateF = "";
        $scope.cityF = "";
        $scope.isVendor = 0;
        $scope.siteId = $.map($scope.authentication.userSite, function (o) { return o.SitId; }).join(",");

        $scope.searchParam = {
            companyName: $scope.companyNameF,
            customerCode: $scope.customerCodeF,
            galRepresentative: $scope.galRepresentativeF,
            contactCategoryID: $scope.contactCategoryIDF,
            companyGradation: $scope.companyGradationF,
            OriginCountry: $scope.OriginCountryF,
            commodity: $scope.commodityF,
            continent: $scope.continentF,
            cryName: $scope.cryNameF,
            state: $scope.stateF,
            city: $scope.cityF,
            isVendor: $scope.isVendor,
            siteId: $scope.siteId
            //optionValue: $scope.selectOption,
            //optionValue: $scope.searchBox

        };
        //#region Methods
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

                    var dataitems = entityService.list(listParams).then(
                        function (output) {
                            $scope.validateUser(output);
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
        //#endregion
        var initControls = function (action) {

            switch (action) {
                case "add":
                    $scope.editMode = true;
                    $scope.disabledInsert = false;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = true;
                    $scope.requiredUpdate = true;
                    break;
                case "edit":
                    $scope.editMode = true;
                    $scope.disabledInsert = true;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = false;
                    $scope.requiredUpdate = true;
                    break;
                default:
                    $scope.editMode = false;
                    $scope.disabledInsert = true;
                    $scope.disabledUpdate = true;
                    $scope.requiredInsert = false;
                    $scope.requiredUpdate = false;
                    break;
            }
        };

        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
        };

        $scope.getAccountCategories = function () {
            var getFeeCategory = entityService.getAccountCategories().then(
                function (output) {
                    $scope.lookups.accountCategories = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }

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

        //$scope.updateCities = function (stateid) {
        //    $scope.selectedStateId = stateid;
        //    //$scope.fetchLookupData("lgvwcity", 0, "name", "cities", null);
        //    $scope.citiesList = [];
        //    $scope.lookups.cities.forEach(function (item) {
        //        if (item.stateId == stateid) {
        //            $scope.citiesList.push(item);
        //        }
        //    });
        //};

        $scope.performCustomerContactSearch = function (source, companyName, customerCode, galRepresentative, contactCategoryID, companyGradation, OriginCountry, commodity, continent, cryName, state, city,siteId) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                companyName: companyName,
                customerCode: customerCode,
                galRepresentative: galRepresentative,
                contactCategoryID: (contactCategoryID) ? contactCategoryID.join() : '',
                companyGradation: companyGradation,
                OriginCountry: OriginCountry,
                commodity: commodity,
                continent: continent,
                cryName: cryName,
                state: state,
                city: city,
                isVendor: 0,
                siteId: siteId
                //optionValue: selectOption,
                //seachValue: searchBox
            };
            $scope.customerContactlistTable.reload();
        };


        $scope.saveCustomerContact = function (source, fromList) {
            $scope.$broadcast("show-errors-check-validity");


            if ($scope.entity.companyName == null) {
                ngNotifier.error("Please enter Company Name");
                return;
            }
            if ($scope.entity.customerCode == null) {
                ngNotifier.error("Please enter Customer Code");
                return;
            }
            if ($scope.entity.acyID == null) {
                ngNotifier.error("Please select Account Category");
                return;
            }
            if ($scope.entity.addMoreContact) {
                if ($scope.contactList.length == 0) {
                    ngNotifier.error("Please enter Contact details");
                    return;
                }
            }
            if ($scope.entity.branchInfo) {
                if ($scope.branchList.length == 0) {
                    ngNotifier.error("Please enter Branch details");
                    return;
                }
            }
            if ($scope.entity.customerfile != null) {
                if ($scope.entity.customerfile.size > 10485760) {
                    ngNotifier.error("File cannot exceeds more than 10 MB size.");
                    return;
                }
                else if ($scope.entity.customerfile.type != "application/pdf" && $scope.entity.customerfile.type != "application/docx" && $scope.entity.customerfile.type != "application/doc" && $scope.entity.customerfile.type != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && $scope.entity.customerfile.type != "application/xlsx" && $scope.entity.customerfile.type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    ngNotifier.error("Please upload only these file types - pdf,docx,doc,xlsx");
                    return;
                }
                else {
                    $scope.entity.attachment = $scope.entity.customerfile.name;
                }
            }
            //fill categories
            if ($scope.entity.categoryIds != null) {
                for (var i = 0; i < $scope.entity.categoryIds.length; i++) {
                    if (i == 0) {
                        $scope.entity.contactCategoryID = $scope.entity.categoryIds[i];
                    }
                    else {
                        $scope.entity.contactCategoryID = $scope.entity.contactCategoryID + "," + $scope.entity.categoryIds[i];
                    }
                }
            }
            //fill additional contact detail
            //var contactDetailItem = {};
            $scope.entity.additionalContactDTOList = [];
            if ($scope.entity.addMoreContact) {
                if ($scope.contactList.length > 0) {
                    $scope.entity.additionalContactDTOList = $scope.contactList;
                }
            }

            //fill Branch detail
            //var branchDetailItem = {};
            $scope.entity.contactBranchDetailDTOList = [];
            if ($scope.entity.branchInfo) {
                if ($scope.branchList.length > 0) {
                    $scope.entity.contactBranchDetailDTOList = $scope.branchList;
                }
            }

            //fill Commodity details
            var commodityItem = {};
            $scope.entity.contactCommodityDTOList = [];
            //$scope.entity.contactCommodityDTOList = [];
            //if ($scope.selectedCommodities != null) {
            //    for (var i = 0; i < $scope.selectedCommodities.length; i++) {
            //        commodityItem = {};
            //        commodityItem.commodityID = $scope.selectedCommodities[i].commodityId;
            //        $scope.entity.contactCommodityDTOList.push(commodityItem);
            //    }
            //}

            var selectedCommodities = $("select[name=commodityIds]").val();
            selectedCommodities.forEach(function (o) {
                commodityItem = {};
                commodityItem.commodityID = o;
                $scope.entity.contactCommodityDTOList.push(commodityItem);
            });

            //fill origion details
            //var originItem = {};
            //$scope.entity.contactOrigionDTOList = [];
            //if ($scope.selectedCountries != null) {
            //    for (var i = 0; i < $scope.selectedCountries.length; i++) {
            //        originItem = {};
            //        originItem.origionID = $scope.selectedCountries[i].countryId;
            //        $scope.entity.contactOrigionDTOList.push(originItem);
            //    }
            //}
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
            

            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.createdBy = $scope.$parent.authentication.userId;
            $scope.entity.ModifiedBy = $scope.$parent.authentication.userId;
            entityService.saveCustomerContact($scope.entity).then(
                function (output) {
                    $scope.contactID = output.data.data.contactID;
                    if ($scope.entity.customerfile != null) {
                        $scope.uploadAttachment($scope.entity.customerfile, $scope.contactID[0]);
                    }
                    $scope.entity = {};
                    $scope.contactList = [];
                    $scope.branchList = [];
                    $scope.selectedCommodities = [];
                    $scope.selectedCountries = [];
                    $scope.customerContactlistTable.reload();
                    $scope.goBack();
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

        var saveAttachment = function (filename) {
            entityService.saveAttachment(filename).then(
                function (output) {
                    //$scope.Total = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };


        $scope.showContactDetail = function (action, id) {

            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.contactID = id;
            $scope.entity.contactID = parseInt(id);
            if ($scope.contactID > 0) {
                entityService.getContactDetail($scope.contactID).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        $scope.entity = output.data.data;
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
                        var temp = new Array();
                        if ($scope.entity.contactCategoryID != null) {
                            temp = $scope.entity.contactCategoryID.split(",");
                            for (var a in temp) {
                                temp[a] = parseInt(temp[a]);
                            }
                            $scope.entity.categoryIds = temp;
                        }
                        $scope.contactList = [];
                        //Fill Additional contact details

                        if ($scope.entity.additionalContactDTOList != null) {
                            if ($scope.entity.additionalContactDTOList.length > 0) {
                                $scope.entity.addMoreContact = true;
                                $scope.contactList = $scope.entity.additionalContactDTOList;
                            }
                        }
                        //Fill Branch Details
                        $scope.branchList = [];
                        if ($scope.entity.contactBranchDetailDTOList != null) {
                            if ($scope.entity.contactBranchDetailDTOList.length > 0) {
                                $scope.entity.branchInfo = true;
                                $scope.branchList = $scope.entity.contactBranchDetailDTOList;
                            }
                        }
                        $scope.selectedCommodities = [];
                        $scope.selectedCountries = [];
                        var tempCommditiyArray = new Array();
                        if ($scope.entity.contactCommodityDTOList != null) {

                            $scope.lookups.commoditys = $.map($scope.lookups.commoditys, function (o) {
                                o.selected = (Utility.inArray($scope.entity.contactCommodityDTOList, "commodityID", o.commodityId) != -1);
                                return o;
                            });

                            ////for (var i = 0; i < $scope.entity.contactCommodityDTOList.length; i++) {
                            ////    for (var j = 0; j < $scope.lookups.commoditys.length; j++) {
                            ////        if ($scope.lookups.commoditys[j].commodityId == $scope.entity.contactCommodityDTOList[i].commodityID) {
                            ////            tempCommditiyArray[i] = parseInt($scope.lookups.commoditys[j].commodityId)
                            ////            $scope.selectedCommodities.push(
                            ////                {
                            ////                    commodityId: $scope.lookups.commoditys[j].commodityId,
                            ////                    commodityName: $scope.lookups.commoditys[j].name
                            ////                });
                            ////            break;
                            ////        }
                            ////    }
                            ////}
                            ////$scope.entity.commodityIds = tempCommditiyArray;
                            // $("select[name=commodityIds]") = tempCommditiyArray;
                        }
                        var tempOrigionArray = new Array();
                        if ($scope.entity.contactOrigionDTOList != null) {
                            $scope.lookups.origincountries = $.map($scope.lookups.origincountries, function (o) {
                                o.selected = (Utility.inArray($scope.entity.contactOrigionDTOList, "origionID", o.cryId) != -1);
                                return o;
                            });
                            //for (var i = 0; i < $scope.entity.contactOrigionDTOList.length; i++) {
                            //    for (var j = 0; j < $scope.lookups.origincountries.length; j++) {
                            //        if ($scope.lookups.origincountries[j].cryId == $scope.entity.contactOrigionDTOList[i].origionID) {
                            //            tempOrigionArray[i] = parseInt($scope.lookups.origincountries[j].cryId)
                            //            $scope.selectedCountries.push(
                            //                {
                            //                    countryId: $scope.lookups.origincountries[j].cryId,
                            //                    countryName: $scope.lookups.origincountries[j].cryName
                            //                });
                            //            break;
                            //        }
                            //    }

                            //}
                            //$scope.entity.countryIds = tempOrigionArray;
                        }

                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );

            }
            else {
                $scope.goBack();
            }
        };
        var removeBatch = function () {
            var entities = [];
            $scope.items.forEach(function (item) {
                if (item.selected) {
                    entities.push(item);
                }
            });
            if (entities.length === 0) {
                ngNotifier.info("Please, select atleast one record to perform action.");
            } else {
                ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                    entities.forEach(function (entity) {
                        entityService.delete(entity).then(
                            function (output) {
                                $scope.entity = {};
                                $scope.customerContactlistTable.reload();
                                $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });
                    });
                });
            }
        };
        var switchTab = function (title, action) {

            $scope.tabs.forEach(function (tab) {
                tab.active = false;
                tab.disabled = ((action === "add" || action === "edit") && tab !== $scope.tabs[0]);
                if (tab.title === title) {
                    tab.active = true;
                }
            });
        };
        $scope.performContactAction = function (source, fromList) {

            var action = source.currentTarget.attributes["action"].value;

            $scope.$broadcast("show-errors-check-validity");

            if (action != "cancel" && $scope.form.detail != undefined && $scope.form.detail.$invalid) {
                if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
                    ngNotifier.error("Required Field(s) are missing data.");
                }
                else if ($scope.form.detail.usrPwdC.$invalid) {
                    ngNotifier.error("Password do not match with Confirm Password.");
                }
                return;
            }

            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {
                $scope.showContactDetail(action, source.currentTarget.attributes["entityId"].value);
            } else {
                initControls(action);
            }

            switchTab("Detail", action);

            switch (action) {
                case "search":
                    filterList();
                    break;
                case "add":
                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    $("input[input-date]").each(function (index, element) { $(element).val(null); });
                    break;
                case "edit":
                    lastAction = action;
                    break;
                case "save":
                    save(action);
                    break;
                case "saveEmail":
                    $scope.entity.isSendEmailNow = true;
                    save(action);
                    break;
                case "cancel":
                    $scope.showContactDetail('viewDetail', $scope.entity.contactID);
                    lastAction = "";
                    break;
                case "delete":
                    remove();
                    lastAction = "";
                    break;
                case "deleteBatch":
                    removeBatch();
                    lastAction = "";
                    break;
                case "verify":
                case "activate":
                case "deactivate":
                    $scope.changeStatus(action);
                    lastAction = "";
                    break;
                default:
                    lastAction = "";
                    break;
            }

            if ($scope.afterPerformAction != undefined) {
                $scope.afterPerformAction(source, fromList);
            }
        };

        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":
                    $scope.getLatestCustomerCode();
                    break;
            }

        }

        $scope.getLatestCustomerCode = function () {

            var getCustomerCode = entityService.getLatestCustomerCode().then(
                function (output) {
                    $scope.entity.customerCode = output.data.data.customerCode;

                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };
        $scope.addContactRow = function () {
            var contactDetailItem = {};

            if ($scope.entity.name != null && $scope.entity.name != "") {
                contactDetailItem.name = $scope.entity.name;
            }
            else {
                ngNotifier.error("Please enter Contact Name");
                return;
            }
            if ($scope.entity.designation != null && $scope.entity.designation != "") {
                contactDetailItem.designation = $scope.entity.designation;
            }
            else {
                ngNotifier.error("Please enter Designation");
                return;
            }
            if ($scope.entity.contactNo1 != null) { $scope.entity.contactNo = $scope.entity.contactNo1 + "|"; }
            else { $scope.entity.contactNo = "|"; }
            if ($scope.entity.contactNo2 != null) { $scope.entity.contactNo += $scope.entity.contactNo2 + "|"; }
            else { $scope.entity.contactNo += "|"; }
            if ($scope.entity.contactNo3 != null) { $scope.entity.contactNo += $scope.entity.contactNo3; }

            if ($scope.entity.contactCellNo1 != null) { $scope.entity.contactCellNo = $scope.entity.contactCellNo1 + "|"; }
            else { $scope.entity.contactCellNo = "|"; }
            if ($scope.entity.contactCellNo2 != null) { $scope.entity.contactCellNo += $scope.entity.contactCellNo2 + "|"; }
            else { $scope.entity.contactCellNo += "|"; }
            if ($scope.entity.contactCellNo3 != null) { $scope.entity.contactCellNo += $scope.entity.contactCellNo3; }


            if ($scope.entity.contactEmail != null) {
                contactDetailItem.contactEmail = $scope.entity.contactEmail;
            } else { contactDetailItem.contactEmail = ""; }
            if ($scope.entity.contactNo != null) {
                contactDetailItem.contactNo = $scope.entity.contactNo;
            } else { contactDetailItem.contactNo = ""; }
            if ($scope.entity.contactCellNo != null) {
                contactDetailItem.contactCellNo = $scope.entity.contactCellNo;
            } else { contactDetailItem.contactCellNo = ""; }
            $scope.contactList.push(contactDetailItem);
            $scope.entity.name = '';
            $scope.entity.designation = '';
            $scope.entity.contactEmail = '';
            $scope.entity.contactNo = '';
            $scope.entity.contactNo1 = '';
            $scope.entity.contactNo2 = '';
            $scope.entity.contactNo3 = '';
            $scope.entity.contactCellNo = '';
            $scope.entity.contactCellNo1 = '';
            $scope.entity.contactCellNo2 = '';
            $scope.entity.contactCellNo3 = '';

        };
        $scope.removeContactRow = function (name) {
            var index = -1;
            var contactArr = eval($scope.contactList);
            for (var i = 0; i < contactArr.length; i++) {
                if (contactArr[i].name === name) {
                    index = i;
                    break;
                }
            }
            if (index === -1) {
                alert("Something gone wrong");
            }
            $scope.contactList.splice(index, 1);
        }

        $scope.addBranchRow = function () {
            //$scope.contactList.push({ 'name': $scope.entity.name, 'designation': $scope.entity.designation, 'contactEmail': entity.contactEmail, 'contactNo': entity.contactNo, 'cellNo': entity.cellNo });
            var branchDetailItem = {};
            if ($scope.entity.branchName != null && $scope.entity.branchName != "") {
                branchDetailItem.branchName = $scope.entity.branchName;
            }
            else {
                ngNotifier.error("Please enter Branch Name");
                return;
            }
            
            if ($scope.entity.branchTelNo1 != null) { $scope.entity.branchTelNo = $scope.entity.branchTelNo1 + "|"; }
            else { $scope.entity.branchTelNo = "|"; }
            if ($scope.entity.branchTelNo2 != null) { $scope.entity.branchTelNo += $scope.entity.branchTelNo2 + "|"; }
            else { $scope.entity.branchTelNo += "|"; }
            if ($scope.entity.branchTelNo3 != null) { $scope.entity.branchTelNo += $scope.entity.branchTelNo3; }

            if ($scope.entity.branchCellNo1 != null) { $scope.entity.branchCellNo = $scope.entity.branchCellNo1 + "|"; }
            else { $scope.entity.branchCellNo = "|"; }
            if ($scope.entity.branchCellNo2 != null) { $scope.entity.branchCellNo += $scope.entity.branchCellNo2 + "|"; }
            else { $scope.entity.branchCellNo += "|"; }
            if ($scope.entity.branchCellNo3 != null) { $scope.entity.branchCellNo += $scope.entity.branchCellNo3; }

            if ($scope.entity.branchFax1 != null) { $scope.entity.branchFax = $scope.entity.branchFax1 + "|"; }
            else { $scope.entity.branchFax = "|"; }
            if ($scope.entity.branchFax2 != null) { $scope.entity.branchFax += $scope.entity.branchFax2 + "|"; }
            else { $scope.entity.branchFax += "|"; }
            if ($scope.entity.branchFax3 != null) { $scope.entity.branchFax += $scope.entity.branchFax3; }


            if ($scope.entity.branchAddress != null) {
                branchDetailItem.branchAddress = $scope.entity.branchAddress;
            } else { branchDetailItem.branchAddress = ""; }

            if ($scope.entity.branchRegion != null) {
                branchDetailItem.branchRegion = $scope.entity.branchRegion;
            } else { branchDetailItem.branchRegion = ""; }

            if ($scope.entity.branchCountry != null) {
                branchDetailItem.branchCountry = $scope.entity.branchCountry;
            } else { branchDetailItem.branchCountry = ""; }

            if ($scope.entity.branchState != null) {
                branchDetailItem.branchState = $scope.entity.branchState;
                branchDetailItem.branchStateID = $scope.entity.branchStateID;
            } else {
                ngNotifier.error("Please enter Branch State");
                return;
            }
            if ($scope.entity.branchCity != null) {
                branchDetailItem.branchCity = $scope.entity.branchCity;
                branchDetailItem.branchCityID = $scope.entity.branchCityID;
            } else { branchDetailItem.branchCity = ""; }

            if ($scope.entity.branchContactPerson != null) {
                branchDetailItem.branchContactPerson = $scope.entity.branchContactPerson;
            } else {
                ngNotifier.error("Please enter Branch Contact Person");
                return;
            }
            if ($scope.entity.branchTaxID != null) {
                branchDetailItem.branchTaxID = $scope.entity.branchTaxID;
            } else { branchDetailItem.branchTaxID = ""; }

            if ($scope.entity.branchEmail != null) {
                branchDetailItem.branchEmail = $scope.entity.branchEmail;
            } else { branchDetailItem.branchEmail = ""; }

            if ($scope.entity.branchZipCode != null) {
                branchDetailItem.branchZipCode = $scope.entity.branchZipCode;
            } else { branchDetailItem.branchZipCode = ""; }

            if ($scope.entity.branchTelNo != null) {
                branchDetailItem.branchTelNo = $scope.entity.branchTelNo;
            } else { branchDetailItem.branchTelNo = ""; }

            if ($scope.entity.branchCellNo != null) {
                branchDetailItem.branchCellNo = $scope.entity.branchCellNo;
            } else { branchDetailItem.branchCellNo = ""; }

            if ($scope.entity.branchFax != null) {
                branchDetailItem.branchFax = $scope.entity.branchFax;
            } else { branchDetailItem.branchFax = ""; }

            $scope.branchList.push(branchDetailItem);
            $scope.entity.branchName = '';
            $scope.entity.branchTelNo = '';
            $scope.entity.branchTelNo1 = '';
            $scope.entity.branchTelNo2 = '';
            $scope.entity.branchTelNo3 = '';
            $scope.entity.branchAddress = '';
            $scope.entity.branchCellNo = '';
            $scope.entity.branchCellNo1 = '';
            $scope.entity.branchCellNo2= '';
            $scope.entity.branchCellNo3 = '';
            $scope.entity.branchRegion = '';
            $scope.entity.branchRegionID = 0;
            $scope.entity.branchCountry = '';
            $scope.entity.branchCountryID = null;
            $scope.entity.branchState = '';
            $scope.entity.branchStateID = null;
            $scope.entity.branchCity = '';
            $scope.entity.branchCityID = 0;
            $scope.entity.branchContactPerson = '';
            $scope.entity.branchTaxID = '';
            $scope.entity.branchEmail = '';
            $scope.entity.branchZipCode = '';
            $scope.entity.branchFax = '';
            $scope.entity.branchFax1 = '';
            $scope.entity.branchFax2 = '';
            $scope.entity.branchFax3 = '';
            
            
        };
        $scope.removeBranchRow = function (name) {
            var index = -1;
            var branchArr = eval($scope.branchList);
            for (var i = 0; i < branchArr.length; i++) {
                if (branchArr[i].branchName === name) {
                    index = i;
                    break;
                }
            }
            if (index === -1) {
                alert("Something gone wrong");
            }
            $scope.branchList.splice(index, 1);
        }

        $scope.replacePipe = function (text) {
            if (text != null) {
                return text.replace('|', '').replace('|', '');
            }
            else
            {
                return "";
            }
        }

        $scope.afterSave = function (lastAction) {
            $scope.customerContactlistTable.reload();
            $scope.entityId = 0;
            $scope.viewList = true;
            $scope.page.urls.container = "";
        }

        //#region Modal Methods

        $scope.callCountryModal = function (source) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplcountryModal/detail.html",
                controller: "siplcountryModalController",
                resolve: {
                    requestData: function () {
                        return {
                            countryId: ($scope.entity.countryId || 0),
                            continentID: ($scope.entity.continentId || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.countryId = output.data.countryId;
                        $scope.entity.countryName = output.data.name;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.callStateModal = function (source) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplstateModal/detail.html",
                controller: "siplStateModalController",
                resolve: {
                    requestData: function () {
                        return {
                            stateId: ($scope.entity.stateId || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.stateId = output.data.stateId;
                        $scope.entity.stateName = output.data.name;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.callCityModal = function (source) {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCityModal/detail.html",
                controller: "siplCityModalController",
                resolve: {
                    requestData: function () {
                        return {
                            cityId: ($scope.entity.cityId || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.cityId = output.data.cityId;
                        $scope.entity.city = output.data.name;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        $scope.callRegionModal = function () {

            // $scope.$parent.selectedSiteId
            // $scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/continentModal/detail.html",
                controller: "continentModalController",
                resolve: {
                    requestData: function () {
                        return {
                            continentID: ($scope.entity.continentID || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.continentID = output.data.continentID;
                        $scope.entity.continent = output.data.continent;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };


        $scope.uploadAttachment = function (customerfile, contactID) {

            if (customerfile) {

                if (customerfile.size > 10485760) {
                    ngNotifier.error("File cannot exceeds more than 10 MB size.");
                }
                else {
                    var attachment = {
                        DisplayName: contactID + '_' + customerfile.name,
                        ContactId: contactID
                    };
                    entityService.uploadFile(attachment, customerfile).then(
                        function (output) {
                            //ngNotifier.show(output.data.output);
                        },
                        function (output) {
                            ngNotifier.error(output.data.output.messages);
                        }
                    );
                }

            }
        };

        $scope.exportContactReport = function (source, siteId) {

            //var action = source.currentTarget.attributes["action"].value;
            var optionValue;
            var seachValue;
            var optionDateValue;

            var reportParams = {               
                SiteId: siteId
            };
            entityService.exportContactReport(reportParams).then(
                function (output) {
                    var blobData = new Blob([output.data], { type: output.headers()["content-type"] });
                    var fileName = output.headers()["x-filename"];
                    saveAs(blobData, fileName);
                },
                function (output) {
                    ngNotifier.error(output);
                }
            );
        }

        $scope.downloadAttachment = function () {
            //window.open('', '_blank', '');
            if ($scope.entity.attachment != null) {
                entityService.downloadAttachment($scope.entity).then(
                            function (output) {
                                var blob = new Blob([output.data], { type: 'application/octet-stream' });
                                saveAs(blob, output.config.headers.fileName);
                            },
                            function (output) {
                                ngNotifier.logError(output);
                            }
                        );
            }
        }


        var box = $(".dual-list-control");

        $scope.beforeFetchLookupData = function (moduleName, otherId, sortField, lookupKey) {
            var listParams = {
                OtherId: otherId,
                PageIndex: 1,
                PageSize: 60000,
                CwtId: $scope.userWorkTypeId,
                Sort: "{\"" + sortField + "\":\"asc\"}",
                Filter: "[]"
            };
            if (moduleName == "sipluser") {
                var filter = [];
                filter.push(Utility.createFilter("SitId", "numeric", "SitId", $scope.$parent.selectedSiteId, "contains", null));
                listParams.Filter = JSON.stringify(filter);
            }
            
            return listParams;
        };
       
        
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("customerContactController", controller);

});
