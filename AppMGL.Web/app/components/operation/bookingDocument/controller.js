// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "bookingDocumentService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.bookingDocument;
        $scope.tabs = appUrl.bookingDocument.tabs;
        $scope.$parent.pageTitle = "Waiting for Line Confirmation";
        $scope.$parent.breadcrumbs = ["Shipment", "Booking", "Waiting for Line Confirmation"];
        
        $scope.actionRemarksList = [];
        $scope.shipmentDocsList = [];

        $scope.lookups = { siplDepartments: [] };

        $scope.initDropdown = function () {
            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
        };
       
        //#endregion       

        $scope.searchOptions = [
                { optionValue: "", optionName: "-All-" },                
                { optionValue: "SystemRefNo", optionName: "System Ref No" },                
                { optionValue: "LineBookingNo", optionName: "Line Booking No" }
        ];

        $scope.filterBookings = function () {
            var dashboardOption = localStorageService.get("dashboardOption");
            $scope.selectOption = "companyName";
            $scope.searchBox = "";
            if (dashboardOption != null) {
                if (dashboardOption == 'BWLCU') {
                    $scope.dashboardOption = "BWLCU";
                }

                if (dashboardOption == 'BWLCF') {
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
        $scope.galBookingStatusID = 7;

        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            departmentID: 0,
            galBookingStatusID: 7,
            dashboardOption: $scope.dashboardOption
        };

        

        $scope.bookinglistTable = new NgTableParams(
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

                      var dataitems = entityService.getDocumentList(listParams).then(
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
        $scope.performBookingSearch = function (source, selectOption, searchBox, departmentValue) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox,
                departmentID: 0,
                galBookingStatusID: 7,  //Pending for Confirmation from line 
                dashboardOption:""
            };
            $scope.bookinglistTable.reload();
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
                case "add":
                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    $scope.getDocumentAttachmentDetail(action, documentCommonID);
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
                    deleteShipmentDoc(enquiryID, quotationID);
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

        $scope.deleteShipmentDoc = function (rownum,documentCommonID, filename) {
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            $scope.entities.docName = filename;
            $scope.entities.documentType = "Booking";
            ngNotifier.confirm("Are you sure you want to DELETE Document?", null, function () {
                $scope.shipmentDocsList.splice(rownum, 1);
                entityService.deleteShipmentDoc($scope.entities).then(
                            function (output) {
                                
                                //$scope.entity = {};
                                //$scope.quotationlistTable.reload();
                                //$scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });


            });

        };

        $scope.saveDocumentAttachement = function (source, fromList) {

            if ($scope.entity.fileNo == "" || $scope.entity.fileNo == null) {
                ngNotifier.error("Please enter System Ref");
                return;
            }
            
            if ($scope.entity.vessel == "" || $scope.entity.vessel == null) {
                ngNotifier.error("Please enter vessel");
                return;
            }
            if ($scope.entity.cutOffDate == "" || $scope.entity.cutOffDate == null) {
                ngNotifier.error("Please select Document cutOffDate");
                return;
            }
            var isValid = true;
            if ($scope.entity.customerfile != null) {

                if ($scope.entity.customerfile.length == 0) {
                    ngNotifier.error("Please attach atleast one file");
                    return;
                }
                else {
                    $scope.entity.customerfile.forEach(function (file) {

                        if (file) {

                            if (file.size > 10485760) {
                                ngNotifier.error("File cannot exceeds more than 10 MB size.");
                                isValid = false;
                            }
                            else if (file.type != "application/pdf" && file.type != "application/docx" && file.type != "application/doc" && file.type != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && file.type != "application/xlsx" && file.type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                                ngNotifier.error("Please upload only these file types - pdf,docx,doc,xlsx");
                                isValid = false;
                                return;
                            }
                        }
                    });
                    if (isValid == true) {
                        var fileItem = {};
                        $scope.entity.shipmentDocsDTOList = [];
                        $scope.entity.customerfile.forEach(function (file) {
                            fileItem = {};
                            fileItem.docName = file.name;
                            $scope.entity.shipmentDocsDTOList.push(fileItem);
                        });
                    }

                    //$scope.entity.shipmentDocsDTOList = $scope.shipmentDocsList;
                }
            }
            if (isValid == true) {
                $scope.entity.galBookingStatusID = 18;  //Pending Shipper Confirmation Email
                $scope.entity.siteId = $scope.$parent.selectedSiteId;
                $scope.entity.createdBy = $scope.$parent.authentication.userId;
                $scope.entity.updatedBy = $scope.$parent.authentication.userId;
                entityService.saveDocumentAttachement($scope.entity).then(
                    function (output) {
                        $scope.documentID = output.data.data;
                        if ($scope.entity.customerfile != null) {
                            $scope.uploadAttachment($scope.documentID[0]);
                        }
                        $scope.entity = {};
                        $scope.actionRemarksList = [];
                        $scope.selectOption = "SystemRefNo";
                        $scope.searchBox = "";
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
            }
        };

        $scope.getDocumentAttachmentDetail = function (source, documentCommonID) {
            var action = source.currentTarget.attributes["action"].value;
            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entity = {};
            $scope.entity.documentCommonID = documentCommonID;            

            entityService.getDocumentAttachmentDetail($scope.entity).then(
                 function (output) {
                     if (output.data.resultId == 2005) {
                         ngNotifier.showError($scope.authentication, output);
                         $scope.logOut()
                     }
                     $scope.entity = output.data.data;
                     if ($scope.entity.isPublic == 1)
                     {
                         $scope.entity.isPublic = "1";
                     }
                     else {
                         $scope.entity.isPublic = "0";
                     }
                     $scope.actionRemarksList = [];                     
                     
                     if ($scope.entity.shipmentDocsDTOList != null) {
                         $scope.shipmentDocsList = $scope.entity.shipmentDocsDTOList;
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

        $scope.uploadAttachment = function (documentCommonID) {
            $scope.entity.customerfile.forEach(function (file) {
           
                if (file) {

                    if (file.size > 10485760) {
                        ngNotifier.error("File cannot exceeds more than 10 MB size.");
                    }
                    else if (file.type != "application/pdf" && file.type != "application/docx" && file.type != "application/doc" && file.type != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && file.type != "application/xlsx" && file.type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                        ngNotifier.error("Please upload only these file types - pdf,docx,doc,xlsx");
                        return;
                    }
                    else {
                        var attachment = {
                            DisplayName: documentCommonID + '_' + file.name,
                            DocumentCommonID: documentCommonID,
                            DocumentType: 'Booking'
                        };
                        entityService.uploadFile(attachment, file).then(
                            function (output) {
                                //ngNotifier.show(output.data.output);
                            },
                            function (output) {
                                ngNotifier.error(output.data.output.messages);
                            }
                        );
                    }

                }
            });

        };

        $scope.downloadAttachment = function (documentCommonID,filename) {
            //window.open('', '_blank', '');
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            $scope.entities.attachFile = filename;
            
            if ($scope.entities.attachFile != null) {
                entityService.downloadAttachment($scope.entities).then(
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

        $scope.exportReport = function()
        {            
            var reportParams = {                
                SitId: $scope.$parent.selectedSiteId
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
        }



        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("bookingDocumentController", controller);

});
