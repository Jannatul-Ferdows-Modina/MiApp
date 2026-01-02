// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "bookingConfReadyToSendService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

       
        $scope.$parent.pageTitle = "Booking Conf Ready To Send";
        $scope.$parent.breadcrumbs = ["Shipment", "Booking", "Booking Conf Ready To Send"];
        //#region General
        $scope.page = appUrl.bookingConfReadyToSend;
        $scope.tabs = appUrl.bookingConfReadyToSend.tabs;        

        $scope.actionRemarksList = [];
        $scope.shipmentDocsList = [];

        //$scope.lookups = { siplDepartments: [] };
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
            //$scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
        };
       
        //#endregion       

        $scope.searchOptions = [
                { optionValue: "", optionName: "-All-" },                
                { optionValue: "SystemRefNo", optionName: "System Ref No" },                
                { optionValue: "LineBookingNo", optionName: "Line Booking No" }
        ];
        $scope.selectOption = "SystemRefNo";
        $scope.searchBox = "";
        $scope.departmentID = 0
        //$scope.galBookingStatusID = 18;

        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            departmentID: 0
            //galBookingStatusID: $scope.galBookingStatusID
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

        $scope.UpdateEmail = function (isInterimBooking) {
            if(isInterimBooking == true)
            {
                $scope.entity.emailSubject = "Booking Information : " + $scope.entity.fileNo;
                $scope.entity.emailBody = "Attached herewith kindly find the subject booking Information";
                $scope.entity.galBookingStatusID = 18; //Pending Shipper Confirmation Email
            }
            else {
                $scope.entity.emailSubject = "Booking Confirmation : " + $scope.entity.fileNo;
                $scope.entity.emailBody = "Attached herewith kindly find the subject booking confirmation";
                $scope.entity.galBookingStatusID = 7; //Pending for Confirmation from line
            }
        }
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

                      var dataitems = entityService.getConfReadyToSendList(listParams).then(
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
                departmentID: 0
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

                    break;

            }
        };
        $scope.deleteBookingConfDocs = function (emailEntity) {
            //var emailEntity = {};
            //emailEntity.documentCommonID = $scope.entity.documentCommonID;
            //var fileItem = {
            //};
            //$scope.entity.fileAttachementDTOList =[];

            //$scope.entity.customerfile.forEach(function(file) {
            //    fileItem = { };
            //    fileItem.fileName = file.name;
            //    $scope.entity.fileAttachementDTOList.push(fileItem);
            //});
            //emailEntity.fileAttachementDTOList = $scope.entity.fileAttachementDTOList;


            entityService.deleteBookingConfDocs(emailEntity).then(
                function (output) {
                    var uploadCount = 0;

                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });

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
            emailEntity.createdBy = $scope.$parent.authentication.userId;
            if ($scope.entity.emailcc != '') {
                emailEntity.emailcc = $scope.entity.emailcc;
            }
            if ($scope.entity.emailBcc != '') {
                emailEntity.emailBcc = $scope.entity.emailBcc;
            }
            emailEntity.emailSubject = $scope.entity.emailSubject;
            emailEntity.emailBody = $scope.entity.emailBody;

            emailEntity.documentCommonID = $scope.entity.documentCommonID;
            if($scope.entity.galBookingStatusID == "18")
            {
                emailEntity.galBookingStatusID = 8; //Shipper's Confirmation Awaited
            }
            else{
                emailEntity.galBookingStatusID = 7; //Pending for Confirmation from line
            }

            var fileItem = {};
            $scope.entity.fileAttachementDTOList = [];

            if ($scope.entity.customerfile != null) {
                $scope.entity.customerfile.forEach(function (file) {
                    fileItem = {};
                    fileItem.fileName = file.name;
                    $scope.entity.fileAttachementDTOList.push(fileItem);
                });
                emailEntity.fileAttachementDTOList = $scope.entity.fileAttachementDTOList;
            }
            entityService.sendEmail(emailEntity).then(
                function (output) {
                    
                    $scope.entity = {};                    
                    $scope.bookinglistTable.reload();
                    $scope.goBack();
                    $scope.deleteBookingConfDocs(emailEntity);
                    ngNotifier.show(output.data);
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });

        };

        $scope.saveDocumentAttachement = function () {
            var isValid = true;
            var fileCount = 0;
            //if ($scope.entity.customerfile != null) {
            //    if ($scope.entity.customerfile.length == 0) {
            //        ngNotifier.error("Please attach atleast one file");
            //        return;
            //    }
            //    else {
            if ($scope.entity.customerfile != null) {
                $scope.entity.customerfile.forEach(function (file) {

                    if (file) {
                        fileCount = fileCount + 1;

                        if (file.size > 10485760) {
                            ngNotifier.error("File cannot exceeds more than 10 MB size.");
                            isValid = false;
                            return;
                        }
                        else if (file.type != "application/pdf" && file.type != "application/docx" && file.type != "application/doc" && file.type != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && file.type != "application/xlsx" && file.type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                            ngNotifier.error("Please upload only these file types - pdf,docx,doc,xlsx");
                            isValid = false;
                            return;
                        }
                    }
                });
            }
            if (isValid == true) {
                $scope.uploadAttachment($scope.entity.documentCommonID, fileCount);
            }
            else {
                retun;
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
                     //$scope.entity.isInterimBooking = true;
                     if ($scope.entity.galBookingStatusID == 18) {
                         $scope.entity.emailSubject = "Booking confirmation : " + $scope.entity.fileNo;
                         $scope.entity.emailBody = "Attached herewith kindly find the subject booking confirmation";
                     }
                     else {
                         $scope.entity.emailSubject = "Booking Information : " + $scope.entity.fileNo;
                         $scope.entity.emailBody = "Attached herewith kindly find the subject booking Information";
                         //$scope.entity.galBookingStatusID = 7; //Pending for Confirmation from line
                     }
                     //$scope.entity.emailSubject = "Booking Information : " + $scope.entity.fileNo;
                     //$scope.entity.emailBody = "Attached herewith kindly find the subject booking Information";
                     $scope.entity.attachment = "Shipper_" + $scope.entity.documentCommonID + ".pdf";
                     //$scope.entity.galBookingStatusID = 18;
                     //if ($scope.entity.shipmentDocsDTOList != null) {
                     //    $scope.shipmentDocsList = $scope.entity.shipmentDocsDTOList;
                     //}
                     
                 },
                 function (output) {
                     ngNotifier.showError($scope.authentication, output);
                 }
             );
        };

        

        $scope.uploadAttachment = function (documentCommonID,fileCount) {
            var uploadCount = 0;
            //$scope.deleteBookingConfDocs();
            if ($scope.entity.customerfile != null) {
                $scope.entity.customerfile.forEach(function (file) {

                    var attachment = {
                        DisplayName: "BkgConf" + documentCommonID + '_' + file.name,
                        DocumentCommonID: documentCommonID,
                        DocumentType: 'Booking'
                    };
                    uploadCount = uploadCount + 1;
                    entityService.uploadFile(attachment, file).then(
                        function (output) {
                            if (fileCount == uploadCount) {
                                $scope.sendEmail();
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
                $scope.sendEmail();
            }

        };


        $scope.downloadBookingConfDoc = function () {
            
            $scope.entities = {};
            $scope.entities.documentCommonID = $scope.entity.documentCommonID;
            $scope.entities.attachFile = "Shipper_" + $scope.entity.documentCommonID+".pdf";

            if ($scope.entity.attachment != null) {
                entityService.downloadBookingConfDoc($scope.entities).then(
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

    app.register.controller("bookingConfReadyToSendController", controller);

});
