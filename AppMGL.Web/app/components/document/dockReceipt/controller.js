"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "dockReceiptService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        var lastAction = "";
        $scope.page = appUrl.dockReceipt;
        $scope.tabs = appUrl.dockReceipt.tabs;
        $scope.$parent.pageTitle = "Dock Receipt";
        $scope.$parent.breadcrumbs = ["Shipment", "Trucking", "Dock Receipt"];
        $scope.isPrintAddress = false;
        $scope.documentStatus = 0;
        $scope.fileNo = '';
        $scope.oldfileNo = '';
        $scope.oldcommanId = '';
        $scope.oldfileNo1 = '';
        $scope.oldcommanId1 = 0;
        $scope.olddeliverOrderId = 0;

        //#endregion

        //#region Private 
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
                    SitId: $scope.$parent.selectedSiteId,
                    DocumentStatus: $scope.documentStatus,
                    FileNo:$.trim( $("#txtfileNo").val())
                };
               
                return entityService.getDocumentList(listParams).then(
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

        $scope.filterDocReceiptData = function () {
            var dashboardOption = localStorageService.get("dashboardOption");
            
            if (dashboardOption != null) {
                if (dashboardOption == 'TPDRU' || dashboardOption == 'TPDRF') {
                    $scope.documentStatus = 14;
                }               
                localStorageService.remove("dashboardOption");
            }
            else {
                $scope.documentStatus = 0;
            }
        };
        $scope.filterDocReceiptData();

        var getDockReceipt = function () {

            resetFilterField();
            if ($scope.entity.hblDataSource == "DefaultData") {
                $scope.entity.systemRefNo = $scope.oldfileNo;
            }
            if ($scope.entity.hblDataSource == "DockReceipt") {
                $scope.entity.systemRefNo = $scope.oldfileNo1;
            }
            var params = {
                MiamiRefNo: $scope.entity.miamiRefNo || "",
                SystemRefNo: $scope.entity.systemRefNo || "",
                BookingNo: $scope.entity.bookingNo || "",
                SitId: $scope.$parent.selectedSiteId
            };
            
            entityService.getDockReceipt(params).then(
                function (output) {
                    $scope.document = output.data.data;
                    $scope.document.fileNo = $scope.oldfileNo;
                    $scope.document.documentCommonId = $scope.oldcommanId;
                    $scope.document.deliverOrderId=$scope.olddeliverOrderId;

                    if ($scope.document.commodityDetail.length == 0) {
                        $scope.document.commodityDetail = [initCommodityDetail(0)];
                    }
                    $scope.entity.docCommId = $scope.document.documentCommonId;
                    $scope.detailTable.reload();
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        var viewDocumentList = function () {
            $scope.olddeliverOrderId = $scope.document.deliverOrderId
            if ($scope.entity.hblDataSource == "TemplateData") {
                var modalInstance = $uibModal.open({
                    animation: false,
                    backdrop: "static",
                    keyboard: false,
                    size: "lg",
                    //templateUrl: "app/components/document/viewDocumentList/index.html",
                   // controller: "viewDocumentListController",
                    templateUrl: "app/components/document/viewDockTemplate/index.html",
                    controller: "viewDockTemplateController",
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
                                getDockReceipt();
                            }
                        }
                    },
                    function (output) {
                        ngNotifier.logError(output);
                    });
            }

            if ($scope.entity.hblDataSource == "BlankPage") {
                BlankDockPage();
            }
            if ($scope.entity.hblDataSource == "DefaultData" || $scope.entity.hblDataSource == "DockReceipt") {
                getDockReceipt();
            }
        };

        var resetFilterField = function () {

            $scope.entity.miamiRefNo = ($scope.entity.filterFieldName == "miamiRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.systemRefNo = ($scope.entity.filterFieldName == "systemRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.bookingNo = ($scope.entity.filterFieldName == "bookingNo") ? $scope.entity.filterFieldValue : "";
        };

        var editDocument = function () {

            initControls("edit");
        };

        var saveDocument = function () {

            entityService.saveDockReceipt($scope.document).then(
                function (output) {
                    ngNotifier.show(output.data);
                    initControls("save");
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );         
            sendEmail();
        };

        var saveDocumentPdf = function () {
            $scope.document.createdBy = $scope.$parent.userInfo.usrId;
            $scope.document.modifiedBy = $scope.$parent.userInfo.usrId;
            entityService.saveDockRecPdf($scope.document).then(
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
            getDockReceipt();
        };

        var exportPdf = function () {            
            resetFilterField();
            var reportParams = {
                MiamiRefNo: $scope.entity.miamiRefNo || "",
                SystemRefNo: $scope.entity.systemRefNo || "",
                BookingNo: $scope.entity.bookingNo || "",
                SitId: $scope.$parent.selectedSiteId || "",
                DocCommonId: $scope.entity.docCommId                   
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
            saveDocumentPdf();
        };


        var sendEmail = function () {

            $scope.entity.createdBy = $scope.$parent.authentication.userId;
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "md",
                templateUrl: "app/components/document/sendEmail/index.html",
                controller: function ($scope, $timeout, $uibModalInstance, ngNotifier, requestData) {
                    $scope.entity = {};
                    $scope.entity.emailSubject = "Booking No #" + requestData.entity.systemRefNo;
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
                            MiamiRefNo: $scope.entity.miamiRefNo || "",
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

        $scope.lookups = { departmentTypes: [], copyReceiptTypes: [], viewDockReceipts: [], filterFields: [], consignedToTypes: [] };

        $scope.setLookups = function (source, lookup, output, index) {
            if (lookup == "origin") {
                   $scope.document.transshipment = output.data[0].portId;
            }
            if (lookup == "SIPLContact") {
                $scope.document.checkedBY = output.data[0].contactID;
                $scope.document.exporterAddress = output.data[0].address;
                $scope.document.exporterId = output.data[0].contactID;
            }
          if (lookup == "SIPLContact1") {
                
                $scope.document.consigneeAddress = output.data[0].address;
                $scope.document.consigneeId = output.data[0].contactID;
            }
            if (lookup == "SIPLContact2") {
                
                $scope.document.consignedToAddress = output.data[0].address;
                $scope.document.consignedToId = output.data[0].contactID;
            }
            if (lookup == "SIPLContact3") {

                $scope.document.fwdAgentAddress = output.data[0].address;
                $scope.document.fwdAgentId = output.data[0].contactID;
            }
            if (lookup == "pickupLocation") {

                $scope.document.emptyPickup = output.data[0].companyName+' '+output.data[0].address;
                $scope.document.EmptyPickupId = output.data[0].contactID;
            }
            if (lookup == "returnLocation") {

                $scope.document.fullReturn = output.data[0].companyName + ' ' + output.data[0].address;
                $scope.document.fullReturnId = output.data[0].contactID;
            }
            if (lookup == "DockReceiptSearch") {

                $scope.oldfileNo1 = output.data[0].fileNo;
                $scope.oldcommanId1 = output.data[0].documentCommonId;
                $scope.entity.systemRefNo = $scope.oldfileNo1;

            }

            if (lookup == "CustomerName") {
                $scope.document.customerId = output.data[0].contactID;
                $scope.document.custFirstName = output.data[0].firstName;
                $scope.document.custLastName = output.data[0].lastName;
                $scope.document.custAddressLine1 = output.data[0].fullAddress;
                $scope.document.custCountryName = output.data[0].countryName;
                $scope.document.custCountryCode = output.data[0].countryCode;
                $scope.document.custStateName = output.data[0].stateName;
                $scope.document.custStateCode = output.data[0].stateCode;
                $scope.document.custCityName = output.data[0].cityName;
                $scope.document.custPostalCode = output.data[0].zipCode;
                $scope.document.custPhoneNumber = output.data[0].telNo;
                if (output.data[0].countryCode != "" && output.data[0].countryCode != undefined && output.data[0].countryCode != null)
                    $scope.document.custCountryName = $scope.document.custCountryName + "-" + output.data[0].countryCode
                if (output.data[0].stateCode != "" && output.data[0].stateCode != undefined && output.data[0].stateCode != null)
                    $scope.document.custStateName = $scope.document.custStateName + "-" + output.data[0].stateCode;


            }
            if (lookup == "Country") {
                $scope.document.custCountryId = output.data[0].itemId;
                $scope.document.custCountryCode = output.data[0].itemCode;
                
            }
            if (lookup == "State") {
                $scope.document.custStateId = output.data[0].itemId;
                $scope.document.custStateCode = output.data[0].itemCode;
                

            }
        };

        $scope.clearLookups = function (source, lookup, index) {
            if (lookup == "origin") {
                $scope.document.transshipment = null;
            }
            if (lookup == "SIPLContact") {
                $scope.document.checkedBY = null;
            }
        };

        $scope.updateIsPrintAddress = function (isPrntAddVal) {
            if (isPrntAddVal) {
                $scope.isPrintAddress = true;
                $scope.entity.isPrintAddress = 1;
                //$scope.entity.commodityIds = [];
            }
            else {
                $scope.isPrintAddress = false;
                $scope.entity.isPrintAddress = 2;
                //$scope.entity.commodityIds = [];
            }
        };

        $scope.initDropdown = function () {

            $scope.lookups.departmentTypes = [];
            $scope.lookups.departmentTypes.push({ fieldName: 1, fieldValue: "Ocean" });
            $scope.lookups.departmentTypes.push({ fieldName: 2, fieldValue: "Air" });

            $scope.lookups.copyReceiptTypes = [];
            $scope.lookups.copyReceiptTypes.push({ fieldName: 1, fieldValue: "Transporter" });
            $scope.lookups.copyReceiptTypes.push({ fieldName: 2, fieldValue: "Custom Clearance" });

            $scope.lookups.viewDockReceipts = [];
            $scope.lookups.viewDockReceipts.push({ fieldName: 1, fieldValue: "Normal Flow" });
            $scope.lookups.viewDockReceipts.push({ fieldName: 2, fieldValue: "Exception" });

            $scope.lookups.filterFields = [];
            //$scope.lookups.filterFields.push({ fieldName: "miamiRefNo", fieldValue: "Miami Ref No" });
            $scope.lookups.filterFields.push({ fieldName: "systemRefNo", fieldValue: "System Ref No" });
            $scope.lookups.filterFields.push({ fieldName: "bookingNo", fieldValue: "Booking No" });

            $scope.lookups.consignedToTypes = [];
            $scope.lookups.consignedToTypes.push({ fieldName: 1, fieldValue: "To The Order Of" });
            $scope.lookups.consignedToTypes.push({ fieldName: 2, fieldValue: "To Order" });

            $scope.entity.departmentType = 1;
            $scope.entity.copyReceiptType = 1;
            $scope.entity.viewDockReceipt = 1;
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

        $scope.callCompanyModal = function (source) {

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

            var companyID = $scope.document.fwdAgentId;
            if (source == "emptylocation") {
                // if ($scope.entity.pickupLocationId!="0")
                //    companyID = $scope.entity.pickupLocationId;
                //else
                companyID = 0;
            }

            if (source == "fulllocation") {
                // if ($scope.entity.returnLocationId != "0")
                //    companyID = $scope.entity.returnLocationId;
                // else
                companyID = 0;

            }
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
                            companyID: (companyID || 0),
                            sourcetype: (source || '')
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        if (source == "emptylocation") {
                            $scope.document.emptyPickup = output.data.companyName + ' ' + output.data.address;
                            $scope.document.EmptyPickupId = output.data.contactID;
                        }
                        if (source == "fulllocation") {
                            $scope.document.fullReturn = output.data.companyName + ' ' + output.data.address;
                            $scope.document.fullReturnId = output.data.contactID;

                        }
                        else {
                            $scope.document.fwdAgentId = output.data.contactID;
                            $scope.document.fwdAgentAddress = output.data.address;
                        }
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
                if ($scope.document.commodityDetail != undefined)// add by vikas
                {
                    $scope.entity.weightType = ($scope.document.commodityDetail[0].wtType == "LBS") ? "LBS" : "KILO";
                    $scope.entity.measurementType = ($scope.document.commodityDetail[0].mtType == "CFT") ? "CFT" : "CBM";
                }
                
                params.total($scope.document.commodityDetail==undefined?0: $scope.document.commodityDetail.length);
                return $scope.document.commodityDetail;
            }
        });

        $scope.performSubAction = function (source, target) {

            var action = source.currentTarget.attributes["action"].value;

         /*   if (action != "viewDocumentList") {
                if ($scope.entity.filterFieldValue == null || $scope.entity.filterFieldValue == "") {
                    ngNotifier.info("Please select document before performing action.");
                    return;
                }
            } */

            switch (action) {
                case "searchDoc":
                    $scope.documentTable.reload();
                    break;
                case "search":
                    getDockReceipt();
                    break;
                case "selectDocument":
                                        
                    $scope.onClickTab($scope.tabs[0]);
                    $scope.viewList = false;
                    $scope.page.urls.container = "app/views/shared/container.html";
                    $scope.entity = {};
                    initControls('viewDetail');

                    var fileNo = source.currentTarget.attributes["entityid"].value;
                    var docCommId = source.currentTarget.attributes["doccommid"].value;
                    $scope.oldfileNo = fileNo;
                    $scope.oldcommanId = docCommId;

                    $scope.entity.filterFieldName = "systemRefNo";
                    $scope.entity.filterFieldValue = fileNo;
                    $scope.entity.docCommId = docCommId;
                  //  editDocument();
                    getDockReceipt();
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
        var switchTab = function (title, action) {

            $scope.tabs.forEach(function (tab) {
                tab.active = false;
                tab.disabled = ((action === "add" || action === "edit") && tab !== $scope.tabs[0]);
                if (tab.title === title) {
                    tab.active = true;
                }
            });
        };
        $scope.dynamicRow = function (isAdded, target, index) {

            if (isAdded) {
                $scope.document.commodityDetail.push(initCommodityDetail(index+1));
            }
            else {
                $scope.document.commodityDetail = $scope.document.commodityDetail.filter(function (item) { return item !== target; });
            }
            $scope.detailTable.reload();
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
        var BlankDockPage = function () {
            $scope.document.exporterName = "";
            $scope.document.document = "";
            $scope.document.exporterAddress = "";
            $scope.document.free_Text = "";
           // $scope.document.documentNumber = "";
           // $scope.document.blNumber = "";
           // $scope.document.fileNo = "";
            $scope.document.exportRef = "";
            $scope.document.consignedToName = "";
           // $scope.document.fwdAgentName = "";
            $scope.document.consignedToAddress = "";
            $scope.document.consignedToType = "";
           // $scope.document.fwdAgentAddress = "";
            $scope.document.isPrintAddress = "";
            $scope.document.consigneeName = "";
            $scope.document.ftzNumber = "";
            //$scope.document.sameAsConsignedTo = "";
            $scope.document.consigneeAddress = "";
            $scope.document.exportInstruction = "";
            //$scope.document.carriageBy = "";
            $scope.document.placeOfReceipt = "";
            $scope.document.vessel = "";
            $scope.document.voyage = "";
            $scope.document.landingPort = "";
            $scope.document.loadingPert = "";
            $scope.document.foreignPort = "";
            $scope.document.transshipmentdesc = "";
            $scope.document.moveType = "";
            $scope.document.cutOff = "";
            $scope.document.reqStuffingDate = "";
            $scope.document.booking = "";
            $scope.document.departureDate = "";
            $scope.document.pickupName = "";
            $scope.document.emptyPickup = "";
            $scope.document.deliveredBy = "";
            $scope.document.checkedBydesc = "";
            $scope.document.issued = "";
            $scope.document.fullReturnName = "";
            $scope.document.fullReturn = "";
            $scope.document.commodityDetail = [initCommodityDetail(0)];
             $scope.detailTable.reload();

        };
       
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("dockReceiptController", controller);

});
