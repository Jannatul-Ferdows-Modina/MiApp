"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "usersService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.users;
        $scope.tabs = appUrl.users.tabs;

        //#endregion

        //#region Private Methods

        var initSubInfo = function (type) {
            var count = ($scope.entity.contact.location[type]) ? $scope.entity.contact.location[type].length : 0;
            var subInfo;
            if (type == "emails") {
                subInfo = {
                    emailId: 0,
                    locationId: 0,
                    emailAddress: "",
                    emailTypeId: (count == 0 ? 6001 : 6101),
                    isDefault: (count == 0)
                };
            }
            else if (type == "addresses") {
                subInfo = {
                    addressId: 0,
                    locationId: 0,
                    addressLine1: "",
                    addressLine2: "",
                    countryId: appConfig.defaultCountryId,
                    stateId: appConfig.defaultStateId,
                    city: "",
                    zipCode: "",
                    addressTypeId: (count == 0 ? 7001 : 7101),
                    isDefault: (count == 0)
                };
            }
            else if (type == "phones") {
                subInfo = {
                    phoneId: 0,
                    locationId: 0,
                    countryId: appConfig.defaultCountryId,
                    phoneNumber: "",
                    phoneTypeId: (count == 0 ? 8001 : 8101),
                    isDefault: (count == 0)
                };
            }
            return subInfo;
        };

        var setUserIcon = function () {
            if ($scope.entity.iconFileName) {
                $scope.userIcon = entityService.location + "FileSystem/UserIcons/" + $scope.entity.iconFileName;
            }
        };

        //#endregion

        //#region Lookup 

        $scope.lookups = { userTypes: [], adminTypes: [], genders: [], emailTypes: [], addressTypes: [], phoneTypes: [], countries: [], states: [], emailSubscriptions: [] };

        $scope.initDropdown = function () {

            $scope.fetchLookupData("enumValue", 3000, "EnumValueId", "userTypes", null);
            $scope.fetchLookupData("enumValue", 4000, "EnumValueId", "adminTypes", null);
            $scope.fetchLookupData("enumValue", 22000, "EnumValueId", "genders", null);
            $scope.fetchLookupData("enumValue", 6000, "EnumValueId", "emailTypes", null);
            $scope.fetchLookupData("enumValue", 7000, "EnumValueId", "addressTypes", null);
            $scope.fetchLookupData("enumValue", 8000, "EnumValueId", "phoneTypes", null);
            $scope.fetchLookupData("country", 0, "CountryName", "countries", null);
            $scope.fetchLookupData("state", 0, "StateName", "states", null);
            $scope.fetchLookupData("emailSubscription", 0, "EmailSubscriptionId", "emailSubscriptions", null);
        };

        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "company") {
                $scope.entity.contact.companyId = output.data[0].companyId;
                $scope.entity.contact.companyName = output.data[0].companyName;
            }
        };

        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "company") {
                $scope.entity.contact.companyId = "";
                $scope.entity.contact.companyName = "";
            }
        };

        $scope.changeCountry = function () {
            $scope.fetchLookupData("state", 0, "StateName", "states", null);
        };

        //#endregion

        //#region Detail

        $scope.user = { emailSubscriptions: [] };
        $scope.userIcon = "";

        $scope.updateDisplayName = function () {

            if (!$scope.entity.isNameEditable) {
                $scope.entity.displayName = Utility.getName($scope.entity.contact);
            }
        };

        $scope.afterPerformAction = function (source, fromList) {

            var action = source.currentTarget.attributes["action"].value;

            switch (action) {
                case "add":
                    $scope.entity.userTypeId = appConfig.defaultUserTypeId;
                    $scope.entity.adminTypeId = appConfig.defaultAdminTypeId;
                    $scope.entity.contact = {};
                    $scope.entity.contact.location = {};
                    $scope.entity.contact.location.emails = [initSubInfo("emails")];
                    $scope.entity.contact.location.addresses = [initSubInfo("addresses")];
                    $scope.entity.contact.location.phones = [initSubInfo("phones")];
                    $scope.user.emailSubscriptions = [];
                    break;
                default:
                    //TODO
                    break;
            }
        };

        $scope.dynamicRow = function (type, isAdded, target) {

            if (isAdded) {
                $scope.entity.contact.location[type].push(initSubInfo(type));
            }
            else {
                $scope.entity.contact.location[type] = $scope.entity.contact.location[type].filter(function (item) { return item !== target; });
            }
        };

        $scope.setDefault = function (type, target) {

            $scope.entity.contact.location[type].forEach(function (item) {
                item.isDefault = (item === target);
            });
        };

        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.emailSubscriptions = $.map($scope.user.emailSubscriptions, function (o) {
                    return {
                        usersEmailSubscriptionId: -1,
                        userId: -1,
                        emailSubscriptionId: o.emailSubscriptionId
                    };
                });
            }
            else {
                $scope.entity.emailSubscriptions = $.map($scope.user.emailSubscriptions, function (o) {
                    return {
                        usersEmailSubscriptionId: Utility.inPropertyArray($scope.entity.emailSubscriptions, "emailSubscriptionId", o.emailSubscriptionId, "usersEmailSubscriptionId"),
                        userId: $scope.entity.userId,
                        emailSubscriptionId: o.emailSubscriptionId
                    };
                });
            }
        };

        $scope.afterSave = function (action) {

            setUserIcon();

            if ($scope.entity.contact.location.emails.length == 0) {
                $scope.entity.contact.location.emails = [initSubInfo("emails")];
            }
            if ($scope.entity.contact.location.addresses.length == 0) {
                $scope.entity.contact.location.addresses = [initSubInfo("addresses")];
            }
            if ($scope.entity.contact.location.phones.length == 0) {
                $scope.entity.contact.location.phones = [initSubInfo("phones")];
            }

            $scope.user.emailSubscriptions = $.grep($scope.lookups.emailSubscriptions, function (o) {
                return $scope.entity.emailSubscriptions.some(function (s) {
                    return (o.emailSubscriptionId == s.emailSubscriptionId);
                });
            });
        };

        $scope.afterGetDetail = function () {

            setUserIcon();

            if ($scope.entity.contact.location.emails.length == 0) {
                $scope.entity.contact.location.emails = [initSubInfo("emails")];
            }
            if ($scope.entity.contact.location.addresses.length == 0) {
                $scope.entity.contact.location.addresses = [initSubInfo("addresses")];
            }
            if ($scope.entity.contact.location.phones.length == 0) {
                $scope.entity.contact.location.phones = [initSubInfo("phones")];
            }

            $scope.user.emailSubscriptions = [];
            $scope.user.emailSubscriptions = $.grep($scope.lookups.emailSubscriptions, function (o) {
                return $scope.entity.emailSubscriptions.some(function (s) {
                    return (o.emailSubscriptionId == s.emailSubscriptionId);
                });
            });
        };

        $scope.changeUserType = function (userTypeId) {

            if (userTypeId != 3001) {
                $scope.entity.adminTypeId = appConfig.defaultAdminTypeId;
            }
        };

        $scope.uploadFiles = function ($files) {

            if ($files) {
                $files.forEach(function ($file) {
                    if ($file.size > 10485760) {
                        ngNotifier.error("File cannot be uploaded, due to it exceed the 10 MB limit.");
                    }
                    else {
                        entityService.uploadIcon({ UserId: ($scope.entity.userId || 0) }, $file).then(
                            function (output) {
                                if (output.data.output.resultId == 1001) {
                                    var userIcon = JSON.parse(output.data.output.data);
                                    $scope.entity.iconFileName = userIcon.iconFileName;
                                    $scope.entity.iconFileType = $file.type;
                                    setUserIcon();
                                }
                                else {
                                    ngNotifier.error(output.data.output.messages);
                                }
                            },
                            function (output) {
                                ngNotifier.error(output.data.output.messages);
                            }
                        );
                    }
                });
            }
        };

        $scope.deleteFile = function ($files) {
            $files = null;
            ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                entityService.deleteIcon($scope.entity).then(
                    function (output) {
                        $scope.entity.iconFileName = null;
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });
            });
        };

        //#endregion

        //#region Users - Site

        var openLookupSite = function (action, siteId, siteInfo) {

            var modalInstance = $uibModal.open({
                animation: false,
                size: "lg",
                templateUrl: "app/components/security/usersSite/detail.html",
                controller: "usersSiteController",
                resolve: {
                    requestData: function () {
                        return {
                            action: action,
                            userId: $scope.entity.userId,
                            siteId: siteId,
                            siteInfo: siteInfo,
                            lastAccess: $scope.entity.lastAccess,
                            siteCount: $scope.sitesTable.data.length
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.data) {
                        ngNotifier.show(output.data);
                    }
                    if (output.resultId == 1001) {
                        $scope.getDetail();
                    }
                    if (action == "addC" || action == "editC") {
                        $scope.sitesTable.reload();
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        }

        var remove = function (siteId) {
            if (siteId > 0) {
                ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                    var usersSite = {
                        userId: $scope.entity.userId,
                        siteId: siteId,
                        lastAccess: $scope.entity.lastAccess
                    };
                    entityService.deleteSite(usersSite).then(
                        function (output) {
                            ngNotifier.show(output.data);
                            if (output.data.resultId == 1001) {
                                $scope.getDetail();
                                $scope.sitesTable.reload();
                            }
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        });
                });
            }
        };

        $scope.siteList = [];
        $scope.roles = {};

        $scope.sitesTable = new NgTableParams(
        {
            page: 1,
            count: 1000,
            sorting: $.parseJSON("{ \"SiteCode\": \"asc\" }")
        }, {
            counts: [],
            getData: function (params) {
                var listParams = {
                    UserId: $scope.entity.userId,
                    Sort: JSON.stringify(params.sorting()),
                    Filter: "[]"
                };
                return entityService.listSite(listParams).then(
                    function (output) {
                        $scope.siteList = output.data.data;
                        params.total(output.data.count);
                        return output.data.data;
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });

        $scope.performSubAction = function (source, target) {

            var action = source.currentTarget.attributes["action"].value;

            $scope.$broadcast("show-errors-check-validity");

            if (action != "cancelC" && $scope.form.subdetail != undefined && $scope.form.subdetail.$invalid) {
                if ($scope.form.subdetail.$error.required != undefined && $scope.form.subdetail.$error.required.length > 0) {
                    ngNotifier.error("Required Field(s) are missing data."]);
                }
                return;
            }

            switch (action) {

                case "viewDetailC":
                    openLookupSite(action, target.siteId, target);
                    break;

                case "addC":
                    openLookupSite(action, null, null);
                    break;

                case "editC":
                    openLookupSite(action, target.siteId, target);
                    break;

                case "deleteC":
                    remove(target.siteId);
                    break;
            }
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("usersController", controller);

});
