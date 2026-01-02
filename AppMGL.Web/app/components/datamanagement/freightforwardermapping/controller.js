"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "freightforwardermappingService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        $scope.page = appUrl.freightforwardermapping;
        $scope.tabs = appUrl.freightforwardermapping.tabs;
        $scope.ffNetworkID;
     
        $scope.setLookups = function (source, lookup, output, index) {

            if (lookup == "freightforwardernetwork") {
                $scope.entity.ffNetworkId = output.data[0].ffNetworkID;
                $scope.entity.ffNetworkName = output.data[0].ffNetworkName;
            }
            if (lookup == "SIPLContact") {
                $scope.entity.contactId = output.data[0].contactID;
                $scope.entity.companyName = output.data[0].companyName;
            }
        }
        $scope.clearLookups = function (source, lookup, index) {
            if (lookup == "freightforwardernetwork") {
                $scope.entity.ffNetworkId = null;
            }
            
        }
        
        $scope.searchOptions = [
            { optionValue: "", optionName: "-All-" },
            { optionValue: "CompanyName", optionName: "Company Name" },
            { optionValue: "CustomerCode", optionName: "Customer Code" }
        ];
        
        $scope.companyNameF = "";
        $scope.siteId = $.map($scope.authentication.userSite, function (o) { return o.SitId; }).join(",");
           $scope.searchParam = {
            companyName: $scope.companyNameF,
            customerCode: $scope.customerCodeF,
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

       
     
        $scope.performCustomerContactSearch = function (source, companyName, customerCode, galRepresentative, contactCategoryID, companyGradation, OriginCountry, commodity, continent, cryName, state, city,siteId) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                companyName: companyName,
                customerCode: customerCode,
                siteId: siteId
            };
            $scope.customerContactlistTable.reload();
        };


        $scope.saveCustomerContact = function (source, fromList) {
            $scope.$broadcast("show-errors-check-validity");


            if ($scope.entity.ffNetworkName == null || $scope.entity.ffNetworkId == null) {
                ngNotifier.error("Please enter Netwrok Name");
                return;
            }
            if ($scope.entity.companyName == null || $scope.entity.contactId == null) {
                ngNotifier.error("Please enter Company Name");
                return;
            }
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.createdBy = $scope.$parent.authentication.userId;
            $scope.entity.ModifiedBy = $scope.$parent.authentication.userId;
            entityService.saveForwarderMapping($scope.entity).then(
                function (output) {
                    $scope.entity = {};
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

        $scope.showContactDetail = function (action, id) {

            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.networkMapId = id;
            $scope.entity.networkMapId = parseInt(id);
            if ($scope.networkMapId > 0) {
                entityService.getForwarderMapping($scope.networkMapId).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }
                        $scope.entity = output.data.data[0];
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
                   // lastAction = action;
                    break;
                case "save":
                    save(action);
                    break;
                case "saveEmail":
                    $scope.entity.isSendEmailNow = true;
                    save(action);
                    break;
                case "cancel":
                    $scope.showContactDetail('viewDetail', $scope.entity.networkMapId);
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
            //switch (action) {
            //    case "add":
            //        $scope.getLatestMappingCode();
            //        break;
            //}

        }
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
        $scope.getLatestMappingCode = function () {

            var getCustomerCode = entityService.getLatestMappingCode().then(
                function (output) {
                    $scope.entity.ffNetworkCode = output.data.data.ffNetworkCode;

                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };
        

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

        var box = $(".dual-list-control");

        
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
        $scope.searchValuesCompany = function (viewValue, selectOption) {
            var resultItem = {};
            var lookupFicompanyNameeld = "";
            var lookupField = "companyName";
            if (selectOption == "companyName") {

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
                            resultItem.companyName = o.companyName;
                            resultItem.contactID = o.contactID;
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }

        };
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };
    controller.$inject = injectParams;
    app.register.controller("freightforwardermappingController", controller);

});
