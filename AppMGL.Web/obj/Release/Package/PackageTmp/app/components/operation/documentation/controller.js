// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "documentationService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.documentation;
        $scope.tabs = appUrl.documentation.tabs;

        //$scope.$parent.pageTitle = "Awaited Shipper Confirmation";
        //$scope.$parent.breadcrumbs = ["Shipment", "Booking", "Awaited Shipper Confirmation"];
        
        $scope.actionRemarksList = [];
        $scope.shipmentDocsList = [];
        $scope.isInvalidData = false;
        $scope.lookups = { siplDepartments: [] };
        $scope.dos_Id = 1;
        $scope.ckOptions = {
            toolbar: [
                { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline'] },
                { name: 'insert', items: ['Image', 'Link', 'Unlink'] },
                { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                { name: 'colors', items: ['TextColor', 'BGColor'] },
                { name: 'indent', groups: ['list', 'indent', 'align'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] }
            ]
        };

        $scope.initDropdown = function () {
            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
        };
       
        //#endregion       

        $scope.searchOptions = [
                { optionValue: "", optionName: "-All-" },                
                { optionValue: "SystemRefNo", optionName: "System Ref No" },
                { optionValue: "BookingNo", optionName: "Booking No" },
                { optionValue: "CustomerName", optionName: "Customer Name" },
                { optionValue: "CarrierName", optionName: "Carrier Name" }               
        ];
       

        $scope.updateControls = function () {
            $scope.selectOption = "SystemRefNo";
            $scope.searchBox = "";
            $scope.dashboardOption = "";
            if ($scope.$parent.pageTitle == "Pending Shipping Instruction")
            {
                $scope.dos_Id = 2;
            }
            if ($scope.$parent.pageTitle == "Pending filing of SED") {
                $scope.dos_Id = 3;
            }
            if ($scope.$parent.pageTitle == "Pending B/L instruction Customer") {
                $scope.dos_Id = 4;
            }
            if ($scope.$parent.pageTitle == "Pending B/L instruction Line") {
                $scope.dos_Id = 5;
            }
            if ($scope.$parent.pageTitle == "Pending Draft B/L Line") {
                $scope.dos_Id = 6;
            }
            if ($scope.$parent.pageTitle == "Pending Send Draft B/L Customer") {
                $scope.dos_Id = 7;
            }
            if ($scope.$parent.pageTitle == "Pending Draft B/L Approval From Customer") {
                $scope.dos_Id = 8;
            }
            if ($scope.$parent.pageTitle == "Pending to Send Approved Draft B/L to Line") {
                $scope.dos_Id = 9;
            }
            if ($scope.$parent.pageTitle == "B/L Release Awaited From Line") {
                $scope.dos_Id = 10;
            }
        };

        $scope.filterBookings = function () {
            var dashboardOption = localStorageService.get("dashboardOption");
            $scope.selectOption = "companyName";
            $scope.searchBox = "";
            if (dashboardOption != null) {
                if (dashboardOption == 'BASCU') {
                    $scope.dashboardOption = "BASCU";
                }

                if (dashboardOption == 'BASCF') {
                    $scope.dashboardOption = "";
                }

                localStorageService.remove("dashboardOption");
            }
            else {
                $scope.dashboardOption = "";
            }

        };

        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {
        }

        $scope.clearLookups = function (source, lookup, index) {
        }
        $scope.getEmailIds = function (viewValue, lookupModule, lookupField1, lookupMethod) {
            var resultItem = {};
            return entityService.getEmailIds(viewValue).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}
                            resultItem.email = o.email;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );

        };
        $scope.updateControls();
        

        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            dos_Id: $scope.dos_Id,
            dashboardOption: $scope.dashboardOption
        };
        
        $scope.documentationlistTable = new NgTableParams(
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

                      var dataitems = entityService.getDocumentationList(listParams).then(
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
                seachValue: searchBox,
                dos_Id: $scope.dos_Id,
                dashboardOption: $scope.dashboardOption
            };

            $scope.documentationlistTable.reload();
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

        

        $scope.saveDocumentationDetail = function (source, fromList) {

            
            if ($scope.$parent.pageTitle == "Pending Shipping Instruction") {
                if ($scope.entity.doc_ReceiptDate == "" || $scope.entity.doc_ReceiptDate == null) {
                    ngNotifier.error("Please select Shipping Instruction Received Date1");
                    return;
                }
            }
            if ($scope.$parent.pageTitle == "Pending filing of SED") {
                if ($scope.entity.doc_IS_AES_ITN_REQ == false) {
                    if ($scope.entity.doc_AES_ITN == "" || $scope.entity.doc_AES_ITN == null) {
                        ngNotifier.error("Please enter AES-ITN#");
                        return;
                    }
                }
                else
                {
                    $scope.entity.doc_ReceiptDate = "";
                }
            }
            if ($scope.$parent.pageTitle == "Pending B/L instruction Customer") {
                if ($scope.entity.doc_ReceiptBLInstructionDate == "" || $scope.entity.doc_ReceiptBLInstructionDate == null) {
                    ngNotifier.error("Please select Capture receipt of B/L instruction");
                    return;
                }
            }
            if ($scope.$parent.pageTitle == "Pending B/L instruction Line") {
                if ($scope.entity.doc_BLInstructionLineDate == "" || $scope.entity.doc_BLInstructionLineDate == null) {
                    ngNotifier.error("Please select BL Instruction Submitted Date");
                    return;
                }
            }
            if ($scope.$parent.pageTitle == "Pending Draft B/L Line") {
                if ($scope.entity.doc_DraftBLLineDate == "" || $scope.entity.doc_DraftBLLineDate == null) {
                    ngNotifier.error("Please select Date Of Proof BL");
                    return;
                }
            }
            if ($scope.$parent.pageTitle == "Pending Send Draft B/L Customer") {
                //if ($scope.entity.doc_DraftBLCustomerDate == "" || $scope.entity.doc_DraftBLCustomerDate == null) {
                //    ngNotifier.error("Please enter email");
                //    return;
                //}
            }
            if ($scope.$parent.pageTitle == "Pending Draft B/L Approval From Customer") {
                if ($scope.entity.doc_DraftBLApprovalDate == "" || $scope.entity.doc_DraftBLApprovalDate == null) {
                    ngNotifier.error("Please select Customer Proof Read Date");
                    return;
                }
            }
            if ($scope.$parent.pageTitle == "Pending to Send Approved Draft B/L to Line") {
                if ($scope.entity.doc_ApprovedDraftBLToLineDate == "" || $scope.entity.doc_ApprovedDraftBLToLineDate == null) {
                    ngNotifier.error("Please select Proof BL Confirmed Date");
                    return;
                }
            }
            if ($scope.$parent.pageTitle == "B/L Release Awaited From Line") {
                if ($scope.entity.doc_BLReleaseAwaitedFromLineDate == "" || $scope.entity.doc_BLReleaseAwaitedFromLineDate == null) {
                    ngNotifier.error("Please select Final Released Date");
                    return;
                }
            }

            var isValid = true;
            var fileCount = 0;
            if ($scope.entity.customerfile != null) {

                if ($scope.entity.customerfile.length == 0) {
                    ngNotifier.error("Please attach atleast one file");
                    return;
                }
                else {
                    $scope.entity.customerfile.forEach(function (file) {

                        if (file) {
                            fileCount = fileCount + 1;

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
                            fileItem.docName = 'DOC_' + $scope.entity.dos_Id+ '_' +file.name;
                            $scope.entity.shipmentDocsDTOList.push(fileItem);
                        });
                    }

                    //$scope.entity.shipmentDocsDTOList = $scope.shipmentDocsList;
                }
            }
            if (isValid == true) {                
                $scope.entity.siteId = $scope.$parent.selectedSiteId;
                $scope.entity.createdBy = $scope.$parent.authentication.userId;
                $scope.entity.updatedBy = $scope.$parent.authentication.userId;
                entityService.saveDocumentationDetail($scope.entity).then(
                    function (output) {
                        $scope.documentID = output.data.data;
                        if ($scope.entity.customerfile != null) {
                            $scope.uploadAttachment($scope.documentID[0], fileCount);
                        }
                        if ($scope.entity.dos_Id != 7) {
                            $scope.entity = {};
                            $scope.actionRemarksList = [];
                            $scope.selectOption = "SystemRefNo";
                            $scope.searchBox = "";
                            $scope.documentationlistTable.reload();
                            $scope.goBack();
                            ngNotifier.show(output.data);
                        }
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

        $scope.getDocumentationDetail = function (source, doc_Id, documentCommonID) {
            var action = source.currentTarget.attributes["action"].value;
            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entity = {};
            $scope.entity.doc_Id = doc_Id;
            $scope.entity.documentCommonID = documentCommonID;
            $scope.entity.quotationID = 0
            $scope.entity.enquiryID = 0
            entityService.getDocumentationDetail($scope.entity).then(
                 function (output) {
                     if (output.data.resultId == 2005) {
                         ngNotifier.showError($scope.authentication, output);
                         $scope.logOut()
                     }
                     $scope.entity = output.data.data;                     
                     $scope.actionRemarksList = [];                     
                     
                     if ($scope.entity.nextActionRemarksDTOList != null) {
                         $scope.actionRemarksList = $scope.entity.nextActionRemarksDTOList;
                     }
                     $scope.entity.doc_IS_AES_ITN_REQ = false;
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

        $scope.uploadAttachment = function (documentCommonID, fileCount) {
            var uploadCount = 0;
            //$scope.deleteBookingConfDocs();
            if ($scope.entity.customerfile != null) {
                $scope.entity.customerfile.forEach(function (file) {

                    var attachment = {
                        DisplayName: documentCommonID + '_DOC_' + $scope.entity.dos_Id + '_' + file.name,
                        DocumentCommonID: documentCommonID,
                        DocumentType: $scope.entity.dos_Id
                    };
                    uploadCount = uploadCount + 1;
                    entityService.uploadFile(attachment, file).then(
                        function (output) {
                            if (fileCount == uploadCount) {
                                if ($scope.entity.dos_Id == 7) {
                                    $scope.sendEmail();
                                }
                                uploadCount = 0;
                            }
                        },
                        function (output) {
                            ngNotifier.error(output.data.output.messages);
                        }
                    );
                });
            }
            else {
                if ($scope.entity.dos_Id == 7) {
                    $scope.sendEmail();
                }
            }

        };

        $scope.sendEmail = function () {
            if ($scope.entity.emailTo == undefined) {
                ngNotifier.error("Please enter To Emailid");
                return;
            }
            if ($scope.entity.emailSubject == undefined) {
                ngNotifier.error("Please enter Email Subject");
                return;
            }
            var emailEntity = {};
            emailEntity.emailTo = $scope.entity.emailTo;
            if ($scope.entity.emailcc != '') {
                emailEntity.emailcc = $scope.entity.emailcc;
            }
            if ($scope.entity.emailBcc != '') {
                emailEntity.emailBcc = $scope.entity.emailBcc;
            }
            emailEntity.emailSubject = $scope.entity.emailSubject;
            emailEntity.emailBody = $scope.entity.emailBody;

            emailEntity.documentCommonID = $scope.entity.documentCommonID;
            emailEntity.createdBy = $scope.$parent.authentication.userId;
            emailEntity.updatedBy = $scope.$parent.authentication.userId;
           
            var fileItem = {};
            $scope.entity.fileAttachementDTOList = [];

            if ($scope.entity.customerfile != null) {
                $scope.entity.customerfile.forEach(function (file) {
                    fileItem = {};
                    fileItem.fileName = $scope.entity.documentCommonID + '_DOC_' + $scope.entity.dos_Id + '_' + file.name;
                    $scope.entity.fileAttachementDTOList.push(fileItem);
                });
                emailEntity.fileAttachementDTOList = $scope.entity.fileAttachementDTOList;
            }
            entityService.sendEmail(emailEntity).then(
                function (output) {
                    $scope.entity = {};
                    $scope.actionRemarksList = [];
                    $scope.selectOption = "SystemRefNo";
                    $scope.searchBox = "";
                    $scope.documentationlistTable.reload();
                    $scope.goBack();
                    ngNotifier.show(output.data);
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });

        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("documentationController", controller);

});
