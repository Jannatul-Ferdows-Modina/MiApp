"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "coService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.co;
        $scope.tabs = appUrl.co.tabs;

        $scope.$parent.pageTitle = "CO";
        $scope.$parent.breadcrumbs = ["Document", "CO"];
        $scope.documentStatus = 0;

        //#endregion

        //#region Private 

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

        var initCommodityDetail = function (index) {
            return {
                coOtherID: index,
                coid: 0,
                number: "",
                package: "",
                commodity: "",
                weight: "",
                measuremant: "",
                type: "",
                wtType: "KILO",
                mtType: "CBM"
            };
        };

        var initFreightDetail = function (index) {
            return {
                hoBLSubDetailID: index,
                hoid: 0,
                freightCharge: "",
                prepaid: "",
                collect: "",
                type: ""
            };
        };

        var getCO = function () {

            resetFilterField();
            var params = {
                //MiamiRefNo: $scope.entity.miamiRefNo || "",
                SystemRefNo: $scope.entity.systemRefNo || "",
                BookingNo: $scope.entity.bookingNo || "",
                SitId: $scope.$parent.selectedSiteId
            };
            entityService.getCO(params).then(
                function (output) {
                    $scope.document = output.data.data;
                    $scope.entity.docCommId = $scope.document.DocumentCommonId;
                    if ($scope.document.commodityDetail.length == 0) {
                        $scope.document.commodityDetail = [initCommodityDetail(0)];
                    }
                    $scope.detailTable.reload();
                    if ($scope.document.freightDetail.length == 0) {
                        $scope.document.freightDetail = [initFreightDetail(0)];
                    }
                    $scope.freightTable.reload();
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        var viewDocumentList = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/document/viewDocumentList/index.html",
                controller: "viewDocumentListController",
                resolve: {
                    requestData: function () {
                        return {
                            SitId: $scope.$parent.selectedSiteId,
                            documentStatus: $scope.documentStatus
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.data && output.resultId == 1001) {
                        if (output.data.exportRef != "" && output.docCommId != "") {
                            $scope.entity.filterFieldName = "systemRefNo";
                            $scope.entity.filterFieldValue = output.data.exportRef;
                            $scope.entity.docCommId = output.docCommId;
                            getCO();
                        }
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        var resetFilterField = function () {

            //$scope.entity.miamiRefNo = ($scope.entity.filterFieldName == "miamiRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.systemRefNo = ($scope.entity.filterFieldName == "systemRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.bookingNo = ($scope.entity.filterFieldName == "bookingNo") ? $scope.entity.filterFieldValue : "";
        };

        var editDocument = function () {

            initControls("edit");
        };

        var saveDocument = function () {

            entityService.saveCO($scope.document).then(
                function (output) {
                    ngNotifier.show(output.data);
                    initControls("save");
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        var saveCOPdf = function () {
            //$scope.document.createdBy = $scope.$parent.userInfo.usrId;
            //$scope.document.modifiedBy = $scope.$parent.userInfo.usrId;
            entityService.saveCOPdf($scope.document).then(
                function (output) {
                    ngNotifier.show(output.data);
                    initControls("save");
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
            
        };
        var cancelDocument = function () {

            initControls("cancel");
            getCO();
        };

        var exportPdf = function () {

            resetFilterField();
            var reportParams = {
                DocCommonId: $scope.document.documentCommonId || "",
                SystemRefNo: $scope.entity.systemRefNo || "",
                BookingNo: $scope.entity.bookingNo || "",
                SitId: $scope.$parent.selectedSiteId || "",
               // DocCommonId: $scope.document.DocumentCommonId
            };
            entityService.exportPdf(reportParams).then(
                function (output) {
                    var blobData = new Blob([output.data], { type: output.headers()["content-type"] });
                    var fileName = output.headers()["x-filename"];
                    saveAs(blobData, fileName);
                },
                function (output) {
                    ngNotifier.error(output);
                }
            );
            saveCOPdf();
        };

        var sendEmail = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "md",
                templateUrl: "app/components/document/sendEmail/indexco.html",
                controller: function ($scope, $timeout, $uibModalInstance, ngNotifier, requestData) {
                    $scope.send = function () {
                        $scope.$broadcast("show-errors-check-validity");
                        if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
                            ngNotifier.error("Required Field(s) are missing data.");
                            return;
                        }
                        $uibModalInstance.close({ data: $scope.entity, resultId: 1001 });
                    };
                    $scope.close = function () {
                        $uibModalInstance.close({ data: null, resultId: 1001 });
                    };
                },
                resolve: {
                    requestData: function () {
                        return {
                            entity: $scope.entity
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.data && output.resultId == 1001) {
                        resetFilterField();
                        var params = {
                           // MiamiRefNo: $scope.entity.miamiRefNo || "",
                            SystemRefNo: $scope.entity.systemRefNo || "",
                            BookingNo: $scope.entity.bookingNo || "",
                            SitId: $scope.$parent.selectedSiteId,
                            EmailTo: output.data.emailTo,
                            EmailCc: output.data.emailCc,
                            EmailBcc: output.data.emailBcc,
                            EmailSubject: output.data.emailSubject,
                            EmailBody: output.data.emailBody,
                            CreatedBy: $scope.$parent.userInfo.usrId
                        };
                        entityService.sendEmail(params).then(
                            function (output) {
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            }
                        );
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        //#endregion

        //#region Lookup 

        $scope.lookups = { departmentTypes: [], filterFields: [], consignedToTypes: [] };


        $scope.setLookups = function (source, lookup, output, index) {
            if (lookup == "SIPLContact1") {
                $scope.document.exporterID = output.data[0].contactID;
                $scope.document.exporterAddress = output.data[0].address;
            }
            if (lookup == "SIPLContact") {
                $scope.document.ConsignedToId = output.data[0].contactID;
                // $scope.entity.expAddress = output.data[0].address;
                $scope.document.consignedToAddress = output.data[0].address;

            }
            else if (lookup == "SIPLContactAgent") {
                $scope.document.FwdAgentId = output.data[0].contactID;
                $scope.document.fwdAgentAddress = output.data[0].address;
                
            }
            else if (lookup == "SIPLContactConsignee") {
                $scope.document.ConsigneeId = output.data[0].contactID;
                //$scope.entity.destinationDoorAddress = output.data[0].address;
                $scope.document.consigneeAddress = output.data[0].address;
            }
            else if (lookup == "discharge") {
                $scope.document.ForeignPortId = output.data[0].portId;
                //$scope.entity.destinationDoorAddress = output.data[0].address;
            }

            
        };

        $scope.clearLookups = function (source, lookup, index) {

            if (lookup == "SIPLContact") {
                $scope.document.ConsignedToId = null;
                // $scope.entity.expAddress = output.data[0].address;
            }
            else if (lookup == "SIPLContactAgent") {
                $scope.document.FwdAgentId = null;

            }
            else if (lookup == "SIPLContactConsignee") {
                $scope.document.ConsigneeId = null;
                //$scope.entity.destinationDoorAddress = output.data[0].address;
            }
            else if (lookup == "discharge") {
                $scope.document.ForeignPortId = null;
                //$scope.entity.destinationDoorAddress = output.data[0].address;
            }

        };
        $scope.initDropdown = function () {

            $scope.lookups.departmentTypes = [];
            $scope.lookups.departmentTypes.push({ fieldName: 1, fieldValue: "Ocean" });
            $scope.lookups.departmentTypes.push({ fieldName: 2, fieldValue: "Air" });

            $scope.lookups.filterFields = [];
           // $scope.lookups.filterFields.push({ fieldName: "miamiRefNo", fieldValue: "Miami Ref No" });
            $scope.lookups.filterFields.push({ fieldName: "systemRefNo", fieldValue: "System Ref No" });
            $scope.lookups.filterFields.push({ fieldName: "bookingNo", fieldValue: "Booking No" });

            $scope.lookups.consignedToTypes = [];
            $scope.lookups.consignedToTypes.push({ fieldName: 1, fieldValue: "To The Order Of" });
            $scope.lookups.consignedToTypes.push({ fieldName: 2, fieldValue: "To Order" });

            $scope.entity.departmentType = 1;
            $scope.entity.filterFieldName = "systemRefNo";

            $scope.entity.weightType = "KILO";
            $scope.entity.measurementType = "CBM";
        };

        $scope.callExporter = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCompanyModal/detail.html",
                controller: "siplCompanyModalController",
                resolve: {
                    requestData: function () {
                        return {
                            companyID: ($scope.document.exporterId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.document.exporterId = output.data.contactID;
                        $scope.document.exporterAddress = output.data.address;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.callConsignedTo = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCompanyModal/detail.html",
                controller: "siplCompanyModalController",
                resolve: {
                    requestData: function () {
                        return {
                            companyID: ($scope.document.consignedToId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.document.consignedToId = output.data.contactID;
                        $scope.document.consignedToAddress = output.data.address;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.callForwardingAgent = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCompanyModal/detail.html",
                controller: "siplCompanyModalController",
                resolve: {
                    requestData: function () {
                        return {
                            companyID: ($scope.document.fwdAgentId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.document.fwdAgentId = output.data.contactID;
                        $scope.document.fwdAgentAddress = output.data.address;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.callConsignee = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCompanyModal/detail.html",
                controller: "siplCompanyModalController",
                resolve: {
                    requestData: function () {
                        return {
                            companyID: ($scope.document.consigneeId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.document.consigneeId = output.data.contactID;
                        $scope.document.consigneeAddress = output.data.address;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.callLandingPort = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplPortModal/detail.html",
                controller: "siplPortModalController",
                resolve: {
                    requestData: function () {
                        return {
                            originID: ($scope.document.landingPortId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
               function (output) {
                   if (output.resultId == 1001) {
                       $scope.document.landingPortId = output.data[0].portId;
                       $scope.document.landingPort = output.data.name;
                   }
               },
               function (output) {
                   ngNotifier.logError(output);
               });
        }

        $scope.callForeignPort = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplPortModal/detail.html",
                controller: "siplPortModalController",
                resolve: {
                    requestData: function () {
                        return {
                            originID: ($scope.document.foreignPortId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
               function (output) {
                   if (output.resultId == 1001) {
                       $scope.document.foreignPortId = output.data[0].portId;
                       $scope.document.foreignPort = output.data.name;
                   }
               },
               function (output) {
                   ngNotifier.logError(output);
               });
        }

        //#endregion

        //#region Detail

        $scope.document = {};

        $scope.detailTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"DocumentCommonId\": \"asc\" }")
        }, {
            counts: [],
            getData: function (params) {
                if ($scope.document && $scope.document.commodityDetail && $scope.document.commodityDetail.length == 0) {
                    params.total(0);
                    return null;
                }
                $scope.entity.weightType = ($scope.document.commodityDetail[0].wtType == "LBS") ? "LBS" : "KILO";
                $scope.entity.measurementType = ($scope.document.commodityDetail[0].mtType == "CFT") ? "CFT" : "CBM";
                params.total($scope.document.commodityDetail.length);
                return $scope.document.commodityDetail;
            }
        });

        $scope.freightTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"DocumentCommonId\": \"asc\" }")
        }, {
            counts: [],
            getData: function (params) {
                if ($scope.document && $scope.document.freightDetail && $scope.document.freightDetail.length == 0) {
                    params.total(0);
                    return null;
                }
                params.total($scope.document.freightDetail.length);
                return $scope.document.freightDetail;
            }
        });

        $scope.performSubAction = function (source, target) {

            var action = source.currentTarget.attributes["action"].value;

            if (action != "viewDocumentList") {
                if ($scope.entity.filterFieldValue == null || $scope.entity.filterFieldValue == "") {
                    ngNotifier.info("Please select document before performing action.");
                    return;
                }
            }

            switch (action) {
                case "search":
                    getCO();
                    break;
                case "viewDocumentList":
                    viewDocumentList();
                    break;
                case "editDocument":
                    editDocument();
                    break;
                case "saveDocument":
                    saveDocument();
                    break;
                case "cancelDocument":
                    cancelDocument();
                    break;
                case "exportPdf":
                    exportPdf();
                    break;
                case "sendEmail":
                    sendEmail();
                    break;
                default:
                    //TODO
                    break;
            }
        };

        $scope.dynamicRow = function (isAdded, target, index) {

            if (isAdded) {
                $scope.document.commodityDetail.push(initCommodityDetail(index + 1));
            }
            else {
                $scope.document.commodityDetail = $scope.document.commodityDetail.filter(function (item) { return item !== target; });
            }
            $scope.detailTable.reload();
        };

        $scope.dynamicSubRow = function (isAdded, target, index) {

            if (isAdded) {
                $scope.document.freightDetail.push(initFreightDetail(index + 1));
            }
            else {
                $scope.document.freightDetail = $scope.document.freightDetail.filter(function (item) { return item !== target; });
            }
            $scope.freightTable.reload();
        };

        $scope.sameAsShipper = function () {

            if ($scope.document.sameAsShipper) {
                $scope.document.exporterId = $scope.document.consignedToId;
                $scope.document.exporterName = $scope.document.consignedToName;
                $scope.document.exporterAddress = $scope.document.consignedToAddress;
            }
            else {
                $scope.document.exporterId = 0;
                $scope.document.exporterName = "";
                $scope.document.exporterAddress = "";
            }
        };

        $scope.sameAsConsignedTo = function () {

            if ($scope.document.sameAsConsignedTo) {
                $scope.document.consigneeId = $scope.document.consignedToId;
                $scope.document.consigneeName = $scope.document.consignedToName;
                $scope.document.consigneeAddress = $scope.document.consignedToAddress;
            }
            else {
                $scope.document.consigneeId = 0;
                $scope.document.consigneeName = "";
                $scope.document.consigneeAddress = "";
            }
        };

        $scope.updateWeight = function () {
            $scope.document.commodityDetail.forEach(function (c) {
                if ($scope.entity.weightType == "LBS") {
                    c.weight = c.weight * 2.205;
                    c.wtType = "LBS";
                }
                else {
                    c.weight = c.weight / 2.205;
                    c.wtType = "KILO";
                }
            });
        };

        $scope.updateMeasurement = function () {
            $scope.document.commodityDetail.forEach(function (c) {
                if ($scope.entity.measurementType == "CFT") {
                    c.measuremant = c.measuremant * 35.315;
                    c.mtType = "CFT";
                }
                else {
                    c.measuremant = c.measuremant / 35.315;
                    c.mtType = "CBM";
                }
            });
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("coController", controller);

});
