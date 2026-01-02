"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "authService", "contractRateModalService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, entityService, requestData) {


        $scope.contractID;
        $scope.documentCommonID = (requestData.contractID) ? requestData.contractID : 0;
        var isupload = (requestData.isupload);
        if (isupload == 0)
        {
            isupload = 0;
        }
        else
        {
            isupload = 1;
        }
        $scope.isupload = isupload;

        $scope.documentTable = new NgTableParams(
       {
           page: 1,
           count: 10,
           sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }")
       }, {
           counts: [],
           getData: function (params) {
               var listParams = {
                   PageIndex: params.page(),
                   PageSize: params.count(),
                   Sort: JSON.stringify(params.sorting()),
                   SitId: requestData.SitId,
                   contractID: requestData.contractID
               };
               return entityService.getDocumentAttachmentDetail(listParams).then(
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

      
        $scope.saveDocumentAttachement = function (source, fromList) {


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
                            $scope.entity.documentCommonID = $scope.documentCommonID;
                            $scope.entity.sdDocName = file.name;
                            //fileItem = {};
                            //fileItem.docName = file.name;
                            //$scope.entity.shipmentDocsDTOList.push(fileItem);
                        });
                    }

                    //$scope.entity.shipmentDocsDTOList = $scope.shipmentDocsList;
                }
            }
            if ($scope.entity.customerfile == undefined) {
                ngNotifier.error("Please attach atleast one file");
                return;
            }
            if (isValid == true) {
                $scope.entity.siteId = $scope.$parent.selectedSiteId;
                //$scope.entity.createdBy = $scope.$parent.authentication.userId;
                // $scope.entity.updatedBy = $scope.$parent.authentication.userId;
                entityService.saveDocumentAttachement($scope.entity).then(
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

        $scope.deleteShipmentDoc = function (rownum, documentCommonID, filename) {
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            $scope.entities.docName = filename;
            ngNotifier.confirm("Are you sure you want to DELETE Document?", null, function () {
                //$scope.shipmentDocsList.splice(rownum, 1);
                entityService.deleteShipmentDoc($scope.entities).then(
                            function (output) {

                                //$scope.entity = {};
                                //$scope.quotationlistTable.reload();
                                //$scope.goBack();
                                ngNotifier.show(output.data);
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
                            DocumentCommonID: documentCommonID
                        };
                        entityService.uploadFile(attachment, file).then(
                            function (output) {
                                //ngNotifier.show(output.data.output);
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

        $scope.downloadAttachment = function (documentCommonID, filename) {
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


        //#endregion

        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("contractRateModalController", controller);

});