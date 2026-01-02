"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "userService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.user;
        $scope.tabs = appUrl.user.tabs;
        $scope.isUnitUser = false;
        $scope.lookups = { siplContinents: [], countries: [], origincountries: [], states: [], lgvwstates: [], commoditys: [], contactCategories: [], cities: [], companyGradations: [], users: [], accountCategories: [], sites: [] };
        //#endregion

        //#region Private
        $scope.jobrole = [];
        var checkEmail = function () {

            var params = {
                Email: $scope.entity.contact.cntEmail
            };

            entityService.checkEmail(params).then(
               
                function (output) {
                    debugger;
                    if (output.data.resultId == 1001) {
                        ngNotifier.info(output.data.messages.join(","));
                        if (output.data.data) {
                            $scope.form.detail.cntEmail.$invalid = true;
                        }
                    }
                    else {
                        ngNotifier.showError($scope.authentication, output);
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });
        };

        var resetPassword = function () {

            var params = {
                Email: $scope.entity.contact.cntEmail
            };

            entityService.resetPassword(params).then(
                function (output) {
                    if (output.data.resultId == 1001) {
                        ngNotifier.show(output.data);
                    }
                    else {
                        ngNotifier.showError($scope.authentication, output);
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });
        };

        //#endregion

        //#region Lookup

        $scope.lookups = { siteTypes: [], titles: [], departments: [], contactWorkTypes: [], roles: [], units: [], jobrole: [] };

        $scope.initDropdown = function () {

            $scope.lookups.contactTypes = [
                { cntTypeId: 0, cntTypeName: "Internal" },
                { cntTypeId: 1, cntTypeName: "External" }
            ];

            $scope.lookups.contactTypesAll = [
                { cntTypeId: '', cntTypeName: "Both" },
               { cntTypeId: '0', cntTypeName: "Internal" },
               { cntTypeId: '1', cntTypeName: "External" }
            ];
            $scope.lookups.units = $scope.$parent.authentication.userSite;
            $scope.fetchLookupData("title", 0, "TtlName", "titles", null);
            $scope.fetchLookupData("department", 0, "DptName", "departments", null);
            $scope.fetchLookupData("contactWorkType", 0, "CwtName", "contactWorkTypes", null);
            $scope.fetchLookupData("continent", 0, "name", "siplContinents", null);
            $scope.fetchLookupData("siplcountry", 0, "name", "siplcountries", null);
            $scope.fetchLookupData("lgvwstate", 0, "name", "lgvwstates", null);
            $scope.fetchLookupData("userjobrole", 0, "name", "jobrole", null);
            $scope.getUserRoles($scope.$parent.userWorkTypeId);
          };

        $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey == "departments") { $scope.lookups.departments.unshift({ "dptId": "", "dptName": "" }); } //
            if (lookupKey == "jobrole") {
                if ($scope.lookups.jobrole != undefined) {
                    if ($scope.lookups.jobrole.length > 0) {
                        $scope.lookups.jobrole.forEach(function (o) {

                            $scope.jobrole.push(o);

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
        $scope.getUserRoles = function (userWorkTypeId) {
            entityService.getUserRoles(userWorkTypeId).then(
                function (output) {
                    $scope.lookups.roles = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            )
        };

        $scope.afterPerformAction = function (source, fromList) {
            debugger;
            var action = source.currentTarget.attributes["action"].value;

            switch (action) {
                case "add":
                    $scope.entity.contact = {};
                    $scope.entity.contact.ttlId = 105;
                    $scope.entity.contact.dptId = 105;
                    $scope.entity.contact.cntType = 0;
                    if ($scope.$parent.userWorkTypeId == 2) {
                        $scope.isUnitUser = true;
                        $scope.entity.contact.cwtId = 2;
                    }
                    else {
                        $scope.isUnitUser = false;
                    }
                    $scope.entity.contact.cntStatus = true;
                    $scope.entity.usrValidFrom = Utility.getDateISO();

                    //$scope.entity.fullName = "Farhan Sajid";
                    //$scope.entity.contact.cntFirstName = "Farhan";
                    //$scope.entity.contact.cntLastName = "Sajid";
                    //$scope.entity.contact.cntEmail = "fsm.expert@outlook.com";
                    //$scope.entity.contact.cwtId = 1;
                    break;
                case "edit":
                    if ($scope.$parent.userWorkTypeId == 2) {
                        $scope.isUnitUser = true;
                    }
                    else {
                        $scope.isUnitUser = false;
                    }
                    break;
                case "resetPassword":
                    resetPassword();
                    break;
            }
        };

        $scope.performSubAction = function (source, target) {

            var action = source.currentTarget.attributes["action"].value;

            switch (action) {
                case "checkEmail":
                   // checkEmail();
                    break;
            }
        };

        $scope.beforeSave = function (action, lastAction) {

            if ($scope.entity.isLocked) {
                $scope.entity.usrIsLocked = 1;
                if ($scope.entity.usrLockedComments == null || $scope.entity.usrLockedComments == '') {
                    ngNotifier.info("Please enter User Locked Comments");
                    $scope.hasError = true;
                    return;
                }
                else {
                    $scope.hasError = false;
                }
            }
            else {
                $scope.entity.usrIsLocked = 0;
                $scope.hasError = false;
            }
            if ($scope.entity.contact.telNo1 != null) { $scope.entity.contact.telNo = $scope.entity.contact.telNo1 + "|"; }
            else { $scope.entity.contact.telNo = "|"; }
            if ($scope.entity.contact.telNo2 != null) { $scope.entity.contact.telNo += $scope.entity.contact.telNo2 + "|"; }
            else { $scope.entity.contact.telNo += "|"; }
            if ($scope.entity.contact.telNo3 != null) { $scope.entity.contact.telNo += $scope.entity.contact.telNo3; }


            if ($scope.entity.contact.cellNo1 != null) { $scope.entity.contact.cellNo = $scope.entity.contact.cellNo1 + "|"; }
            else { $scope.entity.contact.cellNo = "|"; }
            if ($scope.entity.contact.cellNo2 != null) { $scope.entity.contact.cellNo += $scope.entity.contact.cellNo2 + "|"; }
            else { $scope.entity.contact.cellNo += "|"; }
            if ($scope.entity.contact.cellNo3 != null) { $scope.entity.contact.cellNo += $scope.entity.contact.cellNo3; }


            if ($scope.entity.contact.fax1 != null) { $scope.entity.contact.fax = $scope.entity.contact.fax1 + "|"; }
            else { $scope.entity.contact.fax = "|"; }
            if ($scope.entity.contact.fax2 != null) { $scope.entity.contact.fax += $scope.entity.contact.fax2 + "|"; }
            else { $scope.entity.contact.fax += "|"; }
            if ($scope.entity.contact.fax3 != null) { $scope.entity.contact.fax += $scope.entity.contact.fax3; }
            if (lastAction == "add") {
                $scope.entity.usrPwdCreatedTs = Utility.getDateISO(new Date());
                $scope.entity.usrCreatedTs = Utility.getDateISO(new Date());
                $scope.entity.usrCreatedBy = $scope.$parent.userInfo.usrId;
                $scope.entity.contact.cntCreatedTs = Utility.getDateISO(new Date());
                $scope.entity.contact.cntCreatedBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.usrUpdatedTs = Utility.getDateISO(new Date());
                $scope.entity.usrUpdatedBy = $scope.$parent.userInfo.usrId;
                $scope.entity.contact.cntUpdatedTs = Utility.getDateISO(new Date());
                $scope.entity.contact.cntUpdatedBy = $scope.$parent.userInfo.usrId;
            }
            var userUnitRole = {};
            $scope.entity.userUnitRoleList = [];
            var selectedUserRoles = $("select[name=userRoles]").val();
            var selectedUnits = $("select[name=units]").val();
            if (selectedUserRoles.length > 0) {
                selectedUserRoles.forEach(function (roleId) {
                    if (selectedUnits.length > 0) {
                        selectedUnits.forEach(function (unitId) {
                            userUnitRole = {};
                            userUnitRole.rleId = parseInt(roleId);
                            userUnitRole.sitId = parseInt(unitId);
                            $scope.entity.userUnitRoleList.push(userUnitRole);
                        });
                    }
                    else {
                        userUnitRole = {};
                        userUnitRole.rleId = parseInt(roleId);
                        $scope.entity.userUnitRoleList.push(userUnitRole);
                    }
                });
            }

            if ($scope.entity.contact.userjobroleid != null) {
                for (var i = 0; i < $scope.entity.contact.userjobroleid.length; i++) {
                    if (i == 0) {
                        $scope.entity.contact.userroleid = $scope.entity.contact.userjobroleid[i];
                    }
                    else {
                        $scope.entity.contact.userroleid = $scope.entity.contact.userroleid + "," + $scope.entity.contact.userjobroleid[i];
                    }
                }
            }
        };

        $scope.afterSave = function () {
            if ($scope.entity.usrIsLocked == 1) {
                $scope.entity.isLocked = true;
            }
            else { $scope.entity.isLocked = false; }
        }

        $scope.afterGetDetail = function () {
           
            if ($scope.entity.userUnitRoleList != null) {

                //if ($scope.lookups.roles != null) {
                //    $scope.lookups.roles = $scope.lookups.roles.filter(function (role) {
                //        if ($scope.entity.contact.cwtId == 1)
                //            return role.rleId != 98;
                //        else
                //            return role.rleId != 99 && role.rleId != 98;
                //    });
                //}

                $scope.lookups.units = $.map($scope.lookups.units, function (o) {
                    o.selected = (Utility.inArray($scope.entity.userUnitRoleList, "sitId", o.SitId) != -1);
                    return o;
                });

                $scope.lookups.roles = $.map($scope.lookups.roles, function (o) {
                    o.selected = (Utility.inArray($scope.entity.userUnitRoleList, "rleId", o.rleId) != -1);
                    return o;
                });
            }
            if ($scope.entity.usrIsLocked == 1) {
                $scope.entity.isLocked = true;
            }
            else { $scope.entity.isLocked = false; }
            if ($scope.entity.contact.stateId != null || $scope.entity.contact.stateId != undefined)
            $scope.getCities($scope.entity.contact.stateId)
            var telphArrary = new Array();
            var celphArrary = new Array();
            var faxArrary = new Array();
            if ($scope.entity.contact.telNo != null && $scope.entity.contact.telNo != "" && $scope.entity.contact.telNo.indexOf("|") != "-1") {
                telphArrary = $scope.entity.contact.telNo.split("|")
                $scope.entity.contact.telNo1 = telphArrary[0];
                $scope.entity.contact.telNo2 = telphArrary[1];
                $scope.entity.contact.telNo3 = telphArrary[2];
            }
            else {
                $scope.entity.contact.telNo1 = $scope.entity.contact.telNo;
            }

            if ($scope.entity.contact.cellNo != null && $scope.entity.contact.cellNo != "" && $scope.entity.contact.cellNo.indexOf("|") != "-1") {
                celphArrary = $scope.entity.contact.cellNo.split("|")
                $scope.entity.contact.cellNo1 = celphArrary[0];
                $scope.entity.contact.cellNo2 = celphArrary[1];
                $scope.entity.contact.cellNo3 = celphArrary[2];
            }
            else {
                $scope.entity.contact.cellNo1 = $scope.entity.cellNo;
            }
            if ($scope.entity.contact.fax != null && $scope.entity.contact.fax != "" && $scope.entity.contact.fax.indexOf("|") != "-1") {
                faxArrary = $scope.entity.contact.fax.split("|")
                $scope.entity.contact.fax1 = faxArrary[0];
                $scope.entity.contact.fax2 = faxArrary[1];
                $scope.entity.contact.fax3 = faxArrary[2];
            }
            else {
                $scope.entity.contact.fax1 = $scope.entity.contact.fax;
            }
            var temp1 = new Array();
            if ($scope.entity.contact.userroleid != null) {
                temp1 = $scope.entity.contact.userroleid.split(",");
                for (var a in temp1) {
                    temp1[a] = parseInt(temp1[a]);
                }
                $scope.entity.contact.userjobroleid = temp1;
            }
        };

        $scope.updateUnitRole = function ()
        {
           
            if ($scope.entity.userUnitRoleList != null) {
                $scope.entity.userUnitRoleList = [];
                $scope.lookups.units = $.map($scope.lookups.units, function (o) {
                    o.selected = (Utility.inArray($scope.entity.userUnitRoleList, "sitId", o.sitId) != -1);
                    return o;
                });

                $scope.lookups.roles = $.map($scope.lookups.roles, function (o) {
                    o.selected = (Utility.inArray($scope.entity.userUnitRoleList, "rleId", o.rleId) != -1);
                    return o;
                });
            }
        }

        $scope.updateFullName = function () {

            var firstName = ($scope.entity.contact && $scope.entity.contact.cntFirstName != "") ? $scope.entity.contact.cntFirstName : "";
            var lastName = ($scope.entity.contact && $scope.entity.contact.cntLastName != "") ? $scope.entity.contact.cntLastName : "";
            $scope.entity.fullName = firstName + (lastName != "" ? " " + lastName : "");
            $scope.entity = output.data.data;
            
        };
        $scope.CntFirstNameF = "";
        $scope.CntLastNameF = "";
        $scope.DptNameF = "";
        $scope.cntTypeF = "";
        $scope.searchParam = {
           
            firstNameF: $scope.CntFirstNameF,
            lastNameF: $scope.CntLastNameF,
            dptNameF: $scope.DptNameF,
            cntTypeF: $scope.cntTypeF
           
        };
        $scope.userlistTableNew = new NgTableParams(
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
                    UserId: $scope.authentication.userId,
                    UserWorkTypeId: $scope.$parent.userWorkTypeId,
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
        //#endregion
        $scope.performUserSearchNew = function (source, CntFirstNameF, CntLastNameF, DptNameF, cntTypeF) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                firstNameF: CntFirstNameF,
                lastNameF: CntLastNameF,
                dptNameF: DptNameF,
                cntTypeF: cntTypeF
            };
            $scope.userlistTableNew.reload();
        };
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };
    
    controller.$inject = injectParams;

    app.register.controller("userController", controller);

});
