"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "authService", "documentUploadModelService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService, requestData) {
                
        $scope.documentCommonID = (requestData.documentCommonID) ? requestData.documentCommonID : 0;
        $scope.siteId = requestData.siteId;
        $scope.userId = requestData.userId;
        $scope.documentType = 'Booking';

        $scope.searchParam = {           
            documentCommonID: requestData.documentCommonID,
            documentType: $scope.documentType
        };

        $scope.documentTable = new NgTableParams(
       {
           page: 1,
           count: 10,
           sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }")
       }, {
           counts: [],
           getData: function (params) {               
               var listParams = {                  
                   Filter: JSON.stringify($scope.searchParam)
               };
               entityService.getCustomerDocumentDetails(listParams).then(
                   function (output) {
                       params.total(output.data.count);
                       $scope.shipmentDocsList = [];                       
                       $scope.entity = output.data.data;
                       if($scope.documentType == 'HBL')
                       {
                           $scope.entity.docType = "0"
                       }
                       if($scope.documentType == 'MBL')
                       {
                           $scope.entity.docType = "1"
                       }
                       if($scope.documentType == 'DockReceipt')
                       {
                           $scope.entity.docType = "2"
                       }
                       if($scope.documentType == 'CO')
                       {
                           $scope.entity.docType = "3"
                       }
                       if($scope.documentType == 'Shipper')
                       {
                           $scope.entity.docType = "4"
                       }
                       if($scope.documentType == 'Booking')
                       {
                           $scope.entity.docType = "5"
                       }
                       if($scope.documentType == 'Other')
                       {
                           $scope.entity.docType = "6"
                       }
                       if ($scope.entity.shipmentDocsDTOList != null) {
                           $scope.shipmentDocsList = $scope.entity.shipmentDocsDTOList;
                           $scope.entity.docType = $scope.entity.shipmentDocsDTOList[0].docType;
                       }
                       //else {
                       //    $scope.entity.docType = "5";
                       //}
                   },
                   function (output) {
                       ngNotifier.showError($scope.authentication, output);
                   }
               );
           }
       });

        $scope.getBookingDocuments = function (docType) {
           
            $scope.searchParam = {
                documentCommonID: requestData.documentCommonID,
                documentType: docType
            };
            // $scope.bookinglistTable.reload();
            var listParams = {
                Filter: JSON.stringify($scope.searchParam)
            };
            var dataitems = entityService.getCustomerDocumentDetails(listParams).then(
                   function (output) {                      
                       //params.total(output.data.count);
                       var docType = $scope.entity.docType;
                       $scope.entity = output.data.data;
                       $scope.entity.docType = docType;
                       $scope.shipmentDocsList = [];
                       if ($scope.entity.shipmentDocsDTOList != null) {
                           $scope.shipmentDocsList = $scope.entity.shipmentDocsDTOList;
                       }

                   },
                   function (output) {
                       ngNotifier.showError($scope.authentication, output);
                   }
               );
        };

        $scope.afterPerformAction = function (source, fromList) {                              
            //$scope.entity.docType = "1";
        };

        $scope.saveDocumentAttachement = function () {


            var isValid = true;
            if ($scope.entity.customerfile != null) {

                if ($scope.entity.customerfile.length == 0) {
                    ngNotifier.error("Please select atleast one file");
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

                    
                }
            }
            if ($scope.entity.customerfile == undefined) {
                ngNotifier.error("Please attach atleast one file");
                return;
            }
            if (isValid == true) {
                $scope.entity.siteId = $scope.siteId;
                $scope.entity.createdBy = $scope.userId;
                $scope.entity.updatedBy = $scope.userId;
                $scope.entity.documentCommonID = $scope.documentCommonID;
                if ($scope.entity.docType == "0") {
                    $scope.entity.documentType = "HBL";
                }
                if ($scope.entity.docType == "1") {
                    $scope.entity.documentType = "MBL";
                }
                if ($scope.entity.docType == "2") {
                    $scope.entity.documentType = "DockReceipt";
                }
                if ($scope.entity.docType == "3") {
                    $scope.entity.documentType = "CO";
                }
                if ($scope.entity.docType == "4") {
                    $scope.entity.documentType = "Shipper";
                }
                if ($scope.entity.docType == "5")
                {
                    $scope.entity.documentType = "Booking";
                }
                if ($scope.entity.docType == "6") {
                    $scope.entity.documentType = "Other";
                }
                
                
                entityService.saveCustomerDocAttachements($scope.entity).then(
                    function (output) {
                        $scope.documentID = $scope.documentCommonID;
                        if ($scope.entity.customerfile != null) {
                            $scope.uploadAttachment($scope.documentID);                            
                        }
                        $scope.entity = {};
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

        //$scope.loadDocumentAttachmentDetail = function (source, documentCommonID) {
        //    {
        //        page: 1,
        //        count: 10,
        //        sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }")
        //    }, {
        //        counts: [],
        //        getData: function (params) {
        //            var listParams = {
        //                PageIndex: params.page(),
        //                PageSize: params.count(),
        //                Sort: JSON.stringify(params.sorting()),
        //                SitId: requestData.SitId,
        //                contractID: requestData.contractID
        //            };
        //            return entityService.getDocumentAttachmentDetail(listParams).then(
        //                function (output) {
        //                    params.total(output.data.count);
        //                    return output.data.data;
        //                },
        //                function (output) {
        //                    ngNotifier.showError($scope.authentication, output);
        //                }
        //            );
        //        }
        //};

        $scope.deleteShipmentDoc = function (rownum, documentCommonID, filename,documentType,isSystemGenerated) {
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            $scope.entities.docName = filename;
            $scope.entities.documentType = documentType;
            $scope.entities.isSystemGenerated = isSystemGenerated;

            ngNotifier.confirm("Are you sure you want to DELETE Document?", null, function () {
                //$scope.shipmentDocsList.splice(rownum, 1);
                entityService.deleteShipmentDoc($scope.entities).then(
                            function (output) {

                                //$scope.entity = {};
                                //$scope.quotationlistTable.reload();
                                //$scope.goBack();
                                $scope.entity.documentType = $scope.entities.documentType;
                                $scope.documentType = $scope.entities.documentType;
                                $scope.documentTable.reload();
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });


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
                            DocumentType: $scope.entity.documentType
                        };
                        entityService.uploadFile(attachment, file).then(
                            function (output) {
                                //ngNotifier.show(output.data.output);
                                $scope.documentType = $scope.entity.documentType
                                $scope.documentTable.reload();
                                
                            },
                            function (output) {
                                ngNotifier.error(output.data.output.messages);
                            }
                        );
                    }

                }
            });

        };

        $scope.downloadAttachment = function (documentCommonID, filename, documentType, isSystemGenerated) {
            //window.open('', '_blank', '');
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            $scope.entities.attachFile = filename;
            $scope.entities.documentType = documentType;
            if (isSystemGenerated) {
                $scope.entities.isSystemGenerated = 1;
            }
            else {
                $scope.entities.isSystemGenerated = 0;
            }
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


        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("documentUploadModelController", controller);

});