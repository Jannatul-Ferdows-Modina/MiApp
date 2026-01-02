// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "aestemplateService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        $scope.page = appUrl.aestemplate;
        $scope.tabs = appUrl.aestemplate.tabs;
        $scope.initDropdown = function () {
            $scope.tempType = [
                { optionValue: "--Select--", optionName: "--Select--" },
                { optionValue: "USPPI", optionName: "USPPI Template" },
                { optionValue: "Ultimate", optionName: "Ultimate Consignee" }
            ];
            $scope.searchOptions = [
                { optionValue: "--All--", optionName: "--All--" },
                { optionValue: "USPPI", optionName: "USPPI Company" },
                { optionValue: "Ultimate", optionName: "Ultimate Consignee" }
            ];
          $scope.entity.tempType = '--Select--';
        };
        $scope.selectOption = "--All--";
        $scope.searchBox = "";
        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox
                
        };
        $scope.templatelistTable = new NgTableParams(
            {
                page: 1,
                count: 10,
                sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")
            }, {
            getData: function (params) {
                var listParams = {
                    SiteId: $scope.$parent.selectedSiteId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),
                    Filter: JSON.stringify($scope.searchParam)
                };

                    var dataitems = entityService.getAesTemplateList(listParams).then(
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
        $scope.performBookingSearch = function (source, selectOption, searchBox) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox
            };

            $scope.templatelistTable.reload();
        };
        $scope.performBookingAction = function (source, fromList) {

            var action = source.currentTarget.attributes["action"].value;


            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {

                ///$scope.showBookingDetail(action, documentCommonID);

            } else {
                initControls(action);
            }

            //switchTab("Detail", action);

            switch (action) {
                case "search":
                    filterList();
                    break;
                case "add":
                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    //$scope.getDocumentAttachmentDetail(action, documentCommonID);
                    break;
                case "copy":
                    //lastAction = action;
                    //$scope.entity.enquiryID = 0;                    
                    break;
                    //lastAction = 'copy';
                    //$scope.entityId = 0;
                    //$scope.entity = {};
                    //$("input[input-date]").each(function (index, element) { $(element).val(null); });
                    break;
                case "edit":
                    //lastAction = action;
                    break;
                case "save":
                    save(action);
                    break;
                case "saveEmail":
                    $scope.entity.isSendEmailNow = true;
                    save(action);
                    break;
                case "cancel":

                    $scope.goBack();
                    break;
                case "delete":
                    remove();
                    //lastAction = "";
                    break;
                case "deleteBatch":

                    //lastAction = "";
                    break;
                case "verify":
                case "activate":
                case "deactivate":
                    $scope.changeStatus(action);
                    lastAction = "";
                    break;
                default:
                    //lastAction = "";
                    break;
            }

            if ($scope.afterPerformAction != undefined) {
                $scope.afterPerformAction(source, fromList);
            }
        };
        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
        };
        var initControls = function (action) {

            switch (action) {
                case "add":
                    $scope.editMode = true;
                    $scope.disabledInsert = false;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = true;
                    $scope.requiredUpdate = true;
                    break;
                case "copy":
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
        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":

                    break;
                case "edit":
                    //$scope.getcarrierAllRates($scope.entity.enquiryID);


                    break;

            }
        };
        $scope.searchValues = function (viewValue, selectType, searchRouteType) {
            var resultItem = {};
            var lookupModule;
            var routeType = "";
            var lookupField = "name";
            if (selectType == "companyName") {

                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getCompanySearch(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}

                            resultItem.name = o.companyName;
                            resultItem.contactID = o.contactID;
                            resultItem.address = o.address;
                            resultItem.fullAddress = o.fullAddress;
                            resultItem.countryName = o.countryName
                            resultItem.countryCode = o.countryCode
                            resultItem.stateName = o.stateName
                            resultItem.stateCode = o.stateCode
                            resultItem.cityName = o.cityName
                            resultItem.zipCode = o.zipCode
                            resultItem.email = o.email
                            resultItem.telNo = o.telNo
                            resultItem.firstName = o.firstName
                            resultItem.lastName = o.lastName
                            resultItem.contactPerson = o.contactPerson
                            resultItem.idNumber = o.idNumber
                            resultItem.irsNumber = o.irsNumber
                            resultItem.idNumberType = o.idNumberType
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }


            if (selectType == "country") {
                lookupModule = "Booking";
                lookupField = "countryDestination";

                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getCountry(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}
                            resultItem.itemName = o.itemName;
                            resultItem.itemId = o.itemId;
                            resultItem.itemCode = o.itemCode;
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }
            if (selectType == "state") {
                lookupModule = "Booking";
                lookupField = "originState";

                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getState(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}
                            resultItem.itemId = o.itemId;
                            resultItem.itemName = o.itemName;
                            resultItem.itemCode = o.itemCode;
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }
        };
        $scope.selectTypeaheadsli = function ($item, $model, $label, source, stype) {

            var lookupModule = null;
            var lookupIndex = null;
            var target = $(source.currentTarget);


            if (stype == "country") {
                $scope.entity.countryId = $item.itemId;
                $scope.entity.countryCode = $item.itemCode;
            }

            if (stype == "state") {
                $scope.entity.stateId = $item.itemId;
                $scope.entity.stateCode = $item.itemCode;

            }
            if (stype == "companyName") {
                $scope.entity.companyId = $item.contactID;
                $scope.entity.firstName = $item.firstName;
                $scope.entity.lastName = $item.lastName;
                $scope.entity.addressLine1 = $item.fullAddress;
                $scope.entity.countryName = $item.countryName
                $scope.entity.countryCode = $item.countryCode
                $scope.entity.stateName = $item.stateName
                $scope.entity.stateCode = $item.stateCode
                $scope.entity.cityName = $item.cityName
                $scope.entity.zipCode = $item.zipCode
                $scope.entity.phoneNumber = $item.telNo


            }

            if (source.type == "click") {
                target = $(source.currentTarget).parent().parent().find("input");
            }

            var lookupModule = target.attr("lookup-module");
            var lookupIndex = target.attr("lookup-index");

            var output = { data: [] };
            output.data.push($item);

            if ($scope.setLookups != undefined) {
                $scope.setLookups(source, lookupModule, output, lookupIndex);
            }
        };
        $scope.saveAesTemplate = function () {
            var errormsg = "";
            $scope.$broadcast("show-errors-check-validity");
            if ($scope.entity.companyName == "" || $scope.entity.companyName == undefined) {
                errormsg += "<li>Company Name is required.</li>";

            }
            if ($scope.entity.firstName == "" || $scope.entity.firstName == undefined) {
                errormsg += "<li>First Name is required.</li>";

            }
            if ($scope.entity.lastName == "" || $scope.entity.lastName == undefined) {
                errormsg += "<li>Last Name is required.</li>";

            }
            if ($scope.entity.phoneNumber == "" || $scope.entity.phoneNumber == undefined) {
                errormsg += "<li>Phone No is required.</li>";

            }
            if ($scope.entity.addressLine1 == "" || $scope.entity.addressLine1 == undefined) {
                errormsg += "<li>Address line 1 is required.</li>";
            }
            if ($scope.entity.templateType == "Ultimate") {
                if ($scope.entity.countryName == "" || $scope.entity.countryName == undefined) {
                    errormsg += "<li>Country Name is required.</li>";
                }
            }
            if ($scope.entity.postalCode == "" || $scope.entity.postalCode == undefined) {
                errormsg += "<li>Postal Code is required.</li>";
            }
            if ($scope.entity.cityName == "" || $scope.entity.cityName == undefined) {
                errormsg += "<li>City Name is required.</li>";
            }
            if ($scope.entity.stateName == "" || $scope.entity.stateName == undefined) {
                errormsg += "<li>State Name is required.</li>";
            }
            if ($scope.entity.templateType == "" || $scope.entity.templateType == undefined || $scope.entity.templateType == "--Select--") {
                errormsg += "<li>Template Type is required.</li>";
            }
            if (errormsg != "") {
                var fullerrormsg = "<ol>" + errormsg + "</ol>";
                //alert(errormsg);
                alertMessage(fullerrormsg);
                return true;
            }
            
            $scope.entity.createdBy = $scope.$parent.authentication.userId;
            entityService.saveAesTemplate($scope.entity).then(
                function (output) {
                    $scope.entity = {};
                    $scope.searchBox = "";
                    $scope.templatelistTable.reload();
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
                }

            );
        };
        $scope.getAesTemplateDetail = function (source, id) {
            var action = source.currentTarget.attributes["action"].value;
            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entity = {};
            $scope.entity.templateId = id;
            entityService.getAesTemplateDetail($scope.entity).then(
                function (output) {
                    if (output.data.resultId == 2005) {
                        ngNotifier.showError($scope.authentication, output);
                        $scope.logOut()
                    }

                    if (output.data.data != null) {
                        $scope.entity = output.data.data;
                    }

                   
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };
        var alertMessage = function (msg) {
            var entity = {};
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/ees/eesSub/message.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                    $scope.cancel = 1;
                    $scope.msg = msg;
                    $scope.select = function (action) {

                        if (action == 'cancel') {
                            $scope.cancel = 0;
                            return;
                        }
                        $uibModalInstance.close();
                    };
                },
                resolve: {
                    requestData: function () {
                        return {

                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                   
                    if (output == "close") {

                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };
    controller.$inject = injectParams;
    app.register.controller("aestemplateController", controller);

});
