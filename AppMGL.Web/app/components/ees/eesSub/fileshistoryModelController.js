"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "eessubService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {

        $scope.siteId = (requestData.siteId) ? requestData.siteId : 0;
        $scope.FileList = (requestData.FileList) ? requestData.FileList : '';
        $scope.popupdate = (requestData.popupdate) ? requestData.popupdate : '';
        $scope.popupdate1 = (requestData.popupdate1) ? requestData.popupdate1 : '';
        $scope.dcommonid = (requestData.dcommonid) ? requestData.dcommonid : '';
        $scope.aesid = (requestData.aesid) ? requestData.aesid : '';
        $scope.itanno = (requestData.itanno) ? requestData.itanno : '';
        $scope.divdata = $scope.popupdate;
        $scope.divdata1 = $scope.popupdate1;

        $scope.closemodal = function (action) {
            
            $uibModalInstance.close();
        };
        $scope.closemodalfile = function (action) {
            $uibModalInstance.close();
            $scope.GetEssFileList($scope.dcommonid);
            var outputdata = {};
            outputdata.itanno = $scope.itanno;
            $uibModalInstance.close(outputdata);
        };
        $scope.DownloadEssFile = function (documentCommonID, aesFileName) {
            aesFileName = aesFileName 
            entityService.DownloadEssFile(documentCommonID, aesFileName).then(
                function (output) {
                    var blob = new Blob([output.data], { type: 'application/octet-stream' });
                    saveAs(blob, aesFileName);

                },
                function (output) {
                    alert("File not available");
                    
                }
            );
        };
        $scope.ViewFile = function (documentCommonID, aesFileName, id, itanno) {
            aesFileName = aesFileName;
            var outputdata;
            var inputputdata;
            entityService.ViewInputFile(aesFileName).then(
                function (output) {
                    inputputdata = output.data.content;

                },
                function (output) {
                    inputputdata = "File not available please wait..";

                }
            );
            entityService.ViewFile(documentCommonID, aesFileName).then(
                function (output) {
                    outputdata = output.data.content;
                    var strlen = outputdata.lastIndexOf("~~~");
                    var endlen = outputdata.lastIndexOf("###");
                    if (strlen > 0 && endlen > 0) {
                        var itanno1 = outputdata.substring(strlen + 3, endlen);
                        var replavevalue = '~~~' + itanno1 + '###';
                        outputdata = outputdata.replaceAll(replavevalue, '');
                        if (itanno == "" || itanno == undefined || itanno == null)
                            itanno = itanno1;
                    }
                   
                    $scope.showviewpopup(outputdata, inputputdata, documentCommonID, id, itanno);
                 },
                function (output) {
                    outputdata="File not available";
                    $scope.showviewpopup(outputdata, inputputdata, documentCommonID, id, itanno);

                }
            );
            
        };
        $scope.ViewInputFile = function (aesFileName) {
            entityService.ViewInputFile(aesFileName).then(
                function (output) {
                    $scope.showviewpopup(output.data.content);
                },
                function (output) {
                    alert("File not available");

                }
            );
        };
        $scope.showviewpopup = function (popdate, inpopdate, documentCommonID, id, itanno) {
            
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/ees/eesSub/popupdate.html",
                controller: "fileshistoryModelController",
                resolve: {
                    requestData: function () {
                        return {
                            siteId: $scope.$parent.selectedSiteId,
                            popupdate: popdate,
                            popupdate1: inpopdate,
                            dcommonid: documentCommonID,
                            aesid: id,
                            itanno: itanno
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    $uibModalInstance.close();
                    if (itanno != '' && itanno != undefined) {
                        $("#originalITN").val(itanno);
                    }
                    $("#messagelist").html('');
                    $("#messagelist").html(popdate);
                    
                },
                function (output) {
                    ngNotifier.logError(output);
                });
           
        };
        $scope.FileHelp = function () {
            window.open('app/components/ees/eesSub/ErrorCode.pdf', '_blank', 'fullscreen=yes');
        };
        $scope.UploadEssFile = function (documentCommonID, aesFileName,id) {
            if (aesFileName != "" && aesFileName != null) {
                var listParams = {
                    DocumentCommonID: documentCommonID.toString(),
                    FileName: aesFileName
                };
                entityService.UploadEssFile(listParams).then(
                    function (output) {
                        alert(output.data.output.messages[0]);
                        var index = $scope.FileList.map((o) => o.id).indexOf(id);
                        $scope.FileList[index].isuploaded = 1;
                        //$scope.GetEssFileList($scope.dcommonid);
                    }

                );
            }
            else { alert("File not generated yet. please generate file first"); }
        };
        $scope.EditRecord = function (documentCommonID, Id) {
            var outputdata = {};
            outputdata.documentCommonID = documentCommonID;
            outputdata.Id = Id;
            $uibModalInstance.close(outputdata);
            //$scope.showViewDocumentModel(documentCommonID, '', Id)
        };
        $scope.UpdateStatus = function (type, dcommonid, aesid) {

            var itanno = $("#itnno").val();
            if (itanno == undefined)
                itanno = "";
            if (itanno == "" && type == "A") {
                alert("Please Enter ITAN No.");
                return false;
            }
            entityService.UpdateStatus(type, dcommonid, aesid, itanno).then(
                function (output) {
                    if (itanno != '' && itanno != undefined) {
                        $("#originalITN").val(itanno);
                    }
                    alert("Status Updated Successfully.");
                    $uibModalInstance.close();
                },
                function (output) {
                   // alert("File not available");

                }
            );
        };
        $scope.GetEssFileList = function (documentCommonID) {
            entityService.GetEssFileList(documentCommonID).then(
                function (output) {

                    $scope.FileList = output.data.data;
                    var modalInstance = $uibModal.open({
                        animation: false,
                        backdrop: "static",
                        keyboard: false,
                        size: "lg",
                        templateUrl: "app/components/ees/eesSub/eesfilehistory.html",
                        controller: "fileshistoryModelController",
                        resolve: {
                            requestData: function () {
                                return {
                                    siteId: $scope.siteId,
                                    FileList: $scope.FileList,
                                    dcommonid: documentCommonID
                                };
                            }
                        }
                    });
                },
                function (output) {
                    

                }
            );
        };
        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };
    controller.$inject = injectParams;
    app.register.controller("fileshistoryModelController", controller);
});
