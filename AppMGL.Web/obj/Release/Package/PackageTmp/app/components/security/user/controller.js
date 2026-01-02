"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "userService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.user;
        $scope.tabs = appUrl.user.tabs;
        $scope.isUnitUser = false;

        //#endregion

        //#region Private

        var checkEmail = function () {

            var params = {
                Email: $scope.entity.contact.cntEmail
            };

            entityService.checkEmail(params).then(
                function (output) {
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

        $scope.lookups = { siteTypes: [], titles: [], departments: [], contactWorkTypes: [], roles: [], units: [] };

        $scope.initDropdown = function () {

            $scope.lookups.contactTypes = [
                { cntTypeId: 0, cntTypeName: "Internal" },
                { cntTypeId: 1, cntTypeName: "External" }
            ];
            $scope.lookups.units = $scope.$parent.authentication.userSite;
            $scope.fetchLookupData("title", 0, "TtlName", "titles", null);
            $scope.fetchLookupData("department", 0, "DptName", "departments", null);
            $scope.fetchLookupData("contactWorkType", 0, "CwtName", "contactWorkTypes", null);
            //$scope.fetchLookupData("role", 0, "rleName", "roles", null);
            //$scope.fetchLookupData("site", 0, "sitName", "units", null);
            $scope.getUserRoles($scope.$parent.userWorkTypeId);
          };

        $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey == "departments") { $scope.lookups.departments.unshift({ "dptId": "", "dptName": "" }); } //

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

        //#endregion

        //#region Detail

        $scope.afterPerformAction = function (source, fromList) {

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
                    checkEmail();
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

            //if ($scope.lookups.roles != null) {
            //    $scope.lookups.roles = $scope.lookups.roles.filter(function (role) {
            //        if ($scope.entity.contact.cwtId == 1)
            //            return role.rleId != 98;
            //        else
            //            return role.rleId != 99 && role.rleId != 98;
            //    });
            //}

        }

        $scope.updateFullName = function () {

            var firstName = ($scope.entity.contact && $scope.entity.contact.cntFirstName != "") ? $scope.entity.contact.cntFirstName : "";
            var lastName = ($scope.entity.contact && $scope.entity.contact.cntLastName != "") ? $scope.entity.contact.cntLastName : "";
            $scope.entity.fullName = firstName + (lastName != "" ? " " + lastName : "");
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };
    
    controller.$inject = injectParams;

    app.register.controller("userController", controller);

});
