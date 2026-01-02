// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "bookingCaptureContainerAbandonementService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
       
        $scope.$groupsv = [];

        $scope.page = appUrl.bookingCaptureContainerAbandonement;
        $scope.tabs = appUrl.bookingCaptureContainerAbandonement.tabs;

        $scope.$parent.pageTitle = "Capture Container # , Seal # & Container Status at Destination";
        $scope.$parent.breadcrumbs = ["Documentation", "Container Abandonement", "Capture Container # , Seal # & Container Status at Destination"];

        $scope.dispatchContainerList = [];
        $scope.actionRemarksList = [];

        $scope.lookups = { siplDepartments: [] };

        $scope.initDropdown = function () {
            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
        };
        $scope.searchValues = function (viewValue, selectOption) {
            var resultItem = {};
            if (selectOption == "customer") {
                return $scope.callTypeahead(viewValue, 'SIPLContact', 'companyName', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.companyName;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
        };
        //#endregion       

        $scope.searchOptions = [
                { optionValue: "", optionName: "-All-" },
                { optionValue: "customer", optionName: "Customer" },
                { optionValue: "SystemRefNo", optionName: "System Ref No" },               
                { optionValue: "LineBookingNo", optionName: "Line Booking No" }
        ];

        $scope.filterBookings = function () {
            var dashboardOption = localStorageService.get("dashboardOption");
            $scope.selectOption = "companyName";
            $scope.searchBox = "";
            if (dashboardOption != null) {
                if (dashboardOption == 'TCCSU') {
                    $scope.dashboardOption = "TCCSU";
                }
                if (dashboardOption == 'TCCSF') {
                    $scope.dashboardOption = "";
                }

                localStorageService.remove("dashboardOption");
            }
            else {
                $scope.dashboardOption = "";
            }

        };
        $scope.filterBookings();
        $scope.selectOption = "SystemRefNo";
        $scope.searchBox = "";
        $scope.departmentID = 0;
        $scope.destuffed = "";
        $scope.DateofDestuffedFrom = "";
        $scope.DateofDestuffedTo = "";
        
        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            departmentID: 0,
            dashboardOption: $scope.dashboardOption,
            destuffed: $scope.destuffed,
            DateofDestuffedFrom: $scope.DateofDestuffedFrom,
            DateofDestuffedTo: $scope.DateofDestuffedTo
        };

        

        $scope.bookinglistTable = new NgTableParams(
              {
                  page: 1,
                  count: 10,
                  sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }"),
                  group: {
                      documentCommonID: "asc"
                  }
              }, {
                  counts: [],
                  getData: function (params) {
                      var listParams = {
                          SiteId: $scope.$parent.selectedSiteId,
                          ModuleId: 0,
                          PageIndex: params.page(),
                          PageSize: params.count(),
                          Sort: JSON.stringify(params.sorting()),
                          Filter: JSON.stringify($scope.searchParam)

                      };

                      return entityService.getDispatchContainerList(listParams).then(
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
        $scope.performBookingSearch = function (source, selectOption, searchBox, departmentValue, destuffed, DateofDestuffedFrom, DateofDestuffedTo) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox,
                departmentID: departmentValue,
                dashboardOption: "",
                destuffed: destuffed,
                DateofDestuffedFrom: DateofDestuffedFrom,
                DateofDestuffedTo: DateofDestuffedTo

            };

            if (action == 'search') {
                $scope.bookinglistTable.reload();
            } else if (action == 'exportReport') {

                var listParams = {
                    SiteId: $scope.$parent.selectedSiteId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 0,
                    PageSize: 10,
                    Sort: JSON.stringify($.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")),
                    Filter: JSON.stringify($scope.searchParam)

                };
                entityService.exportReport(listParams).then(
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
        };

        $scope.performBookingAction = function (source, fromList) {

            var action = source.currentTarget.attributes["action"].value;

            //$scope.$broadcast("show-errors-check-validity");

            //if (action != "cancel" && $scope.form.detail != undefined && $scope.form.detail.$invalid) {
            //    if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
            //        ngNotifier.error("Required Field(s) are missing data.");
            //    }
            //    else if ($scope.form.detail.usrPwdC.$invalid) {
            //        ngNotifier.error("Password do not match with Confirm Password.");
            //    }
            //    return;
            //}

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
                case "exportReport":
                    exportReport();
                    break;
                case "add":
                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    $scope.showDocumentDetail(action, documentCommonID, fileno, refno, blnumber);
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
                    //$scope.showQuotationDetail(action, enquiryID, quotationID);
                    //$scope.showEnquiryDetail('viewDetail', $scope.entity.enquiryID, $scope.entity.isComplete);
                    //lastAction = "";
                    $scope.goBack();
                    break;
                case "delete":
                    remove();
                    //lastAction = "";
                    break;
                case "deleteBatch":
                    removeBatch(enquiryID, quotationID);
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
                    $scope.getcarrierAllRates($scope.entity.enquiryID);


                    break;

            }
        };

        var removeBatch = function (enquiryID, quotationID) {
            var entity = {};
            entity.enquiryID = enquiryID;
            entity.quotationID = quotationID;

            ngNotifier.confirm("Are you sure you want to DELETE Quotation?", null, function () {
                var modalInstance = $uibModal.open({
                    animation: false,
                    backdrop: "static",
                    keyboard: false,
                    size: "lg",
                    templateUrl: "app/components/operation/quotationApproval/cancelRemarks.html",
                    controller: function ($scope, $timeout, $uibModalInstance, requestData) {

                        $scope.select = function (action) {
                            //$scope.deleteRemarks = deleteRemarks.value;
                            if (action == 'delete' && $scope.deleteRemarks == null) {
                                ngNotifier.error("Please Enter Delete Remarks");
                                return;
                            }
                            var outputData = {}
                            outputData.remarks = $scope.deleteRemarks;
                            outputData.action = action;
                            $uibModalInstance.close(outputData);
                        };
                    },
                    resolve: {
                        requestData: function () {
                            return {
                                deleteRemarks: $scope.deleteRemarks
                            };
                        }
                    }
                });

                modalInstance.result.then(
                    function (output) {
                        if (output.action == "delete") {
                            entity.remarks = output.remarks;
                            entityService.deleteQuotation(entity).then(
                            function (output) {
                                $scope.entity = {};
                                $scope.quotationlistTable.reload();
                                $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });

                        }
                        else if (output == "close") {

                        }
                    },
                    function (output) {
                        ngNotifier.logError(output);
                    });

            });

        };
        $scope.saveDispatchContainerData = function (source, fromList) {

            if ($scope.entity.fileNo == "" || $scope.entity.fileNo == null) {

                ngNotifier.error("Please enter System Ref");
                return;
            }
            if ($scope.dispatchContainerList.length > 0) {
                $scope.entity.dispatchContainerDTOList = $scope.dispatchContainerList;
            }
            else {
                ngNotifier.error("Please enter expense details");
                return;
            }
            
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            $scope.entity.updatedBy = $scope.$parent.authentication.userId;
            entityService.saveDispatchContainerData($scope.entity).then(
                function (output) {
                    //$scope.enquiryID = output.data.data;
                    $scope.entity = {};
                    $scope.dispatchContainerList = [];
                    $scope.actionRemarksList = [];
                    $scope.selectOption = "SystemRefNo";
                    $scope.searchBox = "";
                    $scope.destuffed = "";
                    $scope.DateofDestuffedFrom = "";
                    $scope.DateofDestuffedTo = "";
                    $scope.bookinglistTable.reload();
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

        $scope.showDocumentDetail = function (source, documentCommonID, fileno, refno, blnumber) {
            var action = source.currentTarget.attributes["action"].value;
            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entity = {};
            $scope.entity.documentCommonID = documentCommonID;
            $scope.entity.fileNo = fileno;
            $scope.entity.exportRef = refno;
            $scope.entity.blNumber = blnumber;
            $scope.entity.siteId = $scope.$parent.selectedSiteId;

            entityService.getDispatchContainerDetail($scope.entity).then(
                 function (output) {
                     if (output.data.resultId == 2005) {
                         ngNotifier.showError($scope.authentication, output);
                         $scope.logOut()
                     }
                     $scope.entity = output.data.data;
                     $scope.dispatchContainerList = [];
                     $scope.actionRemarksList = [];
                     
                     if ($scope.entity.dispatchContainerDTOList != null) {
                         $scope.dispatchContainerList = $scope.entity.dispatchContainerDTOList;
                     }
                     if ($scope.entity.nextActionRemarksDTOList != null) {
                         $scope.actionRemarksList = $scope.entity.nextActionRemarksDTOList;
                     }
                     //$scope.afterGetDetail(action);
                 },
                 function (output) {
                     ngNotifier.showError($scope.authentication, output);
                 }
             );
        };

        $scope.showNextActionRemarksModel = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/bookingCaptureContainer/nextActionRemarks.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                    $scope.actionRemarks = requestData.actionRemarks
                    $scope.select = function (action) {
                        var outputData = {}
                        if (action == 'update') {

                        }
                        else {
                            outputData.action = 'close';
                        }
                        $uibModalInstance.close(outputData);
                    };
                },
                resolve: {
                    requestData: function () {
                        return {
                            actionRemarks: $scope.actionRemarksList
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.action == "update") {

                    }
                    else if (output == "close") {

                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        //#endregion


        var exportReport = function () {

            var listParams = {
                SiteId: $scope.$parent.selectedSiteId,
                ModuleId: $scope.page.moduleId,
                PageIndex: params.page(),
                PageSize: params.count(),
                Sort: JSON.stringify(params.sorting()),
                Filter: JSON.stringify($scope.searchParam)

            };
            entityService.exportReport(reportParams).then(
                function (output) {
                    var blobData = new Blob([output.data], { type: output.headers()["content-type"] });
                    var fileName = output.headers()["x-filename"];
                    saveAs(blobData, fileName);
                },
                function (output) {
                    ngNotifier.error(output);
                }
            );
        };

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("bookingCaptureContainerAbandonementController", controller);

});
