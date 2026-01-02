"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "freightforwardernetworkService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

       
        $scope.page = appUrl.freightforwardernetwork;
        $scope.tabs = appUrl.freightforwardernetwork.tabs;
        $scope.ffNetworkID;
        $scope.contactList = [];
        $scope.branchList = [];
        $scope.citiesList = [];
        $scope.branchCitiesList = [];
        $scope.searchCitiesList = [];
        $scope.categories = [];
        $scope.selectedStateId = 0;
        $scope.sitelist = [];
         $scope.lookups = { siplContinents: [], countries: [], origincountries: [], states: [], lgvwstates: [], commoditys: [], contactCategories: [], cities: [], companyGradations: [], users: [], accountCategories: [], sites: [] };
        $scope.iscorporat = false;
        $scope.initDropdown = function () {
            $scope.fetchLookupData("country", 0, "CryName", "countries", null);
            $scope.fetchLookupData("country", 0, "CryName", "origincountries", null);
            $scope.fetchLookupData("state", 0, "ustName", "states", null);
            $scope.fetchLookupData("continent", 0, "name", "siplContinents", null);
            $scope.fetchLookupData("siplcountry", 0, "name", "siplcountries", null);
            $scope.fetchLookupData("lgvwstate", 0, "name", "lgvwstates", null);
            $scope.fetchLookupData("sipluser", 0, "name", "users", null);
            $scope.fetchLookupData("CustomerContact", 0, "SitName", "sitelist", "SiteList");
            var sitAll = $.map($scope.authentication.userSite, function (o) { return o.SitId; }).join(",");
            $scope.lookups.sites = angular.copy($scope.authentication.userSite);
            if ($scope.lookups.sites != undefined) {
                $scope.lookups.sites.unshift({ SitId: sitAll, SitName: "All" });
            }
            $scope.siteId = sitAll;
        };

        $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey == "countries") { $scope.lookups.countries.unshift({ "cryId": "", "cryName": "" }); } //
            if (lookupKey == "origincountries") { $scope.lookups.origincountries.unshift({ "cryId": "", "cryName": "" }); } //
            if (lookupKey == "states") {

                $scope.lookups.states.unshift({ "ustId": "", "ustName": "" });
            }
            if (lookupKey == "commoditys") {
                if ($scope.lookups.commoditys != undefined) {
                    $scope.lookups.commoditys.unshift({ "commodityId": "", "name": "" });
                }
            }
            
            if (lookupKey == "cities") { $scope.lookups.cities.unshift({ "cityId": "", "name": "" }); }
            if (lookupKey == "users") {
                if ($scope.lookups.users != undefined) {
                    $scope.lookups.users.unshift({ "userId": "", "name": "" });
                }
            }  
            if (lookupKey == "companyGradations") {
                if ($scope.lookups.companyGradations != undefined) {
                    $scope.lookups.companyGradations.unshift({ "gradationId": "", "companyGradation": "" });
                }
            }
            if (lookupKey == "contactCategories") {
                if ($scope.lookups.contactCategories != undefined) {
                    if ($scope.lookups.contactCategories.length > 0) {
                        $scope.lookups.contactCategories.forEach(function (o) {
                            if (o.isVendor == false) {
                                $scope.categories.push(o);
                            }
                        });
                    };
                };
            };
            if (lookupKey == "sitelist") {
                if ($scope.lookups.sitelist != undefined) {
                    if ($scope.lookups.sitelist.length > 0) {
                        $scope.lookups.sitelist.forEach(function (o) {

                            $scope.sitelist.push(o);

                        });
                    };
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

                    var dataitems = entityService.list(listParams).then(
                        function (output) {
                            $scope.validateUser(output);
                            $scope.items = output.data.data;
                            params.total(output.data.count);

                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        }
                    );

                }
            });
       
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
            };
            $scope.customerContactlistTable.reload();
        };


        $scope.saveCustomerContact = function (source, fromList) {
            $scope.$broadcast("show-errors-check-validity");


            if ($scope.entity.ffNetworkName == null) {
                ngNotifier.error("Please enter Netwrok Name");
                return;
            }
            if ($scope.entity.ffNetworkCode == null) {
                ngNotifier.error("Please enter Netwrok Code");
                return;
            }
            if ($scope.entity.addMoreContact) {
                if ($scope.contactList.length == 0) {
                    ngNotifier.error("Please enter Contact details");
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
            $scope.entity.additionalFFContactDTOList = [];
            if ($scope.entity.addMoreContact) {
                if ($scope.contactList.length > 0) {
                    $scope.entity.additionalFFContactDTOList = $scope.contactList;
                }
            }
            var originItem = {};
            $scope.entity.contactOrigionDTOList = [];
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
                    $scope.contactID = output.data.data.ffNetworkID;
                    if ($scope.entity.customerfile != null) {
                        $scope.uploadAttachment($scope.entity.customerfile, $scope.ffNetworkID);
                    }
                    $scope.entity = {};
                    $scope.contactList = [];
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
            $scope.ffNetworkID = id;
            $scope.entity.ffNetworkID = parseInt(id);
            if ($scope.ffNetworkID > 0) {
                entityService.getContactDetail($scope.ffNetworkID).then(
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
                        $scope.entity.siteids = parseInt($scope.entity.siteids);
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
                        $scope.contactList = [];
                        if ($scope.entity.additionalFFContactDTOList != null) {
                            if ($scope.entity.additionalFFContactDTOList.length > 0) {
                                $scope.entity.addMoreContact = true;
                                $scope.contactList = $scope.entity.additionalFFContactDTOList;
                            }
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
                    $scope.entity.ffNetworkCode = output.data.data.ffNetworkCode;

                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };
        $scope.addContactRow = function () {
            var contactDetailItem = {};

            if ($scope.entity.name != null && $scope.entity.name != "") {
                contactDetailItem.contactName = $scope.entity.name;
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
                if (contactArr[i].contactName === name) {
                    index = i;
                    break;
                }
            }
            if (index === -1) {
                alert("Something gone wrong");
            }
            $scope.contactList.splice(index, 1);
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

        $scope.callCountryModal = function (source) {
                       
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
                        },
                        function (output) {
                            ngNotifier.error(output.data.output.messages);
                        }
                    );
                }

            }
        };

        $scope.exportContactReport = function (source, siteId) {
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

       
        $scope.downloadAttachmentAction = function (obj) {

            if (obj.attachment != null) {
                entityService.downloadAttachment(obj).then(
                            function (output) {
                                var blob = new Blob([output.data], { type: 'application/octet-stream' });
                                saveAs(blob, output.config.headers.fileName);
                            },
                            function (output) {
                                ngNotifier.logError(output);
                            }
                        );
            }
           
        };
       
        $scope.callTypeaheadnetwork = function (viewValue, lookupModule, lookupField1, lookupMethod, fieldType, lookupField2, valueType) {

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
                SiteId: $scope.selectedSite.siteId,
                CwtId: $scope.userWorkTypeId,
                ModuleId: $scope.page.moduleId,
                PageIndex: 1,
                PageSize: 15,
                Sort: "{ \"" + lookupField1 + "\": \"asc\" }",
                Filter: JSON.stringify(criteria)
            };

            return entityService.lookup(lookupModule, lookupMethod, listParams).then(
                function (output) {

                    if ($scope.isInvalidData != undefined) {
                        if (output.data.data.length == 0) { $scope.isInvalidData = true; }
                        else { $scope.isInvalidData = false; }
                    }
                    return output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };
       
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };
    controller.$inject = injectParams;
    app.register.controller("freightforwardernetworkController", controller);

});
