"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "siteService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.site;
        $scope.tabs = appUrl.site.tabs;
        $scope.lookups = { siplContinents: [], countries: [],states: [], lgvwstates: [],  cities: [] };
        //#endregion

        //#region Lookup

        $scope.lookups = { siteTypes: [], countries: [], states: [] };

        $scope.initDropdown = function () {

            $scope.lookups.siteTypes = [
                { sitTypeId: 0, sitTypeName: "Franchise" },
                { sitTypeId: 1, sitTypeName: "Own" }
            ];

            $scope.fetchLookupData("country", 0, "CryName", "countries", null);
            $scope.fetchLookupData("state", 0, "UstName", "states", null);
            $scope.fetchLookupData("continent", 0, "name", "siplContinents", null);
            $scope.fetchLookupData("siplcountry", 0, "name", "siplcountries", null);
            $scope.fetchLookupData("lgvwstate", 0, "name", "lgvwstates", null);
        };

        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "contact") {
                $scope.role.cntId = output.data[0].cntId;
                $scope.role.fullName = output.data[0].fullName;
            }
            else if (lookup == "role") {
                $scope.role.rleId = output.data[0].rleId;
                $scope.role.rleName = output.data[0].rleName;
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
        };
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
        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "contact") {
                $scope.role.cntId = null;
                $scope.role.fullName = "";
            }
            else if (lookup == "role") {
                $scope.role.rleId = null;
                $scope.role.rleName = "";
            }
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
        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {

            if (lookupModule == "contact" || lookupModule == "role") {
                if ($scope.role[lookupField] == null || $scope.role[lookupField] == "") {
                    $scope.clearLookups(source, lookupModule, lookupIndex);
                }
            }
        };

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {
            if ($scope.entity.location.cntTelNo1 != null) { $scope.entity.location.cntTelNo = $scope.entity.location.cntTelNo1 + "|"; }
            else { $scope.entity.location.cntTelNo = "|"; }
            if ($scope.entity.location.cntTelNo2 != null) { $scope.entity.location.cntTelNo += $scope.entity.location.cntTelNo2 + "|"; }
            else { $scope.entity.location.cntTelNo += "|"; }
            if ($scope.entity.location.cntTelNo3 != null) { $scope.entity.location.cntTelNo += $scope.entity.location.cntTelNo3; }


            if ($scope.entity.location.cntCellNo1 != null) { $scope.entity.location.cntCellNo = $scope.entity.location.cntCellNo1 + "|"; }
            else { $scope.entity.location.cntCellNo = "|"; }
            if ($scope.entity.location.cntCellNo2 != null) { $scope.entity.location.cntCellNo += $scope.entity.location.cntCellNo2 + "|"; }
            else { $scope.entity.location.cntCellNo += "|"; }
            if ($scope.entity.location.cntCellNo3 != null) { $scope.entity.location.cntCellNo += $scope.entity.location.cntCellNo3; }


            if ($scope.entity.location.cntFax1 != null) { $scope.entity.location.cntFax = $scope.entity.location.cntFax1 + "|"; }
            else { $scope.entity.location.cntFax = "|"; }
            if ($scope.entity.location.cntFax2 != null) { $scope.entity.location.cntFax += $scope.entity.location.cntFax2 + "|"; }
            else { $scope.entity.location.cntFax += "|"; }
            if ($scope.entity.location.cntFax3 != null) { $scope.entity.location.cntFax += $scope.entity.location.cntFax3; }
            if (lastAction == "add") {
                $scope.entity.sitCreatedTs = new Date();
                $scope.entity.sitCreatedBy = $scope.$parent.userInfo.usrId;
                $scope.entity.location.lcnCreatedTs = new Date();
                $scope.entity.location.lcnCreatedBy = $scope.$parent.userInfo.usrId;
            }
            else {
                $scope.entity.sitUpdatedTs = new Date();
                $scope.entity.sitUpdatedBy = $scope.$parent.userInfo.usrId;
                $scope.entity.location.lcnUpdatedTs = new Date();
                $scope.entity.location.lcnUpdatedBy = $scope.$parent.userInfo.usrId;
            }
        };

        //#endregion

        //#region Mapping

        var lastActionC = "";

        var fetchAction = function (id) {
            if (id) {
                $scope.role = $.extend(true, {}, $.grep($scope.roleTable.data, function (obj) { return obj.scrId == id; })[0]);
            }
        };

        var disableControls = function (flag, action) {
            $("div > button[action='editC'] , div > button[action='deleteC']").prop("disabled", flag);
            $scope.disabledInput = flag;
            $scope.requiredInput = !flag;
            if (action == "viewDetailC") {
                $scope.disabledInput = true;
                $scope.requiredInput = false;
            }
        };

        var save = function () {

            if (lastActionC === "addC") {
                $scope.role.sitId = $scope.entity.sitId;
                $scope.role.scrCreatedTs = new Date();
                $scope.role.scrCreatedBy = $scope.$parent.userInfo.usrId;
                entityService.insertRole($scope.role).then(
                    function (output) {
                        $scope.role = {};
                        $scope.roleTable.reload();
                        ngNotifier.show(output.data);
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });
            } else if (lastActionC === "editC" && $scope.role.scrId > 0) {
                $scope.role.scrUpdatedTs = new Date();
                $scope.role.scrUpdatedBy = $scope.$parent.userInfo.usrId;
                entityService.updateRole($scope.role, $scope.role.scrId).then(
                    function (output) {
                        $scope.role = {};
                        $scope.roleTable.reload();
                        ngNotifier.show(output.data);
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });
            }
        };

        var remove = function (id) {

            if (id == null) {
                id = $scope.role.scrId;
            }
            if (id > 0) {
                fetchAction(id);
                ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                    entityService.deleteRole($scope.role).then(
                        function (output) {
                            $scope.role = {};
                            $scope.roleTable.reload();
                            ngNotifier.show(output.data);
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        });
                });
            }
        };

        $scope.editModeSub = false;
        $scope.disabledInput = true;
        $scope.requiredInput = false;
        $scope.role = {};

        $scope.performSubAction = function (source, id) {

            var action = source.currentTarget.attributes["action"].value;

            $scope.$broadcast("show-errors-check-validity");

            if (action != "cancelC" && $scope.form.subdetail != undefined && $scope.form.subdetail.$invalid) {
                if ($scope.form.subdetail.$error.required != undefined && $scope.form.subdetail.$error.required.length > 0) {
                    ngNotifier.error($scope.resource["ErrRequiredFields"]);
                }
                return;
            }

            switch (action) {

                case "viewDetailC":
                    lastActionC = "";
                    $scope.editModeSub = false;
                    fetchAction(id);
                    disableControls(false, action);
                    break;

                case "addC":
                    lastActionC = action;
                    $scope.editModeSub = true;
                    $scope.role = {};
                    disableControls(false, action);
                    break;

                case "editC":
                    lastActionC = action;
                    $scope.editModeSub = true;
                    fetchAction(id);
                    disableControls(false, action);
                    break;

                case "saveC":
                    save();
                    lastActionC = "";
                    $scope.editModeSub = false;
                    disableControls(true, action);
                    break;

                case "cancelC":
                    lastActionC = "";
                    $scope.editModeSub = false;
                    $scope.role = {};
                    disableControls(true, action);
                    break;

                case "deleteC":
                    remove(id);
                    lastActionC = "";
                    $scope.editModeSub = false;
                    disableControls(true, action);
                    break;
            }
        };

        $scope.roleTable = new NgTableParams(
            {
                page: 1,
                count: 10,
                sorting: $.parseJSON("{ \"CntId\": \"asc\" }")
            }, {
                counts: [],
                getData: function (params) {
                    var param = {
                        SitId: $scope.entity.sitId
                    };
                    return entityService.listRole(param).then(
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

        $scope.siteListTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")
        }, {
            getData: function (params) {
            var listParams = {
                UserId: $scope.authentication.userId,
                UserWorkTypeId : $scope.$parent.userWorkTypeId,
                SiteId: $scope.selectedSite.siteId,
                ModuleId: $scope.page.moduleId,
                PageIndex: params.page(),
                PageSize: params.count(),
                Sort: JSON.stringify(params.sorting()),
                Filter: JSON.stringify($scope.criteria)
            };
            return entityService.list(listParams).then(
                function (output) {
                    $scope.validateUser(output);
                    $scope.items = output.data.data;
                    params.total(output.data.count);
                    return output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }
    });
        //#endregion
        $scope.afterGetDetail = function () {

            if ($scope.entity.location.cntStateId != null || $scope.entity.location.cntStateId != undefined)
                $scope.getCities($scope.entity.location.cntStateId)
            var telphArrary = new Array();
            var celphArrary = new Array();
            var faxArrary = new Array();
            if ($scope.entity.location.cntTelNo != null && $scope.entity.location.cntTelNo != "" && $scope.entity.location.cntTelNo.indexOf("|") != "-1") {
                telphArrary = $scope.entity.location.cntTelNo.split("|")
                $scope.entity.location.cntTelNo1 = telphArrary[0];
                $scope.entity.location.cntTelNo2 = telphArrary[1];
                $scope.entity.location.cntTelNo3 = telphArrary[2];
            }
            else {
                $scope.entity.location.cntTelNo1 = $scope.entity.location.cntTelNo;
            }

            if ($scope.entity.location.cntCellNo != null && $scope.entity.location.cntCellNo != "" && $scope.entity.location.cntCellNo.indexOf("|") != "-1") {
                celphArrary = $scope.entity.location.cntCellNo.split("|")
                $scope.entity.location.cntCellNo1 = celphArrary[0];
                $scope.entity.location.cntCellNo2 = celphArrary[1];
                $scope.entity.location.cntCellNo3 = celphArrary[2];
            }
            else {
                $scope.entity.location.cntCellNo1 = $scope.entity.cntCellNo;
            }
            if ($scope.entity.location.cntFax != null && $scope.entity.location.cntFax != "" && $scope.entity.location.cntFax.indexOf("|") != "-1") {
                faxArrary = $scope.entity.location.cntFax.split("|")
                $scope.entity.location.cntFax1 = faxArrary[0];
                $scope.entity.location.cntFax2 = faxArrary[1];
                $scope.entity.location.cntFax3 = faxArrary[2];
            }
            else {
                $scope.entity.location.cntFax1 = $scope.entity.location.cntFax;
            }
        };
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("siteController", controller);

});
