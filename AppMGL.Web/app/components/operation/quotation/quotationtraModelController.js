"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "ngNotifier", "authService", "quotationService", "requestData"];

    var controller = function ($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService, requestData) {
         var lastAction = "";
        $scope.siteId = (requestData.siteId) ? requestData.siteId : 0;
        $scope.actionRemarks = (requestData.actionRemarks) ? requestData.actionRemarks : '';
        $scope.quotationID = (requestData.quotationID) ? requestData.quotationID : '0';
        $scope.exportReportTracking = function () {          
            var reportParams = {               
                SitId: $scope.siteId
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
        $scope.closeQuotationtraModel = function (action) {
            $uibModalInstance.close();
        };
        $scope.uploadQuotationFinal = function () {

            var customerfile = document.getElementById('qutfile').files[0],
                r = new FileReader();
            if (customerfile) {
                if (customerfile.size > 10485760) {
                    ngNotifier.error("File cannot exceeds more than 10 MB size.");
                }
                else {
                    var attachment = {
                        FileName: 'Tracking',
                        SiteId: $scope.siteId
                    };
                    entityService.uploadQuotationFinalFile(attachment, customerfile).then(
                        function (output) {
                            angular.element("input[type='file']").val(null);
                            alert("File Uploaded successfully.")
                        },
                        function (output) {
                            ngNotifier.error(output.data.output.messages);
                        }
                    );
                }

            }
        };
        $scope.downloadAttachment = function () {
            //window.open('', '_blank', '');
            //if ($scope.entity.attachment != null) {
            entityService.downloadQuotationFinal($scope.siteId).then(
                    function (output) {
                        var blob = new Blob([output.data], { type: 'application/octet-stream'});
                        saveAs(blob, "QuatationTrack_" + $scope.siteId+".xlsx");
                       // var blob = new Blob([data], { type: "application/vnd.openxmlformats - officedocument.spreadsheetml.sheet" });
                     //var objectUrl = URL.createObjectURL(blob);
                    // window.open(objectUrl);
                    },
                    function (output) {
                        ngNotifier.logError(output);
                    }
                );
            //}
        };
        $scope.deleteTempQuotation = function () {
            var rest = entityService.deleteTempQuotation($scope.siteId).then(
                function (output) {
                    alert("Quotation Deleted Successfully.")
                }

            );
        };
        $scope.saveQuationRemarks = function () {
            if ($scope.entity.lastRemarks == null) {
                ngNotifier.error("Please Enter Remarks");
                return;
            }
            if ($scope.entity.lastRemarkDate == null) {
                ngNotifier.error("Please Remark date");
                return;
            }
            if ($scope.entity.nextActionDueDate == null) {
                ngNotifier.error("Please select Next Action Due date");
                return;
               }
                
           
            $scope.entity.quotationID = $scope.quotationID;
            entityService.SaveQuationRemarks($scope.entity).then(
                function (output) {
                    ngNotifier.show(output.data);
                    entityService.getQuotationRemark($scope.quotationID).then(
                        function (output) {
                            if (output.data.resultId == 2005) {
                                ngNotifier.showError($scope.authentication, output);
                                $scope.logOut()
                            }
                            $scope.actionRemarks = output.data.data;
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        }
                    );
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);

                });
        };
        angular.extend(this, new modalController($scope, $timeout, $uibModal, $uibModalInstance, localStorageService, ngNotifier, authService, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("quotationtraModelController", controller);

});
